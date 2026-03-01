# Sprint 16 — Reporting Service + Survey Service

**Roadmap phase:** Phase 11
**Depends on:** Sprint 9 complete (Orders and payments data available); Sprint 6 complete (Catalog data available); Sprint 3 complete (Identity user data available)
**Blocker:** None

## Goal

A Reporting service exposing admin-only aggregate read endpoints for sales, orders, and product data, plus a self-contained Survey service where users can answer questions — completing the full GoodMarket feature set.

## Tasks

### Reporting Service

- [ ] `.\Create-Microservice.ps1 -MicroserviceName Reporting`
- [ ] Add MassTransit (`MassTransit`, `MassTransit.RabbitMQ`) to `GoodMarket.Reporting.Api`
- [ ] Add `GoodMarket.Shared.Contracts` project reference to `GoodMarket.Reporting.Api`
- [ ] Add Reporting domain entities (read models / projections) to `GoodMarket.Reporting.Api/Domain/`:
  - `DailySalesReport` — `Date`, `TotalOrders`, `TotalRevenue`, `AverageOrderValue`
  - `ProductSalesReport` — `ProductId`, `ProductName`, `UnitsSold`, `TotalRevenue`, `Period`
  - `UserRegistrationReport` — `Date`, `NewUsers`, `CumulativeUsers`
- [ ] Create `goodmarket_reporting` DB and generate migration for projection tables
- [ ] Implement MassTransit consumers to build projections:
  - `OrderPaidConsumer` — upsert `DailySalesReport` and `ProductSalesReport` for today
  - `UserRegisteredConsumer` — upsert `UserRegistrationReport` for today
  - `OrderCancelledConsumer` — decrement `DailySalesReport` totals for the order date
- [ ] Implement queries and handlers:
  - `GetSalesReportQuery` — returns `DailySalesReport` rows for a date range
  - `GetTopProductsQuery` — returns top N products by units sold in a period
  - `GetUserRegistrationsQuery` — returns `UserRegistrationReport` rows for a date range
- [ ] Implement REST endpoints (all admin role required):
  - `GET /api/v1/reports/sales?from=...&to=...` — daily sales totals
  - `GET /api/v1/reports/products/top?period=...&limit=...` — top selling products
  - `GET /api/v1/reports/users?from=...&to=...` — user registrations over time
- [ ] Register Reporting service in AppHost, wire to `postgres` and `rabbitmq`
- [ ] Add Gateway routes: `/api/reports/{**catch-all}` → Reporting service

### Survey Service

- [ ] `.\Create-Microservice.ps1 -MicroserviceName Survey`
- [ ] Add Survey domain entities to `GoodMarket.Survey.Api/Domain/`:
  - `Question` — `Id`, `Text`, `QuestionType` (MultipleChoice/OpenText/Rating), `IsActive`, `DisplayOrder`
  - `QuestionOption` — `Id`, `QuestionId`, `OptionText`, `DisplayOrder` (for MultipleChoice)
  - `SurveyResponse` — `Id`, `UserId`, `QuestionId`, `SelectedOptionId` (nullable), `OpenTextAnswer` (nullable), `Rating` (nullable), `SubmittedAt`
- [ ] Create `goodmarket_survey` DB and generate migration
- [ ] Implement commands and handlers:
  - `CreateQuestionCommand` — create question with options (admin only)
  - `UpdateQuestionCommand` — update question and options (admin only)
  - `DeleteQuestionCommand` — soft delete question (admin only)
  - `SubmitResponseCommand` — record a user's answer to a question (requires auth)
- [ ] Implement queries and handlers:
  - `GetActiveQuestionsQuery` — return active survey questions with options
  - `GetSurveyResultsQuery` — return aggregated results per question (admin only)
- [ ] Implement REST endpoints:
  - `GET /api/v1/survey/questions` — public, returns active questions
  - `POST /api/v1/survey/responses` — authenticated, submit a response
  - `POST /api/v1/survey/questions` — admin only, create question
  - `PUT /api/v1/survey/questions/{id}` — admin only
  - `DELETE /api/v1/survey/questions/{id}` — admin only
  - `GET /api/v1/survey/results` — admin only, aggregated results
- [ ] Register Survey service in AppHost, wire to `postgres`
- [ ] Add Gateway routes: `/api/survey/{**catch-all}` → Survey service
- [ ] Add Survey screens to Admin Desktop app (`SurveyPage` widget):
  - Questions management tab — list, create, edit, delete questions
  - Results tab — aggregated chart/table of responses per question
- [ ] Write integration tests for Reporting projections: simulate OrderPaid event, verify DailySalesReport row created; simulate UserRegistered event, verify UserRegistrationReport row created

## Definition of Done

- [ ] `GET /api/reports/sales?from=2026-01-01&to=2026-01-31` returns daily sales data (admin token required)
- [ ] `GET /api/reports/products/top?period=30d&limit=10` returns top 10 products by revenue
- [ ] `DailySalesReport` projection is updated when `OrderPaid` event is received
- [ ] `GET /api/survey/questions` returns active survey questions (public, no auth)
- [ ] `POST /api/survey/responses` records a user answer and returns 201
- [ ] `GET /api/survey/results` (admin) returns aggregated response counts per option
- [ ] Survey management is accessible from the Admin Desktop app sidebar
- [ ] All report and survey endpoints return 403 for non-admin users on admin-only routes
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
