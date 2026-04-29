import { expect, test, type APIRequestContext, type APIResponse } from '@playwright/test';

const AUTH_COOKIE = {
 ACCESS: 'accessToken',
 REFRESH: 'refreshToken',
} as const;

const BASE_URL = process.env.QA_BASE_URL || 'http://127.0.0.1:3100';
const AUTH_TEST_USER = process.env.BENCHMARK_ADMIN_USERNAME?.trim() || process.env.BENCHMARK_USERNAME?.trim() || 'Lucifer';
const AUTH_TEST_PASSWORD = process.env.BENCHMARK_ADMIN_PASSWORD?.trim() || process.env.BENCHMARK_PASSWORD?.trim() || 'Sontung123!';

function createUnsignedJwt(expSeconds: number): string {
 const header = Buffer.from(JSON.stringify({ alg: 'none', typ: 'JWT' })).toString('base64url');
 const payload = Buffer.from(JSON.stringify({ sub: 'e2e-user', exp: expSeconds })).toString('base64url');
 return `${header}.${payload}.`;
}

function buildForgedCookieHeader(): string {
 const exp = Math.floor(Date.now() / 1000) + 10 * 60;
 const accessToken = createUnsignedJwt(exp);
 const refreshToken = `forged-refresh-${Date.now()}`;
 return `${AUTH_COOKIE.ACCESS}=${accessToken}; ${AUTH_COOKIE.REFRESH}=${refreshToken}`;
}

async function requestPath(
 path: string,
 request: APIRequestContext,
 cookieHeader?: string,
): Promise<APIResponse> {
 return request.get(`${BASE_URL}${path}`, {
  headers: cookieHeader ? { Cookie: cookieHeader } : undefined,
  maxRedirects: 0,
 });
}

function expectRedirectToLogin(response: APIResponse): void {
 const status = response.status();
 const location = response.headers()['location'] ?? '';
 expect([301, 302, 303, 307, 308]).toContain(status);
 expect(location).toContain('/vi/login');
}

function expectRedirectToHandshake(response: APIResponse): string {
 const status = response.status();
 const location = response.headers()['location'] ?? '';
 expect([301, 302, 303, 307, 308]).toContain(status);
 expect(location).toContain('/api/auth/session/handshake?next=');
 return location;
}

function buildCookieHeaderFromSetCookie(response: APIResponse): string {
 const setCookieHeaders = response
  .headersArray()
  .filter((header) => header.name.toLowerCase() === 'set-cookie')
  .map((header) => header.value);
 const cookiePairs = setCookieHeaders
  .map((cookie) => cookie.split(';')[0]?.trim())
  .filter((cookie): cookie is string => Boolean(cookie));

 return cookiePairs.join('; ');
}

async function loginAndCollectCookies(request: APIRequestContext): Promise<string | null> {
 const loginResponse = await request.post(`${BASE_URL}/api/auth/login`, {
  data: {
   emailOrUsername: AUTH_TEST_USER,
   password: AUTH_TEST_PASSWORD,
  },
  maxRedirects: 0,
 });

 if (loginResponse.status() >= 500) {
  return null;
 }

 expect(loginResponse.status(), 'login response status').toBe(200);
 const cookieHeader = buildCookieHeaderFromSetCookie(loginResponse);
 expect(cookieHeader, 'access cookie must be present').toContain(`${AUTH_COOKIE.ACCESS}=`);
 expect(cookieHeader, 'refresh cookie must be present').toContain(`${AUTH_COOKIE.REFRESH}=`);
 expect(cookieHeader, 'refresh cookie must not be double-encoded').not.toContain('%252B');
 return cookieHeader;
}

test.describe('auth middleware hardening', () => {
 test('forged access/refresh cookies cannot render protected shell and are redirected to login via handshake', async ({ request }) => {
  const cookieHeader = buildForgedCookieHeader();
  const firstLoad = await requestPath('/vi/profile', request, cookieHeader);
  const secondLoad = await requestPath('/vi/profile', request, cookieHeader);
  const handshakeLocation = expectRedirectToHandshake(firstLoad);
  expectRedirectToHandshake(secondLoad);
  const handshakeUrl = handshakeLocation.startsWith('http')
   ? handshakeLocation
   : `${BASE_URL}${handshakeLocation}`;

  const handshakeRedirect = await request.get(handshakeUrl, {
   headers: { Cookie: cookieHeader },
   maxRedirects: 0,
  });

  const sessionResponse = await request.get(`${BASE_URL}/api/auth/session`, {
   headers: { Cookie: cookieHeader },
   maxRedirects: 0,
  });

  expectRedirectToLogin(handshakeRedirect);
  expect([401, 403]).toContain(sessionResponse.status());
 });

 test('anonymous requests are redirected to login on protected route', async ({ request }) => {
  const response = await requestPath('/vi/wallet', request);
  expectRedirectToLogin(response);
 });

 test('valid login keeps authenticated navigation and wallet API is not 401', async ({ request }) => {
  const cookieHeader = await loginAndCollectCookies(request);
  if (!cookieHeader) {
   test.skip(true, 'Auth upstream unavailable in current environment (login returned 5xx).');
   return;
  }

  const balanceResponse = await request.get(`${BASE_URL}/api/wallet/balance`, {
   headers: { Cookie: cookieHeader },
   maxRedirects: 0,
  });
  expect(balanceResponse.status(), 'wallet balance should not be unauthorized after login').not.toBe(401);

  const protectedRouteResponse = await requestPath('/vi/wallet', request, cookieHeader);
  expect(
   [301, 302, 303, 307, 308],
   'protected route should render for authenticated cookie, not redirect to login',
  ).not.toContain(protectedRouteResponse.status());
 });
});
