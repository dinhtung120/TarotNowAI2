import { Check } from 'lucide-react';
import type { LocaleId } from '@/features/home/shared/app-shell/i18n-controls/language-switcher/localeConfig';
import { cn } from '@/lib/utils';

interface LanguageSwitcherOptionProps {
  id: LocaleId;
  flag: string;
  isActive: boolean;
  label: string;
  onSelect: (localeId: LocaleId) => void;
}

export default function LanguageSwitcherOption({ id, flag, isActive, label, onSelect }: LanguageSwitcherOptionProps) {
  return (
    <button type="button" onClick={() => onSelect(id)} className={cn('min-h-11 w-full rounded-xl px-3 py-2.5 text-left text-xs font-semibold transition-all border', isActive ? 'tn-lang-option-active' : 'tn-lang-option-inactive')}>
      <span className={cn('inline-flex w-full items-center justify-between gap-3')}>
        <span className={cn('inline-flex items-center gap-2')}>
          <span className={cn('text-base leading-none')} aria-hidden="true">{flag}</span>
          <span>{label}</span>
        </span>
        {isActive ? <Check className={cn('h-3.5 w-3.5 tn-text-accent')} /> : null}
      </span>
    </button>
  );
}
