import { describe, expect, it } from 'vitest';
import { NextRequest } from 'next/server';
import { proxy } from '../proxy';

function createRequest(url: string, cookieHeader?: string): NextRequest {
 const headers = new Headers();
 if (cookieHeader) {
  headers.set('cookie', cookieHeader);
 }

 return new NextRequest(url, { headers });
}

describe('auth middleware', () => {
 it('redirects protected route to login when no auth cookies exist', () => {
  const response = proxy(createRequest('https://www.tarotnow.xyz/vi/wallet'));
  expect(response.status).toBe(307);
  expect(response.headers.get('location')).toBe('https://www.tarotnow.xyz/vi/login');
 });

 it('redirects protected route to handshake when refresh cookie exists but access cookie is missing', () => {
  const response = proxy(
   createRequest('https://www.tarotnow.xyz/vi/wallet', 'refreshToken=refresh-value'),
  );

  expect(response.status).toBe(307);
  expect(response.headers.get('location')).toBe(
   'https://www.tarotnow.xyz/api/auth/session/handshake?next=%2Fvi%2Fwallet',
  );
 });

 it('redirects authenticated users away from auth entry routes', () => {
  const response = proxy(
   createRequest('https://www.tarotnow.xyz/vi/login', 'accessToken=access-value'),
  );

  expect(response.status).toBe(307);
  expect(response.headers.get('location')).toBe('https://www.tarotnow.xyz/vi');
 });

 it('skips api routes', () => {
  const response = proxy(createRequest('https://www.tarotnow.xyz/vi/api/user-context/metadata'));
  expect(response.status).toBe(200);
 });
});
