import { NextRequest, NextResponse } from 'next/server';
import { AUTH_COOKIE, AUTH_HEADER, AUTH_SESSION } from '@/shared/auth/authConstants';
import { AUTH_ERROR } from '@/shared/models/authErrors';
import { buildProblemResponse } from '@/app/api/_shared/problemDetails';
export { buildProblemResponse };

const AUTH_COOKIE_DOMAIN = process.env.AUTH_COOKIE_DOMAIN?.trim() || undefined;
const AUTH_COOKIE_SECURE = process.env.AUTH_COOKIE_SECURE?.trim().toLowerCase();

function resolveHostFromRequest(request: NextRequest): string | undefined {
 const forwardedHost = request.headers.get('x-forwarded-host')?.split(',')[0]?.trim();
 if (forwardedHost) {
  return forwardedHost.split(':')[0]?.trim().toLowerCase();
 }

 const rawHost = request.headers.get('host')?.trim() ?? request.nextUrl.host;
 if (!rawHost) {
  return undefined;
 }

 return rawHost.split(':')[0]?.trim().toLowerCase();
}

function resolveCookieDomain(request?: NextRequest): string | undefined {
 if (!AUTH_COOKIE_DOMAIN) {
  return undefined;
 }

 if (!request) {
  return AUTH_COOKIE_DOMAIN;
 }

 const requestHost = resolveHostFromRequest(request);
 if (!requestHost) {
  return AUTH_COOKIE_DOMAIN;
 }

 const normalizedDomain = AUTH_COOKIE_DOMAIN.toLowerCase();
 if (requestHost === normalizedDomain || requestHost.endsWith(`.${normalizedDomain}`)) {
  return AUTH_COOKIE_DOMAIN;
 }

 return undefined;
}

function resolveCookieCleanupDomains(request?: NextRequest): string[] {
 const domains = new Set<string>();
 const resolved = resolveCookieDomain(request);
 if (resolved) {
  domains.add(resolved.toLowerCase());
 }

 if (!AUTH_COOKIE_DOMAIN) {
  return Array.from(domains);
 }

 const normalized = AUTH_COOKIE_DOMAIN.trim().toLowerCase();
 if (normalized.length === 0) {
  return Array.from(domains);
 }

 domains.add(normalized);
 const withoutLeadingDot = normalized.startsWith('.') ? normalized.slice(1) : normalized;
 if (withoutLeadingDot.length > 0) {
  domains.add(withoutLeadingDot);
 }

 if (withoutLeadingDot.startsWith('www.') && withoutLeadingDot.length > 4) {
  domains.add(withoutLeadingDot.slice(4));
 }

 return Array.from(domains);
}

function shouldUseSecureCookie(request?: NextRequest): boolean {
 if (AUTH_COOKIE_SECURE === 'true') {
  return true;
 }

 if (AUTH_COOKIE_SECURE === 'false') {
  return false;
 }

 if (process.env.NODE_ENV === 'production') {
  return true;
 }

 if (!request) {
  return false;
 }

 const forwardedProto = request.headers.get('x-forwarded-proto')?.split(',')[0]?.trim().toLowerCase();
 if (forwardedProto === 'https') {
  return true;
 }

 return request.nextUrl.protocol === 'https:';
}

function decodeTokenValueOnce(value: string): string {
 if (!/%[0-9a-f]{2}/i.test(value)) {
  return value;
 }

 try {
  return decodeURIComponent(value);
 } catch {
  return value;
 }
}

export function normalizeAuthTokenCookieValue(value: string): string {
 let normalized = value.trim();
 if (!normalized) {
  return normalized;
 }

 // Giảm rủi ro double-encode (%252B...) do nhiều lớp proxy/set-cookie.
 for (let attempt = 0; attempt < 2; attempt += 1) {
  const decoded = decodeTokenValueOnce(normalized);
  if (decoded === normalized) {
   break;
  }
  normalized = decoded;
 }

 return normalized;
}

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
   return normalizeAuthTokenCookieValue(value);
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
  const normalizedCookieValue = normalizeAuthTokenCookieValue(cookieValue);

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
      value: normalizedCookieValue,
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
      value: normalizedCookieValue,
      maxAgeSeconds: remaining,
     };
    }
   }
  }

  return { value: normalizedCookieValue };
 }

 return undefined;
}

export function setAccessCookie(
 response: NextResponse,
 accessToken: string,
 ttlSeconds: number,
 request?: NextRequest,
): void {
 const domain = resolveCookieDomain(request);
 response.cookies.set({
  name: AUTH_COOKIE.ACCESS,
  value: accessToken,
  httpOnly: true,
  secure: shouldUseSecureCookie(request),
  sameSite: 'strict',
  path: '/',
  ...(domain ? { domain } : {}),
  maxAge: Math.max(1, ttlSeconds),
 });
}

export function setRefreshCookieFromHeaders(
 response: NextResponse,
 headers: Headers,
 request?: NextRequest,
): boolean {
 const parsed = parseRefreshCookieMetadata(headers);
 const refreshTokenRaw = parsed?.value ?? parseRefreshTokenFromSetCookie(headers);
 const refreshToken = refreshTokenRaw ? normalizeAuthTokenCookieValue(refreshTokenRaw) : undefined;
 if (!refreshToken) {
  return false;
 }

 const domain = resolveCookieDomain(request);
 response.cookies.set({
  name: AUTH_COOKIE.REFRESH,
  value: refreshToken,
  httpOnly: true,
  secure: shouldUseSecureCookie(request),
  sameSite: 'strict',
  path: '/',
  ...(domain ? { domain } : {}),
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

export function setDeviceCookie(response: NextResponse, deviceId: string, request?: NextRequest): void {
 const domain = resolveCookieDomain(request);
 response.cookies.set({
  name: AUTH_COOKIE.DEVICE,
  value: deviceId,
  httpOnly: false,
  secure: shouldUseSecureCookie(request),
  sameSite: 'strict',
  path: '/',
  ...(domain ? { domain } : {}),
  maxAge: 365 * 24 * 60 * 60,
 });
}

export function clearAuthCookies(response: NextResponse, request?: NextRequest): void {
 const secure = shouldUseSecureCookie(request);
 const cleanupDomains = resolveCookieCleanupDomains(request);
 const appendClearCookieHeader = (name: string, domain?: string): void => {
  const segments = [
   `${name}=`,
   'Path=/',
   'Max-Age=0',
   'HttpOnly',
   'SameSite=Strict',
  ];

  if (secure) {
   segments.push('Secure');
  }

  if (domain) {
   segments.push(`Domain=${domain}`);
  }

  response.headers.append('set-cookie', segments.join('; '));
 };

 appendClearCookieHeader(AUTH_COOKIE.ACCESS);
 appendClearCookieHeader(AUTH_COOKIE.REFRESH);

 for (const domain of cleanupDomains) {
  appendClearCookieHeader(AUTH_COOKIE.ACCESS, domain);
  appendClearCookieHeader(AUTH_COOKIE.REFRESH, domain);
 }
}

export function unauthorizedResponse(clearCookies = false, request?: NextRequest): NextResponse {
 const response = buildProblemResponse(401, AUTH_ERROR.UNAUTHORIZED, AUTH_ERROR.UNAUTHORIZED);
 if (clearCookies) {
  clearAuthCookies(response, request);
 }

 return response;
}

export function getSetCookieHeaders(headers: Headers): string[] {
 if (typeof headers.getSetCookie === 'function') {
  return headers.getSetCookie();
 }

 const raw = headers.get('set-cookie');
 return raw ? raw.split(/,\s*(?=[a-zA-Z_]+=)/) : [];
}

interface SetCookieTarget {
 headers: Headers;
}

export function appendSetCookieHeaders(source: Headers, target: SetCookieTarget): void {
 for (const cookie of getSetCookieHeaders(source)) {
  if (cookie.trim().length > 0) {
   target.headers.append('set-cookie', cookie);
  }
 }
}
