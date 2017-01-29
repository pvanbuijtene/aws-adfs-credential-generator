#addin "Cake.FileHelpers"

var target      = Argument("target", "Default");
var configuration = Argument("configuration", "Release");
var artifactsDir    = Directory("./artifacts");
var solution    = "./src/AwsAdfsCredentialGenerator.sln";

Task("Clean")
    .Does(() =>
{
    CleanDirectory(artifactsDir);
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

Task("Zip")
    .IsDependentOn("Build")
    .Does(() =>
{
    var version = FileReadText("version.txt");
    Zip("./src/AwsAdfsCredentialGenerator/bin/" + configuration,
        "./artifacts/AwsAdfsCredentialGenerator." + version + ".zip");
});

Task("Default")
    .IsDependentOn("Update-Version")
    .IsDependentOn("Zip");

RunTarget(target);