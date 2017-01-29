#addin "Cake.FileHelpers"

var target      = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var buildDir    = Directory("./artifacts");
var solution    = "./src/AwsAdfsCredentialGenerator.sln";

Task("Clean")
    .Does(() =>
{
    CleanDirectory(buildDir);
});

Task("Restore-NuGet-Packages")
    .IsDependentOn("Clean")
    .Does(() =>
{
    NuGetRestore(solution);
});

Task("Update-Version")
    .Does(() =>
{
    var version = FileReadText("version.txt");
    ReplaceTextInFiles("src/AwsAdfsCredentialGenerator/AssemblyInfo.cs", "1.0.0.0", version);
});

Task("Build")
    .IsDependentOn("Restore-NuGet-Packages")
    .Does(() =>
{
    MSBuild(solution, settings => settings.SetConfiguration(configuration));
});

Task("Default")
    .IsDependentOn("Update-Version")
    .IsDependentOn("Build");

RunTarget(target);