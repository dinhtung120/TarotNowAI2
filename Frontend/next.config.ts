import createNextIntlPlugin from 'next-intl/plugin';

// Trỏ file cấu hình i18n
const withNextIntl = createNextIntlPlugin('./src/i18n/request.ts');

/** @type {import('next').NextConfig} */
const nextConfig = {
  // ...cấu hình Next.js (nếu có)
};

// Export sau khi wrap plugin
export default withNextIntl(nextConfig);
