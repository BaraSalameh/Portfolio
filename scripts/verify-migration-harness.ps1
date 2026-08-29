$ErrorActionPreference = 'Stop'
$harnessPath = Join-Path $PSScriptRoot 'prepare-migration.ps1'
$tokens = $null
$parseErrors = $null
[System.Management.Automation.Language.Parser]::ParseFile(
    $harnessPath,
    [ref]$tokens,
    [ref]$parseErrors) | Out-Null
if ($parseErrors.Count -gt 0) {
    throw "Migration harness contains PowerShell parse errors: $($parseErrors.Message -join '; ')"
}

$source = Get-Content -Raw -LiteralPath $harnessPath
foreach ($required in @(
    'DATABASE_URL_UNPOOLED', 'has-pending-model-changes', '--idempotent',
    'Get-FileHash', 'ReviewedSha256', 'database update', 'FromMigration',
    'ToMigration', 'already applied', '--json')) {
    if ($source -notmatch [regex]::Escape($required)) {
        throw "Migration harness is missing safety invariant: $required"
    }
}
foreach ($requiredApplyGuard in @('actualCurrentMigration', 'expectedCurrentMigration', 'unreviewed migration range')) {
    if ($source -notmatch [regex]::Escape($requiredApplyGuard)) {
        throw "Migration harness is missing stage-continuity guard: $requiredApplyGuard"
    }
}
foreach ($requiredConnectionGuard in @('$null -eq $_.applied', 'could not verify the database migration state')) {
    if ($source -notmatch [regex]::Escape($requiredConnectionGuard)) {
        throw "Migration harness is missing indeterminate-database-state guard: $requiredConnectionGuard"
    }
}
if ($source -match 'GetEnvironmentVariable\([''"]DATABASE_URL[''"]\)') {
    throw 'Migration harness must not read or fall back to the pooled DATABASE_URL.'
}
if ($source -notmatch 'database update \$ToMigration') {
    throw 'Migration apply must update only through the explicitly reviewed ToMigration boundary.'
}
if ($source -match 'migrations script --idempotent') {
    throw 'Migration generation must not use an unbounded all-pending script command.'
}
if ($source -match 'IsPathFullyQualified') {
    throw 'Migration harness must remain compatible with Windows PowerShell 5.1; use IsPathRooted.'
}

# Exercise the review path because PowerShell can parse a multiline method call
# differently from the static syntax check above. Use a syntactically valid local
# connection only; review mode lists migrations with --no-connect and does not
# update a database.
$migrationDirectory = Join-Path $PSScriptRoot '..\DataAccess\PostgreSqlMigrations'
$migrationIds = @(Get-ChildItem -LiteralPath $migrationDirectory -Filter '*.cs' |
    Where-Object { $_.Name -match '^\d{14}_.+\.cs$' -and $_.Name -notlike '*.Designer.cs' } |
    Sort-Object Name |
    Select-Object -ExpandProperty BaseName)
if ($migrationIds.Count -lt 2) {
    throw 'Migration harness execution check requires at least two PostgreSQL migrations.'
}
$reviewOutputPath = Join-Path ([IO.Path]::GetTempPath()) "portfolio-migration-review-$([Guid]::NewGuid()).sql"
$previousDirectConnection = [Environment]::GetEnvironmentVariable('DATABASE_URL_UNPOOLED')
try {
    [Environment]::SetEnvironmentVariable(
        'DATABASE_URL_UNPOOLED',
        'postgresql://model:model@localhost/model',
        'Process')
    & $harnessPath `
        -OutputPath $reviewOutputPath `
        -FromMigration $migrationIds[-2] `
        -ToMigration $migrationIds[-1] `
        -Force
    $reviewHash = if (Test-Path -LiteralPath $reviewOutputPath -PathType Leaf) {
        (Get-FileHash -Algorithm SHA256 -LiteralPath $reviewOutputPath).Hash
    }
    if (-not (Test-Path -LiteralPath $reviewOutputPath -PathType Leaf) -or
        (Get-Item -LiteralPath $reviewOutputPath).Length -eq 0 -or
        $reviewHash -notmatch '^[A-F0-9]{64}$') {
        throw 'Migration harness review path did not produce a non-empty hashed SQL artifact.'
    }
}
finally {
    [Environment]::SetEnvironmentVariable(
        'DATABASE_URL_UNPOOLED',
        $previousDirectConnection,
        'Process')
    Remove-Item -LiteralPath $reviewOutputPath -ErrorAction SilentlyContinue
}

Write-Host 'Migration harness verification passed.'
