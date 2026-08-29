# Architecture and Dependency Rules

## System shape

Portfolio is a layered modular monolith deployed as one stateless ASP.NET Core API on
Vercel. PostgreSQL/Neon is the durable system of record. This is the intended deployment
model for the measured target scale; a queue, broker, or independently deployed service
requires evidence that an in-process module and database-backed job cannot meet an
observed requirement.

The compile-time dependency direction is:

```text
Portfolio/API ───────► Application ───────► Domain
      │                    ▲
      └────► DataAccess ───┘
                   └──────────────► Domain
```

`Portfolio/API` is the composition root. It may reference every internal project to
register concrete implementations, but controllers communicate with use cases through
MediatR and Application contracts. `DataAccess` references Application because it
implements Application-owned ports; Application never references DataAccess.

## Layer responsibilities

### Domain

- Entities, enums, value semantics, and rules that do not require I/O.
- No ASP.NET Core, Entity Framework Core, configuration, logging, email, or token
  packages.
- No reference to Application, DataAccess, or Portfolio/API.

### Application

- Commands, queries, DTOs, validation, authorization-independent use-case rules, and
  ports for persistence, current time/user, tokens, email, metrics, and maintenance.
- Organized by Account, Admin, Client, and Owner feature slices.
- May use MediatR where a request has a distinct use-case boundary. It must not use
  service location or reference the HTTP request.
- `IAppDbContext` is intentionally an EF-backed Application port for the current
  modular-monolith stage. This keeps translated projections and set-based mutations in
  their use-case slices while preserving the correct assembly dependency direction.
  Do not expose this port to Domain or API. Replace a slice with narrower query/command
  ports only when it improves testing, removes provider coupling, or creates a real
  independent boundary; do not create one repository per entity mechanically.

### DataAccess/Infrastructure

- EF Core context/configuration, PostgreSQL migrations, transactions, database exception
  translation, password hashing, token issuance, SMTP delivery, cleanup, and readiness
  implementations.
- Implements Application-owned interfaces and contains provider-specific behavior.
- Must not own HTTP authentication handlers, cookies, authorization policies,
  controllers, middleware, routes, or response contracts.
- Runtime database traffic uses Neon's pooled connection. Migration tooling accepts only
  the direct/admin connection.

### Portfolio/API

- HTTP routes and v1 compatibility contracts, middleware, cookies, JWT bearer
  authentication, authorization policies, CSRF/CORS, rate limits, health endpoints,
  OpenAPI, telemetry, and dependency composition.
- Controllers use constructor-injected MediatR and propagate `RequestAborted`.
- HTTP failures are emitted centrally as RFC 7807 responses; handlers express expected
  failures through intentional Application/domain results or exceptions.

Architecture tests enforce the assembly direction, Domain purity, API isolation from EF
Core, Infrastructure ownership of Application ports, authenticated owner mutation
context, and API ownership of HTTP authentication.

## Decision records

### ADR-001: Retain the modular monolith

**Status:** Accepted.

The target is tens of thousands of users with horizontally scaled stateless API
instances. Independent deployment, distributed transactions, and broker operations add
failure modes without current load evidence. Feature slices and compile-time boundaries
provide the required separation inside one deployable unit. Reconsider only after
instrumentation shows an independently scalable workload or isolation requirement.

### ADR-002: PostgreSQL is the sole executable migration history

**Status:** Accepted.

`DataAccess/PostgreSqlMigrations` is authoritative and generated with Npgsql. The legacy
SQL Server history remains non-compiling archival documentation. Deployment uses
additive migration first, compatible code second, and destructive cleanup in a later
release. Detailed commands and rollback constraints are in `DEPLOYMENT.md` and
`OPERATIONS.md`.

### ADR-003: Preserve custom JWT authentication at the API boundary

**Status:** Accepted.

Infrastructure issues and hashes tokens, while Portfolio/API owns bearer/cookie token
extraction, the single default authentication scheme, validation parameters, and role
policies. A fallback policy requires authentication, so new routes fail closed; public,
account, health, and maintenance-secret surfaces opt out explicitly. Explicit
Authorization headers take precedence over the browser cookie.
Refresh rotation, replay detection, revocation, and durable persistence remain server
side. This separation prevents HTTP framework concerns from leaking into Infrastructure.

### ADR-004: Preserve v1 action routes during hardening

**Status:** Accepted.

Existing `/api/{controller}/{action}` routes and successful response shapes remain stable
while correctness and security change internally. The same 61 operations are also
available at `/api/v1/{controller}/{action}` through explicit URL-segment versioning;
contract tests prove their documented response schemas match the legacy surface.
Redesigned contracts must use an explicit new API version and coexist with v1 until the
frontend migrates. Do not silently change a v1 success payload to achieve internal
consistency.

### ADR-005: Database-backed serverless maintenance

**Status:** Accepted.

No correctness-critical hosted worker runs in a Vercel API instance. Email delivery and
cleanup are bounded, authenticated, idempotent cron operations backed by durable rows,
claims, and leases. Add an external queue only if measured throughput, latency, or
delivery isolation exceeds this design.

## Change rules

1. Add or update a contract test before intentionally changing an existing route.
2. Add an architecture test when introducing a new dependency boundary.
3. Keep owner predicates in every owner-scoped query and mutation; never rely on a DTO
   identifier as authorization evidence.
4. Keep reads bounded and projected, use `AsNoTracking` when tracking is unnecessary,
   and verify query count before adding caching.
5. Propagate cancellation through controller, use case, persistence, and dependency I/O.
6. Keep migrations backward-compatible with the preceding application release.
7. Never allow EF Core to choose an implicit single-query shape for multiple sibling
   collections. Infrastructure promotes that diagnostic to an exception; use an
   explicit split query or a measured projection and retain a query-shape test.
8. Application services use `IAppDbContext.ExecuteInTransactionAsync` for multi-step
   units of work. The outermost caller owns commit/rollback; nested services participate
   in the current transaction and must never commit it independently.
7. Record a new ADR here when a decision changes system shape, data ownership,
   authentication, compatibility, or deployment sequencing.

Operational response and secret rotation are defined in `OPERATIONS.md`; deployment and
migration execution are defined in `DEPLOYMENT.md`; performance evidence is recorded in
`PERFORMANCE.md`; the roadmap status and Preview evidence are in
`PRODUCTION_READINESS.md`.
