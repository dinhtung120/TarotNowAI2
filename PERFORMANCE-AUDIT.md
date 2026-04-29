# PERFORMANCE AUDIT - TarotNow `/vi`

## Executive Summary
- Audit date (UTC): `2026-04-29T10:42:32.529Z`
- Benchmark target: `https://www.tarotnow.xyz/vi`
- Matrix executed: `Chromium` x `desktop/mobile` x `logged-out/logged-in-admin/logged-in-reader`
- Total pages benchmarked: `204`
- Total requests captured: `12,226`
- Request severity thresholds:
  - `>35 requests/page`: `Critical`
  - `>25 requests/page`: `High`
  - request `>800ms`: `High`
  - request `>400ms`: `Medium`

### Headline Results
- Pages `>25 requests`: `204/204` (100%)
- Pages `>35 requests`: `194/204` (95.1%)
- Slow requests `>800ms`: `162`
- Slow requests `>400ms`: `940` (includes `>800ms`)
- Duplicate request records (`isDuplicate=true`): `8,097`
- Pages with pending requests: `123/204`

### Coverage Status
- Covered route families:
  - Landing/home, login/register/forgot/reset/verify
  - Reading, reading history
  - Inventory
  - Gacha + gacha history
  - Collection
  - Profile + tabs (`/profile`, `/profile/mfa`, `/profile/reader`)
  - Readers directory + reader detail pages
  - Chat/messages shell (`/chat`)
  - Leaderboard
  - Community
  - Quest/Mission (`/gamification`)
  - Wallet + deposit/withdraw/history
  - Notifications
  - Legal pages
- Dynamic coverage notes:
  - Reading session init flows (`daily_1`, `spread_3`, `spread_5`, `spread_10`) returned `401` in benchmark env.
  - `reading/session/[id]`, `reading/history/[id]`, `chat/[id]` were marked `coverage-blocked` when IDs were unavailable.
- Route mapping note:
  - Runtime does not expose `/vi/user/*`; protected routes are effectively under `/vi/*` with route groups.

## Detailed Metrics Table

### Scenario Summary
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Total requests | Pending requests | Login bootstrap |
| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |
| logged-out | desktop | 32 | 37.9 | 3005 | 1213 | 0 | yes |
| logged-in-admin | desktop | 35 | 69.8 | 3606 | 2444 | 113 | yes |
| logged-in-reader | desktop | 35 | 70.5 | 3064 | 2467 | 90 | yes |
| logged-out | mobile | 32 | 37.5 | 3020 | 1201 | 0 | yes |
| logged-in-admin | mobile | 35 | 69.8 | 3183 | 2444 | 81 | yes |
| logged-in-reader | mobile | 35 | 70.2 | 3067 | 2457 | 85 | yes |

### Most Expensive Routes (by request volume)
| Route | Pages sampled | Avg req/page | Max req | Avg nav (ms) | Max nav (ms) | Avg LCP (ms) | Avg TBT (ms) |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| `/vi/community` | 6 | 67.2 | 86 | 4793 | 10795 | 1661 | 43.8 |
| `/vi/reading/history` | 6 | 66.3 | 81 | 3623 | 4013 | 1591 | 63.3 |
| `/vi/profile/reader` | 6 | 66.2 | 82 | 3439 | 4027 | 1248 | 33.8 |
| `/vi/leaderboard` | 6 | 65.2 | 83 | 3812 | 8614 | 1382 | 14.8 |
| `/vi/collection` | 6 | 65.0 | 80 | 3540 | 3900 | 1789 | 23.8 |
| `/vi/reading` | 6 | 64.0 | 77 | 3752 | 4313 | 1327 | 29.7 |
| `/vi/inventory` | 6 | 64.0 | 77 | 3542 | 4022 | 1207 | 50.8 |
| `/vi/gacha` | 6 | 64.0 | 77 | 3528 | 3811 | 1308 | 43.3 |
| `/vi/gacha/history` | 6 | 64.0 | 77 | 3621 | 4051 | 1513 | 27.2 |
| `/vi/chat` | 6 | 61.5 | 81 | 3637 | 7528 | 1246 | 13.7 |

### Top Slow Requests (`>800ms`, sampled)
| Method | URL (normalized) | Count | Max (ms) | Avg (ms) | Type |
| --- | --- | ---: | ---: | ---: | --- |
| GET | `https://www.tarotnow.xyz/vi` | 8 | 2833 | 1281 | html |
| GET | `https://www.tarotnow.xyz/themes/prismatic-royal.css` | 5 | 3488 | 1678 | static |
| GET | `https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js` | 4 | 3805 | 3103 | static |
| GET | `https://www.tarotnow.xyz/_next/static/chunks/0~dckf69l39fe.js` | 4 | 3075 | 2226 | static |
| GET | `https://www.tarotnow.xyz/_next/static/chunks/0boqrfoz8awwu.js` | 4 | 3896 | 3306 | static |
| GET | `https://www.tarotnow.xyz/_next/static/chunks/17jnkfnr-ry28.js` | 4 | 4280 | 2477 | static |
| GET | `https://www.tarotnow.xyz/_next/static/chunks/12p7z~rbu83t1.js` | 4 | 4800 | 2509 | static |
| GET | `https://www.tarotnow.xyz/_next/static/chunks/0sw~iwigqoi1o.js` | 4 | 4274 | 2436 | static |
| GET | `https://www.tarotnow.xyz/_next/static/chunks/08nnjyw~vjmez.js` | 4 | 4273 | 2100 | static |
| GET | `https://www.tarotnow.xyz/vi/community` | 4 | 2204 | 1606 | html |

### Duplicate Request Hotspots (sample)
| Request key | Duplicate count |
| --- | ---: |
| `POST https://www.tarotnow.xyz/cdn-cgi/rum` | 562 |
| `GET https://www.tarotnow.xyz/vi/register` | 322 |
| `GET https://www.tarotnow.xyz/vi/login` | 249 |
| `GET https://www.tarotnow.xyz/_next/static/chunks/0kjazl8b8k9ly.css` | 232 |
| `GET https://www.tarotnow.xyz/_next/static/chunks/0fqgq6m2b-440.css` | 232 |
| `GET https://www.tarotnow.xyz/themes/prismatic-royal.css` | 232 |
| `GET https://www.tarotnow.xyz/_next/static/chunks/0dg6ntv_3jdd4.js` | 232 |
| `GET https://www.tarotnow.xyz/_next/static/chunks/14l~.r2kq13je.js` | 232 |
| `GET https://www.tarotnow.xyz/vi/readers` | 197 |
| `GET https://www.tarotnow.xyz/vi/reading` | 190 |

## Major Issues Found

### Critical
1. Request explosion across authenticated surfaces
- Evidence: authenticated scenarios average `~70 requests/page`; highest page `/vi/community` at `86` requests.
- Impact: poor navigation responsiveness, high data usage, cache churn.

2. Static chunk waterfall and asset latency
- Evidence: many `_next/static/chunks/*` requests repeatedly exceed `800ms`, max `~4.8s` per chunk.
- Impact: route transition delays, especially on leaderboard/community/chat clusters.

3. Repeated background fetches and duplicate route-level loads
- Evidence: high duplicate counts for `/vi/login`, `/vi/register`, `/vi/readers`, `/vi/reading` and static bundles.
- Impact: over-fetching and avoidable network pressure.

4. Pending network accumulation on logged-in routes
- Evidence: pending requests observed on `123` pages, concentrated in authenticated matrices.
- Impact: hidden background contention and potential UI race conditions.

### High
1. Dynamic reading coverage blocked by auth/API behavior
- Evidence: controlled reading-init probes returned `401` for spread types.
- Impact: prevents complete performance visibility of `reading/session/[id]` and related dynamic flows.

2. Route shell still carries heavy static payload even for low-interaction pages
- Evidence: authless and legal pages still carry `33-44` requests.
- Impact: first navigation cost remains high even outside core product flows.

### Medium
1. Over-eager prefetch and global realtime hooks were broad by default
- Impact: unnecessary background requests during simple browsing.
- Status: mitigated in this refactor set.

2. SSR/client query-key mismatch risk (notifications)
- Impact: hydration misses and extra client refetch.
- Status: fixed in this refactor set.

### Low
1. Widespread `unoptimized` image usage for avatars/cards
- Impact: misses Next image optimization path when source host is optimizable.
- Status: partially mitigated with conditional optimization strategy.

## Optimization Plan (Prioritized)

### P0 - Network load reduction on authenticated shell
- Tighten global realtime + unread polling mount conditions by route and role.
- Reduce default refetch pressure (`focus`, `mount`) for navbar/dropdown/unread hooks.
- Align server prefetch keys with client keys to avoid hydration misses.

### P1 - Route prefetch and middleware overhead
- Make Link prefetch opt-in by default for non-critical paths.
- Increase prefetch cooldown windows to reduce burst behavior.
- Cache static CSP directive segments in middleware/proxy path.

### P2 - Server component and hydration efficiency
- Parallelize independent server layout operations (`dehydrate` + `messages`) after auth handshake.
- Keep query stale windows coherent between server-hydrated data and client hooks.

### P3 - Image/asset policy hardening
- Replace blanket `unoptimized` with conditional strategy based on source host and URL type.
- Keep remote pattern allowlist explicit for trusted image hosts.

## Recommended Refactors

### Implemented in this cycle
- Benchmark harness rewrite (Playwright)
  - File: `Frontend/tests/tarotnow-navigation-benchmark.spec.ts`
  - Added: full network timeline, per-page/per-request schema, recursive crawl, dynamic route probes, 3 auth scenarios, desktop/mobile matrix, robust context-race guards.

- TanStack Query key alignment and refetch tuning
  - `Frontend/src/shared/server/prefetch/runners/user/notifications.ts`
  - `Frontend/src/features/notifications/application/useNotificationDropdown.ts`
  - `Frontend/src/shared/lib/appQueryClient.ts`
  - `Frontend/src/shared/application/hooks/useChatUnreadNotifications.ts`
  - `Frontend/src/features/chat/application/useChatInboxPage.ts`

- Layout/global-hook request suppression
  - `Frontend/src/shared/infrastructure/navigation/normalizePathname.ts`
  - `Frontend/src/shared/components/common/Navbar.tsx`

- Prefetch strategy and runtime policy throttling
  - `Frontend/src/i18n/routing.tsx`
  - `Frontend/src/shared/infrastructure/navigation/useOptimizedNavigation.ts`
  - `Frontend/src/shared/infrastructure/navigation/optimizedLinkPrefetch.ts`
  - `Frontend/src/shared/config/runtimePolicyFallbacks.ts`

- Middleware/proxy optimization
  - `Frontend/src/proxy.ts`
  - Cached CSP/static connect-src calculation to reduce per-request middleware compute cost.

- Server Components render path optimization
  - `Frontend/src/app/[locale]/(user)/layout.tsx`
  - Parallelized independent async operations after auth handshake.

- Image optimization guardrail
  - `Frontend/src/shared/infrastructure/http/assetUrl.ts`
  - `Frontend/src/shared/components/common/navbar/avatar-menu/NavbarAvatarTrigger.tsx`
  - `Frontend/src/features/reader/presentation/components/readers-directory/card/ReaderDirectoryCardAvatar.tsx`
  - `Frontend/src/features/home/presentation/components/featured-readers/FeaturedReaderAvatar.tsx`
  - `Frontend/next.config.ts`

## Before/After Comparison Note
- Current benchmark target is production (`https://www.tarotnow.xyz/vi`).
- Local refactors in this workspace are not yet deployed to that production host, so a true runtime delta on production cannot be measured in this turn.
- Baseline artifacts are available now and ready for post-deploy A/B rerun using the same matrix and thresholds.

## Benchmark Artifacts
- JSON raw: `Frontend/test-results/benchmark/tarotnow-benchmark.json`
- Pages CSV: `Frontend/test-results/benchmark/tarotnow-benchmark-pages.csv`
- Requests CSV: `Frontend/test-results/benchmark/tarotnow-benchmark-requests.csv`
- Route map: `Frontend/test-results/benchmark/tarotnow-route-map.json`
- Auto reports:
  - `Frontend/test-results/benchmark/tarotnow-benchmark-report.md`
  - `Frontend/test-results/benchmark/tarotnow-benchmark-analysis.md`

## Validation Performed
- Benchmark run: `npx playwright test tests/tarotnow-navigation-benchmark.spec.ts --project=chromium`
- Lint check on all modified files: passed.
- Full `tsc --noEmit` currently fails due pre-existing test typing issues unrelated to this patch set.
