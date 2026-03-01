# Sprint 6 — Catalog: Queries, Filtering, Pagination, Bulk Import

**Roadmap phase:** Phase 3 (part 2)
**Depends on:** Sprint 5 complete (Catalog write commands implemented)
**Blocker:** None

## Goal

Catalog service exposes all public read endpoints — filtered and paginated product lists, category trees, brand lists, search — plus a bulk product import command, completing the Catalog service.

## Tasks

- [ ] Implement `GetProductsQuery` and handler — paginated, filterable (categoryId, brandId, specificationIds, priceRange, inStock), sorted (price asc/desc, newest)
- [ ] Implement `GetProductByIdQuery` and handler — return full product detail including language rows for requested locale, specifications, inventory, image URLs
- [ ] Implement `SearchProductsQuery` and handler — full-text search on product name and description language rows
- [ ] Implement `GET /api/v1/products` endpoint (public, no auth) with query string filters: `categoryId`, `brandId`, `page`, `pageSize`, `sort`, `locale`
- [ ] Implement `GET /api/v1/products/{id}` endpoint (public)
- [ ] Implement `GET /api/v1/products/search?q=...` endpoint (public)
- [ ] Implement `GetCategoriesQuery` and handler — return category tree (hierarchical, with child categories) for given locale
- [ ] Implement `GET /api/v1/categories` endpoint (public) — returns full category tree
- [ ] Implement `GET /api/v1/categories/{id}` endpoint (public)
- [ ] Implement `GetBrandsQuery` and handler — return brands optionally filtered by categoryId
- [ ] Implement `GET /api/v1/brands` endpoint (public)
- [ ] Implement `GetSpecificationsQuery` and handler — return specification types and specifications for a given categoryId
- [ ] Implement `GET /api/v1/specifications?categoryId=...` endpoint (public)
- [ ] Implement `BulkImportProductsCommand` and handler — accept a CSV/JSON payload, upsert products, emit `ProductCreated`/`ProductUpdated` per product
- [ ] Implement `POST /api/v1/products/import` endpoint (admin role)
- [ ] Add response caching (in-memory or Redis) to `GetCategoriesQuery` and `GetBrandsQuery` results
- [ ] Write integration tests for query endpoints: pagination cursor is stable, filters narrow results correctly, search returns ranked results
- [ ] Verify Catalog service is fully registered in AppHost and all routes reachable through Gateway

## Definition of Done

- [ ] `GET /api/products?categoryId=1&page=1&pageSize=20` returns paginated, filtered results
- [ ] `GET /api/products/search?q=laptop` returns matching products ranked by relevance
- [ ] `GET /api/categories` returns a correctly nested category tree
- [ ] `POST /api/products/import` upserts products and emits events for each
- [ ] All public endpoints return correct locale-specific language fields when `locale` param is provided
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
