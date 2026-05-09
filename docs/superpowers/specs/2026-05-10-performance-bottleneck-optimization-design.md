# Performance Bottleneck Optimization Design

**Goal:** Fix and optimize bottlenecks identified by the two current performance reports while separating FE-owned fixes from backend, CDN, infra, and data work.

**Source reports:**
- `PERFORMANCE-AUDIT.md`
- `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`

**Scope:** Design only. Runtime source changes happen in a later implementation plan.

---

## Problem Summary

The app feels slow because several costs stack during first navigation:

- Many protected and semi-protected routes load 28-35 requests.
- Static chunk fan-out dominates most route families.
- Navigation/load often reaches ~2900-4100ms while FCP/LCP are much lower, meaning visible content appears before late chunks/media/telemetry/realtime finish.
- Collection and community have media-heavy bottlenecks and duplicate media candidates.
- Profile mobile has measurable CLS regression.
- Auth/session and realtime startup are cross-cutting costs, but auth must stay security-safe.
- Browser timing cannot prove whether slow HTML/API cases come from Next, backend, CDN, proxy, or edge without extra timing evidence.
- Benchmark classification currently treats same-product media hosts as `third-party`, which blurs ownership.

---

## Design Principles

1. **Measure before risky changes.** Browser evidence can identify symptoms, but backend/CDN changes need Server-Timing, trace IDs, or header evidence.
2. **Apply safe FE wins early.** Chunk fan-out, CLS, duplicate render, and measured media windows are FE-owned and can be optimized behind targeted verification.
3. **Preserve auth and realtime semantics.** Session remains conservative; realtime is deferred/deduped, not removed globally.
4. **Do not optimize every high request count equally.** Static chunks, telemetry, media, auth, realtime, and API each have different owners and fixes.
5. **Keep changes staged.** Each fix group gets targeted benchmark verification before broader production benchmark.

---

## Architecture

### Layer 1: Measurement and Ownership Classification

Upgrade benchmark/reporting so each request can be assigned to the right owner.

Required classifications:

| Category | Definition | Owner |
|---|---|---|
| `html` | same-origin document request | FE / Next / CDN / Backend if SSR calls backend |
| `api` | same-origin `/api/*` and proxied backend paths | FE / Backend |
| `static` | same-origin scripts, styles, fonts, local images | FE / CDN |
| `same-site-media` | `img.tarotnow.xyz`, `media.tarotnow.xyz` | CDN / Data / FE |
| `telemetry` | Cloudflare RUM script/beacon | Infra / External telemetry |
| `websocket` | realtime transport | FE / Backend realtime |
| `third-party` | external vendor hosts not controlled by TarotNow | Vendor / Infra |

Measurement outputs should include:

- route family summary by category;
- duplicate request groups;
- slow request groups >400ms and >800ms;
- transfer-heavy requests;
- cache/header issues;
- chunk count per route;
- top route owners and recommended next action.

### Layer 2: Safe FE Optimization

Apply optimizations with low product/security risk:

1. **Chunk fan-out reduction**
   - Use build artifacts and benchmark chunk lists to identify repeated 24/44 chunk routes.
   - Trim broad shared imports from app shell and feature public exports.
   - Lazy-load non-above-fold widgets in community, inventory/gacha, admin, profile/wallet, and reader/chat routes.
   - Preserve route architecture rules: app routes import feature entry points through established public exports where available.

2. **Profile mobile CLS fix**
   - Stabilize above-fold profile layout.
   - Reserve dimensions for avatars/media/cards/panels that shift.
   - Keep localization and accessibility behavior unchanged.

3. **Media duplicate and transfer reduction**
   - Inspect community and collection data/render paths before changing code.
   - If duplicate URLs are caused by duplicate render, dedupe at FE mapping boundary.
   - If duplicate URLs are valid content/data, do not hide them in FE; mark Data/CDN owner.
   - Reduce initial media window where product UX allows.
   - Keep `shouldUseUnoptimizedImage` allowlist-only; no blanket `unoptimized`.

4. **Auth/session verification**
   - Current code already reuses fresh/in-flight full session snapshots for lite checks.
   - Verify production benchmark confirms reduced duplicate session calls.
   - Do not extend auth cache TTLs or relax `no-store`.

5. **Presence realtime verification and hardening**
   - Current code delays presence startup by 10 seconds.
   - Verify negotiate leaves the initial route benchmark window.
   - If duplicate negotiate remains, harden lifecycle so one enabled provider route lifetime creates at most one active connection attempt.
   - Do not remove realtime globally.

### Layer 3: Evidence-Gated Backend/CDN/Infra Work

Add timing/header evidence before changing backend, proxy, or CDN behavior.

1. **Server-Timing / trace capture**
   - Add or capture timing for slow HTML documents and proxied APIs.
   - Split browser→Next, Next processing, Next→backend, backend processing, and response serialization where feasible.

2. **CDN/header audit**
   - Check `_next/static/chunks/*` for immutable long caching.
   - Check `/themes/*.css` for cache strategy.
   - Check `img.tarotnow.xyz` and `media.tarotnow.xyz` for cache hits, byte size, and variant correctness.
   - Check HTML documents for correct no-cache behavior where auth-sensitive.

3. **Media variant validation**
   - Collection thumb variants currently transfer ~173-326KB each in evidence.
   - Verify whether `variant=thumb` is truly thumb-sized and cacheable.
   - If too large, optimize media generation/CDN variant, not FE rendering alone.

---

## Component Plan

### Benchmark and Audit Components

- `Frontend/tests/tarotnow-navigation-benchmark.spec.ts`
  - Update classification for same-site media hosts.
  - Preserve existing telemetry classification.
  - Keep production benchmark safety guard.

- `Frontend/scripts/generate-performance-audit.mjs`
  - Add same-site media summary.
  - Add owner-based remediation sections.
  - Keep detailed metrics table stable enough for comparison.

- `PERFORMANCE-AUDIT.md`
  - Regenerated after benchmark runs.
  - Not hand-edited except if report script is unavailable.

### App Shell and Chunk Components

- `Frontend/src/app/_shared/app-shell/common/AppQueryProvider.tsx`
- `Frontend/src/app/_shared/providers/PresenceProvider.tsx`
- `Frontend/src/app/[locale]/**`
- `Frontend/src/features/**/public.ts`

Design intent:

- Keep shared providers only for truly global behavior.
- Move route-specific heavy modules behind dynamic/lazy boundaries.
- Avoid loading chat/realtime/community/admin-specific code on unrelated routes.

### Auth and Realtime Components

- `Frontend/src/features/auth/shared/auth/clientSessionSnapshot.ts`
- `Frontend/src/app/_shared/hooks/usePresenceConnection.ts`
- `Frontend/src/features/chat/shared/realtime/realtimeSessionGuard.ts`

Design intent:

- Keep full→lite session dedupe.
- Keep short-lived/no-store session behavior.
- Ensure presence startup is deferred and one connection attempt is active per mounted provider route lifetime.

### Media and Profile Components

- `Frontend/src/shared/http/assetUrl.ts`
- community post media components under `Frontend/src/features/community/**`
- collection card components under `Frontend/src/features/collection/**`
- profile components under `Frontend/src/features/profile/**`

Design intent:

- Use existing image policy path.
- Fix only proven duplicate render bugs.
- Reduce initial media load where product still shows enough useful content.
- Stabilize profile mobile layout above fold.

### Proxy/API Evidence Components

- `Frontend/src/app/api/_shared/forwardUpstreamJsonWithTimeout.ts`
- selected `Frontend/src/app/api/**/route.ts`

Design intent:

- Add measurement support only where needed to separate frontend proxy time from backend time.
- Avoid backend blame without traces.

---

## Staged Roadmap

### Stage 1: Measurement Corrections

1. Add `same-site-media` request category.
2. Regenerate audit report.
3. Add route-family owner summary.
4. Build route chunk graph from current build artifacts or benchmark request list.

Success criteria:

- `img.tarotnow.xyz` and `media.tarotnow.xyz` no longer counted as external third-party.
- Report lists top FE/static, same-site-media, telemetry, API, and HTML suspects separately.
- No runtime behavior changes.

### Stage 2: FE Safe Wins

1. Fix profile mobile CLS.
2. Verify auth session dedupe under production benchmark.
3. Verify and harden presence deferred negotiation.
4. Reduce or lazy-load non-critical widgets on worst chunk fan-out routes.
5. Inspect and fix proven FE duplicate media render bugs.

Success criteria:

- `/vi/profile` mobile CLS <0.05, target <0.02.
- Duplicate presence negotiate removed from collection route evidence.
- Top hotspot routes drop request count or static chunk count without visual regression.
- No auth/security behavior relaxed.

### Stage 3: Media/CDN and Backend Evidence

1. Capture CDN headers for static, theme, media, and HTML asset classes.
2. Capture Server-Timing/trace for slow HTML/API paths.
3. Decide whether media variant generation/CDN cache config needs changes.
4. Decide whether backend/API optimization is needed for slow document/proxy paths.

Success criteria:

- Each backend/CDN change has trace/header evidence.
- Collection media bytes materially drop if variant issue is confirmed.
- Slow >800ms requests decrease from current baseline.

### Stage 4: Full Verification

1. Run targeted hotspot benchmark.
2. Run full production benchmark.
3. Compare against current report baseline.
4. Regenerate `PERFORMANCE-AUDIT.md`.

Success criteria:

- Top protected routes move below 30 requests where feasible.
- Static chunk count on worst routes moves down from 24/33 toward low 20s or less.
- Slow >800ms request count decreases.
- No failed/pending requests introduced.
- Browser spot checks pass for changed UI flows.

---

## Non-Goals

- Do not remove realtime globally.
- Do not aggressively cache auth/session responses.
- Do not do blanket `unoptimized` for all images.
- Do not make backend/CDN config changes from browser timing alone.
- Do not refactor unrelated frontend architecture outside measured performance bottlenecks.
- Do not optimize benchmark scores by hiding real user work.

---

## Risks

| Risk | Mitigation |
|---|---|
| Auth regression from caching | No TTL expansion; only verify current dedupe and keep no-store |
| Realtime feature regression | Defer/dedupe only; targeted tests and route checks |
| Wrong owner assignment | Separate same-site media and add Server-Timing/header evidence |
| Visual regressions from lazy loading | Browser spot checks on changed routes |
| Chunk split increases waterfalls | Compare chunk count and nav metrics after each stage |
| Media dedupe hides valid content | Only dedupe after inspecting route data/render path |

---

## Verification Matrix

| Change type | Required checks |
|---|---|
| Benchmark/report classification | targeted benchmark test or report generation; inspect `PERFORMANCE-AUDIT.md` diff |
| Auth session logic | targeted auth session tests; production benchmark session request check |
| Presence realtime | targeted presence tests; benchmark duplicate negotiate check |
| UI/profile CLS | frontend lint; browser route check; targeted benchmark CLS |
| Media rendering | browser route check; targeted benchmark transfer/duplicate check |
| Chunk/lazy loading | frontend lint/build; targeted benchmark request/chunk check |
| API timing evidence | route-handler tests if behavior changes; inspect timing output |
| CDN/header audit | header probe output; no source behavior change unless evidence supports it |

---

## Final Acceptance Criteria

- Reports classify same-site media separately from true third-party.
- Current auth full→lite dedupe and presence defer are verified against production benchmark.
- Profile mobile CLS is fixed below threshold.
- Worst route families have lower request/chunk or transfer burden with no broken UI.
- Backend/CDN work is backed by timing/header evidence, not assumption.
- Updated `PERFORMANCE-AUDIT.md` shows fewer slow >800ms requests and clearer owner breakdown.
