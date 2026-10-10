# Compliance Monitor

Compliance Monitor is a React + TypeScript frontend backed by an ASP.NET Core API.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Node.js](https://nodejs.org/) and npm
- A PostgreSQL database with the schema expected by the API: the `History` table and the `result` enum (I have included my db credentials in my email so that this does not need to be added from scratch)
- A Hugging Face API token (I have included my API token in my email since the free tier is no longer available)


## Configure local secrets

The backend reads `ConnectionStrings:DefaultConnection` and `HUGGINGFACE_API_TOKEN` from ASP.NET Core configuration. For local development, store these values with .NET User Secrets so credentials do not go into the repository. From the repository root, run:

```powershell
dotnet user-secrets init --project .\Backend\Backend.csproj
dotnet user-secrets set "ConnectionStrings:DefaultConnection" "<your PostgreSQL connection string>" --project .\Backend\Backend.csproj
dotnet user-secrets set "HUGGINGFACE_API_TOKEN" "<your Hugging Face token>" --project .\Backend\Backend.csproj
```

ASP.NET Core also loads `Backend/appsettings.Development.json` with the Development launch profile, but that file is tracked in this repository. Prefer User Secrets for real credentials; do not put secrets in the tracked file or commit them.

## Run locally

Run the backend and frontend in separate terminals, from the repository root.

In the first terminal, start the backend with the HTTPS profile:

```powershell
dotnet run --project .\Backend\Backend.csproj --launch-profile https
```

The API listens at `https://localhost:7089`. If your machine does not trust the local HTTPS development certificate yet, run this once:

```powershell
dotnet dev-certs https --trust
```

In a second terminal, install the frontend dependencies and start Vite:

```powershell
Set-Location .\Frontend
npm ci
npm run dev
```

Open the local URL printed by Vite (by default, `http://127.0.0.1:5173`). The frontend calls the API at `https://localhost:7089` by default. To use a different API origin, set `VITE_API_BASE_URL` before starting Vite. The backend CORS policy currently allows the default Vite origins `http://localhost:5173` and `http://127.0.0.1:5173`.

## Run unit tests

From the repository root, run:

```powershell
dotnet test .\Backend.Test\Backend.Test.csproj
```

This runs the backend's xUnit tests. The test suite uses in-memory data and stubbed HTTP responses, so you do not need to start the app or configure the PostgreSQL database or Hugging Face token.
