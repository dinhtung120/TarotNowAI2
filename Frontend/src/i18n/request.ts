/*
 * ===================================================================
 * COMPONENT/FILE: Cấu hình i18n Request (request.ts)
 * BỐI CẢNH (CONTEXT):
 *   Nơi thiết lập cấu hình đa ngôn ngữ phía Server-Side (Next.js App Router).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Lấy `locale` từ Request hiện tại thông qua `requestLocale`.
 *   - Nạp dictionary từ modular messages (`messages/{locale}/<domain>.json`)
 *     và merge về một object runtime duy nhất.
 *   - Hoán đổi fallback locale nếu `locale` request không hợp lệ.
 * ===================================================================
 */
import { getRequestConfig } from 'next-intl/server';
import { routing } from './routing';
import { loadLocaleMessages, mergeMessages } from './messages';

type AppLocale = (typeof routing.locales)[number];
const FALLBACK_LOCALE: AppLocale = 'en';

const isAppLocale = (value: string): value is AppLocale =>
 routing.locales.includes(value as AppLocale);

export default getRequestConfig(async ({ requestLocale }) => {
 const localeCandidate = await requestLocale;
 const locale = localeCandidate && isAppLocale(localeCandidate)
 ? localeCandidate
 : routing.defaultLocale;
 const fallbackMessages = await loadLocaleMessages(FALLBACK_LOCALE);
 const localeMessages = locale === FALLBACK_LOCALE ? fallbackMessages : await loadLocaleMessages(locale);

 return {
  locale,
  // Explicit fallback chain: locale -> en
  messages: mergeMessages(fallbackMessages, localeMessages)
 };
});
