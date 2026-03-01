# Sprint 2 — Identity Core: Login, Refresh Token, Logout

**Roadmap phase:** Phase 1 (part 1)
**Depends on:** Sprint 1 complete (AppHost and Gateway running)
**Blocker:** None

## Goal

A working authentication flow where a user can register, log in to receive a JWT access token and refresh token, silently refresh the access token, and log out with token revocation.

## Tasks

- [ ] Add `RefreshToken` domain entity to `GoodMarket.Identity.Api/Domain/Users/` with fields: `Id`, `UserId`, `Token`, `ExpiresAt`, `RevokedAt`, `CreatedAt`
- [ ] Add `RefreshToken` EF Core configuration to `ApplicationDbContext` and create a migration
- [ ] Add `IJwtTokenService` interface and `JwtTokenService` implementation to generate signed JWT access tokens (per ADR-007)
- [ ] Add `IRefreshTokenService` interface and `RefreshTokenService` implementation: generate, store, validate, and rotate refresh tokens
- [ ] Implement `LoginUserCommand` and `LoginUserCommandHandler` in `UseCases/Account/Commands/Login/`
  - Validate credentials via `UserManager`
  - Issue JWT access token via `IJwtTokenService`
  - Generate and persist refresh token via `IRefreshTokenService`
  - Return `LoginResultDto` (access token, refresh token, expiry)
- [ ] Implement `POST /api/v1/auth/login` minimal API endpoint in `Endpoints/Account/`
- [ ] Implement `RefreshTokenCommand` and `RefreshTokenCommandHandler` in `UseCases/Account/Commands/RefreshToken/`
  - Validate incoming refresh token
  - Rotate refresh token (revoke old, issue new)
  - Issue new JWT access token
- [ ] Implement `POST /api/v1/auth/refresh` minimal API endpoint
- [ ] Implement `LogoutCommand` and `LogoutCommandHandler` in `UseCases/Account/Commands/Logout/`
  - Revoke refresh token by setting `RevokedAt`
- [ ] Implement `POST /api/v1/auth/logout` minimal API endpoint (requires valid JWT, reads user id from `X-User-Id` header)
- [ ] Register `IJwtTokenService`, `IRefreshTokenService` in `IdentityConfiguration.cs` DI setup
- [ ] Add JWT settings (issuer, audience, secret, expiry) to `appsettings.json` and bind to a typed `JwtOptions` record
- [ ] Wire Identity service database to the Aspire `postgres` resource: use `goodmarket_identity` database

## Definition of Done

- [ ] `POST /api/v1/auth/register` still works (existing endpoint from scaffolding)
- [ ] `POST /api/v1/auth/login` returns a valid JWT access token and refresh token for a known user
- [ ] JWT issued by Identity can be validated at the Gateway (secret/issuer match)
- [ ] `POST /api/v1/auth/refresh` returns a new access token and rotated refresh token
- [ ] Old refresh token is rejected after rotation
- [ ] `POST /api/v1/auth/logout` revokes the refresh token; subsequent refresh calls return 401
- [ ] All endpoints are reachable through the Gateway at `/api/auth/...`
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
