# Production readiness

## Architecture boundaries

The application remains a modular monolith. `Domain` owns entities and rules and has no framework dependencies. `Application` owns use cases, contracts, and persistence abstractions under `Application.Common.Persistence` and has no ASP.NET dependency. `DataAccess` implements those abstractions and owns PostgreSQL, password hashing, token issuance, email delivery, and authentication registration. `Portfolio` owns HTTP result mapping, request identity, cookies, middleware, and composition. Database readiness and persistence-exception classification cross the boundary through Application interfaces, so the API project has no direct Entity Framework Core package or assembly reference. New dependencies must point inward; architecture tests enforce the Domain boundary, namespace ownership, and the `DataAccess -> Application -> Domain` project direction.

Existing `/api/{controller}/{action}` routes are the v1 compatibility surface. Breaking request or response changes require a new versioned route and a coordinated frontend migration.

## Deployment and migrations

1. Restore, build, test, scan packages, and build the container through CI.
2. Generate and review an idempotent migration script.
3. Apply additive PostgreSQL migrations using `DATABASE_URL_UNPOOLED` before deploying dependent code.
4. Deploy and verify `/health/live`, `/health/ready`, authentication, and a public portfolio read.
   Use `scripts/smoke-preview.ps1` so the protected Preview check also proves legacy/v1 shape compatibility, v2 rejection, RFC 7807 errors, cache/security headers, and maintenance authorization without mutating data.
   Hobby Preview deployments use `vercel.preview.json`, which intentionally omits cron
   registration while retaining the production cron policy in `vercel.json`.
5. Keep the previous deployment available for rollback. Destructive schema changes belong in a later release after old code is no longer running.

CI installs SDK 8.0.424 exactly, and the build/runtime container stages pin the .NET
8.0.424 SDK and ASP.NET Core 8.0.30 runtime to their official multi-platform manifest
digests. A checked-in verifier rejects floating SDK channels or un-digested .NET images.
Servicing updates require an explicit reviewed pin change and full Preview acceptance.
The workflow also pins every external GitHub Action to a full commit SHA and selects
Ubuntu 24.04 explicitly. The same reproducibility verifier rejects mutable action tags
and `*-latest` runner aliases before build or test execution. The PostgreSQL integration
service also retains a readable major-version tag while resolving through a reviewed
multi-platform manifest digest; the verifier rejects any floating CI service image.

CI is the only allowed GitHub Actions workflow; the obsolete Azure Production workflow
has been removed and the verifier rejects additional workflow files. Vercel remains the
sole deployment platform. Dependabot now proposes bounded weekly servicing updates for
NuGet, GitHub Actions, and container images, while major upgrades remain individually
reviewed and qualified.

The durable email dispatcher runs daily at 07:17 UTC and cleanup runs daily at 03:17 UTC.
A checked-in verifier prevents either schedule from drifting or the Preview configuration
from registering production jobs. Both schedules are compatible with Vercel Hobby.
Confirmation-producing requests dispatch their own committed outbox message immediately;
the accepted tradeoff is that a failed attempt can wait up to one day for cron recovery.
Confirmation and resend transactions share a PostgreSQL advisory lock derived from the
account ID. Resend reloads the account after acquiring that lock, so concurrent resends
produce only one replacement link and a resend racing with successful confirmation
cannot create a fresh link for an already-confirmed account. Suppressed concurrent calls
retain the public endpoint's generic-success response.

The application never applies migrations during startup. SQL Server migrations under `DataAccess/Migrations` are archive-only; `DataAccess/PostgreSqlMigrations` is the executable history.

Design-time tooling now validates that a Neon `DATABASE_URL_UNPOOLED` host is genuinely direct and rejects `-pooler` hosts. The review-first migration harness fails on model drift, requires explicit `FromMigration`/`ToMigration` boundaries, emits only that idempotent SQL range plus SHA-256, and requires the exact reviewed hash before apply. Apply also proves the database is currently at the reviewed starting migration and refuses already-applied targets or implicit downgrades.

Treat the pooled runtime and direct migration URLs as a credential pair: rotate or refresh both when Neon credentials change, and verify the direct connection before every migration. Vercel production fails startup when a Neon runtime host lacks the `-pooler` marker, and design-time tooling requires `DATABASE_URL_UNPOOLED` without fallback. Runtime Npgsql pools use the same bounded policy for URI and keyword connection strings: minimum 0, maximum 20 per instance, 60-second idle pruning, 10-second connection timeout, and 30-second command timeout.

Both URI and keyword Neon connection formats fail before use when the database, username,
or password is absent; validation errors identify only the setting and never echo its
contents. URI hosts are canonicalized through their ASCII host representation, including
removing IPv6 address brackets before passing the host to Npgsql.

Remote PostgreSQL runtime and migration connections must use `Require`, `VerifyCA`, or `VerifyFull` SSL mode. URI-style PostgreSQL URLs default to `Require` but preserve an explicit `verify-ca` or `verify-full` value and Neon's `channel_binding=require`; duplicate, missing, or unsupported security query parameters fail without exposing connection contents. Native keyword strings using plaintext or opportunistic modes likewise fail before connection. Loopback PostgreSQL remains available without TLS for local development and CI.

`AppDbContext` centrally assigns create/update/delete timestamps, converts tracked deletes of audited entities into soft deletes, and applies global active-record filters. Use `IgnoreQueryFilters()` only for explicit administrative, audit, or recovery workflows. The `CentralizePersistenceConventions` migration normalizes email casing, makes refresh revocation timestamps nullable, and adds indexes for token rotation, cleanup, owner ordering, messages, and active skill uniqueness. It fails before changing indexes if legacy duplicate tokens, emails, or active user-skill rows require manual reconciliation.

Required dependents carry matching active-principal filters so query results do not change unexpectedly when required navigations are included. All foreign keys use restrictive physical-delete behavior: ownership workflows must soft-delete or intentionally remove dependents instead of relying on database cascades. Administrative role and language deletion is rejected while the lookup is still in use. The `RestrictPhysicalCascadeDeletes` migration changes foreign-key actions and should be scheduled in its own maintenance-aware release after reviewing lock duration on a production-sized schema.

## Operations

The alert definitions, role-based escalation, incident playbooks, rollback rules, and
secret-rotation procedures are maintained in `OPERATIONS.md`. External telemetry and
real on-call destinations are an explicitly deferred post-launch integration and remain
tracked operational debt.

The intended modular-monolith shape, dependency rules, and accepted architecture
decisions are recorded in `ARCHITECTURE.md` and guarded by architecture tests.
JWT bearer extraction, validation, and authorization policies are now composed only in
Portfolio/API; Infrastructure retains token issuance but has no HTTP authentication or
request dependency. Architecture coverage requires every non-lookup Owner handler—not
only mutations—to consume authenticated user context. Certificate edits explicitly load
and reconcile their active media collection, preserve unchanged rows, soft-delete
replaced media, and cannot affect another owner. An active-only unique database index
prevents concurrent duplicate URLs per certificate and its migration rejects unresolved
legacy duplicates.

All 61 legacy action operations now have explicit `/api/v1/{controller}/{action}` aliases
using ASP.NET API Versioning while the original unversioned paths remain the assumed v1
compatibility surface. Unsupported URL versions do not route, and OpenAPI contract tests
prove every explicit-v1 response definition matches its legacy counterpart.
Path-sensitive security policy now recognizes both
legacy and versioned controller segments: account/admin/owner GET responses remain
non-cacheable, while public versioned reads retain their explicit cache policy. Refresh
cookies are issued and deleted at both narrow account paths, so explicit-v1 refresh and
anonymous logout work without exposing the credential to unrelated API routes. The
version-aware policy is covered by focused route and cookie tests.

Serverless retention cleanup now selects and deletes deterministic batches of at most
5,000 refresh tokens, outbox rows, and confirmations per invocation. Repeated full
batches emit a distinct maintenance metric outcome and operator warning instead of
allowing an unbounded delete to exceed the request timeout or hold long PostgreSQL locks.
Refresh-token cleanup is expiry-driven: revoked credentials remain as replay-detection
markers until their natural expiry, then the existing expiry index makes them eligible
for bounded deletion. This preserves family-wide replacement revocation throughout the
entire period in which a stolen credential could otherwise be replayed.
Expiry and lease comparisons use half-open validity semantics consistently: credentials
and leases are valid only while their deadline is greater than the current UTC instant,
and cleanup/reclaim operations become eligible at exact equality rather than one run
later. Retention cutoffs follow the same boundary rule.
Each outbox item now receives a fresh full lease when it is claimed rather than sharing
the batch-selection timestamp; slow earlier SMTP calls therefore cannot make a later
claim immediately reclaimable by an overlapping worker.
Completion and retry updates are conditional on the same claim identifier. If a worker
loses that lease during delivery, it no longer reports the message as processed or
throws while finalizing a stale claim; it records a bounded `lease_lost` delivery
outcome and leaves state under the current owner. Because SMTP cannot provide an atomic
commit with PostgreSQL, delivery remains intentionally at-least-once and templates must
remain safe for a rare duplicate after a process crash or expired lease.

Refresh rejection, revoked-token replay, and concurrent reuse now emit distinct
low-cardinality authentication failure reasons only after the revocation transaction
commits. Metrics never include the raw credential, its hash, or account identifiers, and
failed transactions do not produce false replay signals. The complete local suite
contains 367 tests.

Public contact submissions normalize the sender email and enforce a durable two-minute
cooldown for each sender/recipient pair before creating the message and outbox entry.
The check uses persisted contact history, so separate Vercel instances and cold starts
share the same decision; suppressed and unknown-recipient requests retain the legacy
generic-success response. The existing `(UserID, IsDeleted, CreatedAt)` index supports
the inbox scan, while the partial `(UserID, Email, CreatedAt)` index aligns with the
sender/recipient cooldown predicate. A PostgreSQL transaction-scoped advisory lock, derived from the
recipient and normalized sender, serializes simultaneous first submissions across API
instances; the cooldown recheck, contact insert, and outbox insert then commit in one
transaction. A rare 64-bit lock-key collision only serializes unrelated senders and
cannot bypass the guard. This persistence control complements, but does not replace,
the process-local IP token bucket or a deployment-level edge control.

Password hashing uses ASP.NET Identity v3 PBKDF2 with a startup-validated work factor.
The stateless password verifier is application-wide, so its dummy-account hash is paid
once per instance lifetime rather than on every request. Both known- and unknown-account login
paths consequently perform one bounded password verification, reducing timing leakage
and preventing nonexistent-account attempts from consuming twice the expected CPU.

The authorization fallback requires an authenticated, confirmed principal with a
non-empty GUID user identifier. Public endpoints must opt out explicitly, while role
controllers retain their narrower owner/admin policies. A newly added endpoint that
omits authorization metadata therefore cannot run under an anonymous, unconfirmed, or
structurally incomplete identity.

- Runtime logs are structured JSON and include `CorrelationId`. The same ID is returned through `X-Correlation-ID`.
- Global exception logs contain only the exception type, translated status, and trace ID. Exception messages and stack traces are deliberately excluded from log state so provider responses, internal paths, SQL details, and credentials cannot escape through the centralized error path.
- Exceptions raised after response headers have started are never rewritten as Problem Details. The handler records only sanitized type/trace metadata and declines handling so the server terminates the partial response without masking the original failure with a second response-write exception.
- Requests have a 30-second processing limit. Expired requests return a sanitized `504 application/problem+json` response with the correlation ID and increment `portfolio.http.request.timeouts`; response writing intentionally does not reuse the already-cancelled timeout token.
- Rate-limit rejections increment `portfolio.rate_limit.rejections` with a bounded policy label (`authentication`, `contact`, `global`, or `other`) before returning the existing RFC 7807 response. No client address, route value, account identifier, or credential enters the metric.
- Rate-limit buckets canonicalize IPv4-mapped IPv6 addresses to IPv4 and normalize IPv6 text, preventing one client from receiving multiple quotas solely because proxy/address representation changed. Requests without a resolved address share the single bounded `unknown` bucket.
- Custom request counters and latency histograms use the final exception-translated status code and the matched route template, never raw route values. This keeps 5xx alerts accurate without creating user-ID or username metric cardinality.
- Structured request-completion logs use the same matched route template, or the bounded `unmatched` sentinel. Raw paths and resource identifiers are never emitted by this centralized logging path.
- ASP.NET Core, outbound HTTP, runtime, and custom request telemetry is registered through OpenTelemetry. Set `OTEL_EXPORTER_OTLP_ENDPOINT` (and provider-specific `OTEL_EXPORTER_OTLP_HEADERS`) to export traces and metrics; no exporter connection is attempted when the endpoint is absent.
- Production accepts only HTTPS OTLP endpoints without embedded credentials, queries, or fragments. Development permits plaintext HTTP solely for loopback collectors, preventing accidental cleartext telemetry export while retaining local collector workflows.
- `/health/live` verifies process liveness. `/health/ready` verifies PostgreSQL with an eight-second timeout, chosen from Preview cold-start evidence; `/health` remains a compatibility alias for readiness.
- Vercel invokes `/api/maintenance/cleanup` daily. The route requires `Authorization: Bearer $CRON_SECRET` and is safe to retry.
- The production .NET 8 container runs as the built-in unprivileged `app` user. It writes logs only to standard output and does not require application-directory writes; development-only file-backed data-protection keys are never enabled in Production.
- No correctness-critical `BackgroundService` runs inside the serverless API process. Cleanup is set-based and owned by the authenticated maintenance route.
- Cookie-authenticated mutations require an exact configured Origin. Security and throttling failures use `application/problem+json` and include a trace ID.
- Production CORS/CSRF origins are canonical scheme-host-port HTTPS origins; paths, queries, fragments, embedded credentials, and wildcards fail startup validation. Development alone permits HTTP loopback origins.
- Forwarded client IP and scheme headers are trusted without a static proxy allowlist only when the platform-provided `VERCEL=1` sentinel is present. Vercel overwrites `X-Forwarded-For` at its edge; the same container retains ASP.NET's loopback-only proxy trust when run directly or on another host.
- Alert on sustained 5xx responses, readiness failures, database pool exhaustion, authentication spikes, SMTP failures, failed maintenance runs, and cron staleness using the initial conditions in `OPERATIONS.md` when the deferred external monitoring integration is implemented. Until then, use bounded post-deploy CLI scans and the Vercel runtime/Cron dashboards.

## Preview acceptance evidence (through 2026-08-28)

- Vercel Preview `dpl_Gg9sJG1F9dyuHNQ5Pa17Ev2NMGdV` (`portfolio-j3rzqp5jr-albaraa-salamas-projects.vercel.app`) deployed the bounded migration-harness and refresh-token transition build `READY` with `Security__AllowLegacyRefreshTokenLookup` absent, proving the secure default-off configuration starts against the fully migrated isolated Preview branch. The protected ten-check smoke suite passed; error-level, HTTP 500, and compatibility-warning log queries were empty. This validates the normal post-cutover artifact, not the temporary enabled transition state, which is covered by PostgreSQL integration tests and must be used only during the documented Production hash stage.
- Vercel Preview `dpl_DM4rrH8cFzeikZWHfAUv5QdKeeSg` (`portfolio-hpyfhjga4-albaraa-salamas-projects.vercel.app`) deployed the endpoint-specific legacy-resend browser-origin protection `READY` in `iad1` from the Preview-only configuration. A protected live request carrying `Sec-Fetch-Site: cross-site` and no trusted Origin received actual HTTP 403 with sanitized `application/problem+json`, proving the middleware blocks cross-site resend triggers before model validation or email enqueue. The complete ten-check protected smoke suite also passed, and subsequent error-level and HTTP 500 log scans were empty.
- Vercel Preview `dpl_BnUTDeHJpi54tFu7DJQaEaTdgWx5` (`portfolio-lh3pj1de9-albaraa-salamas-projects.vercel.app`) built the current immediate-confirmation and bounded multi-batch recovery changes from locked packages and the pinned .NET 8.0.424 SDK / ASP.NET Core 8.0.30 runtime. The immutable 85.85 MB container deployed `READY` in `iad1` using the Preview-only configuration, so no production cron was registered.
- The protected-deployment smoke harness passed all ten checks against that deployment: liveness, PostgreSQL readiness, the `/health` compatibility alias, legacy and explicit-v1 public reads with matching shapes, v2 rejection, RFC 7807 validation, owner authentication challenge, and maintenance-credential rejection. Vercel returned no error-level or HTTP 500 logs for the deployment during the qualification window. Valid maintenance credentials were intentionally not used, so the smoke run did not dispatch email or mutate cleanup state.
- A read-only Vercel observability audit found zero drains, no project monitoring resource beyond Neon, and no `OTEL_EXPORTER_OTLP_ENDPOINT` in Preview or Production. The application emits structured JSON and is instrumented for OpenTelemetry, but the Hobby deployment is not externally retained or alerted. `scripts/audit-vercel-observability.ps1 -RequireExternalExport` remains the metadata-only gate for the explicitly deferred monitoring integration.

- Vercel Preview deployment `dpl_AxFsmHvvfAg8eLreQaR8Jcwkwa6W` built the production container and reached `READY`.
- Liveness and PostgreSQL readiness returned 200 with correlation and `Server-Timing` headers.
- The public list and portfolio projection returned the preserved v1 response shapes; hidden email, phone, birth date, and gender values were returned as `null`.
- An unauthenticated cleanup request returned 401, and invalid login input returned RFC 7807 validation output.
- Release build, formatting, warning baseline, 26 automated tests, and direct/transitive vulnerability scan passed.
- A later Preview pass applied restrictive foreign keys, verified 15 PostgreSQL/model invariants, and eliminated EF Core required-navigation/query-filter runtime warnings. The complete local suite now contains 39 tests.
- Owner collection routes now support stable, bounded pagination with a compatibility default and maximum of 100 items. Public portfolio projections and nested relation collections are also capped at 100; a PostgreSQL command interceptor guards the projection against major N+1 regressions. Cross-owner skill-resource links are rejected before persistence. The complete suite now contains 47 tests, including 19 PostgreSQL/model tests.
- Public portfolio lookup requires a confirmed account, matching the public listing invariant. Email, phone, birth date, and gender are conditionally suppressed inside the translated PostgreSQL projection unless their explicit display preferences are enabled, then scrubbed again after projection as defense in depth. A model test verifies all four gates remain SQL-translatable.
- Public portfolio reads include only blog posts in the Published status whose publication date is no later than the injected current UTC date; Draft, Scheduled, Archived, PendingReview, Rejected, Deleted, and future-dated rows remain owner-only even if handler validation was bypassed. The existing add/edit command accepts an optional Draft/Published status, defaults new content to Draft, rejects unsupported transitions, and rejects a Published status with a future publication date. `IX_BlogPost_PublicVisibility` supports the owner/status/deletion/publication projection.
- Every audited mutable entity uses PostgreSQL `xmin` optimistic concurrency. Stale writers, serialization failures, and deadlocks receive an RFC 7807 `409` instructing clients to retry or reload; only verified PostgreSQL integrity-constraint SQL states are translated to data conflicts. Generic EF update failures, connection outages, and timeouts remain 5xx incidents rather than being mislabeled as client conflicts. Npgsql's generated migration SQL is regression-tested to ensure it never attempts to physically create or drop PostgreSQL's system `xmin` column. Reorder commands require the complete active owner collection, reject duplicates/omissions, cap input at 500, and commit multi-row changes atomically. Creation-time ordering is database-serialized through an owner/collection counter and insert triggers, so concurrent project, education, experience, and certificate creates receive distinct append positions without a race-prone `MAX()+1` query.
- Owner create/edit commands validate bounded text, URLs, date ranges, lookup IDs, skill IDs, and ownership-sensitive foreign keys before persistence. Blog publication dates are now persisted, new posts receive the seeded Draft status, and active slugs are unique per owner with migration preflight protection.
- Shared casing normalization is total for empty, whitespace-only, and separator-heavy input and uses invariant casing rather than process culture. Valid lookup names normalize consistently across server regions, while future call sites cannot trigger empty-token indexing failures.
- Shared admin lookups validate names, reject deletion while referenced, and use active-only unique indexes so soft-deleted names may be recreated without permitting duplicate active values. The complete suite now contains 54 tests, including 26 PostgreSQL/model tests.

## Current deployment audit (2026-08-27)

- Vercel Preview `dpl_2GdHjQBqSpbTsmVhNCptxK4Yayv9` (`portfolio-jr7mmo0dm-albaraa-salamas-projects.vercel.app`) deployed the current hardened workspace `READY` in `iad1` from the Preview-only configuration after the isolated branch reached the final `IndexContactSubmissionCooldown` migration. The protected smoke harness passed all ten acceptance checks, and deployment-scoped scans returned zero error-level and zero HTTP 500 log entries during the qualification window.
- Vercel Preview `dpl_HMvfxdb7tSxrc2GuHUaBiwxNkHUy` (`portfolio-ovh1obpml-albaraa-salamas-projects.vercel.app`) rebuilt the current workspace against an isolated Neon Preview branch, using the pinned SDK/runtime container in `iad1`, and reached `READY`. Production and Preview now have separate Sensitive pooled/direct credential pairs; unused Neon integration aliases and the obsolete client-URL setting were removed. All 19 reviewed hardening migrations applied to the isolated branch from SQL SHA-256 `7226E4B599D4D99F2EAF84553E57E3BA49F44AA5873133035E5F4922731CD773` under EF's migration lock. The protected-deployment smoke harness passed all ten checks both before and after migration: liveness, PostgreSQL readiness, the `/health` alias, legacy and explicit-v1 public reads with matching shapes, v2 rejection, RFC 7807 validation, owner authentication challenge, and maintenance-credential rejection. The checks also validate correlation, cache/security, content-type, API-version, and `WWW-Authenticate` headers where applicable.
- Vercel Production `dpl_Fk86hVVcmY6LH3AkWdRWR7wiANCB` (`portfolio-j3voly67o-albaraa-salamas-projects.vercel.app`) deployed the hash-only hardened container `READY` in `iad1` after all PostgreSQL stages completed. All ten acceptance checks passed against that host after the final migration; bounded error-level and HTTP 5xx scans returned no entries.
- The pre-hardening deployment `dpl_3T3MdQmjwxrxDJBamExnFgc7gbU7` must no longer be used as a rollback target because refresh-token hashing is irreversible. The temporary compatibility deployment `dpl_43BjViZt8hwcncShsdRpZb5utYmD` remains the oldest application-safe rollback target after hashing; normal rollback should prefer the newer hash-only deployment immediately preceding the current release.
- No error-level runtime logs were reported for the refreshed deployment after the smoke run. This short qualification window supplements, but does not replace, retained Production monitoring.
- Preview Deployment Protection remains enabled. The smoke harness's `-UseVercelCli` transport uses the authenticated CLI bypass without copying a secret; unattended CI can instead supply `VERCEL_AUTOMATION_BYPASS_SECRET`.
- A preliminary protected-Preview performance run completed against the current sparse dataset with 50 requests per scenario at concurrency four and zero errors. Liveness measured 331.5 ms wall p50 / 2,227.6 ms p95; the public user list measured 293.0 ms p50 / 329.4 ms p95. A temporary named Automation Bypass credential was kept in memory, revoked in a guarded cleanup, and confirmed absent afterward; anonymous access again redirects to Deployment Protection. These measurements remove the transport blocker but are not representative-data acceptance or SLO evidence. `PERFORMANCE.md` records the limitations, including an invalid `Server-Timing` sample that exceeded wall time.
- The Vercel team plan was re-verified through the authenticated team API as Hobby. The production outbox and cleanup jobs both use supported daily schedules. Preview uses `vercel.preview.json` with an explicit empty cron set; the canonical root `vercel.json` remains in container uploads because excluding it was proven to produce a healthy deployment with zero registered cron definitions. Vercel schedules cron jobs only for Production deployments.
- Preview now has all startup-required application, database, JWT, Cron, CORS, frontend, and SMTP variable names. SMTP values were copied from the existing local Development cache and stored as Vercel Sensitive values; no email job runs automatically on Preview.
- Preview and Production now each have an independently generated Sensitive `Security__EmailConfirmationSecret`; neither value was printed or written to the repository, and the scopes do not share key material. The optional previous-key slot remains absent because no confirmation-key rotation is in progress.
- A metadata-only environment audit verifies required credential presence, uniqueness, scope, and Vercel Sensitive type without retrieving values. Production `Email__Password` is now Production-only Sensitive, and the complete live audit passes.
- Production has an independently generated Sensitive `CRON_SECRET`; it was generated in memory and never printed or written locally. The database is fully migrated through `IndexContactSubmissionCooldown`. Every bounded stage was generated, reviewed, hash-pinned, applied under EF's migration lock, and followed by the required compatibility deployment and smoke gates. Legacy refresh tokens were hashed only while the compatible release was live, after which `Security__AllowLegacyRefreshTokenLookup` was set to `false`, the hash-only release was deployed, and the temporary variable was removed so future releases use the secure default.
- Vercel's authenticated project metadata confirms both enabled daily cron definitions belong to `dpl_Fk86hVVcmY6LH3AkWdRWR7wiANCB`: cleanup at `17 3 * * *` and email-outbox recovery at `17 7 * * *`. A deployment with the root `vercel.json` excluded was proven to register zero definitions, so the configuration verifier now rejects that packaging regression.
- The isolated Preview database is fully migrated through `IndexContactSubmissionCooldown`. Its bounded idempotent SQL was reviewed and applied from SHA-256 `81CA00768970A6618EFEDC205B54B31D1A278FE1E67C8BF561B73D85E643FC0B`; the migration acquired EF's exclusive migration lock and completed successfully before the latest Preview deployment.
- Development configuration ignores Vercel's literal `[SENSITIVE]` export marker instead of treating it as a credential. Contract tests inject and restore their own early process configuration, so their startup and API evidence no longer depends on an ignored developer `.env.local` file.

## Email delivery consistency

Registration, unconfirmed login, confirmation resend, and public contact submission now persist an email outbox message in the same database commit as their business data. Confirmation-producing requests make a best-effort targeted dispatch only after that commit; a transient failure to start dispatch is logged and measured as `deferred` without converting an already-committed registration or resend into an API failure. The authenticated `/api/maintenance/email-outbox` cron endpoint claims bounded batches atomically, prevents overlapping workers from processing the same message, retries with exponential backoff up to five attempts, and emits terminal-failure logs without message contents or credentials. Dependency timeouts count as failed delivery attempts while genuine caller cancellation remains retryable after the lease expires. Every claim starts a fresh five-minute lease, which exceeds the maximum permitted SMTP timeout and prevents an overlapping worker from reclaiming a message while delivery is still legitimately running. Raw confirmation tokens are generated only during dispatch and only their hashes are persisted. Completed and terminal outbox rows are retained for a 30-day operator replay window; pending confirmation cleanup does not remove records that still have an active delivery job. Cleanup is exposed through an Application interface and implemented by Infrastructure rather than issuing persistence operations from the HTTP controller.

Routine successful delivery is represented by bounded metrics and the batch summary rather than one log event per message. Retryable failures log only the low-cardinality message kind and attempt number; outbox identifiers appear only on terminal and operator-replay events where the ID is required for remediation.

Confirmation tokens are deterministically derived from the confirmation ID with a dedicated keyed HMAC, so an ambiguous SMTP timeout and subsequent retry cannot invalidate a link that may already have been delivered. The confirmation key is independent from JWT signing, preventing routine JWT rotation from corrupting queued confirmation delivery. A validated previous-key slot lets the dispatcher reconstruct old-key tokens during a staged confirmation-key rotation. Registration, resend, and unconfirmed-login flows dispatch their exact outbox message immediately after its transaction commits; this targeted operation cannot drain unrelated work. Vercel invokes the dispatcher daily as recovery and cleanup daily. Recovery drains at most ten 20-message batches (200 messages) under a dedicated 240-second endpoint timeout, below the Vercel Hobby Function limit, and reports `succeeded_batch_full` when that cap is reached so remaining backlog is actionable. A confirmation token receives its 15-minute validity window only when dispatch begins, and the email explains that an expired link requires requesting another confirmation email. Terminal delivery logs are an alert source; an operator can replay only exhausted, unprocessed messages through the authenticated `POST /api/maintenance/email-outbox/{messageId}/replay` endpoint.

Security-sensitive runtime configuration is bound once into immutable JWT, confirmation-token, SMTP, branding, and security settings. Production startup rejects weak or reused secrets, missing SMTP fields, disabled SMTP TLS, invalid host syntax, ports/timeouts, non-HTTPS public URLs, and invalid CORS origins before accepting traffic. SMTP hosts must be bare DNS names or IP addresses rather than URIs, credential-bearing strings, or paths. Notification templates HTML-encode untrusted identity fields and every configured/generated URL at the HTML attribute boundary, percent-encode confirmation tokens, and use the injected UTC clock.

Branding configuration requires web URLs without embedded credentials or fragments. The frontend base additionally rejects query strings so generated login/confirmation paths remain well formed; logo URLs may retain signed CDN queries. Production requires HTTPS, while Development HTTP is restricted to loopback hosts.

User-authored portfolio links use one shared HTTP/HTTPS validation policy. Scalar and
collection URL fields reject non-web schemes and embedded credentials before database
access; certificate-media reconciliation repeats the check defensively inside the use
case so non-HTTP callers cannot bypass transport validation.

All account endpoints are covered by the authentication rate-limit policy. Unknown-account login performs a dummy password-hash verification to reduce timing-based enumeration. Confirmation resend and public contact submission return the same success response for unknown targets, preventing account discovery. `HashLegacyRefreshTokens` converted all pre-hardening raw Production refresh-token rows to hashes after its transformed-value collision preflight passed. The default-off transition lookup was enabled only for the compatible bridge deployment, then explicitly disabled before the final hash-only deployments. Any legacy row rotated during that window was atomically replaced with its hash, and new sessions are always hashed. Email confirmation consumes the link atomically but no longer creates authentication cookies from a public GET request, preventing login CSRF; users explicitly log in after confirmation.

Logout hashes an anonymous caller's refresh cookie before querying and never compares the raw credential with persisted data. Login and refresh prepare the replacement session first, commit its hashed refresh-token row, and only then publish access and refresh cookies. A persistence failure therefore cannot attach credentials for a nonexistent session to an error response.

Refresh cookies are HttpOnly, Secure, SameSite=None, and scoped separately to `/api/Account` and `/api/v1/Account`, including deletion. `RememberMe` controls browser-cookie persistence only: both session and persistent refresh credentials use the configured 30-day server lifetime, while `RememberMe=false` omits cookie expiry so closing the browser ends the session. Refresh rotation preserves this choice. Access cookies remain API-wide because authorization middleware consumes them on protected owner and administrator routes.

All API traffic is additionally bounded by a per-client token bucket (120 requests per minute with no queue); authentication and contact endpoints retain their stricter policies. Liveness, readiness, and the temporary `/health` alias are exempt so platform probes cannot be starved by application traffic.

An explicit `Authorization` header is authoritative when both bearer credentials and an access cookie are present; browser cookies are used only as a fallback. This prevents a stale browser cookie from overriding a valid API-client token while retaining the secured custom JWT cookie flow.

Owner and administrator policies require an authenticated identity, the expected role, a confirmed-account claim, and a non-empty GUID user identifier. Malformed but correctly signed identities are rejected at authorization rather than reaching handlers that assume a valid current user. Authentication challenges, authorization failures, and maintenance-credential failures return RFC 7807 bodies with trace IDs; bearer challenges retain the `WWW-Authenticate` header.

The authorization fallback policy requires an authenticated user, so newly introduced endpoints fail closed when authorization metadata is omitted. Account, public client, health, and maintenance-secret routes explicitly opt out; owner and administrator routes retain their stricter confirmed-role policies. Contract tests guard both sides of this boundary.

The owner full-information projection now applies the same deterministic 100-item ceiling as public portfolio reads to every top-level and nested collection. Search strings are capped at 200 characters and ten terms, page numbers are bounded, and the confirmed-user listing has an index aligned with its filter and stable ordering. Outbox metric labels are carried in the initial batch query rather than reread per message.

All API actions now dispatch through a single base method that propagates `HttpContext.RequestAborted` into MediatR and EF operations; the mediator itself is private so derived controllers cannot bypass the rule. SMTP combines caller cancellation with its bounded delivery timeout. Client-abort cancellations are classified as HTTP 499 and are not logged as unexpected 500 or SMTP-provider failures; dependency timeouts retain their intentional delivery-failure signal.

Profile, social-link, user-preference, chart-preference, and public username inputs now have explicit string, URL, phone, range, and future-date validation. Preference and chart handlers validate referenced lookup records intentionally instead of surfacing foreign-key conflicts. A global MVC validator rejects `Guid.Empty` for scalar, nullable, and nested collection identifiers. Handler-level audit timestamp writes were removed; `AppDbContext` is the single source of create/update/delete timestamps for audited entities.

Profile birth-date validation resolves the application UTC clock from MVC's validation
context, so year/day boundaries use the same time source as tokens, cleanup, outbox, and
persistence conventions. Standalone metadata tools retain a UTC fallback, while a
fixed-clock boundary test keeps runtime business validation deterministic.

All owner-supplied profile, project, certificate, blog, and social-link URLs share an
Application-layer HTTP URL validator. It permits absolute HTTP/HTTPS locations while
rejecting FTP, executable/custom schemes, relative paths, empty values, and embedded
credentials before handler execution. Optional URL fields continue to accept `null`.

Global MVC validation rejects the Unicode null character in every scalar or collection
string before a handler runs. PostgreSQL text cannot represent that character; returning
the existing sanitized validation Problem Details prevents anonymous poison payloads
from becoming database exceptions while retaining newlines and tabs in rich text.

Request bodies are capped centrally at 1 MiB. Declared oversized bodies receive a
sanitized RFC 7807 `413` before model binding, while Kestrel enforces the same byte limit
as the streaming backstop for chunked or dishonest requests. Multipart bodies share the
same ceiling, with bounded form key length, value length, and value count. Vercel or an
upstream proxy may impose a stricter platform limit but must never be configured higher
as a replacement for the application and transport controls.

The complete suite contains 367 tests: 223 unit, 7 architecture, 82 PostgreSQL/model integration, and 55 HTTP contract tests. The contract suite checks a reviewed, canonical OpenAPI snapshot and independently proves that all 61 legacy Account, Admin, Client, and Owner operations and HTTP methods remain represented. Database-dependent test bodies run in CI against PostgreSQL 16; local runs without `TEST_DATABASE_URL` exercise model metadata but cannot substitute for CI database evidence. Two-context concurrency and partial-failure tests verify that overlapping outbox workers claim one message only once and that one failed delivery does not stop the remainder of a claimed batch. Targeted immediate dispatch is proven not to drain unrelated messages, and daily recovery coverage proves work spanning multiple 20-message batches is consumed in one bounded run. Exact expiration boundaries are covered so cleanup and lease recovery occur when timestamps equal the current instant. Explicit migration compatibility coverage proves legacy raw refresh rows rotate successfully and are replaced with hashes only when the transition switch is enabled. Separate two-context tests verify that concurrent refresh reuse revokes the winning replacement session, concurrent confirmation resends create only one replacement link, and concurrent owner-item creates receive distinct append positions. Transaction tests prove that failed owned units of work roll back and composed services participate in an existing transaction without trying to commit it. Contact cooldown and confirmation-resend concurrency coverage execute their database bodies in that PostgreSQL-enabled CI job; a local run without the test connection only verifies discovery and construction. Owner skill-graph mutations reject empty or duplicate identifiers and cap the aggregate nested relation workload at 500 before database access; language bulk updates likewise reject ambiguous duplicate and empty identifiers through centralized model validation. Owner lookup search terms use invariant normalization, with a Turkish-process-culture regression test preventing locale-dependent matching. PostgreSQL check constraints reject invalid gender, negative ordering, reversed education/experience/certificate dates, unsupported outbox kinds, invalid attempt counts, and inconsistent outbox lease state. Production and design-time EF options elevate implicit sibling-collection joins to errors; high-cardinality skill reconciliation and certificate editing explicitly use split queries to prevent Cartesian result growth. The owner inbox combines total and unread counts into one aggregate and has PostgreSQL coverage proving a complete page requires exactly two database commands. The durable contact cooldown has a predicate-aligned partial index and migration-script coverage that keeps timeout setup and index creation in the same idempotent guard. Contract tests prove both the transport/form configuration and the sanitized pre-binding oversized-body response, assert the dedicated maintenance timeout remains below the serverless ceiling, and verify cross-site browser requests cannot abuse the legacy resend GET. SMTP timeout boundary and lease/deadline relationship tests prevent a delivery attempt from outliving its claim or cron request. A refresh-lifetime policy test prevents a second server lifetime from diverging from the documented 30-day invariant. A metrics privacy test verifies that unexpected strings cannot escape the bounded dimension vocabulary.

The staged Production migration and deployment are complete. External OTLP/log retention, alert routing, and representative-data performance baselines remain explicitly deferred follow-up work; the checked-in instrumentation, audit, seeder, and measurement harnesses preserve those integration paths.

The `Portfolio.Api` meter exports `portfolio.http.requests`, `portfolio.http.request.duration`, `portfolio.authentication.failures`, `portfolio.email.delivery`, and `portfolio.readiness.failures`. Every string-valued metric dimension is normalized through an explicit allowlist; unexpected caller or client values collapse to `other`/`OTHER` instead of exposing data or creating unbounded time series. Configure backend alerts for sustained 5xx responses, authentication-failure spikes, terminal email outcomes, and PostgreSQL readiness failures. Database connection exhaustion should be alerted from Neon pool metrics because the application cannot reliably infer provider-wide pool saturation from one serverless instance.

Client correlation IDs are preserved only when they are 1-128 characters from the ASCII token set `A-Z`, `a-z`, `0-9`, `-`, `_`, `.`, and `:`. Ambiguous values are replaced with the current trace or server request identifier before entering response headers, structured logs, or trace attributes.

Unhandled exceptions are recorded by type, message-free stack, status, and trace ID without attaching the exception object or message to JSON logs, Problem Details, or OpenTelemetry events. SMTP logs exclude recipient and sender addresses, configured hosts, and provider response messages; readiness logs exclude PostgreSQL messages and connection identity. Operators correlate sanitized events through the trace ID and inspect provider-side diagnostics under their separate access controls.

## Incident and rollback checklist

1. Correlate the failing request using `X-Correlation-ID` and Vercel runtime logs.
2. Check readiness and PostgreSQL/Neon connection usage.
3. Stop a bad rollout by promoting the last known-good Vercel deployment.
4. Do not roll back a database migration unless its reviewed down-script is known to preserve data; prefer a forward corrective migration.
5. Rotate a suspected secret in Vercel, invalidate affected refresh sessions, and redeploy. Never log secret values, JWTs, cookies, or database URLs.

## Required production configuration

- `DATABASE_URL`: pooled Neon runtime connection.
- `DATABASE_URL_UNPOOLED`: direct/admin migration connection.
- `ApplicationSettings__JWT_Secret`: random secret of at least 32 bytes.
- `ApplicationSettings__JWT_Issuer` and `ApplicationSettings__JWT_Audience`.
- `Security__EmailConfirmationSecret`: a separate random secret of at least 32 bytes.
- `Security__PreviousEmailConfirmationSecret`: only during a staged confirmation-key rotation.
- `CORS_ALLOWED_ORIGINS`: exact HTTPS frontend origins, comma-separated.
- `CRON_SECRET`: random Vercel Cron authentication secret of at least 32 bytes.
- SMTP settings listed in `.env.example` when email is enabled.
- Optional `OTEL_EXPORTER_OTLP_ENDPOINT` and `OTEL_EXPORTER_OTLP_HEADERS` for the selected telemetry backend.

Generate JWT, email-confirmation, and Cron secrets independently and rotate them through Vercel environment variables. Startup rejects short, low-diversity, repeated, placeholder, and reused key material without placing its value in the exception. JWT rotation invalidates existing access tokens but no longer invalidates confirmation links; confirmation-key rotation uses the previous-key slot until old pending and replayable work is resolved.

Production startup fails closed when the Cron secret or allowed CORS origins are missing. Refresh tokens are stored as hashes, rotated atomically, revoked rather than deleted during logout, and all active sessions are revoked when replay is detected. Rotation claims the presented credential and inserts its replacement in one database transaction; cookies are published only after commit. A competing refresh that loses the conditional claim waits for that transaction and then revokes the replacement as part of family-wide reuse detection.

Password hashes use ASP.NET Core Identity's versioned PBKDF2 format with an explicit, startup-validated work factor (`Security:PasswordHashIterations`, default 220,000; allowed range 100,000-1,000,000). Existing lower-work-factor hashes remain valid and are transparently rehashed during the next successful login, in the same save that persists the session. Before invoking PBKDF2, verification rejects malformed, oversized, unknown-format, or database-supplied hashes requesting more than 1,000,000 iterations, preventing corrupted credential rows from becoming a CPU-exhaustion primitive. Login and registration cap password input at 256 characters to bound hashing cost. Benchmark this setting in the Vercel runtime before raising it; do not lower it below the validated floor.

API and health responses set `nosniff`, frame denial, no-referrer, restrictive permissions, and an API-safe content security policy. Authenticated, administrative, account, maintenance, health, state-changing, cookie-setting, and all error responses are explicitly non-cacheable; successful anonymous portfolio reads retain their reviewed endpoint cache policy. Cacheable public reads vary by `Origin`, preventing shared caches from mixing CORS representations, and a resolved authenticated principal forces `no-store` even if a future authentication transport does not use the current headers. Non-development deployments enable HSTS after trusted forwarded headers are applied, so Vercel's original HTTPS scheme is interpreted correctly.

Anonymous user search can match an email address only when the owner explicitly enabled the public-email preference. This prevents private-email account enumeration while preserving the existing public-search contract for usernames, names, and intentionally published email addresses. PostgreSQL translation is covered so the privacy predicate cannot silently fall back to client-side filtering.

Login, registration, and public contact recipient resolution share invariant email normalization. Contact submission resolves confirmed accounts only and returns the same public success shape for missing or ineligible targets, preventing account enumeration and outbox amplification against accounts that do not yet have a public portfolio. The `User` CLR primary key is non-nullable to match PostgreSQL; its migration is deliberately metadata-only because the existing primary key is already `NOT NULL` with a UUID default.

Certificate media updates validate and trim every URL, reject duplicates and non-HTTP(S) schemes before database access, soft-delete replaced media explicitly, and append the bounded replacement set instead of relying on EF orphan deletion. The database column is limited to 2048 characters, with a migration preflight that aborts if oversized legacy values exist.

Owner language updates reconcile the tracked composite-key rows in place: retained languages update proficiency, missing languages are deleted, and new languages are appended. AutoMapper no longer replaces the navigation collection with duplicate tracked keys. The complete change persists through one `SaveChanges` transaction and has PostgreSQL-backed regression coverage.

Owner skill updates likewise avoid deep AutoMapper graph replacement. Removed skills are soft-deleted, retained skills reconcile education, experience, project, and certificate join sets explicitly, and new skills create bounded relation sets. Ownership validation runs before mutation, while one `SaveChanges` transaction commits the complete graph. PostgreSQL coverage exercises retained, removed, and added skills together with composite relation replacement.

Confirmation resend retains its legacy generic-success v1 contract but enforces a durable two-minute cooldown per account in addition to the process-local IP rate limit. Because that legacy state-changing route is a GET, endpoint-specific Fetch Metadata and trusted-origin enforcement rejects cross-site browser images, navigations, and fetches without breaking non-browser clients that omit browser metadata; a trusted cross-origin frontend remains supported. The same queue policy applies when a valid but unconfirmed user logs in, preventing that path from bypassing resend controls. The cooldown is derived from persisted confirmation outbox enqueue times, so it is shared by every Vercel instance and does not reset on cold starts. Cooldown evaluation, old-token revocation, replacement creation, password-hash upgrade, and outbox enqueue commit atomically. A `(Kind, CreatedAt)` index supports the abuse-control lookup, and unknown, confirmed, or throttled accounts remain indistinguishable to callers.

All owner mutation handlers are required by an architecture test to consume the authenticated-current-user abstraction, and their entity queries constrain ownership in PostgreSQL. Unsafe browser requests with an authentication cookie require an allowed origin. Unsafe first-time browser requests with any `Origin` header—including login and registration before a cookie exists—are also rejected unless the origin is allowed, closing login-CSRF while preserving cookie-free, origin-free API clients.

Database commands use bounded connection and command timeouts, but EF does not implicitly replay request transactions. An automatic execution strategy cannot distinguish a failed commit from a committed response loss and could repeat confirmation, outbox, or refresh state changes on the same tracked context. Transient database failures therefore return an error for an intentional client/job retry; only application workflows with explicit idempotency and claim semantics perform automatic retries.

Package versions are locked for reproducible restore, including inside the production container build. CI parses the NuGet vulnerability report and fails on High or Critical direct or transitive findings; the informational `dotnet list` exit code is not treated as an enforcement mechanism. The current compatibility line retains `net8.0`, EF Core 9, and Npgsql 9 while consuming their latest compatible servicing patches; major framework upgrades require a separate migration and preview qualification.

Release builds promote every analyzer/compiler warning to an error, and the repository explicitly enables cancellation-token forwarding rule CA2016. A full async-call audit found no synchronous blocking and no missing request-token propagation through EF Core, transactions, SMTP, maintenance, or outbox operations; the timeout response intentionally uses an uncancelled token only after request timeout cancellation so the terminal RFC 7807 response can still be serialized.

Forwarded client identity is accepted from one proxy hop only when the platform-provided `VERCEL=1` sentinel is present; outside that runtime, ASP.NET Core's known proxy/network allowlists remain intact. `X-Forwarded-For` and `X-Forwarded-Proto` must be supplied symmetrically before either is trusted. Rate-limit partitions canonicalize IPv4-mapped IPv6 addresses so one client cannot gain a second bucket through textual address variants. The ASP.NET limiters are intentionally a per-instance defense layer: Production must also retain Vercel Firewall or another edge/distributed authentication and contact limit, because horizontally scaled serverless instances do not share in-memory counters.
