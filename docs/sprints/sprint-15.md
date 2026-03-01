# Sprint 15 — Mobile App: Decision, Foundation, and Core Screens

**Roadmap phase:** Phase 10
**Depends on:** Sprint 12 complete (all backend APIs stable and tested)
**Blocker:** ADR-013 — React Native vs Flutter decision must be made before this sprint begins; document the final decision in `docs/decisions.md`

## Goal

Mobile app project is bootstrapped with the chosen framework, authenticated via the Gateway JWT flow, and has working screens for browsing products, categories, and viewing a product detail page.

## Tasks

### Framework Decision and Project Setup

- [ ] Finalise ADR-013: choose React Native or Flutter and update `docs/decisions.md` with the decision and rationale
- [ ] Bootstrap the mobile project inside `Clients/GoodMarket.Mobile/`:
  - If React Native: `npx react-native@latest init GoodMarketMobile --template react-native-template-typescript`
  - If Flutter: `flutter create goodmarket_mobile`
- [ ] Configure API base URL as an environment variable pointing to the Gateway
- [ ] Install HTTP client library:
  - React Native: `axios` or `fetch` with interceptors
  - Flutter: `dio` package
- [ ] Implement `AuthService` (mobile) — `login(email, password)`, `refreshToken()`, `logout()` — store JWT in device secure storage (`react-native-keychain` or `flutter_secure_storage`)
- [ ] Implement token refresh interceptor — automatically refresh access token when a 401 is received

### Authentication Screens

- [ ] Build `LoginScreen` — email/password form, calls `AuthService.login()`, navigates to home on success
- [ ] Build `RegisterScreen` — registration form, calls Gateway `POST /api/auth/register`
- [ ] Implement navigation guard — redirect unauthenticated users to `LoginScreen` when accessing protected routes

### Navigation and Layout

- [ ] Set up bottom tab navigation with tabs: Home, Categories, Search, Cart, Account
- [ ] Implement app header component with site name/logo

### Product Browsing Screens

- [ ] Build `HomeScreen` — displays sliders (from `GET /api/sliders`) and featured products (from `GET /api/products?featured=true`) in horizontally scrollable rows
- [ ] Build `CategoriesScreen` — displays category tree from `GET /api/categories` as a scrollable list with category icons/images
- [ ] Build `ProductListScreen` — paginated product grid with filter support (category, brand, price range); fetches from `GET /api/products`
- [ ] Build `ProductDetailScreen` — full product info, image carousel, specifications list, in-stock status, Add to Cart button
- [ ] Build `SearchScreen` — search bar calling `GET /api/products/search?q=...`, results displayed as product list
- [ ] Build `ProductCard` mobile component — image, name, price, add-to-cart shortcut

### Cart Screen (Basic)

- [ ] Build `CartScreen` — list of cart items with quantities, totals, and a Proceed to Checkout button
- [ ] Implement `addToCart` action — calls `POST /api/cart/items`
- [ ] Implement `removeFromCart` action — calls `DELETE /api/cart/items/{itemId}`

### Account Screen (Basic)

- [ ] Build `AccountScreen` — shows logged-in user email, links to Order History and Logout
- [ ] Build `OrderHistoryScreen` — paginated list of orders with status badges (from `GET /api/orders`)

## Definition of Done

- [ ] App launches and shows `LoginScreen`; valid credentials navigate to home
- [ ] `HomeScreen` displays real sliders and featured products from backend
- [ ] `ProductListScreen` loads paginated products and supports category filtering
- [ ] `ProductDetailScreen` shows correct product info including price and specifications
- [ ] `CartScreen` shows live cart contents; add/remove item persists to Cart service
- [ ] `OrderHistoryScreen` shows user's past orders
- [ ] Token refresh works silently — expired access token is refreshed without user action
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
