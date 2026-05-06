// @vitest-environment node

import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { createHmac } from 'node:crypto';
import { cookies } from 'next/headers';
import { AUTH_COOKIE } from '@/shared/auth/authConstants';
import { serverHttpRequest } from '@/shared/http/serverHttpClient';
import {
 getServerAccessToken,
 getServerAccessTokenOrRefresh,
 getServerSessionSnapshot,
} from '@/shared/auth/serverAuth';

vi.mock('next/headers', () => ({
 cookies: vi.fn(),
}));

vi.mock('@/shared/http/serverHttpClient', () => ({
 serverHttpRequest: vi.fn(),
}));

const TEST_JWT_SECRET = 'TarotNow_Frontend_Test_Secret_At_Least_32_Characters';
const FORGED_JWT_SECRET = 'Forged_Token_Secret_Key_For_Attack_Simulation_123';
const TEST_JWT_ISSUER = 'TarotNowAI';
const TEST_JWT_AUDIENCE = 'TarotNowAIUsers';

const mockedCookies = vi.mocked(cookies);
const mockedServerHttpRequest = vi.mocked(serverHttpRequest);

function buildHs256Jwt(
 expInSeconds: number,
 secret: string,
 overrides: Record<string, unknown> = {},
): string {
 const now = Math.floor(Date.now() / 1000);
 const header = Buffer.from(
  JSON.stringify({ alg: 'HS256', typ: 'JWT' }),
 ).toString('base64url');
 const payload = Buffer.from(
  JSON.stringify({
   sub: 'user-1',
   role: 'user',
   iss: TEST_JWT_ISSUER,
   aud: TEST_JWT_AUDIENCE,
   exp: now + expInSeconds,
   ...overrides,
  }),
 ).toString('base64url');
 const signature = createHmac('sha256', secret)
  .update(`${header}.${payload}`)
  .digest('base64url');

 return `${header}.${payload}.${signature}`;
}

function buildJwt(expInSeconds: number): string {
 return buildHs256Jwt(expInSeconds, TEST_JWT_SECRET);
}

function buildForgedSignedLikeJwt(expInSeconds: number): string {
 return buildHs256Jwt(expInSeconds, FORGED_JWT_SECRET);
}

function buildUnsignedJwt(expInSeconds: number): string {
 const now = Math.floor(Date.now() / 1000);
 const header = Buffer.from(JSON.stringify({ alg: 'none', typ: 'JWT' })).toString('base64url');
 const payload = Buffer.from(JSON.stringify({ sub: 'user-1', exp: now + expInSeconds })).toString('base64url');
 return `${header}.${payload}.`;
}

describe('serverAuth', () => {
 const originalNodeEnv = process.env.NODE_ENV;

 beforeEach(() => {
  vi.clearAllMocks();
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

 it('does not attempt hidden refresh when access token is missing', async () => {
  mockedCookies.mockResolvedValue({
   get: () => undefined,
  } as never);

  const result = await getServerAccessTokenOrRefresh();

  expect(result).toBeUndefined();
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
 });

 it('returns undefined when token uses alg=none', async () => {
  const forgedToken = buildUnsignedJwt(600);
  mockedCookies.mockResolvedValue({
   get: (name: string) => (name === AUTH_COOKIE.ACCESS ? { value: forgedToken } : undefined),
  } as never);

  const result = await getServerAccessTokenOrRefresh();

 expect(result).toBeUndefined();
 expect(mockedServerHttpRequest).not.toHaveBeenCalled();
});

 it('returns undefined when token is signed-like but signature is invalid', async () => {
  const forgedToken = buildForgedSignedLikeJwt(600);
  mockedCookies.mockResolvedValue({
   get: (name: string) => (name === AUTH_COOKIE.ACCESS ? { value: forgedToken } : undefined),
  } as never);

  const result = await getServerAccessTokenOrRefresh();

  expect(result).toBeUndefined();
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
 });

 it('returns undefined in production when verifier config is missing', async () => {
  process.env.NODE_ENV = 'production';
  delete process.env.JWT_SECRETKEY;
  const signedLikeToken = buildForgedSignedLikeJwt(600);
  mockedCookies.mockResolvedValue({
   get: (name: string) => (name === AUTH_COOKIE.ACCESS ? { value: signedLikeToken } : undefined),
  } as never);

  const result = await getServerAccessTokenOrRefresh();

  expect(result).toBeUndefined();
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
 });

 it('returns undefined when token is expiring soon', async () => {
  const expiringToken = buildJwt(5);
  mockedCookies.mockResolvedValue({
   get: (name: string) => (name === AUTH_COOKIE.ACCESS ? { value: expiringToken } : undefined),
  } as never);

  const result = await getServerAccessTokenOrRefresh();

  expect(result).toBeUndefined();
  expect(mockedServerHttpRequest).not.toHaveBeenCalled();
 });

 it('returns valid access token when token is cryptographically valid and not expiring', async () => {
  const validToken = buildJwt(600);
  mockedCookies.mockResolvedValue({
   get: (name: string) => (name === AUTH_COOKIE.ACCESS ? { value: validToken } : undefined),
  } as never);

  const result = await getServerAccessToken();

  expect(result).toBe(validToken);
 });

 it('returns authenticated session when token valid and profile resolves', async () => {
  const validToken = buildJwt(600);
  mockedCookies.mockResolvedValue({
   get: (name: string) => (name === AUTH_COOKIE.ACCESS ? { value: validToken } : undefined),
  } as never);
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: {
    id: 'user-1',
    email: 'user@example.com',
    username: 'user1',
    displayName: 'User One',
    role: 'user',
    status: 'active',
   },
  } as never);

  const session = await getServerSessionSnapshot();

  expect(session.authenticated).toBe(true);
  expect(session.user?.id).toBe('user-1');
  expect(mockedServerHttpRequest).toHaveBeenCalledWith('/profile', expect.objectContaining({
   method: 'GET',
   token: validToken,
  }));
 });

 it('returns unauthenticated snapshot when profile API returns error', async () => {
  const validToken = buildJwt(600);
  mockedCookies.mockResolvedValue({
   get: (name: string) => (name === AUTH_COOKIE.ACCESS ? { value: validToken } : undefined),
  } as never);
  mockedServerHttpRequest.mockResolvedValue({
   ok: false,
   status: 401,
   headers: new Headers(),
   error: 'Unauthorized',
  } as never);

  const session = await getServerSessionSnapshot();

  expect(session).toEqual({
   authenticated: false,
   user: null,
  });
 });

 it('normalizes profile by falling back to JWT claims when profile fields are missing', async () => {
  const subjectClaimKey = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
  const roleClaimKey = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
  const validToken = buildHs256Jwt(600, TEST_JWT_SECRET, {
   role: '',
   sub: null,
   [subjectClaimKey]: 'subject-from-claim',
   [roleClaimKey]: ['  ', 'admin'],
  });
  mockedCookies.mockResolvedValue({
   get: (name: string) => (name === AUTH_COOKIE.ACCESS ? { value: validToken } : undefined),
  } as never);
  mockedServerHttpRequest.mockResolvedValue({
   ok: true,
   status: 200,
   headers: new Headers(),
   data: {
    id: '',
    email: 'reader@example.com',
    username: 'reader_user',
    displayName: '',
    role: '',
    status: 'suspended',
    avatarUrl: null,
    level: '12',
    exp: '240',
   },
  } as never);

  const session = await getServerSessionSnapshot();

  expect(session.authenticated).toBe(true);
  expect(session.user).toEqual(expect.objectContaining({
   id: 'subject-from-claim',
   role: 'admin',
   displayName: 'reader_user',
   status: 'Suspended',
   level: 12,
   exp: 240,
   avatarUrl: null,
  }));
 });
});
