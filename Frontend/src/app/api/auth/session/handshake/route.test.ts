import { beforeEach, describe, expect, it, vi } from 'vitest';
import { NextRequest, NextResponse } from 'next/server';
import { GET } from '@/app/api/auth/session/handshake/route';
import { appendSetCookieHeaders } from '@/app/api/auth/_shared';
import { getSessionRouteResponse } from '@/app/api/auth/session/sessionRouteHandler';

vi.mock('@/app/api/auth/_shared', () => ({
 appendSetCookieHeaders: vi.fn(),
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
const mockedGetSessionRouteResponse = vi.mocked(getSessionRouteResponse);

function createHandshakeRequest(nextPath = '/vi/profile'): NextRequest {
 return new NextRequest(`https://0.0.0.0:3000/api/auth/session/handshake?next=${encodeURIComponent(nextPath)}`, {
  headers: {
   host: '0.0.0.0:3000',
   'x-forwarded-proto': 'https',
   'x-forwarded-host': 'www.tarotnow.xyz',
   'x-forwarded-port': '443',
  },
 });
}

describe('GET /api/auth/session/handshake', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('redirects failed handshakes back to the public login origin, not the internal app host', async () => {
  mockedGetSessionRouteResponse.mockResolvedValue(
   NextResponse.json({ success: false }, { status: 401 }),
  );

  const response = await GET(createHandshakeRequest(),);

  expect(response.status).toBe(307);
  expect(response.headers.get('location')).toBe('https://www.tarotnow.xyz/vi/login');
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
});
