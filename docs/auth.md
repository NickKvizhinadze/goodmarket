# Authentication & Authorization

## Strategy Overview

GoodMarket uses a custom JWT-based auth system built inside the Identity microservice. There is no external identity provider (no Keycloak, no Duende IdentityServer) — full control over the token lifecycle.

## Token Model

| Token | Purpose | Storage |
|---|---|---|
| Access token (JWT) | Authenticate API requests, short-lived | Per client (see below) |
| Refresh token | Obtain new access tokens without re-login, long-lived | Per client (see below) |

Access tokens are short-lived (e.g. 15 minutes). Refresh tokens are rotated on use — each refresh issues a new refresh token and invalidates the old one.

## Identity Service Responsibilities

- User registration
- Login (issues access token + refresh token)
- Token refresh
- Logout / token revocation
- Email confirmation
- Password reset (OTP-based)
- Google OAuth2 callback — after Google auth, we issue our own JWT (we do not use Google tokens downstream)

## Gateway Validation

JWT validation happens **once at the API Gateway (YARP)**. Downstream microservices do not validate JWTs themselves.

**Flow:**
```
Client → [JWT in request] → Gateway
                              │
                        Validate JWT
                              │
                    ┌─────────┴──────────┐
                    │ Invalid            │ Valid
                    ▼                   ▼
                401 Unauthorized   Strip JWT
                                   Add headers:
                                   X-User-Id: {userId}
                                   X-User-Roles: {roles}
                                        │
                                        ▼
                                  Downstream service
                                  (reads headers, trusts gateway)
```

Downstream services read `X-User-Id` and `X-User-Roles` headers. They do not need JWT middleware — they trust the internal network (all traffic is routed through Aspire's internal network and the gateway).

## Social Logins

Google OAuth2 is supported (minimum). Flow:

1. Client redirects user to Google
2. Google redirects back to Identity service callback endpoint
3. Identity service verifies Google token, finds or creates local user
4. Identity service issues its own JWT (access + refresh)
5. Client receives our JWT — downstream is identical to email/password login

Additional providers (Facebook, Apple, etc.) can be added to the Identity service later without affecting any other service.

## Per-Client Token Handling

### Next.js (Customer Storefront)
- JWT stored in **HttpOnly cookies** — not accessible to JavaScript, protected from XSS
- Next.js middleware handles token refresh transparently
- On logout: cookies cleared server-side

### Desktop App (C++ Admin)
- JWT stored **locally on disk** (encrypted, OS credential store where possible)
- App implements refresh token flow — checks expiry before each request, refreshes if needed
- On logout: local token deleted

### Mobile App (TBD)
- Same pattern as desktop — JWT stored in secure device storage
- Refresh token flow handled by the client

## Authorization Model

Role-based access control (RBAC) via JWT claims.

| Role | Access |
|---|---|
| `customer` | Storefront, own orders, own profile |
| `admin` | All admin endpoints via desktop app |

Roles are included in the JWT claims and forwarded by the gateway as the `X-User-Roles` header. Individual services enforce role requirements on their endpoints.
