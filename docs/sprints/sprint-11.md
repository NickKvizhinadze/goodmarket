# Sprint 11 — Next.js Storefront: Auth, Layout, Product and Category Pages

**Roadmap phase:** Phase 8 (part 1)
**Depends on:** Sprint 6 complete (Catalog read endpoints working); Sprint 2 complete (Identity login/refresh working); Sprint 10 complete (CMS sliders/pages available for homepage)
**Blocker:** None

## Goal

A Next.js App Router storefront with working authentication (login/register via JWT in HttpOnly cookies), a shared layout with navigation and header, SEO-optimised server-rendered product listing and category pages, and homepage CMS content integration.

## Tasks

- [ ] Initialise Next.js project: `npx create-next-app@latest goodmarket-storefront --typescript --app --tailwind --src-dir` inside `Clients/` directory, add to solution tracking
- [ ] Configure Next.js `next.config.ts` to proxy API calls to the Gateway base URL (via environment variable `NEXT_PUBLIC_API_BASE_URL`)
- [ ] Implement Next.js middleware (`src/middleware.ts`) to read `access_token` HttpOnly cookie and attach as `Authorization: Bearer` header on API requests; redirect unauthenticated users away from protected routes
- [ ] Implement auth API route handlers:
  - `POST /api/auth/login` — call Gateway `/api/auth/login`, set `access_token` and `refresh_token` HttpOnly cookies
  - `POST /api/auth/logout` — call Gateway `/api/auth/logout`, clear cookies
  - `POST /api/auth/refresh` — call Gateway `/api/auth/refresh`, rotate cookies
- [ ] Implement `AuthProvider` client component and `useAuth` hook for client-side auth state (read from cookie presence, not value)
- [ ] Build login page (`/login`) — form that calls `/api/auth/login` route handler
- [ ] Build register page (`/register`) — form that calls Gateway `POST /api/auth/register`
- [ ] Build root layout (`src/app/layout.tsx`) — includes header with navigation, cart icon with item count, user menu (login/logout/account links)
- [ ] Build `Header` server component — fetches CMS settings for site name/logo from Gateway `GET /api/settings`
- [ ] Build `NavigationMenu` component — fetches category tree from Gateway `GET /api/categories`, renders nested nav
- [ ] Build homepage (`src/app/page.tsx`) — server component fetching:
  - Sliders from Gateway `GET /api/sliders`
  - Featured products from Gateway `GET /api/products?featured=true`
  - Advertisements from Gateway `GET /api/advertisements`
- [ ] Build `SliderBanner` component for homepage sliders
- [ ] Build category listing page (`src/app/categories/[slug]/page.tsx`) — server component, SSR, fetches products filtered by category
- [ ] Build product listing page (`src/app/products/page.tsx`) — server component with filter sidebar (categories, brands, price range, specifications) and paginated product grid
- [ ] Build `ProductCard` component — displays product image, name (locale-aware), price, add-to-cart button
- [ ] Build product detail page (`src/app/products/[id]/page.tsx`) — SSR, full product details, specifications table, image gallery, add-to-cart form
- [ ] Add `generateMetadata` to product and category pages for SEO (title, description, Open Graph tags from product data)
- [ ] Add `generateStaticParams` to product detail pages for static generation of popular products
- [ ] Configure locale selection (GE/EN/RU) — store preference in cookie, pass as `locale` query param to Gateway

## Definition of Done

- [ ] `POST /api/auth/login` sets HttpOnly cookies; refreshed on subsequent requests via middleware
- [ ] Homepage loads server-rendered with sliders and featured products from CMS and Catalog
- [ ] `/categories/{slug}` and `/products` pages are server-rendered with correct meta tags (visible in page source)
- [ ] `/products/{id}` product detail page has full product information and correct Open Graph tags
- [ ] Navigation category tree is populated from the Catalog service
- [ ] All pages load without client-side API calls visible in browser network tab for initial render (SSR confirmed)
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
