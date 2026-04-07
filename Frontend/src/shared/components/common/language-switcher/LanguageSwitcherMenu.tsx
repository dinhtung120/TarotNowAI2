import type { LocaleId } from '@/shared/components/common/language-switcher/localeConfig';
import { LOCALE_OPTIONS } from '@/shared/components/common/language-switcher/localeConfig';
import LanguageSwitcherOption from '@/shared/components/common/language-switcher/LanguageSwitcherOption';
import { cn } from '@/lib/utils';

interface LanguageSwitcherMenuProps {
  currentLocale: string;
  getLocaleLabel: (localeId: LocaleId) => string;
  menuAria: string;
  menuTitle: string;
  onSelect: (localeId: LocaleId) => void;
}

export default function LanguageSwitcherMenu({ currentLocale, getLocaleLabel, menuAria, menuTitle, onSelect }: LanguageSwitcherMenuProps) {
  return (
    <div className={cn('absolute right-0 mt-2 w-48 rounded-2xl p-2 tn-panel shadow-xl')}>
      <div className={cn('px-2 pb-1 tn-text-10 font-black uppercase tn-tracking-018 tn-text-secondary')}>{menuTitle}</div>
      <div className={cn('space-y-1')} role="listbox" aria-label={menuAria}>
        {LOCALE_OPTIONS.map((option) => (
          <LanguageSwitcherOption key={option.id} id={option.id} flag={option.flag} label={getLocaleLabel(option.id)} isActive={option.id === currentLocale} onSelect={onSelect} />
        ))}
      </div>
    </div>
  );
}
