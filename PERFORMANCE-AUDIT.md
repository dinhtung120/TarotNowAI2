# PERFORMANCE-AUDIT - TarotNow `/vi`

## Executive Summary
- Audit timestamp (UTC): `2026-04-29T18:30:17.976Z`
- Target: `https://www.tarotnow.xyz/vi`
- Baseline matrix: `Chromium` x `desktop/mobile` x `logged-out/logged-in-admin/logged-in-reader`
- Total pages benchmarked: `190`
- Total requests captured: `10,151`
- Severity gates:
  - `>35 requests/page` => `Critical`
  - `>25 requests/page` => `High`
  - request `>800ms` => `High`
  - request `>400ms` => `Medium`

### Headline Results
- Pages `>25 requests`: `190/190` (100%)
- Pages `>35 requests`: `172/190` (90.5%)
- Slow requests `>800ms`: `141`
- Slow requests `400-800ms`: `720`
- Pages with pending non-websocket requests: `69`

### Coverage Status
- Covered route families:
  - Home/Landing, Auth (`/login`, `/register`, `/forgot-password`, `/reset-password`, `/verify-email`)
  - Reading + reading history + reading session
  - Inventory, Gacha + history, Collection
  - Profile + tabs (`/profile`, `/profile/mfa`, `/profile/reader`)
  - Readers + reader detail, Reader apply
  - Chat/messages shell
  - Notifications, Leaderboard, Community
  - Quest/Mission (`/gamification`)
  - Wallet (`/wallet`, `deposit`, `deposit/history`, `withdraw`)
  - Admin surfaces (`/admin` + child pages)
- Dynamic coverage/runtime notes:
  - `reading.init.daily_1: blocked (400)`
  - `reading-history-detail: coverage-blocked (no history id found)`
  - `chat-room-detail: coverage-blocked (no conversation id found)`

## Detailed Metrics Table

### Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Total requests | Pending requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 9 | 36.8 | 2800 | 331 | 0 | yes |
| logged-in-admin | desktop | 48 | 71.4 | 3339 | 3426 | 127 | yes |
| logged-in-reader | desktop | 38 | 62.7 | 3201 | 2381 | 148 | yes |
| logged-out | mobile | 9 | 35.4 | 2858 | 319 | 0 | yes |
| logged-in-admin | mobile | 48 | 43.1 | 3182 | 2068 | 6 | yes |
| logged-in-reader | mobile | 38 | 42.8 | 2924 | 1626 | 4 | yes |

### Most Expensive Routes (by avg request volume)
| Route | Samples | Avg req/page | Max req | Avg nav (ms) | Max nav (ms) | Avg LCP (ms) | Avg TBT (ms) |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| `/vi/admin` | 2 | 358.5 | 681 | 12365 | 21965 | 1092 | 0.0 |
| `/vi/reading/session/31bfd0d0-fe5b-44b9-a81f-54a1cd5f3527` | 1 | 74.0 | 74 | 2701 | 2701 | 1392 | 362.0 |
| `/vi/reading/session/3d5a761c-c8d3-4ce1-9c6d-470373630aad` | 1 | 72.0 | 72 | 2969 | 2969 | 1468 | 17.0 |
| `/vi/community` | 4 | 70.3 | 79 | 3982 | 4715 | 1137 | 5.0 |
| `/vi/readers` | 4 | 68.3 | 77 | 2809 | 2986 | 1135 | 106.8 |
| `/vi/collection` | 4 | 66.8 | 80 | 3464 | 3872 | 1344 | 183.3 |
| `/vi/profile/reader` | 4 | 66.3 | 76 | 2888 | 3405 | 1155 | 36.8 |
| `/vi/wallet/withdraw` | 4 | 64.3 | 79 | 3257 | 4361 | 1364 | 89.0 |
| `/vi/gacha` | 4 | 60.8 | 77 | 3827 | 4878 | 1379 | 39.0 |
| `/vi/gacha/history` | 4 | 58.0 | 77 | 3625 | 4952 | 1206 | 88.5 |

### High Slow Requests (`>800ms`, sample)
| Scenario | Viewport | Route | Method | Duration (ms) | TTFB (ms) | Category | URL |
| --- | --- | --- | --- | ---: | ---: | --- | --- |
| logged-in-admin | mobile | `/vi/wallet/deposit/history` | GET | 6643 | 67.9 | static | `.../_next/static/chunks/14zfgnebl8n68.js` |
| logged-in-admin | mobile | `/vi/wallet/deposit/history` | GET | 6472 | 88.7 | static | `.../_next/static/chunks/08nnjyw~vjmez.js` |
| logged-in-admin | mobile | `/vi/wallet/deposit/history` | GET | 6421 | 331.0 | static | `.../themes/prismatic-royal.css` |
| logged-in-admin | mobile | `/vi/wallet/deposit/history` | GET | 6375 | 77.1 | static | `.../_next/static/chunks/04tx~cql46cw0.js` |
| logged-in-admin | mobile | `/vi/wallet/deposit/history` | GET | 6330 | 98.3 | static | `.../_next/static/chunks/15fy9wk7qbfwp.js` |
| logged-in-admin | desktop | `/vi/admin` | GET | 1328 | 327.0 | html | `https://www.tarotnow.xyz/vi/admin` |
| logged-in-reader | desktop | `/vi/wallet/withdraw` | GET | 1198 | 672.3 | html | `https://www.tarotnow.xyz/vi/wallet/withdraw` |
| logged-in-admin | desktop | `/vi/community` | GET | 1186 | 633.2 | html | `https://www.tarotnow.xyz/vi/community` |

### Duplicate Request Hotspots (non-telemetry)
| Request key | Duplicate count |
| --- | ---: |
| `GET https://www.tarotnow.xyz/vi` | 324 |
| `GET https://www.tarotnow.xyz/_next/static/media/7178...woff2` | 247 |
| `GET https://www.tarotnow.xyz/_next/static/media/caa3...woff2` | 247 |
| `GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css` | 247 |
| `GET https://www.tarotnow.xyz/_next/static/chunks/0fqgq6m2b-440.css` | 247 |
| `GET https://www.tarotnow.xyz/themes/prismatic-royal.css` | 247 |
| `GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js` | 247 |
| `GET https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js` | 247 |

### Manual Browser Use Verification (production)
- Artifact: `Frontend/test-results/benchmark/tarotnow-manual-browser-verify.json`
- Admin verification:
  - Login success.
  - Full route sweep pass for reading/inventory/gacha/collection/profile/chat/leaderboard/community/gamification and admin child pages.
  - Community interaction probe executed (comment toggle).
- Reader verification:
  - Logout path discovered at avatar menu.
  - Reader login validated from login form.
  - Core protected routes pass (`/reading`, `/inventory`, `/chat`, `/community`, `/gamification`).
  - Admin route role-gated redirect observed (`/vi/admin` -> `/vi`).
- Manual anomaly captured:
  - Repeated console `Minified React error #418` appeared during rapid interactive traversal.

## Major Issues Found

### Critical
1. Authenticated surfaces remain far above request budget
- Evidence: `logged-in-admin desktop` averages `71.4 req/page`; `logged-in-reader desktop` `62.7 req/page`.
- Impact: slow transitions, large transfer cost, unstable UX under weaker networks.

2. Severe admin route burst behavior on desktop
- Evidence: `/vi/admin` recorded `681` requests and ~`22s` navigation in baseline matrix.
- Impact: extreme overhead and likely redundant session/static reload loop.

3. Static bundle/asset latency spikes
- Evidence: many static chunks/css on `/wallet/deposit/history` exceeded `6s` in mobile-admin path.
- Impact: high LCP/TTI degradation and interaction stalls.

### High
1. Duplicate static chunk/css/font fetches across route transitions
- Evidence: core static assets repeated `~247` times each in one run.
- Impact: cache inefficiency and avoidable bandwidth.

2. Pending non-websocket requests on authenticated scenarios
- Evidence: `69` pages had pending carry-over requests.
- Impact: hidden contention + stale state risk.

3. Dynamic route discovery partially blocked by runtime data state
- Evidence: `reading.init` blocked and some detail IDs unavailable.
- Impact: limits deterministic coverage on dynamic detail flows.

### Medium
1. Console stability warning (`React #418`) in manual pass
- Impact: possible hydration/render mismatch in certain navigation sequences.

2. Realtime/polling invalidation previously too broad
- Impact: over-fetch pressure on nav/chat/notification domains.

### Low
1. Image host latency variance remains high on collection-heavy screens
- Impact: inconsistent perceived performance, especially mobile.

## Optimization Plan (Impact-Priority)

### P0 - Request/duplication reduction on authenticated shell
- Gate prefetch by intent + network profile.
- Restrict realtime invalidation to domain-scoped keys.
- Eliminate SSR/CSR key mismatch that forces redundant refetch.

### P1 - Auth/middleware and server fetch overhead
- Apply auth checks only on document navigations.
- Dedupe session snapshot reads request-scope.
- Parallelize independent server layout fetches.

### P2 - TanStack Query tuning
- Increase stale windows for stable resources.
- Normalize admin query-key contracts and targeted invalidation.
- Disable reconnect refetch where realtime signal already exists.

### P3 - Image and route strategy
- Conditional `unoptimized` instead of blanket bypass.
- Enforce `sizes` + lazy policy on cards/lists/avatars.
- Block prefetch for expensive, low-value routes (`/admin`, `/chat`, `/community`, `/notifications`, wallet-heavy paths).

### P4 - Benchmark reliability
- Isolate measurement by route on fresh `page` within same authenticated context (implemented in harness).

## Recommended Refactors

### Implemented in this cycle
1. Benchmark harness upgrade
- `Frontend/tests/tarotnow-navigation-benchmark.spec.ts`
- Added multi-source crawl, dynamic discovery, route-family reporting, interaction notes, and per-route page isolation.

2. Middleware / proxy optimization
- `Frontend/src/proxy.ts`
- Reduced auth gate overhead to protected document navigations only.

3. TanStack Query contract normalization
- `Frontend/src/features/admin/application/adminQueryKeys.ts` (new)
- Updated admin hooks + SSR prefetch runner:
  - `.../useAdminDashboard.ts`
  - `.../useAdminUsers.ts`
  - `.../useAdminUsersMutations.ts`
  - `.../useAdminDeposits.ts`
  - `.../useAdminReaderRequests.ts`
  - `.../useAdminReadings.ts`
  - `.../useAdminDisputes.ts`
  - `.../useAdminWithdrawals.ts`
  - `Frontend/src/shared/server/prefetch/runners/admin.ts`
- `Frontend/src/shared/lib/appQueryClient.ts` stale window tuning.

4. Server component/layout and auth dedupe
- `Frontend/src/shared/server/auth/cachedSessionSnapshot.ts` (new)
- Wired into session handshake + auth redirect + site layout:
  - `.../sessionHandshake.ts`
  - `.../redirectAuthenticatedAuthEntry.ts`
  - `Frontend/src/app/[locale]/(site)/layout.tsx`
- Parallelized server layout fetches in site/admin layouts.

5. Realtime/custom-hook tightening
- `Frontend/src/shared/infrastructure/navigation/normalizePathname.ts`
- `Frontend/src/shared/components/common/Navbar.tsx`
- `Frontend/src/shared/application/hooks/usePresenceConnection.registration.domainEvents.ts`
- `Frontend/src/shared/application/hooks/useChatUnreadNotifications.ts`

6. Notification over-fetch suppression
- `Frontend/src/features/notifications/application/useNotificationDropdown.ts`
- `Frontend/src/shared/components/common/NotificationDropdown.tsx`

7. Image + prefetch strategy hardening
- Navigation/prefetch policy:
  - `Frontend/src/shared/infrastructure/navigation/useOptimizedLink.tsx`
  - `.../optimizedLinkPrefetch.ts`
  - `.../useOptimizedNavigation.ts`
  - `.../prefetchPolicy.ts`
- Conditional image optimization and sizing updates across chat/community/profile/reader/reading surfaces.

## Deploy Preflight Status (workspace)
- Deploy is currently blocked by missing required access/secrets:
  - Missing env: `BE_PRIVATE_IP` (required by `deploy_fe.sh`).
  - Missing env: `DB_PRIVATE_IP` (required by `deploy_be.sh`).
  - `gh` not authenticated in workspace (`gh auth status` reports no login).
  - No local Docker GHCR auth configured (`~/.docker/config.json` has no auth entries).
- Result: production deploy/post-deploy benchmark cannot proceed until credentials are provided.

## Artifacts
- Raw JSON: `Frontend/test-results/benchmark/tarotnow-benchmark.json`
- Pages CSV: `Frontend/test-results/benchmark/tarotnow-benchmark-pages.csv`
- Requests CSV: `Frontend/test-results/benchmark/tarotnow-benchmark-requests.csv`
- Route map: `Frontend/test-results/benchmark/tarotnow-route-map.json`
- Auto reports:
  - `Frontend/test-results/benchmark/tarotnow-benchmark-report.md`
  - `Frontend/test-results/benchmark/tarotnow-benchmark-analysis.md`
- Manual verification:
  - `Frontend/test-results/benchmark/tarotnow-manual-browser-verify.json`
