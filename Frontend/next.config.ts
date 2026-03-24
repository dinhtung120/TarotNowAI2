import createNextIntlPlugin from 'next-intl/plugin';

/*
 * ===================================================================
 * COMPONENT/FILE: Cấu hình Next.js (next.config.ts)
 * BỐI CẢNH (CONTEXT):
 *   File thiết lập vòng đời cài đặt chính của Next.js App Router.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Tích hợp Plugin `next-intl` để hỗ trợ Đa ngôn ngữ (i18n) cho toàn bộ ứng dụng.
 *   - Có thể thêm các cấu hình Webpack, Image Optimization, Redirects tại đây sau này.
 * ===================================================================
 */
const withNextIntl = createNextIntlPlugin('./src/i18n/request.ts');
const API_VERSION_PATH = '/api/v1';

function resolveApiOrigin(): string {
 const raw = process.env.NEXT_PUBLIC_API_URL?.trim() || 'http://localhost:5037/api/v1';
 const normalized = raw.replace(/\/+$/, '');
 if (normalized.endsWith(API_VERSION_PATH)) {
  return normalized.slice(0, -API_VERSION_PATH.length);
 }
 return normalized;
}

const apiOrigin = resolveApiOrigin();
const contentSecurityPolicy = [
 "default-src 'self'",
 "base-uri 'self'",
 "frame-ancestors 'none'",
 "form-action 'self'",
 "img-src 'self' data: blob: https:",
 "font-src 'self' data: https:",
 "style-src 'self' 'unsafe-inline' https:",
 "script-src 'self' 'unsafe-inline' 'unsafe-eval' https:",
 "connect-src 'self' https: ws: wss:",
].join('; ');

/** @type {import('next').NextConfig} */
const nextConfig = {
 poweredByHeader: false,
 async rewrites() {
  return [
   {
    source: '/api/v1/chat',
    destination: `${apiOrigin}/api/v1/chat`,
   },
   {
    source: '/api/v1/chat/:path*',
    destination: `${apiOrigin}/api/v1/chat/:path*`,
   },
  ];
 },
 async headers() {
  return [
   {
    source: '/:path*',
    headers: [
     { key: 'Content-Security-Policy', value: contentSecurityPolicy },
     { key: 'Referrer-Policy', value: 'strict-origin-when-cross-origin' },
     { key: 'X-Frame-Options', value: 'DENY' },
     { key: 'X-Content-Type-Options', value: 'nosniff' },
     { key: 'Permissions-Policy', value: 'camera=(), microphone=(), geolocation=()' },
    ],
   },
  ];
 },
};

// Export sau khi wrap plugin
export default withNextIntl(nextConfig);
