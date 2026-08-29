# Performance measurement

Performance targets are established from Preview and production evidence, not invented before traffic is measured. Keep this file with the release evidence and update the baseline table after each material query or infrastructure change.

## Dataset profile

Use an anonymized Preview database representative of the expected moderate SaaS workload:

- at least 10,000 confirmed users;
- 20 projects, 10 experiences, 10 education records, 20 certificates, and 30 skills for sampled portfolio owners;
- at least 100 contact messages for authenticated workflow samples;
- realistic soft-deleted rows and relation density;
- no production credentials, messages, biographies, or other personal content.

Apply all migrations before measurement. Warm each endpoint once, then capture a cold serverless invocation separately from warm steady-state samples.

Create the dataset only on a disposable, isolated Preview branch. The checked-in seeder ignores `DATABASE_URL` and `DATABASE_URL_UNPOOLED`; it accepts only `PERFORMANCE_DATABASE_URL_UNPOOLED`, verifies a direct Neon endpoint, requires the exact database name twice, refuses pending migrations, and refuses a target with more than five non-performance users by default. Credentials are read from process environment variables and are not printed:

```powershell
$secureUrl = Read-Host 'Paste isolated Preview direct database URL' -AsSecureString
$pointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($secureUrl)
$securePassword = Read-Host 'Choose the disposable performance-owner password' -AsSecureString
$passwordPointer = [Runtime.InteropServices.Marshal]::SecureStringToBSTR($securePassword)
try {
  $env:PERFORMANCE_DATABASE_URL_UNPOOLED = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($pointer)
  $env:PERFORMANCE_OWNER_PASSWORD = [Runtime.InteropServices.Marshal]::PtrToStringBSTR($passwordPointer)
  dotnet run --project ./tools/Portfolio.PerformanceSeeder --configuration Release --no-build -- `
    --expected-database '<database name from the URL>' `
    --expected-host '<exact direct Preview endpoint hostname from the URL>' `
    --confirm SEED_ISOLATED_PREVIEW |
    Set-Content ./scripts/performance-dataset.local.json -Encoding utf8NoBOM
}
finally {
  Remove-Item Env:PERFORMANCE_DATABASE_URL_UNPOOLED,Env:PERFORMANCE_OWNER_PASSWORD -ErrorAction SilentlyContinue
  [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($pointer)
  [Runtime.InteropServices.Marshal]::ZeroFreeBSTR($passwordPointer)
  Remove-Variable secureUrl,pointer,securePassword,passwordPointer -ErrorAction SilentlyContinue
}
```

The seed is idempotent by the `perf-` marker and emits a non-secret cardinality manifest. It creates 10,000 confirmed synthetic users and a `perf-owner` portfolio with the relation density described above. The `.local.json` output is ignored by Git. Never increase `--maximum-existing-users` merely to bypass a refusal; create another isolated branch instead.

## HTTP smoke measurement

Run the checked-in PowerShell harness against an HTTPS Preview deployment:

The harness requires PowerShell 7 or later (`pwsh`); Windows PowerShell 5.1 does not
support its parallel-request or HTTP status-inspection primitives.

```powershell
pwsh ./scripts/measure-api.ps1 `
  -BaseUrl https://preview.example.com `
  -Iterations 200 `
  -Concurrency 8
```

Add representative public usernames to `-Paths` when measuring full portfolio projections. Never pass access tokens or passwords on a shared command line; authenticated owner workflows should use a temporary test account and process-scoped secrets.

For declarative public and owner scenarios, copy the checked-in example to an ignored temporary file, adjust every mutation for a disposable test account, and provide credentials only through process environment variables:

```powershell
$env:PERFORMANCE_OWNER_EMAIL = 'perf-owner@example.invalid'
$env:PERFORMANCE_OWNER_PASSWORD = '<same disposable password used by the seeder>'
$env:PERFORMANCE_TRUSTED_ORIGIN = '<configured Preview frontend origin>'
$env:VERCEL_AUTOMATION_BYPASS_SECRET = '<Preview protection bypass secret>'
pwsh ./scripts/measure-api.ps1 `
  -BaseUrl https://preview.example.com `
  -ScenariosFile ./scripts/performance-scenarios.local.json `
  -DatasetManifest ./scripts/performance-dataset.local.json `
  -OutputJson ./artifacts/performance-preview.json `
  -Iterations 200 `
  -Concurrency 8
```

The harness can authenticate once through the normal login flow using the disposable owner's email/password, or accept `PERFORMANCE_BEARER_TOKEN` from a dedicated load-testing tool. It accepts GET, POST, PUT, PATCH, and DELETE scenarios with optional JSON bodies, per-scenario iteration overrides, and expected status codes. It performs one warmup per scenario unless `-SkipWarmup` is supplied, reports wall-clock p50/p95/p99 and the API's `Server-Timing` p95 when available, and fails on unexpected responses. It also reports cache hits and the number of usable versus discarded application-timing samples. Origin-generated `Server-Timing` values replayed by a shared cache, or values that exceed the observed client wall time beyond rounding tolerance, are excluded from application percentiles instead of being misreported as measurements of the current request. Cookie-authenticated mutations also require the configured trusted frontend origin so the request exercises the real CSRF policy. The example profile edit is limited to one measured iteration and is intentionally state-changing; use it only against the disposable Preview owner.

`-DatasetManifest` rejects insufficient cardinality and resolves `{{ownerUsername}}` in scenario paths. `-OutputJson` records the deployment URL, timestamp, concurrency, dataset evidence, and results together. This prevents a sparse-data smoke run from being mistaken for representative performance acceptance.

## Baseline record

| Date | Deployment | Dataset | Flow | Requests | Errors | p50 | p95 | p99 | Notes |
| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| 2026-08-27 | `dpl_j7vFuchkvkhLTAJjjZuww1Gr61Bp` Preview | Existing sparse Preview data | Liveness | 50 | 0 | 331.5 ms | 2,227.6 ms | 2,986.2 ms | Preliminary protected-Preview wall-clock sample at concurrency 4; includes serverless/platform variance |
| 2026-08-27 | `dpl_j7vFuchkvkhLTAJjjZuww1Gr61Bp` Preview | Existing sparse Preview data | Public user list | 50 | 0 | 293.0 ms | 329.4 ms | 607.1 ms | Preliminary protected-Preview wall-clock sample at concurrency 4; not representative-scale evidence |
| 2026-08-27 | `dpl_HMvfxdb7tSxrc2GuHUaBiwxNkHUy` isolated Preview | Production-branch clone after all reviewed migrations | Liveness | 50 | 0 | 399.9 ms | 2,360.5 ms | 3,469.2 ms | Post-migration protected-Preview sample at concurrency 4; includes serverless/platform variance |
| 2026-08-27 | `dpl_HMvfxdb7tSxrc2GuHUaBiwxNkHUy` isolated Preview | Production-branch clone after all reviewed migrations | Public user list | 50 | 0 | 295.2 ms | 389.6 ms | 397.1 ms | Post-migration protected-Preview sample at concurrency 4; sparse clone, not representative-scale evidence |
| Pending | Preview | Pending representative seed | Public user list | — | — | — | — | — | Record after Preview deployment |
| Pending | Preview | Pending representative seed | Public portfolio projection | — | — | — | — | — | Record after Preview deployment |
| Pending | Preview | Pending representative seed | Owner edit workflow | — | — | — | — | — | Record after Preview deployment |

## Preview smoke evidence

The 2026-08-25 Preview deployment was exercised against a one-user development dataset. These are single-request `Server-Timing` application durations, not release thresholds or a representative load baseline:

| Flow | Status | Application duration | Notes |
| --- | ---: | ---: | --- |
| Liveness | 200 | 80.8 ms | Process-only check |
| Readiness | 200 | 1,606.3 ms | Cold/near-cold PostgreSQL connection |
| Public user list | 200 | 882.0 ms | Cold/near-cold database-backed request |
| Public portfolio projection | 200 | 209.4 ms | One user with empty related collections; privacy fields suppressed |

Deployment evidence: `dpl_AxFsmHvvfAg8eLreQaR8Jcwkwa6W`. Repeat the load harness with the representative anonymized dataset above before setting SLOs or making caching decisions.

The 2026-08-27 preliminary load sample used a named, temporary Vercel Automation
Bypass credential. The credential was created only for the run, kept in process memory,
revoked in a `finally` block, and independently confirmed absent afterward; an anonymous
request again received the expected Deployment Protection redirect. The liveness sample's
high wall-clock tail demonstrates platform/cold-instance variance. The user-list response
also exposed a cached origin `Server-Timing` application duration greater than its measured
wall time. That historical header sample remains excluded from the table; the harness now
detects cache-hit/aged responses and physically impossible timing values automatically.
No release threshold or caching decision should be derived from this sparse-data run.

Define latency and error-rate release thresholds only after a stable baseline and production traffic distribution are available. Investigate query counts and generated SQL before adding caches. A cache is accepted only when measurements show a meaningful improvement without weakening ownership or freshness guarantees.

Public portfolio queries use split-query projection with explicit 100-item limits on every top-level and nested collection. Integration verification asserts a fixed upper bound of 30 database commands for the complete projection, preventing collection size from turning into per-row N+1 queries. Preview still uses a one-user sparse dataset, so this structural guard is not a substitute for the representative load baseline above.

All offset-based list contracts cap the combined offset at 100,000 rows in addition to
their page-number and 100-item page-size bounds. Requests beyond that window fail model
validation before PostgreSQL executes an ordered scan. Introduce a versioned cursor-based
contract if measured product requirements need traversal beyond this compatibility window.
