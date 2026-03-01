# Sprint 12 — Next.js Storefront: Cart, Checkout, User Account

**Roadmap phase:** Phase 8 (part 2)
**Depends on:** Sprint 11 complete (Next.js auth and product pages working); Sprint 7 complete (Cart service); Sprint 9 complete (Orders payment flow)
**Blocker:** None

## Goal

Next.js storefront is feature-complete with a functional cart drawer, a multi-step checkout flow with BOG/TBC payment redirect, and a user account area showing order history and profile management.

## Tasks

- [ ] Implement cart state management using React Context + `useReducer` (`CartProvider`, `useCart` hook) — syncs with Gateway `GET /api/cart` on mount for authenticated users
- [ ] Implement `addToCart` action — calls Gateway `POST /api/cart/items`; updates local cart state optimistically
- [ ] Implement `removeFromCart` action — calls Gateway `DELETE /api/cart/items/{itemId}`
- [ ] Implement `updateQuantity` action — calls Gateway `PUT /api/cart/items/{itemId}`
- [ ] Build `CartDrawer` component — slide-in panel showing cart items, quantities, subtotal, coupon input, checkout button
- [ ] Build coupon code input — calls Gateway `POST /api/cart/coupon`, shows discount applied
- [ ] Build cart summary page (`/cart`) — full page cart view with item editing and proceed-to-checkout button
- [ ] Build checkout page (`/checkout`) — multi-step form:
  - Step 1: Shipping address form (name, address line, city, postal code, phone)
  - Step 2: Shipping method selection (fetch from Orders service or static config)
  - Step 3: Payment method selection (BOG / TBC)
  - Step 4: Order summary review
- [ ] Implement checkout submission — `POST /api/orders` to create order, then `POST /api/orders/{id}/pay?provider=...` to get payment redirect URL, redirect user to bank payment page
- [ ] Build payment return page (`/checkout/return`) — handles redirect back from bank, polls `GET /api/orders/{id}/payment` until status resolves, shows success or failure
- [ ] Build order confirmation page (`/orders/{id}/confirmation`) — shows placed order details
- [ ] Build user account layout (`/account/layout.tsx`) — sidebar with links: Orders, Profile, Addresses (requires auth guard in middleware)
- [ ] Build order history page (`/account/orders`) — fetches `GET /api/orders`, paginated list of orders with status badges
- [ ] Build order detail page (`/account/orders/{id}`) — full order details including items, payment status, shipping info
- [ ] Build profile page (`/account/profile`) — display user info, change password form (calls Gateway `POST /api/auth/password-reset/request`)
- [ ] Add guest cart merge call on login — after `POST /api/auth/login` succeeds, call `POST /api/cart/merge` with guest cart ID from cookie
- [ ] Add out-of-stock badge to `ProductCard` and cart items (read `isOutOfStock` from cart item)
- [ ] Register Next.js app in AppHost as a Node.js app resource (or as an external app pointing to `npm run dev` start command)

## Definition of Done

- [ ] `CartDrawer` opens and shows live cart data from the Cart service
- [ ] Checkout completes: order is created, user is redirected to BOG/TBC payment page
- [ ] `/checkout/return` correctly shows success when payment callback has been processed
- [ ] `/account/orders` lists user's orders with correct statuses
- [ ] Guest cart is merged into user cart automatically on login
- [ ] All account pages redirect to `/login` if accessed unauthenticated
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
