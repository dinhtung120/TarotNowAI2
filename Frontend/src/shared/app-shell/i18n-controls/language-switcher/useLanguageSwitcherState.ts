import { useEffect, useMemo, useRef, useState } from 'react';
import { LOCALE_OPTIONS, isValidLocale } from '@/shared/app-shell/i18n-controls/language-switcher/localeConfig';

interface UseLanguageSwitcherStateArgs {
  locale: string;
}

export function useLanguageSwitcherState({ locale }: UseLanguageSwitcherStateArgs) {
  const containerRef = useRef<HTMLDivElement>(null);
  const [isOpen, setIsOpen] = useState(false);

  useEffect(() => {
    if (!isOpen) return;

    const onKeyDown = (event: KeyboardEvent) => {
      if (event.key === 'Escape') setIsOpen(false);
    };

    const onMouseDown = (event: MouseEvent) => {
      if (containerRef.current && !containerRef.current.contains(event.target as Node)) setIsOpen(false);
    };

    document.addEventListener('keydown', onKeyDown);
    document.addEventListener('mousedown', onMouseDown);
    return () => {
      document.removeEventListener('keydown', onKeyDown);
      document.removeEventListener('mousedown', onMouseDown);
    };
  }, [isOpen]);

  const activeLocale = useMemo(() => {
    if (!isValidLocale(locale)) return LOCALE_OPTIONS[0];
    return LOCALE_OPTIONS.find((option) => option.id === locale) ?? LOCALE_OPTIONS[0];
  }, [locale]);

  return { activeLocale, containerRef, isOpen, setIsOpen };
}
