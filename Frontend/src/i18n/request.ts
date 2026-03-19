/*
 * ===================================================================
 * COMPONENT/FILE: Cấu hình i18n Request (request.ts)
 * BỐI CẢNH (CONTEXT):
 *   Nơi thiết lập cấu hình đa ngôn ngữ phía Server-Side (Next.js App Router).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Lấy `locale` từ Request hiện tại thông qua `requestLocale`.
 *   - Tải động (Dynamic Import) các file JSON ngôn ngữ từ thư mục `/messages` 
 *     tương ứng với `locale` vừa trích xuất được.
 *   - Hoán đổi fallback về ngôn ngữ mặc định (Tiếng Việt) nếu `locale` không hợp lệ.
 * ===================================================================
 */
import { getRequestConfig } from 'next-intl/server';
import { routing } from './routing';

type AppLocale = (typeof routing.locales)[number];

const isAppLocale = (value: string): value is AppLocale =>
 routing.locales.includes(value as AppLocale);

export default getRequestConfig(async ({ requestLocale }) => {
 const localeCandidate = await requestLocale;
 const locale = localeCandidate && isAppLocale(localeCandidate)
 ? localeCandidate
 : routing.defaultLocale;

 return {
  locale,
  // Tải JSON messages tương ứng với ngôn ngữ
  messages: (await import(`../../messages/${locale}.json`)).default
 };
});
