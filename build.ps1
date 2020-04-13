# ----------------------------------------------
# Build script
# ----------------------------------------------

param
(
    [switch] $Release,
    [switch] $ExcludeSamples,
    [switch] $Pack,
    [switch] $Run
)

# ----------------------------------------------
# Main
# ----------------------------------------------

$ErrorActionPreference = "Stop"

Import-module "$PSScriptRoot/.psscripts/build-functions.ps1" -Force

Write-BuildHeader "Starting Giraffe.Razor build script"

$giraffeRazor = "./src/Giraffe.Razor/Giraffe.Razor.fsproj"
$sampleApp    = "./samples/GiraffeRazorSample/GiraffeRazorSample.fsproj"

$version = Get-ProjectVersion $giraffeRazor
Update-AppVeyorBuildVersion $version

if (Test-IsAppVeyorBuildTriggeredByGitTag)
{
    $gitTag = Get-AppVeyorGitTag
    Test-CompareVersions $version $gitTag
}

Write-DotnetCoreVersions
Remove-OldBuildArtifacts

$configuration = if ($Release.IsPresent) { "Release" } else { "Debug" }

Write-Host "Building Giraffe.Razor..." -ForegroundColor Magenta
dotnet-build   $giraffeRazor "-c $configuration"

if (!$ExcludeSamples.IsPresent -and !$Run.IsPresent)
{
    Write-Host "Building and testing samples..." -ForegroundColor Magenta
    dotnet-build   $sampleApp
}

if ($Run.IsPresent)
{
    Write-Host "Launching sample application..." -ForegroundColor Magenta
    dotnet-build   $sampleApp
    dotnet-run     $sampleApp
}

if ($Pack.IsPresent)
{
    Write-Host "Packaging all NuGet packages..." -ForegroundColor Magenta
    dotnet-pack $giraffeRazor "-c $configuration"
}

Write-SuccessFooter "Giraffe.Razor build completed successfully!"