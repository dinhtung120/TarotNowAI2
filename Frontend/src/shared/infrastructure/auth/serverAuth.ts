import { cookies } from 'next/headers';
import type { AuthResponse, UserProfile } from '@/features/auth/domain/types';
import { AUTH_COOKIE, AUTH_HEADER, AUTH_SESSION } from '@/shared/infrastructure/auth/authConstants';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

function resolveAccessTtlSeconds(payload: Pick<AuthResponse, 'expiresInSeconds'>): number {
 if (typeof payload.expiresInSeconds === 'number' && payload.expiresInSeconds > 0) {
  return payload.expiresInSeconds;
 }

 return AUTH_SESSION.DEFAULT_ACCESS_TTL_SECONDS;
}

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

function parseRefreshTokenFromHeaders(headers: Headers): string | undefined {
 let cookieStrings: string[];
 if (typeof headers.getSetCookie === 'function') {
  cookieStrings = headers.getSetCookie();
 } else {
  const raw = headers.get('set-cookie');
  if (!raw) {
   return undefined;
  }

  cookieStrings = raw.split(/,\s*(?=[a-zA-Z_]+=)/);
 }

 for (const cookieString of cookieStrings) {
  const firstPair = cookieString.trim().split(';')[0];
  const separator = firstPair.indexOf('=');
  if (separator <= 0) {
   continue;
  }

  const name = firstPair.substring(0, separator).trim();
  const value = firstPair.substring(separator + 1).trim();
  if (name === AUTH_COOKIE.REFRESH && value.length > 0) {
   return value;
  }
 }

 return undefined;
}

async function refreshServerAccessToken(): Promise<string | undefined> {
 const cookieStore = await cookies();
 const refreshToken = cookieStore.get(AUTH_COOKIE.REFRESH)?.value;
 if (!refreshToken) {
  return undefined;
 }

 const deviceId = cookieStore.get(AUTH_COOKIE.DEVICE)?.value ?? '';
 const tokenFingerprint = refreshToken.length > 16 ? refreshToken.slice(-16) : 'anonymous';
 const timeBucket = Math.floor(Date.now() / 30_000);
 const idempotencyKey = `srv-refresh:${tokenFingerprint}:${timeBucket}`;

 const result = await serverHttpRequest<AuthResponse>('/auth/refresh', {
  method: 'POST',
  headers: {
   Cookie: `${AUTH_COOKIE.REFRESH}=${refreshToken}`,
   [AUTH_HEADER.IDEMPOTENCY_KEY]: idempotencyKey,
   [AUTH_HEADER.DEVICE_ID]: deviceId,
  },
  cache: 'no-store',
  fallbackErrorMessage: AUTH_ERROR.UNAUTHORIZED,
 });

 if (!result.ok || !result.data.accessToken) {
  cookieStore.delete(AUTH_COOKIE.ACCESS);
  cookieStore.delete(AUTH_COOKIE.REFRESH);
  return undefined;
 }

 cookieStore.set({
  name: AUTH_COOKIE.ACCESS,
  value: result.data.accessToken,
  httpOnly: true,
  secure: true,
  sameSite: 'strict',
  path: '/',
  maxAge: Math.max(1, resolveAccessTtlSeconds(result.data)),
 });

 const newRefreshToken = parseRefreshTokenFromHeaders(result.headers);
 if (newRefreshToken) {
  cookieStore.set({
   name: AUTH_COOKIE.REFRESH,
   value: newRefreshToken,
   httpOnly: true,
   secure: true,
   sameSite: 'strict',
   path: '/',
   maxAge: AUTH_SESSION.DEFAULT_REFRESH_TTL_SECONDS,
  });
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
