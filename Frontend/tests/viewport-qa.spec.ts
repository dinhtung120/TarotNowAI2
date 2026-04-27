import fs from 'node:fs';
import path from 'node:path';
import { expect, test } from '@playwright/test';
import {
 QA_JWT,
 QA_TOKEN_EXP,
 ROUTES,
 VIEWPORTS,
} from './helpers/viewportQaData';
import { metricsScript } from './helpers/viewportMetrics';

test.describe.configure({ mode: 'serial' });
test.setTimeout(8 * 60 * 1000);
const RUN_VIEWPORT_QA = process.env.RUN_VIEWPORT_QA === 'true';

test('viewport QA (mobile/tablet/desktop)', async ({ page }) => {
 test.skip(!RUN_VIEWPORT_QA, 'Set RUN_VIEWPORT_QA=true to run viewport quality gate.');

 const baseURL = process.env.QA_BASE_URL || 'http://127.0.0.1:3100';
 const reportRows: Array<Record<string, unknown>> = [];
 const baseHost = new URL(baseURL).hostname;

 await page.context().addCookies([
  {
   name: 'accessToken',
   value: QA_JWT,
   domain: baseHost,
   path: '/',
   expires: QA_TOKEN_EXP,
   httpOnly: true,
   secure: false,
   sameSite: 'Lax',
  },
 ]);

 for (const viewport of VIEWPORTS) {
  await page.setViewportSize({ width: viewport.width, height: viewport.height });

  for (const route of ROUTES) {
   const pageErrors: string[] = [];
   const failedRequests: string[] = [];
   const consoleErrors: string[] = [];

   const onPageError = (err: Error) => pageErrors.push(err.message);
   const onRequestFailed = (request: { url: () => string; failure: () => { errorText?: string } | null }) => {
    const failure = request.failure();
    failedRequests.push(`${request.url()} - ${failure?.errorText || 'request failed'}`);
   };
   const onConsole = (msg: { type: () => string; text: () => string }) => {
    if (msg.type() === 'error') consoleErrors.push(msg.text());
   };

   page.on('pageerror', onPageError);
   page.on('requestfailed', onRequestFailed);
   page.on('console', onConsole);

   try {
    await page.goto(`${baseURL}${route.path}`, { waitUntil: 'domcontentloaded', timeout: 60_000 });
    await page.waitForTimeout(900);
    const metrics = await page.evaluate(metricsScript);

    reportRows.push({
     route: route.name,
     requestedPath: route.path,
     finalPath: new URL(page.url()).pathname,
     viewport: viewport.name,
     width: viewport.width,
     height: viewport.height,
     title: metrics.title,
     hasHorizontalOverflow: metrics.hasHorizontalOverflow,
     documentWidth: metrics.documentWidth,
     viewportWidth: metrics.viewportWidth,
     overflowingNodes: metrics.overflowingNodes,
     offscreenInteractive: metrics.offscreenInteractive,
     smallTapTargets: metrics.smallTapTargets,
     pageErrors,
     failedRequests: failedRequests.slice(0, 15),
     consoleErrors: consoleErrors.slice(0, 15),
    });
   } catch (err) {
    reportRows.push({
     route: route.name,
     requestedPath: route.path,
     viewport: viewport.name,
     width: viewport.width,
     height: viewport.height,
     fatalError: err instanceof Error ? err.message : String(err),
    });
   } finally {
    page.off('pageerror', onPageError);
    page.off('requestfailed', onRequestFailed);
    page.off('console', onConsole);
   }
  }
 }

 const reportDir = path.resolve(process.cwd(), 'test-results');
 fs.mkdirSync(reportDir, { recursive: true });
 fs.writeFileSync(
  path.join(reportDir, 'viewport-qa-report.json'),
  JSON.stringify(
   {
    generatedAt: new Date().toISOString(),
    baseURL,
    viewportCount: VIEWPORTS.length,
    routeCount: ROUTES.length,
    totalChecks: VIEWPORTS.length * ROUTES.length,
    rows: reportRows,
   },
   null,
   2
  ),
  'utf8'
 );

 const fatalRows = reportRows.filter((row) => typeof row.fatalError === 'string' && row.fatalError.length > 0);
 const overflowRows = reportRows.filter((row) => row.hasHorizontalOverflow === true);
 const pageErrorRows = reportRows.filter((row) => Array.isArray(row.pageErrors) && row.pageErrors.length > 0);
 const failedRequestRows = reportRows.filter((row) => Array.isArray(row.failedRequests) && row.failedRequests.length > 0);

 expect(
  fatalRows,
  `Viewport QA fatal errors detected:\n${JSON.stringify(fatalRows.slice(0, 5), null, 2)}`,
 ).toHaveLength(0);
 expect(
  overflowRows,
  `Viewport QA horizontal overflow detected:\n${JSON.stringify(overflowRows.slice(0, 5), null, 2)}`,
 ).toHaveLength(0);
 expect(
  pageErrorRows,
  `Viewport QA page errors detected:\n${JSON.stringify(pageErrorRows.slice(0, 5), null, 2)}`,
 ).toHaveLength(0);
 expect(
  failedRequestRows,
  `Viewport QA failed requests detected:\n${JSON.stringify(failedRequestRows.slice(0, 5), null, 2)}`,
 ).toHaveLength(0);
});
