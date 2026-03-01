# Sprint 13 — Admin Desktop App: Framework Decision and Project Setup

**Roadmap phase:** Phase 9 (part 1)
**Depends on:** Sprint 12 complete (all backend services and Gateway fully operational)
**Blocker:** ADR-012 — C++ UI framework must be decided before this sprint begins (Qt is frontrunner; confirm or choose alternative and document in `docs/decisions.md`)

## Goal

Admin desktop application project is scaffolded with the chosen C++ framework, connected to the Gateway API with JWT authentication, and able to display a login screen and main shell window.

## Tasks

- [ ] Finalise ADR-012: confirm Qt as the C++ UI framework (or choose alternative) and update `docs/decisions.md` with the final decision and rationale
- [ ] Create `Clients/GoodMarket.Admin.Desktop/` directory structure
- [ ] Initialise Qt project: create `CMakeLists.txt` (or `.pro` file) for `GoodMarket.Admin.Desktop`
- [ ] Add project to `GoodMarket.sln` or maintain as a standalone CMake build (document the build process in `Clients/GoodMarket.Admin.Desktop/README.md`)
- [ ] Add Qt modules to `CMakeLists.txt`: `Core`, `Gui`, `Widgets`, `Network`
- [ ] Implement `ApiClient` class (`ApiClient.h` / `ApiClient.cpp`) wrapping `QNetworkAccessManager`:
  - `post(endpoint, body)` — async POST with JSON body
  - `get(endpoint)` — async GET
  - `put(endpoint, body)` — async PUT
  - `delete_(endpoint)` — async DELETE
  - Attaches `Authorization: Bearer {accessToken}` header to all requests
  - Configurable base URL from application settings (pointing to Gateway)
- [ ] Implement `AuthService` class (`AuthService.h` / `AuthService.cpp`):
  - `login(email, password)` — POST to `/api/auth/login`, store JWT access token and refresh token in `QSettings`
  - `refreshToken()` — POST to `/api/auth/refresh`, update stored tokens
  - `logout()` — POST to `/api/auth/logout`, clear `QSettings`
  - `isAuthenticated()` — return true if valid access token in `QSettings`
- [ ] Implement `LoginWindow` (`LoginWindow.h` / `LoginWindow.cpp` / `LoginWindow.ui`):
  - Email and password input fields
  - Login button triggers `AuthService::login()`
  - Shows error message on failed login (invalid credentials or network error)
  - On success, opens `MainWindow` and closes itself
- [ ] Implement `MainWindow` shell (`MainWindow.h` / `MainWindow.cpp` / `MainWindow.ui`):
  - Left sidebar navigation (Products, Categories, Brands, Orders, Users, CMS, Settings)
  - Central stacked widget area for page content
  - Top bar with logged-in user name and logout button
  - Logout button calls `AuthService::logout()` and returns to `LoginWindow`
- [ ] Implement token refresh timer — check token expiry and call `AuthService::refreshToken()` 60 seconds before expiry using `QTimer`
- [ ] Implement application settings dialog (`AppSettings.h`) — configure Gateway base URL, stored in `QSettings`
- [ ] Build and run the application: login screen appears, valid credentials open the main window shell

## Definition of Done

- [ ] Qt project builds with CMake without errors on the development machine
- [ ] Login window accepts credentials and calls Gateway `POST /api/auth/login`
- [ ] Successful login stores JWT in `QSettings` and opens the main window shell
- [ ] Failed login shows an error message (wrong credentials, network unreachable)
- [ ] Main window sidebar navigation is visible with all section links (pages are empty placeholders at this stage)
- [ ] Logout clears stored tokens and returns to the login screen
- [ ] Token refresh timer fires correctly before access token expires
- [ ] Integration tests passing (where applicable)
- [ ] Registered in Aspire AppHost (if new service)
- [ ] Reachable through Gateway (if new service with HTTP endpoints)
