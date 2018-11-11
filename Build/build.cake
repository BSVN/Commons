#tool "nuget:?package=coveralls.io&version=1.4.2"
#addin "nuget:?package=Cake.Git&version=0.18.0"
#addin nuget:?package=Nuget.Core
#addin "nuget:?package=Cake.Coveralls&version=0.9.0"

using NuGet;

var target = Argument("target", "Default");
var artifactsDir = "./artifacts/";
var solutionPath = "./BSN.Commons.sln";
var project = "./src/BSN.Commons/BSN.Commons.csproj";
var testFolder = "./tests/BSN.Commons.Tests/";
var testProject = testFolder + "BSN.Commons.Tests.csproj";
var coverageResultsFileName = "coverage.xml";
var currentBranch = Argument<string>("currentBranch", GitBranchCurrent("./").FriendlyName);
var isReleaseBuild = string.Equals(currentBranch, "master", StringComparison.OrdinalIgnoreCase);
var configuration = "Release";
var nugetApiKey = Argument<string>("nugetApiKey", null);
var coverallsToken = Argument<string>("coverallsToken", null);
var nugetSource = "https://api.nuget.org/v3/index.json";
var target = Argument("target", "Default");

Task("Clean")
    .Does(() => {
        if (DirectoryExists(artifactsDir))
        {
            DeleteDirectory(
                artifactsDir, 
                new DeleteDirectorySettings {
                    Recursive = true,
                    Force = true
                }
            );
        }
        CreateDirectory(artifactsDir);
        DotNetCoreClean(solutionPath);
});


Task("Restore")
    .Does(() => {
        DotNetCoreRestore(solutionPath);
});

Task("Build")
    .IsDependentOn("Clean")
    .IsDependentOn("Restore")
    .Does(() => {
        DotNetCoreBuild(
            solutionPath,
            new DotNetCoreBuildSettings 
            {
                Configuration = configuration
            }
        );
});

Task("Test")
    .Does(() => {
        var settings = new DotNetCoreTestSettings
        {
            ArgumentCustomization = args => args.Append("/p:CollectCoverage=true")
                                                .Append("/p:CoverletOutputFormat=opencover")
                                                .Append("/p:CoverletOutput=./" + coverageResultsFileName)
        };
        DotNetCoreTest(testProject, settings);
        MoveFile(testFolder + coverageResultsFileName, artifactsDir + coverageResultsFileName);
});

Task("UploadCoverage")
    .IsDependentOn("Test")
    .Does(() =>
    {
        CoverallsIo(artifactsDir + coverageResultsFileName, new CoverallsIoSettings()
        {
            RepoToken = coverallsToken
        });
});

Task("Package")
    .Does(() => {
        var settings = new DotNetCorePackSettings
        {
            OutputDirectory = artifactsDir,
            NoBuild = true
        };
        DotNetCorePack(project, settings);
});

Task("Publish")
    .IsDependentOn("Package")
    .Does(() => {
        var pushSettings = new DotNetCoreNuGetPushSettings 
        {
            Source = nugetSource,
            ApiKey = nugetApiKey
        };

        var pkgs = GetFiles(artifactsDir + "*.nupkg");
        foreach(var pkg in pkgs) 
        {
            if(!IsNuGetPublished(pkg)) 
            {
                Information($"Publishing \"{pkg}\".");
                DotNetCoreNuGetPush(pkg.FullPath, pushSettings);
            }
            else {
                Information($"Bypassing publishing \"{pkg}\" as it is already published.");
            }
            
        }
});

private bool IsNuGetPublished(FilePath packagePath) {
    var package = new ZipPackage(packagePath.FullPath);

    var latestPublishedVersions = NuGetList(
        package.Id,
        new NuGetListSettings 
        {
            Prerelease = true
        }
    );

    return latestPublishedVersions.Any(p => package.Version.Equals(new SemanticVersion(p.Version)));
}

Task("BuildAndTest")
    .IsDependentOn("Build")
    .IsDependentOn("Test");

Task("CompleteWithoutPublish")
    .IsDependentOn("Build")
    .IsDependentOn("Test")
    .IsDependentOn("UploadCoverage");

if(isReleaseBuild)
{
    Information("Release build");
    Task("Complete")
        .IsDependentOn("Build")
        .IsDependentOn("Test")
        .IsDependentOn("UploadCoverage")
        .IsDependentOn("Publish");
}
else
{
    Information("Development build");
    Task("Complete")
        .IsDependentOn("Build")
        .IsDependentOn("Test")
        .IsDependentOn("UploadCoverage");
}

Task("Default")
    .IsDependentOn("Restore");


RunTarget(target);