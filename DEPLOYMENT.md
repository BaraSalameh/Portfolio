# Vercel and Neon deployment

## Required environment variables

Copy the variable names from `.env.example` into the Vercel project. Never commit
their values. Neon supplies `DATABASE_URL`; use its pooled URL for the API.

Use a newly generated JWT signing key. Configure `CORS_ALLOWED_ORIGINS` as a
comma-separated list of exact HTTPS origins. Set `EnableSwagger=true` only for a
deliberately exposed environment.

SMTP configuration is optional for the initial deployment. Email-dependent flows
remain unavailable until `Email__SmtpHost`, `Email__SmtpPort`, `Email__Username`,
`Email__Password`, and `Email__From` are added to each applicable Vercel environment.
`Email__EnableSsl` defaults to `true`, and `Email__TimeoutMilliseconds` defaults to
`30000`. Use the SMTP submission port required by the provider (commonly 587 when
STARTTLS is enabled), and ensure the configured sender is permitted for the account.

## Bootstrap order

1. Install the Vercel CLI and authenticate with `vercel whoami`.
2. Link this repository to the intended Vercel project.
3. Install Neon through the Vercel Marketplace and attach it to the project.
4. Pull environment variables locally and verify all names in `.env.example` exist.
5. Run the PostgreSQL migration explicitly using Neon's direct/admin URL when available.
6. Push a feature branch and verify the Vercel preview before merging to `master`.

Do not run EF Core migrations from application startup. For later releases, apply a
backward-compatible database migration before deploying code that depends on it.

## Migration commands

With `DATABASE_URL_UNPOOLED` (preferred) or `DATABASE_URL` set in the current process:

```powershell
dotnet ef migrations script --idempotent --project DataAccess --startup-project Portfolio
dotnet ef database update --project DataAccess --startup-project Portfolio
```

## Verification

Call `GET /health`. A healthy API and database return HTTP 200; database connectivity
failure returns HTTP 503 without connection details.
