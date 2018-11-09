var target = Argument("target", "Default");

Task("Default")
  .Does(() =>
{
  MSBuild("../BSN.Commons.sln");
});

RunTarget(target);