// @vitest-environment node

import { afterEach, beforeEach, describe, expect, it } from 'vitest';
import { createHmac } from 'node:crypto';
import { verifyAccessToken } from '@/shared/server/auth/accessTokenVerifier';
import {
 PROTECTED_ROUTE_AUTH_DECISION,
 resolveProtectedRouteAuthDecision,
} from '@/shared/server/auth/protectedRouteAuthDecision';

const TEST_JWT_SECRET = 'TarotNow_Frontend_Test_Secret_At_Least_32_Characters';
const FORGED_JWT_SECRET = 'Forged_Token_Secret_Key_For_Attack_Simulation_123';
const TEST_JWT_ISSUER = 'TarotNowAI';
const TEST_JWT_AUDIENCE = 'TarotNowAIUsers';

function createHs256Jwt(expSeconds: number, secret: string): string {
 const header = Buffer.from(
  JSON.stringify({ alg: 'HS256', typ: 'JWT' }),
 ).toString('base64url');
 const payload = Buffer.from(
  JSON.stringify({
   sub: 'audit-user',
   role: 'user',
   iss: TEST_JWT_ISSUER,
   aud: TEST_JWT_AUDIENCE,
   exp: expSeconds,
  }),
 ).toString('base64url');
 const signature = createHmac('sha256', secret)
  .update(`${header}.${payload}`)
  .digest('base64url');

 return `${header}.${payload}.${signature}`;
}

function createSignedJwt(expSeconds: number): string {
 return createHs256Jwt(expSeconds, TEST_JWT_SECRET);
}

function createForgedSignedLikeJwt(expSeconds: number): string {
 return createHs256Jwt(expSeconds, FORGED_JWT_SECRET);
}

function createUnsignedJwt(expSeconds: number): string {
 const header = Buffer.from(JSON.stringify({ alg: 'none', typ: 'JWT' })).toString('base64url');
 const payload = Buffer.from(JSON.stringify({ sub: 'audit-user', exp: expSeconds })).toString('base64url');
 return `${header}.${payload}.`;
}

describe('resolveProtectedRouteAuthDecision', () => {
 const originalNodeEnv = process.env.NODE_ENV;

 beforeEach(() => {
  process.env.JWT_SECRETKEY = TEST_JWT_SECRET;
  process.env.JWT_ISSUER = TEST_JWT_ISSUER;
  process.env.JWT_AUDIENCE = TEST_JWT_AUDIENCE;
  process.env.NODE_ENV = 'test';
  delete process.env.AUTH_VERIFIER_POLICY;
 });

 afterEach(() => {
  process.env.NODE_ENV = originalNodeEnv;
  delete process.env.AUTH_VERIFIER_POLICY;
 });

 it('allows protected route when access token is cryptographically valid', async () => {
  const accessToken = createSignedJwt(Math.floor(Date.now() / 1000) + 3600);
  const verification = await verifyAccessToken(accessToken);
  expect(verification).toEqual(expect.objectContaining({ valid: true, reason: 'verified' }));

  const decision = await resolveProtectedRouteAuthDecision({
   accessToken,
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

 it('redirects to handshake when access token is invalid but refresh cookie exists', async () => {
  const decision = await resolveProtectedRouteAuthDecision({
   accessToken: createUnsignedJwt(Math.floor(Date.now() / 1000) + 3600),
   refreshToken: 'forged-refresh-token',
   locale: 'vi',
   nextPath: '/vi/profile',
  });

  expect(decision.decision).toBe(PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_HANDSHAKE);
  expect(decision.redirectPath).toContain('/api/auth/session/handshake?next=');
  expect(decision.reason).toBe('access_token_invalid_refresh_present');
 });

 it('redirects to handshake when access token is missing but refresh cookie exists', async () => {
  const decision = await resolveProtectedRouteAuthDecision({
   accessToken: undefined,
   refreshToken: 'refresh-token-only',
   locale: 'vi',
   nextPath: '/vi/profile',
  });

  expect(decision.decision).toBe(PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_HANDSHAKE);
  expect(decision.redirectPath).toContain('/api/auth/session/handshake?next=');
  expect(decision.reason).toBe('access_token_invalid_refresh_present');
 });

 it('redirects to handshake when signed-like token has invalid signature and refresh cookie exists', async () => {
  const decision = await resolveProtectedRouteAuthDecision({
   accessToken: createForgedSignedLikeJwt(Math.floor(Date.now() / 1000) + 3600),
   refreshToken: 'forged-refresh-token',
   locale: 'vi',
   nextPath: '/vi/profile',
  });

  expect(decision.decision).toBe(PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_HANDSHAKE);
  expect(decision.redirectPath).toContain('/api/auth/session/handshake?next=');
  expect(decision.reason).toBe('access_token_invalid_refresh_present');
 });

 it('allows protected route when verifier config is missing in non-production policy', async () => {
  const signedLikeToken = createForgedSignedLikeJwt(Math.floor(Date.now() / 1000) + 3600);
  delete process.env.JWT_SECRETKEY;

  const decision = await resolveProtectedRouteAuthDecision({
   accessToken: signedLikeToken,
   refreshToken: 'forged-refresh-token',
   locale: 'en',
   nextPath: '/en/wallet',
  });

  expect(decision).toEqual({
   decision: PROTECTED_ROUTE_AUTH_DECISION.ALLOW,
   redirectPath: null,
   reason: 'access_token_unverified_missing_verifier_config',
  });
 });

 it('fails closed in production when verifier config is missing and refresh token exists', async () => {
  process.env.NODE_ENV = 'production';
  const signedLikeToken = createForgedSignedLikeJwt(Math.floor(Date.now() / 1000) + 3600);
  delete process.env.JWT_SECRETKEY;

  const decision = await resolveProtectedRouteAuthDecision({
   accessToken: signedLikeToken,
   refreshToken: 'refresh-token',
   locale: 'en',
   nextPath: '/en/wallet',
  });

  expect(decision).toEqual({
   decision: PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_HANDSHAKE,
   redirectPath: '/api/auth/session/handshake?next=%2Fen%2Fwallet',
   reason: 'access_token_invalid_missing_verifier_config',
  });
 });

 it('redirects to login when both access and refresh cookies are missing', async () => {
  const decision = await resolveProtectedRouteAuthDecision({
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

 it('redirects to login when access token is invalid and refresh cookie is missing', async () => {
  const decision = await resolveProtectedRouteAuthDecision({
   accessToken: createUnsignedJwt(Math.floor(Date.now() / 1000) + 3600),
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
