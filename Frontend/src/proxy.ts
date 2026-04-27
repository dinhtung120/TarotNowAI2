import { NextRequest, NextResponse } from 'next/server';
import createMiddleware from 'next-intl/middleware';
import { routing } from './i18n/routing';
import { PROTECTED_PREFIXES } from '@/shared/config/authRoutes';
import { AUTH_COOKIE } from '@/shared/infrastructure/auth/authConstants';
import { getPublicApiOrigin } from '@/shared/infrastructure/http/apiUrl';

const intlMiddleware = createMiddleware(routing);
const localeSet = new Set(routing.locales);
const DEFAULT_CANONICAL_HOST = 'www.tarotnow.xyz';
const CANONICAL_HOST = (process.env.NEXT_PUBLIC_CANONICAL_HOST?.trim().toLowerCase() || DEFAULT_CANONICAL_HOST);
const AUTH_COOKIE_DOMAIN = process.env.AUTH_COOKIE_DOMAIN?.trim() || undefined;

const resolveNonCanonicalHosts = (canonicalHost: string): Set<string> => {
 const hosts = new Set<string>();
 if (canonicalHost.startsWith('www.') && canonicalHost.length > 4) {
  hosts.add(canonicalHost.slice(4));
 }

 // Tương thích ngược với policy domain hiện tại của TarotNow.
 hosts.add('tarotnow.xyz');
 return hosts;
};

const NON_CANONICAL_HOSTS = resolveNonCanonicalHosts(CANONICAL_HOST);

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
 response.cookies.set({
  name: AUTH_COOKIE.ACCESS,
  value: '',
  httpOnly: true,
  secure: true,
  sameSite: 'strict',
  path: '/',
  ...(AUTH_COOKIE_DOMAIN ? { domain: AUTH_COOKIE_DOMAIN } : {}),
  maxAge: 0,
 });
 response.cookies.set({
  name: AUTH_COOKIE.REFRESH,
  value: '',
  httpOnly: true,
  secure: true,
  sameSite: 'strict',
  path: '/',
  ...(AUTH_COOKIE_DOMAIN ? { domain: AUTH_COOKIE_DOMAIN } : {}),
  maxAge: 0,
 });
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

function createCspNonce(): string {
 const bytes = new Uint8Array(16);
 crypto.getRandomValues(bytes);
 let ascii = '';
 for (const value of bytes) {
  ascii += String.fromCharCode(value);
 }

 return btoa(ascii);
}

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

const shouldRedirectToCanonicalHost = (request: NextRequest): boolean => {
 const hostname = request.nextUrl.hostname.toLowerCase();
 if (hostname === CANONICAL_HOST) {
  return false;
 }

 return NON_CANONICAL_HOSTS.has(hostname);
};

const createCanonicalRedirectResponse = (request: NextRequest): NextResponse => {
 const redirectUrl = request.nextUrl.clone();
 redirectUrl.hostname = CANONICAL_HOST;
 redirectUrl.protocol = 'https:';
 redirectUrl.port = '';
 return NextResponse.redirect(redirectUrl, 301);
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

const buildContentSecurityPolicy = (nonce: string): string => {
 const isProduction = process.env.NODE_ENV === 'production';
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

 const styleSrc = isProduction
  ? `style-src 'self' 'nonce-${nonce}'`
  : "style-src 'self' 'unsafe-inline'";
 const scriptSrc = isProduction
  ? `script-src 'self' 'nonce-${nonce}' https://static.cloudflareinsights.com`
  : `script-src 'self' 'unsafe-eval' 'nonce-${nonce}' https://static.cloudflareinsights.com`;

 const cspParts = [
  "default-src 'self'",
  "base-uri 'self'",
  "frame-ancestors 'none'",
  "form-action 'self'",
  "img-src 'self' data: blob: https: http:",
  "font-src 'self' data: https:",
  "media-src 'self' blob: data:",
  styleSrc,
  scriptSrc,
  "worker-src 'self' blob:",
  `connect-src ${connectSrc}`,
 ];

 if (isProduction) {
  /**
   * Runtime UI libraries (ví dụ react-hot-toast) dùng style attributes cho positioning/animation.
   * Chặn tuyệt đối style attributes sẽ làm hỏng login/home và các flow toast toàn app.
   */
  cspParts.push("style-src-attr 'unsafe-inline'");
  cspParts.push('upgrade-insecure-requests');
 }

 return cspParts.join('; ');
};

const withResponseCsp = (response: NextResponse, nonce: string): NextResponse => {
 response.headers.set('x-nonce', nonce);
 response.headers.set('Content-Security-Policy', buildContentSecurityPolicy(nonce));
 return response;
};

/**
 * Middleware cố ý chỉ làm auth presence guard cho route protected để tránh network hop.
 * Refresh/validate token được xử lý ở API layer và client session manager.
 */
export default async function proxy(request: NextRequest) {
 if (shouldRedirectToCanonicalHost(request)) {
  return createCanonicalRedirectResponse(request);
 }

 if (shouldBypassMiddleware(request)) {
  return NextResponse.next();
 }

 const { pathname } = request.nextUrl;
 const locale = resolveLocale(pathname);
 const pathWithoutLocale = stripLocalePrefix(pathname);
 const isDocument = isDocumentRequest(request);
 const cspNonce = isDocument ? createCspNonce() : null;

 const isProtectedRoute = PROTECTED_PREFIXES.some((prefix) => matchesPrefix(pathWithoutLocale, prefix));
 const accessToken = isProtectedRoute ? request.cookies.get(AUTH_COOKIE.ACCESS)?.value : undefined;
 const refreshToken = isProtectedRoute ? request.cookies.get(AUTH_COOKIE.REFRESH)?.value : undefined;

 if (isProtectedRoute && !accessToken && !refreshToken) {
  const loginUrl = new URL(`/${locale}/login`, request.url);
  const response = NextResponse.redirect(loginUrl);
  clearAuthCookies(response);
  return isDocument && cspNonce ? withResponseCsp(response, cspNonce) : response;
 }

 const response = intlMiddleware(request);
 if (isDocument && cspNonce) {
  return withResponseCsp(response, cspNonce);
 }

 return response;
}

export const config = {
 matcher: ['/((?!api|_next|_vercel|favicon.ico|.*\\..*).*)'],
};
