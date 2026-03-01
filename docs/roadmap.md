# Implementation Roadmap

Phased plan for building GoodMarket from the ground up. Phases are ordered by dependency — each phase unblocks the next.

## Phase 0 — Foundation (Do First, Unblocks Everything)

### 0.1 — .NET Aspire Setup
- Create `GoodMarket.AppHost` project — Aspire orchestrator, wires all services and infrastructure
- Create `GoodMarket.ServiceDefaults` project — shared Aspire defaults applied to every service (OpenTelemetry, health checks, resilience)
- Register PostgreSQL as an Aspire resource
- Register RabbitMQ as an Aspire resource
- Wire existing Identity service into AppHost

### 0.2 — API Gateway
- Create `GoodMarket.Gateway` project (YARP)
- Configure JWT validation middleware
- Configure rate limiting (per user/IP)
- Configure identity header forwarding to downstream services
- Register in AppHost
- See `api-gateway.md` for full routing design

### 0.3 — Shared Contracts Library
- Create `GoodMarket.Shared.Contracts` — integration event message contracts shared across services
- Events published/consumed via RabbitMQ reference types from this library

---

## Phase 1 — Complete Identity Service

Already scaffolded. Remaining work:

- Login endpoint (JWT access token + refresh token)
- Refresh token endpoint
- Logout / token revocation
- Google OAuth2 callback → issue our own JWT
- Email confirmation flow
- Password reset (OTP-based, matching old project's `IOneTimeCodeService`)
- Publish `UserRegistered` integration event to RabbitMQ
- Integration tests

---

## Phase 2 — Media Service

Small, self-contained, unblocks Catalog (products need images).

- Image upload endpoint (store to... TBD — LocalStack removed, decide storage later)
- Image delete endpoint
- Return public URLs
- Integration tests

> Note: File storage provider TBD since LocalStack was removed. Options: Azure Blob Storage, AWS S3 (production), or local disk for dev via Aspire.

---

## Phase 3 — Catalog Service

Largest service. Covers products, categories, brands, specifications from the old monolith.

**Domain:** Product, Category, Brand, Specification, SpecificationType, Inventory, City

**Multi-language:** Keep the Language entity pattern from the old project (ProductLanguage, CategoryLanguage, etc.) — supports GE, EN, RU.

**Commands:** CreateProduct, UpdateProduct, DeleteProduct, UpdateInventory, ImportProducts (replaces old ProductParser)

**Queries:** GetProducts (filtered, paginated), GetProductById, GetCategories, GetBrands, GetSpecifications, SearchProducts

**Events published:** `ProductCreated`, `ProductUpdated`, `ProductDeleted`, `InventoryUpdated`

---

## Phase 4 — Cart Service

- Add / remove / update cart items
- Apply coupon / discount codes
- Calculate totals (incl. shipping)
- Merge guest cart with user cart on login
- Subscribe to `ProductUpdated`, `InventoryUpdated` to invalidate stale cart items

---

## Phase 5 — Orders Service

- Create order from cart
- Order status state machine: Pending → Confirmed → Shipped → Delivered → Cancelled
- Payment integration (BOG Bank, TBC Bank — same providers as old project)
- Payment webhook / callback handling
- Shipping configuration
- Order history per user
- Events published: `OrderPlaced`, `OrderPaid`, `OrderCancelled`
- Subscribes to payment provider callbacks

---

## Phase 6 — Notifications Service

Fully event-driven — no public HTTP API.

- Subscribes to: `UserRegistered`, `OrderPlaced`, `OrderPaid`, `OrderCancelled`, `PasswordResetRequested`
- Email sending (SMTP or SendGrid)
- SMS sending (same provider as old project)
- Email/SMS templates per event type

---

## Phase 7 — CMS Service

Replaces old project's Pages, Sliders, Advertisements, Settings.

- Pages CRUD with multi-language support (GE, EN, RU)
- Sliders CRUD
- Advertisements CRUD
- Settings key-value store (site config)
- Public read endpoints + admin write endpoints

---

## Phase 8 — Customer Storefront (Next.js)

- Next.js App Router (TypeScript)
- Communicates with all services via API Gateway
- JWT stored in HttpOnly cookies via Next.js middleware
- SEO-optimised product/category pages (SSR)
- Cart UI
- Checkout + payment flow
- User account area (orders, profile, addresses)

---

## Phase 9 — Admin Desktop App (C++)

- C++ desktop application for internal admin use
- Framework TBD — see `client-apps.md`
- Manage products, categories, brands, orders, users, CMS content
- Communicates with services via API Gateway using JWT (stored locally)

---

## Phase 10 — Mobile App

- Technology TBD — React Native or Flutter
- Customer-facing: browse products, cart, checkout, order tracking
- See `client-apps.md` for decision context

---

## Phase 11 — Reporting & Survey

Lower priority, implement last.

**Reporting:** Aggregate data from Orders, Catalog, Identity. Read-only views or event-sourced projections.

**Survey:** Self-contained — Questions, Answers, responses per user.

---

## Build Order Summary

```
Phase 0  → AppHost + Gateway + Shared.Contracts
Phase 1  → Identity (complete)
Phase 2  → Media
Phase 3  → Catalog
Phase 4  → Cart
Phase 5  → Orders
Phase 6  → Notifications       ← can run in parallel with Phase 4/5
Phase 7  → CMS                 ← can run in parallel with Phase 4/5
Phase 8  → Next.js Storefront
Phase 9  → Admin Desktop (C++)
Phase 10 → Mobile App
Phase 11 → Reporting + Survey
```
