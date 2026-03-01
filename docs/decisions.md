# Architecture Decision Records

All architectural decisions made during planning. Each entry captures what was decided, what was rejected, and why.

| ADR | Topic | Decision |
|---|---|---|
| 001 | Orchestration | .NET Aspire replaces docker-compose |
| 002 | Observability | Aspire dashboard only (OTLP) |
| 003 | Messaging | RabbitMQ replaces LocalStack SQS/SNS |
| 004 | File storage | Deferred — Azure Blob / AWS S3 / MinIO |
| 005 | Database | One PostgreSQL instance, separate DB per service |
| 006 | Inter-service comms | Async (RabbitMQ) by default, sync HTTP in 2 cases only |
| 007 | Authentication | Custom JWT in Identity service |
| 008 | JWT validation | Gateway only — forwards identity headers downstream |
| 009 | API Gateway shape | Single unified YARP gateway for all clients |
| 010 | Gateway routing | Domain-concept prefixes (`/api/products`, `/api/orders`, …) |
| 011 | Customer frontend | Next.js (TypeScript, App Router) |
| 012 | Admin panel | C++ desktop app, framework TBD (Qt frontrunner) |
| 013 | Mobile app | Deferred — React Native or Flutter |

---

## ADR-001 — Orchestration: .NET Aspire over docker-compose

**Decision:** Use .NET Aspire as the local development orchestrator.

**Rejected:** `docker-compose`

**Reason:** Aspire provides automatic service discovery, a built-in dashboard for traces/logs/metrics (OTLP), and manages infrastructure containers (PostgreSQL, RabbitMQ) automatically. It eliminates the need for manual docker-compose configuration and gives a better local dev experience out of the box.

---

## ADR-002 — Observability: Aspire Dashboard Only

**Decision:** Use the .NET Aspire built-in dashboard as the sole observability tool for local development.

**Rejected:** Jaeger, Prometheus, Grafana, Loki, standalone OpenTelemetry Collector

**Reason:** The Aspire dashboard receives OTLP directly and surfaces distributed traces, structured logs, and metrics in one UI with zero configuration. Running 5 separate observability containers for local dev is unnecessary overhead. Production observability stack to be decided separately when needed.

---

## ADR-003 — Messaging: RabbitMQ over LocalStack (SQS/SNS)

**Decision:** Use RabbitMQ for all async inter-service messaging.

**Rejected:** LocalStack (AWS SQS + SNS emulation)

**Reason:** RabbitMQ is simpler to run and reason about for a self-contained microservices system. LocalStack added unnecessary cloud emulation overhead with no benefit at this stage. RabbitMQ is managed by Aspire via `Aspire.Hosting.RabbitMQ`.

---

## ADR-004 — File Storage: Decision Deferred

**Decision:** File storage provider for the Media service is not yet decided.

**Options under consideration:**
- Azure Blob Storage (Azurite for local dev via Aspire)
- AWS S3 (MinIO for local dev via Aspire)
- Self-hosted MinIO (dev and production)

**Reason deferred:** No cloud hosting target has been chosen yet. Decision should be made when the Media service is being built (Phase 2 in the roadmap).

---

## ADR-005 — Database: One PostgreSQL Instance, Separate Database per Service

**Decision:** Run a single PostgreSQL container (managed by Aspire) with one logical database per microservice.

**Rejected:** Separate PostgreSQL instance per service

**Reason:** Separate instances per service would mean 9+ database containers running locally — significant overhead with no benefit at this stage. A single instance with isolated databases maintains the database-per-service pattern (no cross-service joins, independent migrations, independent connection strings) while being simple to operate. Can be split into separate instances later if scaling requires it, without any service code changes.

| Service | Database |
|---|---|
| Identity | `goodmarket_identity` |
| Catalog | `goodmarket_catalog` |
| Cart | `goodmarket_cart` |
| Orders | `goodmarket_orders` |
| CMS | `goodmarket_cms` |
| Media | `goodmarket_media` |
| Reporting | `goodmarket_reporting` |
| Survey | `goodmarket_survey` |

---

## ADR-006 — Inter-Service Communication: Async by Default, Sync Minimised

**Decision:** RabbitMQ events for all inter-service communication except two specific sync HTTP cases.

**Sync HTTP (exceptions):**

| Caller | Target | Reason |
|---|---|---|
| Cart | Catalog | Validate product exists and fetch current price on add-to-cart |
| Orders | Cart | Read cart contents at checkout to build the order |

**Reason:** Async event-driven communication decouples services and improves resilience. Sync HTTP is only justified where a real-time response is strictly required. Cart keeps a local product snapshot (name, price, image) kept up to date via Catalog events — so after the initial add, no further calls to Catalog are needed.

---

## ADR-007 — Authentication: Custom JWT over External Identity Provider

**Decision:** Build a custom JWT issuer inside the Identity microservice (ASP.NET Identity + custom token logic).

**Rejected:** Keycloak, Duende IdentityServer, OpenIddict

**Reason:** Keycloak is a heavy external dependency with limited customisation. Duende IdentityServer and OpenIddict add complexity that isn't needed at this stage. Custom JWT gives full control over the token lifecycle (access token + refresh token with rotation) and keeps the Identity service self-contained.

---

## ADR-008 — JWT Validation: Gateway Only

**Decision:** JWT is validated once at the API Gateway. Downstream services do not run JWT validation middleware — they read trusted identity headers forwarded by the gateway (`X-User-Id`, `X-User-Roles`).

**Rejected:** Each service validates JWT independently; both gateway and per-service validation

**Reason:** Validating in every service duplicates auth middleware across 9+ services and requires each to hold the signing key. The gateway is the single entry point — trusting the internal network behind it is an acceptable tradeoff for this architecture. Per-service validation adds security depth but significant complexity without meaningful benefit given all traffic routes through Aspire's internal network.

---

## ADR-009 — API Gateway: Single Unified YARP Gateway

**Decision:** One YARP gateway serves all clients (Next.js, desktop, mobile).

**Rejected:** Separate BFF (Backend for Frontend) per client type

**Reason:** A BFF per client means maintaining 3 separate gateway projects with overlapping routing rules. At this scale, a single gateway with domain-concept route prefixes is sufficient. Next.js server components can handle any client-specific response aggregation themselves.

---

## ADR-010 — Gateway Routing: Domain-Concept Prefixes

**Decision:** Gateway routes use domain-concept prefixes that hide internal service boundaries.

**Rejected:** Service-name prefixes (e.g. `/api/catalog/products`, `/api/identity/auth`)

**Reason:** Domain-concept routes (`/api/products`, `/api/auth`) decouple the public API contract from internal service ownership. Services can be split, merged, or renamed without changing any client-facing URLs.

---

## ADR-011 — Customer Storefront: Next.js

**Decision:** Next.js (TypeScript, App Router) for the customer-facing e-commerce site.

**Reason:** Server-side rendering is critical for SEO on product and category pages. Next.js App Router provides SSR, excellent TypeScript support, and a natural fit for a storefront that needs both static and dynamic pages. JWT stored in HttpOnly cookies via Next.js middleware.

---

## ADR-012 — Admin Panel: C++ Desktop Application

**Decision:** Build the admin control panel as a native desktop application in C++.

**Framework:** TBD — Qt is the frontrunner (cross-platform, mature, well-documented). Decision deferred until admin app development begins.

**Rejected:** Web-based admin panel

**Reason:** Desktop gives native performance for data-heavy admin operations and is an opportunity to learn C++. Admin is internal-use only so there is no need for web deployment.

---

## ADR-013 — Mobile App: Decision Deferred

**Decision:** Mobile app technology not yet decided.

**Options:**
- React Native — shares TypeScript/ecosystem with Next.js
- Flutter — better cross-platform UI consistency, Dart language

**Reason deferred:** Mobile is the last item in the roadmap. The decision should be made when the backend is stable and mobile development is ready to begin.

