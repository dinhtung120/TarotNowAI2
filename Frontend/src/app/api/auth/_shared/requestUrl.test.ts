import { describe, expect, it } from 'vitest';
import { NextRequest } from 'next/server';
import { buildPublicRequestUrl } from '@/app/api/auth/_shared/requestUrl';

describe('buildPublicRequestUrl', () => {
 it('prefers forwarded proto and host over the internal request url', () => {
  const request = new NextRequest('https://0.0.0.0:3000/api/auth/session/handshake?next=%2Fvi%2Fprofile', {
   headers: {
    host: '0.0.0.0:3000',
    'x-forwarded-proto': 'https',
    'x-forwarded-host': 'www.tarotnow.xyz',
    'x-forwarded-port': '443',
   },
  });

  expect(buildPublicRequestUrl(request, '/vi/login').toString()).toBe('https://www.tarotnow.xyz/vi/login');
 });

 it('keeps non-default forwarded ports on the public redirect url', () => {
  const request = new NextRequest('http://0.0.0.0:3000/api/auth/session/handshake?next=%2Fvi%2Fprofile', {
   headers: {
    host: '0.0.0.0:3000',
    'x-forwarded-proto': 'https',
    'x-forwarded-host': 'staging.tarotnow.xyz',
    'x-forwarded-port': '8443',
   },
  });

  expect(buildPublicRequestUrl(request, '/vi/login').toString()).toBe('https://staging.tarotnow.xyz:8443/vi/login');
 });

 it('drops forwarded port 80 for https redirects', () => {
  const request = new NextRequest('http://0.0.0.0:3000/api/auth/session/handshake?next=%2Fvi%2Fprofile', {
   headers: {
    host: '0.0.0.0:3000',
    'x-forwarded-proto': 'https',
    'x-forwarded-host': 'www.tarotnow.xyz',
    'x-forwarded-port': '80',
   },
  });

  expect(buildPublicRequestUrl(request, '/vi/login').toString()).toBe('https://www.tarotnow.xyz/vi/login');
 });

 it('drops explicit :80 in forwarded host for https redirects', () => {
  const request = new NextRequest('http://0.0.0.0:3000/api/auth/session/handshake?next=%2Fvi%2Fprofile', {
   headers: {
    host: '0.0.0.0:3000',
    'x-forwarded-proto': 'https',
    'x-forwarded-host': 'www.tarotnow.xyz:80',
   },
  });

  expect(buildPublicRequestUrl(request, '/vi/login').toString()).toBe('https://www.tarotnow.xyz/vi/login');
 });

 it('ignores hostile forwarded hosts when building public URLs', () => {
  const request = new NextRequest('https://www.tarotnow.xyz/api/auth/session/handshake', {
   headers: {
    'x-forwarded-host': 'evil.tld',
    'x-forwarded-proto': 'https',
   },
  });

  expect(buildPublicRequestUrl(request, '/vi/login').toString()).toBe('https://www.tarotnow.xyz/vi/login');
 });
});
