import fs from 'node:fs';
import path from 'node:path';
import { test } from '@playwright/test';
import {
 QA_JWT,
 QA_TOKEN_EXP,
 ROUTES,
 VIEWPORTS,
} from './helpers/viewportQaData';
import { metricsScript } from './helpers/viewportMetrics';

test.describe.configure({ mode: 'serial' });
test.setTimeout(8 * 60 * 1000);

test('viewport QA (mobile/tablet/desktop)', async ({ page }) => {
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
});
