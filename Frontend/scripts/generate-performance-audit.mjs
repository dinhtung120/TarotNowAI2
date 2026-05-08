import { readFile, writeFile } from 'node:fs/promises';
import path from 'node:path';

const rootDir = path.resolve(process.cwd(), '..');
const benchmarkDir = path.resolve(process.cwd(), 'test-results', 'benchmark');
const defaultInput = path.join(benchmarkDir, 'tarotnow-benchmark.json');
const inputPath = path.resolve(process.cwd(), process.env.PERFORMANCE_AUDIT_INPUT ?? defaultInput);
const outputPath = path.resolve(rootDir, 'PERFORMANCE-AUDIT.md');

const requestHigh = 25;
const requestCritical = 35;
const slowMedium = 400;
const slowHigh = 800;

function format(value, digits = 0) {
  if (value === null || value === undefined || Number.isNaN(Number(value))) return '-';
  return Number(value).toFixed(digits);
}

function featureOf(page) {
  return page.feature ?? 'other';
}

function severityForPage(page) {
  if (page.failedRequestCount > 0 || page.pendingCount > 0 || page.requestCount > requestCritical) return 'Critical';
  if (page.requestCount > requestHigh || page.slowRequests?.some((request) => (request.durationMs ?? 0) > slowHigh)) return 'High';
  if (page.slowRequests?.some((request) => (request.durationMs ?? 0) > slowMedium)) return 'Medium';
  return 'Low';
}

function allPages(result) {
  return result.scenarios.flatMap((scenario) => scenario.pages ?? []);
}

function allRequests(result) {
  return allPages(result).flatMap((page) => (page.requests ?? []).map((request) => ({ ...request, page })));
}

function stripLocale(route, localePrefix) {
  if (route === localePrefix) return '/';
  return route.startsWith(`${localePrefix}/`) ? route.slice(localePrefix.length) : route;
}

function routeFamily(route, localePrefix) {
  const routePath = stripLocale(route, localePrefix);
  if (routePath === '/') return 'home';

  const firstSegment = routePath.split('/').filter(Boolean)[0] ?? 'home';
  if (['login', 'register', 'forgot-password', 'reset-password', 'verify-email', 'legal'].includes(firstSegment)) return 'auth-public';
  if (['inventory', 'gacha', 'collection'].includes(firstSegment)) return 'inventory-gacha-collection';
  if (['profile', 'wallet', 'notifications'].includes(firstSegment)) return 'profile-wallet-notifications';
  if (['reader', 'readers', 'chat'].includes(firstSegment)) return 'reader-chat';
  if (['community', 'leaderboard', 'gamification'].includes(firstSegment)) return 'community-leaderboard-quest';
  return firstSegment;
}

function routeFamilyLines(result, pages) {
  const localePrefix = result.localePrefix ?? '/vi';
  const groups = new Map();

  for (const page of pages) {
    const family = routeFamily(page.route, localePrefix);
    const key = `${page.scenario}|${page.viewport}|${family}`;
    const current = groups.get(key) ?? {
      scenario: page.scenario,
      viewport: page.viewport,
      family,
      pages: 0,
      requests: 0,
      nav: 0,
      pending: 0,
      failed: 0,
    };

    current.pages += 1;
    current.requests += Number(page.requestCount ?? 0);
    current.nav += Number(page.navigationMs ?? 0);
    current.pending += Number(page.pendingCount ?? 0);
    current.failed += Number(page.failedRequestCount ?? 0);
    groups.set(key, current);
  }

  return [...groups.values()]
    .sort((left, right) => left.scenario.localeCompare(right.scenario) || left.viewport.localeCompare(right.viewport) || left.family.localeCompare(right.family))
    .map((entry) => `| ${entry.scenario} | ${entry.viewport} | ${entry.family} | ${entry.pages} | ${format(entry.requests / entry.pages, 1)} | ${format(entry.nav / entry.pages)} | ${entry.pending} | ${entry.failed} |`);
}

function tableOrEmpty(lines, emptyLine) {
  return lines.length > 0 ? lines : [emptyLine];
}

function buildReport(result) {
  const pages = allPages(result);
  const requests = allRequests(result);
  const criticalPages = pages.filter((page) => severityForPage(page) === 'Critical');
  const highPages = pages.filter((page) => severityForPage(page) === 'High');
  const mediumPages = pages.filter((page) => severityForPage(page) === 'Medium');
  const slowCriticalRequests = requests.filter((request) => (request.durationMs ?? 0) > slowHigh);
  const slowMediumRequests = requests.filter((request) => (request.durationMs ?? 0) > slowMedium && (request.durationMs ?? 0) <= slowHigh);
  const duplicateGroups = pages.flatMap((page) => (page.duplicateRequestGroups ?? []).map((group) => ({ ...group, page })));
  const pendingPages = pages.filter((page) => page.pendingCount > 0);

  const scenarioLines = result.scenarios.map((scenario) => {
    const scenarioPages = scenario.pages ?? [];
    const totalRequests = scenarioPages.reduce((sum, page) => sum + page.requestCount, 0);
    const avgRequests = scenarioPages.length > 0 ? totalRequests / scenarioPages.length : 0;
    const avgNav = scenarioPages.length > 0 ? scenarioPages.reduce((sum, page) => sum + page.navigationMs, 0) / scenarioPages.length : 0;
    const failed = scenarioPages.reduce((sum, page) => sum + page.failedRequestCount, 0);
    const pending = scenarioPages.reduce((sum, page) => sum + page.pendingCount, 0);
    return `| ${scenario.scenario} | ${scenario.viewport} | ${scenarioPages.length} | ${format(avgRequests, 1)} | ${format(avgNav)} | ${pending} | ${failed} | ${scenario.loginBootstrapSucceeded ? 'yes' : 'no'} |`;
  });

  const metricLines = pages
    .sort((left, right) => {
      const severityRank = { Critical: 0, High: 1, Medium: 2, Low: 3 };
      return severityRank[severityForPage(left)] - severityRank[severityForPage(right)] || right.requestCount - left.requestCount;
    })
    .map((page) => `| ${severityForPage(page)} | ${featureOf(page)} | ${page.scenario} | ${page.viewport} | ${page.route} | ${page.requestCount} | ${page.requestBreakdown?.api ?? 0} | ${page.requestBreakdown?.static ?? 0} | ${page.requestBreakdown?.thirdParty ?? 0} | ${format(page.navigationMs)} | ${format(page.fcpMs)} | ${format(page.lcpMs)} | ${format(page.tbt, 1)} | ${format(page.cls, 4)} | ${page.pendingCount} | ${page.failedRequestCount} | ${page.totalTransferBytes ?? 0} |`);

  const slowLines = [...slowCriticalRequests, ...slowMediumRequests]
    .sort((left, right) => (right.durationMs ?? 0) - (left.durationMs ?? 0))
    .slice(0, 80)
    .map((request) => `| ${(request.durationMs ?? 0) > slowHigh ? 'Critical' : 'Medium'} | ${featureOf(request.page)} | ${request.page.scenario} | ${request.page.viewport} | ${request.page.route} | ${request.method} | ${request.status ?? '-'} | ${format(request.durationMs)} | ${format(request.ttfbMs)} | ${request.category} | ${request.url} |`);

  const duplicateLines = duplicateGroups
    .filter((entry) => !entry.key.includes('/cdn-cgi/rum'))
    .sort((left, right) => right.count - left.count)
    .slice(0, 60)
    .map((entry) => `| High | ${featureOf(entry.page)} | ${entry.page.scenario} | ${entry.page.viewport} | ${entry.page.route} | ${entry.count} | ${entry.key} |`);

  const pendingLines = pendingPages.flatMap((page) =>
    (page.pendingUrls ?? []).map((url) => `| Critical | ${featureOf(page)} | ${page.scenario} | ${page.viewport} | ${page.route} | ${url} |`),
  );

  const issueBullets = [
    criticalPages.length > 0 ? `- Critical: ${criticalPages.length} page(s) có request count >${requestCritical}, pending request, failed request, hoặc issue nghiêm trọng.` : '- Critical: chưa phát hiện page Critical theo benchmark hiện tại.',
    highPages.length > 0 ? `- High: ${highPages.length} page(s) vượt ngưỡng >${requestHigh} requests hoặc có request >${slowHigh}ms.` : '- High: chưa phát hiện page High theo benchmark hiện tại.',
    mediumPages.length > 0 ? `- Medium: ${mediumPages.length} page(s) có request trong dải ${slowMedium}-${slowHigh}ms.` : '- Medium: chưa phát hiện page Medium theo benchmark hiện tại.',
    duplicateGroups.length > 0 ? `- Duplicate: ${duplicateGroups.length} nhóm duplicate request cần kiểm tra over-fetch/cache key.` : '- Duplicate: chưa phát hiện duplicate request business đáng kể.',
    pendingPages.length > 0 ? `- Pending: ${pendingPages.length} page(s) có pending request không phải websocket/eventsource.` : '- Pending: chưa phát hiện pending request bất thường.',
  ];

  return [
    '# PERFORMANCE AUDIT - TarotNow',
    '',
    '## Executive Summary',
    '',
    `- Generated at (UTC): ${new Date().toISOString()}`,
    `- Benchmark generated at (UTC): ${result.generatedAtUtc}`,
    `- Base origin: ${result.baseOrigin}`,
    `- Locale prefix: ${result.localePrefix}`,
    `- Benchmark mode: ${result.benchmarkMode}`,
    `- Total scenarios: ${result.scenarios.length}`,
    `- Total pages measured: ${pages.length}`,
    `- Critical pages: ${criticalPages.length}`,
    `- High pages: ${highPages.length}`,
    `- Medium pages: ${mediumPages.length}`,
    `- Slow requests >${slowHigh}ms: ${slowCriticalRequests.length}`,
    `- Slow requests ${slowMedium}-${slowHigh}ms: ${slowMediumRequests.length}`,
    `- Request thresholds: >${requestHigh} suspicious, >${requestCritical} severe`,
    `- Slow request thresholds: >${slowMedium}ms optimize, >${slowHigh}ms serious`,
    '',
    '## Scenario Coverage',
    '',
    '| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed | Login bootstrap |',
    '| --- | --- | ---: | ---: | ---: | ---: | ---: | --- |',
    ...scenarioLines,
    '',
    '## Route Family Coverage',
    '',
    '| Scenario | Viewport | Family | Pages | Avg requests/page | Avg nav (ms) | Pending | Failed |',
    '| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |',
    ...tableOrEmpty(routeFamilyLines(result, pages), '| - | - | - | - | - | - | - | - |'),
    '',
    '## Detailed Metrics Table',
    '',
    '| Severity | Feature | Scenario | Viewport | Route | Requests | API | Static | Third-party | Nav (ms) | FCP (ms) | LCP (ms) | TBT (ms) | CLS | Pending | Failed | Transfer bytes |',
    '| --- | --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |',
    ...tableOrEmpty(metricLines, '| - | - | - | - | - | - | - | - | - | - | - | - | - | - | - | - | - |'),
    '',
    '## Major Issues Found',
    '',
    ...issueBullets,
    '',
    '### Slow Requests',
    '',
    '| Severity | Feature | Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |',
    '| --- | --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |',
    ...tableOrEmpty(slowLines, '| - | - | - | - | - | - | - | - | - | - | - |'),
    '',
    '### Duplicate API / Request Candidates',
    '',
    '| Severity | Feature | Scenario | Viewport | Route | Count | Request Key |',
    '| --- | --- | --- | --- | --- | ---: | --- |',
    ...tableOrEmpty(duplicateLines, '| - | - | - | - | - | - | - |'),
    '',
    '### Pending Requests',
    '',
    '| Severity | Feature | Scenario | Viewport | Route | URL |',
    '| --- | --- | --- | --- | --- | --- |',
    ...tableOrEmpty(pendingLines, '| - | - | - | - | - | - |'),
    '',
    '## Optimization Plan',
    '',
    '1. Fix shared Critical issues first: auth/session loops, layout-level fetch churn, failed requests, pending non-persistent requests.',
    '2. Fix feature Critical/High issues next, starting with the feature that has the most affected page-scenario combinations.',
    '3. Fix duplicate API calls by inspecting query keys, staleTime, refetch triggers, and parent/child component fetch ownership.',
    '4. Fix image/cache issues by checking Next Image usage, remote patterns, dimensions, lazy/eager strategy, and modal reopen behavior.',
    '5. Re-run the affected feature benchmark after every hotspot fix, then run full matrix before final deploy validation.',
    '',
    '## Recommended Refactors',
    '',
    '- Middleware/session: inspect only if report shows session API churn, handshake redirects, or auth-related duplicate requests.',
    '- TanStack Query: inspect feature hooks whose API endpoints appear in duplicate request candidates.',
    '- App Router layouts: inspect layout/provider boundaries if multiple unrelated features share the same duplicate or slow request pattern.',
    '- Custom hooks: inspect effects only when a route shows repeated interaction or post-load requests.',
    '- Image loading: inspect collection/gacha/community routes when slow static/image requests dominate.',
    '- Route prefetch: inspect Link usage only when benchmark shows route-navigation prefetch churn causing unnecessary requests.',
    '',
    '## Final Validation',
    '',
    '- Baseline benchmark: pending until run is recorded.',
    '- Feature benchmark after fixes: pending until hotspot is selected.',
    '- Local verification: pending.',
    '- GitHub Actions: pending.',
    '- Post-deploy full production benchmark: pending.',
    '',
  ].join('\n');
}

const raw = await readFile(inputPath, 'utf8');
const result = JSON.parse(raw);
const report = buildReport(result);
await writeFile(outputPath, report, 'utf8');
console.log(`Wrote ${outputPath}`);
