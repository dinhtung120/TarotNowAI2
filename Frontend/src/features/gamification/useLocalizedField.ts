import { useLocale } from 'next-intl';

export function useLocalizedField() {
  const locale = useLocale();

  const localize = (viField: string | null | undefined, enField: string | null | undefined): string => {
    // Nếu ngôn ngữ hiện tại không phải tiếng Việt (ví dụ: en, zh), ưu tiên hiển thị tiếng Anh.
    // Nếu không có tiếng Anh, fallback sang tiếng Việt.
    if (locale !== 'vi') {
      return enField || viField || '';
    }
    
    // Nếu ngôn ngữ là tiếng Việt, ưu tiên tiếng Việt.
    return viField || enField || '';
  };

  return { localize };
}
