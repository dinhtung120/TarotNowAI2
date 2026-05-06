export const LOCALE_OPTIONS = [
  { id: 'vi', flag: '🇻🇳' },
  { id: 'en', flag: '🇺🇸' },
  { id: 'zh', flag: '🇨🇳' },
] as const;

const LOCALE_ID_SET: ReadonlySet<string> = new Set(LOCALE_OPTIONS.map((option) => option.id));

export type LocaleId = (typeof LOCALE_OPTIONS)[number]['id'];

export function isValidLocale(locale: string | null): locale is LocaleId {
  return Boolean(locale && LOCALE_ID_SET.has(locale));
}
