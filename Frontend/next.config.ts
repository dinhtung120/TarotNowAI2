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

/** @type {import('next').NextConfig} */
const nextConfig = {
  // ...cấu hình Next.js (nếu có)
};

// Export sau khi wrap plugin
export default withNextIntl(nextConfig);
