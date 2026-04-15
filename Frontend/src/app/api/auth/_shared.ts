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

interface ParsedRefreshCookieMetadata {
 value: string;
 maxAgeSeconds?: number;
}

function parseRefreshCookieMetadata(headers: Headers): ParsedRefreshCookieMetadata | undefined {
 const cookieStrings = typeof headers.getSetCookie === 'function'
  ? headers.getSetCookie()
  : (headers.get('set-cookie')?.split(/,\s*(?=[a-zA-Z_]+=)/) ?? []);

 for (const cookieString of cookieStrings) {
  const parts = cookieString
   .split(';')
   .map((segment) => segment.trim())
   .filter((segment) => segment.length > 0);
  if (parts.length === 0) {
   continue;
  }

  const [namePart, ...attributeParts] = parts;
  const separator = namePart.indexOf('=');
  if (separator <= 0) {
   continue;
  }

  const cookieName = namePart.substring(0, separator).trim();
  const cookieValue = namePart.substring(separator + 1).trim();
  if (cookieName !== AUTH_COOKIE.REFRESH || cookieValue.length === 0) {
   continue;
  }

  const attributes = new Map<string, string>();
  for (const attribute of attributeParts) {
   const attributeSeparator = attribute.indexOf('=');
   if (attributeSeparator <= 0) {
    attributes.set(attribute.toLowerCase(), '');
    continue;
   }

   const key = attribute.substring(0, attributeSeparator).trim().toLowerCase();
   const value = attribute.substring(attributeSeparator + 1).trim();
   attributes.set(key, value);
  }

  const maxAgeRaw = attributes.get('max-age');
  if (maxAgeRaw && /^-?\d+$/.test(maxAgeRaw)) {
   const maxAgeParsed = Number.parseInt(maxAgeRaw, 10);
   if (Number.isFinite(maxAgeParsed) && maxAgeParsed > 0) {
    return {
     value: cookieValue,
     maxAgeSeconds: maxAgeParsed,
    };
   }
  }

  const expiresRaw = attributes.get('expires');
  if (expiresRaw) {
   const expiresDate = new Date(expiresRaw);
   if (!Number.isNaN(expiresDate.getTime())) {
    const remaining = Math.floor((expiresDate.getTime() - Date.now()) / 1000);
    if (remaining > 0) {
     return {
      value: cookieValue,
      maxAgeSeconds: remaining,
     };
    }
   }
  }

  return { value: cookieValue };
 }

 return undefined;
}

export function setAccessCookie(response: NextResponse, accessToken: string, ttlSeconds: number): void {
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

export function setRefreshCookieFromHeaders(response: NextResponse, headers: Headers): boolean {
 const parsed = parseRefreshCookieMetadata(headers);
 const refreshToken = parsed?.value ?? parseRefreshTokenFromSetCookie(headers);
 if (!refreshToken) {
  return false;
 }

 response.cookies.set({
  name: AUTH_COOKIE.REFRESH,
  value: refreshToken,
  httpOnly: true,
  secure: true,
  sameSite: 'strict',
  path: '/',
  maxAge: Math.max(1, parsed?.maxAgeSeconds ?? AUTH_SESSION.DEFAULT_REFRESH_TTL_SECONDS),
 });
 return true;
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

export function setDeviceCookie(response: NextResponse, deviceId: string): void {
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
