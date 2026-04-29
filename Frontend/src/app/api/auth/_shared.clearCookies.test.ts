import { afterEach, describe, expect, it, vi } from 'vitest';
import { NextRequest, NextResponse } from 'next/server';

const ORIGINAL_AUTH_COOKIE_DOMAIN = process.env.AUTH_COOKIE_DOMAIN;
const ORIGINAL_NODE_ENV = process.env.NODE_ENV;

function restoreEnv(): void {
 if (ORIGINAL_AUTH_COOKIE_DOMAIN === undefined) {
  delete process.env.AUTH_COOKIE_DOMAIN;
 } else {
  process.env.AUTH_COOKIE_DOMAIN = ORIGINAL_AUTH_COOKIE_DOMAIN;
 }

 if (ORIGINAL_NODE_ENV === undefined) {
  delete process.env.NODE_ENV;
 } else {
  process.env.NODE_ENV = ORIGINAL_NODE_ENV;
 }
}

afterEach(() => {
 vi.resetModules();
 restoreEnv();
});

describe('clearAuthCookies', () => {
 it('clears host-only + configured domain + legacy apex domain variants', async () => {
  process.env.NODE_ENV = 'production';
  process.env.AUTH_COOKIE_DOMAIN = 'www.tarotnow.xyz';
  vi.resetModules();

  const request = new NextRequest('https://www.tarotnow.xyz/api/auth/session', {
   headers: {
    host: 'www.tarotnow.xyz',
    'x-forwarded-proto': 'https',
    'x-forwarded-host': 'www.tarotnow.xyz',
   },
  });
  const response = NextResponse.json({ ok: true });

  const { clearAuthCookies } = await import('@/app/api/auth/_shared');
  clearAuthCookies(response, request);

  const cookieHeaders = typeof response.headers.getSetCookie === 'function'
   ? response.headers.getSetCookie()
   : (response.headers.get('set-cookie')?.split(/,\s*(?=[a-zA-Z_]+=)/) ?? []);
  const joinedHeaders = cookieHeaders.join('\n');

  expect(joinedHeaders).toContain('accessToken=');
  expect(joinedHeaders).toContain('refreshToken=');
  expect(joinedHeaders).toContain('Domain=www.tarotnow.xyz');
  expect(joinedHeaders).toContain('Domain=tarotnow.xyz');
  expect(joinedHeaders).toContain('Max-Age=0');
 });
});
