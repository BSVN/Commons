#tool "nuget:?package=coveralls.io&version=1.4.2"
#tool "nuget:?package=GitVersion.CommandLine&version=5.1.3"
#tool "nuget:?package=gitlink&version=3.1.0"
#tool "nuget:?package=GitReleaseNotes&version=0.7.1"
#tool "nuget:?package=NUnit.ConsoleRunner&version=3.11.1"

#addin "nuget:?package=Cake.Git&version=0.21.0"
#addin "nuget:?package=Nuget.Core&version=2.14.0"
#addin "nuget:?package=Cake.Coveralls&version=0.10.1"
#addin "nuget:?package=Cake.Coverlet&version=2.3.4"

using NuGet;

var target = Argument("target", "Default");
var artifactsDir = "./artifacts/";
var solutionPath = "../BSN.Commons.sln";
var projectName = "BSN.Commons";
var projectFolder = "../Source/";
var solutionVersion = "1.9.3";
var projects = new List<(string path, string name, string version)>
{
    ("BSN.Commons/", "BSN.Commons.csproj", solutionVersion),
    ("BSN.Commons.Users/", "BSN.Commons.Users.csproj", solutionVersion),
    ("BSN.Commons.PresentationInfrastructure/", "BSN.Commons.PresentationInfrastructure.csproj", solutionVersion),
    ("BSN.Commons.Orm.EntityFramework/", "BSN.Commons.Orm.EntityFramework.csproj", solutionVersion),
    ("BSN.Commons.Orm.EntityFrameworkCore/", "BSN.Commons.Orm.EntityFrameworkCore.csproj", solutionVersion)
};

var mainProject = "../Source/BSN.Commons/BSN.Commons.csproj";
var presentationProject = "../Source/BSN.Commons.PresentationInfrastructure/BSN.Commons.PresentationInfrastructure.csproj";
var testFolder = "../Test/";
var testProjects = new List<(string path, string name, string dll)>
{
    ("BSN.Commons.Tests/", "BSN.Commons.Tests.csproj", "bin/Release/net472/BSN.Commons.Tests.dll"),
    ("BSN.Commons.Orm.EntityFramework.Tests/", "BSN.Commons.Orm.EntityFramework.Tests.csproj", "bin/Release/net48/BSN.Commons.Orm.EntityFramework.Tests.dll"),
	("BSN.Commons.Orm.EntityFrameworkCore.Tests/", "BSN.Commons.Orm.EntityFrameworkCore.Tests.csproj", "bin/Release/netcoreapp3.1/BSN.Commons.Orm.EntityFrameworkCore.Tests.dll")
};
var coverageResultsFileName = "coverage.xml";
var testResultsFileName = "nunitResults.xml";
var currentBranch = Argument<string>("currentBranch", GitBranchCurrent("../").FriendlyName);
var isReleaseBuild = string.Equals(currentBranch, "master", StringComparison.OrdinalIgnoreCase);
var configuration = "Release";
var nugetApiKey = Argument<string>("nugetApiKey", null);
var coverallsToken = Argument<string>("coverallsToken", null);
var nugetSource = "https://api.nuget.org/v3/index.json";
var repoUrl = "https://github.com/BSVN/Commons.git";
var projectUrl = "https://github.com/BSVN/Commons";

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
        NuGetRestore(solutionPath);
});

Task("Version")
    .Does(() => {
        foreach (var project in projects)
        {
           var finalVersion=project.version;
           if (project.name.Contains("EntityFramework.csproj") || project.name.Contains("Users.csproj"))
           {
               string pureName = project.name.Remove(project.name.IndexOf(".csproj"));
               UpdateVersion(pureName + ".nuspec", projectFolder + project.path + "/Properties/AssemblyInfo.cs", finalVersion);
               continue;
           }
           else
           {
               UpdateVersion(projectFolder + project.path + project.name, finalVersion);
           }
        }
});

Task("Build")
    .IsDependentOn("Restore")
    .IsDependentOn("Clean")
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
    .IsDependentOn("Build")
    .Does(() => {
        foreach (var testProject in testProjects)
        {
            var specificCoverageResultsFileName = testProject.name + coverageResultsFileName;
            var specificTestResultsFileName = testProject.name + testResultsFileName;

            var settings = new DotNetCoreTestSettings {
                VSTestReportPath = new FilePath(artifactsDir + specificTestResultsFileName)
            };

            var coverletSettings = new CoverletSettings {
                CollectCoverage = true,
                CoverletOutputFormat = CoverletOutputFormat.opencover,
                CoverletOutputDirectory = Directory(artifactsDir),
                CoverletOutputName = specificCoverageResultsFileName
            };
    
            DotNetCoreTest(testFolder + testProject.path + testProject.name, settings, coverletSettings);


            if (AppVeyor.IsRunningOnAppVeyor)
                AppVeyor.UploadTestResults(artifactsDir + specificTestResultsFileName, AppVeyorTestResultsType.XUnit);
        }
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
    .IsDependentOn("Version")
    .Does(() => {
        var settings = new DotNetCorePackSettings
        {
            OutputDirectory = artifactsDir,
            NoBuild = true,
            Configuration = configuration
        };

        foreach (var project in projects)
        {
            if (project.name.Contains("EntityFramework.csproj") || project.name.Contains("Users.csproj"))
            {
                string pureName = project.name.Remove(project.name.IndexOf(".csproj"));
                var nuGetPackSettings = new NuGetPackSettings
                {   
                    BasePath = projectFolder + project.path + "bin/" + Directory(configuration),
                    OutputDirectory = artifactsDir,
                    ArgumentCustomization = args => args.Append("-Prop Configuration=" + configuration),
                    Files = new [] {
                        new NuSpecContent {
                            Source = pureName + ".dll",
                            Target = "lib"
                        }
                    }
                };

                NuGetPack(pureName + ".nuspec", nuGetPackSettings);

                continue;
            }
            DotNetCorePack(projectFolder + project.path + project.name, settings);
        }

        if (AppVeyor.IsRunningOnAppVeyor)
        {
            foreach (var file in GetFiles(artifactsDir + "**/*"))
                AppVeyor.UploadArtifact(file.FullPath);
        }
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

private void UpdateVersion(string projectPath, string version)
{
    Information("UpdateVersion .................................................");
    Information(projectPath);
    // Update projectPath.json
    string pureVersion = XmlPeek(projectPath, "//Version");
    Information(pureVersion);
    string assemblyVersion = XmlPeek(projectPath, "//AssemblyVersion");
    string fileVersion = XmlPeek(projectPath, "//FileVersion");

    var updatedProjectJson = System.IO.File.ReadAllText(projectPath)
        .Replace(pureVersion, version)
        .Replace(fileVersion, version)
        .Replace(assemblyVersion, version);

    System.IO.File.WriteAllText(projectPath, updatedProjectJson);
}

private void UpdateVersion(string nuspecPath, string assemblyInfoPath, string version)
{
    Information("UpdateVersion .................................................");
    Information(nuspecPath);
    // Update nuspec file
    string pureVersion = XmlPeek(nuspecPath, "//version");
    Information(pureVersion);

    var updatedProjectJson = System.IO.File.ReadAllText(nuspecPath)
        .Replace(pureVersion, version);

    System.IO.File.WriteAllText(nuspecPath, updatedProjectJson);

    var assemblyInfo = ParseAssemblyInfo(assemblyInfoPath);
    Information("Change AssemblyInfo.cs of " + assemblyInfoPath);
    Information("Before change version is: " + assemblyInfo.AssemblyVersion);
    CreateAssemblyInfo(assemblyInfoPath, new AssemblyInfoSettings {
        FileVersion = version,
        InformationalVersion = version,
        Version = version,
        CLSCompliant = assemblyInfo.ClsCompliant,
        Company = assemblyInfo.Company,
        ComVisible = assemblyInfo.ComVisible,
        Configuration = assemblyInfo.Configuration,
        Copyright = assemblyInfo.Copyright,
        Description = assemblyInfo.Description,
        Guid = assemblyInfo.Guid,
        InternalsVisibleTo = assemblyInfo.InternalsVisibleTo,
        Product = assemblyInfo.Product,
        Title = assemblyInfo.Title,
        Trademark = assemblyInfo.Trademark
    });
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
    .IsDependentOn("Build")
	.IsDependentOn("Test")
    .IsDependentOn("Package");


RunTarget(target);
