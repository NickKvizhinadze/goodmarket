# Client Applications

## Overview

| App | Technology | Status |
|---|---|---|
| Customer Storefront | Next.js (TypeScript, App Router) | Planned |
| Admin Control Panel | Desktop — C++ (framework TBD) | Tech decision deferred |
| Mobile App | TBD — React Native or Flutter | Decision deferred |

All clients communicate exclusively through the API Gateway. No client talks directly to a microservice.

---

## Customer Storefront — Next.js

**Purpose:** Public-facing e-commerce site for customers to browse, search, and purchase products.

**Technology:**
- Next.js (App Router, TypeScript)
- Server-side rendering (SSR) for product/category pages — critical for SEO
- Next.js middleware handles JWT storage (HttpOnly cookies) and transparent token refresh

**Key areas:**
- Homepage (sliders, featured products, advertisements)
- Product catalogue (category/brand/specification filtering, search, pagination)
- Product detail page
- Cart
- Checkout + payment (BOG / TBC Bank redirect flows)
- User account: order history, profile, addresses
- Static pages (CMS-driven)
- Multi-language support: Georgian, English, Russian

**Auth:** JWT in HttpOnly cookies. Middleware refreshes token before expiry automatically.

---

## Admin Control Panel — C++ Desktop App

**Purpose:** Internal admin tool for managing the platform — products, orders, users, CMS content, reports.

**Technology:** C++ (framework decision deferred — see options below)

**Why desktop:**
- Admin tool for internal use only — no need for a web deployment
- Opportunity to learn C++
- Native performance for data-heavy operations (product bulk management, reports)

**Planned framework options:**

| Framework | Notes |
|---|---|
| **Qt** | Frontrunner — most mature C++ UI framework, cross-platform (Windows/macOS/Linux), large ecosystem, excellent documentation for learning. `Qt Widgets` for desktop-style UI. |
| wxWidgets | Lighter than Qt, uses native OS controls for a more native look |
| Dear ImGui | Immediate-mode rendering, simpler API, popular in tools/internal apps. Less polished but fast to build with. |
| Win32 / MFC | Windows-only, very low level, steep learning curve — not recommended |

**Decision:** Qt is the likely choice. Decision to be finalized when development of the admin app begins.

**Admin areas (mirrors old monolith ControlPanel):**
- Products management (CRUD, inventory, bulk import)
- Categories management
- Brands management
- Specifications management
- Orders management and status updates
- Users management
- CMS: Pages, Sliders, Advertisements, Settings
- Reports and analytics
- Survey management

**Auth:** JWT stored in OS credential store or encrypted local file. App handles refresh token flow before each request.

---

## Mobile App — TBD

**Purpose:** Customer-facing mobile app for iOS and Android.

**Technology decision deferred.** Options:

| Option | Notes |
|---|---|
| **React Native** | Shares ecosystem/mindset with Next.js. Code sharing potential with the web frontend (hooks, API clients, TypeScript types). Larger community for e-commerce use cases. |
| **Flutter** | Dart language, excellent performance, truly single codebase for iOS + Android. Strong UI consistency across platforms. |

**Decision criteria when ready:**
- If team already knows React/TypeScript → React Native
- If starting fresh and want best performance/consistency → Flutter

**Planned features:**
- Product browsing and search
- Product detail
- Cart and checkout
- Order tracking
- User account

**Auth:** JWT in secure device storage (iOS Keychain / Android Keystore). Refresh token flow handled client-side.

---

## What Was Removed from Scope

- **Seller/Vendor Portal** — removed. Admin control panel handles all internal management. No external seller self-service needed.
