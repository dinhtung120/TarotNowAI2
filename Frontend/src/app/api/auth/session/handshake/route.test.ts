import { beforeEach, describe, expect, it, vi } from 'vitest';
import { NextRequest, NextResponse } from 'next/server';
import { GET } from '@/app/api/auth/session/handshake/route';
import { appendSetCookieHeaders, clearAuthCookies } from '@/app/api/auth/_shared';
import { getSessionRouteResponse } from '@/app/api/auth/session/sessionRouteHandler';

vi.mock('@/app/api/auth/_shared', () => ({
 appendSetCookieHeaders: vi.fn(),
 clearAuthCookies: vi.fn(),
}));

vi.mock('@/app/api/auth/session/sessionRouteHandler', () => ({
 SESSION_MODE: {
  FULL: 'full',
  LITE: 'lite',
 },
 getSessionRouteResponse: vi.fn(),
}));

vi.mock('@/i18n/routing', () => ({
 routing: {
  locales: ['vi', 'en', 'zh'],
  defaultLocale: 'vi',
 },
}));

const mockedAppendSetCookieHeaders = vi.mocked(appendSetCookieHeaders);
const mockedClearAuthCookies = vi.mocked(clearAuthCookies);
const mockedGetSessionRouteResponse = vi.mocked(getSessionRouteResponse);

function createHandshakeRequest(nextPath = '/vi/profile', cookieHeader?: string): NextRequest {
 const headers = new Headers({
  host: '0.0.0.0:3000',
  'x-forwarded-proto': 'https',
  'x-forwarded-host': 'www.tarotnow.xyz',
  'x-forwarded-port': '443',
  accept: 'text/html,application/xhtml+xml',
  'sec-fetch-dest': 'document',
  'sec-fetch-mode': 'navigate',
 });
 if (cookieHeader) {
  headers.set('cookie', cookieHeader);
 }

 return new NextRequest(`https://0.0.0.0:3000/api/auth/session/handshake?next=${encodeURIComponent(nextPath)}`, {
  headers,
 });
}

function createHandshakeRequestWithPort(
 forwardedPort: string,
 nextPath = '/vi/profile',
): NextRequest {
 const headers = new Headers({
  host: '0.0.0.0:3000',
  'x-forwarded-proto': 'https',
  'x-forwarded-host': 'www.tarotnow.xyz',
  'x-forwarded-port': forwardedPort,
  accept: 'text/html,application/xhtml+xml',
  'sec-fetch-dest': 'document',
  'sec-fetch-mode': 'navigate',
 });

 return new NextRequest(`https://0.0.0.0:3000/api/auth/session/handshake?next=${encodeURIComponent(nextPath)}`, {
  headers,
 });
}

describe('GET /api/auth/session/handshake', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('skips handshake for non-document navigation requests and redirects to locale login', async () => {
  const request = new NextRequest('https://0.0.0.0:3000/api/auth/session/handshake?next=%2Fvi%2Fprofile', {
   headers: {
    host: '0.0.0.0:3000',
    'x-forwarded-proto': 'https',
    'x-forwarded-host': 'www.tarotnow.xyz',
    'x-forwarded-port': '443',
    accept: 'text/x-component',
    rsc: '1',
    'next-router-prefetch': '1',
    purpose: 'prefetch',
   },
  });

  const response = await GET(request);

  expect(response.status).toBe(307);
  expect(response.headers.get('location')).toBe('https://www.tarotnow.xyz/vi/login');
  expect(mockedGetSessionRouteResponse).not.toHaveBeenCalled();
 });

 it('redirects failed handshakes back to the public login origin, not the internal app host', async () => {
  const failedResponse = NextResponse.json({ success: false }, { status: 401 });
  mockedGetSessionRouteResponse.mockResolvedValue(failedResponse);

  const response = await GET(createHandshakeRequest());

  expect(response.status).toBe(307);
  expect(response.headers.get('location')).toBe('https://www.tarotnow.xyz/vi/login');
  expect(mockedAppendSetCookieHeaders).toHaveBeenCalledWith(failedResponse.headers, response);
 });

 it('never appends :80 on public https redirects when proxy forwards port 80', async () => {
  const failedResponse = NextResponse.json({ success: false }, { status: 401 });
  mockedGetSessionRouteResponse.mockResolvedValue(failedResponse);

  const response = await GET(createHandshakeRequestWithPort('80'));

  expect(response.status).toBe(307);
  expect(response.headers.get('location')).toBe('https://www.tarotnow.xyz/vi/login');
  expect(mockedAppendSetCookieHeaders).toHaveBeenCalledWith(failedResponse.headers, response);
 });

 it('redirects successful handshakes back to the requested public path and forwards cookies', async () => {
  const sessionResponse = NextResponse.json({ success: true, authenticated: true }, { status: 200 });
  sessionResponse.headers.append('set-cookie', 'accessToken=rotated; Path=/; HttpOnly');
  mockedGetSessionRouteResponse.mockResolvedValue(sessionResponse);

  const response = await GET(createHandshakeRequest('/vi/wallet'));

  expect(response.status).toBe(307);
  expect(response.headers.get('location')).toBe('https://www.tarotnow.xyz/vi/wallet');
  expect(mockedAppendSetCookieHeaders).toHaveBeenCalledWith(sessionResponse.headers, response);
 });

 it('triggers loop guard on repeated handshakes for the same path within 5 seconds', async () => {
  const nowSpy = vi.spyOn(Date, 'now');
  nowSpy.mockReturnValue(1_000);

  const failedResponse = NextResponse.json({ success: false }, { status: 401 });
  mockedGetSessionRouteResponse.mockResolvedValue(failedResponse);

  const response = await GET(createHandshakeRequest('/vi/admin', '__tn_handshake_guard=%2Fvi%2Fadmin:999:1'));

  expect(response.status).toBe(307);
  expect(response.headers.get('location')).toBe('https://www.tarotnow.xyz/vi/login');
  expect(response.headers.get('x-handshake-loop-guard')).toBe('triggered');
  expect(mockedClearAuthCookies).toHaveBeenCalledWith(response, expect.any(NextRequest));
  expect(mockedGetSessionRouteResponse).not.toHaveBeenCalled();

  nowSpy.mockRestore();
 });
});
