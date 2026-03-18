import { NextRequest, NextResponse } from 'next/server';
import createMiddleware from 'next-intl/middleware';
import { routing } from './i18n/routing';

// Khởi tạo middleware của next-intl
const intlMiddleware = createMiddleware(routing);
const localeSet = new Set(routing.locales);

const decodeJwtPayload = (token: string): Record<string, unknown> => {
 const payload = token.split('.')[1];
 if (!payload) {
 throw new Error('Invalid token');
 }

 // JWT dùng base64url, cần normalize trước khi decode
 const base64 = payload.replace(/-/g, '+').replace(/_/g, '/');
 const padded = base64.padEnd(Math.ceil(base64.length / 4) * 4, '=');
 return JSON.parse(atob(padded)) as Record<string, unknown>;
};

const isExpired = (decodedPayload: Record<string, unknown>, leewaySeconds = 0) => {
 const exp = decodedPayload["exp"];
 if (typeof exp !== "number" || !Number.isFinite(exp)) return true;
 const nowSeconds = Math.floor(Date.now() / 1000);
 return nowSeconds >= exp - Math.max(0, leewaySeconds);
};

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

/**
 * Proxy xử lý đa chức năng:
 * 1. Đa ngôn ngữ (next-intl)
 * 2. Bảo mật (Phân quyền Admin)
 */
export default async function proxy(request: NextRequest) {
 const { pathname } = request.nextUrl;
 const locale = resolveLocale(pathname);
 const pathWithoutLocale = stripLocalePrefix(pathname);

 const isProtectedRoute = PROTECTED_PREFIXES.some((p) => matchesPrefix(pathWithoutLocale, p));
 const isAdminRoute = matchesPrefix(pathWithoutLocale, "/admin");

 const token = request.cookies.get("accessToken")?.value;
 let decodedPayload: Record<string, unknown> | null = null;
 let shouldClearCookies = false;

 if (token) {
  try {
   decodedPayload = decodeJwtPayload(token);
   if (isExpired(decodedPayload, 5)) {
    decodedPayload = null;
    shouldClearCookies = true;
   }
  } catch {
   decodedPayload = null;
   shouldClearCookies = true;
  }
 }

 if (isProtectedRoute) {
  if (!decodedPayload) {
   const loginUrl = new URL(`/${locale}/login`, request.url);
   const response = NextResponse.redirect(loginUrl);
   clearAuthCookies(response);
   return response;
  }
 }

 if (isAdminRoute) {
  // Nếu là admin route, yêu cầu role admin ngay trên Edge để tránh lộ portal.
  // (BE vẫn validate role lại ở phía API)
  const role =
   decodedPayload?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] ??
   decodedPayload?.["role"];

  if (String(role) !== "admin") {
   const homeUrl = new URL(`/${locale}`, request.url);
   return NextResponse.redirect(homeUrl);
  }
 }

 // Nếu hợp lệ hoặc không phải admin route, tiếp tục xử lý i18n
 const response = intlMiddleware(request);
 if (shouldClearCookies) {
  clearAuthCookies(response);
 }
 return response;
}

export const config = {
 // Matcher cho các đường dẫn cần xử lý i18n và auth
 matcher: ['/((?!api|_next|_vercel|.*\\..*).*)']
};
