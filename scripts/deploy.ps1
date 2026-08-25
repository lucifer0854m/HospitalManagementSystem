param(
    [switch]$Build,
    [switch]$Detach
)

$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$environmentFile = Join-Path $projectRoot '.env'

if (-not (Test-Path $environmentFile)) {
    throw "Missing .env. Copy .env.example to .env and set strong database and administrator credentials."
}

$composeArguments = @('compose', '--env-file', $environmentFile, 'up')
if ($Build) { $composeArguments += '--build' }
if ($Detach) { $composeArguments += '--detach' }

Push-Location $projectRoot
try { & docker @composeArguments } finally { Pop-Location }
