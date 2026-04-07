import { useLocale } from 'next-intl';

export function useLocalizedField() {
  const locale = useLocale();

  const localize = (viField: string | null | undefined, enField: string | null | undefined): string => {
    if (locale !== 'vi') {
      return enField || viField || '';
    }
    
    return viField || enField || '';
  };

  return { localize };
}
