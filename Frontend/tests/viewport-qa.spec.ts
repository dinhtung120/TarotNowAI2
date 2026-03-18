import fs from "node:fs";
import path from "node:path";
import { test } from "@playwright/test";

type RouteItem = {
  name: string;
  path: string;
};

type ViewportItem = {
  name: string;
  width: number;
  height: number;
};

type PageMetrics = {
  title: string;
  viewportWidth: number;
  documentWidth: number;
  hasHorizontalOverflow: boolean;
  overflowingNodes: string[];
  offscreenInteractive: string[];
  smallTapTargets: string[];
};

const VIEWPORTS: ViewportItem[] = [
  { name: "mobile", width: 390, height: 844 },
  { name: "tablet", width: 834, height: 1194 },
  { name: "desktop", width: 1440, height: 900 },
];

const ROUTES: RouteItem[] = [
  { name: "Home", path: "/vi" },
  { name: "Legal/TOS", path: "/vi/legal/tos" },
  { name: "Legal/Privacy", path: "/vi/legal/privacy" },
  { name: "Legal/AI Disclaimer", path: "/vi/legal/ai-disclaimer" },
  { name: "Login", path: "/vi/login" },
  { name: "Register", path: "/vi/register" },
  { name: "Forgot Password", path: "/vi/forgot-password" },
  { name: "Reset Password", path: "/vi/reset-password" },
  { name: "Verify Email", path: "/vi/verify-email" },
  { name: "Reading Setup", path: "/vi/reading" },
  { name: "Reading History", path: "/vi/reading/history" },
  { name: "Reading Session", path: "/vi/reading/session/00000000-0000-0000-0000-000000000000" },
  { name: "Reader Directory", path: "/vi/readers" },
  { name: "Reader Detail", path: "/vi/readers/00000000-0000-0000-0000-000000000000" },
  { name: "Inbox", path: "/vi/chat" },
  { name: "Chat Room", path: "/vi/chat/00000000-0000-0000-0000-000000000000" },
  { name: "Collection", path: "/vi/collection" },
  { name: "Wallet", path: "/vi/wallet" },
  { name: "Wallet/Deposit", path: "/vi/wallet/deposit" },
  { name: "Wallet/Withdraw", path: "/vi/wallet/withdraw" },
  { name: "Profile", path: "/vi/profile" },
  { name: "Profile/MFA", path: "/vi/profile/mfa" },
  { name: "Profile/Reader", path: "/vi/profile/reader" },
  { name: "Reader Apply", path: "/vi/reader/apply" },
  { name: "Admin Dashboard", path: "/vi/admin" },
  { name: "Admin/Users", path: "/vi/admin/users" },
  { name: "Admin/Deposits", path: "/vi/admin/deposits" },
  { name: "Admin/Promotions", path: "/vi/admin/promotions" },
  { name: "Admin/Readings", path: "/vi/admin/readings" },
  { name: "Admin/Reader Requests", path: "/vi/admin/reader-requests" },
  { name: "Admin/Withdrawals", path: "/vi/admin/withdrawals" },
  { name: "Admin/Disputes", path: "/vi/admin/disputes" },
];

const QA_TOKEN_EXP = Math.floor(Date.now() / 1000) + 7 * 24 * 60 * 60;
const QA_JWT = [
  Buffer.from(JSON.stringify({ alg: "HS256", typ: "JWT" })).toString("base64url"),
  Buffer.from(
    JSON.stringify({
      sub: "qa-user",
      nameid: "qa-user",
      exp: QA_TOKEN_EXP,
      role: "admin",
    }),
  ).toString("base64url"),
  "qa-signature",
].join(".");

const QA_AUTH_STATE = JSON.stringify({
  state: {
    user: {
      id: "qa-user",
      email: "qa@example.com",
      username: "qa-user",
      displayName: "QA User",
      role: "admin",
    },
    accessToken: QA_JWT,
  },
  version: 0,
});

const metricsScript = (): PageMetrics => {
  const doc = document.documentElement;
  const body = document.body;
  const viewportWidth = window.innerWidth;
  const documentWidth = Math.max(doc.scrollWidth, body.scrollWidth);

  const isVisible = (el: HTMLElement) => {
    const style = window.getComputedStyle(el);
    const rect = el.getBoundingClientRect();
    return (
      rect.width > 0 &&
      rect.height > 0 &&
      style.visibility !== "hidden" &&
      style.display !== "none" &&
      style.opacity !== "0"
    );
  };

  const toSelector = (el: Element) => {
    const id = el.getAttribute("id");
    if (id) return `#${id}`;
    const cls = (el.getAttribute("class") || "").trim().split(/\s+/).filter(Boolean);
    if (cls.length > 0) return `${el.tagName.toLowerCase()}.${cls.slice(0, 2).join(".")}`;
    return el.tagName.toLowerCase();
  };

  const overflowingNodes = Array.from(document.querySelectorAll<HTMLElement>("body *"))
    .filter((el) => {
      if (!isVisible(el)) return false;
      const rect = el.getBoundingClientRect();
      return rect.right > viewportWidth + 2 && rect.left < viewportWidth;
    })
    .slice(0, 20)
    .map((el) => `${toSelector(el)} (${Math.round(el.getBoundingClientRect().width)}px)`);

  const interactiveNodes = Array.from(
    document.querySelectorAll<HTMLElement>(
      'a, button, input, select, textarea, [role="button"], [role="link"]',
    ),
  ).filter(isVisible);

  const offscreenInteractive = interactiveNodes
    .filter((el) => {
      const rect = el.getBoundingClientRect();
      return rect.left < -1 || rect.right > viewportWidth + 1;
    })
    .slice(0, 20)
    .map((el) => toSelector(el));

  const smallTapTargets = interactiveNodes
    .filter((el) => {
      const rect = el.getBoundingClientRect();
      return rect.width < 44 || rect.height < 44;
    })
    .slice(0, 20)
    .map((el) => {
      const rect = el.getBoundingClientRect();
      return `${toSelector(el)} (${Math.round(rect.width)}x${Math.round(rect.height)})`;
    });

  return {
    title: document.title,
    viewportWidth,
    documentWidth,
    hasHorizontalOverflow: documentWidth > viewportWidth + 1,
    overflowingNodes,
    offscreenInteractive,
    smallTapTargets,
  };
};

test.describe.configure({ mode: "serial" });
test.setTimeout(8 * 60 * 1000);

test("viewport QA (mobile/tablet/desktop)", async ({ page }) => {
  const baseURL = process.env.QA_BASE_URL || "http://127.0.0.1:3100";
  const reportRows: Array<Record<string, unknown>> = [];
  const baseHost = new URL(baseURL).hostname;

  await page.context().addCookies([
    {
      name: "accessToken",
      value: QA_JWT,
      domain: baseHost,
      path: "/",
      expires: QA_TOKEN_EXP,
      httpOnly: false,
      secure: false,
      sameSite: "Lax",
    },
  ]);

  await page.addInitScript(([key, value]) => {
    localStorage.setItem(key as string, value as string);
  }, ["tarot-now-auth", QA_AUTH_STATE]);

  for (const viewport of VIEWPORTS) {
    await page.setViewportSize({ width: viewport.width, height: viewport.height });

    for (const route of ROUTES) {
      const pageErrors: string[] = [];
      const failedRequests: string[] = [];
      const consoleErrors: string[] = [];

      const onPageError = (err: Error) => pageErrors.push(err.message);
      const onRequestFailed = (request: { url: () => string; failure: () => { errorText?: string } | null }) => {
        const failure = request.failure();
        const message = failure?.errorText || "request failed";
        failedRequests.push(`${request.url()} - ${message}`);
      };
      const onConsole = (msg: { type: () => string; text: () => string }) => {
        if (msg.type() === "error") consoleErrors.push(msg.text());
      };

      page.on("pageerror", onPageError);
      page.on("requestfailed", onRequestFailed);
      page.on("console", onConsole);

      try {
        await page.goto(`${baseURL}${route.path}`, { waitUntil: "domcontentloaded", timeout: 60_000 });
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
        page.off("pageerror", onPageError);
        page.off("requestfailed", onRequestFailed);
        page.off("console", onConsole);
      }
    }
  }

  const reportDir = path.resolve(process.cwd(), "test-results");
  fs.mkdirSync(reportDir, { recursive: true });
  const reportPath = path.join(reportDir, "viewport-qa-report.json");
  fs.writeFileSync(
    reportPath,
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
      2,
    ),
    "utf8",
  );
});
