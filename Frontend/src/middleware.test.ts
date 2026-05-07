import { afterEach, describe, expect, it, vi } from 'vitest';
import { NextRequest, NextResponse } from 'next/server';
import proxy from './proxy';

vi.mock('@/shared/http/apiUrl', () => ({
 getPublicApiOrigin: () => 'https://api.tarotnow.test',
}));

vi.mock('next-intl/middleware', () => ({
 default: () => (request: NextRequest) => {
  if (request.nextUrl.pathname === '/') {
   return NextResponse.redirect(new URL('/vi', request.url));
  }

  return NextResponse.next();
 },
}));

vi.mock('@/i18n/routing', () => ({
 routing: {
  locales: ['vi', 'en', 'zh'],
  defaultLocale: 'vi',
 },
}));

const originalNodeEnv = process.env.NODE_ENV;

afterEach(() => {
 process.env.NODE_ENV = originalNodeEnv;
 vi.resetModules();
});

function createRequest(url: string, cookieHeader?: string): NextRequest {
 const headers = new Headers({ accept: 'text/html' });
 if (cookieHeader) {
  headers.set('cookie', cookieHeader);
 }

 return new NextRequest(url, { headers });
}

describe('auth middleware', () => {
 it('redirects locale-less root path to default locale', async () => {
  const response = await proxy(createRequest('https://www.tarotnow.xyz/'));
  expect(response.status).toBe(307);
  expect(response.headers.get('location')).toBe('https://www.tarotnow.xyz/vi');
 });

 it('redirects protected route to login when no auth cookies exist', async () => {
  const response = await proxy(createRequest('https://www.tarotnow.xyz/vi/wallet'));
  expect(response.status).toBe(307);
  expect(response.headers.get('location')).toBe('https://www.tarotnow.xyz/vi/login');
 });

 it('allows protected document requests when a refresh cookie exists', async () => {
  const response = await proxy(
   createRequest('https://www.tarotnow.xyz/vi/wallet', 'refreshToken=refresh-value'),
  );

  expect(response.status).toBe(200);
 });


 it('skips api routes', async () => {
  const response = await proxy(createRequest('https://www.tarotnow.xyz/vi/api/user-context/metadata'));
  expect(response.status).toBe(200);
 });

 it('clears auth cookies without Secure on http requests', async () => {
  const response = await proxy(createRequest('http://localhost:3000/vi/wallet'));
  const setCookie = response.headers.get('set-cookie') ?? '';

  expect(setCookie).toContain('accessToken=');
  expect(setCookie).toContain('refreshToken=');
  expect(setCookie).not.toContain('Secure');
 });

 it('omits http image source from production CSP', async () => {
  process.env.NODE_ENV = 'production';
  vi.resetModules();
  const { default: productionProxy } = await import('./proxy');

  const response = await productionProxy(createRequest('https://www.tarotnow.xyz/vi'));
  const csp = response.headers.get('Content-Security-Policy') ?? '';

  expect(csp).toContain("img-src 'self' data: blob: https:");
  expect(csp).not.toContain("img-src 'self' data: blob: https: http:");
 });
});
