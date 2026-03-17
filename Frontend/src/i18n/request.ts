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
