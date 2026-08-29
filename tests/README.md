# Test suites

- `Portfolio.UnitTests` covers deterministic services and helpers.
- `Portfolio.ArchitectureTests` enforces project dependency boundaries.
- `Portfolio.ContractTests` protects the existing v1 controller surface and compares the generated OpenAPI contract with the reviewed `v1-openapi.snapshot.json` artifact.
- `Portfolio.IntegrationTests` runs against PostgreSQL when `TEST_DATABASE_URL` is set. CI always supplies it.

Run all suites with `dotnet test Portfolio.sln -c Release`. Integration tests intentionally use PostgreSQL rather than EF Core's in-memory provider.

When an intentional, backward-compatible API contract change alters OpenAPI, regenerate the snapshot from the repository root with `$env:UPDATE_API_CONTRACT_SNAPSHOT='1'; dotnet test tests/Portfolio.ContractTests/Portfolio.ContractTests.csproj -c Release --filter FullyQualifiedName~OpenApiSnapshotTests; Remove-Item Env:UPDATE_API_CONTRACT_SNAPSHOT`, review the JSON diff, and commit it with the implementation. Never update the snapshot merely to make an unexplained contract failure pass.
