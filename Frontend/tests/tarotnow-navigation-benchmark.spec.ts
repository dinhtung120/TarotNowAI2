import { promises as fs } from 'node:fs';
import path from 'node:path';
import { expect, test, type Browser, type Page, type Request, type Response } from '@playwright/test';

type BenchmarkScenario = 'logged-out' | 'logged-in-admin' | 'logged-in-reader';
type BenchmarkViewportId = 'desktop' | 'mobile';
type BenchmarkMode = 'full-matrix' | 'targeted-hotspots' | 'feature-matrix';
type BenchmarkFeature =
  | 'auth-public'
  | 'reading'
  | 'inventory-gacha-collection'
  | 'profile-wallet-notifications'
  | 'reader-chat'
  | 'community-leaderboard-quest'
  | 'admin'
  | 'other';

type RequestCategory = 'html' | 'api' | 'static' | 'third-party' | 'telemetry' | 'websocket' | 'other';
type RequestFailureClass = 'none' | 'app-regression' | 'server-rate-limit' | 'expected-navigation-abort' | 'external-telemetry';
type SeverityLevel = 'critical' | 'high' | 'medium' | 'low' | 'none';

interface DuplicateRequestGroup {
  key: string;
  count: number;
}

interface RequestMetric {
  scenario: BenchmarkScenario;
  viewport: BenchmarkViewportId;
  route: string;
  feature: BenchmarkFeature;
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
  failureClass: RequestFailureClass;
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
  feature: BenchmarkFeature;
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
  interactionRequestCount: number;
  requestSeverity: SeverityLevel;
  documentReloadCount: number;
  handshakeRedirectCount: number;
  sessionApiCallCount: number;
  failedRequestCount: number;
  collectionImageRequestCount: number;
  collectionImageSlowRequestMediumCount: number;
  collectionImageSlowRequestHighCount: number;
  collectionImageFirstLoadRequestCount: number;
  collectionImageReopenRequestCount: number;
  collectionImageCacheHitCount: number;
  pendingCount: number;
  pendingUrls: string[];
  totalResponseBytes: number;
  totalTransferBytes: number;
  requestBreakdown: PageBreakdown;
  thirdPartyBreakdown: Array<{ host: string; count: number }>;
  duplicateRequestGroups: DuplicateRequestGroup[];
  slowRequests: RequestMetric[];
  requests: RequestMetric[];
  interactionNotes: string[];
  coverageBlocked: string[];
}

interface RouteInteractionProbeResult {
  notes: string[];
  collectionReloadStartedAtMs: number | null;
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
  runId: string;
  artifactPaths?: {
    immutableJson: string;
    latestJson: string;
    legacyJson: string;
  };
  baseOrigin: string;
  localePrefix: string;
  benchmarkMode: BenchmarkMode;
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
  feature: BenchmarkFeature;
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
  failureClass: RequestFailureClass;
  waterfallDepth: number;
  pendingAgeMs: number | null;
  isDuplicate: boolean;
}

interface DynamicRouteDiscovery {
  readingSessionIds: string[];
  readingHistoryIds: string[];
  readerIds: string[];
  chatIds: string[];
  communityPostIds: string[];
  coverageNotes: string[];
}

interface RouteCollectionResult {
  routes: string[];
  notes: string[];
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
const RUN_NAVIGATION_BENCHMARK_TARGETED = process.env.RUN_NAVIGATION_BENCHMARK_TARGETED === 'true';
const RUN_NAVIGATION_BENCHMARK_FEATURE = process.env.RUN_NAVIGATION_BENCHMARK_FEATURE === 'true';
const BENCHMARK_FEATURE = (process.env.BENCHMARK_FEATURE?.trim() || 'inventory-gacha-collection') as BenchmarkFeature;
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

const OUTPUT_DIR = path.resolve(process.cwd(), 'benchmark-results', 'benchmark');
const OUTPUT_RUN_ID = (process.env.BENCHMARK_RUN_ID?.trim() || new Date().toISOString().replace(/[:.]/g, '-'));
const OUTPUT_RUN_DIR = path.join(OUTPUT_DIR, 'runs', OUTPUT_RUN_ID);
const OUTPUT_LATEST_DIR = path.join(OUTPUT_DIR, 'latest');
const OUTPUT_INDEX_JSON = path.join(OUTPUT_DIR, 'index.json');
const OUTPUT_JSON = path.join(OUTPUT_DIR, 'tarotnow-benchmark.json');
const OUTPUT_PAGES_CSV = path.join(OUTPUT_DIR, 'tarotnow-benchmark-pages.csv');
const OUTPUT_REQUESTS_CSV = path.join(OUTPUT_DIR, 'tarotnow-benchmark-requests.csv');
const OUTPUT_MD = path.join(OUTPUT_DIR, 'tarotnow-benchmark-report.md');
const OUTPUT_ANALYSIS_MD = path.join(OUTPUT_DIR, 'tarotnow-benchmark-analysis.md');
const OUTPUT_ROUTE_MAP = path.join(OUTPUT_DIR, 'tarotnow-route-map.json');
const OUTPUT_TARGETED_JSON = path.join(OUTPUT_DIR, 'tarotnow-benchmark-hotspots.json');
const OUTPUT_TARGETED_PAGES_CSV = path.join(OUTPUT_DIR, 'tarotnow-benchmark-hotspots-pages.csv');
const OUTPUT_TARGETED_REQUESTS_CSV = path.join(OUTPUT_DIR, 'tarotnow-benchmark-hotspots-requests.csv');
const OUTPUT_TARGETED_MD = path.join(OUTPUT_DIR, 'tarotnow-benchmark-hotspots-report.md');
const OUTPUT_TARGETED_ANALYSIS_MD = path.join(OUTPUT_DIR, 'tarotnow-benchmark-hotspots-analysis.md');
const OUTPUT_TARGETED_ROUTE_MAP = path.join(OUTPUT_DIR, 'tarotnow-route-map-hotspots.json');
const OUTPUT_FEATURE_JSON = path.join(OUTPUT_DIR, `tarotnow-benchmark-${BENCHMARK_FEATURE}.json`);
const OUTPUT_FEATURE_PAGES_CSV = path.join(OUTPUT_DIR, `tarotnow-benchmark-${BENCHMARK_FEATURE}-pages.csv`);
const OUTPUT_FEATURE_REQUESTS_CSV = path.join(OUTPUT_DIR, `tarotnow-benchmark-${BENCHMARK_FEATURE}-requests.csv`);
const OUTPUT_FEATURE_MD = path.join(OUTPUT_DIR, `tarotnow-benchmark-${BENCHMARK_FEATURE}-report.md`);
const OUTPUT_FEATURE_ANALYSIS_MD = path.join(OUTPUT_DIR, `tarotnow-benchmark-${BENCHMARK_FEATURE}-analysis.md`);
const OUTPUT_FEATURE_ROUTE_MAP = path.join(OUTPUT_DIR, `tarotnow-route-map-${BENCHMARK_FEATURE}.json`);

const CONTROLLED_SPREAD_TYPES = ['daily_1', 'spread_3', 'spread_5', 'spread_10'] as const;
const MAX_DYNAMIC_READING_SESSIONS = 4;
const ORIGIN_ROUTE_DISCOVERY_ENDPOINTS = ['/sitemap.xml', '/robots.txt'] as const;
const PROTECTED_ROUTE_PREFIXES = [
  '/profile',
  '/wallet',
  '/chat',
  '/collection',
  '/reading',
  '/reader',
  '/readers',
  '/gamification',
  '/leaderboard',
  '/gacha',
  '/inventory',
  '/community',
  '/notifications',
  '/admin',
] as const;

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

const HOTSPOT_ROUTE_SEEDS = [
  `${LOCALE_PREFIX}/admin`,
  `${LOCALE_PREFIX}/wallet/deposit/history`,
  `${LOCALE_PREFIX}/community`,
  `${LOCALE_PREFIX}/collection`,
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

interface BenchmarkOutputPaths {
  json: string;
  pagesCsv: string;
  requestsCsv: string;
  reportMd: string;
  analysisMd: string;
  routeMap: string;
  latestJson: string;
  legacyJson: string;
}

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

function stripLocalePath(pathname: string): string {
  if (pathname === LOCALE_PREFIX) {
    return '/';
  }

  const withoutPrefix = pathname.startsWith(`${LOCALE_PREFIX}/`)
    ? pathname.slice(LOCALE_PREFIX.length)
    : pathname;
  return withoutPrefix.startsWith('/') ? withoutPrefix : `/${withoutPrefix}`;
}

function isProtectedRoutePath(route: string): boolean {
  const pathWithoutLocale = stripLocalePath(route);
  return PROTECTED_ROUTE_PREFIXES.some((prefix) =>
    pathWithoutLocale === prefix || pathWithoutLocale.startsWith(`${prefix}/`),
  );
}

function isAdminRoutePath(route: string): boolean {
  const pathWithoutLocale = stripLocalePath(route);
  return pathWithoutLocale === '/admin' || pathWithoutLocale.startsWith('/admin/');
}

function isAuthEntryRoutePath(route: string): boolean {
  const pathWithoutLocale = stripLocalePath(route);
  return ['/login', '/register', '/forgot-password', '/reset-password', '/verify-email'].includes(pathWithoutLocale);
}

function filterRoutesForScenario(
  scenario: BenchmarkScenario,
  routes: string[],
): { routes: string[]; notes: string[] } {
  const notes: string[] = [];
  const filteredRoutes: string[] = [];
  let removedCount = 0;

  for (const route of routes) {
    if (scenario === 'logged-out' && isProtectedRoutePath(route)) {
      removedCount += 1;
      continue;
    }

    if (scenario === 'logged-in-reader' && isAdminRoutePath(route)) {
      removedCount += 1;
      continue;
    }

    if (scenario !== 'logged-out' && isAuthEntryRoutePath(route)) {
      removedCount += 1;
      continue;
    }

    filteredRoutes.push(route);
  }

  if (scenario === 'logged-out') {
    notes.push(`scenario-filter:logged-out-protected-routes-skipped=${removedCount}`);
  } else if (scenario === 'logged-in-reader') {
    notes.push(`scenario-filter:reader-auth-entry-admin-routes-skipped=${removedCount}`);
  } else {
    notes.push(`scenario-filter:admin-auth-entry-routes-skipped=${removedCount}`);
  }

  return {
    routes: [...new Set(filteredRoutes)],
    notes,
  };
}

function extractRouteCandidatesFromRawText(raw: string): string[] {
  const routeCandidates = new Set<string>();
  const routePattern = /\/vi(?:\/[a-z0-9\-._~%!$&'()*+,;=:@/]*)?/gi;
  for (const match of raw.matchAll(routePattern)) {
    const route = match[0];
    if (!route) continue;
    const normalized = normalizeRoutePath(route);
    if (normalized) {
      routeCandidates.add(normalized);
    }
  }
  return [...routeCandidates];
}

async function collectOriginDiscoveredRoutes(): Promise<RouteCollectionResult> {
  const notes: string[] = [];
  const routes = new Set<string>();

  for (const endpoint of ORIGIN_ROUTE_DISCOVERY_ENDPOINTS) {
    const endpointUrl = `${BASE_ORIGIN}${endpoint}`;
    try {
      const response = await fetch(endpointUrl, {
        method: 'GET',
        redirect: 'follow',
      });
      if (!response.ok) {
        notes.push(`origin-discovery:${endpoint}:status-${response.status}`);
        continue;
      }

      const body = await response.text();
      const discovered = extractRouteCandidatesFromRawText(body);
      if (discovered.length === 0) {
        notes.push(`origin-discovery:${endpoint}:no-route-candidates`);
        continue;
      }

      for (const route of discovered) {
        routes.add(route);
      }
      notes.push(`origin-discovery:${endpoint}:routes-${discovered.length}`);
    } catch {
      notes.push(`origin-discovery:${endpoint}:request-failed`);
    }
  }

  if (routes.size === 0) {
    notes.push('origin-discovery:fallback-to-static-and-dom-crawl');
  }

  return {
    routes: [...routes].sort((left, right) => left.localeCompare(right)),
    notes,
  };
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

function tryParseUrl(requestUrl: string): URL | null {
  try {
    return new URL(requestUrl);
  } catch {
    return null;
  }
}

function tryParsePathnameFromUrl(requestUrl: string): string | null {
  return tryParseUrl(requestUrl)?.pathname ?? null;
}

function classifyRequestFailure(request: Pick<RequestMetric, 'url' | 'status' | 'failed' | 'failureText' | 'category'>): RequestFailureClass {
  if (request.category === 'telemetry') {
    return request.failed || request.status === null || (request.status >= 400) ? 'external-telemetry' : 'none';
  }

  const parsed = tryParseUrl(request.url);
  const isFirstParty = parsed?.origin === BASE_ORIGIN;
  if (request.failed && request.failureText?.includes('net::ERR_ABORTED')) {
    return isFirstParty ? 'expected-navigation-abort' : 'external-telemetry';
  }

  if (request.status === 429) {
    return 'server-rate-limit';
  }

  if (typeof request.status === 'number' && request.status >= 400) {
    return isFirstParty ? 'app-regression' : 'external-telemetry';
  }

  return request.failed ? 'app-regression' : 'none';
}

function isActionableFailedRequest(request: RequestMetric): boolean {
  return request.failureClass === 'app-regression' || request.failureClass === 'server-rate-limit';
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

function resolveRouteFamily(route: string): string {
  const pathWithoutLocale = stripLocalePath(route);
  if (pathWithoutLocale === '/') return 'home';
  if (pathWithoutLocale === '/login'
    || pathWithoutLocale === '/register'
    || pathWithoutLocale === '/forgot-password'
    || pathWithoutLocale === '/reset-password'
    || pathWithoutLocale === '/verify-email') return 'auth';
  if (pathWithoutLocale === '/reading' || pathWithoutLocale.startsWith('/reading/')) return 'reading';
  if (pathWithoutLocale === '/inventory' || pathWithoutLocale.startsWith('/inventory/')) return 'inventory';
  if (pathWithoutLocale === '/gacha' || pathWithoutLocale.startsWith('/gacha/')) return 'gacha';
  if (pathWithoutLocale === '/collection' || pathWithoutLocale.startsWith('/collection/')) return 'collection';
  if (pathWithoutLocale === '/profile' || pathWithoutLocale.startsWith('/profile/')) return 'profile';
  if (pathWithoutLocale === '/reader' || pathWithoutLocale.startsWith('/reader/')) return 'reader';
  if (pathWithoutLocale === '/readers' || pathWithoutLocale.startsWith('/readers/')) return 'readers';
  if (pathWithoutLocale === '/chat' || pathWithoutLocale.startsWith('/chat/')) return 'chat';
  if (pathWithoutLocale === '/leaderboard' || pathWithoutLocale.startsWith('/leaderboard/')) return 'leaderboard';
  if (pathWithoutLocale === '/community' || pathWithoutLocale.startsWith('/community/')) return 'community';
  if (pathWithoutLocale === '/gamification' || pathWithoutLocale.startsWith('/gamification/')) return 'gamification';
  if (pathWithoutLocale === '/wallet' || pathWithoutLocale.startsWith('/wallet/')) return 'wallet';
  if (pathWithoutLocale === '/notifications' || pathWithoutLocale.startsWith('/notifications/')) return 'notifications';
  if (pathWithoutLocale === '/legal' || pathWithoutLocale.startsWith('/legal/')) return 'legal';
  if (pathWithoutLocale === '/admin' || pathWithoutLocale.startsWith('/admin/')) return 'admin';
  return 'other';
}

function resolveBenchmarkFeature(route: string): BenchmarkFeature {
  const pathWithoutLocale = stripLocalePath(route);
  if (
    pathWithoutLocale === '/'
    || pathWithoutLocale === '/login'
    || pathWithoutLocale === '/register'
    || pathWithoutLocale === '/forgot-password'
    || pathWithoutLocale === '/reset-password'
    || pathWithoutLocale === '/verify-email'
    || pathWithoutLocale.startsWith('/legal')
  ) return 'auth-public';

  if (pathWithoutLocale === '/reading' || pathWithoutLocale.startsWith('/reading/')) return 'reading';

  if (
    pathWithoutLocale === '/inventory'
    || pathWithoutLocale.startsWith('/inventory/')
    || pathWithoutLocale === '/gacha'
    || pathWithoutLocale.startsWith('/gacha/')
    || pathWithoutLocale === '/collection'
    || pathWithoutLocale.startsWith('/collection/')
  ) return 'inventory-gacha-collection';

  if (
    pathWithoutLocale === '/profile'
    || pathWithoutLocale.startsWith('/profile/')
    || pathWithoutLocale === '/wallet'
    || pathWithoutLocale.startsWith('/wallet/')
    || pathWithoutLocale === '/notifications'
    || pathWithoutLocale.startsWith('/notifications/')
  ) return 'profile-wallet-notifications';

  if (
    pathWithoutLocale === '/reader'
    || pathWithoutLocale.startsWith('/reader/')
    || pathWithoutLocale === '/readers'
    || pathWithoutLocale.startsWith('/readers/')
    || pathWithoutLocale === '/chat'
    || pathWithoutLocale.startsWith('/chat/')
  ) return 'reader-chat';

  if (
    pathWithoutLocale === '/community'
    || pathWithoutLocale.startsWith('/community/')
    || pathWithoutLocale === '/leaderboard'
    || pathWithoutLocale.startsWith('/leaderboard/')
    || pathWithoutLocale === '/gamification'
    || pathWithoutLocale.startsWith('/gamification/')
  ) return 'community-leaderboard-quest';

  if (pathWithoutLocale === '/admin' || pathWithoutLocale.startsWith('/admin/')) return 'admin';

  return 'other';
}

function isCollectionImageRequestUrl(requestUrl: string): boolean {
  try {
    const parsed = new URL(requestUrl);
    const host = parsed.hostname.toLowerCase();
    if (host === 'img.tarotnow.xyz' || host === 'media.tarotnow.xyz') {
      return true;
    }

    if (parsed.pathname === '/api/collection/card-image') {
      return true;
    }

    if (parsed.pathname !== '/_next/image') {
      return false;
    }

    const encodedUpstream = parsed.searchParams.get('url');
    if (!encodedUpstream) {
      return false;
    }

    const upstream = decodeURIComponent(encodedUpstream);
    return upstream.includes('/api/collection/card-image')
      || upstream.includes('img.tarotnow.xyz')
      || upstream.includes('media.tarotnow.xyz');
  } catch {
    return requestUrl.includes('/api/collection/card-image')
      || requestUrl.includes('img.tarotnow.xyz')
      || requestUrl.includes('media.tarotnow.xyz');
  }
}

function buildRouteFamilySummaryLines(result: BenchmarkRunResult): string[] {
  const aggregates = new Map<string, {
    scenario: BenchmarkScenario;
    viewport: BenchmarkViewportId;
    family: string;
    pages: number;
    requests: number;
    navigationMs: number;
    pending: number;
  }>();

  for (const scenario of result.scenarios) {
    for (const page of scenario.pages) {
      const family = resolveRouteFamily(page.route);
      const key = `${scenario.scenario}|${scenario.viewport}|${family}`;
      const current = aggregates.get(key);
      if (!current) {
        aggregates.set(key, {
          scenario: scenario.scenario,
          viewport: scenario.viewport,
          family,
          pages: 1,
          requests: page.requestCount,
          navigationMs: page.navigationMs,
          pending: page.pendingCount,
        });
        continue;
      }

      current.pages += 1;
      current.requests += page.requestCount;
      current.navigationMs += page.navigationMs;
      current.pending += page.pendingCount;
    }
  }

  return [...aggregates.values()]
    .sort((left, right) => {
      if (left.scenario !== right.scenario) return left.scenario.localeCompare(right.scenario);
      if (left.viewport !== right.viewport) return left.viewport.localeCompare(right.viewport);
      return left.family.localeCompare(right.family);
    })
    .map((entry) => {
      const avgRequests = entry.requests / entry.pages;
      const avgNavigation = entry.navigationMs / entry.pages;
      return `| ${entry.scenario} | ${entry.viewport} | ${entry.family} | ${entry.pages} | ${avgRequests.toFixed(1)} | ${avgNavigation.toFixed(0)} | ${entry.pending} |`;
    });
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
  const metric = {
    scenario,
    viewport,
    route: record.route,
    feature: record.feature,
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
    failureClass: 'none' as RequestFailureClass,
    waterfallDepth: record.waterfallDepth,
    pendingAgeMs: record.pendingAgeMs,
    isDuplicate: record.isDuplicate,
  };

  metric.failureClass = classifyRequestFailure(metric);
  return metric;
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

function withRunDir(filePath: string): string {
  return path.join(OUTPUT_RUN_DIR, path.basename(filePath));
}

function withLatestDir(filePath: string): string {
  return path.join(OUTPUT_LATEST_DIR, path.basename(filePath));
}

function buildOutputPaths(paths: Omit<BenchmarkOutputPaths, 'latestJson' | 'legacyJson'>): BenchmarkOutputPaths {
  return {
    ...paths,
    json: withRunDir(paths.json),
    pagesCsv: withRunDir(paths.pagesCsv),
    requestsCsv: withRunDir(paths.requestsCsv),
    reportMd: withRunDir(paths.reportMd),
    analysisMd: withRunDir(paths.analysisMd),
    routeMap: withRunDir(paths.routeMap),
    latestJson: withLatestDir(paths.json),
    legacyJson: paths.json,
  };
}

function resolveOutputPaths(mode: BenchmarkMode): BenchmarkOutputPaths {
  if (mode === 'targeted-hotspots') {
    return buildOutputPaths({
      json: OUTPUT_TARGETED_JSON,
      pagesCsv: OUTPUT_TARGETED_PAGES_CSV,
      requestsCsv: OUTPUT_TARGETED_REQUESTS_CSV,
      reportMd: OUTPUT_TARGETED_MD,
      analysisMd: OUTPUT_TARGETED_ANALYSIS_MD,
      routeMap: OUTPUT_TARGETED_ROUTE_MAP,
    });
  }

  if (mode === 'feature-matrix') {
    return buildOutputPaths({
      json: OUTPUT_FEATURE_JSON,
      pagesCsv: OUTPUT_FEATURE_PAGES_CSV,
      requestsCsv: OUTPUT_FEATURE_REQUESTS_CSV,
      reportMd: OUTPUT_FEATURE_MD,
      analysisMd: OUTPUT_FEATURE_ANALYSIS_MD,
      routeMap: OUTPUT_FEATURE_ROUTE_MAP,
    });
  }

  return buildOutputPaths({
    json: OUTPUT_JSON,
    pagesCsv: OUTPUT_PAGES_CSV,
    requestsCsv: OUTPUT_REQUESTS_CSV,
    reportMd: OUTPUT_MD,
    analysisMd: OUTPUT_ANALYSIS_MD,
    routeMap: OUTPUT_ROUTE_MAP,
  });
}

const BENCHMARK_FEATURES: BenchmarkFeature[] = [
  'auth-public',
  'reading',
  'inventory-gacha-collection',
  'profile-wallet-notifications',
  'reader-chat',
  'community-leaderboard-quest',
  'admin',
  'other',
];

function assertBenchmarkFeature(value: BenchmarkFeature): void {
  if (!BENCHMARK_FEATURES.includes(value)) {
    throw new Error(`Unsupported BENCHMARK_FEATURE: ${value}`);
  }
}

function filterRoutesForBenchmarkFeature(routes: string[], feature: BenchmarkFeature): string[] {
  return routes.filter((route) => resolveBenchmarkFeature(route) === feature);
}

function resolveSeedRoutesForMode(
  mode: BenchmarkMode,
  fullSeedRoutes: string[],
): string[] {
  if (mode === 'targeted-hotspots') {
    return HOTSPOT_ROUTE_SEEDS
      .map((route) => normalizeRoutePath(route))
      .filter((route): route is string => Boolean(route));
  }

  if (mode === 'feature-matrix') {
    assertBenchmarkFeature(BENCHMARK_FEATURE);
    return filterRoutesForBenchmarkFeature(fullSeedRoutes, BENCHMARK_FEATURE);
  }

  return fullSeedRoutes;
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

function pickIdsFromRoutes(routes: string[], matcher: RegExp, limit: number): string[] {
  const ids = new Set<string>();
  for (const route of routes) {
    const match = matcher.exec(route);
    matcher.lastIndex = 0;
    if (!match?.[1]) continue;
    ids.add(match[1]);
    if (ids.size >= limit) {
      break;
    }
  }
  return [...ids];
}

async function collectRouteIdsFromUi(
  page: Page,
  routePath: string,
  matcher: RegExp,
  limit: number,
  notePrefix: string,
): Promise<{ ids: string[]; notes: string[] }> {
  const notes: string[] = [];
  try {
    await page.goto(`${BASE_ORIGIN}${routePath}`, {
      waitUntil: 'domcontentloaded',
      timeout: NAVIGATION_TIMEOUT_MS,
    });
    await page.waitForTimeout(800);
    const routes = await discoverRoutesFromPage(page);
    const ids = pickIdsFromRoutes(routes, matcher, limit);
    if (ids.length === 0) {
      notes.push(`${notePrefix}:ui-discovery-empty`);
    } else {
      notes.push(`${notePrefix}:ui-discovery-${ids.length}`);
    }
    return { ids, notes };
  } catch {
    notes.push(`${notePrefix}:ui-discovery-failed`);
    return { ids: [], notes };
  }
}

async function collectCommunityPostIds(page: Page): Promise<string[]> {
  try {
    const response = await page.request.get(`${BASE_ORIGIN}/api/v1/community/posts?page=1&pageSize=12&visibility=public`, {
      timeout: 12_000,
      failOnStatusCode: false,
    });
    if (!response.ok()) return [];
    const payload = await response.json().catch(() => null) as {
      data?: Array<{ id?: string }>;
    } | null;
    return (payload?.data ?? [])
      .map((item) => item.id ?? '')
      .filter((id) => id.length > 0)
      .slice(0, 6);
  } catch {
    return [];
  }
}

async function probeCommunityPostDetails(page: Page, postIds: string[]): Promise<{ ids: string[]; notes: string[] }> {
  if (postIds.length === 0) {
    return { ids: [], notes: ['community-post-detail:probe-skipped-no-id'] };
  }

  const ids: string[] = [];
  const notes: string[] = [];
  for (const postId of postIds.slice(0, 6)) {
    try {
      const response = await page.request.get(
        `${BASE_ORIGIN}${LOCALE_PREFIX}/community/${encodeURIComponent(postId)}`,
        {
          timeout: 12_000,
          failOnStatusCode: false,
        },
      );
      if (response.ok()) {
        ids.push(postId);
        notes.push(`community-post-detail:${postId}:page-${response.status()}`);
      } else {
        notes.push(`community-post-detail:${postId}:stale-page-${response.status()}`);
      }
    } catch {
      notes.push(`community-post-detail:${postId}:request-failed`);
    }
  }

  return { ids, notes };
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
      communityPostIds: [],
      coverageNotes: ['dynamic-routes: skipped for logged-out scenario.'],
    };
  }

  const readingSessionIds: string[] = [];
  const readingHistoryIds: string[] = [];
  const readerIds: string[] = [];
  const chatIds: string[] = [];
  const communityPostIds: string[] = [];

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

  const uiReaders = await collectRouteIdsFromUi(page, `${LOCALE_PREFIX}/readers`, /^\/vi\/readers\/([^/]+)$/i, 6, 'reader-detail');
  readerIds.push(...uiReaders.ids);
  coverageNotes.push(...uiReaders.notes);

  const uiChats = await collectRouteIdsFromUi(page, `${LOCALE_PREFIX}/chat`, /^\/vi\/chat\/([^/]+)$/i, 6, 'chat-room-detail');
  chatIds.push(...uiChats.ids);
  coverageNotes.push(...uiChats.notes);

  const uiHistory = await collectRouteIdsFromUi(
    page,
    `${LOCALE_PREFIX}/reading/history`,
    /^\/vi\/reading\/history\/([^/]+)$/i,
    6,
    'reading-history-detail',
  );
  readingHistoryIds.push(...uiHistory.ids);
  coverageNotes.push(...uiHistory.notes);

  const apiCommunityPostIds = await collectCommunityPostIds(page);
  if (apiCommunityPostIds.length === 0) {
    coverageNotes.push('community-posts:api-discovery-empty');
  } else {
    coverageNotes.push(`community-posts:api-discovery-${apiCommunityPostIds.length}`);
  }
  const validCommunityPosts = await probeCommunityPostDetails(page, apiCommunityPostIds);
  communityPostIds.push(...validCommunityPosts.ids);
  coverageNotes.push(...validCommunityPosts.notes);

  return {
    readingSessionIds: [...new Set(readingSessionIds)].slice(0, 6),
    readingHistoryIds: [...new Set(readingHistoryIds)].slice(0, 6),
    readerIds: [...new Set(readerIds)].slice(0, 6),
    chatIds: [...new Set(chatIds)].slice(0, 6),
    communityPostIds: [...new Set(communityPostIds)].slice(0, 6),
    coverageNotes,
  };
}

function buildSeedRoutes(
  baseRoutes: string[],
  dynamic: DynamicRouteDiscovery,
  scenario: BenchmarkScenario,
): { routes: string[]; notes: string[] } {
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

  for (const id of dynamic.communityPostIds) {
    routes.add(`${LOCALE_PREFIX}/community/${id}`);
  }

  const scenarioFiltered = filterRoutesForScenario(scenario, [...routes]);
  return {
    routes: scenarioFiltered.routes,
    notes: scenarioFiltered.notes,
  };
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

async function runRouteInteractionProbe(page: Page, route: string): Promise<RouteInteractionProbeResult> {
  const notes: string[] = [];
  let collectionReloadStartedAtMs: number | null = null;

  if (route === `${LOCALE_PREFIX}/community`) {
    try {
      const postCards = page.locator('.tn-community-card');
      const postCount = await postCards.count();
      if (postCount === 0) {
        notes.push('interaction-community:no-post-card');
      } else {
        const firstPost = postCards.nth(0);
        const commentButton = firstPost.locator('button:has(svg.lucide-message-circle)');
        if (await commentButton.count() > 0) {
          await commentButton.nth(0).click({ timeout: 6_000 });
          notes.push('interaction-community:toggle-comments');
        } else {
          notes.push('interaction-community:comment-button-missing');
        }

        const reactionButton = firstPost.locator('button:has-text("👍")');
        if (await reactionButton.count() > 0) {
          await reactionButton.nth(0).click({ timeout: 6_000 });
          notes.push('interaction-community:react-like');
        } else {
          notes.push('interaction-community:reaction-button-missing');
        }
      }
      await page.waitForTimeout(700);
    } catch {
      notes.push('interaction-community:failed');
    }
  }

  if (route === `${LOCALE_PREFIX}/collection`) {
    try {
      for (let index = 0; index < 2; index += 1) {
        await page.mouse.wheel(0, 900);
        await page.waitForTimeout(450);
      }
      notes.push('interaction-collection:scroll-2-viewports');
    } catch {
      notes.push('interaction-collection:scroll-failed');
    }

    try {
      const cards = page.locator('[data-tn-collection-card="true"]');
      const count = await cards.count();
      const openCount = Math.min(5, count);
      for (let index = 0; index < openCount; index += 1) {
        await cards.nth(index).click({ timeout: 8_000 });
        const closeButton = page.locator('[data-tn-collection-modal-close="true"]');
        if (await closeButton.count() > 0) {
          await closeButton.first().click({ timeout: 8_000 });
        } else {
          await page.keyboard.press('Escape');
        }
        await page.waitForTimeout(220);
      }
      notes.push(`interaction-collection:modal-opened-${openCount}`);
    } catch {
      notes.push('interaction-collection:modal-open-failed');
    }

    try {
      const cards = page.locator('[data-tn-collection-card="true"]');
      if (await cards.count() > 0) {
        collectionReloadStartedAtMs = Date.now();
        await cards.nth(0).click({ timeout: 8_000 });
        const closeButton = page.locator('[data-tn-collection-modal-close="true"]');
        if (await closeButton.count() > 0) {
          await closeButton.first().click({ timeout: 8_000 });
        } else {
          await page.keyboard.press('Escape');
        }
        await page.waitForTimeout(220);
        notes.push('interaction-collection:reopen-first-card');
      }
    } catch {
      notes.push('interaction-collection:reopen-failed');
    }
  }

  return {
    notes,
    collectionReloadStartedAtMs,
  };
}

async function benchmarkNavigation(
  page: Page,
  scenario: BenchmarkScenario,
  viewport: BenchmarkViewportId,
  route: string,
  navFrom: string | null,
): Promise<PageBenchmarkMetric> {
  const routeUrl = `${BASE_ORIGIN}${route}`;
  const feature = resolveBenchmarkFeature(route);
  const requests = new Map<Request, MutableRequestMetric>();
  const responseMap = new Map<Request, Response>();
  const finalizeTasks: Array<Promise<void>> = [];
  const coverageBlocked: string[] = [];
  const runtimeConsoleErrors = new Set<string>();
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
        feature,
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
        feature,
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

  const onConsoleMessage = (message: { type: () => string; text: () => string }): void => {
    if (message.type() !== 'error') {
      return;
    }

    const text = message.text();
    const normalized = text.trim();
    if (!normalized) {
      return;
    }

    if (normalized.includes('#418') || normalized.toLowerCase().includes('hydration')) {
      runtimeConsoleErrors.add(`console-error:${normalized}`);
    }
  };

  await page.route('**/*', onRoute);
  page.on('request', onRequest);
  page.on('response', onResponse);
  page.on('requestfinished', onRequestFinished);
  page.on('requestfailed', onRequestFailed);
  page.on('console', onConsoleMessage);

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

  const interactionStartedAtMs = Date.now();
  const interactionProbe = await runRouteInteractionProbe(page, route);
  coverageBlocked.push(...interactionProbe.notes);

  await page.waitForTimeout(SETTLE_AFTER_NAVIGATION_MS);
  await Promise.allSettled(finalizeTasks);

  page.off('request', onRequest);
  page.off('response', onResponse);
  page.off('requestfinished', onRequestFinished);
  page.off('requestfailed', onRequestFailed);
  page.off('console', onConsoleMessage);
  await page.unroute('**/*', onRoute);

  const navigationMs = Date.now() - startedAtMs;
  const finalUrl = page.url();
  const paintMetrics = await collectNavigationPaintMetrics(page);
  const settledAtMs = Date.now();

  const requestRecords = [...requests.values()];
  const baseRequestRecords = requestRecords.filter((request) => request.startedAtMs < interactionStartedAtMs);
  const interactionRequestCount = Math.max(0, requestRecords.length - baseRequestRecords.length);

  const duplicateCounter = new Map<string, number>();
  for (const request of baseRequestRecords) {
    const key = normalizeRequestKey(request.method, request.url);
    duplicateCounter.set(key, (duplicateCounter.get(key) ?? 0) + 1);
  }

  const duplicateSet = new Set<string>(
    [...duplicateCounter.entries()].filter(([, count]) => count > 1).map(([key]) => key),
  );

  for (const request of requestRecords) {
    const key = normalizeRequestKey(request.method, request.url);
    request.isDuplicate = request.startedAtMs < interactionStartedAtMs && duplicateSet.has(key);
    if (request.finishedAtMs === null && !request.failed) {
      request.pendingAgeMs = Math.max(0, settledAtMs - request.startedAtMs);
    }
  }

  const requestMetrics = requestRecords.map((record) => toRequestMetric(record, scenario, viewport));
  const baseRequestMetrics = requestMetrics.filter((request, index) =>
    requestRecords[index]?.startedAtMs < interactionStartedAtMs,
  );

  const pendingNonPersistent = requestMetrics.filter((request) =>
    request.durationMs === null
    && !request.failed
    && request.resourceType !== 'document'
    && request.resourceType !== 'websocket'
    && request.resourceType !== 'eventsource',
  );

  const normalizedRoute = normalizeRoutePath(route) ?? route;
  const matchingDocumentRequests = baseRequestMetrics.filter((request) =>
    request.resourceType === 'document'
    && normalizeRoutePath(request.url) === normalizedRoute,
  );
  const documentReloadCount = Math.max(0, matchingDocumentRequests.length - 1);

  const handshakeRedirectCount = baseRequestMetrics.filter((request) => {
    const pathname = tryParsePathnameFromUrl(request.url);
    if (pathname !== '/api/auth/session/handshake') {
      return false;
    }

    return typeof request.status === 'number' && request.status >= 300 && request.status < 400;
  }).length;

  const sessionApiCallCount = baseRequestMetrics.filter((request) => {
    const pathname = tryParsePathnameFromUrl(request.url);
    return pathname === '/api/auth/session';
  }).length;

  const failedRequestCount = baseRequestMetrics.filter(isActionableFailedRequest).length;

  const slowRequests = baseRequestMetrics.filter((request) => (request.durationMs ?? 0) > SLOW_REQUEST_MEDIUM_THRESHOLD_MS);
  const collectionImageRequests = requestRecords.filter((request) => isCollectionImageRequestUrl(request.url));
  const collectionReloadStartedAtMs = interactionProbe.collectionReloadStartedAtMs;
  const collectionImageRequestCount = collectionImageRequests.length;
  const collectionImageSlowRequestHighCount = collectionImageRequests.filter((request) =>
    (request.durationMs ?? 0) > SLOW_REQUEST_HIGH_THRESHOLD_MS,
  ).length;
  const collectionImageSlowRequestMediumCount = collectionImageRequests.filter((request) => {
    const duration = request.durationMs ?? 0;
    return duration > SLOW_REQUEST_MEDIUM_THRESHOLD_MS && duration <= SLOW_REQUEST_HIGH_THRESHOLD_MS;
  }).length;
  const collectionImageCacheHitCount = collectionImageRequests.filter((request) =>
    request.status === 304,
  ).length;
  const collectionImageFirstLoadRequestCount = collectionImageRequests.filter((request) =>
    collectionReloadStartedAtMs === null || request.startedAtMs < collectionReloadStartedAtMs,
  ).length;
  const collectionImageReopenRequestCount = collectionImageRequests.filter((request) =>
    collectionReloadStartedAtMs !== null && request.startedAtMs >= collectionReloadStartedAtMs,
  ).length;
  const totalResponseBytes = baseRequestMetrics.reduce((sum, request) => sum + request.responseBytes, 0);
  const totalTransferBytes = baseRequestMetrics.reduce((sum, request) => sum + request.transferBytes, 0);

  const duplicateRequestGroups = [...duplicateCounter.entries()]
    .filter(([, count]) => count > 1)
    .sort((left, right) => right[1] - left[1])
    .map(([key, count]) => ({ key, count }));

  const thirdPartyCounter = new Map<string, number>();
  for (const request of baseRequestMetrics) {
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
    feature,
    navFrom,
    finalUrl,
    navigationMs,
    domContentLoadedMs: paintMetrics.domContentLoadedMs,
    loadMs: paintMetrics.loadMs,
    fcpMs: paintMetrics.fcpMs,
    lcpMs: paintMetrics.lcpMs,
    cls: paintMetrics.cls,
    tbt: paintMetrics.tbt,
    requestCount: baseRequestMetrics.length,
    interactionRequestCount,
    requestSeverity: deriveRequestSeverity(baseRequestMetrics.length),
    documentReloadCount,
    handshakeRedirectCount,
    sessionApiCallCount,
    failedRequestCount,
    collectionImageRequestCount,
    collectionImageSlowRequestMediumCount,
    collectionImageSlowRequestHighCount,
    collectionImageFirstLoadRequestCount,
    collectionImageReopenRequestCount,
    collectionImageCacheHitCount,
    pendingCount: pendingNonPersistent.length,
    pendingUrls: pendingNonPersistent.map((request) => request.url),
    totalResponseBytes,
    totalTransferBytes,
    requestBreakdown: buildBreakdown(baseRequestMetrics),
    thirdPartyBreakdown,
    duplicateRequestGroups,
    slowRequests,
    requests: requestMetrics,
    interactionNotes: [...interactionProbe.notes, ...runtimeConsoleErrors],
    coverageBlocked,
  };
}

async function runScenario(
  browser: Browser,
  scenario: BenchmarkScenario,
  viewportProfile: ViewportProfile,
  seedRoutes: string[],
  routeDiscoveryNotes: string[],
  options?: {
    enableRecursiveDomDiscovery?: boolean;
  },
): Promise<ScenarioBenchmarkResult> {
  const context = await browser.newContext(viewportProfile.contextOptions);
  const bootstrapPage = await context.newPage();
  await installPaintObservers(bootstrapPage);

  let loginBootstrapSucceeded = true;
  let loginBootstrapNotes: string[] = [];
  const coverageNotes: string[] = [...routeDiscoveryNotes];

  const credential = resolveScenarioCredential(scenario);
  if (credential) {
    const loginResult = await loginAsUser(bootstrapPage, credential, resolveExpectedRole(scenario));
    loginBootstrapSucceeded = loginResult.success;
    loginBootstrapNotes = loginResult.notes;
  }

  const dynamicRoutes = await discoverDynamicRoutesForScenario(bootstrapPage, scenario);
  coverageNotes.push(...dynamicRoutes.coverageNotes);

  const seededRoutes = buildSeedRoutes(seedRoutes, dynamicRoutes, scenario);
  coverageNotes.push(...seededRoutes.notes);
  const queue: string[] = seededRoutes.routes;
  const enableRecursiveDomDiscovery = options?.enableRecursiveDomDiscovery ?? true;
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

    const routePage = await context.newPage();
    await installPaintObservers(routePage);

    const metric = await benchmarkNavigation(routePage, scenario, viewportProfile.id, normalizedRoute, navFrom);
    pageMetrics.push(metric);
    visited.add(normalizedRoute);
    navFrom = normalizedRoute;

    if (enableRecursiveDomDiscovery) {
      const discoveredRoutes = await discoverRoutesFromPage(routePage);
      for (const discovered of discoveredRoutes.slice(0, DISCOVERY_LIMIT_PER_PAGE)) {
        if (visited.has(discovered)) continue;
        if (queue.includes(discovered)) continue;
        if (scenario === 'logged-out' && isProtectedRoutePath(discovered)) continue;
        if (scenario === 'logged-in-reader' && isAdminRoutePath(discovered)) continue;
        if (scenario !== 'logged-out' && isAuthEntryRoutePath(discovered)) continue;
        queue.push(discovered);
      }
    }

    await routePage.close();
  }

  await bootstrapPage.close();
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
    'feature',
    'final_url',
    'request_count',
    'interaction_request_count',
    'request_severity',
    'document_reload_count',
    'handshake_redirect_count',
    'session_api_call_count',
    'failed_request_count',
    'collection_image_request_count',
    'collection_image_slow_400_800_count',
    'collection_image_slow_over_800_count',
    'collection_image_first_load_count',
    'collection_image_reopen_count',
    'collection_image_cache_hit_count',
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
    'interaction_notes',
  ].join(','));

  for (const scenario of result.scenarios) {
    for (const page of scenario.pages) {
      lines.push([
        toCsvValue(page.scenario),
        toCsvValue(page.viewport),
        toCsvValue(page.route),
        toCsvValue(page.feature),
        toCsvValue(page.finalUrl),
        toCsvValue(page.requestCount),
        toCsvValue(page.interactionRequestCount),
        toCsvValue(page.requestSeverity),
        toCsvValue(page.documentReloadCount),
        toCsvValue(page.handshakeRedirectCount),
        toCsvValue(page.sessionApiCallCount),
        toCsvValue(page.failedRequestCount),
        toCsvValue(page.collectionImageRequestCount),
        toCsvValue(page.collectionImageSlowRequestMediumCount),
        toCsvValue(page.collectionImageSlowRequestHighCount),
        toCsvValue(page.collectionImageFirstLoadRequestCount),
        toCsvValue(page.collectionImageReopenRequestCount),
        toCsvValue(page.collectionImageCacheHitCount),
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
        toCsvValue(page.interactionNotes.join('|')),
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
    'feature',
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
    'failure_class',
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
          toCsvValue(request.feature),
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
          toCsvValue(request.failureClass),
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
  const collectionFocusPages = allPages.filter((page) => page.route === `${LOCALE_PREFIX}/collection`);

  const highSlowRequests = allPages.flatMap((page) =>
    page.slowRequests
      .filter((request) => (request.durationMs ?? 0) > SLOW_REQUEST_HIGH_THRESHOLD_MS)
      .map((request) => ({ ...request, scenario: page.scenario, viewport: page.viewport, route: page.route, feature: page.feature })),
  );

  const mediumSlowRequests = allPages.flatMap((page) =>
    page.slowRequests
      .filter((request) => {
        const duration = request.durationMs ?? 0;
        return duration > SLOW_REQUEST_MEDIUM_THRESHOLD_MS && duration <= SLOW_REQUEST_HIGH_THRESHOLD_MS;
      })
      .map((request) => ({ ...request, scenario: page.scenario, viewport: page.viewport, route: page.route, feature: page.feature })),
  );

  const summaryLines = result.scenarios.map((scenario) => {
    const pageCount = scenario.pages.length;
    const avgNavigation = pageCount > 0
      ? scenario.pages.reduce((sum, page) => sum + page.navigationMs, 0) / pageCount
      : 0;
    const totalRequests = scenario.pages.reduce((sum, page) => sum + page.requestCount, 0);
    const pending = scenario.pages.reduce((sum, page) => sum + page.pendingCount, 0);
    const documentReloads = scenario.pages.reduce((sum, page) => sum + page.documentReloadCount, 0);
    const sessionApiCalls = scenario.pages.reduce((sum, page) => sum + page.sessionApiCallCount, 0);
    const failedRequests = scenario.pages.reduce((sum, page) => sum + page.failedRequestCount, 0);
    return `| ${scenario.scenario} | ${scenario.viewport} | ${pageCount} | ${avgNavigation.toFixed(0)} | ${totalRequests} | ${pending} | ${documentReloads} | ${sessionApiCalls} | ${failedRequests} | ${scenario.loginBootstrapSucceeded ? 'yes' : 'no'} |`;
  });
  const routeFamilySummaryLines = buildRouteFamilySummaryLines(result);

  const pageLines = allPages.map((page) =>
    `| ${page.scenario} | ${page.viewport} | ${page.feature} | ${page.route} | ${page.requestCount} | ${page.interactionRequestCount} | ${page.requestSeverity} | ${page.documentReloadCount} | ${page.handshakeRedirectCount} | ${page.sessionApiCallCount} | ${page.failedRequestCount} | ${page.collectionImageRequestCount} | ${page.collectionImageSlowRequestMediumCount} | ${page.collectionImageSlowRequestHighCount} | ${page.collectionImageFirstLoadRequestCount} | ${page.collectionImageReopenRequestCount} | ${page.collectionImageCacheHitCount} | ${formatNumber(page.navigationMs)} | ${formatNumber(page.domContentLoadedMs)} | ${formatNumber(page.loadMs)} | ${formatNumber(page.fcpMs)} | ${formatNumber(page.lcpMs)} | ${formatNumber(page.cls, 4)} | ${formatNumber(page.tbt, 1)} | ${page.totalTransferBytes} |`,
  );

  const collectionFocusLines = collectionFocusPages.length > 0
    ? collectionFocusPages.map((page) =>
      `| ${page.scenario} | ${page.viewport} | ${page.route} | ${page.collectionImageRequestCount} | ${page.collectionImageSlowRequestMediumCount} | ${page.collectionImageSlowRequestHighCount} | ${page.collectionImageFirstLoadRequestCount} | ${page.collectionImageReopenRequestCount} | ${page.collectionImageCacheHitCount} |`,
    )
    : ['| - | - | - | - | - | - | - | - | - |'];

  const slowHighLines = highSlowRequests.length > 0
    ? highSlowRequests
      .sort((left, right) => (right.durationMs ?? 0) - (left.durationMs ?? 0))
      .slice(0, 80)
      .map((request) =>
        `| ${request.scenario} | ${request.viewport} | ${request.feature} | ${request.route} | ${request.method} | ${request.status ?? '-'} | ${formatNumber(request.durationMs)} | ${formatNumber(request.ttfbMs)} | ${request.category} | ${request.url} |`,
      )
    : ['| - | - | - | - | - | - | - | - | - |'];

  const slowMediumLines = mediumSlowRequests.length > 0
    ? mediumSlowRequests
      .sort((left, right) => (right.durationMs ?? 0) - (left.durationMs ?? 0))
      .slice(0, 80)
      .map((request) =>
        `| ${request.scenario} | ${request.viewport} | ${request.feature} | ${request.route} | ${request.method} | ${request.status ?? '-'} | ${formatNumber(request.durationMs)} | ${formatNumber(request.ttfbMs)} | ${request.category} | ${request.url} |`,
      )
    : ['| - | - | - | - | - | - | - | - | - |'];

  const suspiciousLines = suspiciousPages.length > 0
    ? suspiciousPages.map((page) =>
      `| ${page.scenario} | ${page.viewport} | ${page.feature} | ${page.route} | ${page.requestCount} | ${page.requestSeverity} | ${page.requestBreakdown.api} | ${page.requestBreakdown.static} | ${page.requestBreakdown.thirdParty} |`,
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
    `- Benchmark mode: ${result.benchmarkMode}`,
    `- Thresholds: >${REQUEST_COUNT_CRITICAL_THRESHOLD} requests = Critical, >${REQUEST_COUNT_HIGH_THRESHOLD} = High, request >${SLOW_REQUEST_HIGH_THRESHOLD_MS}ms = High, >${SLOW_REQUEST_MEDIUM_THRESHOLD_MS}ms = Medium`,
    `- Critical pages (request count): ${criticalPages.length}`,
    `- High pages (request count): ${suspiciousPages.length}`,
    `- High slow requests: ${highSlowRequests.length}`,
    `- Medium slow requests: ${mediumSlowRequests.length}`,
    '',
    '## Scenario Summary',
    '| Scenario | Viewport | Pages Benchmarked | Avg Navigation (ms) | Total Requests | Pending Requests | Document Reloads | Session API Calls | Failed Requests | Login bootstrap |',
    '| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |',
    ...summaryLines,
    '',
    '## Route-Family Summary',
    '| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |',
    '| --- | --- | --- | ---: | ---: | ---: | ---: |',
    ...(routeFamilySummaryLines.length > 0 ? routeFamilySummaryLines : ['| - | - | - | - | - | - | - |']),
    '',
    '## Per-Page Metrics',
    '| Scenario | Viewport | Feature | Route | Requests | Interaction Requests | Severity | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Collection Img Requests | Collection Img 400-800ms | Collection Img >800ms | Collection Img First Load | Collection Img Reopen | Collection Img Cache Hits | Navigate (ms) | DOMContentLoaded (ms) | Load (ms) | FCP (ms) | LCP (ms) | CLS | TBT (ms) | Transfer Bytes |',
    '| --- | --- | --- | --- | ---: | ---: | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: |',
    ...pageLines,
    '',
    '## Collection Image Focus',
    '| Scenario | Viewport | Route | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |',
    '| --- | --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |',
    ...collectionFocusLines,
    '',
    '## Suspicious Pages (>25 requests)',
    '| Scenario | Viewport | Feature | Route | Request Count | Severity | API | Static | Third-party |',
    '| --- | --- | --- | --- | ---: | --- | ---: | ---: | ---: |',
    ...suspiciousLines,
    '',
    '## High Slow Requests (>800ms)',
    '| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |',
    '| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |',
    ...slowHighLines,
    '',
    '## Medium Slow Requests (400-800ms)',
    '| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | Category | URL |',
    '| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- | --- |',
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
  const collectionPages = allPages.filter((page) => page.route === `${LOCALE_PREFIX}/collection`);
  const highRequestPages = allPages.filter((page) => page.requestCount > REQUEST_COUNT_HIGH_THRESHOLD);
  const criticalRequestPages = allPages.filter((page) => page.requestCount > REQUEST_COUNT_CRITICAL_THRESHOLD);
  const highSlowRequests = allPages
    .flatMap((page) => page.slowRequests.map((request) => ({ ...request, scenario: page.scenario, viewport: page.viewport, route: page.route, feature: page.feature })))
    .filter((request) => (request.durationMs ?? 0) > SLOW_REQUEST_HIGH_THRESHOLD_MS)
    .sort((left, right) => (right.durationMs ?? 0) - (left.durationMs ?? 0));
  const mediumSlowRequests = allPages
    .flatMap((page) => page.slowRequests.map((request) => ({ ...request, scenario: page.scenario, viewport: page.viewport, route: page.route, feature: page.feature })))
    .filter((request) => {
      const duration = request.durationMs ?? 0;
      return duration > SLOW_REQUEST_MEDIUM_THRESHOLD_MS && duration <= SLOW_REQUEST_HIGH_THRESHOLD_MS;
    })
    .sort((left, right) => (right.durationMs ?? 0) - (left.durationMs ?? 0));

  const topSlowPages = [...allPages]
    .sort((left, right) => right.navigationMs - left.navigationMs)
    .slice(0, 10);

  const duplicateSummary = allPages
    .flatMap((page) => page.duplicateRequestGroups.map((group) => ({ ...group, scenario: page.scenario, viewport: page.viewport, route: page.route, feature: page.feature })))
    .filter((entry) => !entry.key.includes('/cdn-cgi/rum'))
    .sort((left, right) => right.count - left.count)
    .slice(0, 20);

  const collectionImageSummaryLines = collectionPages.length > 0
    ? collectionPages.map((page) =>
      `| ${page.scenario} | ${page.viewport} | ${page.collectionImageRequestCount} | ${page.collectionImageSlowRequestMediumCount} | ${page.collectionImageSlowRequestHighCount} | ${page.collectionImageFirstLoadRequestCount} | ${page.collectionImageReopenRequestCount} | ${page.collectionImageCacheHitCount} |`,
    )
    : ['| - | - | - | - | - | - | - | - |'];

  const scenarioLines = result.scenarios.map((scenario) => {
    const totalRequests = scenario.pages.reduce((sum, page) => sum + page.requestCount, 0);
    const avgNavigationMs = scenario.pages.length > 0
      ? scenario.pages.reduce((sum, page) => sum + page.navigationMs, 0) / scenario.pages.length
      : 0;
    const avgRequestsPerPage = scenario.pages.length > 0 ? totalRequests / scenario.pages.length : 0;
    const pendingCount = scenario.pages.reduce((sum, page) => sum + page.pendingCount, 0);
    const documentReloads = scenario.pages.reduce((sum, page) => sum + page.documentReloadCount, 0);
    const handshakeRedirects = scenario.pages.reduce((sum, page) => sum + page.handshakeRedirectCount, 0);
    const sessionApiCalls = scenario.pages.reduce((sum, page) => sum + page.sessionApiCallCount, 0);
    const failedRequests = scenario.pages.reduce((sum, page) => sum + page.failedRequestCount, 0);
    return `| ${scenario.scenario} | ${scenario.viewport} | ${scenario.pages.length} | ${avgRequestsPerPage.toFixed(1)} | ${avgNavigationMs.toFixed(0)} | ${pendingCount} | ${documentReloads} | ${handshakeRedirects} | ${sessionApiCalls} | ${failedRequests} | ${scenario.loginBootstrapSucceeded ? 'yes' : 'no'} |`;
  });
  const routeFamilySummaryLines = buildRouteFamilySummaryLines(result);

  const topSlowPageLines = topSlowPages.map((page) =>
    `| ${page.scenario} | ${page.viewport} | ${page.feature} | ${page.route} | ${page.navigationMs} | ${page.requestCount} | ${formatNumber(page.lcpMs)} | ${formatNumber(page.tbt, 1)} | ${formatNumber(page.cls, 4)} |`,
  );

  const highSlowRequestLines = highSlowRequests.slice(0, 15).map((request) =>
    `| ${request.scenario} | ${request.viewport} | ${request.feature} | ${request.route} | ${request.method} | ${request.status ?? '-'} | ${formatNumber(request.durationMs)} | ${formatNumber(request.ttfbMs)} | ${request.url} |`,
  );

  const mediumSlowRequestLines = mediumSlowRequests.slice(0, 15).map((request) =>
    `| ${request.scenario} | ${request.viewport} | ${request.feature} | ${request.route} | ${request.method} | ${request.status ?? '-'} | ${formatNumber(request.durationMs)} | ${formatNumber(request.ttfbMs)} | ${request.url} |`,
  );

  const duplicateLines = duplicateSummary.map((entry) =>
    `| ${entry.scenario} | ${entry.viewport} | ${entry.feature} | ${entry.route} | ${entry.count} | ${entry.key} |`,
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
    (() => {
      const handshakeLoops = allPages.reduce((sum, page) => sum + page.handshakeRedirectCount, 0);
      return handshakeLoops > 0
        ? `High: phát hiện ${handshakeLoops} handshake redirect(s), cần kiểm tra vòng lặp auth/session.`
        : 'Không phát hiện handshake redirect bất thường.';
    })(),
    (() => {
      if (collectionPages.length === 0) {
        return 'Collection-focus: không có sample `/vi/collection` trong run.';
      }
      const totalCollectionImageSlowHigh = collectionPages.reduce((sum, page) => sum + page.collectionImageSlowRequestHighCount, 0);
      const totalCollectionImageRequests = collectionPages.reduce((sum, page) => sum + page.collectionImageRequestCount, 0);
      return `Collection-focus: ${totalCollectionImageSlowHigh} image request(s) >${SLOW_REQUEST_HIGH_THRESHOLD_MS}ms trên ${totalCollectionImageRequests} image request(s).`;
    })(),
  ];

  return [
    '# TarotNow Benchmark Analysis',
    '',
    `- Run time (UTC): ${result.generatedAtUtc}`,
    `- Base: ${BASE_URL}`,
    `- Benchmark mode: ${result.benchmarkMode}`,
    '- Matrix: Chromium desktop + mobile, logged-out + logged-in-admin + logged-in-reader',
    '',
    '## Scenario Summary',
    '| Scenario | Viewport | Pages | Avg requests/page | Avg nav (ms) | Pending | Doc Reloads | Handshake Redirects | Session API Calls | Failed Requests | Login bootstrap |',
    '| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: | ---: | ---: | --- |',
    ...scenarioLines,
    '',
    '## Route-Family Summary',
    '| Scenario | Viewport | Route Family | Pages | Avg Requests/Page | Avg Navigation (ms) | Pending |',
    '| --- | --- | --- | ---: | ---: | ---: | ---: |',
    ...(routeFamilySummaryLines.length > 0 ? routeFamilySummaryLines : ['| - | - | - | - | - | - | - |']),
    '',
    '## Key Findings',
    ...keyFindings.map((item, index) => `${index + 1}. ${item}`),
    '',
    '## Top Slow Pages',
    '| Scenario | Viewport | Feature | Route | Navigate (ms) | Request count | LCP (ms) | TBT (ms) | CLS |',
    '| --- | --- | --- | --- | ---: | ---: | ---: | ---: | ---: |',
    ...(topSlowPageLines.length > 0 ? topSlowPageLines : ['| - | - | - | - | - | - | - | - |']),
    '',
    `## High Slow Requests (> ${SLOW_REQUEST_HIGH_THRESHOLD_MS}ms)`,
    '| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |',
    '| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |',
    ...(highSlowRequestLines.length > 0 ? highSlowRequestLines : ['| - | - | - | - | - | - | - | - |']),
    '',
    `## Medium Slow Requests (${SLOW_REQUEST_MEDIUM_THRESHOLD_MS}-${SLOW_REQUEST_HIGH_THRESHOLD_MS}ms)`,
    '| Scenario | Viewport | Feature | Route | Method | Status | Duration (ms) | TTFB (ms) | URL |',
    '| --- | --- | --- | --- | --- | ---: | ---: | ---: | --- |',
    ...(mediumSlowRequestLines.length > 0 ? mediumSlowRequestLines : ['| - | - | - | - | - | - | - | - |']),
    '',
    '## Duplicate Request Candidates (Non-telemetry)',
    '| Scenario | Viewport | Feature | Route | Count | Request Key |',
    '| --- | --- | --- | --- | ---: | --- |',
    ...(duplicateLines.length > 0 ? duplicateLines : ['| - | - | - | - | - |']),
    '',
    '## Collection Image Metrics',
    '| Scenario | Viewport | Image Requests | Image 400-800ms | Image >800ms | First-load Img Requests | Reopen Img Requests | 304 Cache Hits |',
    '| --- | --- | ---: | ---: | ---: | ---: | ---: | ---: |',
    ...collectionImageSummaryLines,
    '',
    '## Notes',
    '- Duplicate `/cdn-cgi/rum` được xem là telemetry của Cloudflare, không coi là business over-fetch.',
    '- Coverage dynamic route có thể bị `coverage-blocked` nếu environment không có dữ liệu phù hợp tại thời điểm chạy.',
  ].join('\n');
}

async function writeRouteMap(result: BenchmarkRunResult, outputPath: string): Promise<void> {
  const map = result.scenarios.map((scenario) => ({
    scenario: scenario.scenario,
    viewport: scenario.viewport,
    visitedRoutes: scenario.visitedRoutes,
    coverageNotes: scenario.coverageNotes,
  }));

  await fs.writeFile(outputPath, JSON.stringify(map, null, 2), 'utf8');
}

async function writeJsonAtomic(filePath: string, value: unknown): Promise<void> {
  const tempPath = `${filePath}.tmp`;
  await fs.writeFile(tempPath, JSON.stringify(value, null, 2), 'utf8');
  await fs.rename(tempPath, filePath);
}

async function readBenchmarkIndex(): Promise<unknown[]> {
  try {
    const raw = await fs.readFile(OUTPUT_INDEX_JSON, 'utf8');
    const parsed = JSON.parse(raw) as unknown;
    return Array.isArray(parsed) ? parsed : [];
  } catch {
    return [];
  }
}

async function writeBenchmarkIndex(result: BenchmarkRunResult, outputPaths: BenchmarkOutputPaths): Promise<void> {
  await fs.mkdir(OUTPUT_DIR, { recursive: true });
  const index = await readBenchmarkIndex();
  index.push({
    runId: result.runId,
    generatedAtUtc: result.generatedAtUtc,
    baseOrigin: result.baseOrigin,
    benchmarkMode: result.benchmarkMode,
    json: path.relative(process.cwd(), outputPaths.json),
    latestJson: path.relative(process.cwd(), outputPaths.latestJson),
    legacyJson: path.relative(process.cwd(), outputPaths.legacyJson),
  });
  await writeJsonAtomic(OUTPUT_INDEX_JSON, index);
}

async function writeBenchmarkArtifacts(result: BenchmarkRunResult, outputPaths: BenchmarkOutputPaths): Promise<void> {
  await fs.mkdir(path.dirname(outputPaths.json), { recursive: true });
  await fs.mkdir(path.dirname(outputPaths.latestJson), { recursive: true });
  await fs.mkdir(path.dirname(outputPaths.legacyJson), { recursive: true });

  const resultWithPaths: BenchmarkRunResult = {
    ...result,
    artifactPaths: {
      immutableJson: path.relative(process.cwd(), outputPaths.json),
      latestJson: path.relative(process.cwd(), outputPaths.latestJson),
      legacyJson: path.relative(process.cwd(), outputPaths.legacyJson),
    },
  };

  const json = JSON.stringify(resultWithPaths, null, 2);
  await fs.writeFile(outputPaths.json, json, 'utf8');
  await fs.writeFile(outputPaths.pagesCsv, createPagesCsv(resultWithPaths), 'utf8');
  await fs.writeFile(outputPaths.requestsCsv, createRequestsCsv(resultWithPaths), 'utf8');
  await fs.writeFile(outputPaths.reportMd, createMarkdownReport(resultWithPaths), 'utf8');
  await fs.writeFile(outputPaths.analysisMd, createAnalysisReport(resultWithPaths), 'utf8');
  await writeRouteMap(resultWithPaths, outputPaths.routeMap);
  await writeJsonAtomic(outputPaths.latestJson, resultWithPaths);
  await writeJsonAtomic(outputPaths.legacyJson, resultWithPaths);
  await writeBenchmarkIndex(resultWithPaths, outputPaths);
}

test.describe('TarotNow production benchmark', () => {
  test.describe.configure({ mode: 'serial' });
  test.setTimeout(45 * 60 * 1000);

  async function runBenchmarkSuite(browser: Browser, mode: BenchmarkMode): Promise<BenchmarkRunResult> {
    const staticRoutes = await collectStaticLocaleRoutes();
    const originRouteCollection = await collectOriginDiscoveredRoutes();
    const fullSeedRoutes = [...new Set([...CORE_ROUTE_SEEDS, ...staticRoutes, ...originRouteCollection.routes])];
    const modeSeedRoutes = resolveSeedRoutesForMode(mode, fullSeedRoutes);

    const scenarios: BenchmarkScenario[] = ['logged-out', 'logged-in-admin', 'logged-in-reader'];
    const scenarioResults: ScenarioBenchmarkResult[] = [];

    for (const viewport of VIEWPORT_PROFILES) {
      for (const scenario of scenarios) {
        const result = await runScenario(browser, scenario, viewport, modeSeedRoutes, originRouteCollection.notes, {
          enableRecursiveDomDiscovery: mode !== 'targeted-hotspots',
        });
        scenarioResults.push(result);
      }
    }

    return {
      generatedAtUtc: new Date().toISOString(),
      runId: OUTPUT_RUN_ID,
      baseOrigin: BASE_ORIGIN,
      localePrefix: LOCALE_PREFIX,
      benchmarkMode: mode,
      thresholds: {
        requestCountHigh: REQUEST_COUNT_HIGH_THRESHOLD,
        requestCountCritical: REQUEST_COUNT_CRITICAL_THRESHOLD,
        slowRequestMediumMs: SLOW_REQUEST_MEDIUM_THRESHOLD_MS,
        slowRequestHighMs: SLOW_REQUEST_HIGH_THRESHOLD_MS,
      },
      scenarios: scenarioResults,
    };
  }

  function assertScenarioIntegrity(runResult: BenchmarkRunResult): void {
    for (const scenario of AUTH_REQUIRED_SCENARIOS) {
      for (const viewport of VIEWPORT_PROFILES) {
        const record = runResult.scenarios.find((item) => item.scenario === scenario && item.viewport === viewport.id);
        expect(record?.loginBootstrapSucceeded ?? false).toBeTruthy();
      }
    }

    for (const scenario of runResult.scenarios) {
      if (
        (runResult.benchmarkMode === 'targeted-hotspots' || runResult.benchmarkMode === 'feature-matrix')
        && scenario.scenario === 'logged-out'
        && scenario.pages.length === 0
      ) {
        continue;
      }

      expect(scenario.pages.length).toBeGreaterThan(0);
    }
  }

  test('benchmark navigation and api timing for full vi route matrix', async ({ browser }) => {
    test.skip(!RUN_NAVIGATION_BENCHMARK, 'Set RUN_NAVIGATION_BENCHMARK=true to run benchmark.');
    assertSafeBenchmarkEnvironment();

    const runResult = await runBenchmarkSuite(browser, 'full-matrix');
    await writeBenchmarkArtifacts(runResult, resolveOutputPaths('full-matrix'));
    assertScenarioIntegrity(runResult);
  });

  test('benchmark hotspot routes for cycle2 targeted verification', async ({ browser }) => {
    test.skip(!RUN_NAVIGATION_BENCHMARK_TARGETED, 'Set RUN_NAVIGATION_BENCHMARK_TARGETED=true to run targeted benchmark.');
    assertSafeBenchmarkEnvironment();

    const runResult = await runBenchmarkSuite(browser, 'targeted-hotspots');
    await writeBenchmarkArtifacts(runResult, resolveOutputPaths('targeted-hotspots'));
    assertScenarioIntegrity(runResult);
  });

  test('benchmark one feature route matrix', async ({ browser }) => {
    test.skip(!RUN_NAVIGATION_BENCHMARK_FEATURE, 'Set RUN_NAVIGATION_BENCHMARK_FEATURE=true and BENCHMARK_FEATURE=<feature> to run feature benchmark.');
    assertSafeBenchmarkEnvironment();
    assertBenchmarkFeature(BENCHMARK_FEATURE);

    const runResult = await runBenchmarkSuite(browser, 'feature-matrix');
    await writeBenchmarkArtifacts(runResult, resolveOutputPaths('feature-matrix'));
    assertScenarioIntegrity(runResult);
  });
});
