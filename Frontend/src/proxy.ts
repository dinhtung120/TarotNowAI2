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

const resolveLocale = (pathname: string) => {
 const maybeLocale = pathname.split('/')[1];
 if (maybeLocale && localeSet.has(maybeLocale as (typeof routing.locales)[number])) {
 return maybeLocale;
 }

 return routing.defaultLocale;
};

/**
 * Proxy xử lý đa chức năng:
 * 1. Đa ngôn ngữ (next-intl)
 * 2. Bảo mật (Phân quyền Admin)
 */
export default async function proxy(request: NextRequest) {
 const { pathname } = request.nextUrl;

 // Kiểm tra nếu là route admin
 // Hỗ trợ cả /admin, /vi/admin, /en/admin, v.v.
 const isAdminRoute = /^\/([a-z]{2}\/)?admin(\/.*)?$/.test(pathname);

 if (isAdminRoute) {
 const locale = resolveLocale(pathname);

 // Lấy accessToken từ cookie
 const token = request.cookies.get('accessToken')?.value;

 if (!token) {
 // Nếu không có token, redirect về trang login
 // Đảm bảo không redirect lặp nếu đã ở trang login (mặc dù matcher đã loại trừ nhưng vẫn check cho chắc)
 const loginUrl = new URL(`/${locale}/login`, request.url);
 return NextResponse.redirect(loginUrl);
 }

 try {
 // Giải mã JWT để kiểm tra Role phía Client (BE vẫn validate lại kỹ hơn)
 const decodedPayload = decodeJwtPayload(token);
 const role = decodedPayload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || decodedPayload["role"];

 if (role !== 'admin') {
 // Nếu không phải admin, chặn ngay lập tức và đẩy về trang chủ
 const homeUrl = new URL(`/${locale}`, request.url);
 return NextResponse.redirect(homeUrl);
 }
 } catch (error) {
 console.error("Middleware Auth Error:", error);
 const loginUrl = new URL(`/${locale}/login`, request.url);
 return NextResponse.redirect(loginUrl);
 }
 }

 // Nếu hợp lệ hoặc không phải admin route, tiếp tục xử lý i18n
 return intlMiddleware(request);
}

export const config = {
 // Matcher cho các đường dẫn cần xử lý i18n và auth
 matcher: ['/((?!api|_next|_vercel|.*\\..*).*)']
};
