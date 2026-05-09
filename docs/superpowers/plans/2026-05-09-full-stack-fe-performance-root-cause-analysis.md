# Full-Stack FE Performance Root-Cause Analysis Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Produce a detailed full-stack root-cause report explaining why TarotNow frontend feels slow, with evidence mapped to FE, backend, CDN, infra, and data ownership.

**Architecture:** This is an analysis-only workflow. Gather evidence from production benchmark artifacts, FE source, Next.js build artifacts, API/proxy/session/realtime paths, and CDN/cache headers, then write a single root-cause report. No production code changes are allowed in this phase.

**Tech Stack:** Next.js App Router, React, TypeScript, TanStack Query, Playwright benchmark artifacts, Node.js analysis scripts, GitHub Actions deployment history, Cloudflare/CDN headers.

---

## File Structure

**Create:**
- `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md` — final report with executive summary, evidence tables, root-cause tree, and fix roadmap.

**Read only:**
- `PERFORMANCE-AUDIT.md` — generated audit baseline.
- `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json` — full-matrix benchmark artifact.
- `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark-hotspots.json` — targeted hotspot benchmark artifact.
- `Frontend/tests/tarotnow-navigation-benchmark.spec.ts` — benchmark methodology, thresholds, route sets, classification behavior.
- `Frontend/src/app/_shared/app-shell/common/AppQueryProvider.tsx` — root client providers and app shell cost.
- `Frontend/src/app/_shared/providers/PresenceProvider.tsx` — presence enablement policy.
- `Frontend/src/app/_shared/hooks/usePresenceConnection.ts` — SignalR startup timing and negotiation behavior.
- `Frontend/src/features/auth/shared/auth/clientSessionSnapshot.ts` — full/lite session snapshot behavior.
- `Frontend/src/shared/navigation/normalizePathname.ts` — realtime/authless route rules.
- `Frontend/src/shared/http/assetUrl.ts` — image optimizer bypass policy.
- `Frontend/src/features/community/post/components/post-card/PostCardContent.tsx` — community image rendering.
- `Frontend/src/features/inventory/browse/InventoryItemCard.tsx` — inventory icon rendering.
- `Frontend/src/features/collection/cards/components/deck-card/CollectionDeckCardVisual.tsx` — collection image rendering.
- `Frontend/src/features/gacha/result/GachaResultItem.tsx` — gacha reward image rendering.
- `Frontend/src/app/api/auth/session/sessionRouteHandler.ts` — session route full/lite behavior.
- `Frontend/src/proxy.ts` — CSP, Cloudflare, static/proxy behavior.
- `Frontend/package.json` — available scripts.

**Do not modify:**
- Frontend runtime source files.
- Backend source files.
- GitHub Actions workflows.
- Benchmark code.

---

### Task 1: Build measurement baseline from artifacts

**Files:**
- Read: `PERFORMANCE-AUDIT.md`
- Read: `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json`
- Read: `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark-hotspots.json`
- Create later: `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`

- [ ] **Step 1: Verify artifact timestamps and mode**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
for (const path of [
  'Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json',
  'Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark-hotspots.json',
]) {
  if (!fs.existsSync(path)) {
    console.log(`${path}\tmissing`);
    continue;
  }
  const artifact = JSON.parse(fs.readFileSync(path, 'utf8'));
  const stat = fs.statSync(path);
  console.log(`${path}\tmtime=${stat.mtime.toISOString()}\tgenerated=${artifact.generatedAtUtc}\tmode=${artifact.benchmarkMode}\torigin=${artifact.baseOrigin}`);
}
NODE
```
Expected: Each artifact line prints `generated`, `mode`, and `origin`. If a file is missing, record that as missing evidence in the final report.

- [ ] **Step 2: Extract scenario and route severity baseline**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const path = 'Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json';
const artifact = JSON.parse(fs.readFileSync(path, 'utf8'));
const pages = artifact.scenarios.flatMap((scenario) =>
  (scenario.pages || []).map((page) => ({
    scenario: scenario.scenario,
    viewport: scenario.viewport,
    route: page.route,
    feature: page.feature,
    requestCount: page.requestCount,
    navigationMs: page.navigationMs,
    transferBytes: page.transferBytes,
    fcpMs: page.fcpMs,
    lcpMs: page.lcpMs,
    tbtMs: page.tbtMs,
    cls: page.cls,
    api: page.requestBreakdown?.api ?? 0,
    static: page.requestBreakdown?.static ?? 0,
    thirdParty: page.requestBreakdown?.thirdParty ?? 0,
    telemetry: page.requestBreakdown?.telemetry ?? 0,
  })),
);
const critical = pages.filter((page) => page.requestCount > 35);
const high = pages.filter((page) => page.requestCount > 25 && page.requestCount <= 35);
console.log(JSON.stringify({
  generatedAtUtc: artifact.generatedAtUtc,
  baseOrigin: artifact.baseOrigin,
  totalPages: pages.length,
  criticalPages: critical.length,
  highPages: high.length,
  topByRequests: pages.sort((a, b) => b.requestCount - a.requestCount || b.navigationMs - a.navigationMs).slice(0, 20),
}, null, 2));
NODE
```
Expected: JSON summary with `totalPages`, `criticalPages`, `highPages`, and `topByRequests`.

- [ ] **Step 3: Extract slow request baseline**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const artifact = JSON.parse(fs.readFileSync('Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json', 'utf8'));
const requests = artifact.scenarios.flatMap((scenario) =>
  (scenario.pages || []).flatMap((page) =>
    (page.requests || []).map((request) => ({
      scenario: scenario.scenario,
      viewport: scenario.viewport,
      route: page.route,
      feature: page.feature,
      method: request.method,
      status: request.status,
      category: request.category,
      durationMs: request.durationMs,
      transferBytes: request.transferBytes,
      cacheControl: request.cacheControl,
      cacheIssue: request.cacheIssue,
      isDuplicate: request.isDuplicate,
      url: request.url,
    })),
  ),
);
console.log(JSON.stringify({
  slowOver800: requests.filter((request) => request.durationMs > 800).length,
  slow400To800: requests.filter((request) => request.durationMs > 400 && request.durationMs <= 800).length,
  topSlowRequests: requests.sort((a, b) => b.durationMs - a.durationMs).slice(0, 30),
  topTransferRequests: requests.sort((a, b) => (b.transferBytes || 0) - (a.transferBytes || 0)).slice(0, 30),
}, null, 2));
NODE
```
Expected: JSON summary with slow request counts and top slow/transfer requests.

- [ ] **Step 4: Record baseline findings in notes**

Create a working notes section in the final report draft with:
```markdown
## Measurement Baseline

- Full artifact generated at: <value>
- Base origin: <value>
- Total pages: <value>
- Critical pages: <value>
- High pages: <value>
- Slow requests >800ms: <value>
- Slow requests 400-800ms: <value>

### Top request-count routes
| Requests | Nav ms | Transfer bytes | Scenario | Viewport | Route | API | Static | Third-party | Telemetry |
| ---: | ---: | ---: | --- | --- | --- | ---: | ---: | ---: | ---: |
```
Expected: Draft has exact numbers from Steps 2-3, not estimates.

---

### Task 2: Diagnose route families

**Files:**
- Read: `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json`
- Write: `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`

- [ ] **Step 1: Generate route-family summary**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const artifact = JSON.parse(fs.readFileSync('Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json', 'utf8'));
const familyOf = (route) => {
  if (route.includes('/admin')) return 'admin';
  if (route.includes('/community') || route.includes('/leaderboard') || route.includes('/gamification')) return 'community-leaderboard-quest';
  if (route.includes('/inventory') || route.includes('/gacha') || route.includes('/collection')) return 'inventory-gacha-collection';
  if (route.includes('/reading')) return 'reading';
  if (route.includes('/profile') || route.includes('/wallet') || route.includes('/notifications')) return 'profile-wallet-notifications';
  if (route.includes('/chat') || route.includes('/readers') || route.includes('/reader/apply')) return 'reader-chat';
  if (route.includes('/login') || route.includes('/register') || route.includes('/forgot-password') || route.includes('/reset-password') || route.includes('/verify-email') || route.includes('/legal') || route === '/vi') return 'home-auth-legal';
  return 'other';
};
const rows = new Map();
for (const scenario of artifact.scenarios) {
  for (const page of scenario.pages || []) {
    const family = familyOf(page.route);
    const key = `${scenario.scenario}\t${scenario.viewport}\t${family}`;
    const row = rows.get(key) || { scenario: scenario.scenario, viewport: scenario.viewport, family, pages: 0, requests: 0, nav: 0, transfer: 0, api: 0, static: 0, thirdParty: 0, telemetry: 0 };
    row.pages += 1;
    row.requests += page.requestCount;
    row.nav += page.navigationMs;
    row.transfer += page.transferBytes || 0;
    row.api += page.requestBreakdown?.api || 0;
    row.static += page.requestBreakdown?.static || 0;
    row.thirdParty += page.requestBreakdown?.thirdParty || 0;
    row.telemetry += page.requestBreakdown?.telemetry || 0;
    rows.set(key, row);
  }
}
for (const row of [...rows.values()].sort((a, b) => (b.requests / b.pages) - (a.requests / a.pages))) {
  console.log(`${row.scenario}\t${row.viewport}\t${row.family}\tpages=${row.pages}\tavgReq=${(row.requests / row.pages).toFixed(1)}\tavgNav=${(row.nav / row.pages).toFixed(0)}\tavgTransfer=${Math.round(row.transfer / row.pages)}\tapi=${row.api}\tstatic=${row.static}\tthird=${row.thirdParty}\ttelemetry=${row.telemetry}`);
}
NODE
```
Expected: TSV summary grouped by scenario, viewport, and family.

- [ ] **Step 2: Classify each route family**

Add this table to the report and fill from Step 1:
```markdown
## Route Family Diagnosis

| Family | Main symptom | Evidence | Dominant category | Likely owner | Root-cause class | Fix direction |
| --- | --- | --- | --- | --- | --- | --- |
| home-auth-legal | <exact symptom> | <numbers> | <category> | <owner> | <class> | <fix> |
| community-leaderboard-quest | <exact symptom> | <numbers> | <category> | <owner> | <class> | <fix> |
| inventory-gacha-collection | <exact symptom> | <numbers> | <category> | <owner> | <class> | <fix> |
| reading | <exact symptom> | <numbers> | <category> | <owner> | <class> | <fix> |
| profile-wallet-notifications | <exact symptom> | <numbers> | <category> | <owner> | <class> | <fix> |
| reader-chat | <exact symptom> | <numbers> | <category> | <owner> | <class> | <fix> |
| admin | <exact symptom> | <numbers> | <category> | <owner> | <class> | <fix> |
```
Expected: Every row names owner as `FE`, `Backend`, `CDN`, `Infra`, `Data`, or `Mixed`.

---

### Task 3: Analyze browser waterfall and request classification

**Files:**
- Read: `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json`
- Read: `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark-hotspots.json`
- Read: `Frontend/tests/tarotnow-navigation-benchmark.spec.ts`
- Write: `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`

- [ ] **Step 1: Extract duplicate requests and duplicate media**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const artifact = JSON.parse(fs.readFileSync('Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json', 'utf8'));
for (const scenario of artifact.scenarios) {
  for (const page of scenario.pages || []) {
    const counts = new Map();
    for (const request of page.requests || []) {
      const key = `${request.method} ${request.url}`;
      counts.set(key, (counts.get(key) || 0) + 1);
    }
    const duplicates = [...counts.entries()].filter(([, count]) => count > 1);
    if (duplicates.length > 0) {
      console.log(`\n${scenario.scenario}\t${scenario.viewport}\t${page.route}\trequests=${page.requestCount}`);
      for (const [key, count] of duplicates.slice(0, 10)) console.log(`${count}\t${key}`);
    }
  }
}
NODE
```
Expected: Duplicate request groups, or no output if none.

- [ ] **Step 2: Extract per-route host/category breakdown for top routes**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const artifact = JSON.parse(fs.readFileSync('Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json', 'utf8'));
const pages = artifact.scenarios.flatMap((scenario) => (scenario.pages || []).map((page) => ({ scenario, page })));
for (const { scenario, page } of pages.sort((a, b) => b.page.requestCount - a.page.requestCount).slice(0, 12)) {
  const groups = new Map();
  for (const request of page.requests || []) {
    const url = new URL(request.url);
    const key = `${request.category}\t${url.hostname}`;
    groups.set(key, (groups.get(key) || 0) + 1);
  }
  console.log(`\n${scenario.scenario}\t${scenario.viewport}\t${page.route}\trequests=${page.requestCount}`);
  for (const [key, count] of [...groups.entries()].sort((a, b) => b[1] - a[1])) console.log(`${count}\t${key}`);
}
NODE
```
Expected: Host/category grouping showing when `www.tarotnow.xyz` is counted as third-party because base origin is `tarotnow.xyz`.

- [ ] **Step 3: Read benchmark classification logic**

Read `Frontend/tests/tarotnow-navigation-benchmark.spec.ts` around request category and base origin logic.
Expected findings to record:
- How `BASE_ORIGIN` is chosen.
- How categories are assigned.
- Whether cross-host same-site assets are counted as `third-party`.
- How duplicate keys are normalized.

- [ ] **Step 4: Write browser waterfall findings**

Add to report:
```markdown
## Browser Waterfall and Classification Findings

### True waste
- <duplicate API/media if found>

### Classification artifacts
- <same-site host mismatch findings>

### Route waterfall pattern
- <JS chunk waterfall/static fan-out findings>

### Telemetry/realtime impact
- <Cloudflare RUM and SignalR findings>
```
Expected: Section distinguishes real FE waste from measurement/category artifacts.

---

### Task 4: Analyze Next.js/frontend architecture root causes

**Files:**
- Read: `Frontend/src/app/_shared/app-shell/common/AppQueryProvider.tsx`
- Read: `Frontend/src/app/[locale]/(user)/layout.tsx`
- Read: `Frontend/src/app/[locale]/(user)/community/page.tsx`
- Read: `Frontend/src/app/[locale]/(user)/inventory/page.tsx`
- Read: `Frontend/src/app/[locale]/(user)/gacha/page.tsx`
- Read: `Frontend/src/shared/navigation/useOptimizedLink.tsx`
- Read: `Frontend/package.json`
- Write: `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`

- [ ] **Step 1: Locate shared providers and hydration boundaries**

Run:
```bash
grep -R "AppQueryProvider\|PresenceProvider\|HydrationBoundary\|dehydrateAppQueries\|useOptimizedLink" -n Frontend/src/app Frontend/src/features Frontend/src/shared | head -160
```
Expected: Source paths showing shared app providers, route-level hydration, and optimized link behavior.

- [ ] **Step 2: Inspect static chunk fan-out from benchmark**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const artifact = JSON.parse(fs.readFileSync('Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json', 'utf8'));
const chunkCounts = new Map();
for (const scenario of artifact.scenarios) {
  for (const page of scenario.pages || []) {
    const chunks = (page.requests || []).filter((request) => {
      try {
        const url = new URL(request.url);
        return url.pathname.startsWith('/_next/static/chunks/') && url.pathname.endsWith('.js');
      } catch {
        return false;
      }
    });
    chunkCounts.set(`${scenario.scenario}\t${scenario.viewport}\t${page.route}`, chunks.length);
  }
}
for (const [route, count] of [...chunkCounts.entries()].sort((a, b) => b[1] - a[1]).slice(0, 40)) {
  console.log(`${count}\t${route}`);
}
NODE
```
Expected: Routes with highest JS chunk counts.

- [ ] **Step 3: Inspect available bundle/build scripts**

Run:
```bash
node -e "const pkg=require('./Frontend/package.json'); console.log(JSON.stringify(pkg.scripts, null, 2));"
```
Expected: Print scripts. If bundle analyzer is unavailable, record that bundle attribution is limited to `.next` artifacts and benchmark request sizes.

- [ ] **Step 4: Write frontend architecture findings**

Add to report:
```markdown
## Next.js and Frontend Architecture Findings

- Shared providers loaded across protected routes: <paths and effect>
- Hydration boundary pattern: <paths and effect>
- Static chunk fan-out: <evidence>
- Prefetch/navigation behavior: <evidence>
- Main FE root cause: <summary>
```
Expected: Concrete file paths and evidence, not generic framework advice.

---

### Task 5: Analyze auth/session/realtime root causes

**Files:**
- Read: `Frontend/src/features/auth/shared/auth/clientSessionSnapshot.ts`
- Read: `Frontend/src/features/auth/shared/hooks/useAuth.ts`
- Read: `Frontend/src/features/auth/session/components/AuthSessionManager.ts`
- Read: `Frontend/src/app/_shared/providers/PresenceProvider.tsx`
- Read: `Frontend/src/app/_shared/hooks/usePresenceConnection.ts`
- Read: `Frontend/src/app/api/auth/session/sessionRouteHandler.ts`
- Read: `Frontend/src/shared/navigation/normalizePathname.ts`
- Write: `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`

- [ ] **Step 1: Extract auth/session and presence request counts**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const artifact = JSON.parse(fs.readFileSync('Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json', 'utf8'));
for (const scenario of artifact.scenarios) {
  for (const page of scenario.pages || []) {
    const authSession = [];
    const presence = [];
    for (const request of page.requests || []) {
      const url = new URL(request.url);
      if (url.pathname === '/api/auth/session') authSession.push(`${url.search || '(full)'} ${Math.round(request.durationMs)}ms`);
      if (url.pathname === '/api/v1/presence/negotiate') presence.push(`${Math.round(request.durationMs)}ms`);
    }
    if (authSession.length > 0 || presence.length > 0) {
      console.log(`${scenario.scenario}\t${scenario.viewport}\t${page.route}\tsession=${authSession.join('|') || '-'}\tpresence=${presence.join('|') || '-'}`);
    }
  }
}
NODE
```
Expected: Route-level session and presence timing rows.

- [ ] **Step 2: Inspect security-sensitive session behavior**

Read `Frontend/src/app/api/auth/session/sessionRouteHandler.ts` and record:
- Full mode returns user profile.
- Lite mode validates access token without returning profile.
- Refresh path may set cookies.
- Unsafe caching is not acceptable for auth freshness.

- [ ] **Step 3: Inspect realtime startup behavior**

Read `Frontend/src/app/_shared/hooks/usePresenceConnection.ts` and record:
- Startup delay.
- Session guard.
- SignalR import and negotiate path.
- Reconnect cooldown behavior.

- [ ] **Step 4: Write auth/realtime root cause findings**

Add to report:
```markdown
## Auth, Session, and Realtime Findings

| Finding | Evidence | Owner | Safe fix | Unsafe fix to avoid |
| --- | --- | --- | --- | --- |
| Session checks add request cost | <routes/timings> | FE/Backend | dedupe full/lite, avoid duplicate bootstrap | long stale auth cache |
| Presence negotiation competes with initial load on some flows | <routes/timings> | FE/Backend | defer/connect on demand/route policy | removing realtime entirely |
```
Expected: Findings preserve auth security constraints.

---

### Task 6: Analyze image/media root causes

**Files:**
- Read: `Frontend/src/shared/http/assetUrl.ts`
- Read: `Frontend/src/features/community/post/components/post-card/PostCardContent.tsx`
- Read: `Frontend/src/features/inventory/browse/InventoryItemCard.tsx`
- Read: `Frontend/src/features/collection/cards/components/deck-card/CollectionDeckCardVisual.tsx`
- Read: `Frontend/src/features/gacha/result/GachaResultItem.tsx`
- Read: `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json`
- Write: `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`

- [ ] **Step 1: Extract image requests by host/path**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const artifact = JSON.parse(fs.readFileSync('Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json', 'utf8'));
const images = [];
for (const scenario of artifact.scenarios) {
  for (const page of scenario.pages || []) {
    for (const request of page.requests || []) {
      const url = new URL(request.url);
      if (url.pathname.startsWith('/_next/image') || url.hostname === 'media.tarotnow.xyz' || url.hostname === 'img.tarotnow.xyz') {
        images.push({
          scenario: scenario.scenario,
          viewport: scenario.viewport,
          route: page.route,
          durationMs: request.durationMs,
          transferBytes: request.transferBytes,
          cacheControl: request.cacheControl,
          url: request.url,
        });
      }
    }
  }
}
console.log(JSON.stringify({
  totalImageRequests: images.length,
  topByTransfer: images.sort((a, b) => (b.transferBytes || 0) - (a.transferBytes || 0)).slice(0, 30),
  topByDuration: images.sort((a, b) => b.durationMs - a.durationMs).slice(0, 30),
}, null, 2));
NODE
```
Expected: Image request count and top images by transfer/duration.

- [ ] **Step 2: Extract duplicate media URLs**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const artifact = JSON.parse(fs.readFileSync('Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json', 'utf8'));
for (const scenario of artifact.scenarios) {
  for (const page of scenario.pages || []) {
    const counts = new Map();
    for (const request of page.requests || []) {
      const url = new URL(request.url);
      if (url.hostname === 'media.tarotnow.xyz' || url.hostname === 'img.tarotnow.xyz') {
        counts.set(url.href, (counts.get(url.href) || 0) + 1);
      }
    }
    const duplicates = [...counts.entries()].filter(([, count]) => count > 1);
    if (duplicates.length > 0) {
      console.log(`\n${scenario.scenario}\t${scenario.viewport}\t${page.route}`);
      for (const [url, count] of duplicates) console.log(`${count}\t${url}`);
    }
  }
}
NODE
```
Expected: Duplicate CDN media rows or no output.

- [ ] **Step 3: Inspect image rendering policy**

Read listed image files and record:
- Which components use `unoptimized`.
- Which components specify `sizes`.
- Which components use `priority` or eager loading.
- Which hosts bypass Next optimizer.

- [ ] **Step 4: Write image/media findings**

Add to report:
```markdown
## Image and Media Findings

| Area | Evidence | Root cause | Owner | Fix direction |
| --- | --- | --- | --- | --- |
| Community images | <URLs/timings/duplicates> | <cause> | FE/Data/CDN | <fix> |
| Inventory icons | <URLs/timings> | <cause> | FE/CDN | <fix> |
| Collection cards | <URLs/timings> | <cause> | FE/CDN | <fix> |
| Gacha media | <URLs/timings> | <cause> | FE/CDN | <fix> |
```
Expected: Image findings separate optimizer issues from content duplication and CDN latency.

---

### Task 7: Analyze backend/API/proxy root causes

**Files:**
- Read: `Frontend/src/app/api/auth/session/sessionRouteHandler.ts`
- Read: `Frontend/src/app/api/auth/refresh/refreshRouteHandler.ts`
- Read: `Frontend/src/features/community/shared/actions/communityActions.ts`
- Read: `Frontend/src/features/inventory/shared` files as needed
- Read: `Frontend/src/features/gacha/shared` files as needed
- Read: `Frontend/src/shared/http` files as needed
- Write: `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`

- [ ] **Step 1: Extract API requests over 400ms**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const artifact = JSON.parse(fs.readFileSync('Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json', 'utf8'));
const apiRequests = [];
for (const scenario of artifact.scenarios) {
  for (const page of scenario.pages || []) {
    for (const request of page.requests || []) {
      const url = new URL(request.url);
      if ((request.category === 'api' || url.pathname.startsWith('/api/')) && request.durationMs > 400) {
        apiRequests.push({
          scenario: scenario.scenario,
          viewport: scenario.viewport,
          route: page.route,
          method: request.method,
          status: request.status,
          durationMs: request.durationMs,
          transferBytes: request.transferBytes,
          url: request.url,
        });
      }
    }
  }
}
console.log(JSON.stringify(apiRequests.sort((a, b) => b.durationMs - a.durationMs).slice(0, 60), null, 2));
NODE
```
Expected: Slow API list, or empty list if none.

- [ ] **Step 2: Map slow APIs to frontend source paths**

Run:
```bash
grep -R "api/auth/session\|api/v1/presence\|community/feed\|collection\|gacha\|inventory\|wallet\|reading" -n Frontend/src/app Frontend/src/features Frontend/src/shared | head -220
```
Expected: Source paths that initiate or proxy suspect API requests.

- [ ] **Step 3: Identify where evidence ends**

For each slow API, classify:
- Browser-to-Next visible only.
- Next route handler code visible.
- Backend upstream hidden from current repo evidence.
- Needs backend logs/APM/API probe before final blame.

- [ ] **Step 4: Write backend/API findings**

Add to report:
```markdown
## Backend/API and Proxy Findings

| Endpoint | Slow evidence | Visible code path | Current confidence | Missing evidence | Owner |
| --- | --- | --- | --- | --- | --- |
```
Expected: No endpoint is blamed on backend unless current evidence supports it.

---

### Task 8: Analyze CDN/cache/domain topology root causes

**Files:**
- Read: `Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json`
- Read: `Frontend/src/proxy.ts`
- Write: `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`

- [ ] **Step 1: Extract cache issues and headers**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const artifact = JSON.parse(fs.readFileSync('Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json', 'utf8'));
const requests = artifact.scenarios.flatMap((scenario) =>
  (scenario.pages || []).flatMap((page) =>
    (page.requests || []).map((request) => ({
      scenario: scenario.scenario,
      viewport: scenario.viewport,
      route: page.route,
      category: request.category,
      transferBytes: request.transferBytes,
      durationMs: request.durationMs,
      cacheControl: request.cacheControl,
      cacheIssue: request.cacheIssue,
      url: request.url,
    })),
  ),
);
const cacheIssues = requests.filter((request) => request.cacheIssue);
console.log(JSON.stringify(cacheIssues.sort((a, b) => (b.transferBytes || 0) - (a.transferBytes || 0)).slice(0, 60), null, 2));
NODE
```
Expected: Cache issue rows or empty list.

- [ ] **Step 2: Extract host topology**

Run:
```bash
node - <<'NODE'
const fs = require('fs');
const artifact = JSON.parse(fs.readFileSync('Frontend/benchmark-results/benchmark/latest/tarotnow-benchmark.json', 'utf8'));
const hosts = new Map();
for (const scenario of artifact.scenarios) {
  for (const page of scenario.pages || []) {
    for (const request of page.requests || []) {
      const url = new URL(request.url);
      const row = hosts.get(url.hostname) || { requests: 0, transfer: 0, slowOver800: 0, categories: new Map() };
      row.requests += 1;
      row.transfer += request.transferBytes || 0;
      if (request.durationMs > 800) row.slowOver800 += 1;
      row.categories.set(request.category, (row.categories.get(request.category) || 0) + 1);
      hosts.set(url.hostname, row);
    }
  }
}
for (const [host, row] of [...hosts.entries()].sort((a, b) => b[1].requests - a[1].requests)) {
  console.log(`${host}\trequests=${row.requests}\ttransfer=${row.transfer}\tslowOver800=${row.slowOver800}\tcategories=${JSON.stringify(Object.fromEntries(row.categories))}`);
}
NODE
```
Expected: Host summary for `tarotnow.xyz`, `www.tarotnow.xyz`, media hosts, Cloudflare hosts.

- [ ] **Step 3: Probe public CDN headers for representative URLs**

Run:
```bash
node - <<'NODE'
const https = require('https');
const urls = [
  'https://tarotnow.xyz/vi',
  'https://www.tarotnow.xyz/themes/prismatic-royal.css',
  'https://static.cloudflareinsights.com/beacon.min.js',
];
function head(url) {
  return new Promise((resolve) => {
    const req = https.request(url, { method: 'HEAD' }, (res) => {
      resolve({ url, status: res.statusCode, cacheControl: res.headers['cache-control'], cfCacheStatus: res.headers['cf-cache-status'], contentType: res.headers['content-type'] });
    });
    req.on('error', (error) => resolve({ url, error: error.message }));
    req.setTimeout(10000, () => { req.destroy(new Error('timeout')); });
    req.end();
  });
}
Promise.all(urls.map(head)).then((rows) => console.log(JSON.stringify(rows, null, 2)));
NODE
```
Expected: Header summary. If a URL blocks HEAD or times out, record as inconclusive.

- [ ] **Step 4: Write CDN/cache/domain findings**

Add to report:
```markdown
## CDN, Cache, and Domain Topology Findings

| Host | Role | Evidence | Root cause implication | Owner | Fix direction |
| --- | --- | --- | --- | --- | --- |
| tarotnow.xyz | canonical app domain | <numbers> | <implication> | Infra/CDN | <fix> |
| www.tarotnow.xyz | asset/API/app host | <numbers> | <implication> | Infra/CDN/FE | <fix> |
| media.tarotnow.xyz | community/icon CDN | <numbers> | <implication> | CDN/Data/FE | <fix> |
| static.cloudflareinsights.com | RUM | <numbers> | <implication> | Infra | <fix> |
```
Expected: Section explains host mismatch and cache behavior.

---

### Task 9: Write root-cause tree and roadmap

**Files:**
- Write: `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`

- [ ] **Step 1: Build root-cause tree table**

Add this table to report:
```markdown
## Root-Cause Tree

| Symptom | Evidence | Root cause | Owner | Confidence | Fix class | Expected impact | Risk |
| --- | --- | --- | --- | --- | --- | --- | --- |
| High request count across most protected routes | <numbers> | <cause> | <owner> | <High/Medium/Low> | <fix class> | <impact> | <risk> |
| Slow desktop community route | <numbers> | <cause> | <owner> | <High/Medium/Low> | <fix class> | <impact> | <risk> |
| Slow collection navigation | <numbers> | <cause> | <owner> | <High/Medium/Low> | <fix class> | <impact> | <risk> |
| Home TBT/LCP issue | <numbers> | <cause> | <owner> | <High/Medium/Low> | <fix class> | <impact> | <risk> |
| Session/realtime overhead | <numbers> | <cause> | <owner> | <High/Medium/Low> | <fix class> | <impact> | <risk> |
| Media transfer/duplicates | <numbers> | <cause> | <owner> | <High/Medium/Low> | <fix class> | <impact> | <risk> |
| CDN/domain classification and connection overhead | <numbers> | <cause> | <owner> | <High/Medium/Low> | <fix class> | <impact> | <risk> |
```
Expected: Every row has evidence and confidence.

- [ ] **Step 2: Build prioritized roadmap**

Add:
```markdown
## Prioritized Fix Roadmap

### P0: Measure before changing
1. <exact measurement>

### P1: High-impact low-risk FE fixes
1. <fix>

### P2: Backend/CDN fixes requiring owner confirmation
1. <fix>

### P3: Medium-impact cleanup
1. <fix>

### Not recommended now
1. <change> — rejected because <risk>
```
Expected: Roadmap does not recommend unsafe auth/session cache changes.

- [ ] **Step 3: Add verification plan**

Add:
```markdown
## Verification Plan

| Proposed fix class | Verification command or probe | Success threshold |
| --- | --- | --- |
| FE startup/request reduction | `ALLOW_PRODUCTION_BENCHMARK=true RUN_NAVIGATION_BENCHMARK_TARGETED=true BENCHMARK_BASE_ORIGIN=https://tarotnow.xyz npx playwright test tests/tarotnow-navigation-benchmark.spec.ts --project=chromium --grep "targeted verification"` | Hotspot request count decreases without failed/pending requests |
| Full-site regression check | `ALLOW_PRODUCTION_BENCHMARK=true RUN_NAVIGATION_BENCHMARK=true BENCHMARK_BASE_ORIGIN=https://tarotnow.xyz npx playwright test tests/tarotnow-navigation-benchmark.spec.ts --project=chromium --grep "full vi route matrix"` | No increase in critical/high pages |
| Build correctness | `NEXT_PUBLIC_API_URL=https://www.tarotnow.xyz NEXT_PUBLIC_BASE_URL=https://www.tarotnow.xyz npm run build` from `Frontend/` | Build exits 0 |
| API latency confirmation | Node HTTPS probe or backend logs for suspect endpoints | p95 under agreed target |
| CDN/cache confirmation | HEAD/GET header probe for static/media assets | cache headers match expected policy |
```
Expected: Verification section names commands and thresholds.

---

### Task 10: Self-review and commit report

**Files:**
- Read: `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`
- Commit: report only

- [ ] **Step 1: Scan for placeholders**

Run:
```bash
grep -n "TBD\|TODO\|<value>\|<numbers>\|<cause>\|<owner>\|<fix>\|<impact>\|<risk>" docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md || true
```
Expected: No output. If output appears, replace placeholders with evidence or `Missing evidence: ...` wording.

- [ ] **Step 2: Confirm no source code changes**

Run:
```bash
git status --short
```
Expected: Only `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md` is modified/untracked before commit. If source files changed, stop and revert those accidental edits before committing.

- [ ] **Step 3: Commit report**

Run:
```bash
git add -f docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md
git commit -m "$(cat <<'EOF'
docs: analyze full-stack frontend performance root causes

Map current frontend slowness to browser, Next.js, auth/realtime, media, backend, CDN, and infra causes with prioritized follow-up fixes.

Co-Authored-By: Claude Opus 4.7 <noreply@anthropic.com>
EOF
)"
```
Expected: Commit succeeds with only the report file.

- [ ] **Step 4: Report completion**

Tell the user:
```markdown
Root-cause report complete: `docs/superpowers/reports/2026-05-09-full-stack-fe-performance-root-cause.md`.
Commit: `<sha>`.
Next: choose whether to execute P0 measurement or start P1 FE fixes.
```
Expected: User can choose next step.
