param(
    [string]$OutputPath = 'artifacts/migration.sql',
    [string]$FromMigration = '0',
    [string]$ToMigration,
    [switch]$Apply,
    [string]$ReviewedSha256,
    [switch]$Force
)

$ErrorActionPreference = 'Stop'
$connection = [Environment]::GetEnvironmentVariable('DATABASE_URL_UNPOOLED')
if ([string]::IsNullOrWhiteSpace($connection)) {
    throw 'DATABASE_URL_UNPOOLED is required. Migration tooling never falls back to DATABASE_URL.'
}
if ([string]::IsNullOrWhiteSpace($ToMigration)) {
    throw 'ToMigration is required so review and apply are bounded to an explicit migration stage.'
}
if ($Apply -and $ReviewedSha256 -notmatch '^[A-Fa-f0-9]{64}$') {
    throw 'Apply requires ReviewedSha256 containing the exact 64-character SHA-256 printed during review.'
}

$fullOutputPath = if ([IO.Path]::IsPathRooted($OutputPath)) {
    [IO.Path]::GetFullPath($OutputPath)
}
else {
    [IO.Path]::GetFullPath((Join-Path (Get-Location) $OutputPath))
}
if ((Test-Path -LiteralPath $fullOutputPath) -and -not $Force) {
    throw "Migration output already exists: $fullOutputPath. Use -Force only when intentionally regenerating it."
}
$outputDirectory = Split-Path -Parent $fullOutputPath
if (-not (Test-Path -LiteralPath $outputDirectory)) {
    New-Item -ItemType Directory -Path $outputDirectory | Out-Null
}

$migrationInventoryArguments = @(
    'ef', 'migrations', 'list',
    '--project', 'DataAccess',
    '--startup-project', 'DataAccess',
    '--configuration', 'Release',
    '--json')
if (-not $Apply) {
    $migrationInventoryArguments += '--no-connect'
}
$migrationInventoryOutput = & dotnet @migrationInventoryArguments 2>&1
if ($LASTEXITCODE -ne 0) {
    $migrationInventoryOutput | Write-Host
    throw 'EF migration inventory could not be read.'
}
$migrationInventoryText = $migrationInventoryOutput | Out-String
$jsonStart = $migrationInventoryText.IndexOf('[')
$jsonEnd = $migrationInventoryText.LastIndexOf(']')
if ($jsonStart -lt 0 -or $jsonEnd -le $jsonStart) {
    throw 'EF migration inventory did not contain a JSON migration list.'
}
$migrationInventoryJson = $migrationInventoryText.Substring(
    $jsonStart,
    $jsonEnd - $jsonStart + 1)
$migrationInventory = $migrationInventoryJson | ConvertFrom-Json
if ($Apply -and @($migrationInventory | Where-Object { $null -eq $_.applied }).Count -gt 0) {
    # `dotnet ef migrations list` can exit zero after a connection/authentication
    # failure and return every applied value as null. Never interpret that state
    # as an empty database or continue toward an update.
    throw 'EF could not verify the database migration state. Correct the direct credential or connectivity before applying migrations.'
}
$target = @($migrationInventory | Where-Object {
    $_.id -eq $ToMigration -or $_.name -eq $ToMigration -or $_.safeName -eq $ToMigration
})
if ($target.Count -ne 1) {
    throw "ToMigration must identify exactly one known PostgreSQL migration: $ToMigration"
}
$targetIndex = [array]::IndexOf([object[]]$migrationInventory, $target[0])
if ($FromMigration -ne '0') {
    $from = @($migrationInventory | Where-Object {
        $_.id -eq $FromMigration -or $_.name -eq $FromMigration -or $_.safeName -eq $FromMigration
    })
    if ($from.Count -ne 1) {
        throw "FromMigration must be '0' or identify exactly one known PostgreSQL migration: $FromMigration"
    }
    $fromIndex = [array]::IndexOf([object[]]$migrationInventory, $from[0])
    if ($fromIndex -ge $targetIndex) {
        throw 'FromMigration must precede ToMigration.'
    }
}
if ($Apply -and $target[0].applied -eq $true) {
    throw 'ToMigration is already applied. Refusing database update because EF could interpret an older target as a downgrade.'
}
if ($Apply) {
    $appliedMigrations = @($migrationInventory | Where-Object { $_.applied -eq $true })
    $actualCurrentMigration = if ($appliedMigrations.Count -eq 0) {
        '0'
    } else {
        [string]$appliedMigrations[-1].id
    }
    $expectedCurrentMigration = if ($FromMigration -eq '0') {
        '0'
    } else {
        [string]$from[0].id
    }
    if ($actualCurrentMigration -ne $expectedCurrentMigration) {
        throw "Database is currently at '$actualCurrentMigration', not the reviewed FromMigration '$expectedCurrentMigration'. Refusing to apply an unreviewed migration range."
    }
}

$modelCheckOutput = & dotnet ef migrations has-pending-model-changes `
    --project DataAccess `
    --startup-project DataAccess `
    --configuration Release 2>&1
$modelCheckExitCode = $LASTEXITCODE
$modelCheckOutput | Write-Host
if ($modelCheckExitCode -ne 0) {
    $modelCheckText = $modelCheckOutput | Out-String
    if ($modelCheckText -match 'Changes have been made to the model since the last migration') {
        throw 'The EF model has changes without a migration. Create and review the migration before release.'
    }

    throw 'EF migration-model verification could not run. Correct the configuration, credentials, or design-time startup failure before generating or applying migrations.'
}

& dotnet ef migrations script $FromMigration $ToMigration --idempotent `
    --project DataAccess `
    --startup-project DataAccess `
    --configuration Release `
    --output $fullOutputPath
if ($LASTEXITCODE -ne 0) { throw 'Idempotent migration script generation failed.' }

$actualSha256 = (Get-FileHash -Algorithm SHA256 -LiteralPath $fullOutputPath).Hash.ToUpperInvariant()
Write-Host "Migration script: $fullOutputPath"
Write-Host "SHA-256: $actualSha256"

if (-not $Apply) {
    Write-Host "Review only the bounded stage from '$FromMigration' through '$ToMigration' and its preflight guards."
    Write-Host 'Re-run with the same boundaries plus -Apply, -Force, and -ReviewedSha256 only after approval.'
    return
}
if ($actualSha256 -ne $ReviewedSha256.ToUpperInvariant()) {
    throw 'Generated migration SQL does not match ReviewedSha256; review the new script instead of applying it.'
}

& dotnet ef database update $ToMigration `
    --project DataAccess `
    --startup-project DataAccess `
    --configuration Release
if ($LASTEXITCODE -ne 0) { throw 'Database migration failed.' }
Write-Host "Database migration stage through '$ToMigration' applied from reviewed SQL state $actualSha256."
