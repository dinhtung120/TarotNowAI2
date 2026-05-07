# Sequential Performance Optimization Design

**Goal:** Optimize the remaining production performance bottlenecks from `PERFORMANCE-AUDIT.md` in three gated phases, deploying and validating each phase before starting the next.

**Approved approach:** Sequential gated optimization: Phase 1 shared auth/session/presence, Phase 2 collection image latency, Phase 3 bundle/static request reduction.

## Current Audit Baseline

Source: `PERFORMANCE-AUDIT.md` generated from production full-matrix benchmark for `https://www.tarotnow.xyz/vi`.

Key baseline numbers:

- Benchmark mode: `full-matrix`
- Total pages measured: `190`
- Critical pages: `24`
- High pages: `135`
- Medium pages: `31`
- Slow requests >800ms: `291`
- Slow requests 400-800ms: `1346`
- Duplicate request groups: `92`
- Pending pages: `11`

Important existing fix:

- Collection proxy images no longer go through `/_next/image?...collection%2Fcard-image...`.
- Remaining collection issue is latency on direct `/api/collection/card-image` requests, not optimizer 400s.

## Optimization Rules

Each phase must be completed independently before the next starts:

1. Investigate root cause for the phase-specific bottleneck.
2. Write failing tests for behavior changes where applicable.
3. Implement the minimum safe code change.
4. Run local verification.
5. Commit and push the phase.
6. Wait for GitHub Actions to pass.
7. Run the phase-specific production benchmark.
8. Update `PERFORMANCE-AUDIT.md` with phase results.
9. Continue only if the benchmark confirms no regression and the targeted bottleneck improved or has a documented external cause.

Do not bundle unrelated optimizations across phases. If a phase reveals a broader architectural issue, stop after documenting the evidence and ask before expanding scope.

## Phase 1: Shared Auth, Session, and Presence

### Problem

The audit shows pending and duplicate background requests across public and authenticated routes, especially:

- `/api/v1/presence/negotiate?negotiateVersion=1`
- `/api/readers?page=1&pageSize=4`
- `/api/auth/session?mode=lite`
- `/api/me/runtime-policies`

These appear on routes that should not need all of them, including auth/public routes such as `/vi/login`, `/vi/register`, and `/vi/forgot-password`.

### Hypothesis to verify

A shared provider, layout, or global client hook is starting authenticated/session/presence work too broadly. The desired state is that public auth pages do not start presence negotiation or reader/session fan-out unless a visible component or protected feature actually needs it.

### Design

- Trace request origin from the frontend providers/layouts/hooks that initialize auth, session, presence, and homepage reader data.
- Move or gate presence negotiation so it runs only after authenticated state is confirmed and only on routes/features that need live presence.
- Ensure public auth routes do not fetch reader lists unless the page intentionally renders a reader preview.
- Keep auth fail-closed behavior intact: do not skip required session validation for protected routes.
- Prefer existing TanStack Query controls (`enabled`, stable query keys, stale times) over ad-hoc global flags.

### Success criteria

- Targeted production benchmark for `auth-public` shows fewer pending requests on `/vi/login`, `/vi/register`, and `/vi/forgot-password`.
- No auth regression in login/protected-route navigation.
- `npm run lint` passes.
- Relevant auth/session tests or risk coverage gates pass if touched.

### Verification gate

- Local: `npm run lint`
- Add targeted unit tests for any changed route gating/query enabling logic.
- Production after deploy: targeted benchmark for `auth-public` and any protected route touched by the fix.
- Browser check: login flow and one protected route load successfully.

## Phase 2: Collection Image Latency

### Problem

After the optimizer bypass fix, collection images now return `200` directly from `/api/collection/card-image`, but many requests remain slow: roughly 1.2s to 3.8s in the audit.

### Hypothesis to verify

The collection image proxy likely re-fetches upstream image bytes too often or lacks effective HTTP/browser/CDN caching for stable `src + iv` combinations. The `iv` parameter already gives a versioned cache key, so the proxy should be cacheable when authorization constraints allow it.

### Design

- Inspect the `/api/collection/card-image` route implementation and any upstream fetch/cache headers.
- Confirm whether the response is user-sensitive. If the proxy returns only public card art for a versioned `src + iv`, cache it aggressively. If authorization or ownership affects the bytes, cache only at the correct private boundary.
- Preserve existing SSRF and URL allowlist protections.
- Add or adjust cache headers and fetch caching so repeated card thumbnails avoid origin/upstream waterfalls.
- Keep the current `unoptimized` image decision for this proxy path.

### Success criteria

- Targeted production benchmark for `inventory-gacha-collection` shows lower slow request count or lower top latency for `/api/collection/card-image`.
- No reintroduction of `/_next/image?...collection%2Fcard-image...` requests.
- Collection page still opens cards and zoom modal correctly.

### Verification gate

- Local: route/unit tests for cache header behavior if the route is testable.
- Local: `npm run lint`.
- Production after deploy: `inventory-gacha-collection` benchmark.
- Browser check: `/vi/collection`, open card modal, close/reopen; inspect console/network for image errors.

## Phase 3: Bundle and Static Request Reduction

### Problem

The audit shows 27-50 static requests on several auth/admin/public routes. Examples include many shared chunks, fonts, theme CSS, and telemetry script requests. This makes route loads look high request-count even when API count is low.

### Hypothesis to verify

Shared client providers, broad imports, or eager route prefetching are pulling too many chunks into routes that need minimal UI. Auth pages and admin pages likely have different optimization opportunities and should not be treated as one bundle.

### Design

- Use the benchmark plus Next build output to identify whether request count comes from unavoidable framework chunks or app-level shared imports.
- Start with auth/public routes because they have the highest static counts and simplest expected UI.
- Inspect root/nested layouts and feature public exports for heavy client imports used by auth pages.
- Move heavy components behind route-level dynamic imports only where the UI does not need them for first paint.
- Review Link prefetch behavior only where benchmark shows unnecessary route-navigation request churn.
- Keep accessibility, localization, and existing UI behavior intact.

### Success criteria

- Targeted benchmark for `auth-public` shows reduced static request count or transfer bytes on `/vi/login`, `/vi/register`, and `/vi/forgot-password`.
- Targeted benchmark for `admin` shows no regression and preferably lower static request count on the worst admin routes.
- Final full-matrix benchmark updates `PERFORMANCE-AUDIT.md` after all three phases.

### Verification gate

- Local: `npm run lint` and `npm run build` if bundle/layout behavior changes.
- Production after deploy: targeted `auth-public` and `admin` benchmarks.
- Browser check: auth pages, one admin list route, and one normal authenticated route.
- Final: full production benchmark and final audit update.

## Reporting and Commit Strategy

Each phase gets its own code commit and its own audit/report commit when benchmark data is updated. Commit only relevant files. Do not include local files such as `.claude/settings.local.json`, scheduled task locks, `.playwright-mcp/`, or raw credentials.

After each deploy, record in `PERFORMANCE-AUDIT.md`:

- Phase name
- Commit SHA
- GitHub Actions run ID/status
- Targeted benchmark mode and routes/features
- Before/after metric summary
- Remaining bottlenecks and whether to proceed to the next phase

## Open Constraints

- Production benchmark side effects must remain bounded and equivalent to the already approved audit workflow.
- If GitHub Actions fails, fix CI before benchmarking the phase.
- If a phase does not improve metrics, do not proceed silently; document the evidence and decide whether to refine the phase or move to the next bottleneck.
