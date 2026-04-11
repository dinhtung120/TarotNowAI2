
import { getRequestConfig } from 'next-intl/server';
import { routing } from './routing';
import { loadLocaleMessages, mergeMessages, type MessageDictionary } from './messages';

type AppLocale = (typeof routing.locales)[number];
const FALLBACK_LOCALE: AppLocale = 'vi';

const isAppLocale = (value: string): value is AppLocale =>
 routing.locales.includes(value as AppLocale);

const mergedMessagesCache = new Map<AppLocale, MessageDictionary>();

export default getRequestConfig(async ({ requestLocale }) => {
 const localeCandidate = await requestLocale;
 const locale = localeCandidate && isAppLocale(localeCandidate)
 ? localeCandidate
 : routing.defaultLocale;

 if (process.env.NODE_ENV === 'production') {
  const hit = mergedMessagesCache.get(locale);
  if (hit) {
   return { locale, messages: hit };
  }
 }

 const fallbackMessages = await loadLocaleMessages(FALLBACK_LOCALE);
 const localeMessages = locale === FALLBACK_LOCALE ? fallbackMessages : await loadLocaleMessages(locale);
 const messages = mergeMessages(fallbackMessages, localeMessages);

 if (process.env.NODE_ENV === 'production') {
  mergedMessagesCache.set(locale, messages);
 }

 return {
  locale,
  messages
 };
});
