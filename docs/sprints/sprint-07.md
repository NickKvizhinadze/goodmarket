# Sprint 7 — Cart Service

**Roadmap phase:** Phase 4
**Depends on:** Sprint 6 complete (Catalog fully operational — Cart makes sync HTTP call to Catalog on add-to-cart, per ADR-006)
**Blocker:** None

## Goal

A fully working Cart service where users can add, update, and remove items, apply coupons, view totals, and have their guest cart merged on login — with local product snapshots kept up to date via Catalog events.

## Tasks

- [ ] `.\Create-Microservice.ps1 -MicroserviceName Cart`
- [ ] Add MassTransit (`MassTransit`, `MassTransit.RabbitMQ`) to `GoodMarket.Cart.Api`
- [ ] Add `GoodMarket.Shared.Contracts` project reference to `GoodMarket.Cart.Api`
- [ ] Add Cart domain entities to `GoodMarket.Cart.Api/Domain/`:
  - `Cart` — `Id`, `UserId` (nullable for guest), `GuestId` (cookie-based), `CreatedAt`, `UpdatedAt`
  - `CartItem` — `Id`, `CartId`, `ProductId`, `ProductName`, `ProductImageUrl`, `UnitPrice`, `Quantity`, `IsOutOfStock`
  - `Coupon` — `Id`, `Code`, `DiscountType` (percentage/fixed), `DiscountValue`, `ExpiresAt`, `IsActive`
- [ ] Add EF Core configurations for Cart entities, create `goodmarket_cart` DB, generate migration
- [ ] Add typed HTTP client `ICatalogClient` to call Catalog service at `GET /api/v1/products/{id}` for price and existence validation (per ADR-006)
- [ ] Implement `AddItemCommand` and handler — call `ICatalogClient` to validate product and get current price, add `CartItem` with product snapshot
- [ ] Implement `UpdateItemQuantityCommand` and handler — update `CartItem.Quantity`
- [ ] Implement `RemoveItemCommand` and handler — delete `CartItem`
- [ ] Implement `ClearCartCommand` and handler — delete all `CartItem` rows for a cart
- [ ] Implement `ApplyCouponCommand` and handler — validate `Coupon` code, attach to cart, recalculate totals
- [ ] Implement `GetCartQuery` and handler — return cart with items, totals (subtotal, discount, shipping estimate)
- [ ] Implement `MergeGuestCartCommand` and handler — merge anonymous cart into authenticated user cart on login
- [ ] Implement REST endpoints:
  - `GET /api/v1/cart` — get current user or guest cart
  - `POST /api/v1/cart/items` — add item
  - `PUT /api/v1/cart/items/{itemId}` — update quantity
  - `DELETE /api/v1/cart/items/{itemId}` — remove item
  - `DELETE /api/v1/cart` — clear cart
  - `POST /api/v1/cart/coupon` — apply coupon
  - `POST /api/v1/cart/merge` — merge guest cart (requires auth)
- [ ] Subscribe to `ProductUpdated` event — update `CartItem.ProductName`, `CartItem.UnitPrice`, `CartItem.ProductImageUrl` snapshot
- [ ] Subscribe to `InventoryUpdated` event — set `CartItem.IsOutOfStock = true` if quantity drops to 0
- [ ] Subscribe to `ProductDeleted` event — remove affected `CartItem` rows from all carts
- [ ] Register Cart service in AppHost, wire to `postgres` and `rabbitmq`
- [ ] Add Gateway routes: `/api/cart/{**catch-all}` → Cart service
- [ ] Write integration tests: add item validates price from Catalog, coupon discount applied correctly, guest merge preserves items

## Definition of Done

- [ ] `POST /api/cart/items` adds a product with price snapshot from Catalog
- [ ] `POST /api/cart/coupon` applies discount and correct totals are returned
- [ ] `POST /api/cart/merge` merges guest cart into user cart on login
- [ ] `InventoryUpdated` event causes `CartItem.IsOutOfStock` to be set correctly
- [ ] Cart endpoints reachable through Gateway; user carts are isolated by `X-User-Id` header
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
