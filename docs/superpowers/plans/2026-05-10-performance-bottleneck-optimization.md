# Performance Bottleneck Optimization Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Fix measured TarotNow performance bottlenecks from `PERFORMANCE-AUDIT.md` and `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md` without weakening auth, removing realtime, or making unevidenced backend/CDN changes.

**Architecture:** First correct benchmark ownership classification so same-product media hosts stop looking like external third-party work. Then apply safe frontend changes with targeted tests and browser/benchmark checks: profile mobile CLS, presence lifecycle hardening, proven duplicate media rendering, and measured chunk fan-out reductions. Backend/CDN work in this plan is evidence capture only unless timing/header data proves a safe follow-up change.

**Tech Stack:** Next.js 16 App Router, React 19, TypeScript, TanStack Query 5, SignalR presence, Vitest, Playwright production benchmark, Node audit scripts.

---

## File Map

- `PERFORMANCE-AUDIT.md` — generated audit baseline and final regenerated report.
- `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md` — source root-cause report; read-only during implementation.
- `Frontend/tests/tarotnow-navigation-benchmark.spec.ts` — request classification, request breakdown, route benchmark output.
- `Frontend/scripts/generate-performance-audit.mjs` — generated report sections and owner summaries.
- `Frontend/src/features/profile/**` — profile mobile layout and CLS fix target.
- `Frontend/src/app/_shared/hooks/usePresenceConnection.ts` — delayed SignalR presence startup and one-attempt lifecycle hardening.
- `Frontend/src/app/_shared/hooks/usePresenceConnection.test.tsx` — presence lifecycle/timer regression tests.
- `Frontend/src/features/auth/shared/auth/clientSessionSnapshot.ts` — read-only auth full→lite snapshot dedupe verification anchor.
- `Frontend/src/features/auth/shared/auth/clientSessionSnapshot.test.ts` — auth dedupe regression tests.
- `Frontend/src/shared/http/assetUrl.ts` — image optimization policy; allowlist-only changes if evidence requires.
- `Frontend/src/features/community/**` — community duplicate media investigation/fix target.
- `Frontend/src/features/collection/**` — collection media window and duplicate candidate investigation/fix target.
- `Frontend/src/app/[locale]/**` and `Frontend/src/features/**/public.ts` — chunk fan-out/lazy-load inspection and scoped route changes.
- `Frontend/src/app/api/_shared/forwardUpstreamJsonWithTimeout.ts` and selected `Frontend/src/app/api/**/route.ts` — optional timing evidence only.

---

### Task 1: Add same-site media request classification

**Files:**
- Modify: `Frontend/tests/tarotnow-navigation-benchmark.spec.ts`

- [ ] **Step 1: Add the failing classification test or inline assertion harness**

Add a small unit-style helper section near existing benchmark helpers only if this file already has helper tests. If it does not, add temporary local assertions inside a new `test.describe('request classification helpers', ...)` block that never navigates production pages:

```ts
test.describe('request classification helpers', () => {
  test('classifies tarot media hosts as same-site media', () => {
    expect(classifyRequestCategory(createRequestLike('https://img.tarotnow.xyz/light-god-50/a.avif', 'image'))).toBe('same-site-media');
    expect(classifyRequestCategory(createRequestLike('https://media.tarotnow.xyz/community/a.webp', 'image'))).toBe('same-site-media');
    expect(classifyRequestCategory(createRequestLike('https://static.vendor.example/a.js', 'script'))).toBe('third-party');
  });
});
```

If `classifyRequestCategory` is not exported/testable and Playwright helper tests are not feasible, skip this test block and use Step 2 plus targeted benchmark verification in Step 5 as the regression check.

- [ ] **Step 2: Extend the category type**

Change:

```ts
type RequestCategory = 'html' | 'api' | 'static' | 'third-party' | 'telemetry' | 'websocket' | 'other';
```

to:

```ts
type RequestCategory = 'html' | 'api' | 'static' | 'same-site-media' | 'third-party' | 'telemetry' | 'websocket' | 'other';
```

- [ ] **Step 3: Add same-site media hosts before third-party fallback**

Add near benchmark constants:

```ts
const SAME_SITE_MEDIA_HOSTS = new Set(['img.tarotnow.xyz', 'media.tarotnow.xyz']);
```

Update `classifyRequestCategory` so telemetry remains first, `/api/` remains API, and media host detection happens before `!isSameOrigin` returns `third-party`:

```ts
let parsedUrl: URL | null = null;
try {
  parsedUrl = new URL(url);
} catch {
  parsedUrl = null;
}

if (parsedUrl && SAME_SITE_MEDIA_HOSTS.has(parsedUrl.hostname.toLowerCase())) {
  return 'same-site-media';
}
```

Keep same-origin `image` requests in `static`.

- [ ] **Step 4: Extend page breakdown and cache issue handling**

Add `sameSiteMedia: number` to `PageBreakdown`, initialize it to `0`, and add switch handling:

```ts
case 'same-site-media':
  breakdown.sameSiteMedia += 1;
  break;
```

Update cache issue logic so same-site media receives the same cache-header checks as static/third-party assets:

```ts
if (category === 'static' || category === 'same-site-media' || category === 'third-party') {
```

- [ ] **Step 5: Run targeted benchmark classification check**

Run from `Frontend`:

```bash
npm run test -- tarotnow-navigation-benchmark.spec.ts --grep "request classification helpers"
```

Expected if helper test exists: PASS.

If no helper test exists, run a targeted hotspot benchmark instead:

```bash
TAROTNOW_BENCHMARK_MODE=targeted-hotspots npm run benchmark:navigation
```

Expected: benchmark JSON includes `requestBreakdown.sameSiteMedia` for routes loading `img.tarotnow.xyz` or `media.tarotnow.xyz`, and those hosts no longer increase `thirdParty`.

- [ ] **Step 6: Commit measurement classification change**

```bash
git add Frontend/tests/tarotnow-navigation-benchmark.spec.ts
git commit -m "test: classify same-site media in navigation benchmark"
```

---

### Task 2: Add audit owner summaries

**Files:**
- Modify: `Frontend/scripts/generate-performance-audit.mjs`
- Regenerate: `PERFORMANCE-AUDIT.md`

- [ ] **Step 1: Update detailed metrics table columns**

Change detailed table header from API/static/third-party only to include same-site media:

```md
| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Same-site media | Third-party | Nav ms | DCL ms | Load ms | FCP ms | LCP ms | CLS | TBT ms | Transfer KB | Slow >800 | Slow 400-800 | Failed | Pending |
```

Update each row to use:

```js
page.requestBreakdown?.sameSiteMedia ?? 0
```

between `static` and `thirdParty`.

- [ ] **Step 2: Add route-family owner summary helper**

Add a helper that sums request counts by family and owner category:

```js
function buildOwnerSummary(pages, localePrefix) {
  const families = new Map();
  for (const page of pages) {
    const family = routeFamily(page.route, localePrefix);
    const current = families.get(family) ?? {
      family,
      pages: 0,
      requests: 0,
      api: 0,
      static: 0,
      sameSiteMedia: 0,
      telemetry: 0,
      websocket: 0,
      thirdParty: 0,
      html: 0,
      other: 0,
    };
    const breakdown = page.requestBreakdown ?? {};
    current.pages += 1;
    current.requests += page.requestCount ?? 0;
    current.api += breakdown.api ?? 0;
    current.static += breakdown.static ?? 0;
    current.sameSiteMedia += breakdown.sameSiteMedia ?? 0;
    current.telemetry += breakdown.telemetry ?? 0;
    current.websocket += breakdown.websocket ?? 0;
    current.thirdParty += breakdown.thirdParty ?? 0;
    current.html += breakdown.html ?? 0;
    current.other += breakdown.other ?? 0;
    families.set(family, current);
  }
  return [...families.values()].sort((a, b) => b.requests - a.requests);
}
```

- [ ] **Step 3: Render owner summary and next action**

Add a section after route family coverage:

```js
function renderOwnerSummary(pages, localePrefix) {
  const summary = buildOwnerSummary(pages, localePrefix);
  const lines = [
    '## Owner Summary by Route Family',
    '',
    '| Route family | Pages | Requests | Static | Same-site media | API | HTML | Websocket | Telemetry | Third-party | Likely next action |',
    '|---|---:|---:|---:|---:|---:|---:|---:|---:|---:|---|',
  ];

  for (const row of summary) {
    const nextAction = resolveOwnerNextAction(row);
    lines.push(`| ${row.family} | ${row.pages} | ${row.requests} | ${row.static} | ${row.sameSiteMedia} | ${row.api} | ${row.html} | ${row.websocket} | ${row.telemetry} | ${row.thirdParty} | ${nextAction} |`);
  }

  return lines.join('\n');
}
```

Use this action resolver:

```js
function resolveOwnerNextAction(row) {
  if (row.static >= row.api && row.static >= row.sameSiteMedia) return 'FE chunk/lazy-load review';
  if (row.sameSiteMedia > 0) return 'Media CDN/data variant review';
  if (row.api > 0 || row.html > 0) return 'Server-Timing/API trace capture';
  if (row.websocket > 0) return 'Realtime lifecycle verification';
  if (row.telemetry > 0) return 'Infra telemetry review';
  return 'No dominant owner';
}
```

- [ ] **Step 4: Regenerate audit**

Run from repo root or documented script location:

```bash
node Frontend/scripts/generate-performance-audit.mjs
```

Expected: `PERFORMANCE-AUDIT.md` regenerates with `Same-site media` and `Owner Summary by Route Family`.

- [ ] **Step 5: Inspect audit diff**

Run:

```bash
git diff -- Frontend/scripts/generate-performance-audit.mjs PERFORMANCE-AUDIT.md
```

Expected: diff only changes reporting/classification presentation. No runtime source files changed.

- [ ] **Step 6: Commit audit reporting change**

```bash
git add Frontend/scripts/generate-performance-audit.mjs PERFORMANCE-AUDIT.md
git commit -m "chore: add owner summary to performance audit"
```

---

### Task 3: Verify auth session dedupe remains safe

**Files:**
- Read: `Frontend/src/features/auth/shared/auth/clientSessionSnapshot.ts`
- Test: `Frontend/src/features/auth/shared/auth/clientSessionSnapshot.test.ts`
- Inspect: `PERFORMANCE-AUDIT.md`

- [ ] **Step 1: Confirm regression tests cover full→lite reuse**

Check `clientSessionSnapshot.test.ts` includes these behaviors:

```ts
it('reuses a fresh full snapshot for lite session checks', async () => {
  mockedFetchWithTimeout.mockResolvedValue(createSessionResponse('user-1'));

  await getClientSessionSnapshot({ mode: 'full', maxAgeMs: 10_000 });
  const liteSnapshot = await getClientSessionSnapshot({ mode: 'lite', maxAgeMs: 10_000 });

  expect(liteSnapshot.authenticated).toBe(true);
  expect(mockedFetchWithTimeout).toHaveBeenCalledTimes(1);
});
```

and:

```ts
it('reuses an in-flight full request for lite session checks', async () => {
  let resolveResponse: (response: Response) => void = () => undefined;
  mockedFetchWithTimeout.mockReturnValue(new Promise<Response>((resolve) => {
    resolveResponse = resolve;
  }));

  const fullPromise = getClientSessionSnapshot({ mode: 'full', maxAgeMs: 10_000 });
  const litePromise = getClientSessionSnapshot({ mode: 'lite', maxAgeMs: 10_000 });

  resolveResponse(createSessionResponse('user-1'));

  await expect(fullPromise).resolves.toMatchObject({ authenticated: true });
  await expect(litePromise).resolves.toMatchObject({ authenticated: true });
  expect(mockedFetchWithTimeout).toHaveBeenCalledTimes(1);
});
```

If either test is missing, add it exactly.

- [ ] **Step 2: Run auth snapshot tests**

Run from `Frontend`:

```bash
npm run test -- clientSessionSnapshot.test.ts
```

Expected: PASS. No `cache: 'no-store'` behavior removed, no TTL increased.

- [ ] **Step 3: Verify benchmark evidence**

Run targeted hotspot benchmark after Task 1/2 are complete:

```bash
TAROTNOW_BENCHMARK_MODE=targeted-hotspots npm run benchmark:navigation
```

Expected: duplicate `/api/auth/session?mode=lite` requests decrease or remain bounded; no failed auth/session requests appear.

- [ ] **Step 4: Commit only if tests were added or changed**

If no source/test files changed, do not commit. Record result in final implementation summary.

If tests were added:

```bash
git add Frontend/src/features/auth/shared/auth/clientSessionSnapshot.test.ts
git commit -m "test: cover client session snapshot reuse"
```

---

### Task 4: Harden presence startup lifecycle

**Files:**
- Modify: `Frontend/src/app/_shared/hooks/usePresenceConnection.ts`
- Test: `Frontend/src/app/_shared/hooks/usePresenceConnection.test.tsx`

- [ ] **Step 1: Write failing test for duplicate negotiate prevention**

Add test that mounts the hook with `enabled: true`, authenticated state, advances timers by `10_000`, rerenders with stable auth/query dependencies, and asserts one connection start/negotiate call:

```ts
it('starts at most one delayed presence connection for a stable route lifetime', async () => {
  mockAuthenticatedSession();
  const { rerender } = renderHook(({ enabled }) => usePresenceConnection({ enabled }), {
    initialProps: { enabled: true },
    wrapper: createQueryWrapper(),
  });

  await vi.advanceTimersByTimeAsync(10_000);
  await Promise.resolve();
  rerender({ enabled: true });
  await vi.advanceTimersByTimeAsync(10_000);
  await Promise.resolve();

  expect(mockConnectionStart).toHaveBeenCalledTimes(1);
});
```

Adapt helper names to existing test utilities in `usePresenceConnection.test.tsx`.

- [ ] **Step 2: Run test and verify failure or existing pass**

Run from `Frontend`:

```bash
npm run test -- usePresenceConnection.test.tsx
```

Expected before code change: FAIL if duplicate lifecycle bug exists, PASS if current hook already satisfies invariant.

- [ ] **Step 3: Implement minimal lifecycle guard only if test fails**

If the test fails, add a ref that tracks pending/active startup for the mounted hook lifetime:

```ts
const startupRef = useRef<'idle' | 'scheduled' | 'connecting' | 'connected'>('idle');
```

Set `startupRef.current = 'scheduled'` before `setTimeout`, `connecting` inside timer before `init()`, `connected` after successful start, and reset to `idle` only in cleanup/stop paths. Before scheduling, return early unless state is `idle`.

Do not remove `PRESENCE_CONNECT_DELAY_MS = 10_000`. Do not disable realtime globally.

- [ ] **Step 4: Run presence tests**

```bash
npm run test -- usePresenceConnection.test.tsx
```

Expected: PASS.

- [ ] **Step 5: Verify benchmark duplicate negotiate**

Run targeted hotspot benchmark:

```bash
TAROTNOW_BENCHMARK_MODE=targeted-hotspots npm run benchmark:navigation
```

Expected: `/api/v1/presence/negotiate?negotiateVersion=1` is outside initial route benchmark window or appears at most once per route lifetime; no realtime failed requests introduced.

- [ ] **Step 6: Commit presence hardening if code changed**

```bash
git add Frontend/src/app/_shared/hooks/usePresenceConnection.ts Frontend/src/app/_shared/hooks/usePresenceConnection.test.tsx
git commit -m "fix: prevent duplicate delayed presence startup"
```

If test already passed and no code changed, commit only added regression test:

```bash
git add Frontend/src/app/_shared/hooks/usePresenceConnection.test.tsx
git commit -m "test: cover delayed presence startup lifecycle"
```

---

### Task 5: Fix profile mobile CLS

**Files:**
- Inspect/modify: `Frontend/src/features/profile/**`
- Verify: `PERFORMANCE-AUDIT.md`

- [ ] **Step 1: Locate above-fold profile components**

Run:

```bash
grep -RIn "avatar\|Avatar\|profile\|cover\|skeleton\|image\|Image" Frontend/src/features/profile Frontend/src/app/[locale] | head -160
```

Expected: list of profile header/card/media components used by `/vi/profile`.

- [ ] **Step 2: Capture current mobile layout shift locally**

Start frontend dev server if not running:

```bash
npm run dev
```

Open `/vi/profile` mobile viewport in browser. Expected baseline: visible profile content renders without auth crash; current audit shows mobile CLS around `0.0889` for reader scenario.

- [ ] **Step 3: Reserve dimensions for shifting above-fold elements**

For avatar/media/card/panel elements found in Step 1, add fixed Tailwind sizing or aspect ratio already used nearby. Example acceptable change for avatar wrapper:

```tsx
<div className="h-20 w-20 shrink-0 overflow-hidden rounded-full">
  <Image
    src={avatarUrl}
    alt={avatarAlt}
    width={80}
    height={80}
    className="h-20 w-20 object-cover"
  />
</div>
```

For stat cards/panels, reserve `min-h-*` only on containers that visibly shift in mobile evidence. Do not redesign profile page.

- [ ] **Step 4: Run frontend lint**

From `Frontend`:

```bash
npm run lint
```

Expected: PASS. No introduced `any`, no image-policy violation.

- [ ] **Step 5: Browser-check profile mobile**

Use mobile viewport on `/vi/profile`. Verify:

```text
- profile header stays stable while avatar/media load
- no visible overlap or jump above fold
- localized text still displays
- keyboard focus remains visible for touched controls
```

- [ ] **Step 6: Run targeted benchmark for profile CLS**

```bash
TAROTNOW_BENCHMARK_MODE=targeted-hotspots npm run benchmark:navigation
```

Expected: `/vi/profile` mobile CLS < `0.05`, target < `0.02`.

- [ ] **Step 7: Commit profile CLS fix**

```bash
git add Frontend/src/features/profile
git commit -m "fix: stabilize profile mobile layout"
```

---

### Task 6: Investigate and fix proven duplicate media rendering

**Files:**
- Inspect/modify: `Frontend/src/features/community/**`
- Inspect/modify: `Frontend/src/features/collection/**`
- Inspect/modify only if policy evidence requires: `Frontend/src/shared/http/assetUrl.ts`

- [ ] **Step 1: Extract duplicate media URLs from benchmark output**

Run:

```bash
grep -nE "media.tarotnow.xyz|img.tarotnow.xyz|Duplicate" PERFORMANCE-AUDIT.md | head -200
```

Expected: includes collection/community duplicate candidates from current audit.

- [ ] **Step 2: Trace community media mapping**

Run:

```bash
grep -RIn "mediaUrl\|mediaUrls\|attachments\|images\|shouldUseUnoptimizedImage\|<Image" Frontend/src/features/community | head -220
```

Expected: bounded list of feed/post media render paths.

- [ ] **Step 3: Trace collection media mapping and initial window**

Run:

```bash
grep -RIn "card-image\|variant=thumb\|light-god\|collection\|initial\|slice\|limit\|<Image" Frontend/src/features/collection Frontend/src/app/api/collection | head -240
```

Expected: bounded list of collection card image render and fetch/window logic.

- [ ] **Step 4: Decide duplicate owner before changing code**

Use this decision table:

```text
Same URL rendered twice by same list item/component instance -> FE mapping/render bug; fix at mapping boundary.
Same URL appears in two different content records -> Data owner; do not hide in FE.
Same logical image requested once as _next/image and once as direct CDN URL -> FE image policy/path mismatch; fix through existing assetUrl policy only.
Same URL repeated by browser cache validation but one is 304 -> CDN/cache evidence; do not dedupe FE.
```

- [ ] **Step 5: Add a focused regression test only for FE-owned duplicate**

If community mapping duplicates media within one post, add/extend the nearest component test:

```ts
it('renders each post media URL once', () => {
  render(<PostMediaGrid media={[{ url: mediaUrl }, { url: mediaUrl }]} />);
  expect(screen.getAllByRole('img')).toHaveLength(1);
});
```

Adapt component/import names to actual community media component.

If collection duplicates cards from data records, do not write this test; record Data/CDN owner in final summary.

- [ ] **Step 6: Implement minimal FE dedupe if proven**

At the mapping boundary, dedupe by normalized URL while preserving order:

```ts
const uniqueMedia = media.filter((item, index, items) => {
  const url = normalizeMediaUrl(item.url);
  return items.findIndex((candidate) => normalizeMediaUrl(candidate.url) === url) === index;
});
```

Use existing normalizer if available. If no normalizer exists, compare trimmed URL strings in the local component only. Do not create broad utilities for one component.

- [ ] **Step 7: Run targeted component tests and lint**

```bash
npm run test -- community
npm run test -- collection
npm run lint
```

Expected: relevant tests PASS; lint PASS.

- [ ] **Step 8: Browser-check community and collection**

Open `/vi/community` and `/vi/collection`. Verify:

```text
- media still visible
- no valid distinct content hidden
- initial collection still shows enough cards
- no broken image icons
```

- [ ] **Step 9: Run targeted benchmark transfer/duplicate check**

```bash
TAROTNOW_BENCHMARK_MODE=targeted-hotspots npm run benchmark:navigation
```

Expected: duplicate FE-owned media request group disappears or transfer drops; Data/CDN-owned duplicates are documented, not hidden.

- [ ] **Step 10: Commit media fix if code changed**

```bash
git add Frontend/src/features/community Frontend/src/features/collection Frontend/src/shared/http/assetUrl.ts
git commit -m "fix: reduce duplicate media rendering"
```

Stage only files actually changed.

---

### Task 7: Reduce measured chunk fan-out on worst routes

**Files:**
- Inspect/modify: `Frontend/src/app/[locale]/**`
- Inspect/modify: `Frontend/src/features/**/public.ts`
- Inspect/modify: worst-route feature components from benchmark chunk list

- [ ] **Step 1: Build current chunk hotspot list**

Run from `Frontend` after a production build exists:

```bash
npm run build
```

Then inspect benchmark request lists:

```bash
node -e "const fs=require('fs');const p='benchmark-results/benchmark/latest/tarotnow-benchmark.json';const data=JSON.parse(fs.readFileSync(p,'utf8'));for(const page of data.pages??[]){const chunks=(page.requests??[]).filter(r=>/\/_next\/static\/chunks\//.test(r.url)).length;if(chunks>=24) console.log(chunks, page.scenario, page.viewport, page.route)}"
```

Expected: `/vi/community`, `/vi/inventory`, `/vi/gacha`, `/vi/collection`, `/vi/profile`, reader/chat/admin candidates appear.

- [ ] **Step 2: Inspect imports for one route family at a time**

For each hotspot family, run a focused grep. Example for community:

```bash
grep -RIn "from '@/features/.*public'\|dynamic(\|next/dynamic\|lazy\|from '@/features/community\|from '@/features/chat\|from '@/features/admin" Frontend/src/app/[locale] Frontend/src/features/community Frontend/src/features/*/public.ts | head -240
```

Expected: identify broad public export or above-fold component importing non-critical widgets.

- [ ] **Step 3: Choose one safe lazy boundary**

Pick the first non-above-fold widget from measured hotspot routes using this priority:

```text
1. community sidebar/modals/composers below first content
2. inventory/gacha secondary panels below first content
3. profile/wallet secondary tabs below first screen
4. admin heavy widgets not needed for initial route
5. reader/chat non-critical panels after first message/list render
```

Do not lazy-load auth-critical gates, route layout shells, localization setup, or first meaningful content.

- [ ] **Step 4: Add dynamic import with loading placeholder**

Use existing route pattern. If none exists, use Next dynamic import:

```tsx
import dynamic from 'next/dynamic';

const SecondaryWidget = dynamic(() => import('./SecondaryWidget').then((mod) => mod.SecondaryWidget), {
  loading: () => <div className="min-h-24" aria-hidden="true" />,
});
```

Keep placeholder dimensions stable. Do not add suspense architecture unless nearby route already uses it.

- [ ] **Step 5: Run lint/build**

```bash
npm run lint
npm run build
```

Expected: PASS. If chunk count increases or build warns about invalid dynamic import, revert or choose smaller boundary.

- [ ] **Step 6: Browser-check changed routes**

Open each changed route. Verify:

```text
- first content appears
- lazy widget appears after load
- no layout jump from placeholder
- no console errors
```

- [ ] **Step 7: Run targeted benchmark chunk check**

```bash
TAROTNOW_BENCHMARK_MODE=targeted-hotspots npm run benchmark:navigation
```

Expected: changed hotspot route static chunk/request count drops or navigation/load improves. If no improvement, revert this lazy boundary and try one safer boundary before committing.

- [ ] **Step 8: Commit chunk reduction**

```bash
git add Frontend/src/app Frontend/src/features
git commit -m "perf: lazy load non-critical route widgets"
```

Stage only files actually changed.

---

### Task 8: Capture CDN and backend timing evidence

**Files:**
- Create if needed: `Frontend/scripts/probe-performance-headers.mjs`
- Modify only if trace headers already exist and behavior is safe: `Frontend/src/app/api/_shared/forwardUpstreamJsonWithTimeout.ts`
- Inspect: selected `Frontend/src/app/api/**/route.ts`

- [ ] **Step 1: Add local header probe script only if no equivalent exists**

Search first:

```bash
find Frontend/scripts -maxdepth 1 -type f -name "*header*" -o -name "*probe*"
```

If no equivalent exists, create `Frontend/scripts/probe-performance-headers.mjs`:

```js
const urls = [
  'https://www.tarotnow.xyz/',
  'https://www.tarotnow.xyz/themes/prismatic-royal.css',
  'https://www.tarotnow.xyz/_next/static/chunks/main-app.js',
  'https://media.tarotnow.xyz/community/2ef125e575c84b8d990e4c90ed64d5f3.webp',
  'https://img.tarotnow.xyz/light-god-50/',
];

for (const url of urls) {
  try {
    const response = await fetch(url, { method: 'HEAD' });
    console.log(JSON.stringify({
      url,
      status: response.status,
      cacheControl: response.headers.get('cache-control'),
      cfCacheStatus: response.headers.get('cf-cache-status'),
      contentType: response.headers.get('content-type'),
      contentLength: response.headers.get('content-length'),
      etag: response.headers.get('etag'),
      serverTiming: response.headers.get('server-timing'),
    }));
  } catch (error) {
    console.log(JSON.stringify({ url, error: error instanceof Error ? error.message : String(error) }));
  }
}
```

Replace sample media URLs with current URLs copied from `PERFORMANCE-AUDIT.md` before running.

- [ ] **Step 2: Run header probe**

```bash
node Frontend/scripts/probe-performance-headers.mjs
```

Expected: JSON lines for HTML, theme CSS, static chunks, media hosts. Missing/weak cache headers are evidence, not automatic source changes.

- [ ] **Step 3: Capture Server-Timing availability**

Probe slow HTML/API URLs from audit:

```bash
curl -I "https://www.tarotnow.xyz/vi/collection"
curl -I "https://www.tarotnow.xyz/api/auth/session?mode=lite"
```

Expected: headers show whether `server-timing`, cache, and trace IDs exist. If no timing headers exist, record “missing evidence” in final summary.

- [ ] **Step 4: Add proxy timing only if source already supports trace propagation**

Inspect `forwardUpstreamJsonWithTimeout.ts` and selected route handlers. If they already have trace/header utilities, add timing fields using existing helper style only. Example shape:

```ts
response.headers.set('Server-Timing', `upstream;dur=${Math.round(upstreamDurationMs)}`);
```

Do not invent new backend tracing infrastructure in this plan. Do not expose sensitive tokens or upstream URLs.

- [ ] **Step 5: Run API route tests if timing source changed**

```bash
npm run test -- app/api
npm run lint
```

Expected: PASS. Response body unchanged; only safe headers added.

- [ ] **Step 6: Commit evidence tooling or timing header change**

If only script added:

```bash
git add Frontend/scripts/probe-performance-headers.mjs
git commit -m "chore: add performance header probe"
```

If timing headers changed:

```bash
git add Frontend/src/app/api Frontend/scripts/probe-performance-headers.mjs
git commit -m "chore: expose frontend proxy timing evidence"
```

---

### Task 9: Run full verification and regenerate audit

**Files:**
- Regenerate: `PERFORMANCE-AUDIT.md`
- Inspect: `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json`
- Inspect: `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark-hotspots.json`

- [ ] **Step 1: Run focused frontend checks**

From `Frontend`:

```bash
npm run lint
npm run build
```

Expected: PASS.

- [ ] **Step 2: Run relevant unit tests**

```bash
npm run test -- clientSessionSnapshot.test.ts usePresenceConnection.test.tsx
```

Add community/profile/collection test files to the command if Tasks 5 or 6 changed those components.

Expected: PASS.

- [ ] **Step 3: Browser spot-check changed UI flows**

Check these routes in browser with desktop and mobile where relevant:

```text
/vi/profile
/vi/community
/vi/collection
/vi/inventory
/vi/gacha
```

Expected: no broken UI, no missing first content, no console errors from changed lazy/media/profile logic.

- [ ] **Step 4: Run targeted hotspot benchmark**

```bash
TAROTNOW_BENCHMARK_MODE=targeted-hotspots npm run benchmark:navigation
```

Expected:

```text
- /vi/profile mobile CLS < 0.05, target < 0.02
- duplicate presence negotiate removed or outside initial benchmark window
- same-site media categorized separately
- no failed/pending request regression
```

- [ ] **Step 5: Run full production benchmark**

```bash
TAROTNOW_BENCHMARK_MODE=full-matrix npm run benchmark:navigation
```

Expected:

```text
- total pages measured remains 170 unless route matrix intentionally changed
- no critical pages introduced
- worst route static chunk count drops from current 24/33 where touched
- slow >800ms count decreases from baseline 50 if backend/CDN variance allows
```

- [ ] **Step 6: Regenerate audit report**

```bash
node Frontend/scripts/generate-performance-audit.mjs
```

Expected: `PERFORMANCE-AUDIT.md` includes updated generated timestamp, same-site media classification, owner summary, and changed benchmark metrics.

- [ ] **Step 7: Inspect final diff**

```bash
git status --short
git diff --stat
git diff -- PERFORMANCE-AUDIT.md Frontend/tests/tarotnow-navigation-benchmark.spec.ts Frontend/scripts/generate-performance-audit.mjs
```

Expected: only intended source/report changes. No secrets, local env files, or unrelated generated files staged.

- [ ] **Step 8: Commit final audit update**

```bash
git add PERFORMANCE-AUDIT.md Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark-hotspots.json
git commit -m "chore: refresh performance audit after bottleneck fixes"
```

If benchmark result files are intentionally ignored or not tracked, stage only `PERFORMANCE-AUDIT.md`.

---

## Final Acceptance Checklist

- [ ] Same-site media hosts classify separately from true third-party.
- [ ] Audit report shows owner summary by route family.
- [ ] Auth session full→lite dedupe tests pass; no auth TTL/no-store relaxation.
- [ ] Presence startup is delayed and at most one active connection attempt occurs per route lifetime.
- [ ] `/vi/profile` mobile CLS is below `0.05`, target below `0.02`.
- [ ] FE-owned duplicate media render bugs are fixed; Data/CDN-owned duplicates are documented, not hidden.
- [ ] Worst touched route families have lower request/static chunk/transfer burden or documented evidence explaining why not.
- [ ] CDN/backend actions are evidence capture only unless headers/traces prove a safe code change.
- [ ] `npm run lint`, `npm run build`, relevant tests, targeted hotspot benchmark, and full production benchmark complete.
- [ ] `PERFORMANCE-AUDIT.md` regenerated from script, not hand-edited.
