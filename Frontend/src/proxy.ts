import { NextRequest, NextResponse } from 'next/server';
import createMiddleware from 'next-intl/middleware';
import { routing } from './i18n/routing';

// Khởi tạo middleware của next-intl
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
 "/reader", // NOTE: uses segment prefix check, so it won't match "/readers"
 "/admin",
];

const clearAuthCookies = (response: NextResponse) => {
 response.cookies.delete("accessToken");
 response.cookies.delete("refreshToken");
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

 const isProtectedRoute = PROTECTED_PREFIXES.some((p) => matchesPrefix(pathWithoutLocale, p));

 const token = request.cookies.get("accessToken")?.value;

 if (isProtectedRoute) {
  if (!token) {
   const loginUrl = new URL(`/${locale}/login`, request.url);
   const response = NextResponse.redirect(loginUrl);
   clearAuthCookies(response);
   return response;
  }
 }

 // Nếu hợp lệ hoặc không phải admin route, tiếp tục xử lý i18n
 return intlMiddleware(request);
}

export const config = {
 // Matcher cho các đường dẫn cần xử lý i18n và auth
 matcher: ['/((?!api|_next|_vercel|.*\\..*).*)']
};
