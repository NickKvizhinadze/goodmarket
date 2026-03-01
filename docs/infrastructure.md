# Infrastructure

## Orchestration — .NET Aspire

.NET Aspire is used as the local development orchestrator. It replaces `docker-compose` entirely.

**What Aspire provides:**
- Automatic service discovery between microservices
- Built-in dashboard: distributed traces, structured logs, metrics (OTLP receiver)
- Resource management: spins up PostgreSQL, RabbitMQ as containers automatically
- `ServiceDefaults` project applies shared configuration to all services (OpenTelemetry, health checks, resilience)

**Projects to create:**
- `GoodMarket.AppHost` — registers all services and infrastructure resources
- `GoodMarket.ServiceDefaults` — shared Aspire defaults referenced by every microservice

## Database — PostgreSQL

**Strategy: one PostgreSQL instance, separate database per service.**

A single PostgreSQL container is managed by Aspire. Each microservice connects to its own named database — no shared tables, no cross-service joins. Services own their data entirely. This is simple to run locally and in early production, and can be split into separate instances per service later without changing any service code.

- EF Core with independent migrations per service
- Each service only knows its own connection string

| Service | Database name |
|---|---|
| Identity | `goodmarket_identity` |
| Catalog | `goodmarket_catalog` |
| Cart | `goodmarket_cart` |
| Orders | `goodmarket_orders` |
| CMS | `goodmarket_cms` |
| Media | `goodmarket_media` |
| Reporting | `goodmarket_reporting` |
| Survey | `goodmarket_survey` |

## Inter-Service Communication

Two communication modes — async by default, sync only when a live response is strictly required.

### Async — RabbitMQ (integration events)

Used for everything that does not need an immediate reply. Services publish events; consumers react independently.

- Managed by Aspire (`Aspire.Hosting.RabbitMQ`)
- Event contracts defined in `GoodMarket.Shared.Contracts`
- No direct service-to-service coupling — the event contract is the only shared dependency

| Event | Published by | Consumed by |
|---|---|---|
| `UserRegistered` | Identity | Notifications |
| `PasswordResetRequested` | Identity | Notifications |
| `ProductCreated` | Catalog | — |
| `ProductUpdated` | Catalog | Cart |
| `ProductDeleted` | Catalog | Cart |
| `InventoryUpdated` | Catalog | Cart |
| `OrderPlaced` | Orders | Notifications |
| `OrderPaid` | Orders | Notifications, Catalog (inventory deduction) |
| `OrderCancelled` | Orders | Notifications, Cart |
| `OrderShipped` | Orders | Notifications |

### Sync — HTTP (direct service calls)

Used only where a real-time response is required and an async event is not sufficient. Kept to a minimum.

| Caller | Target | Reason |
|---|---|---|
| Cart | Catalog | Validate product exists and fetch current price when adding an item to cart |
| Orders | Cart | Read cart contents at checkout to create the order |

Cart stores a **local snapshot** of product data (name, price, image) synced via Catalog events — so after the initial add, Cart never needs to call Catalog again for reads.

## Observability

All services send telemetry via OTLP to the Aspire dashboard for local development.

- **Traces** — distributed request tracing across services
- **Metrics** — request counts, durations, error rates
- **Logs** — structured logs from all services

**What was removed (previously in docker-compose):**
- Jaeger — replaced by Aspire dashboard traces
- Prometheus — replaced by Aspire dashboard metrics
- Grafana — replaced by Aspire dashboard
- Loki — replaced by Aspire dashboard logs
- OpenTelemetry Collector (standalone) — Aspire receives OTLP directly

> For production, a proper observability stack (Grafana Cloud, Datadog, Azure Monitor, etc.) should be chosen separately. Aspire dashboard is for local development only.

## What Was Removed

| Removed | Reason |
|---|---|
| `docker-compose.yml` | Replaced by Aspire AppHost |
| `otel-collector-config.yml` | No longer needed |
| `prometheus.yml` | No longer needed |
| `grafana-datasources.yml` | No longer needed |
| LocalStack (S3, SQS, SNS) | Removed entirely — RabbitMQ replaces SQS/SNS; file storage provider TBD (decision deferred) |
