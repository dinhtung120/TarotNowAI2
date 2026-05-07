# Performance Audit & Optimization Design

## Context

TarotNowAI2 already has a Playwright-based navigation benchmark in `Frontend/tests/tarotnow-navigation-benchmark.spec.ts`. It covers route discovery, logged-out/admin/reader scenarios, network tracking, request thresholds, slow request thresholds, paint metrics, duplicate requests, pending requests, and benchmark outputs.

The approved approach is a full production audit first, followed by evidence-based hotspot fixes, targeted reruns, deployment verification, and a final full production re-audit. The work should extend the existing benchmark instead of replacing it. The target is `https://www.tarotnow.xyz/vi`, including public and protected routes, with smaller feature-level reruns after each fix.

## Goals

- Measure the full website in logged-out, logged-in admin, and logged-in tarot-reader modes.
- Cover public routes, protected `/user/*`-style product routes, dynamic reading/chat/reader/community details, and routes discoverable through static route scanning, sitemap/robots, and DOM crawling.
- Split benchmark execution by feature area so hotspot fixes can be verified quickly.
- Generate a detailed root-level `PERFORMANCE-AUDIT.md` with severity, metrics, root causes, and optimization plan.
- Implement evidence-based optimizations with minimal, surgical changes.
- Commit and push approved work, monitor GitHub Actions, fix CI failures, and rerun production benchmark after successful deploy.

## Benchmark Architecture

Keep `Frontend/tests/tarotnow-navigation-benchmark.spec.ts` as the primary benchmark foundation. Extend it where needed rather than creating a separate measurement system.

Add feature grouping to benchmark routes and outputs:

- `auth-public`: home, login, register, legal, and public landing pages.
- `reading`: reading landing, spread types, reading session, reading history, and reading detail pages.
- `inventory-gacha-collection`: inventory, gacha, gacha history, collection, collection modal, and collection image probes.
- `profile-wallet-notifications`: profile tabs, MFA, wallet, deposit, withdraw, deposit history, and notifications.
- `reader-chat`: readers list/detail, reader apply/app, chat list, and chat detail.
- `community-leaderboard-quest`: community list/detail/comments/reactions, leaderboard, gamification, quest/mission pages.
- `admin`: admin routes in admin scenario only.

Support both full and targeted runs:

- Full matrix for baseline and final production validation.
- Feature-targeted mode for quick reruns after each hotspot fix.
- Shared output format across modes: JSON, page CSV, request CSV, route map, and markdown summaries.
- Include `feature` or `routeFamily` in page and request outputs so reports can filter hotspot groups.

## Metrics and Request Tracking

For every measured route, collect:

- Request count and request category breakdown: HTML, API, static, third-party, telemetry, websocket, other.
- Timing: TTFB, DOMContentLoaded, Load, FCP, LCP, CLS, TBT, and navigation duration.
- Response bytes and transfer bytes.
- Pending or hanging non-persistent requests.
- Failed requests and 4xx/5xx responses.
- Duplicate request groups with normalized query strings.
- Slow requests: `>400ms` as needs optimization, `>800ms` as severe.
- Route interaction notes and coverage blockers.

Keep request count thresholds:

- `>25` requests: High, suspicious.
- `>35` requests: Critical, severe.

## Audit Report Design

Generate root-level `PERFORMANCE-AUDIT.md` from benchmark data. The report must include:

1. Executive Summary
   - Timestamp, target origin, benchmark modes, route coverage, scenario login status, and severity totals.
2. Detailed Metrics Table
   - Route, feature, scenario, viewport, request count, category breakdown, timings, bytes, pending count, and severity.
3. Major Issues Found
   - Critical/High route counts, slow requests, duplicate API calls, waterfalls, pending requests, failed requests, session/auth churn, image/cache issues, and over-fetching.
4. Optimization Plan
   - Ordered by impact and blast radius: shared bottlenecks before per-page cleanup, severe routes before medium issues.
5. Recommended Refactors
   - Map each issue to the likely code area: middleware/session, TanStack Query keys/defaults, App Router layouts, server/client component boundaries, custom hooks, image loading, cache headers, or route prefetch.
6. Final Validation
   - CI result, post-deploy benchmark delta, remaining issues, and next steps.

## Optimization Execution

Use small evidence-based loops:

1. Run the full production benchmark with logged-out, admin, and tarot-reader scenarios.
2. Generate and inspect `PERFORMANCE-AUDIT.md` from the fresh benchmark data.
3. Analyze report and group hotspots into shared, feature-specific, and asset/cache issues.
4. Inspect code only for the hotspot areas identified by benchmark data.
5. Apply minimal fixes.
6. Run the corresponding feature benchmark.
7. Open the affected flows in a browser and compare manual observations with Playwright metrics.
8. Broaden verification only when the fix touches shared paths.
9. Commit, push, monitor GitHub Actions, and fix CI failures if any.
10. After CI succeeds, rerun the full production benchmark and update `PERFORMANCE-AUDIT.md`.

Optimization targets include:

- TanStack Query: stable query keys, deduping, staleTime/gcTime/refetch behavior, and avoiding over-fetching.
- Middleware/session: reduce repeated session or handshake calls without weakening fail-closed auth behavior.
- App Router layouts and streaming: avoid layout-level refetch churn and unnecessary client rendering.
- Custom hooks: fix repeated effects, unstable dependencies, subscriptions, debounce/throttle issues, and fetch loops only when proven.
- Images: correct lazy/eager strategy, dimensions, CDN/cache usage, and modal reopen behavior.
- Route prefetching: reduce network churn while preserving clear UX wins.

## Safety Boundaries

- Do not remove or weaken auth, ownership, policy, rate-limit, or security guards to improve benchmark numbers.
- Do not change backend or database code unless benchmark and code inspection show the backend is the root bottleneck.
- Do not make broad unrelated refactors.
- Do not commit credentials, `.claude/settings.local.json`, temporary lock files, local-only config, or large benchmark artifacts unless explicitly needed and safe.
- Use provided production credentials only through environment variables or interactive session state, never in repository files or reports.
- Production full audit is approved for this cycle, including bounded login and reading-session creation for coverage.
- Keep production side effects minimal beyond the approved benchmark probes. Prefer read/open/toggle probes where possible, and record write-like probing in coverage notes.
- Keep recursive crawling bounded by route/page limits.

## Verification Strategy

Use the smallest relevant verification first:

- Feature benchmark after feature-specific fixes.
- Full benchmark before and after deploy.
- `npm run lint` for frontend code changes.
- Targeted tests when business logic, auth, cache, hooks, or protected flows change.
- Browser manual checks for major UI hotspot flows after Playwright benchmark finds or fixes issues.
- GitHub Actions monitoring after push, using the frontend-only deploy workflow for frontend-only changes and the broader deploy workflow only when backend/database changes are included.

## Success Criteria

- Baseline production benchmark is captured for logged-out, admin, and tarot-reader modes.
- Benchmark can be rerun by feature group.
- `PERFORMANCE-AUDIT.md` is detailed, severity-ranked, and tied to actual benchmark evidence.
- Critical/High issues are either fixed or documented with a clear reason and next step.
- Local verification passes for the touched scope.
- Changes are committed and pushed.
- Relevant GitHub Actions workflow passes.
- Final production benchmark after deploy shows no severe regression in optimized areas and updates the audit report.
