# Full-Stack FE Performance Root-Cause Analysis Design

**Goal:** Build a detailed root-cause analysis explaining why TarotNow frontend feels slow, tracing symptoms from browser metrics down to FE architecture, API behavior, CDN/cache, and deployment/domain effects.

**Scope:** Analysis only. No code changes in this phase. The output is a root-cause report/spec that separates FE, backend, CDN, infra, and data ownership.

**Primary evidence sources:**
- `PERFORMANCE-AUDIT.md`
- `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json`
- `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark-hotspots.json`
- Next.js build output and `.next` artifact shape when available
- Relevant FE source files under `Frontend/src/app`, `Frontend/src/features`, and `Frontend/src/shared`
- API/session/realtime route handlers and proxy paths in the frontend repo
- Git commits from the recent performance optimization series

---

## Analysis Questions

1. Which routes are slow by user-visible metrics, not just request count?
2. Which request-count problems are true FE waste versus benchmark classification artifacts?
3. Which costs come from static chunks, fonts, theme CSS, and hydration?
4. Which costs come from session/auth/realtime startup?
5. Which costs come from image delivery, CDN cache behavior, and duplicate media use?
6. Which costs likely originate from backend/API/proxy latency?
7. Which costs are caused by domain and CDN topology, especially `tarotnow.xyz`, `www.tarotnow.xyz`, `media.tarotnow.xyz`, and `img.tarotnow.xyz`?
8. Which fixes are high-impact and safe, and which require backend/infra measurement before implementation?

---

## Report Structure

### 1. Executive Summary

Summarize the true performance story in plain language:
- Whether the app is slow because of too many API calls, too many static chunks, image transfer, backend latency, cache misses, realtime startup, or multiple combined causes.
- Current severity by route family.
- Biggest root causes and expected fix classes.

### 2. Measurement Baseline

Extract and compare:
- Total pages measured.
- Critical and high route counts.
- Average request count by scenario and viewport.
- Slow requests above 800ms and 400ms.
- Top routes by request count.
- Top routes by navigation time.
- Top routes by transfer bytes.
- Differences between full-matrix and hotspot artifacts.

Flag stale artifacts explicitly when timestamps differ from latest deployed code.

### 3. Route Family Diagnosis

Group findings by route family:
- Home/auth/legal.
- Community/leaderboard/gamification.
- Inventory/gacha/collection.
- Reading/session/history.
- Profile/wallet/notifications.
- Reader/chat.
- Admin.

For each family, record:
- Symptom.
- Evidence.
- Dominant request categories.
- Root cause class.
- Owner.
- Fix direction.

### 4. Browser Waterfall and Request Classification

Identify:
- True duplicate API calls.
- Duplicate image/media requests.
- Waterfall caused by JS chunk loading.
- Requests categorized as third-party only because host differs from base origin.
- Telemetry/RUM impact.
- Persistent or delayed requests such as SignalR negotiation.

This section must avoid treating every request above 25 as equal. Static chunks, auth/session, image CDN, and backend API have different owners and fixes.

### 5. Next.js and Frontend Architecture Root Causes

Inspect:
- App Router route boundaries.
- Shared app shell providers.
- Query hydration boundaries.
- Client components that force broad hydration.
- Dense navigation/prefetch behavior.
- Chunk fan-out visible in benchmark waterfall.
- Fonts/theme CSS and global CSS loading.

Expected output:
- Which shared client code loads on many routes.
- Which route families pull unusually many static chunks.
- Which client-side startup tasks compete with first route render.

### 6. Auth, Session, and Realtime Root Causes

Inspect:
- `getClientSessionSnapshot` full/lite behavior.
- auth store/session bootstrap.
- `PresenceProvider` and SignalR connection timing.
- `/api/auth/session` and `/api/v1/presence/negotiate` appearance in route waterfalls.
- Whether session checks are required for each route or can be safely deduped/deferred.

Expected output:
- Which requests are security-critical and should not be cached aggressively.
- Which requests are safe to dedupe or move later.
- Which requests require backend latency inspection.

### 7. Image and Media Root Causes

Inspect:
- Next Image usage.
- `shouldUseUnoptimizedImage` policy.
- Media host behavior for `media.tarotnow.xyz` and `img.tarotnow.xyz`.
- `sizes`, `priority`, `loading`, and direct CDN bypasses.
- Duplicate media URLs in community/feed and collection.

Expected output:
- Which image requests are oversized.
- Which images are slow due optimizer/proxy path.
- Which duplicate media requests are data/content issues versus component bugs.

### 8. Backend/API and Proxy Root Causes

Inspect frontend-visible backend paths:
- Next route handlers under `Frontend/src/app/api`.
- Server actions wrapping backend calls.
- Auth refresh/session route behavior.
- Presence negotiation path.
- Business APIs appearing above 400ms/800ms.

Expected output:
- API endpoints needing backend traces.
- Whether slowness is in browser-to-Next, Next-to-backend, backend processing, or CDN/proxy.
- What evidence is missing before assigning backend blame.

### 9. CDN, Cache, and Domain Topology Root Causes

Inspect:
- Cache-Control and cacheability issues in benchmark artifacts.
- Static asset host mismatch: `tarotnow.xyz` base vs `www.tarotnow.xyz` asset loads.
- Cloudflare RUM and CDN behavior.
- Fonts/static chunks cache headers.
- Media CDN cache headers and transfer sizes.

Expected output:
- Which "third-party" rows are not external third-party code.
- Which same-site cross-host requests cost extra connection/cache work.
- Which cache headers are suspicious.

### 10. Root-Cause Tree

Create a table with columns:
- Symptom.
- Evidence.
- Root cause.
- Owner: FE / Backend / CDN / Infra / Data.
- Confidence: High / Medium / Low.
- Fix class.
- Expected impact.
- Risk.

### 11. Prioritized Fix Roadmap

Prioritize by impact and safety:
1. High-impact, low-risk FE changes.
2. High-impact changes requiring backend/infra confirmation.
3. Medium-impact cleanup.
4. Changes not recommended because risk exceeds expected gain.

### 12. Verification Plan

Define checks for follow-up implementation:
- Targeted Playwright hotspot benchmark.
- Full production benchmark after deploy.
- Build/lint/unit tests for touched modules.
- API timing probe for suspect endpoints.
- CDN/header check for static/media assets.

---

## Non-Goals

- Do not change code while producing this analysis.
- Do not hide uncertainty. Label missing evidence clearly.
- Do not blame backend or CDN without route-level evidence.
- Do not optimize finance/auth/session caching in unsafe ways.
- Do not remove realtime behavior; only propose safe defer/dedupe strategies.

---

## Success Criteria

The final report is successful when it explains:
- Why request count remains high across many routes.
- Why some routes have high navigation time despite acceptable FCP/LCP.
- Why community, inventory, gacha, collection, reading, profile, and admin families differ.
- Which bottlenecks are FE-owned versus backend/CDN/infra-owned.
- Which next optimizations are worth doing and which are not.
- What exact evidence should be gathered before risky full-stack changes.
