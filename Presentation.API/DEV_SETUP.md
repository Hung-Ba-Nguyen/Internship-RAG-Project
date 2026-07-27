# Local Development: Auth DB connection string (Dev setup)

This file explains how to set the Auth database connection string locally for Presentation.API without committing secrets to source control.

Recommended: use `dotnet user-secrets` (per-developer, secure for local development)

1. Open a terminal and change directory to the Presentation.API project:

   cd Presentation.API

2. Initialize user secrets for the project (only needed once):

   dotnet user-secrets init

3. Set the connection string (example):

   dotnet user-secrets set "ConnectionStrings:AuthConnection" "Server=localhost;Database=RAG_Auth_DB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=True"

Notes: ASP.NET Core will read the user secret into Configuration at runtime when running the Presentation.API project.

Alternative: set an environment variable (temporary, current terminal session)

PowerShell (current session):

   $env:ConnectionStrings__AuthConnection = "Server=localhost;Database=RAG_Auth_DB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=True"

PowerShell (persistent - uses setx, requires new shell to take effect):

   setx ConnectionStrings__AuthConnection "Server=localhost;Database=RAG_Auth_DB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=True"

Bash (Linux/macOS):

   export ConnectionStrings__AuthConnection='Server=localhost;Database=RAG_Auth_DB;User Id=sa;Password=YourPassword;TrustServerCertificate=True;MultipleActiveResultSets=True'

Alternative: Visual Studio launchSettings.json (for debugging)

- Open `Properties/launchSettings.json` in Presentation.API and add an entry under `environmentVariables`:

```
"environmentVariables": {
  "ConnectionStrings:AuthConnection": "Server=localhost;Database=RAG_Auth_DB;User Id=sa;Password=YourPassword;..."
}
```

Run migrations (example commands from repository root)

1. Add migration (create files under Infrastructure.Identity/Migrations):

   dotnet ef migrations add InitAuthDb -p Infrastructure.Identity -s Presentation.API --context AuthDbContext -o Migrations

2. Apply migration to database:

   dotnet ef database update InitAuthDb -p Infrastructure.Identity -s Presentation.API --context AuthDbContext

Security reminder
- Do NOT commit real credentials to source control. Use user-secrets, environment variables, or your CI secret store.
