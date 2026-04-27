import { beforeEach, describe, expect, it, vi } from 'vitest';
import { cookies } from 'next/headers';
import { AUTH_COOKIE } from '@/shared/infrastructure/auth/authConstants';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { getServerAccessTokenOrRefresh, getServerSessionSnapshot } from '@/shared/infrastructure/auth/serverAuth';

vi.mock('next/headers', () => ({
 cookies: vi.fn(),
}));

vi.mock('@/shared/infrastructure/http/serverHttpClient', () => ({
 serverHttpRequest: vi.fn(),
}));

const mockedCookies = vi.mocked(cookies);
const mockedServerHttpRequest = vi.mocked(serverHttpRequest);

function buildJwt(expInSeconds: number): string {
 const now = Math.floor(Date.now() / 1000);
 const header = Buffer.from(JSON.stringify({ alg: 'HS256', typ: 'JWT' })).toString('base64url');
 const payload = Buffer.from(JSON.stringify({ sub: 'user-1', exp: now + expInSeconds })).toString('base64url');
 return `${header}.${payload}.signature`;
}

describe('serverAuth', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('does not attempt hidden refresh when access token is missing', async () => {
  mockedCookies.mockResolvedValue({
   get: () => undefined,
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
});
