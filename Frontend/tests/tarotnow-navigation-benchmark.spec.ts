import { promises as fs } from 'node:fs';
import path from 'node:path';
import { expect, test, type Browser, type Page, type Request, type Response } from '@playwright/test';

type BenchmarkScenario = 'logged-out' | 'logged-in';

interface RequestMetric {
 route: string;
 method: string;
 resourceType: string;
 url: string;
 status: number | null;
 durationMs: number | null;
 ttfbMs: number | null;
 responseBytes: number;
 failed: boolean;
 failureText: string | null;
}

interface DuplicateRequestGroup {
 key: string;
 count: number;
}

interface PageBenchmarkMetric {
 scenario: BenchmarkScenario;
 route: string;
 fromRoute: string | null;
 finalUrl: string;
 navigationMs: number;
 domContentLoadedMs: number | null;
 loadMs: number | null;
 fcpMs: number | null;
 lcpMs: number | null;
 requestCount: number;
 pendingCount: number;
 pendingUrls: string[];
 totalResponseBytes: number;
 duplicateRequestGroups: DuplicateRequestGroup[];
 slowRequests: RequestMetric[];
 requests: RequestMetric[];
}

interface ScenarioBenchmarkResult {
 scenario: BenchmarkScenario;
 pages: PageBenchmarkMetric[];
 visitedRoutes: string[];
}

interface BenchmarkRunResult {
 generatedAtUtc: string;
 baseOrigin: string;
 localePrefix: string;
 thresholds: {
  suspiciousRequestCount: number;
  slowRequestMs: number;
 };
 scenarios: ScenarioBenchmarkResult[];
}

interface MutableRequestMetric {
 route: string;
 method: string;
 resourceType: string;
 url: string;
 status: number | null;
 startedAtMs: number;
 finishedAtMs: number | null;
 durationMs: number | null;
 ttfbMs: number | null;
 responseBytes: number;
 failed: boolean;
 failureText: string | null;
}

const BASE_ORIGIN = 'https://tarotnow.xyz';
const LOCALE_PREFIX = '/vi';
const BASE_URL = `${BASE_ORIGIN}${LOCALE_PREFIX}`;

const BENCHMARK_USER = process.env.BENCHMARK_USERNAME ?? 'Lucifer';
const BENCHMARK_PASSWORD = process.env.BENCHMARK_PASSWORD ?? 'Sontung123!';

const MAX_PAGES_PER_SCENARIO = 48;
const DISCOVERY_LIMIT_PER_PAGE = 16;
const NAVIGATION_TIMEOUT_MS = 90_000;
const SETTLE_AFTER_NAVIGATION_MS = 2_000;

const SUSPICIOUS_REQUEST_COUNT = 30;
const SLOW_REQUEST_THRESHOLD_MS = 800;

const OUTPUT_DIR = path.resolve(process.cwd(), 'test-results', 'benchmark');
const OUTPUT_JSON = path.join(OUTPUT_DIR, 'tarotnow-benchmark.json');
const OUTPUT_MD = path.join(OUTPUT_DIR, 'tarotnow-benchmark-report.md');
const OUTPUT_ANALYSIS_MD = path.join(OUTPUT_DIR, 'tarotnow-benchmark-analysis.md');

const CORE_ROUTE_SEEDS = [
 `${LOCALE_PREFIX}`,
 `${LOCALE_PREFIX}/login`,
 `${LOCALE_PREFIX}/register`,
 `${LOCALE_PREFIX}/forgot-password`,
 `${LOCALE_PREFIX}/reading`,
 `${LOCALE_PREFIX}/inventory`,
 `${LOCALE_PREFIX}/gacha`,
 `${LOCALE_PREFIX}/gacha/history`,
 `${LOCALE_PREFIX}/collection`,
 `${LOCALE_PREFIX}/profile`,
 `${LOCALE_PREFIX}/profile/mfa`,
 `${LOCALE_PREFIX}/profile/reader`,
 `${LOCALE_PREFIX}/readers`,
 `${LOCALE_PREFIX}/chat`,
 `${LOCALE_PREFIX}/leaderboard`,
 `${LOCALE_PREFIX}/community`,
 `${LOCALE_PREFIX}/gamification`,
 `${LOCALE_PREFIX}/wallet`,
 `${LOCALE_PREFIX}/wallet/deposit`,
 `${LOCALE_PREFIX}/wallet/withdraw`,
 `${LOCALE_PREFIX}/notifications`,
 `${LOCALE_PREFIX}/reader/apply`,
 `${LOCALE_PREFIX}/reading/history`,
] as const;

function formatNumber(value: number | null): string {
 if (value === null || Number.isNaN(value)) return '-';
 return `${value.toFixed(0)}`;
}

function normalizeRoutePath(route: string): string | null {
 const raw = route.trim();
 if (!raw) return null;

 let resolvedPath = raw;
 if (/^https?:\/\//i.test(raw)) {
  try {
   const parsed = new URL(raw);
   if (parsed.origin !== BASE_ORIGIN) return null;
   resolvedPath = parsed.pathname + parsed.search;
  } catch {
   return null;
  }
 }

 if (!resolvedPath.startsWith('/')) return null;
 if (!resolvedPath.startsWith(`${LOCALE_PREFIX}/`) && resolvedPath !== LOCALE_PREFIX) return null;
 if (resolvedPath.includes('/api/')) return null;
 if (/\.[a-z0-9]+$/i.test(resolvedPath)) return null;

 const withoutHash = resolvedPath.split('#')[0] ?? resolvedPath;
 const withoutQuery = withoutHash.split('?')[0] ?? withoutHash;
 return withoutQuery.replace(/\/+$/, '') || LOCALE_PREFIX;
}

function normalizeRequestKey(method: string, requestUrl: string): string {
 try {
  const parsed = new URL(requestUrl);
  const params = [...parsed.searchParams.entries()]
   .filter(([key]) => key !== '_rsc')
   .sort(([left], [right]) => left.localeCompare(right));
  const query = params.map(([key, value]) => `${key}=${value}`).join('&');
  return `${method.toUpperCase()} ${parsed.origin}${parsed.pathname}${query ? `?${query}` : ''}`;
 } catch {
  return `${method.toUpperCase()} ${requestUrl}`;
 }
}

async function safeReadResponseBytes(response: Response | null): Promise<number> {
 if (!response) return 0;

 const contentLength = response.headers()['content-length'];
 if (contentLength) {
  const parsed = Number(contentLength);
  if (Number.isFinite(parsed) && parsed > 0) {
   return parsed;
  }
 }

 try {
  const body = await response.body();
  return body.byteLength;
 } catch {
  return 0;
 }
}

function toRequestMetric(record: MutableRequestMetric): RequestMetric {
 return {
  route: record.route,
  method: record.method,
  resourceType: record.resourceType,
  url: record.url,
  status: record.status,
  durationMs: record.durationMs,
  ttfbMs: record.ttfbMs,
  responseBytes: record.responseBytes,
  failed: record.failed,
  failureText: record.failureText,
 };
}

async function installPaintObservers(page: Page): Promise<void> {
 await page.addInitScript(() => {
  (window as Window & { __tnFcp?: number | null }).__tnFcp = null;
  (window as Window & { __tnLcp?: number | null }).__tnLcp = null;

  try {
   const paintObserver = new PerformanceObserver((list) => {
    for (const entry of list.getEntries()) {
     if (entry.name === 'first-contentful-paint') {
      (window as Window & { __tnFcp?: number | null }).__tnFcp = entry.startTime;
     }
    }
   });
   paintObserver.observe({ type: 'paint', buffered: true });
  } catch {
   // Browser does not support PerformanceObserver paint.
  }

  try {
   const lcpObserver = new PerformanceObserver((list) => {
    const entries = list.getEntries();
    const lastEntry = entries[entries.length - 1];
    if (lastEntry) {
     (window as Window & { __tnLcp?: number | null }).__tnLcp = lastEntry.startTime;
    }
   });
   lcpObserver.observe({ type: 'largest-contentful-paint', buffered: true });
  } catch {
   // Browser does not support LCP observer.
  }
 });
}

async function collectNavigationPaintMetrics(page: Page): Promise<{
 domContentLoadedMs: number | null;
 loadMs: number | null;
 fcpMs: number | null;
 lcpMs: number | null;
}> {
 return page.evaluate(() => {
  const navigationEntry = performance.getEntriesByType('navigation')[0] as PerformanceNavigationTiming | undefined;
  const win = window as Window & { __tnFcp?: number | null; __tnLcp?: number | null };

  const domContentLoadedMs = navigationEntry ? navigationEntry.domContentLoadedEventEnd : null;
  const loadMs = navigationEntry && navigationEntry.loadEventEnd > 0 ? navigationEntry.loadEventEnd : null;
  const fcpMs = typeof win.__tnFcp === 'number' ? win.__tnFcp : null;
  const lcpMs = typeof win.__tnLcp === 'number' ? win.__tnLcp : null;

  return {
   domContentLoadedMs,
   loadMs,
   fcpMs,
   lcpMs,
  };
 });
}

async function discoverRoutesFromPage(page: Page): Promise<string[]> {
 const discoveredHrefs = await page.$$eval('a[href]', (anchors) =>
  anchors
   .map((anchor) => anchor.getAttribute('href') ?? '')
   .filter((href) => href.length > 0),
 );

 const discoveredRoutes = new Set<string>();
 for (const href of discoveredHrefs) {
  const normalized = normalizeRoutePath(href);
  if (normalized) discoveredRoutes.add(normalized);
 }

 return [...discoveredRoutes];
}

async function collectStaticLocaleRoutes(): Promise<string[]> {
 const appLocaleDir = path.resolve(process.cwd(), 'src', 'app', '[locale]');
 const staticRoutes = new Set<string>(CORE_ROUTE_SEEDS);

 const walk = async (currentDir: string): Promise<void> => {
  const entries = await fs.readdir(currentDir, { withFileTypes: true });
  for (const entry of entries) {
   const absolute = path.join(currentDir, entry.name);
   if (entry.isDirectory()) {
    await walk(absolute);
    continue;
   }

   if (!entry.isFile() || entry.name !== 'page.tsx') {
    continue;
   }

   const relativeDir = path.relative(appLocaleDir, path.dirname(absolute));
   const segments = relativeDir
    .split(path.sep)
    .filter((segment) => segment.length > 0)
    .filter((segment) => !segment.startsWith('(') && !segment.endsWith(')'))
    .filter((segment) => segment !== 'api');

   if (segments.some((segment) => segment.startsWith('[') && segment.endsWith(']'))) {
    continue;
   }

   const route = `${LOCALE_PREFIX}${segments.length > 0 ? `/${segments.join('/')}` : ''}`;
   staticRoutes.add(route);
  }
 };

 await walk(appLocaleDir);
 return [...staticRoutes].sort((left, right) => left.localeCompare(right));
}

async function benchmarkNavigation(
 page: Page,
 scenario: BenchmarkScenario,
 route: string,
 fromRoute: string | null,
): Promise<PageBenchmarkMetric> {
 const routeUrl = `${BASE_ORIGIN}${route}`;
 const requests = new Map<Request, MutableRequestMetric>();
 const finalizeTasks: Array<Promise<void>> = [];

 const onRequest = (request: Request): void => {
  requests.set(request, {
   route,
   method: request.method(),
   resourceType: request.resourceType(),
   url: request.url(),
   status: null,
   startedAtMs: Date.now(),
   finishedAtMs: null,
   durationMs: null,
   ttfbMs: null,
   responseBytes: 0,
   failed: false,
   failureText: null,
  });
 };

 const onRequestFinished = (request: Request): void => {
  const pendingTask = (async () => {
   const record = requests.get(request);
   if (!record || record.finishedAtMs !== null) return;

   const response = await request.response();
   const timing = request.timing();

   record.status = response?.status() ?? null;
   record.finishedAtMs = Date.now();
   record.durationMs = Math.max(0, record.finishedAtMs - record.startedAtMs);
   record.ttfbMs = timing.requestStart >= 0 && timing.responseStart >= 0
    ? Math.max(0, timing.responseStart - timing.requestStart)
    : null;
   record.responseBytes = await safeReadResponseBytes(response);
  })();

  finalizeTasks.push(pendingTask);
 };

 const onRequestFailed = (request: Request): void => {
  const record = requests.get(request);
  if (!record) return;
  record.failed = true;
  record.finishedAtMs = Date.now();
  record.durationMs = Math.max(0, record.finishedAtMs - record.startedAtMs);
  record.failureText = request.failure()?.errorText ?? 'Request failed';
 };

 page.on('request', onRequest);
 page.on('requestfinished', onRequestFinished);
 page.on('requestfailed', onRequestFailed);

 const startedAtMs = Date.now();
 try {
  await page.goto(routeUrl, {
   waitUntil: 'domcontentloaded',
   timeout: NAVIGATION_TIMEOUT_MS,
  });
 } catch {
  // Keep collecting diagnostics even when navigation is unstable.
 }

 try {
  await page.waitForLoadState('load', { timeout: 15_000 });
 } catch {
  // Some pages keep long-lived streams; load may not settle in time.
 }

 await page.waitForTimeout(SETTLE_AFTER_NAVIGATION_MS);
 await Promise.allSettled(finalizeTasks);

 page.off('request', onRequest);
 page.off('requestfinished', onRequestFinished);
 page.off('requestfailed', onRequestFailed);

 const navigationMs = Date.now() - startedAtMs;
 const finalUrl = page.url();
 const paintMetrics = await collectNavigationPaintMetrics(page);
 const requestMetrics = [...requests.values()].map(toRequestMetric);

 const pendingNonPersistent = requestMetrics.filter((request) =>
  request.durationMs === null
   && !request.failed
   && request.resourceType !== 'websocket'
   && request.resourceType !== 'eventsource',
 );

 const slowRequests = requestMetrics.filter((request) => (request.durationMs ?? 0) > SLOW_REQUEST_THRESHOLD_MS);
 const totalResponseBytes = requestMetrics.reduce((sum, request) => sum + request.responseBytes, 0);

 const duplicateCounter = new Map<string, number>();
 for (const request of requestMetrics) {
  const key = normalizeRequestKey(request.method, request.url);
  duplicateCounter.set(key, (duplicateCounter.get(key) ?? 0) + 1);
 }
 const duplicateRequestGroups = [...duplicateCounter.entries()]
  .filter(([, count]) => count > 1)
  .sort((left, right) => right[1] - left[1])
  .map(([key, count]) => ({ key, count }));

 return {
  scenario,
  route,
  fromRoute,
  finalUrl,
  navigationMs,
  domContentLoadedMs: paintMetrics.domContentLoadedMs,
  loadMs: paintMetrics.loadMs,
  fcpMs: paintMetrics.fcpMs,
  lcpMs: paintMetrics.lcpMs,
  requestCount: requestMetrics.length,
  pendingCount: pendingNonPersistent.length,
  pendingUrls: pendingNonPersistent.map((request) => request.url),
  totalResponseBytes,
  duplicateRequestGroups,
  slowRequests,
  requests: requestMetrics,
 };
}

async function loginAsBenchmarkUser(page: Page): Promise<void> {
 for (let attempt = 1; attempt <= 3; attempt += 1) {
  await page.goto(`${BASE_URL}/login`, {
   waitUntil: 'domcontentloaded',
   timeout: NAVIGATION_TIMEOUT_MS,
  });

  await page.locator('input[name="emailOrUsername"]').fill(BENCHMARK_USER, { timeout: 20_000 });
  await page.locator('input[name="password"]').fill(BENCHMARK_PASSWORD, { timeout: 20_000 });

  const loginResponsePromise = page.waitForResponse((response) => {
   return response.url().includes('/api/auth/login') && response.request().method() === 'POST';
  }, { timeout: 20_000 }).then(async (response) => {
   const payload = await response.json().catch(() => ({ success: false })) as { success?: boolean };
   return response.ok() && payload.success === true;
  }).catch(() => false);

  const routeChangedPromise = page.waitForURL((url) => !url.pathname.endsWith('/login'), { timeout: 20_000 })
   .then(() => true)
   .catch(() => false);

  await page.locator('form button[type="submit"]').first().click();
  const [loginResponseOk, routeChanged] = await Promise.all([loginResponsePromise, routeChangedPromise]);
  if (!loginResponseOk && !routeChanged) {
   continue;
  }

  await page.goto(`${BASE_URL}/profile`, {
   waitUntil: 'domcontentloaded',
   timeout: NAVIGATION_TIMEOUT_MS,
  });
  await page.waitForFunction(() => !window.location.pathname.endsWith('/login'), null, { timeout: 30_000 });
  await page.waitForTimeout(1_500);
  return;
 }

 throw new Error('Unable to login with benchmark credentials after 3 attempts.');
}

async function runScenario(browser: Browser, scenario: BenchmarkScenario, seedRoutes: string[]): Promise<ScenarioBenchmarkResult> {
 const context = await browser.newContext({ ignoreHTTPSErrors: true });
 const page = await context.newPage();
 await installPaintObservers(page);

 if (scenario === 'logged-in') {
  await loginAsBenchmarkUser(page);
 }

 const queue: string[] = [...seedRoutes];
 const visited = new Set<string>();
 const pageMetrics: PageBenchmarkMetric[] = [];
 let fromRoute: string | null = null;

 while (queue.length > 0 && visited.size < MAX_PAGES_PER_SCENARIO) {
  const nextRoute = queue.shift();
  if (!nextRoute) break;

  const normalizedRoute = normalizeRoutePath(nextRoute);
  if (!normalizedRoute || visited.has(normalizedRoute)) {
   continue;
  }

  const metric = await benchmarkNavigation(page, scenario, normalizedRoute, fromRoute);
  pageMetrics.push(metric);
  visited.add(normalizedRoute);
  fromRoute = normalizedRoute;

  const discoveredRoutes = await discoverRoutesFromPage(page);
  for (const discovered of discoveredRoutes.slice(0, DISCOVERY_LIMIT_PER_PAGE)) {
   if (visited.has(discovered)) continue;
   if (queue.includes(discovered)) continue;
   queue.push(discovered);
  }
 }

 await context.close();
 return {
  scenario,
  pages: pageMetrics,
  visitedRoutes: [...visited],
 };
}

function createMarkdownReport(result: BenchmarkRunResult): string {
 const allPages = result.scenarios.flatMap((scenario) => scenario.pages);
 const suspiciousPages = allPages.filter((page) => page.requestCount > SUSPICIOUS_REQUEST_COUNT);
 const pendingPages = allPages.filter((page) => page.pendingCount > 0);
 const slowRequests = allPages.flatMap((page) =>
  page.slowRequests.map((request) => ({
   scenario: page.scenario,
   route: page.route,
   ...request,
  })),
 );

 const summaryLines = result.scenarios.map((scenario) => {
  const pageCount = scenario.pages.length;
  const avgNavigation = pageCount > 0
   ? scenario.pages.reduce((sum, page) => sum + page.navigationMs, 0) / pageCount
   : 0;
  const totalRequests = scenario.pages.reduce((sum, page) => sum + page.requestCount, 0);
  const pending = scenario.pages.reduce((sum, page) => sum + page.pendingCount, 0);
  return `| ${scenario.scenario} | ${pageCount} | ${avgNavigation.toFixed(0)} | ${totalRequests} | ${pending} |`;
 });

 const pageLines = allPages.map((page) =>
  `| ${page.scenario} | ${page.route} | ${page.finalUrl} | ${page.requestCount} | ${page.pendingCount} | ${formatNumber(page.navigationMs)} | ${formatNumber(page.domContentLoadedMs)} | ${formatNumber(page.loadMs)} | ${formatNumber(page.fcpMs)} | ${formatNumber(page.lcpMs)} | ${page.totalResponseBytes} |`,
 );

 const suspiciousLines = suspiciousPages.length > 0
  ? suspiciousPages.map((page) =>
   `| ${page.scenario} | ${page.route} | ${page.requestCount} | ${page.pendingCount} | ${formatNumber(page.navigationMs)} |`,
  )
  : ['| - | - | - | - | - |'];

 const pendingLines = pendingPages.length > 0
  ? pendingPages.flatMap((page) =>
   page.pendingUrls.map((url) =>
    `| ${page.scenario} | ${page.route} | ${url} |`,
   ),
  )
  : ['| - | - | - |'];

 const slowRequestLines = slowRequests.length > 0
  ? slowRequests
   .sort((left, right) => (right.durationMs ?? 0) - (left.durationMs ?? 0))
   .slice(0, 60)
   .map((request) =>
    `| ${request.scenario} | ${request.route} | ${request.method} | ${request.status ?? '-'} | ${formatNumber(request.durationMs)} | ${formatNumber(request.ttfbMs)} | ${request.url} |`,
   )
  : ['| - | - | - | - | - | - | - |'];

 const duplicateLines = allPages
  .flatMap((page) =>
   page.duplicateRequestGroups.slice(0, 8).map((group) =>
    `| ${page.scenario} | ${page.route} | ${group.count} | ${group.key} |`,
   ),
  )
  .slice(0, 100);

 return [
  '# TarotNow Navigation Benchmark Report',
  '',
  `- Generated at (UTC): ${result.generatedAtUtc}`,
  `- Base URL: ${BASE_URL}`,
  `- Thresholds: suspicious page > ${SUSPICIOUS_REQUEST_COUNT} requests, slow request > ${SLOW_REQUEST_THRESHOLD_MS}ms`,
  '',
  '## Scenario Summary',
  '| Scenario | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests |',
  '| --- | ---: | ---: | ---: | ---: |',
  ...summaryLines,
  '',
  '## Per-Page Metrics',
  '| Scenario | Route | Final URL | Requests | Pending | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | Response Bytes |',
  '| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |',
  ...pageLines,
  '',
  `## Suspicious Pages (Request Count > ${SUSPICIOUS_REQUEST_COUNT})`,
  '| Scenario | Route | Request Count | Pending | Navigate (ms) |',
  '| --- | --- | ---: | ---: | ---: |',
  ...suspiciousLines,
  '',
  `## Slow Requests (>${SLOW_REQUEST_THRESHOLD_MS}ms)`,
  '| Scenario | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |',
  '| --- | --- | --- | ---: | ---: | ---: | --- |',
  ...slowRequestLines,
  '',
  '## Pending Requests',
  '| Scenario | Route | URL |',
  '| --- | --- | --- |',
  ...pendingLines,
  '',
  '## Duplicate Requests (Top groups)',
  '| Scenario | Route | Count | Request Key |',
  '| --- | --- | ---: | --- |',
  ...(duplicateLines.length > 0 ? duplicateLines : ['| - | - | - | - |']),
  '',
 ].join('\n');
}

function createAnalysisReport(result: BenchmarkRunResult): string {
 const allPages = result.scenarios.flatMap((scenario) => scenario.pages);
 const pendingPages = allPages.filter((page) => page.pendingCount > 0);
 const slowRequests = allPages
  .flatMap((page) => page.slowRequests.map((request) => ({ ...request, scenario: page.scenario, route: page.route })))
  .sort((left, right) => (right.durationMs ?? 0) - (left.durationMs ?? 0));
 const duplicateGroups = allPages
  .flatMap((page) => page.duplicateRequestGroups.map((group) => ({ ...group, scenario: page.scenario, route: page.route })));
 const highRiskDuplicates = duplicateGroups.filter((entry) => !entry.key.includes('/cdn-cgi/rum'));
 const topSlowPages = [...allPages]
  .sort((left, right) => right.navigationMs - left.navigationMs)
  .slice(0, 5);

 const scenarioLines = result.scenarios.map((scenario) => {
  const totalRequests = scenario.pages.reduce((sum, page) => sum + page.requestCount, 0);
  const avgNavigationMs = scenario.pages.length > 0
   ? scenario.pages.reduce((sum, page) => sum + page.navigationMs, 0) / scenario.pages.length
   : 0;
  const avgRequestsPerPage = scenario.pages.length > 0 ? totalRequests / scenario.pages.length : 0;
  const pendingCount = scenario.pages.reduce((sum, page) => sum + page.pendingCount, 0);
  return `| ${scenario.scenario} | ${scenario.pages.length} | ${avgRequestsPerPage.toFixed(1)} | ${avgNavigationMs.toFixed(0)} | ${pendingCount} |`;
 });

 const topSlowPageLines = topSlowPages.map((page) =>
  `| ${page.scenario} | ${page.route} | ${page.navigationMs} | ${page.requestCount} | ${page.pendingCount} |`,
 );

 const topSlowRequestLines = slowRequests.slice(0, 12).map((request) =>
  `| ${request.scenario} | ${request.route} | ${request.method} | ${request.status ?? '-'} | ${formatNumber(request.durationMs)} | ${formatNumber(request.ttfbMs)} | ${request.url} |`,
 );

 const criticalFindings = [
  pendingPages.length > 0
   ? `Pending requests xuất hiện trên ${pendingPages.length} trang.`
   : 'Không phát hiện pending request trong lần chạy benchmark này.',
  highRiskDuplicates.length > 0
   ? `Phát hiện ${highRiskDuplicates.length} nhóm request duplicate không phải telemetry.`
   : 'Không thấy duplicate API quan trọng ngoài Cloudflare RUM.',
  slowRequests.length > 0
   ? `Có ${slowRequests.length} request vượt ngưỡng ${SLOW_REQUEST_THRESHOLD_MS}ms.`
   : `Không có request nào vượt ngưỡng ${SLOW_REQUEST_THRESHOLD_MS}ms.`,
 ];

 return [
  '# TarotNow Benchmark Analysis',
  '',
  `- Run time (UTC): ${result.generatedAtUtc}`,
  `- Base: ${BASE_URL}`,
  '- Tooling: Playwright Test (Chromium, serial)',
  '- Thresholds:',
  `  - Suspicious page: > ${SUSPICIOUS_REQUEST_COUNT} requests`,
  `  - Slow request: > ${SLOW_REQUEST_THRESHOLD_MS}ms`,
  '',
  '## Scenario summary',
  '| Scenario | Pages | Avg requests/page | Avg nav (ms) | Pending |',
  '| --- | ---: | ---: | ---: | ---: |',
  ...scenarioLines,
  '',
  '## Top slow pages',
  '| Scenario | Route | Navigate (ms) | Request count | Pending |',
  '| --- | --- | ---: | ---: | ---: |',
  ...(topSlowPageLines.length > 0 ? topSlowPageLines : ['| - | - | - | - | - |']),
  '',
  '## Slow requests',
  '| Scenario | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |',
  '| --- | --- | --- | ---: | ---: | ---: | --- |',
  ...(topSlowRequestLines.length > 0 ? topSlowRequestLines : ['| - | - | - | - | - | - | - |']),
  '',
  '## High-risk findings',
  ...criticalFindings.map((item, index) => `${index + 1}. ${item}`),
  '',
  '## Notes',
  '- Cloudflare RUM duplicate POSTs (`/cdn-cgi/rum`) là telemetry ngoại vi, không phải business API.',
  '- Nếu cần so sánh trước/sau refactor, chạy benchmark trên cùng environment và cùng account.',
 ].join('\n');
}

test.describe('TarotNow production benchmark', () => {
 test.describe.configure({ mode: 'serial' });
 test.setTimeout(30 * 60 * 1000);

 test('benchmark navigation and api timing for logged-out + logged-in', async ({ browser }) => {
  const staticRoutes = await collectStaticLocaleRoutes();
  const seedRoutes = [...new Set([...CORE_ROUTE_SEEDS, ...staticRoutes])];
  const loggedInResult = await runScenario(browser, 'logged-in', seedRoutes);
  const loggedOutResult = await runScenario(browser, 'logged-out', seedRoutes);

  const result: BenchmarkRunResult = {
   generatedAtUtc: new Date().toISOString(),
   baseOrigin: BASE_ORIGIN,
   localePrefix: LOCALE_PREFIX,
   thresholds: {
    suspiciousRequestCount: SUSPICIOUS_REQUEST_COUNT,
    slowRequestMs: SLOW_REQUEST_THRESHOLD_MS,
   },
   scenarios: [loggedOutResult, loggedInResult],
  };

  await fs.mkdir(OUTPUT_DIR, { recursive: true });
  await fs.writeFile(OUTPUT_JSON, JSON.stringify(result, null, 2), 'utf8');
  await fs.writeFile(OUTPUT_MD, createMarkdownReport(result), 'utf8');
  await fs.writeFile(OUTPUT_ANALYSIS_MD, createAnalysisReport(result), 'utf8');

  expect(result.scenarios[0]?.pages.length ?? 0).toBeGreaterThan(0);
  expect(result.scenarios[1]?.pages.length ?? 0).toBeGreaterThan(0);
 });
});
