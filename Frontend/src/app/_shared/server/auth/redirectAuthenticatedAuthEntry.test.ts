// @vitest-environment node

import { beforeEach, describe, expect, it, vi } from 'vitest';
import { cookies } from 'next/headers';
import { redirect } from 'next/navigation';
import { AUTH_COOKIE } from '@/shared/auth/authConstants';
import { getServerSessionSnapshot } from '@/shared/auth/serverAuth';
import {
 PROTECTED_ROUTE_AUTH_DECISION,
 resolveProtectedRouteAuthDecision,
} from '@/shared/server/auth/protectedRouteAuthDecision';
import { redirectAuthenticatedAuthEntry } from '@/app/_shared/server/auth/redirectAuthenticatedAuthEntry';

vi.mock('next/headers', () => ({
 cookies: vi.fn(),
}));

vi.mock('next/navigation', () => ({
 redirect: vi.fn(),
}));

vi.mock('@/shared/auth/serverAuth', () => ({
 getServerSessionSnapshot: vi.fn(),
}));

vi.mock('@/shared/server/auth/protectedRouteAuthDecision', async () => {
 const actual = await vi.importActual<typeof import('@/shared/server/auth/protectedRouteAuthDecision')>(
  '@/shared/server/auth/protectedRouteAuthDecision',
 );

 return {
  ...actual,
  resolveProtectedRouteAuthDecision: vi.fn(),
 };
});

const mockedCookies = vi.mocked(cookies);
const mockedRedirect = vi.mocked(redirect);
const mockedGetServerSessionSnapshot = vi.mocked(getServerSessionSnapshot);
const mockedResolveProtectedRouteAuthDecision = vi.mocked(resolveProtectedRouteAuthDecision);

function createCookieStore({
 accessToken,
 refreshToken,
}: {
 accessToken?: string;
 refreshToken?: string;
}) {
 return {
  get: (name: string) => {
   if (name === AUTH_COOKIE.ACCESS && accessToken) {
    return { value: accessToken };
   }

   if (name === AUTH_COOKIE.REFRESH && refreshToken) {
    return { value: refreshToken };
   }

   return undefined;
  },
 } as never;
}

describe('redirectAuthenticatedAuthEntry', () => {
 beforeEach(() => {
  vi.clearAllMocks();
 });

 it('allows anonymous users to stay on auth entry pages', async () => {
  mockedCookies.mockResolvedValue(createCookieStore({}));
  mockedResolveProtectedRouteAuthDecision.mockResolvedValue({
   decision: PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_LOGIN,
   redirectPath: '/vi/login',
   reason: 'missing_session_cookies',
  });

  await redirectAuthenticatedAuthEntry({ locale: 'vi' });

  expect(mockedRedirect).not.toHaveBeenCalled();
  expect(mockedGetServerSessionSnapshot).not.toHaveBeenCalled();
 });

 it('keeps refresh-only sessions on auth entry pages without handshake redirects', async () => {
  mockedCookies.mockResolvedValue(createCookieStore({ refreshToken: 'refresh-token' }));
  mockedResolveProtectedRouteAuthDecision.mockResolvedValue({
   decision: PROTECTED_ROUTE_AUTH_DECISION.REDIRECT_LOGIN,
   redirectPath: '/vi/login',
   reason: 'missing_session_cookies',
  });

  await redirectAuthenticatedAuthEntry({ locale: 'vi' });

  expect(mockedRedirect).not.toHaveBeenCalled();
  expect(mockedGetServerSessionSnapshot).not.toHaveBeenCalled();
 });

  it('redirects authenticated sessions away from auth entry pages', async () => {
  mockedCookies.mockResolvedValue(createCookieStore({ accessToken: 'access-token' }));
  mockedResolveProtectedRouteAuthDecision.mockResolvedValue({
   decision: PROTECTED_ROUTE_AUTH_DECISION.ALLOW,
   redirectPath: null,
   reason: 'access_token_valid',
  });
  mockedGetServerSessionSnapshot.mockResolvedValue({
   authenticated: true,
   user: {
    id: 'user-1',
    email: 'reader@example.com',
    username: 'reader',
    displayName: 'Reader',
    avatarUrl: null,
    level: 1,
    exp: 0,
    role: 'user',
    status: 'Active',
   },
  });

  await redirectAuthenticatedAuthEntry({ locale: 'vi', fallbackPath: '/vi/profile' });

  expect(mockedGetServerSessionSnapshot).toHaveBeenCalledWith({ allowRefresh: false });
  expect(mockedRedirect).toHaveBeenCalledWith('/vi/profile');
 });

 it('keeps user on auth entry page when session lookup is unauthenticated', async () => {
  mockedCookies.mockResolvedValue(createCookieStore({
   accessToken: 'access-token',
   refreshToken: 'refresh-token',
  }));
  mockedResolveProtectedRouteAuthDecision.mockResolvedValue({
   decision: PROTECTED_ROUTE_AUTH_DECISION.ALLOW,
   redirectPath: null,
   reason: 'access_token_valid',
  });
  mockedGetServerSessionSnapshot.mockResolvedValue({
   authenticated: false,
   user: null,
  });

  await redirectAuthenticatedAuthEntry({ locale: 'en' });

  expect(mockedRedirect).not.toHaveBeenCalled();
 });
});
