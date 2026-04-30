import { readFileSync } from 'node:fs';
import { resolve } from 'node:path';

function parseArgs(argv) {
 const args = {};
 for (let index = 0; index < argv.length; index += 1) {
  const token = argv[index];
  if (!token || !token.startsWith('--')) {
   continue;
  }

  const raw = token.slice(2);
  if (raw.includes('=')) {
   const [key, value] = raw.split('=');
   args[key] = value;
   continue;
  }

  const next = argv[index + 1];
  if (next && !next.startsWith('--')) {
   args[raw] = next;
   index += 1;
   continue;
  }

  args[raw] = 'true';
 }
 return args;
}

function parseNumber(value, fallback) {
 if (value === undefined || value === null || value === '') {
  return fallback;
 }
 const parsed = Number(value);
 return Number.isFinite(parsed) ? parsed : fallback;
}

function loadBenchmark(absolutePath) {
 try {
  const raw = readFileSync(absolutePath, 'utf8');
  const parsed = JSON.parse(raw);
  return parsed;
 } catch (error) {
  throw new Error(`Cannot read benchmark file: ${absolutePath}. ${error instanceof Error ? error.message : String(error)}`);
 }
}

function getAllPages(benchmark) {
 const scenarios = Array.isArray(benchmark?.scenarios) ? benchmark.scenarios : [];
 return scenarios.flatMap((scenario) => Array.isArray(scenario?.pages) ? scenario.pages : []);
}

function isAuthenticatedScenario(scenario) {
 return scenario === 'logged-in-admin' || scenario === 'logged-in-reader';
}

function median(values) {
 if (!Array.isArray(values) || values.length === 0) {
  return 0;
 }

 const sorted = [...values]
  .map((value) => Number(value))
  .filter((value) => Number.isFinite(value))
  .sort((left, right) => left - right);

 if (sorted.length === 0) {
  return 0;
 }

 const midpoint = Math.floor(sorted.length / 2);
 if (sorted.length % 2 === 0) {
  return (sorted[midpoint - 1] + sorted[midpoint]) / 2;
 }

 return sorted[midpoint];
}

function reductionPercent(before, after) {
 if (!Number.isFinite(before) || before <= 0) {
  return 0;
 }

 return ((before - after) / before) * 100;
}

function countSlowRequestsOver(pages, thresholdMs) {
 return pages.reduce((total, page) => {
  const slowRequests = Array.isArray(page?.slowRequests) ? page.slowRequests : [];
  const count = slowRequests.filter((request) => Number(request?.durationMs ?? 0) > thresholdMs).length;
  return total + count;
 }, 0);
}

function isCollectionImageRequest(request) {
 if (!request || typeof request.url !== 'string') {
  return false;
 }

 const url = request.url;
 return url.includes('/api/collection/card-image')
  || url.includes('img.tarotnow.xyz')
  || url.includes('media.tarotnow.xyz')
  || (url.includes('/_next/image') && url.includes('collection%2Fcard-image'));
}

function countCollectionImageSlowRequestsOver(pages, thresholdMs) {
 return pages.reduce((total, page) => {
  const route = page?.route;
  if (route !== '/vi/collection') {
   return total;
  }

  if (typeof page?.collectionImageSlowRequestHighCount === 'number' && thresholdMs >= 800) {
   return total + Number(page.collectionImageSlowRequestHighCount ?? 0);
  }

  const requests = Array.isArray(page?.requests) ? page.requests : [];
  const count = requests
   .filter((request) => isCollectionImageRequest(request))
   .filter((request) => Number(request?.durationMs ?? 0) > thresholdMs)
   .length;
  return total + count;
 }, 0);
}

function pickCollectionDesktopPage(pages) {
 const candidates = pages.filter((page) =>
  page?.route === '/vi/collection'
   && page?.viewport === 'desktop'
   && (page?.scenario === 'logged-in-admin' || page?.scenario === 'logged-in-reader')
 );

 if (candidates.length === 0) {
  return null;
 }

 return candidates.reduce((current, next) => (
  Number(next.requestCount ?? 0) > Number(current.requestCount ?? 0) ? next : current
 ));
}

function pickAdminDesktopPage(pages) {
 const candidates = pages.filter((page) =>
  page?.scenario === 'logged-in-admin'
  && page?.viewport === 'desktop'
  && page?.route === '/vi/admin',
 );
 if (candidates.length === 0) {
  return null;
 }

 return candidates.reduce((current, next) => (
  Number(next.requestCount ?? 0) > Number(current.requestCount ?? 0) ? next : current
 ));
}

function formatPercent(value) {
 return `${value.toFixed(2)}%`;
}

const args = parseArgs(process.argv.slice(2));
const cwd = process.cwd();
const currentFile = resolve(
 cwd,
 args.current || process.env.PERF_CURRENT_BENCHMARK || 'test-results/benchmark/tarotnow-benchmark.json',
);
const baselineFile = resolve(
 cwd,
 args.baseline || process.env.PERF_BASELINE_BENCHMARK || 'test-results/benchmark/tarotnow-benchmark-baseline.json',
);

const minAuthMedianReductionPct = parseNumber(
 args.minAuthMedianReductionPct ?? process.env.PERF_GATE_MIN_AUTH_MEDIAN_REDUCTION_PCT,
 20,
);
const minSlowHighReductionPct = parseNumber(
 args.minSlowHighReductionPct ?? process.env.PERF_GATE_MIN_SLOW_HIGH_REDUCTION_PCT,
 30,
);
const maxAdminDesktopRequestCount = parseNumber(
 args.maxAdminDesktopRequestCount ?? process.env.PERF_GATE_MAX_ADMIN_DESKTOP_REQUESTS,
 220,
);
const maxAdminDesktopDocumentReloads = parseNumber(
 args.maxAdminDesktopDocumentReloads ?? process.env.PERF_GATE_MAX_ADMIN_DESKTOP_DOCUMENT_RELOADS,
 0,
);
const maxAdminDesktopHandshakeRedirects = parseNumber(
 args.maxAdminDesktopHandshakeRedirects ?? process.env.PERF_GATE_MAX_ADMIN_DESKTOP_HANDSHAKE_REDIRECTS,
 0,
);
const maxAdminDesktopSessionApiCalls = parseNumber(
 args.maxAdminDesktopSessionApiCalls ?? process.env.PERF_GATE_MAX_ADMIN_DESKTOP_SESSION_API_CALLS,
 4,
);
const maxCollectionDesktopRequestCount = parseNumber(
 args.maxCollectionDesktopRequestCount ?? process.env.PERF_GATE_MAX_COLLECTION_DESKTOP_REQUESTS,
 60,
);
const minCollectionImageSlowHighReductionPct = parseNumber(
 args.minCollectionImageSlowHighReductionPct ?? process.env.PERF_GATE_MIN_COLLECTION_IMAGE_SLOW_HIGH_REDUCTION_PCT,
 70,
);

const baseline = loadBenchmark(baselineFile);
const current = loadBenchmark(currentFile);

const baselinePages = getAllPages(baseline);
const currentPages = getAllPages(current);

const baselineAuthPages = baselinePages.filter((page) => isAuthenticatedScenario(page?.scenario));
const currentAuthPages = currentPages.filter((page) => isAuthenticatedScenario(page?.scenario));

const baselineAuthMedianRequests = median(baselineAuthPages.map((page) => page?.requestCount ?? 0));
const currentAuthMedianRequests = median(currentAuthPages.map((page) => page?.requestCount ?? 0));
const authMedianReductionPct = reductionPercent(baselineAuthMedianRequests, currentAuthMedianRequests);

const baselineSlowHighCount = countSlowRequestsOver(baselinePages, 800);
const currentSlowHighCount = countSlowRequestsOver(currentPages, 800);
const slowHighReductionPct = reductionPercent(baselineSlowHighCount, currentSlowHighCount);
const baselineCollectionImageSlowHighCount = countCollectionImageSlowRequestsOver(baselinePages, 800);
const currentCollectionImageSlowHighCount = countCollectionImageSlowRequestsOver(currentPages, 800);
const collectionImageSlowHighReductionPct = reductionPercent(
 baselineCollectionImageSlowHighCount,
 currentCollectionImageSlowHighCount,
);

const adminDesktopPage = pickAdminDesktopPage(currentPages);
const collectionDesktopPage = pickCollectionDesktopPage(currentPages);

const failures = [];

if (authMedianReductionPct < minAuthMedianReductionPct) {
 failures.push(
  `Authenticated median request/page reduction is ${formatPercent(authMedianReductionPct)} (required >= ${formatPercent(minAuthMedianReductionPct)}).`,
 );
}

if (baselineSlowHighCount > 0 && slowHighReductionPct < minSlowHighReductionPct) {
 failures.push(
  `High slow-request reduction (>800ms) is ${formatPercent(slowHighReductionPct)} (required >= ${formatPercent(minSlowHighReductionPct)}).`,
 );
}

if (!adminDesktopPage) {
 failures.push('Missing /vi/admin page data for logged-in-admin desktop scenario.');
} else {
 const requestCount = Number(adminDesktopPage.requestCount ?? 0);
 const documentReloadCount = Number(adminDesktopPage.documentReloadCount ?? 0);
 const handshakeRedirectCount = Number(adminDesktopPage.handshakeRedirectCount ?? 0);
 const sessionApiCallCount = Number(adminDesktopPage.sessionApiCallCount ?? 0);

 if (requestCount > maxAdminDesktopRequestCount) {
  failures.push(
   `/vi/admin desktop request count is ${requestCount} (max ${maxAdminDesktopRequestCount}).`,
  );
 }

 if (documentReloadCount > maxAdminDesktopDocumentReloads) {
  failures.push(
   `/vi/admin desktop document reload count is ${documentReloadCount} (max ${maxAdminDesktopDocumentReloads}).`,
  );
 }

 if (handshakeRedirectCount > maxAdminDesktopHandshakeRedirects) {
  failures.push(
   `/vi/admin desktop handshake redirects is ${handshakeRedirectCount} (max ${maxAdminDesktopHandshakeRedirects}).`,
  );
 }

 if (sessionApiCallCount > maxAdminDesktopSessionApiCalls) {
  failures.push(
   `/vi/admin desktop session API calls is ${sessionApiCallCount} (max ${maxAdminDesktopSessionApiCalls}).`,
  );
 }
}

if (!collectionDesktopPage) {
 failures.push('Missing /vi/collection page data for authenticated desktop scenarios.');
} else {
 const requestCount = Number(collectionDesktopPage.requestCount ?? 0);
 if (requestCount > maxCollectionDesktopRequestCount) {
  failures.push(
   `/vi/collection desktop request count is ${requestCount} (max ${maxCollectionDesktopRequestCount}).`,
  );
 }
}

if (baselineCollectionImageSlowHighCount > 0 && collectionImageSlowHighReductionPct < minCollectionImageSlowHighReductionPct) {
 failures.push(
  `Collection image slow-request reduction (>800ms) is ${formatPercent(collectionImageSlowHighReductionPct)} (required >= ${formatPercent(minCollectionImageSlowHighReductionPct)}).`,
 );
}

const summaryLines = [
 `Current file: ${currentFile}`,
 `Baseline file: ${baselineFile}`,
 `Auth median request/page: baseline=${baselineAuthMedianRequests.toFixed(2)} current=${currentAuthMedianRequests.toFixed(2)} reduction=${formatPercent(authMedianReductionPct)}`,
 `Slow requests >800ms: baseline=${baselineSlowHighCount} current=${currentSlowHighCount} reduction=${formatPercent(slowHighReductionPct)}`,
 `Collection image slow requests >800ms: baseline=${baselineCollectionImageSlowHighCount} current=${currentCollectionImageSlowHighCount} reduction=${formatPercent(collectionImageSlowHighReductionPct)}`,
];

if (adminDesktopPage) {
 summaryLines.push(
  `/vi/admin desktop: requests=${adminDesktopPage.requestCount}, docReloads=${adminDesktopPage.documentReloadCount}, handshakes=${adminDesktopPage.handshakeRedirectCount}, sessionApiCalls=${adminDesktopPage.sessionApiCallCount}`,
 );
}
if (collectionDesktopPage) {
 summaryLines.push(
  `/vi/collection desktop: requests=${collectionDesktopPage.requestCount}, imageRequests=${collectionDesktopPage.collectionImageRequestCount ?? 'n/a'}, image>800ms=${collectionDesktopPage.collectionImageSlowRequestHighCount ?? 'n/a'}, imageReloadRequests=${collectionDesktopPage.collectionImageReloadRequestCount ?? 'n/a'}, image304=${collectionDesktopPage.collectionImageCacheHitCount ?? 'n/a'}`,
 );
}

for (const line of summaryLines) {
 console.log(line);
}

if (failures.length > 0) {
 console.error('\nPerf gate failed:');
 for (const failure of failures) {
  console.error(`- ${failure}`);
 }
 process.exit(1);
}

console.log('\nPerf gate passed.');
