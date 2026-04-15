import { cookies } from 'next/headers';
import type { UserProfile } from '@/features/auth/domain/types';
import { AUTH_COOKIE, AUTH_SESSION } from '@/shared/infrastructure/auth/authConstants';
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

function isExpired(token: string | undefined): boolean {
 const exp = parseJwtExp(token);
 if (!exp) {
  return true;
 }

 const now = Math.floor(Date.now() / 1000);
 return exp <= now + Math.max(0, AUTH_SESSION.ACCESS_REFRESH_THRESHOLD_SECONDS);
}

export async function getServerAccessToken(): Promise<string | undefined> {
 const cookieStore = await cookies();
 const accessToken = cookieStore.get(AUTH_COOKIE.ACCESS)?.value;
 if (!accessToken) {
  return undefined;
 }

 return isExpired(accessToken) ? undefined : accessToken;
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
