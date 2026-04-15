import { cookies, headers } from 'next/headers';
import type { AuthResponse, UserProfile } from '@/features/auth/domain/types';
import { AUTH_COOKIE, AUTH_HEADER, AUTH_SESSION } from '@/shared/infrastructure/auth/authConstants';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

function parseJwtExp(token: string | undefined): number | undefined {
 if (!token) {
  return undefined;
 }

 const parts = token.split('.');
 if (parts.length < 2 || !parts[1]) {
  return undefined;
 }

 try {
  const payload = JSON.parse(Buffer.from(parts[1], 'base64url').toString('utf8')) as { exp?: number };
  return typeof payload.exp === 'number' ? payload.exp : undefined;
 } catch {
  return undefined;
 }
}

function isExpiringSoon(token: string | undefined): boolean {
 const exp = parseJwtExp(token);
 if (!exp) {
  return true;
 }

 const now = Math.floor(Date.now() / 1000);
 return exp <= now + Math.max(0, AUTH_SESSION.ACCESS_REFRESH_THRESHOLD_SECONDS);
}

async function getValidAccessTokenFromCookie(): Promise<string | undefined> {
 const cookieStore = await cookies();
 const accessToken = cookieStore.get(AUTH_COOKIE.ACCESS)?.value;
 if (!accessToken || isExpiringSoon(accessToken)) {
  return undefined;
 }

 return accessToken;
}

function normalizeHeaderValue(value: string | null | undefined): string {
 if (!value) {
  return '';
 }

 const trimmed = value.trim();
 return trimmed.length > 0 ? trimmed : '';
}

async function refreshServerAccessToken(
 refreshToken: string,
 deviceId: string,
 userAgent: string,
): Promise<string | undefined> {
 const result = await serverHttpRequest<AuthResponse>('/auth/refresh', {
  method: 'POST',
  headers: {
   Cookie: `${AUTH_COOKIE.REFRESH}=${refreshToken}`,
   [AUTH_HEADER.IDEMPOTENCY_KEY]: crypto.randomUUID(),
   [AUTH_HEADER.DEVICE_ID]: deviceId,
   [AUTH_HEADER.FORWARDED_USER_AGENT]: userAgent,
  },
  cache: 'no-store',
  fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
 });
 if (!result.ok) {
  return undefined;
 }

 const nextAccessToken = result.data.accessToken;
 if (!nextAccessToken || isExpiringSoon(nextAccessToken)) {
  return undefined;
 }

 return nextAccessToken;
}

interface ServerAuthTokenOptions {
 allowRefresh?: boolean;
}

export async function getServerAccessTokenOrRefresh(
 options: ServerAuthTokenOptions = {},
): Promise<string | undefined> {
 const accessToken = await getValidAccessTokenFromCookie();
 if (accessToken) {
  return accessToken;
 }

 if (options.allowRefresh === false) {
  return undefined;
 }

 const cookieStore = await cookies();
 const refreshToken = cookieStore.get(AUTH_COOKIE.REFRESH)?.value;
 if (!refreshToken) {
  return undefined;
 }

 const headerStore = await headers();
 const forwardedDeviceId = normalizeHeaderValue(headerStore.get(AUTH_HEADER.DEVICE_ID));
 const cookieDeviceId = normalizeHeaderValue(cookieStore.get(AUTH_COOKIE.DEVICE)?.value);
 const resolvedDeviceId = cookieDeviceId || forwardedDeviceId || 'server-auth';
 const userAgent = normalizeHeaderValue(headerStore.get('user-agent'));
 return refreshServerAccessToken(refreshToken, resolvedDeviceId, userAgent);
}

export async function getServerAccessToken(): Promise<string | undefined> {
 return getServerAccessTokenOrRefresh();
}

export interface ServerSessionSnapshot {
 authenticated: boolean;
 user: UserProfile | null;
}

interface ServerSessionOptions {
 allowRefresh?: boolean;
}

export async function getServerSessionSnapshot(
 options: ServerSessionOptions = {},
): Promise<ServerSessionSnapshot> {
 const accessToken = await getServerAccessTokenOrRefresh(options);
 if (!accessToken) {
  return { authenticated: false, user: null };
 }

 const profile = await serverHttpRequest<UserProfile>('/profile', {
  method: 'GET',
  token: accessToken,
  cache: 'no-store',
  fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
 });
 if (!profile.ok) {
  return { authenticated: false, user: null };
 }

 return { authenticated: true, user: profile.data };
}
