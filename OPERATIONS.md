# Operations and Incident Runbook

This runbook covers the Portfolio API on Vercel and its Neon PostgreSQL database. The
default incident owner is the API on-call. Escalate database capacity or availability
issues to the database owner and delivery/provider issues to the email owner. Replace
these role names with the current people and paging destinations before Production
promotion.

## Signals and initial alerts

Export OpenTelemetry metrics and traces by configuring `OTEL_EXPORTER_OTLP_ENDPOINT`
with an HTTPS collector URI in Production. Credentials belong in
`OTEL_EXPORTER_OTLP_HEADERS`, never in the endpoint URI; query strings and fragments are
rejected. Development permits plaintext HTTP only for a loopback collector.
Send structured JSON logs to a retained log/error backend. Every
request log and response contains a correlation ID; use it to join an alert, trace, and
all logs for one request.

Before Production promotion, run:

```powershell
& ./scripts/audit-vercel-observability.ps1 -RequireExternalExport
```

The audit reads metadata only and never exports environment-variable values. It accepts
an HTTPS OTLP collector, a Vercel drain, or a recognized project monitoring resource.
On Vercel Hobby, drains are unavailable, so configure an external OTLP collector or an
error-monitoring integration. Runtime logs and manual CLI scans are useful diagnostics
but do not provide retained alerting or paging.

Configure these alerts before Production promotion:

| Signal | Initial condition | Owner | First action |
| --- | --- | --- | --- |
| HTTP 5xx | More than 1% of requests and at least 10 failures over 5 minutes, for 10 minutes | API on-call | Split by route/status and inspect traces for the newest deployment |
| Latency | Route p95 exceeds twice its seven-day same-hour baseline for 15 minutes, with at least 100 requests | API on-call | Check cold starts, database latency, query count, and dependency spans |
| Request timeout | Any sustained `portfolio.http.request.timeouts` increase, or at least 5 events over 5 minutes | API on-call | Split 504 requests by route in request telemetry and inspect database/dependency spans |
| Readiness | 3 consecutive `/health/ready` failures or any `portfolio.readiness.failures` burst over 3 minutes | API on-call + database owner | Check Neon status, connection errors, and pool saturation |
| Authentication | `portfolio.authentication.failures` exceeds three times its seven-day same-hour baseline for 10 minutes, with at least 50 events | API on-call | Split by `reason`; distinguish abuse from a deployment/configuration regression |
| Rate limiting | `portfolio.rate_limit.rejections` exceeds three times its seven-day same-hour baseline for 10 minutes, with at least 25 events | API on-call | Split by bounded `policy`; distinguish expected abuse controls from false positives or a shared-proxy partition issue |
| Oversized payloads | HTTP 413 exceeds three times its seven-day same-hour baseline for 10 minutes, with at least 25 events | API on-call | Split by route and client release; distinguish abuse from a legitimate client contract regression |
| Email delivery | Any `portfolio.email.delivery` with a `terminal` outcome, sustained `deferred` or `lease_lost` outcomes, or `portfolio.maintenance.runs{job=email_outbox,outcome=completed_with_terminal_failures|succeeded_batch_full}` | Email owner | Verify SMTP/database status, configuration, lease duration, and whether daily volume exceeds the 200-message recovery cap; inspect outbox IDs without exposing message contents |
| Maintenance failure | Any `portfolio.maintenance.runs{outcome=failed}` | API on-call | Inspect the cron request trace and safely retry the idempotent endpoint |
| Cleanup backlog | `portfolio.maintenance.runs{job=cleanup,outcome=succeeded_batch_full}` on 2 consecutive daily runs | API on-call + database owner | Inspect eligible row counts and cleanup query latency; run a controlled retry after the first batch completes |
| Cron staleness | No successful email-outbox or cleanup run for 36 hours | API on-call | Check Vercel Cron history, route authorization, and deployment health |

The rate and latency thresholds are safe starting values, not performance acceptance
targets. Recalibrate them after two representative weeks while retaining minimum event
counts and a separate low-traffic correctness alert. Do not weaken readiness,
maintenance-failure, terminal-delivery, or staleness alerts based on traffic volume.
Cleanup deletes at most 5,000 rows from each retention category per invocation. A full
batch is successful but signals that eligible work may remain; repeated full batches are
a backlog condition, not a reason to remove the bound.

Neon pool utilization and connection waiting must also be monitored in the database
dashboard. The application pool maximum is per running instance, so total possible
connections grow with serverless concurrency. Alert before the Neon plan limit is
reached; choose the threshold from the actual plan capacity and reserved migration and
operator connections.

## Severity and triage

- **SEV-1:** security compromise, widespread unavailability, or confirmed data loss.
  Page API on-call immediately, stop unsafe writes if necessary, and notify product and
  security owners.
- **SEV-2:** sustained elevated errors, authentication outage, database exhaustion, or
  email delivery halted. Page the responsible owner and begin mitigation immediately.
- **SEV-3:** partial degradation, failed scheduled maintenance with no immediate user
  impact, or a latency regression. Assign and resolve during the support window.

For every incident, record the UTC start time, deployment ID, migration version,
correlation/trace IDs, affected routes, sanitized exception type, metric screenshots,
and mitigation. Never paste JWTs, cookies, connection strings, SMTP credentials,
confirmation links, or request bodies containing personal data into an incident channel.

## Playbooks

### Elevated 5xx or latency

1. Compare the onset with the newest Vercel deployment and migration time.
2. Group `portfolio.http.requests` and `portfolio.http.request.duration` by route,
   method, matched route template, and status. Open representative traces using their
   correlation IDs. Metric tags and request-completion logs contain matched templates,
   or the bounded `unmatched` sentinel, rather than user-supplied paths or resource IDs.
3. Check PostgreSQL latency/pool pressure and outbound email or HTTP dependency spans.
   An exception after response headers start cannot be translated to Problem Details;
   the API logs sanitized type/trace metadata and aborts the partial response instead of
   attempting an unsafe rewrite. Correlate these events with client transport failures.
4. If compatible code caused the regression, promote the preceding known-good Vercel
   deployment. Do not reverse an additive migration during application rollback.
5. If rollback is unsafe because code depends on a schema change, deploy a forward fix
   that remains compatible with both schema states.

### Readiness or database exhaustion

1. Confirm liveness separately. A healthy `/health/live` with failed readiness points to
   PostgreSQL rather than the process.
2. Check Neon availability, pooled endpoint usage, active/waiting connections, long
   transactions, and timeout/connection exceptions in traces.
3. Reduce abusive or optional traffic, pause nonessential jobs, and increase database
   capacity only with the database owner. Do not switch runtime traffic to the direct
   migration endpoint.
4. Verify recovery with three consecutive readiness successes and normal request error
   and latency rates.

### Authentication failures

1. Split failures by the low-cardinality `reason` tag and compare affected routes and
   client releases.
   Refresh outcomes use `refresh_token_rejected`, `refresh_token_reuse`, and
   `refresh_token_concurrent_reuse`; none includes credential or account material.
2. Check issuer, audience, system clock, CORS/CSRF origin, signing-secret changes, and
   rate-limit events without logging tokens.
3. Treat concentrated credential failures as possible abuse; apply edge controls while
   preserving legitimate refresh and confirmation traffic.

ASP.NET rate-limit buckets are process-local and therefore do not establish a global
per-IP ceiling across horizontally scaled Vercel instances. Keep deployment-level edge
limits enabled for login, registration, confirmation, resend, and contact routes.
Database-backed confirmation and contact cooldowns provide cross-instance account or
sender/recipient suppression, but they are intentionally secondary controls and do not
replace edge filtering. Contact submissions take a transaction-scoped PostgreSQL
advisory lock per normalized sender/recipient pair, so simultaneous API instances
serialize before checking the durable cooldown. Investigate lock waits or repeated
duplicates as database/transaction incidents and tighten the edge policy without
changing the legacy generic-success API contract.

The API accepts at most 1 MiB per request. A declared larger body is rejected with a
sanitized `413` before model binding; Kestrel applies the same streaming limit when the
length is absent or false. Treat isolated 413s as client errors. For a sustained spike,
identify the bounded route template and client release without retaining request bodies;
apply edge blocking for abuse or revise the client payload, and change the limit only
after reviewing memory/concurrency impact and the documented endpoint contract.

### Email failures or stale dispatcher

1. Verify the email-outbox cron ran and authenticated successfully, then check SMTP
   provider status, DNS/domain verification, timeouts, and sanitized delivery errors.
2. Correct the provider or configuration issue. The dispatcher retries bounded pending
   work automatically and overlapping executions use leases. A sustained `lease_lost`
   outcome indicates delivery exceeds the claim window, clock skew, or unexpected job
   overlap and requires investigation; a rare isolated event remains retryable.
3. Replay an exhausted message only after fixing the cause with authenticated
   `POST /api/maintenance/email-outbox/{messageId}/replay`. Record the message ID and
   outcome; never copy its payload into logs or tickets.

Cleanup must retain revoked refresh-token rows until `ExpiresAt`. They are security
markers used to detect replay and revoke replacement sessions, not disposable active
session state. If storage analysis shows revoked-but-unexpired rows being removed,
pause cleanup, investigate the deployed predicate and migration state, and treat replay
detection as degraded until the full token-lifetime window has elapsed after correction.

## Deployment, rollback, and migrations

Apply additive migrations with `DATABASE_URL_UNPOOLED`, verify them, and only then deploy
compatible code using the pooled runtime URL. Promote to Production only after Preview
smoke tests. Destructive cleanup is a later, separately reviewed release after all old
code paths have been retired.

Application rollback means promoting the preceding known-good Vercel deployment and
re-running health and smoke tests. Database down-migrations are not a routine rollback:
use one only after reviewing generated SQL, lock impact, compatibility, and data-loss
risk, with a verified backup and the database owner present.

## Secret rotation

- **JWT signing secret:** create a new independent secret of at least 32 random bytes,
  update Preview and test login/refresh, then update Production. Existing access tokens
  become invalid; refresh cookies remain usable and mint tokens with the new key.
  Confirmation links remain valid because they use independent key material. Keep
  the previous value only in the approved secret manager for the defined emergency
  rollback window, then destroy it.
- **Email-confirmation secret:** set the current value temporarily as
  `Security__PreviousEmailConfirmationSecret`, install a new independently generated
  `Security__EmailConfirmationSecret`, then deploy and verify delivery of both a newly
  queued message and an old-key pending message. Keep the previous slot until all
  old-key pending links and retained replayable outbox messages have expired or been
  explicitly resolved, then remove it. Never rotate current and discard previous in one
  step while old confirmation work exists.
- **Cron secret:** create a new independent secret, update Vercel's `CRON_SECRET`, deploy
  if required by the platform, and verify both maintenance endpoints through an
  authenticated invocation. Failed old credentials must return 401.
- **Database credentials:** rotate both pooled runtime and direct/admin migration
  credentials. Update `DATABASE_URL` and `DATABASE_URL_UNPOOLED` in the correct scopes,
  verify Preview readiness and a migration connection, then Production. Never use the
  direct URL for normal API traffic.
- **SMTP credentials:** pause manual replays, rotate at the provider, update scoped
  Production environment values with `Email__EnableSsl=true`, verify the provider's
  TLS-capable submission endpoint and one non-sensitive Preview delivery, then resume
  dispatch. Pending outbox work remains durable during the change.

After every rotation, inspect logs for accidental secret disclosure, update the rotation
record with UTC time and owner, and revoke the superseded credential.

## Incident closure

Confirm health and alert recovery, account for queued work, preserve sanitized evidence,
and document impact and timeline. For SEV-1 and SEV-2 incidents, assign corrective
actions with owners and dates, add a regression test or monitor where practical, and
review whether rollback and secret-rotation documentation remained accurate.
