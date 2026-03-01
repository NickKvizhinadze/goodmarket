# System Architecture

GoodMarket is an e-commerce platform built with a microservices architecture on .NET 9. Each service is independently deployable, owns its own database, and communicates via HTTP (synchronous) or RabbitMQ (asynchronous).

## High-Level Diagram

```
                        ┌─────────────────────────────────┐
                        │         Clients                 │
                        │  Next.js  │  Desktop  │  Mobile │
                        └─────────────────────────────────┘
                                        │
                                        ▼
                        ┌─────────────────────────────────┐
                        │       API Gateway (YARP)        │
                        │  - JWT validation               │
                        │  - Rate limiting (per user/IP)  │
                        │  - Routes by domain concept     │
                        │  - Forwards identity headers    │
                        └─────────────────────────────────┘
                                        │
          ┌─────────────┬───────────────┼───────────────┬─────────────┐
          ▼             ▼               ▼               ▼             ▼
    ┌──────────┐  ┌──────────┐  ┌──────────────┐ ┌─────────┐  ┌──────────┐
    │ Identity │  │ Catalog  │  │    Orders    │ │  Cart   │  │   CMS    │
    └──────────┘  └──────────┘  └──────────────┘ └─────────┘  └──────────┘
          │             │               │               │             │
          ▼             ▼               ▼               ▼             ▼
    ┌──────────┐  ┌──────────┐  ┌──────────────┐ ┌─────────┐  ┌──────────┐
    │ Postgres │  │ Postgres │  │   Postgres   │ │ Postgres│  │ Postgres │
    └──────────┘  └──────────┘  └──────────────┘ └─────────┘  └──────────┘

                        ┌─────────────────────────────────┐
                        │           RabbitMQ              │
                        │     (async integration events)  │
                        └─────────────────────────────────┘
                                        │
          ┌─────────────┬───────────────┼───────────────┐
          ▼             ▼               ▼               ▼
    ┌──────────┐  ┌──────────┐  ┌──────────────┐ ┌─────────────┐
    │  Media   │  │ Notific. │  │  Reporting   │ │   Survey    │
    └──────────┘  └──────────┘  └──────────────┘ └─────────────┘
```

## Technology Stack

| Concern | Technology |
|---|---|
| Runtime | .NET 9 |
| API style | Minimal APIs + API versioning |
| Orchestration (local dev) | .NET Aspire |
| API Gateway | YARP |
| Auth | ASP.NET Identity + Custom JWT |
| CQRS | Custom Mediator (`GoodMarket.Shared.Mediator`) |
| Result handling | Custom Result type (`GoodMarket.Shared.Result`) |
| Database | PostgreSQL (per service) |
| Messaging | RabbitMQ |
| Observability | .NET Aspire dashboard (OTLP) |
| Testing | xUnit + integration tests per service |
| Customer frontend | Next.js (TypeScript, App Router) |
| Admin desktop | C++ (framework TBD — see `client-apps.md`) |
| Mobile | TBD — React Native or Flutter |

## Shared Libraries

| Library | Purpose |
|---|---|
| `GoodMarket.Shared.Mediator` | Custom CQRS — ICommand, IQuery, ICommandHandler, IQueryHandler, IPipelineBehavior |
| `GoodMarket.Shared.Result` | Result type for operation outcomes, error codes |
| `GoodMarket.Shared.Contracts` | Integration event contracts shared across services (planned) |

## Per-Service Structure

Every microservice follows this layout — see `README.md` for full detail and the scaffold script.

```
ServiceName/
├── src/
│   ├── GoodMarket.ServiceName.Api           # Minimal API entry point
│   ├── GoodMarket.ServiceName.Application   # CQRS handlers, DTOs, use cases
│   ├── GoodMarket.ServiceName.Domain        # Entities, domain logic, events
│   ├── GoodMarket.ServiceName.Infrastructure# EF Core, RabbitMQ, external APIs
│   └── GoodMarket.ServiceName.SharedKernel  # Abstractions internal to service
└── tests/
    └── GoodMarket.ServiceName.Integration   # Integration tests
```

## Communication Patterns

### Async — RabbitMQ (default)
All integration events between services. Fire-and-forget; consumers react independently. See `infrastructure.md` for the full event table.

### Sync — HTTP (exceptions only)
Two cases where a live response is required:

| Caller | Target | Reason |
|---|---|---|
| Cart | Catalog | Validate product + get current price on add-to-cart |
| Orders | Cart | Read cart contents at checkout |

All other cross-service data needs are satisfied by local snapshots kept up to date via events.

## Key Principles

- **One PostgreSQL instance, database per service** — single container, each service has its own database; no shared tables, no cross-service joins
- **Async by default** — RabbitMQ integration events for everything that doesn't need an immediate response
- **Sync only when necessary** — two cases: Cart→Catalog on add-to-cart, Orders→Cart at checkout
- **Gateway owns auth** — JWT validated once at the gateway, identity forwarded as trusted headers to all downstream services
- **No shared ORM models** — each service defines its own domain model independently
- **Mobile app technology** — decision deferred; React Native or Flutter, decided when mobile development begins
- **File storage provider** — decision deferred; Azure Blob, AWS S3, or MinIO
