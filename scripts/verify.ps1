param(
    [switch]$Coverage
)

$ErrorActionPreference = 'Stop'
$solution = Join-Path $PSScriptRoot '..\HospitalManagementSystem.sln'

dotnet restore $solution
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
dotnet build $solution --no-restore --configuration Release --verbosity minimal
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

dotnet test $solution --no-build --configuration Release --verbosity minimal
if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }

if ($Coverage) {
    Write-Host "Collecting coverage for the test project..."
    dotnet test (Join-Path $PSScriptRoot '..\src\HospitalManagement.Tests\HospitalManagement.Tests.csproj') --no-build --configuration Release --collect:"XPlat Code Coverage" --verbosity minimal
    if ($LASTEXITCODE -ne 0) { exit $LASTEXITCODE }
}
