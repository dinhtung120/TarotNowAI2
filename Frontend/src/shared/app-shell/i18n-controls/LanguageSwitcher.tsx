'use client';

import { ChevronDown } from 'lucide-react';
import { useLocale, useTranslations } from 'next-intl';
import { usePathname, useRouter } from '@/i18n/routing';
import LanguageSwitcherMenu from '@/shared/app-shell/i18n-controls/language-switcher/LanguageSwitcherMenu';
import { type LocaleId, isValidLocale } from '@/shared/app-shell/i18n-controls/language-switcher/localeConfig';
import { useLanguageSwitcherState } from '@/shared/app-shell/i18n-controls/language-switcher/useLanguageSwitcherState';
import { cn } from '@/lib/utils';

export default function LanguageSwitcher() {
  const router = useRouter();
  const pathname = usePathname();
  const locale = useLocale();
  const t = useTranslations('LanguageSwitcher');
  const { activeLocale, containerRef, isOpen, setIsOpen } = useLanguageSwitcherState({ locale });

  const getLocaleLabel = (localeId: LocaleId) => t(`locale_${localeId}`);
  const applyLocale = (localeId: LocaleId) => {
    if (!isValidLocale(localeId)) return;
    if (localeId !== locale) router.replace(pathname, { locale: localeId });
    setIsOpen(false);
  };

  return (
    <div ref={containerRef} className={cn('relative')}>
      <button type="button" onClick={() => setIsOpen((prev) => !prev)} className={cn('tn-panel inline-flex min-h-11 items-center gap-2 rounded-xl px-3 py-2 text-xs font-black uppercase tn-tracking-018 tn-text-primary transition-all tn-hover-surface-strong')} aria-label={t('current_aria', { language: getLocaleLabel(activeLocale.id) })} aria-haspopup="listbox" aria-expanded={isOpen}>
        <span className={cn('text-base leading-none')} aria-hidden="true">{activeLocale.flag}</span>
        <ChevronDown className={cn('h-3.5 w-3.5 transition-transform', isOpen ? 'rotate-180' : '')} />
      </button>
      {isOpen ? <LanguageSwitcherMenu currentLocale={locale} getLocaleLabel={getLocaleLabel} menuAria={t('options_aria')} menuTitle={t('menu_title')} onSelect={applyLocale} /> : null}
    </div>
  );
}
