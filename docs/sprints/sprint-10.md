# Sprint 10 — Notifications Service + CMS Service

**Roadmap phase:** Phase 6 + Phase 7
**Depends on:** Sprint 3 complete (Identity events publishing); Sprint 8 complete (OrderPlaced, OrderCancelled events); Sprint 9 complete (OrderPaid, OrderShipped events)
**Blocker:** None

## Goal

A fully event-driven Notifications service sending emails and SMS, and a CMS service with full CRUD for pages, sliders, advertisements, and site settings — both running in Aspire alongside existing services.

## Tasks

### Notifications Service

- [ ] `.\Create-Microservice.ps1 -MicroserviceName Notifications`
- [ ] Add MassTransit (`MassTransit`, `MassTransit.RabbitMQ`) to `GoodMarket.Notifications.Api`
- [ ] Add `GoodMarket.Shared.Contracts` project reference to `GoodMarket.Notifications.Api`
- [ ] Add `IEmailSender` interface and SMTP implementation (configure via `SmtpOptions` in `appsettings.json`)
- [ ] Add `ISmsService` interface and implementation for the SMS provider used in the old project
- [ ] Create email templates directory `GoodMarket.Notifications.Api/Templates/Email/` with `.html` templates for:
  - `UserRegistered` — welcome email
  - `PasswordResetRequested` — OTP email
  - `OrderPlaced` — order confirmation
  - `OrderPaid` — payment confirmation
  - `OrderCancelled` — cancellation notice
  - `OrderShipped` — shipping notification
- [ ] Implement MassTransit consumers (one per event):
  - `UserRegisteredConsumer` — send welcome email
  - `PasswordResetRequestedConsumer` — send OTP email and SMS
  - `OrderPlacedConsumer` — send order confirmation email
  - `OrderPaidConsumer` — send payment confirmation email
  - `OrderCancelledConsumer` — send cancellation email
  - `OrderShippedConsumer` — send shipping notification email + SMS
- [ ] Register all consumers in MassTransit configuration
- [ ] Register Notifications service in AppHost, wire to `rabbitmq` only (no database needed)
- [ ] Note: Notifications service has no public HTTP API (per roadmap Phase 6)

### CMS Service

- [ ] `.\Create-Microservice.ps1 -MicroserviceName Cms`
- [ ] Add CMS domain entities to `GoodMarket.Cms.Api/Domain/`:
  - `Page`, `PageLanguage` — `Id`, `Slug`, `Title`, `Content`, `LanguageCode`, `IsPublished`
  - `Slider`, `SliderLanguage` — `Id`, `Title`, `ImageUrl`, `LinkUrl`, `DisplayOrder`
  - `Advertisement`, `AdvertisementLanguage` — `Id`, `Title`, `ImageUrl`, `LinkUrl`, `Position`, `IsActive`
  - `Setting` — `Id`, `Key`, `Value`, `Description` (key-value store for site config)
- [ ] Add EF Core configurations for all CMS entities, create `goodmarket_cms` DB, generate migration
- [ ] Implement Pages CRUD commands and handlers: `CreatePageCommand`, `UpdatePageCommand`, `DeletePageCommand`, `GetPageBySlugQuery`, `GetPagesQuery`
- [ ] Implement Sliders CRUD commands and handlers: `CreateSliderCommand`, `UpdateSliderCommand`, `DeleteSliderCommand`, `GetSlidersQuery`
- [ ] Implement Advertisements CRUD: `CreateAdvertisementCommand`, `UpdateAdvertisementCommand`, `DeleteAdvertisementCommand`, `GetAdvertisementsQuery`
- [ ] Implement Settings commands and handlers: `SetSettingCommand`, `GetSettingQuery`, `GetAllSettingsQuery`
- [ ] Implement REST endpoints:
  - `GET /api/v1/pages/{slug}` — public read
  - `GET /api/v1/pages` — public list
  - `POST /api/v1/pages`, `PUT /api/v1/pages/{id}`, `DELETE /api/v1/pages/{id}` — admin only
  - `GET /api/v1/sliders` — public
  - `POST /api/v1/sliders`, `PUT /api/v1/sliders/{id}`, `DELETE /api/v1/sliders/{id}` — admin only
  - `GET /api/v1/advertisements` — public
  - `POST /api/v1/advertisements`, `PUT /api/v1/advertisements/{id}`, `DELETE /api/v1/advertisements/{id}` — admin only
  - `GET /api/v1/settings/{key}` — public read
  - `GET /api/v1/settings` — admin only (all settings)
  - `PUT /api/v1/settings/{key}` — admin only
- [ ] Register CMS service in AppHost, wire to `postgres`
- [ ] Add Gateway routes: `/api/pages/...`, `/api/sliders/...`, `/api/advertisements/...`, `/api/settings/...` → CMS service

## Definition of Done

- [ ] Notifications service starts with no public HTTP port; consumers are registered and visible in RabbitMQ management UI
- [ ] `UserRegistered` event triggers welcome email (verifiable in SMTP dev inbox / logs)
- [ ] `OrderPaid` event triggers payment confirmation email
- [ ] `GET /api/pages/{slug}` returns page content with correct language rows
- [ ] `POST /api/pages` (admin) creates a page with GE/EN/RU content
- [ ] `GET /api/settings/{key}` returns current site setting value
- [ ] CMS endpoints reachable through Gateway; write endpoints require admin role
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
