# API Gateway

## Technology

**YARP** (Yet Another Reverse Proxy) — Microsoft's reverse proxy library for .NET.

Project: `GoodMarket.Gateway` (.NET 10, registered in Aspire AppHost)

## Responsibilities

- Single entry point for all clients (Next.js, desktop, mobile)
- JWT validation — validates token, rejects invalid requests with 401
- Identity header forwarding — strips JWT, adds `X-User-Id` and `X-User-Roles` headers
- Rate limiting — per user (authenticated) or per IP (anonymous)
- Request routing to downstream microservices by domain concept

## Route Design

Routes use **domain-concept prefixes** — the internal service that owns the resource is not exposed to clients. This hides internal service boundaries and allows services to be split or merged without changing client-facing URLs.

### Route Map

| Client route | Downstream service | Notes |
|---|---|---|
| `/api/auth/**` | Identity service | Login, register, refresh, password reset |
| `/api/products/**` | Catalog service | Product listings, search, detail |
| `/api/categories/**` | Catalog service | Category tree |
| `/api/brands/**` | Catalog service | Brand listings |
| `/api/cart/**` | Cart service | Cart CRUD, coupon application |
| `/api/orders/**` | Orders service | Order creation, history, status |
| `/api/media/**` | Media service | File uploads |
| `/api/pages/**` | CMS service | Static pages |
| `/api/sliders/**` | CMS service | Homepage sliders |
| `/api/advertisements/**` | CMS service | Advertisements |
| `/api/settings/**` | CMS service | Site settings |
| `/api/surveys/**` | Survey service | Survey questions and answers |
| `/api/reports/**` | Reporting service | Admin reports (admin role required) |

### Versioning

API versioning is handled at the service level (already implemented in Identity). The gateway passes the version segment through transparently.

Example: `/api/v1/auth/login` → Identity service `/api/v1/auth/login`

## Authentication & Authorization at the Gateway

- **Public routes** (no JWT required): `/api/auth/login`, `/api/auth/register`, `/api/auth/refresh`, `/api/products/**` (read), `/api/categories/**`, `/api/brands/**`, `/api/pages/**`, `/api/sliders/**`, `/api/advertisements/**`
- **Authenticated routes** (valid JWT required): `/api/cart/**`, `/api/orders/**`, `/api/auth/logout`, profile endpoints
- **Admin routes** (JWT + `admin` role required): `/api/reports/**`, all admin write endpoints

## Rate Limiting

Applied at the gateway via .NET rate limiting middleware.

| Limit type | Rule |
|---|---|
| Authenticated users | By `X-User-Id` after token validation |
| Anonymous requests | By client IP address |

Specific rate limit values (requests per minute per endpoint group) to be configured during implementation.

## Identity Header Forwarding

After JWT validation the gateway strips the `Authorization` header and adds:

```
X-User-Id: <userId from JWT sub claim>
X-User-Roles: <comma-separated roles from JWT claims>
```

Downstream services read these headers directly. They do not run JWT validation middleware.
