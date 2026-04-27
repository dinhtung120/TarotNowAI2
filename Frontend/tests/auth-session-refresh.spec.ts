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
});
