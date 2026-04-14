import { NextRequest, NextResponse } from 'next/server';
import { AUTH_COOKIE, AUTH_HEADER, AUTH_SESSION } from '@/shared/infrastructure/auth/authConstants';
import { AUTH_ERROR } from '@/shared/domain/authErrors';

export function resolveAccessTtlSeconds(payload: { expiresInSeconds?: number }): number {
 if (typeof payload.expiresInSeconds === 'number' && payload.expiresInSeconds > 0) {
  return payload.expiresInSeconds;
 }

 return AUTH_SESSION.DEFAULT_ACCESS_TTL_SECONDS;
}

function parseRefreshTokenFromSetCookie(headers: Headers): string | undefined {
 let cookieStrings: string[] = [];
 if (typeof headers.getSetCookie === 'function') {
  cookieStrings = headers.getSetCookie();
 } else {
  const raw = headers.get('set-cookie');
  if (raw) {
   cookieStrings = raw.split(/,\s*(?=[a-zA-Z_]+=)/);
  }
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

export function setAccessCookie(response: NextResponse, request: NextRequest, accessToken: string, ttlSeconds: number): void {
 response.cookies.set({
  name: AUTH_COOKIE.ACCESS,
  value: accessToken,
  httpOnly: true,
  secure: true,
  sameSite: 'strict',
  path: '/',
  maxAge: Math.max(1, ttlSeconds),
 });
}

export function setRefreshCookieFromHeaders(response: NextResponse, request: NextRequest, headers: Headers): void {
 const refreshToken = parseRefreshTokenFromSetCookie(headers);
 if (!refreshToken) {
  return;
 }

 response.cookies.set({
  name: AUTH_COOKIE.REFRESH,
  value: refreshToken,
  httpOnly: true,
  secure: true,
  sameSite: 'strict',
  path: '/',
  maxAge: AUTH_SESSION.DEFAULT_REFRESH_TTL_SECONDS,
 });
}

export function resolveDeviceIdFromRequest(request: NextRequest): string {
 const fromHeader = request.headers.get(AUTH_HEADER.DEVICE_ID);
 if (fromHeader && fromHeader.trim().length > 0) {
  return fromHeader.trim().slice(0, 128);
 }

 const fromCookie = request.cookies.get(AUTH_COOKIE.DEVICE)?.value;
 if (fromCookie && fromCookie.trim().length > 0) {
  return fromCookie.trim().slice(0, 128);
 }

 return crypto.randomUUID();
}

export function setDeviceCookie(response: NextResponse, request: NextRequest, deviceId: string): void {
 response.cookies.set({
  name: AUTH_COOKIE.DEVICE,
  value: deviceId,
  httpOnly: false,
  secure: true,
  sameSite: 'strict',
  path: '/',
  maxAge: 365 * 24 * 60 * 60,
 });
}

export function clearAuthCookies(response: NextResponse): void {
 response.cookies.delete(AUTH_COOKIE.ACCESS);
 response.cookies.delete(AUTH_COOKIE.REFRESH);
}

export function unauthorizedResponse(clearCookies = false): NextResponse {
 const response = NextResponse.json({ success: false, error: AUTH_ERROR.UNAUTHORIZED }, { status: 401 });
 if (clearCookies) {
  clearAuthCookies(response);
 }

 return response;
}
