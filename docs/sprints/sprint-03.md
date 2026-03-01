# Sprint 3 — Identity Complete: Google OAuth2, Password Reset, Events

**Roadmap phase:** Phase 1 (part 2)
**Depends on:** Sprint 2 complete (login, refresh, logout working)
**Blocker:** None

## Goal

Identity service is fully complete with Google OAuth2 login, email confirmation, OTP-based password reset, RabbitMQ event publishing, and integration tests covering all auth flows.

## Tasks

- [ ] Add `Google.Apis.Auth` NuGet package to `GoodMarket.Identity.Api`
- [ ] Implement `GoogleCallbackCommand` and `GoogleCallbackCommandHandler` in `UseCases/Account/Commands/GoogleCallback/`
  - Validate Google ID token
  - Find or create `ApplicationUser` from Google profile (email, name)
  - Issue JWT access token and refresh token
- [ ] Implement `GET /api/v1/auth/google` endpoint — redirect to Google OAuth2 consent URL
- [ ] Implement `GET /api/v1/auth/google/callback` endpoint — handle OAuth2 callback, call `GoogleCallbackCommand`
- [ ] Add email confirmation flow:
  - Implement `SendEmailConfirmationCommand` — generate ASP.NET Identity email confirmation token, queue email
  - Implement `POST /api/v1/auth/confirm-email` endpoint — validate token, mark email confirmed
- [ ] Implement `IOneTimeCodeService` for OTP generation and validation (time-based, stored in cache or DB)
- [ ] Implement `RequestPasswordResetCommand` and handler in `UseCases/Account/Commands/PasswordReset/`
  - Generate OTP via `IOneTimeCodeService`
  - Publish `PasswordResetRequested` integration event to RabbitMQ
- [ ] Implement `ConfirmPasswordResetCommand` and handler
  - Validate OTP via `IOneTimeCodeService`
  - Reset user password via `UserManager`
- [ ] Implement `POST /api/v1/auth/password-reset/request` and `POST /api/v1/auth/password-reset/confirm` endpoints
- [ ] Add MassTransit NuGet packages (`MassTransit`, `MassTransit.RabbitMQ`) to `GoodMarket.Identity.Api`
- [ ] Configure MassTransit in `IdentityConfiguration.cs` with RabbitMQ transport (connection string from Aspire service discovery)
- [ ] Publish `UserRegistered` event from `RegisterUserCommandHandler` after successful registration
- [ ] Publish `PasswordResetRequested` event from `RequestPasswordResetCommand` handler
- [ ] Write integration tests in `GoodMarket.Identity.Integration`:
  - Register → Login → Refresh → Logout flow
  - Google callback creates user if not exists
  - Password reset OTP validate and reset
  - Confirm email token validation

## Definition of Done

- [ ] `GET /api/v1/auth/google` initiates Google OAuth2 flow
- [ ] `GET /api/v1/auth/google/callback` issues JWT for valid Google token
- [ ] `POST /api/v1/auth/password-reset/request` publishes `PasswordResetRequested` to RabbitMQ (verifiable via Aspire dashboard)
- [ ] `POST /api/v1/auth/password-reset/confirm` resets password with valid OTP
- [ ] `UserRegistered` event visible in RabbitMQ on new registration
- [ ] All integration tests passing
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
