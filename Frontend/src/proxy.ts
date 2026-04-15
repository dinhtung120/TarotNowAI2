import { NextRequest, NextResponse } from 'next/server';
import createMiddleware from 'next-intl/middleware';
import { routing } from './i18n/routing';
import { PROTECTED_PREFIXES } from '@/shared/config/authRoutes';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { AUTH_COOKIE, AUTH_HEADER, AUTH_SESSION } from '@/shared/infrastructure/auth/authConstants';
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
 * Xóa cookies authentication khi redirect user chưa đăng nhập.
 * Đảm bảo không còn token rác trong browser sau khi bị kick ra khỏi protected route.
 */
const clearAuthCookies = (response: NextResponse | Response): void => {
  if ('cookies' in response) {
    response.cookies.delete(AUTH_COOKIE.ACCESS);
    response.cookies.delete(AUTH_COOKIE.REFRESH);
  }
};

/**
 * Parse mốc hết hạn `exp` từ JWT access token.
 */
const parseJwtExpSeconds = (token: string | undefined): number | undefined => {
  if (!token) return undefined;
  const parts = token.split('.');
  if (parts.length < 2 || !parts[1]) return undefined;

  try {
    const base64 = parts[1].replace(/-/g, '+').replace(/_/g, '/');
    const padded = `${base64}${'='.repeat((4 - (base64.length % 4)) % 4)}`;
    const payload = JSON.parse(atob(padded)) as { exp?: number };
    return typeof payload.exp === 'number' ? payload.exp : undefined;
  } catch {
    return undefined;
  }
};

/**
 * Khi token thiếu hoặc sắp hết hạn, middleware sẽ kích hoạt refresh.
 */
const shouldAttemptRefresh = (accessToken: string | undefined, refreshToken: string | undefined): boolean => {
  if (!refreshToken) return false;
  if (!accessToken) return true;

  const exp = parseJwtExpSeconds(accessToken);
  if (!exp) return true;
  const now = Math.floor(Date.now() / 1000);
  return exp - now <= AUTH_SESSION.ACCESS_REFRESH_THRESHOLD_SECONDS;
};

function getSetCookieHeaders(headers: Headers): string[] {
  if (typeof headers.getSetCookie === 'function') {
    return headers.getSetCookie();
  }

  const raw = headers.get('set-cookie');
  if (!raw) return [];
  return raw.split(/,\s*(?=[a-zA-Z_]+=)/);
}

/**
 * Đồng bộ Set-Cookie từ response refresh vào response middleware.
 */
const appendRefreshCookies = (source: Headers, target: NextResponse): void => {
  for (const cookie of getSetCookieHeaders(source)) {
    if (cookie.trim().length > 0) {
      target.headers.append('set-cookie', cookie);
    }
  }
};

const buildMiddlewareIdempotencyKey = (deviceId: string | undefined): string => {
  const deviceFingerprint = deviceId && deviceId.length > 16
    ? deviceId.slice(-16)
    : deviceId || 'anonymous';
  const timeBucket = Math.floor(Date.now() / 30_000);
  return `mw-refresh:${deviceFingerprint}:${timeBucket}`;
};

const refreshSessionViaInternalRoute = async (
  request: NextRequest,
): Promise<Response> => {
  const refreshUrl = new URL('/api/auth/refresh', request.url);
  const forwardedDeviceId = request.headers.get(AUTH_HEADER.DEVICE_ID)
    ?? request.cookies.get(AUTH_COOKIE.DEVICE)?.value
    ?? '';
  return fetch(refreshUrl, {
    method: 'POST',
    headers: {
      Cookie: request.headers.get('cookie') ?? '',
      [AUTH_HEADER.IDEMPOTENCY_KEY]: buildMiddlewareIdempotencyKey(forwardedDeviceId),
      [AUTH_HEADER.DEVICE_ID]: forwardedDeviceId,
      [AUTH_HEADER.FORWARDED_USER_AGENT]: request.headers.get('user-agent') ?? '',
    },
    cache: 'no-store',
  });
};

const validateSessionViaInternalRoute = async (request: NextRequest): Promise<Response> => {
 const sessionUrl = new URL('/api/auth/session', request.url);
 const forwardedDeviceId = request.headers.get(AUTH_HEADER.DEVICE_ID)
  ?? request.cookies.get(AUTH_COOKIE.DEVICE)?.value
  ?? '';
 return fetch(sessionUrl, {
  method: 'GET',
  headers: {
   Cookie: request.headers.get('cookie') ?? '',
   [AUTH_HEADER.DEVICE_ID]: forwardedDeviceId,
   [AUTH_HEADER.FORWARDED_USER_AGENT]: request.headers.get('user-agent') ?? '',
  },
  cache: 'no-store',
 });
};

const isTerminalAuthFailure = async (response: Response): Promise<boolean> => {
 if (response.status === 401 || response.status === 403) {
  return true;
 }

 try {
  const payload = (await response.clone().json()) as { error?: string };
  const error = payload.error;
  return error === AUTH_ERROR.UNAUTHORIZED
   || error === AUTH_ERROR.TOKEN_EXPIRED
   || error === AUTH_ERROR.TOKEN_REPLAY;
 } catch {
  return false;
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
 * Đọc danh sách connect-src bổ sung từ env (cách nhau bởi dấu phẩy).
 * Ví dụ: NEXT_PUBLIC_CSP_CONNECT_SRC_EXTRA=https://foo.example.com,https://bar.example.com
 */
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

/**
 * Origin R2 upload dùng cho connect-src.
 * Nếu không khai báo explicit, fallback wildcard cho mọi account R2 endpoint.
 */
const resolveR2UploadConnectSrc = (): string => {
  const explicitOrigin = process.env.NEXT_PUBLIC_R2_UPLOAD_ORIGIN?.trim();
  if (explicitOrigin) {
    return explicitOrigin;
  }

  return 'https://*.r2.cloudflarestorage.com';
};

/**
 * Xây dựng chuỗi Content-Security-Policy dựa trên cấu hình API origin.
 * CSP giúp chống XSS bằng cách giới hạn nguồn tài nguyên mà browser được phép tải.
 *
 * connect-src bao gồm cả http(s), ws(s) và endpoint R2 upload để hỗ trợ:
 * - SignalR WebSocket connections
 * - Direct upload qua presigned URL.
 */
const buildContentSecurityPolicy = (): string => {
  const apiOrigin = getPublicApiOrigin().replace(/\/+$/, '');

  /* Chuyển đổi https:// → wss:// hoặc http:// → ws:// cho WebSocket */
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
    // Presigned PUT URL của Cloudflare R2 dùng host dạng <account>.r2.cloudflarestorage.com.
    resolveR2UploadConnectSrc(),
    ...resolveExtraConnectSrc(),
  ]);

  const connectSrc = Array.from(connectSrcSet).filter((value) => value.length > 0).join(' ');

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
    "worker-src 'self' blob:",
    `connect-src ${connectSrc}`,
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
  const accessToken = isProtectedRoute ? request.cookies.get(AUTH_COOKIE.ACCESS)?.value : undefined;
  const refreshToken = isProtectedRoute ? request.cookies.get(AUTH_COOKIE.REFRESH)?.value : undefined;
  const intlResponseWithOptionalCsp = (): NextResponse => {
    const response = intlMiddleware(request);
    if (isDocumentRequest(request)) {
      return withResponseCsp(response);
    }
    return response;
  };

  if (isProtectedRoute && !accessToken && !refreshToken) {
    const loginUrl = new URL(`/${locale}/login`, request.url);
    const response = NextResponse.redirect(loginUrl);
    clearAuthCookies(response);
    return withResponseCsp(response);
  }

  if (isProtectedRoute && shouldAttemptRefresh(accessToken, refreshToken)) {
    let refreshResponse: Response;
    try {
      refreshResponse = await refreshSessionViaInternalRoute(request);
    } catch {
      return intlResponseWithOptionalCsp();
    }
    if (!refreshResponse.ok) {
      if (await isTerminalAuthFailure(refreshResponse)) {
        const loginUrl = new URL(`/${locale}/login`, request.url);
        const response = NextResponse.redirect(loginUrl);
        clearAuthCookies(response);
        return withResponseCsp(response);
      }

      return intlResponseWithOptionalCsp();
    }

    const response = intlMiddleware(request);
    appendRefreshCookies(refreshResponse.headers, response);
    if (isDocumentRequest(request)) {
      return withResponseCsp(response);
    }

    return response;
  }

  if (isProtectedRoute && accessToken && isDocumentRequest(request)) {
    let sessionResponse: Response;
    try {
      sessionResponse = await validateSessionViaInternalRoute(request);
    } catch {
      return intlResponseWithOptionalCsp();
    }
    if (!sessionResponse.ok) {
      if (await isTerminalAuthFailure(sessionResponse)) {
        const loginUrl = new URL(`/${locale}/login`, request.url);
        const response = NextResponse.redirect(loginUrl);
        clearAuthCookies(response);
        return withResponseCsp(response);
      }

      return intlResponseWithOptionalCsp();
    }

    const response = intlMiddleware(request);
    appendRefreshCookies(sessionResponse.headers, response);
    if (isDocumentRequest(request)) {
      return withResponseCsp(response);
    }

    return response;
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
