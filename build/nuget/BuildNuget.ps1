$scriptFolder = Split-Path -Path $MyInvocation.MyCommand.Path;
$buildFolder = Split-Path -Path $scriptFolder;
$workingDirectory = Split-Path $buildFolder -Parent;

$nuspecFile = $scriptFolder + "\VisualTreeAnalyzers.UWP.nuspec";
$nugetPackageOutput = $workingDirectory + "\artifacts\package";

if(Test-Path ($buildFolder + "./Nuget.exe") -PathType Leaf)
{
    # Download nuget.exe in case it doesn't exist
}else {
    Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile  ($buildFolder + "/Nuget.exe")
}
$NUGET_EXE = Join-Path $buildFolder "/Nuget.exe"

Invoke-Expression "$NUGET_EXE pack $nuspecFile -OutputDirectory $nugetPackageOutput"