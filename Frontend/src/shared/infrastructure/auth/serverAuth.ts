import { cookies, headers } from 'next/headers';
import type { AuthResponse, UserProfile } from '@/features/auth/domain/types';
import { AUTH_COOKIE, AUTH_HEADER, AUTH_SESSION } from '@/shared/infrastructure/auth/authConstants';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

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
 return exp - now <= AUTH_SESSION.ACCESS_REFRESH_THRESHOLD_SECONDS;
}

async function refreshServerAccessToken(): Promise<string | undefined> {
 const cookieStore = await cookies();
 const requestHeaders = await headers();
 const refreshToken = cookieStore.get(AUTH_COOKIE.REFRESH)?.value;
 if (!refreshToken) {
  return undefined;
 }

 const deviceId = cookieStore.get(AUTH_COOKIE.DEVICE)?.value ?? '';
 const deviceFingerprint = deviceId.length > 16 ? deviceId.slice(-16) : (deviceId || 'anonymous');
 const timeBucket = Math.floor(Date.now() / 30_000);
 const idempotencyKey = `srv-refresh:${deviceFingerprint}:${timeBucket}`;

 const result = await serverHttpRequest<AuthResponse>('/auth/refresh', {
  method: 'POST',
  headers: {
   Cookie: `${AUTH_COOKIE.REFRESH}=${refreshToken}`,
   [AUTH_HEADER.IDEMPOTENCY_KEY]: idempotencyKey,
   [AUTH_HEADER.DEVICE_ID]: deviceId,
   [AUTH_HEADER.FORWARDED_USER_AGENT]: requestHeaders.get('user-agent') ?? '',
  },
  cache: 'no-store',
  fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
 });

 if (!result.ok || !result.data.accessToken) {
  return undefined;
 }

 return result.data.accessToken;
}

export async function getServerAccessToken(): Promise<string | undefined> {
 const cookieStore = await cookies();
 const accessToken = cookieStore.get(AUTH_COOKIE.ACCESS)?.value;
 if (accessToken && !isExpiringSoon(accessToken)) {
  return accessToken;
 }

 try {
  return await refreshServerAccessToken();
 } catch (error) {
  logger.warn('[serverAuth] refresh failed', error);
  return undefined;
 }
}

export async function getServerAccessTokenOrRefresh(): Promise<string | undefined> {
 return getServerAccessToken();
}

export interface ServerSessionSnapshot {
 authenticated: boolean;
 user: UserProfile | null;
}

export async function getServerSessionSnapshot(): Promise<ServerSessionSnapshot> {
 const accessToken = await getServerAccessToken();
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
