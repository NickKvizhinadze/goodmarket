# Sprint 14 — Admin Desktop App: Core Management Screens

**Roadmap phase:** Phase 9 (part 2)
**Depends on:** Sprint 13 complete (Qt project scaffolded, ApiClient and AuthService working, main window shell visible)
**Blocker:** None

## Goal

Admin desktop application has fully functional screens for managing products, categories, brands, orders, users, and CMS content — all communicating with the Gateway API.

## Tasks

### Products Screen

- [ ] Create `ProductsPage` widget — table view (`QTableWidget` or `QTableView` with model) listing products (name, category, price, stock, status)
- [ ] Implement `ProductsApiService` — wraps `ApiClient` calls: `getProducts(page, filters)`, `createProduct(dto)`, `updateProduct(id, dto)`, `deleteProduct(id)`
- [ ] Add filter bar: search by name, filter by category (dropdown), filter by brand (dropdown)
- [ ] Add pagination controls (prev/next page, page size selector)
- [ ] Implement `ProductFormDialog` (`QDialog`) — form for create/edit product with fields: name (GE/EN/RU tabs), category, brand, price, specifications, image URL (file picker calling Media service upload)
- [ ] Connect create button → `ProductFormDialog` → `POST /api/products`
- [ ] Connect row double-click → `ProductFormDialog` pre-filled → `PUT /api/products/{id}`
- [ ] Connect delete button → confirm dialog → `DELETE /api/products/{id}`
- [ ] Implement inline inventory edit — `PATCH /api/products/{id}/inventory` called directly from table row

### Categories and Brands Screens

- [ ] Create `CategoriesPage` widget — tree view (`QTreeWidget`) displaying category hierarchy
- [ ] Implement `CategoryFormDialog` — name (GE/EN/RU), parent category dropdown
- [ ] Wire create/edit/delete operations to Gateway category endpoints
- [ ] Create `BrandsPage` widget — table view of brands
- [ ] Implement `BrandFormDialog` — name (GE/EN/RU), associated categories (multi-select)
- [ ] Wire create/edit/delete to Gateway brand endpoints

### Orders Screen

- [ ] Create `OrdersPage` widget — table view of orders (order ID, user email, status, total, date)
- [ ] Add filter bar: filter by status (`QComboBox`), date range (`QDateEdit`)
- [ ] Implement `OrderDetailDialog` — read-only view of order items, shipping address, payment status
- [ ] Add status action buttons: Confirm, Ship, Deliver, Cancel — each calls appropriate Gateway endpoint and refreshes table row

### Users Screen

- [ ] Create `UsersPage` widget — table view of registered users (email, registration date, role)
- [ ] Implement `UserDetailDialog` — view user detail (read-only; no edit needed at this stage)

### CMS Screen

- [ ] Create `CmsPage` widget with tab bar: Pages, Sliders, Advertisements, Settings
- [ ] Implement Pages tab — list of pages with slug, title; create/edit via `PageFormDialog` (slug, title, content textarea per GE/EN/RU)
- [ ] Implement Sliders tab — ordered list of sliders; create/edit/delete/reorder
- [ ] Implement Settings tab — key-value table; inline edit of setting values (`PUT /api/settings/{key}`)

### Image Upload Integration

- [ ] Add `MediaUploadHelper` — opens native file dialog (`QFileDialog`), reads image file, POSTs multipart/form-data to `POST /api/media/upload` via `ApiClient`, returns public URL for use in product/slider forms

## Definition of Done

- [ ] Products table loads real data from Gateway, supports filtering and pagination
- [ ] Create/edit product dialog submits correctly and product appears in table after save
- [ ] Order status transitions (Confirm, Ship, Deliver, Cancel) work from the Orders screen
- [ ] CMS settings values are editable and changes are persisted via the Gateway
- [ ] Image upload in product form returns a URL and displays a preview thumbnail
- [ ] All screens are wired to the main window sidebar navigation
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
