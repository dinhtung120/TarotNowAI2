import { NextRequest, NextResponse } from 'next/server';
import createMiddleware from 'next-intl/middleware';
import { routing } from './i18n/routing';
import { getPublicApiOrigin } from '@/shared/infrastructure/http/apiUrl';

/**
 * Khởi tạo next-intl middleware để xử lý đa ngôn ngữ.
 * next-intl middleware tự động detect locale từ URL, cookie, hoặc Accept-Language header
 * và redirect/rewrite request đến đúng locale path.
 */
const intlMiddleware = createMiddleware(routing);

/**
 * Set chứa tất cả các locale hỗ trợ, dùng cho O(1) lookup khi kiểm tra locale.
 */
const localeSet = new Set(routing.locales);

/* ------------------------------------------------------------------ */
/*  Helper functions                                                   */
/* ------------------------------------------------------------------ */

/**
 * Trích xuất locale từ pathname (segment đầu tiên sau /).
 * Nếu pathname không chứa locale hợp lệ, trả về defaultLocale.
 *
 * Ví dụ:
 *   "/vi/reading" → "vi"
 *   "/unknown/page" → defaultLocale ("vi")
 */
const resolveLocale = (pathname: string): string => {
  const maybeLocale = pathname.split('/')[1];
  if (maybeLocale && localeSet.has(maybeLocale as (typeof routing.locales)[number])) {
    return maybeLocale;
  }
  return routing.defaultLocale;
};

/**
 * Loại bỏ locale prefix khỏi pathname để so khớp với PROTECTED_PREFIXES.
 * Trailing slash cũng bị loại bỏ để normalize path.
 *
 * Ví dụ:
 *   "/vi/profile/settings" → "/profile/settings"
 *   "/profile/settings" → "/profile/settings"
 */
const stripLocalePrefix = (pathname: string): string => {
  const maybeLocale = pathname.split('/')[1];
  if (maybeLocale && localeSet.has(maybeLocale as (typeof routing.locales)[number])) {
    const rest = pathname.split('/').slice(2).join('/');
    return `/${rest}`.replace(/\/+$/, '') || '/';
  }
  return pathname.replace(/\/+$/, '') || '/';
};

/**
 * Kiểm tra pathname có khớp với prefix (bằng hoặc bắt đầu bằng prefix/).
 * Đảm bảo "/reader" không match "/reading" vì yêu cầu boundary "/".
 */
const matchesPrefix = (pathname: string, prefix: string): boolean =>
  pathname === prefix || pathname.startsWith(`${prefix}/`);

/**
 * Danh sách các route yêu cầu authentication.
 * Nếu user chưa login (không có accessToken cookie), middleware sẽ redirect về trang login.
 */
const PROTECTED_PREFIXES = [
  '/profile',
  '/wallet',
  '/chat',
  '/collection',
  '/reading',
  '/reader',
  '/admin',
];

/**
 * Xóa cookies authentication khi redirect user chưa đăng nhập.
 * Đảm bảo không còn token rác trong browser sau khi bị kick ra khỏi protected route.
 */
const clearAuthCookies = (response: NextResponse | Response): void => {
  if ('cookies' in response) {
    response.cookies.delete('accessToken');
    response.cookies.delete('refreshToken');
  }
};

/* ------------------------------------------------------------------ */
/*  Request classification helpers                                     */
/* ------------------------------------------------------------------ */

/**
 * Kiểm tra pathname có phải là file tĩnh (có extension) hay không.
 * Dùng để bypass middleware cho các request đến file tĩnh như .js, .css, .png, v.v.
 */
const hasFileExtension = (pathname: string): boolean => {
  const lastSegment = pathname.split('/').pop() ?? '';
  return lastSegment.includes('.');
};

/**
 * Kiểm tra pathname có phải API route hay không.
 * Hỗ trợ cả /api/ và /{locale}/api/ patterns.
 * API routes được xử lý bởi Next.js API layer hoặc rewrite, không cần middleware.
 */
const isApiPath = (pathname: string): boolean =>
  pathname.startsWith('/api/') || /^\/[a-z]{2}(?:-[A-Z]{2})?\/api(?:\/|$)/.test(pathname);

/**
 * Kiểm tra request có phải là document request (HTML page) hay không.
 * Document requests cần CSP headers để bảo vệ trang HTML.
 * Flight/prefetch requests KHÔNG phải document → không cần CSP.
 *
 * Quan trọng: header "RSC" = "1" hoặc accept "text/x-component" cho biết
 * đây là RSC flight request, KHÔNG phải document request.
 */
const isDocumentRequest = (request: NextRequest): boolean => {
  const accept = request.headers.get('accept') ?? '';
  const hasRscMarker =
    request.headers.get('rsc') === '1' ||
    request.nextUrl.searchParams.has('_rsc') ||
    accept.includes('text/x-component');
  return accept.includes('text/html') && !hasRscMarker;
};

/**
 * Kiểm tra request có phải pure static request mà middleware KHÔNG nên xử lý.
 *
 * THIẾT KẾ QUAN TRỌNG:
 * - KHÔNG bypass RSC flight requests ở đây! RSC flight requests vẫn cần
 *   đi qua intlMiddleware để next-intl có thể resolve đúng locale.
 *   Nếu bypass RSC requests, client-side navigation (SPA) sẽ bị broken
 *   vì next-intl không biết locale → Next.js fallback hard navigation.
 * - Chỉ bypass các request hoàn toàn static: _next/, favicon, API routes,
 *   và file có extension.
 */
const shouldBypassMiddleware = (request: NextRequest): boolean => {
  const { pathname } = request.nextUrl;
  return (
    pathname.startsWith('/_next') ||
    pathname.startsWith('/_vercel') ||
    pathname === '/favicon.ico' ||
    isApiPath(pathname) ||
    hasFileExtension(pathname)
  );
};

/* ------------------------------------------------------------------ */
/*  CSP (Content Security Policy)                                      */
/* ------------------------------------------------------------------ */

/**
 * Xây dựng chuỗi Content-Security-Policy dựa trên cấu hình API origin.
 * CSP giúp chống XSS bằng cách giới hạn nguồn tài nguyên mà browser được phép tải.
 *
 * connect-src bao gồm cả http(s) và ws(s) để hỗ trợ SignalR WebSocket connections.
 */
const buildContentSecurityPolicy = (): string => {
  const apiOrigin = getPublicApiOrigin().replace(/\/+$/, '');

  /* Chuyển đổi https:// → wss:// hoặc http:// → ws:// cho WebSocket */
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

/**
 * Thêm CSP header vào response.
 * Chỉ gọi cho document (HTML) requests.
 * RSC payload không cần CSP vì không phải HTML document.
 */
const withResponseCsp = (response: NextResponse): NextResponse => {
  const csp = buildContentSecurityPolicy();
  response.headers.set('Content-Security-Policy', csp);
  return response;
};

/* ------------------------------------------------------------------ */
/*  Main middleware function                                           */
/* ------------------------------------------------------------------ */

/**
 * Middleware chính của ứng dụng, xử lý:
 *
 * 1. **Bypass static resources**: Bỏ qua _next/, favicon, API routes, file tĩnh.
 *    Những request này không cần locale resolution hay auth check.
 *
 * 2. **Auth guard**: Protected routes yêu cầu accessToken cookie.
 *    Nếu thiếu token → redirect về login và xóa auth cookies.
 *
 * 3. **Internationalization (i18n)**: Delegate cho next-intl middleware
 *    để xử lý locale detection và URL rewriting.
 *    QUAN TRỌNG: Cả document requests LẪN RSC flight requests đều đi qua
 *    intlMiddleware để đảm bảo SPA navigation không bị break.
 *
 * 4. **CSP headers**: Chỉ thêm Content-Security-Policy cho document requests.
 *    RSC payloads (text/x-component) không cần CSP.
 */
export default async function proxy(request: NextRequest) {
  const { pathname } = request.nextUrl;

  /*
   * Bỏ qua các request hoàn toàn static.
   * LƯU Ý: RSC flight requests KHÔNG bị bypass ở đây!
   * RSC requests vẫn cần intlMiddleware để resolve locale cho SPA navigation.
   */
  if (shouldBypassMiddleware(request)) {
    return NextResponse.next();
  }

  /* ── Auth guard cho protected routes ── */
  const locale = resolveLocale(pathname);
  const pathWithoutLocale = stripLocalePrefix(pathname);

  const isProtectedRoute = PROTECTED_PREFIXES.some((p) => matchesPrefix(pathWithoutLocale, p));
  const token = isProtectedRoute ? request.cookies.get('accessToken')?.value : undefined;

  if (isProtectedRoute && !token) {
    const loginUrl = new URL(`/${locale}/login`, request.url);
    const response = NextResponse.redirect(loginUrl);
    clearAuthCookies(response);
    return withResponseCsp(response);
  }

  /* ── Delegate cho next-intl middleware ── */
  const response = intlMiddleware(request);

  /*
   * Chỉ gán CSP header cho document requests (HTML pages).
   * RSC flight responses trả về text/x-component, KHÔNG phải HTML,
   * nên không cần và không nên có CSP header (có thể gây lỗi parsing).
   */
  if (isDocumentRequest(request)) {
    return withResponseCsp(response);
  }

  return response;
}

/**
 * Matcher configuration cho middleware.
 *
 * Chỉ chạy middleware cho các route thực tế:
 * - Loại trừ api routes (xử lý bởi Next.js API layer)
 * - Loại trừ _next/* (static assets, webpack HMR, v.v.)
 * - Loại trừ _vercel/* (Vercel internals)
 * - Loại trừ favicon.ico
 * - Loại trừ file có extension (hình ảnh, fonts, v.v.)
 *
 * Lý do dùng regex negative lookahead: Next.js middleware matcher chỉ hỗ trợ
 * path pattern, không hỗ trợ header-based matching. Regex ở đây đảm bảo
 * middleware chỉ invoke cho page routes, giảm overhead cho static requests.
 */
export const config = {
  matcher: ['/((?!api|_next|_vercel|favicon.ico|.*\\..*).*)',],
};
