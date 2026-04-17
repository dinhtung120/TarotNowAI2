import { NextRequest, NextResponse } from 'next/server';
import createMiddleware from 'next-intl/middleware';
import { routing } from './i18n/routing';
import { PROTECTED_PREFIXES } from '@/shared/config/authRoutes';
import { AUTH_COOKIE } from '@/shared/infrastructure/auth/authConstants';
import { getPublicApiOrigin } from '@/shared/infrastructure/http/apiUrl';

const intlMiddleware = createMiddleware(routing);
const localeSet = new Set(routing.locales);
const ROLE_CLAIM_KEY = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
const DEFAULT_REDIRECT_AFTER_ROLE_GUARD = '/profile';

const resolveLocale = (pathname: string): string => {
 const maybeLocale = pathname.split('/')[1];
 if (maybeLocale && localeSet.has(maybeLocale as (typeof routing.locales)[number])) {
  return maybeLocale;
 }
 return routing.defaultLocale;
};

const stripLocalePrefix = (pathname: string): string => {
 const maybeLocale = pathname.split('/')[1];
 if (maybeLocale && localeSet.has(maybeLocale as (typeof routing.locales)[number])) {
  const rest = pathname.split('/').slice(2).join('/');
  return `/${rest}`.replace(/\/+$/, '') || '/';
 }
 return pathname.replace(/\/+$/, '') || '/';
};

const matchesPrefix = (pathname: string, prefix: string): boolean =>
 pathname === prefix || pathname.startsWith(`${prefix}/`);

const clearAuthCookies = (response: NextResponse): void => {
 response.cookies.delete(AUTH_COOKIE.ACCESS);
 response.cookies.delete(AUTH_COOKIE.REFRESH);
};

const hasFileExtension = (pathname: string): boolean => {
 const lastSegment = pathname.split('/').pop() ?? '';
 return lastSegment.includes('.');
};

const isApiPath = (pathname: string): boolean =>
 pathname.startsWith('/api/') || /^\/[a-z]{2}(?:-[A-Z]{2})?\/api(?:\/|$)/.test(pathname);

const isDocumentRequest = (request: NextRequest): boolean => {
 const accept = request.headers.get('accept') ?? '';
 const hasRscMarker =
  request.headers.get('rsc') === '1'
  || request.nextUrl.searchParams.has('_rsc')
  || accept.includes('text/x-component');
 return accept.includes('text/html') && !hasRscMarker;
};

const decodeJwtPayload = (token: string | undefined): Record<string, unknown> | null => {
 if (!token) {
  return null;
 }

 const parts = token.split('.');
 if (parts.length < 2 || !parts[1]) {
  return null;
 }

 const normalized = parts[1].replace(/-/g, '+').replace(/_/g, '/');
 const padded = `${normalized}${'='.repeat((4 - (normalized.length % 4)) % 4)}`;
 try {
  return JSON.parse(atob(padded)) as Record<string, unknown>;
 } catch {
  return null;
 }
};

const resolveRoleFromAccessToken = (token: string | undefined): string => {
 const payload = decodeJwtPayload(token);
 if (!payload) {
  return '';
 }

 const directRole = payload.role;
 if (typeof directRole === 'string' && directRole.trim().length > 0) {
  return directRole.trim();
 }

 const claimRole = payload[ROLE_CLAIM_KEY];
 if (typeof claimRole === 'string' && claimRole.trim().length > 0) {
  return claimRole.trim();
 }

 if (Array.isArray(claimRole)) {
  const firstRole = claimRole.find((value) => typeof value === 'string' && value.trim().length > 0);
  return typeof firstRole === 'string' ? firstRole : '';
 }

 return '';
};

const resolveRoleGuardRedirect = (
 request: NextRequest,
 locale: string,
 pathWithoutLocale: string,
): NextResponse | null => {
 const accessToken = request.cookies.get(AUTH_COOKIE.ACCESS)?.value;
 const role = resolveRoleFromAccessToken(accessToken).toLowerCase();
 if (!role) {
  return null;
 }

 if (matchesPrefix(pathWithoutLocale, '/profile/reader') && role !== 'tarot_reader') {
  return NextResponse.redirect(new URL(`/${locale}${DEFAULT_REDIRECT_AFTER_ROLE_GUARD}`, request.url));
 }

 if (matchesPrefix(pathWithoutLocale, '/admin') && role !== 'admin') {
  return NextResponse.redirect(new URL(`/${locale}${DEFAULT_REDIRECT_AFTER_ROLE_GUARD}`, request.url));
 }

 return null;
};

const shouldBypassMiddleware = (request: NextRequest): boolean => {
 const { pathname } = request.nextUrl;
 return (
  pathname.startsWith('/_next')
  || pathname.startsWith('/_vercel')
  || pathname === '/favicon.ico'
  || isApiPath(pathname)
  || hasFileExtension(pathname)
 );
};

const resolveExtraConnectSrc = (): string[] => {
 const raw = process.env.NEXT_PUBLIC_CSP_CONNECT_SRC_EXTRA?.trim();
 if (!raw) {
  return [];
 }

 return raw
  .split(',')
  .map((value) => value.trim())
  .filter((value) => value.length > 0);
};

const resolveR2UploadConnectSrc = (): string => {
 const explicitOrigin = process.env.NEXT_PUBLIC_R2_UPLOAD_ORIGIN?.trim();
 if (explicitOrigin) {
  return explicitOrigin;
 }

 return 'https://*.r2.cloudflarestorage.com';
};

const buildContentSecurityPolicy = (): string => {
 const apiOrigin = getPublicApiOrigin().replace(/\/+$/, '');
 const wsApiOrigin = apiOrigin.startsWith('https://')
  ? `wss://${apiOrigin.slice('https://'.length)}`
  : apiOrigin.startsWith('http://')
   ? `ws://${apiOrigin.slice('http://'.length)}`
   : '';

 const connectSrcSet = new Set<string>([
  "'self'",
  apiOrigin,
  wsApiOrigin,
  'https://cloudflareinsights.com',
  resolveR2UploadConnectSrc(),
  ...resolveExtraConnectSrc(),
 ]);

 const connectSrc = Array.from(connectSrcSet)
  .filter((value) => value.length > 0)
  .join(' ');

  const cspParts = [
  "default-src 'self'",
  "base-uri 'self'",
  "frame-ancestors 'none'",
  "form-action 'self'",
  "img-src 'self' data: blob: https: http:",
  "font-src 'self' data: https:",
  "media-src 'self' blob: data:",
  "style-src 'self' 'unsafe-inline'",
  "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://static.cloudflareinsights.com",
  "worker-src 'self' blob:",
  `connect-src ${connectSrc}`,
 ];

 if (process.env.NODE_ENV === 'production') {
  cspParts.push('upgrade-insecure-requests');
 }

 return cspParts.join('; ');
};

const withResponseCsp = (response: NextResponse): NextResponse => {
 response.headers.set('Content-Security-Policy', buildContentSecurityPolicy());
 return response;
};

/**
 * Middleware cố ý chỉ làm auth presence guard cho route protected để tránh network hop.
 * Refresh/validate token được xử lý ở API layer và client session manager.
 */
export default function proxy(request: NextRequest) {
 if (shouldBypassMiddleware(request)) {
  return NextResponse.next();
 }

 const { pathname } = request.nextUrl;
 const locale = resolveLocale(pathname);
 const pathWithoutLocale = stripLocalePrefix(pathname);

 const isProtectedRoute = PROTECTED_PREFIXES.some((prefix) => matchesPrefix(pathWithoutLocale, prefix));
 const accessToken = isProtectedRoute ? request.cookies.get(AUTH_COOKIE.ACCESS)?.value : undefined;
 const refreshToken = isProtectedRoute ? request.cookies.get(AUTH_COOKIE.REFRESH)?.value : undefined;

 if (isDocumentRequest(request)) {
  const roleGuardRedirect = resolveRoleGuardRedirect(request, locale, pathWithoutLocale);
  if (roleGuardRedirect) {
   return withResponseCsp(roleGuardRedirect);
  }
 }

 if (isProtectedRoute && !accessToken && !refreshToken) {
  const loginUrl = new URL(`/${locale}/login`, request.url);
  const response = NextResponse.redirect(loginUrl);
  clearAuthCookies(response);
  return withResponseCsp(response);
 }

 const response = intlMiddleware(request);
 if (isDocumentRequest(request)) {
  return withResponseCsp(response);
 }

 return response;
}

export const config = {
 matcher: ['/((?!api|_next|_vercel|favicon.ico|.*\\..*).*)'],
};
