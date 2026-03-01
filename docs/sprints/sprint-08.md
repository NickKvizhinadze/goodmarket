# Sprint 8 — Orders: Lifecycle and State Machine

**Roadmap phase:** Phase 5 (part 1)
**Depends on:** Sprint 7 complete (Cart service operational — Orders reads cart contents via sync HTTP at checkout, per ADR-006)
**Blocker:** None

## Goal

An Orders service that can create an order from a cart, track it through the full state machine (Pending → Confirmed → Shipped → Delivered → Cancelled), and publish lifecycle events to RabbitMQ.

## Tasks

- [ ] `.\Create-Microservice.ps1 -MicroserviceName Orders`
- [ ] Add MassTransit (`MassTransit`, `MassTransit.RabbitMQ`) to `GoodMarket.Orders.Api`
- [ ] Add `GoodMarket.Shared.Contracts` project reference to `GoodMarket.Orders.Api`
- [ ] Add Orders domain entities to `GoodMarket.Orders.Api/Domain/`:
  - `Order` — `Id`, `UserId`, `Status` (enum), `CreatedAt`, `UpdatedAt`, `ShippingAddressId`, `TotalAmount`, `DiscountAmount`, `ShippingCost`
  - `OrderItem` — `Id`, `OrderId`, `ProductId`, `ProductName`, `UnitPrice`, `Quantity`
  - `ShippingAddress` — `Id`, `OrderId`, `FullName`, `AddressLine`, `City`, `PostalCode`, `PhoneNumber`
  - `ShippingConfiguration` — `Id`, `Name`, `Cost`, `EstimatedDays`, `IsActive`
  - `Payment` — `Id`, `OrderId`, `Provider` (BOG/TBC), `ExternalTransactionId`, `Status`, `Amount`, `CreatedAt`
- [ ] Add `OrderStatus` enum: `Pending`, `Confirmed`, `Shipped`, `Delivered`, `Cancelled`
- [ ] Add EF Core configurations for all Orders entities, create `goodmarket_orders` DB, generate migration
- [ ] Add typed HTTP client `ICartClient` to call Cart service at `GET /api/v1/cart` to read cart contents at checkout (per ADR-006)
- [ ] Implement `PlaceOrderCommand` and handler:
  - Call `ICartClient` to fetch cart contents
  - Create `Order` with `OrderItem` rows from cart items
  - Set status to `Pending`
  - Publish `OrderPlaced` integration event (add to `GoodMarket.Shared.Contracts`)
- [ ] Implement `ConfirmOrderCommand` and handler — transition `Pending` → `Confirmed`, publish `OrderConfirmed` event
- [ ] Implement `ShipOrderCommand` and handler — transition `Confirmed` → `Shipped`, persist tracking number, publish `OrderShipped` event
- [ ] Implement `DeliverOrderCommand` and handler — transition `Shipped` → `Delivered`, publish `OrderDelivered` event
- [ ] Implement `CancelOrderCommand` and handler — transition `Pending`/`Confirmed` → `Cancelled`, publish `OrderCancelled` event (add to `GoodMarket.Shared.Contracts`)
- [ ] Implement state machine guard: invalid transitions return a `Result.Failure` error (e.g. cannot cancel a delivered order)
- [ ] Implement `GetOrderQuery` and handler — return full order detail for the owning user
- [ ] Implement `GetOrderHistoryQuery` and handler — return paginated list of orders for a user
- [ ] Implement REST endpoints:
  - `POST /api/v1/orders` — place order (requires auth)
  - `GET /api/v1/orders` — order history (requires auth)
  - `GET /api/v1/orders/{id}` — order detail (requires auth, user can only see own orders)
  - `POST /api/v1/orders/{id}/cancel` — cancel order (requires auth)
  - `POST /api/v1/orders/{id}/confirm` — admin-only transition
  - `POST /api/v1/orders/{id}/ship` — admin-only transition
  - `POST /api/v1/orders/{id}/deliver` — admin-only transition
- [ ] Add `ShippingConfiguration` seed data migration with default shipping options
- [ ] Register Orders service in AppHost, wire to `postgres` and `rabbitmq`
- [ ] Add Gateway routes: `/api/orders/{**catch-all}` → Orders service
- [ ] Write integration tests: place order → confirm → ship → deliver flow, cancel from Pending, invalid state transitions rejected

## Definition of Done

- [ ] `POST /api/orders` creates an order from the current user's cart and emits `OrderPlaced`
- [ ] State machine transitions work correctly and emit events for each transition
- [ ] Invalid state transitions (e.g. cancel delivered order) return a meaningful error
- [ ] `OrderCancelled` event is published and visible in RabbitMQ
- [ ] `GET /api/orders/{id}` returns 403 if the requesting user does not own the order
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
