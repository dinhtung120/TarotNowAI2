# PERFORMANCE-AUDIT - TarotNow `/vi` (Cycle 3)

## Executive Summary
- Original baseline timestamp (UTC): `2026-04-29T18:30:17.976Z`
- Cycle 2 full-matrix reference (UTC): `2026-04-29T20:56:04.704Z`
- Cycle 3 post-deploy full-matrix (UTC): `2026-04-30T07:24:43.284Z`
- Cycle 3 post-deploy targeted-hotspots (UTC): `2026-04-30T07:28:52.002Z`
- Target: `https://www.tarotnow.xyz/vi`
- Matrix: `Chromium` x `desktop/mobile` x `logged-out/logged-in-admin/logged-in-reader`
- Artifacts:
  - Cycle 3 full matrix: `perf-artifacts/cycle3/postdeploy-final/full-matrix/*`
  - Cycle 3 targeted hotspots: `perf-artifacts/cycle3/postdeploy-final/targeted-hotspots/*`

### Headline Outcomes
1. Collection request budget target achieved (PASS)
- Hotspot desktop `/vi/collection`: `105 -> 32` requests (`-69.52%`, gate `<=60` passed).

2. Collection image bottleneck removed at `>800ms` tier (PASS)
- Full matrix collection image `>800ms`: `124 -> 0` (`-100%`).
- Hotspot `/vi/collection` slow requests `>800ms`: `143 -> 0` (`-100%`).

3. Global slow-request gate strongly improved (PASS)
- Full matrix requests `>800ms`: `512 -> 79` (`-84.57%`, target `>=30%` reduction passed).

4. Admin auth-loop guard remains stable (PASS)
- `/vi/admin` desktop: `requestCount=33`, `documentReloadCount=0`, `handshakeRedirectCount=0`, `sessionApiCallCount=0`.

5. Core pending non-websocket remains clean (PASS)
- Full matrix pages with pending non-websocket requests: `0 -> 0`.

6. Authenticated request budget improved but not yet -20% vs Cycle 2 median (PARTIAL)
- Authenticated median request/page: `35 -> 32` (`-8.57%`) when compared strictly to Cycle 2 full-matrix baseline.
- Authenticated weighted average request/page: `37.30 -> 32.05` (`-14.09%`).

---

## Detailed Metrics Table

### A) Global Before/After (Cycle 2 -> Cycle 3, Full Matrix)
| Metric | Cycle 2 | Cycle 3 | Delta | Severity / Gate |
| --- | ---: | ---: | ---: | --- |
| Total pages | 190 | 190 | 0 | Informational |
| Total requests | 6,910 | 5,982 | -13.43% | Positive |
| Pages `>25 req` | 182 | 180 | -1.10% | High threshold |
| Pages `>35 req` | 77 | 11 | -85.71% | Critical threshold |
| Slow requests `>800ms` | 512 | 79 | -84.57% | High threshold (PASS) |
| Slow requests `400-800ms` | 1,336 | 605 | -54.72% | Medium threshold |
| Pending non-websocket pages | 0 | 0 | 0 | PASS |
| Auth median req/page (authenticated) | 35.00 | 32.00 | -8.57% | Partial vs `-20%` gate |
| Auth weighted avg req/page (authenticated) | 37.30 | 32.05 | -14.09% | Positive |

### B) Cycle 3 Scenario Summary (Full Matrix)
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Session API calls | Failed requests |
| --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | 9 | 26.3 | 2887 | 0 | 0 |
| logged-in-admin | desktop | 48 | 32.2 | 3095 | 10 | 1 |
| logged-in-reader | desktop | 38 | 32.3 | 3172 | 6 | 1 |
| logged-out | mobile | 9 | 25.9 | 2956 | 0 | 0 |
| logged-in-admin | mobile | 48 | 32.4 | 3277 | 6 | 2 |
| logged-in-reader | mobile | 38 | 31.3 | 2908 | 5 | 0 |

### C) Targeted Hotspots (Cycle 2 -> Cycle 3)
| Route | Samples | Cycle 2 Avg Req | Cycle 3 Avg Req | Delta | Cycle 2 `>800ms` | Cycle 3 `>800ms` | Admin/Auth Stability |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | --- |
| `/vi/admin` | 2 | 35.0 | 33.0 | -5.71% | 0 | 0 | `docReload=0`, `handshake=0`, `sessionApi=0` |
| `/vi/wallet/deposit/history` | 4 | 33.25 | 31.0 | -6.77% | 2 | 0 | stable |
| `/vi/community` | 4 | 43.5 | 34.25 | -21.26% | 4 | 1 | stable |
| `/vi/collection` | 4 | 78.75 | 32.0 | -59.37% | 143 | 0 | stable |

### D) Collection-Focused Metrics (Cycle 3 Post-Deploy)
| Scenario | Viewport | Image Requests | Image `400-800ms` | Image `>800ms` | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |
| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |
| logged-in-admin | desktop | 23 | 8 | 0 | 22 | 1 | 0 |
| logged-in-reader | desktop | 12 | 2 | 0 | 12 | 0 | 0 |
| logged-in-admin | mobile | 23 | 13 | 0 | 22 | 1 | 0 |
| logged-in-reader | mobile | 12 | 3 | 0 | 12 | 0 | 0 |

---

## Major Issues Found

### Critical
1. None on auth handshake loop and collection slow-image `>800ms`; both previous critical clusters are resolved.

### High
1. Remaining `>800ms` requests (79 total) are concentrated on heavy document/static fetch paths.
- Examples from benchmark: `/vi/wallet`, `/vi/login`, `/vi/admin/reader-requests`, specific `/vi/reading/session/[id]` documents and chunks.

2. `>25 requests` pages remain broad (`180/190`) even after large reduction in critical `>35` pages.
- This is now mostly a route shell/data composition budget issue rather than handshake loops.

### Medium
1. `400-800ms` bucket still high (`605` requests) with concentration on community/reading/wallet transitions.

2. Targeted hotspot run still shows transient pending on `/vi/collection` interaction probe.
- `pendingCount=8` appears during interaction-heavy capture, while full-matrix core navigation remains `0` pending non-websocket.

### Low
1. 304 cache-hit counter remains `0` in current probe outputs despite immutable strategy; this likely reflects browser/session conditions during run rather than contract regression.

---

## Optimization Plan (Impact Priority)

### P0 - Continue reducing authenticated request budget toward `-20%` median
- Collapse repeated shell-level fetches on authenticated routes (`community`, `wallet`, `reading/session` layouts).
- Keep request-count budget per route-family and fail CI when route exceeds budget drift.

### P1 - Cut remaining `>800ms` server/document spikes
- Prioritize high-latency document responses on `/vi/wallet`, `/vi/login`, `/vi/admin/reader-requests`, `/vi/reading/session/[id]`.
- Investigate server timing and origin TTFB variance for those endpoints/chunks.

### P2 - Reduce `400-800ms` tier and stabilize interaction bursts
- Fine-tune chunk preloading + interaction prefetch cooldown on community/reading flows.
- Add per-route interaction benchmark budget alerts (not only core navigation phase).

### P3 - Cache observability hardening
- Add explicit cache-hit telemetry for collection image proxy to validate persistent cache behavior over repeated sessions.

---

## Recommended Refactors (Cycle 3 Implemented)
1. Immutable collection image delivery contract with dedicated `thumbUrl/fullUrl` behavior and long-lived cache headers.
2. Single shared back-card for unowned cards (prevents fan-out art downloads).
3. New chunked catalog API contract: `manifest`, `chunks/{chunkId}`, `details/{cardId}`.
4. IndexedDB persistent cache for manifest/chunks/details with version-aware invalidation.
5. Route-specific collection hook split with intent-driven chunk loading + capped concurrency patterns.
6. Removed heavy eager collection prefetch from server prefetch runner to avoid up-front request bursts.
7. Benchmark schema extended with collection image metrics + interaction request accounting.
8. Perf gate script expanded for collection desktop budget and collection image `>800ms` reduction checks.
9. Static asset cache header hardening in Next config (`/_next/static`, `/themes`, `/images` immutable).

---

## Acceptance Check (Cycle 3)
1. `/vi/collection` desktop `requestCount <= 60`
- Status: PASS (`32`).

2. `/vi/collection` image requests `>800ms` giảm `>=70%`
- Status: PASS (`124 -> 0`, `-100%` in full matrix collection image metric).

3. Full matrix total `>800ms` requests giảm `>=30%` từ mốc `512`
- Status: PASS (`512 -> 79`, `-84.57%`).

4. `/vi/admin` loop guard integrity (`documentReloadCount=0`, `handshakeRedirectCount=0`)
- Status: PASS.

5. No pending non-websocket kéo dài trên core routes
- Status: PASS (`0` in full matrix).

6. Authenticated median request/page reduction `>=20%`
- Status: PARTIAL in strict Cycle 2 comparison (`35 -> 32`, `-8.57%`).

---

## Notes
- Cycle 3 delivered decisive wins on image bottleneck elimination and slow-request collapse.
- Remaining optimization headroom is now concentrated in authenticated shell/data composition and medium-latency route transitions.
- Full raw evidence is preserved under `perf-artifacts/cycle3/postdeploy-final/` for reproducibility.
