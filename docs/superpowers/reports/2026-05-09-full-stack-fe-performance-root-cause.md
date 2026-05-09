# Full-Stack FE Performance Root-Cause Report

**Date:** 2026-05-09
**Scope:** Analysis only. No runtime source changed by this report.
**Primary artifacts:**
- `PERFORMANCE-AUDIT.md`
- `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json`
- `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark-hotspots.json`
- `/tmp/tarot-perf-evidence.json`

---

## 1. Executive Summary

TarotNow FE feels slow because several costs stack on first navigation, not because one endpoint is obviously broken.

Biggest root causes:

1. **Static chunk fan-out is broad.** Many protected routes load 24 JS/CSS chunk requests; some routes show 44 chunk-like loads. This dominates request count on community, inventory, gacha, reading, profile/wallet, reader/chat, and admin pages.
2. **Navigation time is often high while FCP/LCP stay acceptable.** Many routes paint in ~450-650ms and LCP in ~800-1400ms, but total navigation/load reaches ~2900-4100ms because late static chunks, telemetry, session, realtime, and media continue after visible paint.
3. **Image/media transfer is route-specific and heavy.** Collection pulls many `img.tarotnow.xyz` AVIF thumb images at ~170-325KB each, sometimes duplicated in same scenario. Community has duplicate `media.tarotnow.xyz/community/...webp` in evidence.
4. **Session/auth/realtime add cross-cutting startup work.** `/api/auth/session`, `/api/auth/session?mode=lite`, and `/api/v1/presence/negotiate` appear as protected-route startup costs. Session is security-critical; only dedupe/defer is safe, not aggressive caching.
5. **CDN/domain topology pollutes classification and adds connection work.** `img.tarotnow.xyz`, `media.tarotnow.xyz`, `static.cloudflareinsights.com`, and `www.tarotnow.xyz` are separate hosts. Benchmark marks some same-site asset hosts as `third-party`, so request count alone overstates external vendor cost but still reflects real DNS/TLS/connection/cache fragmentation.
6. **Backend/API latency exists but is not primary across all routes.** Slow HTML documents and some API/proxy paths exceed 800ms. Need backend traces before assigning blame to backend processing versus Next route handler/proxy/CDN/edge latency.

Severity by route family:

| Route family | Severity | Main cause |
|---|---:|---|
| Community / leaderboard / gamification | High | chunk fan-out + community media duplicate + optional realtime/session |
| Inventory / gacha / collection | High | chunk fan-out + image optimizer/CDN images + collection duplicate media |
| Reading/session/history | High | chunk fan-out + 2 startup API requests + auth/session context |
| Profile/wallet/notifications | High | chunk fan-out + route POST/server-action duplicates + profile mobile CLS |
| Reader/chat | Medium-high | chunk fan-out + realtime/session paths |
| Admin | Medium-high | chunk fan-out, fewer API/media costs |
| Home/auth/legal | Medium | static shell/chunks + occasional slow HTML, fewer app APIs |

---

## 2. Measurement Baseline

Artifact freshness:

| Artifact | Mode | Generated | Origin | Pages |
|---|---|---:|---|---:|
| `tarotnow-benchmark.json` | full-matrix | 2026-05-09T01:19:17.648Z | `https://www.tarotnow.xyz` | 170 |
| `tarotnow-benchmark-hotspots.json` | targeted-hotspots | 2026-05-08T16:07:29.424Z | `https://www.tarotnow.xyz` | 38 |

Important caveat: artifacts in current checkout are older than later discarded worktree production runs. Treat this report as current committed-artifact root cause, not proof of latest deployed state after discarded worktree experiments.

Baseline counts from full matrix:

| Metric | Value |
|---|---:|
| Total pages measured | 170 |
| Critical pages | 0 |
| High pages | 142 |
| Slow requests >800ms | 50 |
| Slow requests 400-800ms | 313 |

Top user-visible route/request cases:

| Route | Scenario | Viewport | Requests | Nav ms | API | Static | Third | Telemetry | FCP | LCP | CLS |
|---|---|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|
| `/vi/community` | admin | desktop | 35 | 4086 | 1 | 31 | 1 | 1 | 596 | 1176 | 0.004 |
| `/vi/community` | reader | mobile | 35 | 3831 | 1 | 31 | 1 | 1 | 504 | 1208 | 0.005 |
| `/vi/community` | reader | desktop | 35 | 3694 | 1 | 31 | 1 | 1 | 556 | 1100 | 0.004 |
| `/vi/community` | admin | mobile | 35 | 3667 | 1 | 31 | 1 | 1 | 484 | 996 | 0 |
| `/vi/gacha` | reader | mobile | 35 | 2977 | 0 | 33 | 0 | 1 | 488 | 932 | 0.007 |
| `/vi/inventory` | reader | mobile | 35 | 2942 | 0 | 33 | 0 | 1 | 528 | 908 | 0 |
| `/vi/inventory` | reader | desktop | 35 | 2905 | 0 | 33 | 0 | 1 | 572 | 1072 | 0.004 |
| `/vi/reading/session/...` | reader | desktop | 34 | 2973 | 2 | 30 | 0 | 1 | 568 | 976 | 0.004 |
| `/vi/profile` | reader | mobile | 33 | 3773 | 3 | 27 | 0 | 1 | 512 | 1436 | 0.089 |

Slowest individual requests show mixed owners:

| Route | Category | Duration | Transfer | URL pattern | Likely owner |
|---|---|---:|---:|---|---|
| `/vi/profile` | html | 1377ms | 33KB | page document | FE/Next/API/CDN; needs trace |
| `/vi` | static | 1306ms | 81KB | `_next/static/chunks/...js` | FE/CDN |
| `/vi` | static | 1304ms | 40KB | `_next/static/chunks/...js` | FE/CDN |
| `/vi/collection` | third-party | 1223ms | 156KB | `img.tarotnow.xyz/...avif` | CDN/media/data |
| `/vi/collection` | third-party | 1203ms | 211KB | `img.tarotnow.xyz/...avif` | CDN/media/data |
| `/vi` | html | 1173ms | 23KB | page document | FE/Next/CDN |
| `/vi/gacha` | static | 961ms | 18KB | `_next/image?url=media...icon...` | FE image policy/CDN |

Full-matrix versus hotspot artifacts:

- Full-matrix is newer in artifact metadata and covers 170 pages.
- Hotspot artifact is older and narrower at 38 pages.
- For root cause, full-matrix is stronger baseline. Hotspot is useful for repeated problematic routes but should not override newer full-matrix without timestamp check.

---

## 3. Route Family Diagnosis

### Home/auth/legal

- **Symptom:** Logged-out/home/legal pages average ~24.6-25.3 requests and ~2737-2893ms navigation. Some HTML and static chunks exceed 800ms.
- **Evidence:** home-auth-legal family: logged-out desktop 224 requests across 9 pages, avg 24.9 requests, avg nav 2893ms; logged-out mobile avg 24.6 requests, avg nav 2782ms.
- **Dominant categories:** static chunks + telemetry, little/no app API.
- **Root cause class:** FE shell/chunk loading + CDN/static delivery; not API-heavy.
- **Owner:** FE/CDN.
- **Fix direction:** reduce shared shell client code, audit route-level chunk boundaries, check `_next/static` cache/CDN headers, keep Cloudflare RUM outside critical path where possible.

### Community/leaderboard/gamification

- **Symptom:** `/vi/community` is consistently top offender at 35 requests and 3.7-4.1s navigation.
- **Evidence:** community route has 31 static requests, 1 API, 1 same-site media/third-party request, 1 telemetry; duplicate `GET https://media.tarotnow.xyz/community/2ef125e575c84b8d990e4c90ed64d5f3.webp` appears twice in admin desktop evidence.
- **Dominant categories:** static chunks, one API, media duplicate, telemetry.
- **Root cause class:** FE chunk fan-out + media/data duplication + cross-host media.
- **Owner:** FE/Data/CDN.
- **Fix direction:** inspect community feed data for repeated same media; if true duplicate render, dedupe component data; if content duplicate, leave FE alone and fix data/content. Split heavy community widgets if loaded above fold unnecessarily.

### Inventory/gacha/collection

- **Symptom:** Inventory/gacha routes hit 31-35 requests with 28-33 static requests. Collection has biggest transfer bytes due card thumbs.
- **Evidence:** `/vi/gacha` reader mobile 35 requests, 33 static. `/vi/inventory` reader mobile/desktop 35 requests, 33 static. Collection top transfer entries are `img.tarotnow.xyz` AVIF thumbs at ~173-326KB each, with repeated URLs.
- **Dominant categories:** static chunks, image CDN, occasional image optimizer path.
- **Root cause class:** FE chunk fan-out + media sizing/cache/data duplication + some Next Image optimizer usage for media icons.
- **Owner:** FE/CDN/Data.
- **Fix direction:** keep approved unoptimized bypass for CDN-hosted media where it avoids slow optimizer; inspect collection initial window and duplicate URLs; verify `sizes`/thumb variants match rendered size; add CDN header checks for `img.tarotnow.xyz`.

### Reading/session/history

- **Symptom:** Reading sessions average ~29.4-31.2 requests, nav ~2860-2922ms. Individual session pages carry 2 API calls plus 29-30 static requests.
- **Evidence:** `/vi/reading/session/...` reader desktop: 34 requests, 2 API, 30 static, 2973ms nav, FCP 568ms, LCP 976ms.
- **Dominant categories:** static chunks + 2 startup APIs + telemetry.
- **Root cause class:** route code split fan-out + auth/session/reading data startup.
- **Owner:** FE/Backend for API timing.
- **Fix direction:** profile reading data APIs separately; keep security/session no-store; defer non-critical panels below initial render.

### Profile/wallet/notifications

- **Symptom:** profile/wallet family avg ~28.4-28.9 requests and ~2825-2970ms nav. `/vi/profile` reader mobile shows high nav 3773ms and CLS 0.089.
- **Evidence:** `/vi/profile` reader mobile: 33 requests, 3 API, 27 static, FCP 512, LCP 1436, CLS 0.089; duplicates include `POST https://www.tarotnow.xyz/vi/profile` twice.
- **Dominant categories:** static chunks, route POST/server action, app APIs.
- **Root cause class:** FE route action/data duplication + layout shift + chunk fan-out.
- **Owner:** FE/Backend if POST maps to server action latency.
- **Fix direction:** inspect profile page server actions/loaders and mobile layout image/reserved-space. Verify duplicated POST is not benchmark form/action artifact before changing behavior.

### Reader/chat

- **Symptom:** reader/chat routes average ~28.0-28.3 requests and ~2851-2890ms nav.
- **Evidence:** family totals show 234 static requests across 9 pages per scenario and minimal API counts in several cases.
- **Dominant categories:** static chunks, telemetry, realtime/session code likely loaded even when request count does not show many APIs.
- **Root cause class:** shared chat/realtime client bundle + app shell.
- **Owner:** FE.
- **Fix direction:** lazy-load chat-specific realtime/message modules outside non-chat reader pages; avoid broad shared imports from chat public surfaces.

### Admin

- **Symptom:** admin pages average ~29.3-29.6 requests, ~2900ms nav, mostly static.
- **Evidence:** admin desktop: 293 requests across 10 pages, 273 static, 10 telemetry, 0 API in summary; `/vi/admin/gamification` 32 requests, 30 static.
- **Dominant categories:** static chunks.
- **Root cause class:** admin bundle/chunk fan-out.
- **Owner:** FE.
- **Fix direction:** separate heavy admin feature panels; avoid loading gamification/promotions/user-management code outside matching routes.

---

## 4. Browser Waterfall and Request Classification

True duplicate candidates:

| Evidence | Classification | Action |
|---|---|---|
| `POST /api/v1/presence/negotiate?negotiateVersion=1` twice on collection | likely true duplicate realtime negotiation or reconnection | inspect SignalR lifecycle/timer; dedupe connection init if mounted twice |
| `GET /images/collection/back-card.svg` five times | likely component-level repeated asset use; browser cache may make cost low | acceptable if cached; verify transfer/cache before optimizing |
| `img.tarotnow.xyz/light-god-50/...avif` duplicates on collection | likely duplicate cards/rendered thumbs or content/data issue | inspect collection initial data and rendered card list |
| `media.tarotnow.xyz/community/...webp` duplicate on community | possible duplicate post media or component duplicate render | inspect feed data for same URL before code change |
| `POST /vi/profile` and `POST /vi/gamification` twice | likely server action/form route POST or benchmark interaction artifact | inspect HAR/action context before assigning bug |
| duplicated fonts/css/chunks on profile/withdraw | likely navigation/document reload artifact or cache partitioning | inspect benchmark navigation flow and cache headers |

Request-count problems that are not equal:

- 24-33 static chunk requests are FE build/code-split/app-shell cost.
- 1 telemetry request per route is Cloudflare RUM overhead, not app feature waste.
- `img.tarotnow.xyz` and `media.tarotnow.xyz` are same product media hosts, not external vendors; still create extra host-level cost.
- `/api/auth/session` and `/api/v1/presence/negotiate` are cross-cutting startup work; auth is security-critical, presence is deferrable.
- HTML document >800ms is not enough to blame backend; it could be Next server render, auth middleware, CDN, cold edge, or backend proxy.

Waterfall interpretation:

- FCP/LCP are mostly acceptable relative to nav/load. User sees initial content but page remains busy.
- Static chunk fan-out and media transfer extend load completion and interaction readiness.
- Long tail requests make benchmark classify many routes as high even if first paint is not catastrophic.

---

## 5. Next.js and Frontend Architecture Root Causes

Relevant anchors:

- `Frontend/src/app/_shared/app-shell/common/AppQueryProvider.tsx:5` imports `PresenceProvider` and wraps app children.
- `Frontend/src/app/_shared/providers/PresenceProvider.tsx:12` mounts `usePresenceConnection` from shared provider.
- `Frontend/src/shared/navigation/normalizePathname.ts` contains `shouldEnableRealtimeForPath` and enables realtime for most non-auth/non-admin/non-legal paths.
- `Frontend/src/app/[locale]/layout.tsx:62` adds theme stylesheet link via `getThemeStylesheetHref(initialTheme)`.
- `Frontend/src/app/_shared/models/theme.ts:39` maps theme ID to `/themes/${themeId}.css`.

Root causes:

1. **Shared app shell pulls client providers broadly.** TanStack Query provider and PresenceProvider are global-ish. Even if requests are deferred, client code and effects compete with page startup.
2. **Route families share heavy feature surfaces.** Inventory/gacha/community/admin/reader routes show 24 chunks repeatedly. This suggests broad public exports/shared components are pulling more route code than needed per page.
3. **Theme CSS is separate global request.** `/themes/prismatic-royal.css` appears in duplicate evidence. Dynamic theme stylesheet is correct functionally, but it adds global CSS request and can duplicate under reload/cache-miss scenarios.
4. **Fonts/CSS/chunks duplicate under some navigation scenarios.** Evidence lists repeated WOFF2, CSS, and turbopack chunk URLs. This may be benchmark flow/cache issue, but still points to cache/header/navigation validation need.

Not proven:

- No evidence that React CPU/TBT is primary; TBT is 0 in many top routes.
- No evidence that hydration CPU dominates. Problem is more network/chunk/media/backend wait than main-thread blocking.

---

## 6. Auth, Session, and Realtime Root Causes

Relevant anchors:

- `Frontend/src/features/auth/shared/auth/clientSessionSnapshot.ts:136` exports `getClientSessionSnapshot`.
- `Frontend/src/app/api/auth/session/sessionRouteHandler.ts:23` defines full/lite session mode.
- `Frontend/src/app/api/auth/session/sessionRouteHandler.ts:133` handles lite mode.
- `Frontend/src/features/chat/shared/realtime/realtimeSessionGuard.ts:9` calls `getClientSessionSnapshot` before realtime.
- `Frontend/src/app/_shared/providers/PresenceProvider.tsx:12` enables presence connection per route policy.

Findings:

- Session requests are security-critical. They use cookies, token refresh, and auth state; do not cache aggressively or relax `no-store` casually.
- Lite session is cheaper than full when access token is valid, but can still refresh via refresh token. It may set cookies.
- Full session is superset of lite for authenticated user. Safe dedupe direction: reuse fresh/in-flight full snapshot for lite checks. Unsafe direction: long shared cache or stale auth acceptance.
- Presence negotiate is not needed for first content paint on most pages. It can be safely deferred more than auth, and should not block route render.
- Duplicate presence negotiate on collection suggests provider mount/reconnect/timer duplication or benchmark route lifetime overlap. Needs lifecycle trace before code change.

Ownership:

| Request | Safe action | Owner |
|---|---|---|
| `/api/auth/session` | dedupe in-flight/fresh same-mode or full→lite; no aggressive cache | FE/Auth |
| `/api/auth/session?mode=lite` | reuse full snapshot where fresh/in-flight | FE/Auth |
| `/api/v1/presence/negotiate` | defer, dedupe connection init, connect only where user-visible realtime needed | FE/Realtime/Backend |
| backend auth refresh path | trace server timing before optimizing | Backend/Auth |

---

## 7. Image and Media Root Causes

Relevant anchors:

- `Frontend/src/shared/http/assetUrl.ts` provides `shouldUseUnoptimizedImage` policy.
- `Frontend/src/features/community/post/components/post-card/PostCardContent.tsx` uses Next Image for post media with `unoptimized={shouldUseUnoptimizedImage(segment.url)}` and lazy loading.
- `Frontend/src/features/collection/cards/components/deck-card/CollectionDeckCardVisual.tsx` uses `shouldUseUnoptimizedImage(cardImageUrl)`.
- `Frontend/src/features/inventory/browse/InventoryItemCard.tsx:7` imports `shouldUseUnoptimizedImage` for item icons.

Findings:

- Collection transfer is the clearest media bottleneck. Multiple AVIF thumbs from `img.tarotnow.xyz` transfer ~173-326KB each and duplicate in evidence.
- Community duplicate media URL appears in `/vi/community` admin desktop evidence.
- `_next/image?url=https%3A%2F%2Fmedia.tarotnow.xyz%2Ficon...` still appears in slow request evidence for gacha/inventory. Some media icon paths still go through Next optimizer in current artifact.
- Direct CDN bypass is good when CDN already serves correctly sized/cacheable variants. Next optimizer is good only if it reduces bytes and cache is hot; here evidence shows optimizer/proxy can be slow for media icons.

Root cause classes:

| Media issue | Evidence | Likely root cause | Owner |
|---|---|---|---|
| collection large thumbs | `img.tarotnow.xyz` AVIF 173-326KB | thumb variant still too large or rendered window too big | CDN/Data/FE |
| collection duplicate images | repeated exact URLs | same cards rendered twice or data duplicate | FE/Data |
| community duplicate image | same WEBP twice | duplicate post media/data or duplicated component render | FE/Data |
| optimizer icon slow | `_next/image?url=media...icon...` 879-961ms | proxy/optimizer cache miss or policy gap | FE/CDN |

---

## 8. Backend/API and Proxy Root Causes

Relevant frontend-visible API/proxy files:

- `Frontend/src/app/api/auth/session/sessionRouteHandler.ts`
- `Frontend/src/app/api/auth/refresh/refreshRouteHandler.ts`
- `Frontend/src/app/api/_shared/forwardUpstreamJsonWithTimeout.ts`
- `Frontend/src/app/api/collection/route.ts`
- `Frontend/src/app/api/gacha/pools/route.ts`
- `Frontend/src/app/api/gacha/history/route.ts`
- `Frontend/src/app/api/chat/inbox-preview/route.ts`
- `Frontend/src/app/api/chat/unread-count/route.ts`

Findings:

- Many high-request routes have 0-3 API requests. Thus backend request count is not primary global cause.
- Some HTML documents exceed 800ms. That can hide server-side auth, SSR, route handler/proxy, backend, CDN, or cold cache cost.
- API count spikes on profile/reading routes, but not enough evidence from browser artifact alone to say backend processing is slow.
- `forwardUpstreamJsonWithTimeout` indicates several frontend APIs are proxies to backend. Browser timing cannot split browser→Next, Next→backend, backend processing, and backend→Next response.

Endpoints needing backend/infra traces:

| Path pattern | Why |
|---|---|
| page document `/vi/profile` | 1377ms HTML, high nav, CLS route |
| page document `/vi` | 1055-1173ms HTML on logged-out route |
| `/api/auth/session` and lite mode | protected route startup and possible refresh |
| `/api/v1/presence/negotiate` | duplicate negotiate + startup cost |
| profile/gamification POST route actions | duplicate POST evidence |
| gacha/inventory/collection APIs | route-family high nav and media coupling |

Missing evidence before backend blame:

- Server-Timing headers or trace IDs on Next route handlers.
- Backend application timing per proxied endpoint.
- CDN cache status (`CF-Cache-Status`, `Age`) for HTML/static/media.
- Cold versus warm route timings.
- Whether slow HTML includes backend fetches or only SSR/middleware.

---

## 9. CDN, Cache, and Domain Topology Root Causes

Hosts involved:

- `https://www.tarotnow.xyz` base origin in current artifacts.
- `https://img.tarotnow.xyz` collection/card images.
- `https://media.tarotnow.xyz` community/icons/media.
- `https://static.cloudflareinsights.com` Cloudflare RUM script.
- `/cdn-cgi/rum` telemetry beacon on site origin.

Findings:

1. **Same product, multiple hosts.** Benchmark `third-party` category includes `img.tarotnow.xyz`/`media.tarotnow.xyz`, but these are product media hosts. Classification is semantically wrong for vendor cost, but network cost remains real.
2. **Extra hostnames mean extra connection setup.** Browser may need DNS, TLS, HTTP/2/3 connection per host. This is visible on media-heavy pages.
3. **Cache/header suspicion exists.** Static chunks and theme CSS duplicate in evidence. Development server warning from prior verification also noted custom Cache-Control for `/_next/static/:path*`; production headers need explicit check.
4. **Telemetry is small but universal.** Every route has roughly one telemetry request. Not root cause alone, but it raises baseline request count and can extend waterfall tail.
5. **Base domain mismatch matters.** Current artifacts use `https://www.tarotnow.xyz`. Prior user requested production domains including apex `https://tarotnow.xyz`. If apex redirects or assets load from `www`, there may be extra redirect/connection/cache fragmentation. Need current dual-domain benchmark to confirm.

Suspicious headers to verify:

| Asset class | Expected check |
|---|---|
| `_next/static/chunks/*` | immutable long-cache, `CF-Cache-Status`, `Age`, compression |
| `/themes/*.css` | long-cache or versioned cache strategy |
| `img.tarotnow.xyz/*?variant=thumb` | cache hit, byte size, variant correctness |
| `media.tarotnow.xyz/community/*` | cache hit, compression/format, CORS/cache consistency |
| HTML documents | no unintended cache for auth pages; CDN status and server timing |

---

## 10. Root-Cause Tree

| Symptom | Evidence | Root cause | Owner | Confidence | Fix class | Expected impact | Risk |
|---|---|---|---|---|---|---|---|
| Many routes 28-35 requests | 142 high pages; top routes 31-35 requests | static chunk fan-out from app shell and route boundaries | FE | High | split/lazy-load route-specific code, trim shared client imports | High | Medium |
| Nav 3-4s while FCP/LCP OK | community nav 3667-4086ms, FCP 484-596ms | late chunks/media/telemetry/realtime extend load after paint | FE/CDN | High | defer non-critical work and reduce chunk count | High | Low-medium |
| Community high request count | `/vi/community` 35 req, 31 static, duplicate media | feature bundle fan-out + duplicate media/data | FE/Data/CDN | High | inspect feed data/render; split heavy widgets | Medium-high | Low |
| Collection heavy bytes | top transfer `img.tarotnow.xyz` 173-326KB each, duplicates | oversized thumb variant and/or duplicate rendered cards | Data/CDN/FE | High | right-size variants, reduce initial image window, dedupe data | High | Medium |
| Gacha/inventory slow icon optimizer | `_next/image?url=media...icon...` 879-961ms | optimizer/proxy path for CDN media icon | FE/CDN | Medium | expand justified unoptimized allowlist or fix optimizer cache | Medium | Low-medium |
| Protected routes session cost | auth session handler + client session snapshot usage | full/lite session checks can duplicate or refresh | FE/Auth/Backend | Medium | in-flight/fresh dedupe only; server timing | Medium | Medium if cache unsafe |
| Presence negotiate duplicated | collection duplicate `POST /api/v1/presence/negotiate` | multiple connection init/reconnect or benchmark lifetime overlap | FE/Realtime | Medium | lifecycle dedupe and defer | Medium | Low-medium |
| Profile mobile layout shift | `/vi/profile` mobile CLS 0.089 | image/layout reserved space or late content injection | FE | High | reserve dimensions, stabilize above-fold panels | Medium | Low |
| Slow HTML documents | profile 1377ms, home 1055-1173ms | unknown split among Next SSR/middleware/backend/CDN | FE/Backend/CDN | Medium | Server-Timing and trace IDs | Medium-high | Low measurement, medium fixes |
| Duplicated fonts/css/chunks | repeated WOFF2/CSS/chunks in duplicate evidence | cache/navigation flow/header issue | FE/CDN/Benchmark | Low-medium | header check, benchmark cache mode validation | Medium if true | Low |
| Third-party count misleading | media hosts classified third-party | benchmark category uses host mismatch | Benchmark/CDN | High | classify same-site media separately | Better decisions | Low |

---

## 11. Prioritized Fix Roadmap

### 1. High-impact, low-risk FE changes

1. **Measure per-route chunk graph.** Use Next build artifacts to map 24/44 chunk routes to imports. Target public exports and shared shell imports first.
2. **Dedupe auth full→lite session snapshot.** Keep no-store semantics; reuse only fresh/in-flight full snapshot for lite checks.
3. **Defer/dedupe presence connection.** Keep realtime enabled, but ensure one negotiate per route lifetime and connect after first route settles.
4. **Fix profile mobile CLS.** Reserve above-fold media/panel sizes on `/vi/profile` mobile.
5. **Classify media hosts separately in benchmark.** Split `same-site-media` from true third-party to avoid wrong owner decisions.

### 2. High-impact changes requiring backend/infra confirmation

1. **Add Server-Timing/trace IDs to HTML and proxied API routes.** Split browser→Next, Next→backend, backend processing.
2. **Audit CDN cache headers for `_next/static`, `/themes`, `img`, and `media`.** Confirm immutable static caching and media cache hits.
3. **Right-size collection thumb variants.** Requires CDN/data validation because byte size suggests thumb variant may still be too large.
4. **Investigate slow HTML documents.** Profile `/vi/profile`, `/vi`, `/vi/readers/...`, `/vi/reader/apply`, wallet/admin docs.

### 3. Medium-impact cleanup

1. **Inspect route action duplicate POSTs.** Validate whether `/vi/profile` and `/vi/gamification` duplicate POSTs are benchmark artifacts or true duplicate server actions.
2. **Reduce initial image windows.** Especially collection/community/feed grids.
3. **Review theme stylesheet caching.** `/themes/${theme}.css` should have stable cache policy if theme CSS changes are versioned/deploy-scoped.
4. **Lazy-load non-above-fold widgets.** Leaderboards, reward previews, admin tables, sidebars.

### 4. Not recommended now

1. **Do not aggressively cache auth/session responses.** Security and refresh-token correctness risk too high.
2. **Do not remove realtime globally.** User-visible presence/chat behavior may depend on it; defer/dedupe instead.
3. **Do not convert all images to unoptimized blindly.** Use allowlist/justification; only bypass optimizer where CDN variants are already right-sized and cacheable.
4. **Do not blame backend from browser timing alone.** Need traces before backend rewrite or DB/index work.

---

## 12. Verification Plan

After fixes, verify in this order:

1. **Targeted Playwright hotspot benchmark**
   - `/vi/community`
   - `/vi/inventory`
   - `/vi/gacha`
   - `/vi/collection`
   - `/vi/profile`
   - representative `/vi/reading/session/...`
   - one admin route

2. **Full production benchmark**
   - Run full matrix against `https://www.tarotnow.xyz`.
   - Run focused compare against `https://tarotnow.xyz` to catch redirect/domain topology.

3. **Build/lint/unit checks for touched modules**
   - Frontend lint gate from `Frontend/package.json`.
   - Targeted tests for auth session snapshot and presence connection if touched.
   - Component tests where route-level duplicate render is changed.

4. **API timing probe**
   - Add or capture Server-Timing for:
     - `/api/auth/session`
     - `/api/v1/presence/negotiate`
     - profile route POST/server actions
     - collection/gacha route APIs

5. **CDN/header check**
   - `_next/static/chunks/*`
   - `/themes/prismatic-royal.css`
   - `img.tarotnow.xyz/*variant=thumb`
   - `media.tarotnow.xyz/community/*`
   - HTML documents for auth-protected and public pages

Success criteria:

- Top protected routes drop below 30 requests where possible.
- Static chunk count per top route drops from 24/33 toward low 20s or less.
- Collection transfer bytes drop materially without visual regression.
- Duplicate presence negotiate disappears from collection.
- Profile mobile CLS drops below 0.05, ideally below 0.02.
- Slow >800ms request count decreases from 50 in full matrix.
- New benchmark separates `same-site-media` from true third-party.
