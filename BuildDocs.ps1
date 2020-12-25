$workingDirectory = Split-Path -Path $MyInvocation.MyCommand.Path;

$buildFolder = $workingDirectory + "\build";
$docsFolder = $buildFolder + "\docs";

if(-not (Test-Path ($buildFolder + ".\Nuget.exe") -PathType Leaf))
{
    # Download nuget.exe in case it doesn't exist
    Invoke-WebRequest https://dist.nuget.org/win-x86-commandline/latest/nuget.exe -OutFile  ($buildFolder + "\Nuget.exe")
}
$NUGET_EXE = Join-Path $buildFolder "\Nuget.exe"

Invoke-Expression "$NUGET_EXE install docfx.console -Outputdirectory $docsFolder"

$docfx  = (Get-ChildItem -Path $scriptFolder -Filter "docfx.exe" -Recurse).Fullname

if($docfx -is [array])
{
    # There seem to be multiple installations, lets use the first found.
    $docfx = $docfx[0];
}

$configFile = $workingDirectory + "\docfx.json"

Invoke-Expression "$docfx $configFile"

# Delete api folder
Remove-Item ($workingDirectory + "/api") -Recurse
# Remove cache
Remove-Item ($workingDirectory + "/obj") -Recurse