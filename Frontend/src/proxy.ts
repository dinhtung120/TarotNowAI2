import { NextRequest, NextResponse } from 'next/server';
import createMiddleware from 'next-intl/middleware';
import { routing } from './i18n/routing';
import { getPublicApiOrigin } from '@/shared/infrastructure/http/apiUrl';

const intlMiddleware = createMiddleware(routing);
const localeSet = new Set(routing.locales);

const resolveLocale = (pathname: string) => {
  const maybeLocale = pathname.split('/')[1];
  if (maybeLocale && localeSet.has(maybeLocale as (typeof routing.locales)[number])) {
    return maybeLocale;
  }
  return routing.defaultLocale;
};

const stripLocalePrefix = (pathname: string): string => {
  const maybeLocale = pathname.split("/")[1];
  if (maybeLocale && localeSet.has(maybeLocale as (typeof routing.locales)[number])) {
    const rest = pathname.split("/").slice(2).join("/");
    return `/${rest}`.replace(/\/+$/, "") || "/";
  }
  return pathname.replace(/\/+$/, "") || "/";
};

const matchesPrefix = (pathname: string, prefix: string) =>
  pathname === prefix || pathname.startsWith(`${prefix}/`);

const PROTECTED_PREFIXES = [
  "/profile",
  "/wallet",
  "/chat",
  "/collection",
  "/reading",
  "/reader", 
  "/admin",
];

const clearAuthCookies = (response: NextResponse | Response) => {
  // Using generic way to ensure delete works
  if ('cookies' in response) {
    response.cookies.delete("accessToken");
    response.cookies.delete("refreshToken");
  }
};

const buildContentSecurityPolicy = (): string => {
  const apiOrigin = getPublicApiOrigin().replace(/\/+$/, '');
  
  // websocket origin
  const wsApiOrigin = apiOrigin.startsWith('https://')
    ? `wss://${apiOrigin.slice('https://'.length)}`
    : apiOrigin.startsWith('http://')
      ? `ws://${apiOrigin.slice('http://'.length)}`
      : '';

  const cspParts = [
    "default-src 'self'",
    "base-uri 'self'",
    "frame-ancestors 'none'",
    "form-action 'self'",
    "img-src 'self' data: blob: https:",
    "font-src 'self' data: https:",
    "media-src 'self' blob: data:",
    "style-src 'self' 'unsafe-inline'",
    "script-src 'self' 'unsafe-inline' 'unsafe-eval'", // Giản lược để tránh lỗi Hydration do Nonce thay đổi
    `connect-src 'self' ${apiOrigin} ${wsApiOrigin}`.trim(),
  ];

  return cspParts.join('; ');
};

const withResponseCsp = (response: NextResponse): NextResponse => {
  const csp = buildContentSecurityPolicy();
  response.headers.set('Content-Security-Policy', csp);
  return response;
};

export default async function proxy(request: NextRequest) {
  const { pathname } = request.nextUrl;
  const locale = resolveLocale(pathname);
  const pathWithoutLocale = stripLocalePrefix(pathname);

  const isProtectedRoute = PROTECTED_PREFIXES.some((p) => matchesPrefix(pathWithoutLocale, p));
  const token = request.cookies.get("accessToken")?.value;

  if (isProtectedRoute && !token) {
    const loginUrl = new URL(`/${locale}/login`, request.url);
    const response = NextResponse.redirect(loginUrl);
    clearAuthCookies(response);
    return withResponseCsp(response);
  }

  const response = intlMiddleware(request);
  return withResponseCsp(response);
}

export const config = {
  matcher: ['/((?!api|_next|_vercel|.*\\..*).*)']
};
