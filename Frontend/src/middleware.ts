import { NextRequest, NextResponse } from 'next/server';
import createMiddleware from 'next-intl/middleware';
import { routing } from './i18n/routing';

// Khởi tạo middleware của next-intl
const intlMiddleware = createMiddleware(routing);

/**
 * Middleware xử lý đa chức năng:
 * 1. Đa ngôn ngữ (next-intl)
 * 2. Bảo mật (Phân quyền Admin)
 */
export default async function middleware(request: NextRequest) {
    const { pathname } = request.nextUrl;

    // Kiểm tra nếu là route admin
    // Hỗ trợ cả /admin, /vi/admin, /en/admin, v.v.
    const isAdminRoute = pathname.match(/^\/([a-z]{2}\/)?admin(\/.*)?$/);

    if (isAdminRoute) {
        // Lấy accessToken từ cookie
        const token = request.cookies.get('accessToken')?.value;

        if (!token) {
            // Nếu không có token, redirect về trang login
            const locale = pathname.split('/')[1] || 'vi';
            // Đảm bảo không redirect lặp nếu đã ở trang login (mặc dù matcher đã loại trừ nhưng vẫn check cho chắc)
            const loginUrl = new URL(`/${locale}/login`, request.url);
            return NextResponse.redirect(loginUrl);
        }

        try {
            // Giải mã JWT để kiểm tra Role phía Client (BE vẫn validate lại kỹ hơn)
            const payloadBase64 = token.split('.')[1];
            if (!payloadBase64) throw new Error("Invalid token");
            
            const decodedPayload = JSON.parse(Buffer.from(payloadBase64, 'base64').toString());
            const role = decodedPayload["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"] || decodedPayload["role"];

            if (role !== 'admin') {
                // Nếu không phải admin, chặn ngay lập tức và đẩy về trang chủ
                const locale = pathname.split('/')[1] || 'vi';
                const homeUrl = new URL(`/${locale}`, request.url);
                return NextResponse.redirect(homeUrl);
            }
        } catch (error) {
            console.error("Middleware Auth Error:", error);
            const locale = pathname.split('/')[1] || 'vi';
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
