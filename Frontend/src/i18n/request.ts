
import { getRequestConfig } from 'next-intl/server';
import { routing } from './routing';
import { loadLocaleMessages, mergeMessages } from './messages';

type AppLocale = (typeof routing.locales)[number];
const FALLBACK_LOCALE: AppLocale = 'vi';

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
  messages: mergeMessages(fallbackMessages, localeMessages)
 };
});
