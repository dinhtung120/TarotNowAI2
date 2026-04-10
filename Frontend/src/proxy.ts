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
    "script-src 'self' 'unsafe-inline' 'unsafe-eval' https://static.cloudflareinsights.com",
    `connect-src 'self' ${apiOrigin} ${wsApiOrigin} https://cloudflareinsights.com`.trim(),
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

  // 1. Bypass hoàn toàn các request nội bộ của Next.js để tránh vỡ SPA
  // Bao gồm: _next/static, _next/image, favicon, và các file có phần mở rộng (static assets)
  // Đặc biệt: Kiểm tra các header đặc thù của Next.js App Router (RSC, Prefetch)
  const isNextInternal = 
    pathname.startsWith('/_next') || 
    pathname.includes('/api/') ||
    pathname.includes('.') ||
    request.headers.has('x-nextjs-data') ||
    request.headers.get('purpose') === 'prefetch';

  if (isNextInternal) {
    return NextResponse.next();
  }

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
  
  // 2. Chỉ gán CSP cho các trang HTML thực thụ
  // Tránh gán CSP vào các RSC payload (React Server Components) gây ra lỗi reload trang ngầm
  const contentType = response.headers.get('content-type');
  if (contentType?.includes('text/html')) {
    return withResponseCsp(response);
  }

  return response;
}

export const config = {
  // Matcher tối ưu: Chỉ chạy middleware cho các route thực tế, bỏ qua file tĩnh và api
  matcher: [
    /*
     * Match all request paths except for the ones starting with:
     * - api (API routes)
     * - _next/static (static files)
     * - _next/image (image optimization files)
     * - favicon.ico (favicon file)
     * - public files with extensions (svg, png, etc.)
     */
    '/((?!api|_next/static|_next/image|favicon.ico|.*\\..*).*)',
  ],
};
