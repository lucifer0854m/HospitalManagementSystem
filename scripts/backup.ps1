param(
    [Parameter(Mandatory)]
    [string]$OutputPath
)

$ErrorActionPreference = 'Stop'
$projectRoot = Split-Path -Parent $PSScriptRoot
$fullOutputPath = [System.IO.Path]::GetFullPath($OutputPath)
$outputDirectory = Split-Path -Parent $fullOutputPath
if (-not (Test-Path $outputDirectory)) { New-Item -ItemType Directory -Path $outputDirectory -Force | Out-Null }
$environmentFile = Join-Path $projectRoot '.env'
if (-not (Test-Path $environmentFile)) { throw 'Missing .env. Create it from .env.example first.' }
$passwordLine = Get-Content $environmentFile | Where-Object { $_ -match '^MSSQL_SA_PASSWORD=' } | Select-Object -First 1
if (-not $passwordLine) { throw 'MSSQL_SA_PASSWORD is missing from .env.' }
$sqlPassword = $passwordLine.Split('=', 2)[1]

Push-Location $projectRoot
try {
    & docker compose exec -T db /opt/mssql-tools18/bin/sqlcmd -S localhost -U sa -P $sqlPassword -C -Q "BACKUP DATABASE [HospitalManagementDB] TO DISK = N'/var/opt/mssql/data/HospitalManagementDB.bak' WITH INIT"
    if ($LASTEXITCODE -ne 0) { throw 'SQL Server backup failed.' }
    & docker compose cp db:/var/opt/mssql/data/HospitalManagementDB.bak $fullOutputPath
} finally { Pop-Location }
