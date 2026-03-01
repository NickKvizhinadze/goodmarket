# Sprint 1 — Foundation: Aspire, Gateway, Shared.Contracts

**Roadmap phase:** Phase 0
**Depends on:** Nothing — this is the starting sprint
**Blocker:** None

## Goal

A running Aspire AppHost that wires Identity, PostgreSQL, RabbitMQ, and the YARP Gateway together so every subsequent service has a consistent host to register into.

## Tasks

- [ ] Create `GoodMarket.AppHost` project (`dotnet new aspire-apphost -n GoodMarket.AppHost`) and add to `GoodMarket.sln`
- [ ] Create `GoodMarket.ServiceDefaults` project (`dotnet new aspire-servicedefaults -n GoodMarket.ServiceDefaults`) and add to `GoodMarket.sln`
- [ ] Add `GoodMarket.ServiceDefaults` NuGet reference to `GoodMarket.Identity.Api`
- [ ] Call `builder.AddServiceDefaults()` in `GoodMarket.Identity.Api/Program.cs`
- [ ] Add PostgreSQL resource to AppHost: `builder.AddPostgres("postgres")`
- [ ] Add RabbitMQ resource to AppHost: `builder.AddRabbitMQ("rabbitmq")`
- [ ] Register `GoodMarket.Identity.Api` as a project resource in AppHost and wire it to the `postgres` resource
- [ ] Create `GoodMarket.Gateway` project (`dotnet new web -n GoodMarket.Gateway`), add to `GoodMarket.sln`
- [ ] Add `Yarp.ReverseProxy` NuGet package to `GoodMarket.Gateway`
- [ ] Configure YARP in `GoodMarket.Gateway/appsettings.json` — route `/api/auth/{**catch-all}` → Identity service
- [ ] Add JWT validation middleware to `GoodMarket.Gateway/Program.cs` (validates against Identity signing key, per ADR-008)
- [ ] Configure identity header forwarding in Gateway: extract `sub` and `role` claims, forward as `X-User-Id` and `X-User-Roles` headers
- [ ] Configure rate limiting middleware in Gateway (fixed window, per IP)
- [ ] Register `GoodMarket.Gateway` as a project resource in AppHost
- [ ] Create `GoodMarket.Shared.Contracts` class library project, add to `GoodMarket.sln` under `Shared/src/`
- [ ] Add `UserRegistered` integration event record to `GoodMarket.Shared.Contracts`
- [ ] Add `PasswordResetRequested` integration event record to `GoodMarket.Shared.Contracts`
- [ ] Verify `dotnet run --project GoodMarket.AppHost` starts all services and the Aspire dashboard is accessible

## Definition of Done

- [ ] `dotnet run --project GoodMarket.AppHost` starts without errors
- [ ] Aspire dashboard shows Identity, Gateway, PostgreSQL, and RabbitMQ as healthy resources
- [ ] `GET /api/auth/...` requests routed through Gateway reach the Identity service
- [ ] Gateway returns 401 for requests with missing or invalid JWT tokens
- [ ] `X-User-Id` and `X-User-Roles` headers are forwarded to Identity on authenticated requests
- [ ] `GoodMarket.Shared.Contracts` project builds and is referenced from `GoodMarket.Identity.Api`
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
