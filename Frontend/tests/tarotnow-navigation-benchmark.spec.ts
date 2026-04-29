import { promises as fs } from 'node:fs';
import path from 'node:path';
import { expect, test, type Browser, type Page, type Request, type Response } from '@playwright/test';

type BenchmarkScenario = 'logged-out' | 'logged-in-admin' | 'logged-in-reader';
type BenchmarkViewportId = 'desktop' | 'mobile';

type RequestCategory = 'html' | 'api' | 'static' | 'third-party' | 'telemetry' | 'websocket' | 'other';
type SeverityLevel = 'critical' | 'high' | 'medium' | 'low' | 'none';

interface DuplicateRequestGroup {
  key: string;
  count: number;
}

interface RequestMetric {
  scenario: BenchmarkScenario;
  viewport: BenchmarkViewportId;
  route: string;
  method: string;
  resourceType: string;
  initiator: string;
  category: RequestCategory;
  url: string;
  host: string;
  status: number | null;
  durationMs: number | null;
  ttfbMs: number | null;
  responseBytes: number;
  transferBytes: number;
  failed: boolean;
  failureText: string | null;
  waterfallDepth: number;
  pendingAgeMs: number | null;
  isDuplicate: boolean;
}

interface PageBreakdown {
  total: number;
  html: number;
  api: number;
  static: number;
  thirdParty: number;
  telemetry: number;
  websocket: number;
  other: number;
}

interface PageBenchmarkMetric {
  scenario: BenchmarkScenario;
  viewport: BenchmarkViewportId;
  route: string;
  navFrom: string | null;
  finalUrl: string;
  navigationMs: number;
  domContentLoadedMs: number | null;
  loadMs: number | null;
  fcpMs: number | null;
  lcpMs: number | null;
  cls: number | null;
  tbt: number | null;
  requestCount: number;
  requestSeverity: SeverityLevel;
  pendingCount: number;
  pendingUrls: string[];
  totalResponseBytes: number;
  totalTransferBytes: number;
  requestBreakdown: PageBreakdown;
  thirdPartyBreakdown: Array<{ host: string; count: number }>;
  duplicateRequestGroups: DuplicateRequestGroup[];
  slowRequests: RequestMetric[];
  requests: RequestMetric[];
  coverageBlocked: string[];
}

interface ScenarioBenchmarkResult {
  scenario: BenchmarkScenario;
  viewport: BenchmarkViewportId;
  pages: PageBenchmarkMetric[];
  visitedRoutes: string[];
  loginBootstrapSucceeded: boolean;
  loginBootstrapNotes: string[];
  coverageNotes: string[];
}

interface BenchmarkRunResult {
  generatedAtUtc: string;
  baseOrigin: string;
  localePrefix: string;
  thresholds: {
    requestCountHigh: number;
    requestCountCritical: number;
    slowRequestMediumMs: number;
    slowRequestHighMs: number;
  };
  scenarios: ScenarioBenchmarkResult[];
}

interface MutableRequestMetric {
  route: string;
  method: string;
  resourceType: string;
  initiator: string;
  category: RequestCategory;
  url: string;
  host: string;
  status: number | null;
  startedAtMs: number;
  finishedAtMs: number | null;
  durationMs: number | null;
  ttfbMs: number | null;
  responseBytes: number;
  transferBytes: number;
  failed: boolean;
  failureText: string | null;
  waterfallDepth: number;
  pendingAgeMs: number | null;
  isDuplicate: boolean;
}

interface DynamicRouteDiscovery {
  readingSessionIds: string[];
  readingHistoryIds: string[];
  readerIds: string[];
  chatIds: string[];
  coverageNotes: string[];
}

interface ScenarioCredential {
  username: string;
  password: string;
}

interface PaintMetricsSnapshot {
  domContentLoadedMs: number | null;
  loadMs: number | null;
  fcpMs: number | null;
  lcpMs: number | null;
  cls: number | null;
  tbt: number | null;
}

interface ViewportProfile {
  id: BenchmarkViewportId;
  contextOptions: Parameters<Browser['newContext']>[0];
}

const RUN_NAVIGATION_BENCHMARK = process.env.RUN_NAVIGATION_BENCHMARK === 'true';
const BASE_ORIGIN = (process.env.BENCHMARK_BASE_ORIGIN ?? 'http://127.0.0.1:3100').trim().replace(/\/+$/, '');
const LOCALE_PREFIX = '/vi';
const BASE_URL = `${BASE_ORIGIN}${LOCALE_PREFIX}`;
const INCLUDE_ADMIN_ROUTES = process.env.BENCHMARK_INCLUDE_ADMIN === 'true';

const ADMIN_CREDENTIAL: ScenarioCredential = {
  username: process.env.BENCHMARK_ADMIN_USERNAME?.trim() || process.env.BENCHMARK_USERNAME?.trim() || 'Lucifer',
  password: process.env.BENCHMARK_ADMIN_PASSWORD?.trim() || process.env.BENCHMARK_PASSWORD?.trim() || 'Sontung123!',
};

const READER_CREDENTIAL: ScenarioCredential = {
  username: process.env.BENCHMARK_READER_USERNAME?.trim() || 'Test',
  password: process.env.BENCHMARK_READER_PASSWORD?.trim() || process.env.BENCHMARK_PASSWORD?.trim() || 'Sontung123!',
};

const MAX_PAGES_PER_SCENARIO = Number(process.env.BENCHMARK_MAX_PAGES ?? 90);
const DISCOVERY_LIMIT_PER_PAGE = Number(process.env.BENCHMARK_DISCOVERY_LIMIT ?? 24);
const NAVIGATION_TIMEOUT_MS = 90_000;
const SETTLE_AFTER_NAVIGATION_MS = 2_000;

const REQUEST_COUNT_HIGH_THRESHOLD = 25;
const REQUEST_COUNT_CRITICAL_THRESHOLD = 35;
const SLOW_REQUEST_MEDIUM_THRESHOLD_MS = 400;
const SLOW_REQUEST_HIGH_THRESHOLD_MS = 800;

const OUTPUT_DIR = path.resolve(process.cwd(), 'test-results', 'benchmark');
const OUTPUT_JSON = path.join(OUTPUT_DIR, 'tarotnow-benchmark.json');
const OUTPUT_PAGES_CSV = path.join(OUTPUT_DIR, 'tarotnow-benchmark-pages.csv');
const OUTPUT_REQUESTS_CSV = path.join(OUTPUT_DIR, 'tarotnow-benchmark-requests.csv');
const OUTPUT_MD = path.join(OUTPUT_DIR, 'tarotnow-benchmark-report.md');
const OUTPUT_ANALYSIS_MD = path.join(OUTPUT_DIR, 'tarotnow-benchmark-analysis.md');
const OUTPUT_ROUTE_MAP = path.join(OUTPUT_DIR, 'tarotnow-route-map.json');

const CONTROLLED_SPREAD_TYPES = ['daily_1', 'spread_3', 'spread_5', 'spread_10'] as const;
const MAX_DYNAMIC_READING_SESSIONS = 4;

const CORE_ROUTE_SEEDS = [
  `${LOCALE_PREFIX}`,
  `${LOCALE_PREFIX}/login`,
  `${LOCALE_PREFIX}/register`,
  `${LOCALE_PREFIX}/forgot-password`,
  `${LOCALE_PREFIX}/reset-password`,
  `${LOCALE_PREFIX}/verify-email`,
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
  `${LOCALE_PREFIX}/wallet/deposit/history`,
  `${LOCALE_PREFIX}/wallet/withdraw`,
  `${LOCALE_PREFIX}/notifications`,
  `${LOCALE_PREFIX}/reader/apply`,
  `${LOCALE_PREFIX}/reading/history`,
  `${LOCALE_PREFIX}/legal/tos`,
  `${LOCALE_PREFIX}/legal/privacy`,
  `${LOCALE_PREFIX}/legal/ai-disclaimer`,
] as const;

const VIEWPORT_PROFILES: ViewportProfile[] = [
  {
    id: 'desktop',
    contextOptions: {
      viewport: { width: 1440, height: 900 },
      isMobile: false,
      hasTouch: false,
      deviceScaleFactor: 1,
      ignoreHTTPSErrors: true,
    },
  },
  {
    id: 'mobile',
    contextOptions: {
      viewport: { width: 390, height: 844 },
      isMobile: true,
      hasTouch: true,
      deviceScaleFactor: 3,
      ignoreHTTPSErrors: true,
    },
  },
];

const AUTH_REQUIRED_SCENARIOS: BenchmarkScenario[] = ['logged-in-admin', 'logged-in-reader'];

function isLoopbackHost(hostname: string): boolean {
  return hostname === '127.0.0.1'
    || hostname === 'localhost'
    || hostname === '::1';
}

function assertSafeBenchmarkEnvironment(): void {
  const parsed = new URL(BASE_ORIGIN);
  const allowProductionHost = process.env.ALLOW_PRODUCTION_BENCHMARK === 'true';
  if (!allowProductionHost && !isLoopbackHost(parsed.hostname)) {
    throw new Error(`Unsafe BENCHMARK_BASE_ORIGIN: ${BASE_ORIGIN}. Set ALLOW_PRODUCTION_BENCHMARK=true to override.`);
  }

  if (!ADMIN_CREDENTIAL.username || !ADMIN_CREDENTIAL.password) {
    throw new Error('Missing admin credentials. Provide BENCHMARK_ADMIN_USERNAME/BENCHMARK_ADMIN_PASSWORD.');
  }

  if (!READER_CREDENTIAL.username || !READER_CREDENTIAL.password) {
    throw new Error('Missing reader credentials. Provide BENCHMARK_READER_USERNAME/BENCHMARK_READER_PASSWORD.');
  }
}

function formatNumber(value: number | null, digits = 0): string {
  if (value === null || Number.isNaN(value)) return '-';
  return `${value.toFixed(digits)}`;
}

function toCsvValue(value: unknown): string {
  const raw = value === null || value === undefined ? '' : String(value);
  if (raw.includes(',') || raw.includes('"') || raw.includes('\n')) {
    return `"${raw.replaceAll('"', '""')}"`;
  }
  return raw;
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
  const normalized = withoutQuery.replace(/\/+$/, '') || LOCALE_PREFIX;
  if (!INCLUDE_ADMIN_ROUTES && (normalized === `${LOCALE_PREFIX}/admin` || normalized.startsWith(`${LOCALE_PREFIX}/admin/`))) {
    return null;
  }
  return normalized;
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

function classifyRequestCategory(request: Pick<Request, 'url' | 'resourceType'>): RequestCategory {
  const url = request.url();
  const type = request.resourceType();

  if (type === 'websocket' || type === 'eventsource') {
    return 'websocket';
  }

  if (url.includes('/cdn-cgi/rum') || url.includes('cloudflareinsights.com')) {
    return 'telemetry';
  }

  if (url.includes('/api/')) {
    return 'api';
  }

  const isSameOrigin = (() => {
    try {
      return new URL(url).origin === BASE_ORIGIN;
    } catch {
      return false;
    }
  })();

  if (!isSameOrigin) {
    return 'third-party';
  }

  if (type === 'document') return 'html';

  if (['script', 'stylesheet', 'image', 'font', 'media'].includes(type)) {
    return 'static';
  }

  return 'other';
}

function buildBreakdown(requests: RequestMetric[]): PageBreakdown {
  const breakdown: PageBreakdown = {
    total: requests.length,
    html: 0,
    api: 0,
    static: 0,
    thirdParty: 0,
    telemetry: 0,
    websocket: 0,
    other: 0,
  };

  for (const request of requests) {
    switch (request.category) {
      case 'html':
        breakdown.html += 1;
        break;
      case 'api':
        breakdown.api += 1;
        break;
      case 'static':
        breakdown.static += 1;
        break;
      case 'third-party':
        breakdown.thirdParty += 1;
        break;
      case 'telemetry':
        breakdown.telemetry += 1;
        break;
      case 'websocket':
        breakdown.websocket += 1;
        break;
      default:
        breakdown.other += 1;
        break;
    }
  }

  return breakdown;
}

function deriveRequestSeverity(requestCount: number): SeverityLevel {
  if (requestCount > REQUEST_COUNT_CRITICAL_THRESHOLD) return 'critical';
  if (requestCount > REQUEST_COUNT_HIGH_THRESHOLD) return 'high';
  return 'none';
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

async function safeReadTransferBytes(request: Request): Promise<number> {
  try {
    const sizes = await request.sizes();
    const total = sizes.responseBodySize + sizes.responseHeadersSize;
    return Number.isFinite(total) && total > 0 ? total : 0;
  } catch {
    return 0;
  }
}

function toRequestMetric(record: MutableRequestMetric, scenario: BenchmarkScenario, viewport: BenchmarkViewportId): RequestMetric {
  return {
    scenario,
    viewport,
    route: record.route,
    method: record.method,
    resourceType: record.resourceType,
    initiator: record.initiator,
    category: record.category,
    url: record.url,
    host: record.host,
    status: record.status,
    durationMs: record.durationMs,
    ttfbMs: record.ttfbMs,
    responseBytes: record.responseBytes,
    transferBytes: record.transferBytes,
    failed: record.failed,
    failureText: record.failureText,
    waterfallDepth: record.waterfallDepth,
    pendingAgeMs: record.pendingAgeMs,
    isDuplicate: record.isDuplicate,
  };
}

async function installPaintObservers(page: Page): Promise<void> {
  await page.addInitScript(() => {
    type MetricsWindow = Window & {
      __tnFcp?: number | null;
      __tnLcp?: number | null;
      __tnCls?: number;
      __tnTbt?: number;
    };

    const win = window as MetricsWindow;
    win.__tnFcp = null;
    win.__tnLcp = null;
    win.__tnCls = 0;
    win.__tnTbt = 0;

    try {
      const paintObserver = new PerformanceObserver((list) => {
        for (const entry of list.getEntries()) {
          if (entry.name === 'first-contentful-paint') {
            win.__tnFcp = entry.startTime;
          }
        }
      });
      paintObserver.observe({ type: 'paint', buffered: true });
    } catch {
      // Browser does not support paint observer.
    }

    try {
      const lcpObserver = new PerformanceObserver((list) => {
        const entries = list.getEntries();
        const lastEntry = entries[entries.length - 1];
        if (lastEntry) {
          win.__tnLcp = lastEntry.startTime;
        }
      });
      lcpObserver.observe({ type: 'largest-contentful-paint', buffered: true });
    } catch {
      // Browser does not support LCP observer.
    }

    try {
      const clsObserver = new PerformanceObserver((list) => {
        for (const entry of list.getEntries()) {
          const shift = entry as PerformanceEntry & { hadRecentInput?: boolean; value?: number };
          if (!shift.hadRecentInput) {
            win.__tnCls = (win.__tnCls ?? 0) + (shift.value ?? 0);
          }
        }
      });
      clsObserver.observe({ type: 'layout-shift', buffered: true });
    } catch {
      // Browser does not support CLS observer.
    }

    try {
      const longTaskObserver = new PerformanceObserver((list) => {
        for (const entry of list.getEntries()) {
          const blockingTime = Math.max(0, entry.duration - 50);
          win.__tnTbt = (win.__tnTbt ?? 0) + blockingTime;
        }
      });
      longTaskObserver.observe({ type: 'longtask', buffered: true });
    } catch {
      // Browser does not support longtask observer.
    }
  });
}

async function collectNavigationPaintMetrics(page: Page): Promise<PaintMetricsSnapshot> {
  for (let attempt = 1; attempt <= 4; attempt += 1) {
    try {
      const payload = await page.evaluate(() => {
        const navigationEntry = performance.getEntriesByType('navigation')[0] as PerformanceNavigationTiming | undefined;
        const win = window as Window & {
          __tnFcp?: number | null;
          __tnLcp?: number | null;
          __tnCls?: number;
          __tnTbt?: number;
        };

        return {
          domContentLoadedMs: navigationEntry ? navigationEntry.domContentLoadedEventEnd : null,
          loadMs: navigationEntry && navigationEntry.loadEventEnd > 0 ? navigationEntry.loadEventEnd : null,
          fcpMs: typeof win.__tnFcp === 'number' ? win.__tnFcp : null,
          lcpMs: typeof win.__tnLcp === 'number' ? win.__tnLcp : null,
          cls: typeof win.__tnCls === 'number' ? win.__tnCls : null,
          tbt: typeof win.__tnTbt === 'number' ? win.__tnTbt : null,
        } satisfies PaintMetricsSnapshot;
      });

      return payload;
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error);
      const isContextRace = message.includes('Execution context was destroyed')
        || message.includes('Target page, context or browser has been closed');
      if (!isContextRace || attempt === 4) {
        return {
          domContentLoadedMs: null,
          loadMs: null,
          fcpMs: null,
          lcpMs: null,
          cls: null,
          tbt: null,
        };
      }

      await page.waitForTimeout(250 * attempt);
    }
  }

  return {
    domContentLoadedMs: null,
    loadMs: null,
    fcpMs: null,
    lcpMs: null,
    cls: null,
    tbt: null,
  };
}

async function discoverRoutesFromPage(page: Page): Promise<string[]> {
  let discoveredHrefs: string[] = [];
  for (let attempt = 1; attempt <= 4; attempt += 1) {
    try {
      discoveredHrefs = await page.$$eval('a[href]', (anchors) =>
        anchors
          .map((anchor) => anchor.getAttribute('href') ?? '')
          .filter((href) => href.length > 0),
      );
      break;
    } catch (error) {
      const message = error instanceof Error ? error.message : String(error);
      if (!message.includes('Execution context was destroyed') || attempt === 4) {
        return [];
      }
      await page.waitForTimeout(180 * attempt);
    }
  }

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
      if (!INCLUDE_ADMIN_ROUTES && (route === `${LOCALE_PREFIX}/admin` || route.startsWith(`${LOCALE_PREFIX}/admin/`))) {
        continue;
      }
      staticRoutes.add(route);
    }
  };

  await walk(appLocaleDir);
  return [...staticRoutes].sort((left, right) => left.localeCompare(right));
}

async function loginAsUser(page: Page, credential: ScenarioCredential, expectedRole: string): Promise<{
  success: boolean;
  notes: string[];
}> {
  const notes: string[] = [];
  const hasAuthCookies = async (): Promise<boolean> => {
    const cookies = await page.context().cookies();
    return cookies.some((cookie) => cookie.name === 'accessToken' || cookie.name === 'refreshToken');
  };

  for (let attempt = 1; attempt <= 3; attempt += 1) {
    await page.goto(`${BASE_URL}/login`, {
      waitUntil: 'domcontentloaded',
      timeout: NAVIGATION_TIMEOUT_MS,
    });

    await page.locator('input[name="emailOrUsername"]').fill(credential.username, { timeout: 20_000 });
    await page.locator('input[name="password"]').fill(credential.password, { timeout: 20_000 });

    const loginResponsePromise = page.waitForResponse((response) => {
      return response.url().includes('/api/auth/login') && response.request().method() === 'POST';
    }, { timeout: 20_000 }).then(async (response) => {
      const payload = await response.json().catch(() => ({ success: false })) as { success?: boolean; user?: { role?: string } };
      const normalizedRole = payload.user?.role?.toLowerCase() ?? '';
      const roleMatch = expectedRole.length === 0 || normalizedRole === expectedRole;
      return response.ok() && payload.success === true && roleMatch;
    }).catch(() => false);

    const routeChangedPromise = page.waitForURL((url) => !url.pathname.endsWith('/login'), { timeout: 20_000 })
      .then(() => true)
      .catch(() => false);

    await page.locator('form button[type="submit"]').first().click();
    const [loginResponseOk, routeChanged] = await Promise.all([loginResponsePromise, routeChangedPromise]);
    if (!loginResponseOk && !routeChanged) {
      notes.push(`Attempt ${attempt}: login response and route-change both failed.`);
      continue;
    }

    await page.waitForTimeout(1_000);
    const authCookiesReady = await hasAuthCookies();
    if (!authCookiesReady) {
      notes.push(`Attempt ${attempt}: auth cookies not present after login.`);
      continue;
    }

    try {
      const sessionResponse = await page.request.get(`${BASE_ORIGIN}/api/auth/session`, { timeout: 15_000 });
      const sessionPayload = await sessionResponse.json().catch(() => ({ authenticated: false })) as {
        success?: boolean;
        authenticated?: boolean;
        user?: { role?: string };
      };
      const role = sessionPayload.user?.role?.toLowerCase() ?? '';
      if (!sessionResponse.ok || sessionPayload.success === false || !sessionPayload.authenticated) {
        notes.push(`Attempt ${attempt}: session endpoint not authenticated.`);
        continue;
      }

      if (expectedRole.length > 0 && role !== expectedRole) {
        notes.push(`Attempt ${attempt}: role mismatch (${role || 'unknown'}).`);
        continue;
      }
    } catch {
      notes.push(`Attempt ${attempt}: failed to validate authenticated session.`);
      continue;
    }

    await page.waitForTimeout(1_200);
    notes.push(`Attempt ${attempt}: login bootstrap succeeded.`);
    return { success: true, notes };
  }

  notes.push('Login bootstrap failed after 3 attempts.');
  return { success: false, notes };
}

async function collectConversationIds(page: Page): Promise<string[]> {
  try {
    const response = await page.request.get(`${BASE_ORIGIN}/api/v1/conversations?tab=active&page=1&pageSize=20`, {
      timeout: 12_000,
      failOnStatusCode: false,
    });
    if (!response.ok()) return [];
    const payload = await response.json().catch(() => null) as {
      conversations?: Array<{ id?: string }>;
    } | null;

    return (payload?.conversations ?? [])
      .map((item) => item.id ?? '')
      .filter((id) => id.length > 0)
      .slice(0, 3);
  } catch {
    return [];
  }
}

async function collectReaderIds(page: Page): Promise<string[]> {
  try {
    const response = await page.request.get(`${BASE_ORIGIN}/api/readers?page=1&pageSize=12`, {
      timeout: 12_000,
      failOnStatusCode: false,
    });
    if (!response.ok()) return [];
    const payload = await response.json().catch(() => null) as {
      data?: { readers?: Array<{ id?: string }> };
      readers?: Array<{ id?: string }>;
    } | null;

    const readers = payload?.data?.readers ?? payload?.readers ?? [];
    return readers
      .map((item) => item.id ?? '')
      .filter((id) => id.length > 0)
      .slice(0, 3);
  } catch {
    return [];
  }
}

async function collectReadingHistoryIds(page: Page): Promise<string[]> {
  try {
    const response = await page.request.get(`${BASE_ORIGIN}/api/v1/reading/history?page=1&pageSize=10`, {
      timeout: 12_000,
      failOnStatusCode: false,
    });
    if (!response.ok()) return [];
    const payload = await response.json().catch(() => null) as {
      items?: Array<{ id?: string }>;
      data?: { items?: Array<{ id?: string }> };
    } | null;

    const items = payload?.items ?? payload?.data?.items ?? [];
    return items
      .map((item) => item.id ?? '')
      .filter((id) => id.length > 0)
      .slice(0, 5);
  } catch {
    return [];
  }
}

async function createControlledReadingSessions(page: Page): Promise<{ ids: string[]; notes: string[] }> {
  const ids: string[] = [];
  const notes: string[] = [];

  for (const spreadType of CONTROLLED_SPREAD_TYPES) {
    if (ids.length >= MAX_DYNAMIC_READING_SESSIONS) break;

    try {
      const response = await page.request.post(`${BASE_ORIGIN}/api/reading/init`, {
        data: {
          spreadType,
          question: `Benchmark ${spreadType} ${new Date().toISOString()}`,
          currency: 'gold',
        },
        timeout: 15_000,
        failOnStatusCode: false,
      });

      if (!response.ok()) {
        notes.push(`reading.init.${spreadType}: blocked (${response.status()}).`);
        continue;
      }

      const payload = await response.json().catch(() => null) as { sessionId?: string } | null;
      if (!payload?.sessionId) {
        notes.push(`reading.init.${spreadType}: missing sessionId.`);
        continue;
      }

      ids.push(payload.sessionId);
      notes.push(`reading.init.${spreadType}: created ${payload.sessionId}.`);
    } catch {
      notes.push(`reading.init.${spreadType}: request failed.`);
    }
  }

  return { ids, notes };
}

async function discoverDynamicRoutesForScenario(
  page: Page,
  scenario: BenchmarkScenario,
): Promise<DynamicRouteDiscovery> {
  const coverageNotes: string[] = [];
  if (scenario === 'logged-out') {
    return {
      readingSessionIds: [],
      readingHistoryIds: [],
      readerIds: [],
      chatIds: [],
      coverageNotes: ['dynamic-routes: skipped for logged-out scenario.'],
    };
  }

  const readingSessionIds: string[] = [];
  const readingHistoryIds: string[] = [];
  const readerIds: string[] = [];
  const chatIds: string[] = [];

  const controlledSessions = await createControlledReadingSessions(page);
  readingSessionIds.push(...controlledSessions.ids);
  coverageNotes.push(...controlledSessions.notes);

  const historyIds = await collectReadingHistoryIds(page);
  readingHistoryIds.push(...historyIds);
  if (historyIds.length === 0) {
    coverageNotes.push('reading-history-detail: coverage-blocked (no history id found).');
  }

  const discoveredReaders = await collectReaderIds(page);
  readerIds.push(...discoveredReaders);
  if (discoveredReaders.length === 0) {
    coverageNotes.push('reader-detail: coverage-blocked (no reader id found).');
  }

  const conversationIds = await collectConversationIds(page);
  chatIds.push(...conversationIds);
  if (conversationIds.length === 0) {
    coverageNotes.push('chat-room-detail: coverage-blocked (no conversation id found).');
  }

  if (readingSessionIds.length === 0) {
    coverageNotes.push('reading-session-detail: coverage-blocked (could not create session ids).');
  }

  return {
    readingSessionIds: [...new Set(readingSessionIds)].slice(0, 6),
    readingHistoryIds: [...new Set(readingHistoryIds)].slice(0, 6),
    readerIds: [...new Set(readerIds)].slice(0, 6),
    chatIds: [...new Set(chatIds)].slice(0, 6),
    coverageNotes,
  };
}

function buildSeedRoutes(baseRoutes: string[], dynamic: DynamicRouteDiscovery): string[] {
  const routes = new Set<string>(baseRoutes);

  for (const id of dynamic.readingSessionIds) {
    routes.add(`${LOCALE_PREFIX}/reading/session/${id}`);
  }

  for (const id of dynamic.readingHistoryIds) {
    routes.add(`${LOCALE_PREFIX}/reading/history/${id}`);
  }

  for (const id of dynamic.readerIds) {
    routes.add(`${LOCALE_PREFIX}/readers/${id}`);
  }

  for (const id of dynamic.chatIds) {
    routes.add(`${LOCALE_PREFIX}/chat/${id}`);
  }

  return [...routes];
}

function resolveScenarioCredential(scenario: BenchmarkScenario): ScenarioCredential | null {
  if (scenario === 'logged-in-admin') return ADMIN_CREDENTIAL;
  if (scenario === 'logged-in-reader') return READER_CREDENTIAL;
  return null;
}

function resolveExpectedRole(scenario: BenchmarkScenario): string {
  if (scenario === 'logged-in-admin') return 'admin';
  if (scenario === 'logged-in-reader') return 'tarot_reader';
  return '';
}

async function benchmarkNavigation(
  page: Page,
  scenario: BenchmarkScenario,
  viewport: BenchmarkViewportId,
  route: string,
  navFrom: string | null,
): Promise<PageBenchmarkMetric> {
  const routeUrl = `${BASE_ORIGIN}${route}`;
  const requests = new Map<Request, MutableRequestMetric>();
  const responseMap = new Map<Request, Response>();
  const finalizeTasks: Array<Promise<void>> = [];
  const coverageBlocked: string[] = [];
  let inFlightCounter = 0;

  const onRoute = async (routeHandler: Parameters<Parameters<Page['route']>[1]>[0]): Promise<void> => {
    const request = routeHandler.request();
    const existing = requests.get(request);
    if (!existing) {
      const requestUrl = request.url();
      let host = '';
      try {
        host = new URL(requestUrl).host;
      } catch {
        host = '';
      }

      inFlightCounter += 1;
      requests.set(request, {
        route,
        method: request.method(),
        resourceType: request.resourceType(),
        initiator: request.resourceType(),
        category: classifyRequestCategory(request),
        url: requestUrl,
        host,
        status: null,
        startedAtMs: Date.now(),
        finishedAtMs: null,
        durationMs: null,
        ttfbMs: null,
        responseBytes: 0,
        transferBytes: 0,
        failed: false,
        failureText: null,
        waterfallDepth: inFlightCounter,
        pendingAgeMs: null,
        isDuplicate: false,
      });
    }

    await routeHandler.continue();
  };

  const onRequest = (request: Request): void => {
    if (!requests.has(request)) {
      const requestUrl = request.url();
      let host = '';
      try {
        host = new URL(requestUrl).host;
      } catch {
        host = '';
      }

      inFlightCounter += 1;
      requests.set(request, {
        route,
        method: request.method(),
        resourceType: request.resourceType(),
        initiator: request.resourceType(),
        category: classifyRequestCategory(request),
        url: requestUrl,
        host,
        status: null,
        startedAtMs: Date.now(),
        finishedAtMs: null,
        durationMs: null,
        ttfbMs: null,
        responseBytes: 0,
        transferBytes: 0,
        failed: false,
        failureText: null,
        waterfallDepth: inFlightCounter,
        pendingAgeMs: null,
        isDuplicate: false,
      });
    }
  };

  const onResponse = (response: Response): void => {
    const request = response.request();
    responseMap.set(request, response);

    const record = requests.get(request);
    if (!record) return;

    const timing = request.timing();
    record.status = response.status();
    record.ttfbMs = timing.requestStart >= 0 && timing.responseStart >= 0
      ? Math.max(0, timing.responseStart - timing.requestStart)
      : null;
  };

  const onRequestFinished = (request: Request): void => {
    const pendingTask = (async () => {
      const record = requests.get(request);
      if (!record || record.finishedAtMs !== null) return;

      const response = responseMap.get(request) ?? await request.response();
      const timing = request.timing();

      record.status = response?.status() ?? null;
      record.finishedAtMs = Date.now();
      record.durationMs = Math.max(0, record.finishedAtMs - record.startedAtMs);
      record.ttfbMs = timing.requestStart >= 0 && timing.responseStart >= 0
        ? Math.max(0, timing.responseStart - timing.requestStart)
        : record.ttfbMs;
      record.responseBytes = await safeReadResponseBytes(response);
      record.transferBytes = await safeReadTransferBytes(request);
      inFlightCounter = Math.max(0, inFlightCounter - 1);
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
    inFlightCounter = Math.max(0, inFlightCounter - 1);
  };

  await page.route('**/*', onRoute);
  page.on('request', onRequest);
  page.on('response', onResponse);
  page.on('requestfinished', onRequestFinished);
  page.on('requestfailed', onRequestFailed);

  const startedAtMs = Date.now();
  try {
    await page.goto(routeUrl, {
      waitUntil: 'domcontentloaded',
      timeout: NAVIGATION_TIMEOUT_MS,
    });
  } catch {
    coverageBlocked.push(`navigation-failed:${route}`);
  }

  try {
    await page.waitForLoadState('load', { timeout: 15_000 });
  } catch {
    coverageBlocked.push(`load-timeout:${route}`);
  }

  await page.waitForTimeout(SETTLE_AFTER_NAVIGATION_MS);
  await Promise.allSettled(finalizeTasks);

  page.off('request', onRequest);
  page.off('response', onResponse);
  page.off('requestfinished', onRequestFinished);
  page.off('requestfailed', onRequestFailed);
  await page.unroute('**/*', onRoute);

  const navigationMs = Date.now() - startedAtMs;
  const finalUrl = page.url();
  const paintMetrics = await collectNavigationPaintMetrics(page);
  const settledAtMs = Date.now();

  const requestRecords = [...requests.values()];

  const duplicateCounter = new Map<string, number>();
  for (const request of requestRecords) {
    const key = normalizeRequestKey(request.method, request.url);
    duplicateCounter.set(key, (duplicateCounter.get(key) ?? 0) + 1);
  }

  const duplicateSet = new Set<string>(
    [...duplicateCounter.entries()].filter(([, count]) => count > 1).map(([key]) => key),
  );

  for (const request of requestRecords) {
    const key = normalizeRequestKey(request.method, request.url);
    request.isDuplicate = duplicateSet.has(key);
    if (request.finishedAtMs === null && !request.failed) {
      request.pendingAgeMs = Math.max(0, settledAtMs - request.startedAtMs);
    }
  }

  const requestMetrics = requestRecords.map((record) => toRequestMetric(record, scenario, viewport));

  const pendingNonPersistent = requestMetrics.filter((request) =>
    request.durationMs === null
    && !request.failed
    && request.resourceType !== 'websocket'
    && request.resourceType !== 'eventsource',
  );

  const slowRequests = requestMetrics.filter((request) => (request.durationMs ?? 0) > SLOW_REQUEST_MEDIUM_THRESHOLD_MS);
  const totalResponseBytes = requestMetrics.reduce((sum, request) => sum + request.responseBytes, 0);
  const totalTransferBytes = requestMetrics.reduce((sum, request) => sum + request.transferBytes, 0);

  const duplicateRequestGroups = [...duplicateCounter.entries()]
    .filter(([, count]) => count > 1)
    .sort((left, right) => right[1] - left[1])
    .map(([key, count]) => ({ key, count }));

  const thirdPartyCounter = new Map<string, number>();
  for (const request of requestMetrics) {
    if (request.category === 'third-party' || request.category === 'telemetry') {
      const host = request.host || 'unknown';
      thirdPartyCounter.set(host, (thirdPartyCounter.get(host) ?? 0) + 1);
    }
  }

  const thirdPartyBreakdown = [...thirdPartyCounter.entries()]
    .sort((left, right) => right[1] - left[1])
    .slice(0, 20)
    .map(([host, count]) => ({ host, count }));

  return {
    scenario,
    viewport,
    route,
    navFrom,
    finalUrl,
    navigationMs,
    domContentLoadedMs: paintMetrics.domContentLoadedMs,
    loadMs: paintMetrics.loadMs,
    fcpMs: paintMetrics.fcpMs,
    lcpMs: paintMetrics.lcpMs,
    cls: paintMetrics.cls,
    tbt: paintMetrics.tbt,
    requestCount: requestMetrics.length,
    requestSeverity: deriveRequestSeverity(requestMetrics.length),
    pendingCount: pendingNonPersistent.length,
    pendingUrls: pendingNonPersistent.map((request) => request.url),
    totalResponseBytes,
    totalTransferBytes,
    requestBreakdown: buildBreakdown(requestMetrics),
    thirdPartyBreakdown,
    duplicateRequestGroups,
    slowRequests,
    requests: requestMetrics,
    coverageBlocked,
  };
}

async function runScenario(
  browser: Browser,
  scenario: BenchmarkScenario,
  viewportProfile: ViewportProfile,
  seedRoutes: string[],
): Promise<ScenarioBenchmarkResult> {
  const context = await browser.newContext(viewportProfile.contextOptions);
  const page = await context.newPage();
  await installPaintObservers(page);

  let loginBootstrapSucceeded = true;
  let loginBootstrapNotes: string[] = [];
  const coverageNotes: string[] = [];

  const credential = resolveScenarioCredential(scenario);
  if (credential) {
    const loginResult = await loginAsUser(page, credential, resolveExpectedRole(scenario));
    loginBootstrapSucceeded = loginResult.success;
    loginBootstrapNotes = loginResult.notes;
  }

  const dynamicRoutes = await discoverDynamicRoutesForScenario(page, scenario);
  coverageNotes.push(...dynamicRoutes.coverageNotes);

  const queue: string[] = buildSeedRoutes(seedRoutes, dynamicRoutes);
  const visited = new Set<string>();
  const pageMetrics: PageBenchmarkMetric[] = [];
  let navFrom: string | null = null;

  while (queue.length > 0 && visited.size < MAX_PAGES_PER_SCENARIO) {
    const nextRoute = queue.shift();
    if (!nextRoute) break;

    const normalizedRoute = normalizeRoutePath(nextRoute);
    if (!normalizedRoute || visited.has(normalizedRoute)) {
      continue;
    }

    const metric = await benchmarkNavigation(page, scenario, viewportProfile.id, normalizedRoute, navFrom);
    pageMetrics.push(metric);
    visited.add(normalizedRoute);
    navFrom = normalizedRoute;

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
    viewport: viewportProfile.id,
    pages: pageMetrics,
    visitedRoutes: [...visited],
    loginBootstrapSucceeded,
    loginBootstrapNotes,
    coverageNotes,
  };
}

function createPagesCsv(result: BenchmarkRunResult): string {
  const lines: string[] = [];
  lines.push([
    'scenario',
    'viewport',
    'route',
    'final_url',
    'request_count',
    'request_severity',
    'pending_count',
    'navigation_ms',
    'dom_content_loaded_ms',
    'load_ms',
    'fcp_ms',
    'lcp_ms',
    'cls',
    'tbt_ms',
    'response_bytes',
    'transfer_bytes',
    'html',
    'api',
    'static',
    'third_party',
    'telemetry',
    'websocket',
    'other',
  ].join(','));

  for (const scenario of result.scenarios) {
    for (const page of scenario.pages) {
      lines.push([
        toCsvValue(page.scenario),
        toCsvValue(page.viewport),
        toCsvValue(page.route),
        toCsvValue(page.finalUrl),
        toCsvValue(page.requestCount),
        toCsvValue(page.requestSeverity),
        toCsvValue(page.pendingCount),
        toCsvValue(formatNumber(page.navigationMs)),
        toCsvValue(formatNumber(page.domContentLoadedMs)),
        toCsvValue(formatNumber(page.loadMs)),
        toCsvValue(formatNumber(page.fcpMs)),
        toCsvValue(formatNumber(page.lcpMs)),
        toCsvValue(formatNumber(page.cls, 4)),
        toCsvValue(formatNumber(page.tbt, 1)),
        toCsvValue(page.totalResponseBytes),
        toCsvValue(page.totalTransferBytes),
        toCsvValue(page.requestBreakdown.html),
        toCsvValue(page.requestBreakdown.api),
        toCsvValue(page.requestBreakdown.static),
        toCsvValue(page.requestBreakdown.thirdParty),
        toCsvValue(page.requestBreakdown.telemetry),
        toCsvValue(page.requestBreakdown.websocket),
        toCsvValue(page.requestBreakdown.other),
      ].join(','));
    }
  }

  return lines.join('\n');
}

function createRequestsCsv(result: BenchmarkRunResult): string {
  const lines: string[] = [];
  lines.push([
    'scenario',
    'viewport',
    'route',
    'method',
    'status',
    'resource_type',
    'category',
    'duration_ms',
    'ttfb_ms',
    'response_bytes',
    'transfer_bytes',
    'waterfall_depth',
    'pending_age_ms',
    'duplicate',
    'failed',
    'url',
    'failure_text',
  ].join(','));

  for (const scenario of result.scenarios) {
    for (const page of scenario.pages) {
      for (const request of page.requests) {
        lines.push([
          toCsvValue(request.scenario),
          toCsvValue(request.viewport),
          toCsvValue(request.route),
          toCsvValue(request.method),
          toCsvValue(request.status ?? ''),
          toCsvValue(request.resourceType),
          toCsvValue(request.category),
          toCsvValue(formatNumber(request.durationMs)),
          toCsvValue(formatNumber(request.ttfbMs)),
          toCsvValue(request.responseBytes),
          toCsvValue(request.transferBytes),
          toCsvValue(request.waterfallDepth),
          toCsvValue(formatNumber(request.pendingAgeMs)),
          toCsvValue(request.isDuplicate ? 'yes' : 'no'),
          toCsvValue(request.failed ? 'yes' : 'no'),
          toCsvValue(request.url),
          toCsvValue(request.failureText ?? ''),
        ].join(','));
      }
    }
  }

  return lines.join('\n');
}

function createMarkdownReport(result: BenchmarkRunResult): string {
  const allPages = result.scenarios.flatMap((scenario) => scenario.pages);
  const suspiciousPages = allPages.filter((page) => page.requestCount > REQUEST_COUNT_HIGH_THRESHOLD);
  const criticalPages = allPages.filter((page) => page.requestCount > REQUEST_COUNT_CRITICAL_THRESHOLD);
  const pendingPages = allPages.filter((page) => page.pendingCount > 0);

  const highSlowRequests = allPages.flatMap((page) =>
    page.slowRequests
      .filter((request) => (request.durationMs ?? 0) > SLOW_REQUEST_HIGH_THRESHOLD_MS)
      .map((request) => ({ ...request, scenario: page.scenario, viewport: page.viewport, route: page.route })),
  );

  const mediumSlowRequests = allPages.flatMap((page) =>
    page.slowRequests
      .filter((request) => {
        const duration = request.durationMs ?? 0;
        return duration > SLOW_REQUEST_MEDIUM_THRESHOLD_MS && duration <= SLOW_REQUEST_HIGH_THRESHOLD_MS;
      })
      .map((request) => ({ ...request, scenario: page.scenario, viewport: page.viewport, route: page.route })),
  );

  const summaryLines = result.scenarios.map((scenario) => {
    const pageCount = scenario.pages.length;
    const avgNavigation = pageCount > 0
      ? scenario.pages.reduce((sum, page) => sum + page.navigationMs, 0) / pageCount
      : 0;
    const totalRequests = scenario.pages.reduce((sum, page) => sum + page.requestCount, 0);
    const pending = scenario.pages.reduce((sum, page) => sum + page.pendingCount, 0);
    return `| ${scenario.scenario} | ${scenario.viewport} | ${pageCount} | ${avgNavigation.toFixed(0)} | ${totalRequests} | ${pending} | ${scenario.loginBootstrapSucceeded ? 'yes' : 'no'} |`;
  });

  const pageLines = allPages.map((page) =>
    `| ${page.scenario} | ${page.viewport} | ${page.route} | ${page.requestCount} | ${page.requestSeverity} | ${formatNumber(page.navigationMs)} | ${formatNumber(page.domContentLoadedMs)} | ${formatNumber(page.loadMs)} | ${formatNumber(page.fcpMs)} | ${formatNumber(page.lcpMs)} | ${formatNumber(page.cls, 4)} | ${formatNumber(page.tbt, 1)} | ${page.totalTransferBytes} |`,
  );

  const slowHighLines = highSlowRequests.length > 0
    ? highSlowRequests
      .sort((left, right) => (right.durationMs ?? 0) - (left.durationMs ?? 0))
      .slice(0, 80)
      .map((request) =>
        `| ${request.scenario} | ${request.viewport} | ${request.route} | ${request.method} | ${request.status ?? '-'} | ${formatNumber(request.durationMs)} | ${formatNumber(request.ttfbMs)} | ${request.category} | ${request.url} |`,
      )
    : ['| - | - | - | - | - | - | - | - | - |'];

  const slowMediumLines = mediumSlowRequests.length > 0
    ? mediumSlowRequests
      .sort((left, right) => (right.durationMs ?? 0) - (left.durationMs ?? 0))
      .slice(0, 80)
      .map((request) =>
        `| ${request.scenario} | ${request.viewport} | ${request.route} | ${request.method} | ${request.status ?? '-'} | ${formatNumber(request.durationMs)} | ${formatNumber(request.ttfbMs)} | ${request.category} | ${request.url} |`,
      )
    : ['| - | - | - | - | - | - | - | - | - |'];

  const suspiciousLines = suspiciousPages.length > 0
    ? suspiciousPages.map((page) =>
      `| ${page.scenario} | ${page.viewport} | ${page.route} | ${page.requestCount} | ${page.requestSeverity} | ${page.requestBreakdown.api} | ${page.requestBreakdown.static} | ${page.requestBreakdown.thirdParty} |`,
    )
    : ['| - | - | - | - | - | - | - | - |'];

  const pendingLines = pendingPages.length > 0
    ? pendingPages.flatMap((page) =>
      page.pendingUrls.map((url) =>
        `| ${page.scenario} | ${page.viewport} | ${page.route} | ${url} |`,
      ),
    )
    : ['| - | - | - | - |'];

  const coverageLines = result.scenarios.flatMap((scenario) =>
    scenario.coverageNotes.map((note) => `| ${scenario.scenario} | ${scenario.viewport} | ${note} |`),
  );

  return [
    '# TarotNow Navigation Benchmark Report',
    '',
    `- Generated at (UTC): ${result.generatedAtUtc}`,
    `- Base URL: ${BASE_URL}`,
    `- Thresholds: >${REQUEST_COUNT_CRITICAL_THRESHOLD} requests = Critical, >${REQUEST_COUNT_HIGH_THRESHOLD} = High, request >${SLOW_REQUEST_HIGH_THRESHOLD_MS}ms = High, >${SLOW_REQUEST_MEDIUM_THRESHOLD_MS}ms = Medium`,
    `- Critical pages (request count): ${criticalPages.length}`,
    `- High pages (request count): ${suspiciousPages.length}`,
    `- High slow requests: ${highSlowRequests.length}`,
    `- Medium slow requests: ${mediumSlowRequests.length}`,
    '',
    '## Scenario Summary',
    '| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Login bootstrap |',
    '| --- | --- | ---: | ---: | ---: | ---: | --- |',
    ...summaryLines,
    '',
    '## Per-Page Metrics',
    '| Scenario | Viewport | Route | Requests | Severity | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |',
    '| --- | --- | --- | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |',
    ...pageLines,
    '',
    '## Suspicious Pages (>25 requests)',
    '| Scenario | Viewport | Route | Request Count | Severity | API | Static | Third-party |',
    '| --- | --- | --- | ---: | --- | ---: | ---: | ---: |',
    ...suspiciousLines,
    '',
    '## High Slow Requests (>800ms)',
    '| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |',
    '| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |',
    ...slowHighLines,
    '',
    '## Medium Slow Requests (400-800ms)',
    '| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |',
    '| --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |',
    ...slowMediumLines,
    '',
    '## Pending Requests',
    '| Scenario | Viewport | Route | URL |',
    '| --- | --- | --- | --- |',
    ...pendingLines,
    '',
    '## Coverage Notes',
    '| Scenario | Viewport | Note |',
    '| --- | --- | --- |',
    ...(coverageLines.length > 0 ? coverageLines : ['| - | - | no coverage notes |']),
    '',
    '## Login Bootstrap Notes',
    ...(result.scenarios.flatMap((scenario) =>
      scenario.loginBootstrapNotes.length > 0
        ? [
          `### ${scenario.scenario} / ${scenario.viewport}`,
          ...scenario.loginBootstrapNotes.map((note) => `- ${note}`),
          '',
        ]
        : []
    )),
  ].join('\n');
}

function createAnalysisReport(result: BenchmarkRunResult): string {
  const allPages = result.scenarios.flatMap((scenario) => scenario.pages);
  const highRequestPages = allPages.filter((page) => page.requestCount > REQUEST_COUNT_HIGH_THRESHOLD);
  const criticalRequestPages = allPages.filter((page) => page.requestCount > REQUEST_COUNT_CRITICAL_THRESHOLD);
  const highSlowRequests = allPages
    .flatMap((page) => page.slowRequests.map((request) => ({ ...request, scenario: page.scenario, viewport: page.viewport, route: page.route })))
    .filter((request) => (request.durationMs ?? 0) > SLOW_REQUEST_HIGH_THRESHOLD_MS)
    .sort((left, right) => (right.durationMs ?? 0) - (left.durationMs ?? 0));
  const mediumSlowRequests = allPages
    .flatMap((page) => page.slowRequests.map((request) => ({ ...request, scenario: page.scenario, viewport: page.viewport, route: page.route })))
    .filter((request) => {
      const duration = request.durationMs ?? 0;
      return duration > SLOW_REQUEST_MEDIUM_THRESHOLD_MS && duration <= SLOW_REQUEST_HIGH_THRESHOLD_MS;
    })
    .sort((left, right) => (right.durationMs ?? 0) - (left.durationMs ?? 0));

  const topSlowPages = [...allPages]
    .sort((left, right) => right.navigationMs - left.navigationMs)
    .slice(0, 10);

  const duplicateSummary = allPages
    .flatMap((page) => page.duplicateRequestGroups.map((group) => ({ ...group, scenario: page.scenario, viewport: page.viewport, route: page.route })))
    .filter((entry) => !entry.key.includes('/cdn-cgi/rum'))
    .sort((left, right) => right.count - left.count)
    .slice(0, 20);

  const scenarioLines = result.scenarios.map((scenario) => {
    const totalRequests = scenario.pages.reduce((sum, page) => sum + page.requestCount, 0);
    const avgNavigationMs = scenario.pages.length > 0
      ? scenario.pages.reduce((sum, page) => sum + page.navigationMs, 0) / scenario.pages.length
      : 0;
    const avgRequestsPerPage = scenario.pages.length > 0 ? totalRequests / scenario.pages.length : 0;
    const pendingCount = scenario.pages.reduce((sum, page) => sum + page.pendingCount, 0);
    return `| ${scenario.scenario} | ${scenario.viewport} | ${scenario.pages.length} | ${avgRequestsPerPage.toFixed(1)} | ${avgNavigationMs.toFixed(0)} | ${pendingCount} | ${scenario.loginBootstrapSucceeded ? 'yes' : 'no'} |`;
  });

  const topSlowPageLines = topSlowPages.map((page) =>
    `| ${page.scenario} | ${page.viewport} | ${page.route} | ${page.navigationMs} | ${page.requestCount} | ${formatNumber(page.lcpMs)} | ${formatNumber(page.tbt, 1)} | ${formatNumber(page.cls, 4)} |`,
  );

  const highSlowRequestLines = highSlowRequests.slice(0, 15).map((request) =>
    `| ${request.scenario} | ${request.viewport} | ${request.route} | ${request.method} | ${request.status ?? '-'} | ${formatNumber(request.durationMs)} | ${formatNumber(request.ttfbMs)} | ${request.url} |`,
  );

  const mediumSlowRequestLines = mediumSlowRequests.slice(0, 15).map((request) =>
    `| ${request.scenario} | ${request.viewport} | ${request.route} | ${request.method} | ${request.status ?? '-'} | ${formatNumber(request.durationMs)} | ${formatNumber(request.ttfbMs)} | ${request.url} |`,
  );

  const duplicateLines = duplicateSummary.map((entry) =>
    `| ${entry.scenario} | ${entry.viewport} | ${entry.route} | ${entry.count} | ${entry.key} |`,
  );

  const keyFindings = [
    criticalRequestPages.length > 0
      ? `Critical: ${criticalRequestPages.length} page(s) vượt ${REQUEST_COUNT_CRITICAL_THRESHOLD} requests.`
      : `Không có page nào vượt ${REQUEST_COUNT_CRITICAL_THRESHOLD} requests.`,
    highRequestPages.length > 0
      ? `High: ${highRequestPages.length} page(s) vượt ${REQUEST_COUNT_HIGH_THRESHOLD} requests.`
      : `Không có page nào vượt ${REQUEST_COUNT_HIGH_THRESHOLD} requests.`,
    highSlowRequests.length > 0
      ? `High: ${highSlowRequests.length} request vượt ${SLOW_REQUEST_HIGH_THRESHOLD_MS}ms.`
      : `Không có request nào vượt ${SLOW_REQUEST_HIGH_THRESHOLD_MS}ms.`,
    mediumSlowRequests.length > 0
      ? `Medium: ${mediumSlowRequests.length} request trong dải ${SLOW_REQUEST_MEDIUM_THRESHOLD_MS}-${SLOW_REQUEST_HIGH_THRESHOLD_MS}ms.`
      : `Không có request trong dải ${SLOW_REQUEST_MEDIUM_THRESHOLD_MS}-${SLOW_REQUEST_HIGH_THRESHOLD_MS}ms.`,
  ];

  return [
    '# TarotNow Benchmark Analysis',
    '',
    `- Run time (UTC): ${result.generatedAtUtc}`,
    `- Base: ${BASE_URL}`,
    '- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader',
    '',
    '## Scenario Summary',
    '| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Login bootstrap |',
    '| --- | --- | ---: | ---: | ---: | ---: | --- |',
    ...scenarioLines,
    '',
    '## Key Findings',
    ...keyFindings.map((item, index) => `${index + 1}. ${item}`),
    '',
    '## Top Slow Pages',
    '| Scenario | Viewport | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |',
    '| --- | --- | --- | ---: | ---: | ---: | ---: | ---: |',
    ...(topSlowPageLines.length > 0 ? topSlowPageLines : ['| - | - | - | - | - | - | - | - |']),
    '',
    `## High Slow Requests (> ${SLOW_REQUEST_HIGH_THRESHOLD_MS}ms)`,
    '| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |',
    '| --- | --- | --- | --- | ---: | ---: | ---: | --- |',
    ...(highSlowRequestLines.length > 0 ? highSlowRequestLines : ['| - | - | - | - | - | - | - | - |']),
    '',
    `## Medium Slow Requests (${SLOW_REQUEST_MEDIUM_THRESHOLD_MS}-${SLOW_REQUEST_HIGH_THRESHOLD_MS}ms)`,
    '| Scenario | Viewport | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |',
    '| --- | --- | --- | --- | ---: | ---: | ---: | --- |',
    ...(mediumSlowRequestLines.length > 0 ? mediumSlowRequestLines : ['| - | - | - | - | - | - | - | - |']),
    '',
    '## Duplicate Request Candidates (Non-telemetry)',
    '| Scenario | Viewport | Route | Count | Request Key |',
    '| --- | --- | --- | ---: | --- |',
    ...(duplicateLines.length > 0 ? duplicateLines : ['| - | - | - | - | - |']),
    '',
    '## Notes',
    '- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.',
    '- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.',
  ].join('\n');
}

async function writeRouteMap(result: BenchmarkRunResult): Promise<void> {
  const map = result.scenarios.map((scenario) => ({
    scenario: scenario.scenario,
    viewport: scenario.viewport,
    visitedRoutes: scenario.visitedRoutes,
    coverageNotes: scenario.coverageNotes,
  }));

  await fs.writeFile(OUTPUT_ROUTE_MAP, JSON.stringify(map, null, 2), 'utf8');
}

test.describe('TarotNow production benchmark', () => {
  test.describe.configure({ mode: 'serial' });
  test.setTimeout(45 * 60 * 1000);

  test('benchmark navigation and api timing for full vi route matrix', async ({ browser }) => {
    test.skip(!RUN_NAVIGATION_BENCHMARK, 'Set RUN_NAVIGATION_BENCHMARK=true to run benchmark.');
    assertSafeBenchmarkEnvironment();

    const staticRoutes = await collectStaticLocaleRoutes();
    const seedRoutes = [...new Set([...CORE_ROUTE_SEEDS, ...staticRoutes])];

    const scenarios: BenchmarkScenario[] = ['logged-out', 'logged-in-admin', 'logged-in-reader'];
    const scenarioResults: ScenarioBenchmarkResult[] = [];

    for (const viewport of VIEWPORT_PROFILES) {
      for (const scenario of scenarios) {
        const result = await runScenario(browser, scenario, viewport, seedRoutes);
        scenarioResults.push(result);
      }
    }

    const runResult: BenchmarkRunResult = {
      generatedAtUtc: new Date().toISOString(),
      baseOrigin: BASE_ORIGIN,
      localePrefix: LOCALE_PREFIX,
      thresholds: {
        requestCountHigh: REQUEST_COUNT_HIGH_THRESHOLD,
        requestCountCritical: REQUEST_COUNT_CRITICAL_THRESHOLD,
        slowRequestMediumMs: SLOW_REQUEST_MEDIUM_THRESHOLD_MS,
        slowRequestHighMs: SLOW_REQUEST_HIGH_THRESHOLD_MS,
      },
      scenarios: scenarioResults,
    };

    await fs.mkdir(OUTPUT_DIR, { recursive: true });
    await fs.writeFile(OUTPUT_JSON, JSON.stringify(runResult, null, 2), 'utf8');
    await fs.writeFile(OUTPUT_PAGES_CSV, createPagesCsv(runResult), 'utf8');
    await fs.writeFile(OUTPUT_REQUESTS_CSV, createRequestsCsv(runResult), 'utf8');
    await fs.writeFile(OUTPUT_MD, createMarkdownReport(runResult), 'utf8');
    await fs.writeFile(OUTPUT_ANALYSIS_MD, createAnalysisReport(runResult), 'utf8');
    await writeRouteMap(runResult);

    for (const scenario of AUTH_REQUIRED_SCENARIOS) {
      for (const viewport of VIEWPORT_PROFILES) {
        const record = runResult.scenarios.find((item) => item.scenario === scenario && item.viewport === viewport.id);
        expect(record?.loginBootstrapSucceeded ?? false).toBeTruthy();
      }
    }

    for (const scenario of runResult.scenarios) {
      expect(scenario.pages.length).toBeGreaterThan(0);
    }
  });
});
