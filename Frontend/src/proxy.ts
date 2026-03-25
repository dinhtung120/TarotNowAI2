import { NextRequest, NextResponse } from 'next/server';
import createMiddleware from 'next-intl/middleware';
import { routing } from './i18n/routing';
import { resolveApiOrigin } from '@/shared/infrastructure/http/apiUrl';

// Khởi tạo middleware của next-intl
const intlMiddleware = createMiddleware(routing);
const localeSet = new Set(routing.locales);
const apiOrigin = resolveApiOrigin(process.env.NEXT_PUBLIC_API_URL);
const wsApiOrigin = apiOrigin.startsWith('https://')
 ? `wss://${apiOrigin.slice('https://'.length)}`
 : apiOrigin.startsWith('http://')
  ? `ws://${apiOrigin.slice('http://'.length)}`
  : '';

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
 "/reader", // NOTE: uses segment prefix check, so it won't match "/readers"
 "/admin",
];

const clearAuthCookies = (response: NextResponse) => {
 response.cookies.delete("accessToken");
 response.cookies.delete("refreshToken");
};

const NONCE_CHARSET = 'ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789+/';
const NONCE_LENGTH = 24;

const createNonce = (): string => {
 const bytes = crypto.getRandomValues(new Uint8Array(NONCE_LENGTH));
 let nonce = '';

 for (const byte of bytes) {
  nonce += NONCE_CHARSET[byte % NONCE_CHARSET.length];
 }

 return nonce;
};

const buildContentSecurityPolicy = (nonce: string): string => {
 const scriptSources = ["'self'", `'nonce-${nonce}'`, "'strict-dynamic'"];
 if (process.env.NODE_ENV !== 'production') {
  scriptSources.push("'unsafe-eval'");
 }

 return [
  "default-src 'self'",
  "base-uri 'self'",
  "frame-ancestors 'none'",
  "form-action 'self'",
  "img-src 'self' data: blob: https:",
  "font-src 'self' data: https:",
  // Tailwind/runtime style injection still needs inline styles.
  "style-src 'self' 'unsafe-inline'",
  `script-src ${scriptSources.join(' ')}`,
  `connect-src 'self' ${apiOrigin} ${wsApiOrigin}`.trim(),
 ].join('; ');
};

const withRequestCsp = (request: NextRequest, csp: string): NextRequest => {
 const requestHeaders = new Headers(request.headers);
 requestHeaders.set('content-security-policy', csp);
 return new NextRequest(request, { headers: requestHeaders });
};

const withResponseCsp = (response: NextResponse, csp: string): NextResponse => {
 response.headers.set('Content-Security-Policy', csp);
 return response;
};

/*
 * ===================================================================
 * COMPONENT/FILE: Proxy (proxy.ts)
 * BỐI CẢNH (CONTEXT):
 *   Đóng vai trò như một Middleware luồng chặn (Edge Runtime) trước khi Request 
 *   vào các Component.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Tích hợp Middleware Đa ngôn ngữ (`intlMiddleware`) xử lý Routing URL (Ví dụ: /vn/home).
 *   - Chặn các đường dẫn Yêu cầu Đăng nhập (`PROTECTED_PREFIXES`) và chuyển hướng (Redirect) 
 *     về trang Login nếu phát hiện thiếu `accessToken` cookie.
 *   - Middleware này chỉ đóng vai trò UX guard ở tầng Edge.
 *     Authorization thực tế (đặc biệt role admin) vẫn do Backend/API xác thực chữ ký JWT.
 * ===================================================================
 */
export default async function proxy(request: NextRequest) {
 const { pathname } = request.nextUrl;
 const locale = resolveLocale(pathname);
 const pathWithoutLocale = stripLocalePrefix(pathname);
 const csp = buildContentSecurityPolicy(createNonce());
 const requestWithCsp = withRequestCsp(request, csp);

 const isProtectedRoute = PROTECTED_PREFIXES.some((p) => matchesPrefix(pathWithoutLocale, p));

 const token = request.cookies.get("accessToken")?.value;

 if (isProtectedRoute) {
  if (!token) {
   const loginUrl = new URL(`/${locale}/login`, request.url);
   const response = NextResponse.redirect(loginUrl);
   clearAuthCookies(response);
   return withResponseCsp(response, csp);
  }
 }

 // Nếu hợp lệ hoặc không phải admin route, tiếp tục xử lý i18n
 return withResponseCsp(intlMiddleware(requestWithCsp), csp);
}

export const config = {
 // Matcher cho các đường dẫn cần xử lý i18n và auth
 matcher: ['/((?!api|_next|_vercel|.*\\..*).*)']
};
