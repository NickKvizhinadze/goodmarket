# Microservices

## Service Map

| Service | Responsibility | Status |
|---|---|---|
| Identity | Auth, users, JWT, Google OAuth2 | In progress |
| Catalog | Products, categories, brands, specifications, inventory | Planned |
| Cart | Shopping cart, coupons | Planned |
| Orders | Order lifecycle, payments (BOG/TBC) | Planned |
| Media | File/image uploads | Planned |
| CMS | Pages, sliders, advertisements, site settings | Planned |
| Notifications | Email and SMS, event-driven only | Planned |
| Reporting | Analytics, admin reports | Planned (low priority) |
| Survey | Questions and answers | Planned (low priority) |

Also see cross-cutting concerns:
- `GoodMarket.AppHost` — Aspire orchestrator
- `GoodMarket.ServiceDefaults` — shared Aspire config
- `GoodMarket.Gateway` — YARP API gateway

---

## Identity Service

**What it owns:** Users, roles, JWT tokens, Google OAuth2, password reset, email confirmation

**Migrated from old project:** `IUserService`, `IOneTimeCodeService`, `IProfileService` (partial), ASP.NET Identity

**Domain entities:** `ApplicationUser`, `RefreshToken`

**Key endpoints:**
- `POST /api/v1/auth/register`
- `POST /api/v1/auth/login`
- `POST /api/v1/auth/refresh`
- `POST /api/v1/auth/logout`
- `GET  /api/v1/auth/google` (OAuth2 initiation)
- `GET  /api/v1/auth/google/callback`
- `POST /api/v1/auth/password-reset/request`
- `POST /api/v1/auth/password-reset/confirm`

**Publishes:** `UserRegistered`, `PasswordResetRequested`

---

## Catalog Service

**What it owns:** Products, categories, brands, specifications, specification types, inventory, cities

**Migrated from old project:** `IProductService`, `ICategoryService`, `IBrandService`, `ISpecificationService`, `ISpecificationTypeService`, `ICityService`, `IProductParser`

**Domain entities:** `Product`, `ProductLanguage`, `Category`, `CategoryLanguage`, `Brand`, `BrandLanguage`, `BrandCategory`, `Specification`, `SpecificationLanguage`, `SpecificationType`, `SpecificationTypeLanguage`, `SpecificationCategory`, `ProductSpecification`, `Inventory`, `City`, `CityLanguage`

**Multi-language:** All content entities have a corresponding Language entity (GE, EN, RU).

**Key queries:** GetProducts (filtered/paginated), GetProductById, GetCategories, GetBrands, SearchProducts, GetSpecifications

**Key commands:** CreateProduct, UpdateProduct, DeleteProduct, UpdateInventory, BulkImportProducts

**Publishes:** `ProductCreated`, `ProductUpdated`, `ProductDeleted`, `InventoryUpdated`

---

## Cart Service

**What it owns:** Shopping carts, cart items, coupon/discount codes

**Migrated from old project:** `ICartService`, Cart, CartItem, Coupon entities

**Key operations:** AddItem, RemoveItem, UpdateQuantity, ApplyCoupon, GetCart, ClearCart, MergeGuestCart

**Sync HTTP calls (outbound):** `Catalog` — validate product exists and fetch current price when a client adds an item

**Local product snapshot:** Cart stores a copy of product name, price, and image synced via events. After the initial add, no further calls to Catalog are needed for reads.

**Subscribes to:** `ProductUpdated` (sync local snapshot), `InventoryUpdated` (flag out-of-stock items), `ProductDeleted` (remove from carts), `OrderCancelled` (restore cart if needed)

---

## Orders Service

**What it owns:** Orders, order items, payment records, shipping configuration

**Migrated from old project:** `IOrderService`, `IPaymentService`, `ITbcService`, `IBogService`, Order, OrderItem, Payment, ShippingConfiguration entities

**Order state machine:** `Pending` → `Confirmed` → `Shipped` → `Delivered` → `Cancelled`

**Payment providers:** BOG Bank, TBC Bank (same as old project)

**Sync HTTP calls (outbound):** `Cart` — read cart contents at checkout to build the order

**Publishes:** `OrderPlaced`, `OrderPaid`, `OrderCancelled`, `OrderShipped`, `OrderDelivered`

---

## Media Service

**What it owns:** Uploaded files and images, public URLs

**Migrated from old project:** `IPhotosRepository`, Photo entity, file upload logic

**Storage:** TBD — decision deferred. Options: Azure Blob Storage (Azurite for dev), AWS S3 (MinIO for dev), or self-hosted MinIO.

**Key operations:** UploadFile, DeleteFile, GetFileUrl

---

## CMS Service

**What it owns:** Pages, sliders, advertisements, site-wide settings

**Migrated from old project:** `IPageService`, `ISliderService`, `ISettingService`, Page, PageLanguage, Slider, SliderLanguage, Advertisement, AdvertisementLanguage, Setting entities

**Multi-language:** Pages, sliders, advertisements all support GE, EN, RU.

**Endpoints:** Public read endpoints (no auth) + admin write endpoints (admin role)

---

## Notifications Service

**What it owns:** Email sending, SMS sending, message templates

**Migrated from old project:** `IEmailSender`, `ISmsService`

**No public HTTP API** — fully event-driven. Subscribes to RabbitMQ events and sends notifications.

**Subscribes to:** `UserRegistered`, `PasswordResetRequested`, `OrderPlaced`, `OrderPaid`, `OrderCancelled`, `OrderShipped`

---

## Reporting Service

**What it owns:** Aggregate read models for admin reports

**Migrated from old project:** `IReportService`

**Low priority.** Reads data from other services (either via direct DB read replicas or event-sourced projections). Admin role required for all endpoints.

---

## Survey Service

**What it owns:** Survey questions, user answers

**Migrated from old project:** `ISurveyService`, Question, Answer entities

**Low priority.** Self-contained with minimal dependencies on other services.
