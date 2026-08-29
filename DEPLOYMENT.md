# Vercel and Neon deployment

CI and both container stages use exact .NET servicing versions. Container references
also include the official multi-platform manifest digest, so rebuilding the same commit
cannot silently select a different base image. Update the SDK tag, runtime tag, and both
digests together after reviewing each Microsoft servicing release, then run
`scripts/verify-toolchain-pins.ps1`, the complete CI suite, and Preview smoke tests.
Because .NET 8 support ends in November 2026, schedule and verify the .NET 10 LTS upgrade
before that deadline rather than continuing to ship an unsupported pinned runtime.
Third-party GitHub Actions are likewise pinned to full reviewed commit SHAs, with the
human-readable release beside each reference, and CI uses an explicit Ubuntu runner
release. Dependabot or a reviewed maintenance change should update the release comment
and SHA together; never accept a moving major tag or `*-latest` runner alias.

`.github/workflows/ci.yml` is intentionally the repository's only GitHub Actions
workflow. Production deployment remains owned by Vercel; CI rejects an additional
workflow so a stale Azure or other unreviewed deployment channel cannot silently regain
credentials or publish an artifact. Dependabot checks NuGet, pinned GitHub Actions, and
container bases weekly with bounded pull-request queues. Minor and patch servicing may
be grouped, but every update still passes locked restore, vulnerability scanning,
migration drift, tests, container construction, and Preview acceptance before promotion.
Major framework or provider upgrades remain separate reviewed changes.

## Required environment variables

Copy the variable names from `.env.example` into the Vercel project. Never commit
their values. Neon supplies `DATABASE_URL`; use its pooled URL for the API.

Use separately generated JWT signing and email-confirmation keys. Configure `CORS_ALLOWED_ORIGINS` as a
comma-separated list of exact HTTPS origins. Set `EnableSwagger=true` only for a
deliberately exposed environment.

`App__FrontendUrl` must be an HTTPS web base URL without credentials, query, or
fragment. `App__LogoUrl` must be HTTPS without credentials or fragment; a CDN signed
query is allowed. Development permits HTTP only on loopback hosts.

SMTP configuration is required in Production because registration, confirmation,
and contact notifications depend on the durable email outbox. Configure
`Email__SmtpHost`, `Email__SmtpPort`, `Email__Username`, `Email__Password`, and
`Email__From` in every Production and Preview environment used for acceptance.
`Email__EnableSsl` defaults to `true` and is mandatory in Production;
`Email__TimeoutMilliseconds` defaults to `30000`. Use the TLS-capable SMTP submission
port required by the provider (commonly 587 when
STARTTLS is enabled), and ensure the configured sender is permitted for the account.
`Email__SmtpHost` must contain only a DNS hostname or IP address—not `smtp://`, a path,
embedded credentials, surrounding whitespace, or control characters. Runtime failure
logs intentionally omit the configured host, all addresses, and provider messages.

## Bootstrap order

1. Install the Vercel CLI and authenticate with `vercel whoami`.
2. Link this repository to the intended Vercel project.
3. Install Neon through the Vercel Marketplace and attach it to the project.
4. Pull environment variables locally and verify all names in `.env.example` exist.
5. Run the PostgreSQL migration explicitly using Neon's direct/admin URL when available.
6. Push or merge to `main` to create a production deployment. Automatic Vercel
   deployments are disabled for every other Git branch in `vercel.json`.
7. Configure `ApplicationSettings__JWT_Issuer`, `ApplicationSettings__JWT_Audience`,
   and independently generated JWT, email-confirmation, and Cron secrets of at least
   32 bytes. Vercel uses
   `CRON_SECRET` to authenticate the scheduled idempotent maintenance requests.

Production startup intentionally fails if any secret is short, repeated, recognizable
as a placeholder, or reused across purposes, or if `CORS_ALLOWED_ORIGINS` contains no
origin. On Vercel, a Neon `DATABASE_URL` must use the `-pooler` endpoint; migration
tooling accepts only the direct `DATABASE_URL_UNPOOLED`. Verify these settings in
Preview before promoting the deployment.
Every non-loopback PostgreSQL runtime and migration connection must use `SSL Mode=Require`,
`VerifyCA`, or `VerifyFull`; Production and design-time startup reject plaintext or
opportunistic `Disable`, `Allow`, and `Prefer` modes without exposing connection details.
PostgreSQL URI values default to `Require`, preserve explicit `verify-ca`/`verify-full`
and `channel_binding=require`, and reject ambiguous duplicate or malformed security
parameters. URI fragments are forbidden rather than silently ignored.

## Local development

Vercel Development environment variables are the source of truth for local
configuration. Do not duplicate their values in .NET User Secrets and never
manually edit `.env.local`. The Vercel CLI may generate that ignored file as a
local cache; refresh it only through Vercel. Start the API with the variables
injected into the process:

```powershell
npx vercel env run -- dotnet run --project Portfolio
```

Run EF Core commands through the same wrapper so the design-time factory receives
`DATABASE_URL_UNPOOLED`:

```powershell
npx vercel env run -- dotnet ef database update --project DataAccess --startup-project DataAccess
```

For Visual Studio, select the `Vercel Development` launch profile and press F5.
Every non-design-time Debug build refreshes the ignored `.env.local` cache from
Vercel Development before launching. The application loads that cache only when
`ASPNETCORE_ENVIRONMENT=Development`; deployed environments continue to use their
injected process environment variables.

Do not run EF Core migrations from application startup. For later releases, apply a
backward-compatible database migration before deploying code that depends on it.

`BoundMutableTextColumns` is a constraint-enforcement migration, not an additive-first
migration. Deploy and qualify the API validation/password-hash compatibility code first.
In a later release, generate and review the migration SQL, run its no-truncation
preflight against Production, correct any named oversized rows through an audited data
change, and only then apply the `text`-to-`varchar` constraints. Do not promote code that
assumes those database constraints until the enforcement migration succeeds. The
migration uses transaction-local five-second lock acquisition and five-minute statement
timeouts: a busy database causes a clean rollback and a rescheduled maintenance-window
attempt instead of allowing schema locks to queue indefinitely behind live traffic.

`EnforceStateInvariants` is another second-release enforcement migration. It adds checks
as `NOT VALID` first and validates them afterward, reducing the lock level held during
legacy-row scans while still rejecting new impossible states immediately. Its validation
failure names the violated constraint; correct the underlying rows through an audited
change and retry. It uses the same transaction-local lock and statement timeouts.

`AppendOwnerCollectionOrdering` is backward-compatible with the preceding application
release and may be applied before the dependent deployment. It initializes one bounded
counter per owner and collection from the existing maximum order, then installs insert
triggers for projects, education, experience, and certificates. New rows whose order is
zero are assigned an atomic append position; explicit positive import positions remain
unchanged. The counter can remain ahead after a reorder, so later appends may contain
gaps, but positions remain unique and deterministically sort after the current items.
The migration uses transaction-local lock and statement timeouts; retry it in a quiet
window if creation of its table, function, or triggers cannot acquire locks promptly.

Email delivery uses the durable `EmailOutboxMessage` table. Apply the
`AddDurableEmailOutbox` migration before deploying code that enqueues messages. Set
`CRON_SECRET` in Vercel; the platform calls `/api/maintenance/email-outbox` daily at
07:17 UTC and `/api/maintenance/cleanup` daily at 03:17 UTC with its bearer authorization
header. Both schedules are compatible with Vercel Hobby. Registration, resend, and
unconfirmed-login requests attempt their committed confirmation message immediately;
the daily job recovers messages left pending by delivery or process failure. Confirmation
links are created when dispatch starts, remain valid for 15 minutes, and tell recipients
how to request another confirmation email after expiration. A failed immediate attempt
can wait until the next daily recovery run unless the user requests another message.
Each recovery invocation drains up to ten 20-message batches (200 messages total) and
uses a 240-second endpoint timeout below Vercel Hobby's 300-second Function limit. A
full 200-message run emits a backlog warning so capacity pressure is visible instead of
silently carrying work into the following day.

Confirmation-token derivation uses `Security__EmailConfirmationSecret`, not the JWT
signing key, so routine JWT rotation cannot invalidate queued or delivered confirmation
links. To rotate the confirmation key without breaking queued work, first copy the old
value to `Security__PreviousEmailConfirmationSecret`, set a new independent current
value, and deploy. Remove the previous value only after the longest pending-link, retry,
and operator-replay window has elapsed or all old-key work has been explicitly resolved.
Never reuse the JWT or Cron key for either confirmation-key slot.

Preview qualification deliberately omits cron registration so it can run on a Hobby
team without weakening the production schedule. Deploy it with the checked-in Preview
configuration, then run the smoke harness against the returned URL:

```powershell
npx vercel deploy --yes --local-config vercel.preview.json
pwsh ./scripts/smoke-preview.ps1 -BaseUrl 'https://<preview>.vercel.app' -UseVercelCli
```

`-UseVercelCli` uses the authenticated CLI's deployment-protection bypass and does not
require copying a bypass secret into the shell. CI or another noninteractive runner may
instead provide `VERCEL_AUTOMATION_BYPASS_SECRET`; both transports execute the same
acceptance inventory.

Never use `vercel.preview.json` for Production promotion. Deploy Production with
`--local-config vercel.json`. The canonical root file must remain in the container
upload: excluding it causes Vercel to deploy the API successfully while silently
registering zero cron definitions. CI verifies that the Production file retains the
reviewed daily jobs, the Preview file has an explicit empty cron set, and
`.vercelignore` does not exclude `vercel.json`. Vercel invokes cron jobs only for
Production deployments, so uploading the root configuration does not schedule jobs
for a Preview deployment.

The currently deployed legacy release reads raw refresh tokens, while the hardened
release normally reads hashes only. Use the explicit transition sequence below; applying
`HashLegacyRefreshTokens` directly against the legacy release invalidates refresh for
active users, and deploying hash-only lookup first rejects their existing rows.

## Migration commands

Prefer the review-first harness for releases. It rejects a Neon pooler host even when it
is mistakenly stored in `DATABASE_URL_UNPOOLED`, fails on un-migrated model changes,
requires explicit starting and ending migrations, generates only that idempotent SQL
range, and prints its SHA-256. Apply refuses an already-applied target, a downgrade, or a
database whose current migration does not exactly match `FromMigration`. Regenerate the
same bounded range and supply the reviewed hash explicitly:

```powershell
& ./scripts/prepare-migration.ps1 `
    -FromMigration '20260824104401_InitialPostgreSql' `
    -ToMigration '20260824211615_CentralizePersistenceConventions' `
    -OutputPath 'artifacts/01-foundation.sql' -Force
# Review artifacts/01-foundation.sql and record the printed hash through change approval.
& ./scripts/prepare-migration.ps1 `
    -FromMigration '20260824104401_InitialPostgreSql' `
    -ToMigration '20260824211615_CentralizePersistenceConventions' `
    -OutputPath 'artifacts/01-foundation.sql' `
    -Apply -Force -ReviewedSha256 '<approved SHA-256>'
```

Use these reviewed boundaries in order; generate, inspect, hash, and apply each row
separately:

| Stage | From | Through | Deployment condition |
| --- | --- | --- | --- |
| 1 | `InitialPostgreSql` | `CentralizePersistenceConventions` | Resolve duplicate-data preflights first |
| 2 | `CentralizePersistenceConventions` | `EnforceSingleActiveEmailConfirmation` | No unresolved active confirmations |
| 3 | `EnforceSingleActiveEmailConfirmation` | `RestrictPhysicalCascadeDeletes` | Maintenance-aware foreign-key lock review |
| 4 | `RestrictPhysicalCascadeDeletes` | `AddDurableEmailOutbox` | Additive application schema is now available |
| 5 | `AddDurableEmailOutbox` | `HashLegacyRefreshTokens` | First deploy this code with `Security__AllowLegacyRefreshTokenLookup=true` |
| 6 | `HashLegacyRefreshTokens` | `EnforceActiveCertificateMediaUniqueness` | Review every size/uniqueness preflight |
| 7 | `EnforceActiveCertificateMediaUniqueness` | `BoundMutableTextColumns` | Correct every reported oversized row |
| 8 | `BoundMutableTextColumns` | `EnforceStateInvariants` | Correct every invalid state before validation |
| 9 | `EnforceStateInvariants` | `AppendOwnerCollectionOrdering` | Quiet-window trigger/counter installation |
| 10 | `AppendOwnerCollectionOrdering` | `IndexContactSubmissionCooldown` | Quiet-window bounded-lock index build |

After stage 9 and smoke verification, set
`Security__AllowLegacyRefreshTokenLookup=false` in Preview and Production and redeploy
the same qualified artifact. The setting defaults to false and logs a critical startup
event while enabled. New and successfully rotated sessions are always stored hashed;
the compatibility lookup exists only to bridge pre-migration rows. Remove the setting
entirely in the next release after confirming no unhashed rows remain. Never roll back
past the compatibility release after stage 5 because hashing is irreversible.

Review the generated script before applying `CentralizePersistenceConventions`.
Its duplicate-data guards intentionally abort the migration instead of deleting or
merging production records. Resolve reported case-insensitive email, token, or active
user-skill duplicates through an audited data correction, regenerate the script, and
retry the migration before deploying dependent code.

`IndexContactSubmissionCooldown` adds the partial `(UserID, Email, CreatedAt)` index
used by the durable sender/recipient cooldown predicate. PostgreSQL's ordinary index
build blocks writes to `ContactMessage`, so apply this small additive stage in a quiet
window. Its five-second lock timeout fails cleanly instead of waiting behind active
contact submissions, and its five-minute statement timeout bounds the build itself.
Regenerate and retry the same reviewed stage if either timeout is reached.

EF's migration-list command can return exit code zero after a database authentication
or connectivity failure while reporting every migration's applied state as unknown.
The checked-in apply harness treats any such `null` state as a hard failure; never infer
that the database is empty from an unannotated list or bypass this guard.

## Verification

Before Production promotion, configure the telemetry destination, alerts, on-call
destinations, and incident procedures in `OPERATIONS.md`.

After every Production deployment, verify that Vercel registered and enabled the two
reviewed daily jobs rather than relying only on the local JSON file:

```powershell
& ./scripts/audit-vercel-crons.ps1
```

Run `scripts/audit-vercel-environment-security.ps1` from the authenticated linked
project before every promotion. It reads environment-variable metadata only and fails
when required credentials are missing, duplicated, incorrectly scoped, or not Vercel
Sensitive. It never retrieves or prints their values. Vercel currently requires an
existing legacy encrypted variable to be split/removed and re-added to make it
Sensitive; rotate the credential during that conversion rather than copying it through
logs or a shared file.

Run the reproducible read-only Preview acceptance harness. If Vercel Deployment
Protection is enabled, place its automation bypass credential in the environment; never
pass it as a command-line argument:

```powershell
$env:VERCEL_AUTOMATION_BYPASS_SECRET = '<temporary protected value>'
pwsh ./scripts/smoke-preview.ps1 -BaseUrl 'https://deployment.example.vercel.app'
Remove-Item Env:VERCEL_AUTOMATION_BYPASS_SECRET
```

The harness verifies liveness/readiness, the `/health` alias, successful legacy and
explicit-v1 public reads, response-shape compatibility, unsupported-v2 routing,
validation Problem Details, owner authentication challenge behavior, maintenance-secret
protection, cache policy, correlation IDs, and security headers. It does not invoke a
state-changing maintenance operation with valid credentials.

Call `GET /health/live` for process liveness and `GET /health/ready` for database
readiness. `GET /health` remains a compatibility alias. Readiness failure returns
HTTP 503 without connection details.

Verify the OpenAPI document and UI through NSwag only when `EnableSwagger=true`;
Swagger remains disabled by default outside Development.
