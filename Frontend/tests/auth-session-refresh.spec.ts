import { expect, test, type APIRequestContext, type APIResponse } from '@playwright/test';

const AUTH_COOKIE = {
 ACCESS: 'accessToken',
 REFRESH: 'refreshToken',
} as const;

const BASE_URL = process.env.QA_BASE_URL || 'http://127.0.0.1:3100';

function createUnsignedJwt(expSeconds: number): string {
 const header = Buffer.from(JSON.stringify({ alg: 'none', typ: 'JWT' })).toString('base64url');
 const payload = Buffer.from(JSON.stringify({ sub: 'e2e-user', exp: expSeconds })).toString('base64url');
 return `${header}.${payload}.`;
}

function buildAuthCookieHeader(): string {
 const exp = Math.floor(Date.now() / 1000) + 10 * 60;
 const accessToken = createUnsignedJwt(exp);
 const refreshToken = `e2e-refresh-${Date.now()}`;
 return `${AUTH_COOKIE.ACCESS}=${accessToken}; ${AUTH_COOKIE.REFRESH}=${refreshToken}`;
}

async function requestProtected(
 path: string,
 cookieHeader: string,
 request: APIRequestContext,
): Promise<APIResponse> {
 return request.get(`${BASE_URL}${path}`, {
  headers: {
   Cookie: cookieHeader,
  },
  maxRedirects: 0,
 });
}

function assertNotRedirectedToLogin(response: APIResponse): void {
 const status = response.status();
 const location = response.headers()['location'] ?? '';
 const isRedirect = [301, 302, 303, 307, 308].includes(status);

 if (isRedirect) {
  expect(location).not.toContain('/vi/login');
 }
}

test.describe('auth session middleware', () => {
 test('F5 on protected route with valid access cookie does not force login redirect', async ({ request }) => {
  const cookieHeader = buildAuthCookieHeader();
  const firstLoad = await requestProtected('/vi/profile', cookieHeader, request);
  const secondLoad = await requestProtected('/vi/profile', cookieHeader, request);

  assertNotRedirectedToLogin(firstLoad);
  assertNotRedirectedToLogin(secondLoad);
 });

 test('navigating protected links keeps URL when auth cookies exist', async ({ request }) => {
  const cookieHeader = buildAuthCookieHeader();
  const chatLoad = await requestProtected('/vi/chat', cookieHeader, request);
  const walletLoad = await requestProtected('/vi/wallet', cookieHeader, request);

  assertNotRedirectedToLogin(chatLoad);
  assertNotRedirectedToLogin(walletLoad);
 });
});
