# PERFORMANCE-AUDIT - TarotNow `/vi` (Cycle 2)

## Executive Summary
- Baseline timestamp (UTC): `2026-04-29T18:30:17.976Z`
- Post-deploy full-matrix benchmark (UTC): `2026-04-29T20:56:04.704Z`
- Post-deploy targeted-hotspots benchmark (UTC): `2026-04-29T21:00:33.240Z`
- Target: `https://www.tarotnow.xyz/vi`
- Matrix: `Chromium` x `desktop/mobile` x `logged-out/logged-in-admin/logged-in-reader`
- Artifact snapshots (stable):
  - Full matrix: `perf-artifacts/cycle2/full-matrix/*`
  - Targeted hotspots: `perf-artifacts/cycle2/targeted-hotspots/*`

### Headline Outcomes
1. Authenticated request budget improved strongly (PASS)
- Weighted authenticated average requests/page: `55.26 -> 37.30` (`-32.50%`).
- Note: baseline median was not preserved in raw JSON from previous run, so Cycle 2 gate is evaluated with weighted authenticated average from locked baseline table.

2. `/vi/admin` handshake loop fixed (PASS)
- Baseline `/vi/admin` peak behavior: up to `681` requests with very high navigation time.
- Cycle 2 `/vi/admin` (desktop admin): `35` requests, `documentReloadCount=0`, `handshakeRedirectCount=0`, `sessionApiCallCount=0`.

3. Pending non-websocket requests eliminated on core matrix (PASS)
- Pages with pending non-websocket requests: `69 -> 0`.

4. Slow requests `>800ms` did not meet target (FAIL)
- Baseline: `141`
- Cycle 2 full matrix: `512` (`+263.12%`)
- Root cause concentration: third-party image downloads on `/vi/collection` (`img.tarotnow.xyz`), not auth loop.

---

## Detailed Metrics Table

### A) Global Before/After (Full Matrix)
| Metric | Baseline | Cycle 2 | Delta | Gate |
| --- | ---: | ---: | ---: | --- |
| Total pages | 190 | 190 | 0 | - |
| Total requests | 10,151 | 6,910 | -31.93% | Informational |
| Pages `>25 req` | 190 | 182 | -4.21% | High severity threshold |
| Pages `>35 req` | 172 | 77 | -55.23% | Critical severity threshold |
| Slow requests `>800ms` | 141 | 512 | +263.12% | `FAIL` (target was -30%) |
| Slow requests `400-800ms` | 720 | 1,336 | +85.56% | Medium severity threshold |
| Pages with pending non-websocket | 69 | 0 | -100% | `PASS` |

### B) Scenario Summary (Cycle 2 Full Matrix)
| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Session API calls | Failed requests |
| --- | --- | ---: | ---: | ---: | ---: | ---: |
| logged-out | desktop | 9 | 27.4 | 2935 | 0 | 0 |
| logged-in-admin | desktop | 48 | 38.4 | 3275 | 11 | 4 |
| logged-in-reader | desktop | 38 | 37.5 | 3487 | 12 | 6 |
| logged-out | mobile | 9 | 27.4 | 2946 | 0 | 0 |
| logged-in-admin | mobile | 48 | 37.2 | 3439 | 9 | 5 |
| logged-in-reader | mobile | 38 | 35.9 | 3703 | 8 | 5 |

### C) Hotspot Benchmark (Cycle 2 Targeted)
| Route | Samples | Avg req | Max req | Avg nav (ms) | `>800ms` slow req | Doc reload | Handshake redirects | Session API calls |
| --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |
| `/vi/admin` | 2 | 35.0 | 35 | 3150 | 0 | 0 | 0 | 0 |
| `/vi/wallet/deposit/history` | 4 | 33.3 | 34 | 3156 | 2 | 0 | 0 | 1 |
| `/vi/community` | 4 | 43.5 | 44 | 4116 | 4 | 0 | 0 | 6 |
| `/vi/collection` | 4 | 78.8 | 105 | 5082 | 143 | 0 | 0 | 3 |

### D) Most Expensive Routes (Cycle 2, by avg request volume)
| Route | Samples | Avg req/page | Max req | Avg nav (ms) | Avg LCP (ms) | Avg TBT (ms) |
| --- | ---: | ---: | ---: | ---: | ---: | ---: |
| `/vi/collection` | 4 | 79.0 | 105 | 4266 | 1875 | 184.3 |
| `/vi/profile/reader` | 4 | 51.5 | 68 | 3068 | 1236 | 12.8 |
| `/vi/wallet/withdraw` | 4 | 51.3 | 68 | 3394 | 1300 | 26.5 |
| `/vi/community` | 4 | 43.0 | 43 | 4615 | 2286 | 47.3 |
| `/vi/gacha` | 4 | 40.5 | 41 | 3206 | 1236 | 53.8 |

---

## Major Issues Found

### Critical
1. `>800ms` slow-request count regressed heavily and is dominated by third-party image fetches.
- Evidence: full matrix `512` slow requests `>800ms`; targeted `/vi/collection` alone contributes `143`.
- Top offenders are `img.tarotnow.xyz` AVIF assets with multi-second download times.

2. `/vi/collection` still exceeds route-critical request budget.
- Evidence: targeted avg `78.8` req/page, max `105`.

### High
1. Several authenticated routes are still above `>35 req` threshold.
- Examples: `/vi/community` (`43-44`), `/vi/profile/reader` (`avg 51.5`), `/vi/wallet/withdraw` (`avg 51.3`).

2. Medium/slow network pressure remains high on community/collection experiences.
- Evidence: community has elevated nav/LCP; collection carries most slow-request volume.

### Medium
1. Small residual failed-request signals still appear in matrix logs.
- Mostly navigation-abort style failures and telemetry noise; continue monitoring in next cycle.

---

## Optimization Plan (Impact Priority)

### P0 - Collection image delivery (highest impact for `>800ms` gate)
- Move collection card images to stricter budget strategy:
  - serve lower-weight thumbnails for list/grid; defer full-size assets to zoom/detail.
  - reduce initial above-the-fold image count.
  - enforce explicit `sizes` and smaller responsive widths.
- Add request concurrency guard for collection image loading.
- Validate CDN path/cache headers for `img.tarotnow.xyz` (`cache-control: public, immutable`, edge hit ratio).

### P1 - Route budgets still above 35 requests
- Continue trimming authenticated pages with route-specific budgets:
  - `/vi/community`
  - `/vi/profile/reader`
  - `/vi/wallet/withdraw`
- Expand intent-only prefetch denylist for heavy mobile paths.

### P2 - Query and invalidation follow-up
- Keep narrow invalidation scope and extend to remaining modules with broad invalidation.
- Add route-level query telemetry for repeated key bursts.

### P3 - Gate hardening
- Keep new schema fields as required in CI gate:
  - `documentReloadCount`
  - `handshakeRedirectCount`
  - `sessionApiCallCount`
  - `failedRequestCount`
- Persist baseline JSON snapshots outside volatile test output directory.

---

## Recommended Refactors (Cycle 2 Implemented)
1. Auth handshake loop breaker + document-only handshake gating.
2. Session snapshot dedupe and anti re-entry stabilization.
3. Admin request-budget reduction with dashboard summary aggregation.
4. Admin query policy normalization (staleTime/refetch/retry tuning).
5. Narrower invalidation scope for notifications/admin paths.
6. Hydration mismatch mitigation path for relative-time rendering.
7. Conditional image optimization strategy rollout.
8. Benchmark schema expansion + targeted hotspot benchmark mode + perf gate script.

---

## Acceptance Check

1. Gate: authenticated request/page reduction >=20%
- Status: `PASS` using locked baseline weighted authenticated average (`-32.50%`).

2. Gate: reduce `>800ms` requests >=30%
- Status: `FAIL` (`141 -> 512`, regression driven by third-party collection images).

3. Gate: `/vi/admin` no handshake loop burst
- Status: `PASS` (stable `35` requests, zero handshake/document reload/session API on desktop admin route sample).

4. Gate: no pending non-websocket on core routes
- Status: `PASS` (`69 -> 0`).

---

## Notes
- Current deployment has confirmed major wins on auth-loop removal and request-budget normalization.
- Next cycle must prioritize image delivery architecture for collection/feed to recover slow-request gates.
