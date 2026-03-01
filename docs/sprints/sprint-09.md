# Sprint 9 — Orders: Payment Integration (BOG Bank, TBC Bank)

**Roadmap phase:** Phase 5 (part 2)
**Depends on:** Sprint 8 complete (Order lifecycle and state machine working)
**Blocker:** None

## Goal

Orders service supports real payment initiation and callback handling for both BOG Bank and TBC Bank, transitioning orders through payment states and publishing `OrderPaid` events on successful payment.

## Tasks

- [ ] Add `IBogPaymentService` interface in `GoodMarket.Orders.Api/Infrastructure/Payments/Bog/`
- [ ] Implement `BogPaymentService` — integrate BOG Bank payment API:
  - `InitiatePayment(orderId, amount, callbackUrl)` — call BOG API, return redirect URL and external transaction ID
  - `VerifyPayment(transactionId)` — query BOG API for payment status
- [ ] Add `ITbcPaymentService` interface in `GoodMarket.Orders.Api/Infrastructure/Payments/Tbc/`
- [ ] Implement `TbcPaymentService` — integrate TBC Bank payment API:
  - `InitiatePayment(orderId, amount, callbackUrl)` — call TBC API, return redirect URL and external transaction ID
  - `VerifyPayment(transactionId)` — query TBC API for payment status
- [ ] Add BOG and TBC API credentials/config to `appsettings.json` as typed options (`BogPaymentOptions`, `TbcPaymentOptions`)
- [ ] Implement `InitiatePaymentCommand` and handler:
  - Accept `orderId` and `provider` (BOG or TBC)
  - Call appropriate payment service to initiate
  - Persist `Payment` record with status `Pending` and `ExternalTransactionId`
  - Return redirect URL to client
- [ ] Implement `POST /api/v1/orders/{id}/pay` endpoint — initiate payment, returns `{ redirectUrl }` (requires auth, user owns order)
- [ ] Implement `HandleBogCallbackCommand` and handler:
  - Verify signature/authenticity of BOG webhook payload
  - Update `Payment` status to `Completed` or `Failed`
  - If completed: transition order to `Confirmed`, publish `OrderPaid` event (add to `GoodMarket.Shared.Contracts`)
- [ ] Implement `POST /api/v1/webhooks/bog` endpoint — public, no auth, handles BOG async callback
- [ ] Implement `HandleTbcCallbackCommand` and handler:
  - Verify TBC webhook payload
  - Update `Payment` status
  - If completed: transition order, publish `OrderPaid` event
- [ ] Implement `POST /api/v1/webhooks/tbc` endpoint — public, no auth, handles TBC async callback
- [ ] Add idempotency guard: if a webhook is received twice for the same `ExternalTransactionId`, skip processing
- [ ] Implement `GetPaymentStatusQuery` — return payment status for an order
- [ ] Implement `GET /api/v1/orders/{id}/payment` endpoint (requires auth, user owns order)
- [ ] Add Gateway webhook routes: `/api/webhooks/bog` and `/api/webhooks/tbc` → Orders service (no JWT validation on these routes — configure as bypass in Gateway)
- [ ] Write integration tests: initiate BOG payment creates Payment record, simulate callback webhook transitions order to Confirmed, duplicate webhook is idempotent

## Definition of Done

- [ ] `POST /api/orders/{id}/pay?provider=bog` returns a redirect URL from BOG API
- [ ] `POST /api/webhooks/bog` processes callback, transitions order to Confirmed, emits `OrderPaid`
- [ ] `POST /api/webhooks/tbc` processes callback, transitions order to Confirmed, emits `OrderPaid`
- [ ] `OrderPaid` event visible in RabbitMQ after successful payment callback
- [ ] Duplicate callback for same transaction is handled idempotently (no double-transition)
- [ ] Webhook endpoints reachable through Gateway without JWT (no-auth bypass configured)
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
