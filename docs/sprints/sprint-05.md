# Sprint 5 — Catalog: Categories, Brands, Products CRUD Commands

**Roadmap phase:** Phase 3 (part 1)
**Depends on:** Sprint 4 complete (Catalog domain model and migration in place)
**Blocker:** None

## Goal

Catalog service accepts admin write commands — create/update/delete categories, brands, and products (with multi-language support and inventory) — all reachable through the Gateway.

## Tasks

- [ ] Add MassTransit (`MassTransit`, `MassTransit.RabbitMQ`) to `GoodMarket.Catalog.Api` and configure with RabbitMQ transport
- [ ] Add `GoodMarket.Shared.Contracts` project reference to `GoodMarket.Catalog.Api`
- [ ] Implement `CreateCategoryCommand` and handler — persist `Category` + `CategoryLanguage` rows for GE/EN/RU
- [ ] Implement `UpdateCategoryCommand` and handler — update category and all language rows
- [ ] Implement `DeleteCategoryCommand` and handler — soft or hard delete
- [ ] Implement `POST /api/v1/categories`, `PUT /api/v1/categories/{id}`, `DELETE /api/v1/categories/{id}` endpoints (admin role required)
- [ ] Implement `CreateBrandCommand` and handler — persist `Brand` + `BrandLanguage` + `BrandCategory` associations
- [ ] Implement `UpdateBrandCommand` and handler
- [ ] Implement `DeleteBrandCommand` and handler
- [ ] Implement `POST /api/v1/brands`, `PUT /api/v1/brands/{id}`, `DELETE /api/v1/brands/{id}` endpoints (admin role)
- [ ] Implement `CreateProductCommand` and handler — persist `Product` + `ProductLanguage` rows, link specifications via `ProductSpecification`, create `Inventory` row
- [ ] Implement `UpdateProductCommand` and handler — update product, language rows, specification links
- [ ] Implement `DeleteProductCommand` and handler — emit `ProductDeleted` event to RabbitMQ
- [ ] Implement `UpdateInventoryCommand` and handler — update stock quantity, emit `InventoryUpdated` event to RabbitMQ
- [ ] Publish `ProductCreated`, `ProductUpdated`, `ProductDeleted`, `InventoryUpdated` integration events — add these to `GoodMarket.Shared.Contracts`
- [ ] Implement `POST /api/v1/products`, `PUT /api/v1/products/{id}`, `DELETE /api/v1/products/{id}`, `PATCH /api/v1/products/{id}/inventory` endpoints (admin role)
- [ ] Add FluentValidation validators for all commands
- [ ] Add Gateway routes: `/api/categories/...`, `/api/brands/...`, `/api/products/...` → Catalog service

## Definition of Done

- [ ] `POST /api/categories` creates a category with GE/EN/RU language rows
- [ ] `POST /api/products` creates a product with language rows, specifications, and inventory
- [ ] `ProductCreated` and `InventoryUpdated` events visible in RabbitMQ after respective operations
- [ ] All write endpoints require admin role — return 403 without valid admin JWT
- [ ] All endpoints reachable through Gateway
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
