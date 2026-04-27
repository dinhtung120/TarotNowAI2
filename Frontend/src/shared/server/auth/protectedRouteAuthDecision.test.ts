import { describe, expect, it } from 'vitest';
import {
 PROTECTED_ROUTE_AUTH_DECISION,
 resolveProtectedRouteAuthDecision,
} from '@/shared/server/auth/protectedRouteAuthDecision';

function createUnsignedJwt(expSeconds: number): string {
 const header = Buffer.from(JSON.stringify({ alg: 'none', typ: 'JWT' })).toString('base64url');
 const payload = Buffer.from(JSON.stringify({ sub: 'audit-user', exp: expSeconds })).toString('base64url');
 return `${header}.${payload}.`;
}

function createSignedLikeJwt(expSeconds: number): string {
 const header = Buffer.from(JSON.stringify({ alg: 'HS256', typ: 'JWT' })).toString('base64url');
 const payload = Buffer.from(JSON.stringify({ sub: 'audit-user', exp: expSeconds })).toString('base64url');
 return `${header}.${payload}.signature`;
}

describe('resolveProtectedRouteAuthDecision', () => {
 it('allows protected route when access token looks valid and not expiring', () => {
  const decision = resolveProtectedRouteAuthDecision({
   accessToken: createSignedLikeJwt(Math.floor(Date.now() / 1000) + 3600),
   refreshToken: undefined,
   locale: 'vi',
   nextPath: '/vi/profile',
  });

  expect(decision).toEqual({
   decision: PROTECTED_ROUTE_AUTH_DECISION.ALLOW,
   redirectPath: null,
   reason: 'access_token_valid',
  });
 });

 it('redirects to handshake when access token is invalid but refresh cookie exists', () => {
  const decision = resolveProtectedRouteAuthDecision({
   accessToken: createUnsignedJwt(Math.floor(Date.now() / 1000) + 3600),
   refreshToken: 'forged-refresh-token',
   locale: 'vi',
   nextPath: '/vi/profile',
  });

  expect(decision.decision).toBe(PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_HANDSHAKE);
  expect(decision.redirectPath).toContain('/api/auth/session/handshake?next=');
  expect(decision.reason).toBe('access_token_invalid_refresh_present');
 });

 it('redirects to login when both access and refresh cookies are missing', () => {
  const decision = resolveProtectedRouteAuthDecision({
   accessToken: undefined,
   refreshToken: undefined,
   locale: 'en',
   nextPath: '/en/wallet',
  });

  expect(decision).toEqual({
   decision: PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_LOGIN,
   redirectPath: '/en/login',
   reason: 'missing_session_cookies',
  });
 });
});
