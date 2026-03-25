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
type MessageDictionary = Record<string, unknown>;
const FALLBACK_LOCALE: AppLocale = 'en';

const isAppLocale = (value: string): value is AppLocale =>
 routing.locales.includes(value as AppLocale);

const isRecord = (value: unknown): value is MessageDictionary =>
 typeof value === 'object' && value !== null && !Array.isArray(value);

const mergeMessages = (
 fallbackMessages: MessageDictionary,
 localeMessages: MessageDictionary
): MessageDictionary => {
 const merged: MessageDictionary = { ...fallbackMessages };

 for (const [key, value] of Object.entries(localeMessages)) {
  const fallbackValue = merged[key];
  merged[key] = isRecord(fallbackValue) && isRecord(value) ? mergeMessages(fallbackValue, value) : value;
 }

 return merged;
};

const loadMessages = async (locale: AppLocale): Promise<MessageDictionary> =>
 (await import(`../../messages/${locale}.json`)).default as MessageDictionary;

export default getRequestConfig(async ({ requestLocale }) => {
 const localeCandidate = await requestLocale;
 const locale = localeCandidate && isAppLocale(localeCandidate)
 ? localeCandidate
 : routing.defaultLocale;
 const fallbackMessages = await loadMessages(FALLBACK_LOCALE);
 const localeMessages = locale === FALLBACK_LOCALE ? fallbackMessages : await loadMessages(locale);

 return {
  locale,
  // Explicit fallback chain: locale -> en
  messages: mergeMessages(fallbackMessages, localeMessages)
 };
});
