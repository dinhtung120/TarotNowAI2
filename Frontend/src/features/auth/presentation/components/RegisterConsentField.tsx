import { useTranslations } from 'next-intl';
import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import { Link } from '@/i18n/routing';
import type { RegisterFormValues } from '@/features/auth/domain/schemas';
import { cn } from '@/lib/utils';

interface RegisterConsentFieldProps {
  errors: FieldErrors<RegisterFormValues>;
  register: UseFormRegister<RegisterFormValues>;
}

export default function RegisterConsentField({ errors, register }: RegisterConsentFieldProps) {
  const t = useTranslations('Auth');

  return (
    <div className={cn('pt-2')}>
      <label className={cn('group flex cursor-pointer flex-col gap-2 rounded-xl border border-transparent p-3 transition-all hover:border-white/10 hover:bg-white/5')}>
        <div className={cn('flex min-h-11 items-center gap-3')}>
          <div className={cn('relative flex h-11 w-11 items-center justify-center')}>
            <input type="checkbox" {...register('hasConsented')} className={cn('peer absolute inset-0 h-11 w-11 cursor-pointer opacity-0')} />
            <span className={cn('pointer-events-none h-5 w-5 rounded-md border border-[var(--purple-accent)]/50 bg-white/5 transition-all peer-checked:bg-[var(--purple-accent)]')} />
            <svg className={cn('pointer-events-none absolute h-3 w-3 text-white opacity-0 transition-opacity peer-checked:opacity-100')} viewBox="0 0 14 10" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round" />
            </svg>
          </div>
          <span className={cn('text-[11px] font-medium leading-snug text-[var(--text-secondary)] transition-colors group-hover:text-[var(--text-primary)]')}>
            {t('register.consent_prefix')}
          </span>
        </div>
        <div className={cn('ml-8 flex flex-wrap items-center gap-2')}>
          <Link href="/legal/tos" className={cn('inline-flex min-h-11 items-center border-b border-[var(--purple-accent)]/30 px-2 text-[var(--purple-accent)] hover:underline')}>
            {t('register.consent_terms')}
          </Link>
          <span className={cn('text-[11px] font-medium text-[var(--text-secondary)]')}>{t('register.consent_and')}</span>
          <Link href="/legal/privacy" className={cn('inline-flex min-h-11 items-center border-b border-[var(--purple-accent)]/30 px-2 text-[var(--purple-accent)] hover:underline')}>
            {t('register.consent_privacy')}
          </Link>
        </div>
      </label>
      {errors.hasConsented ? <p className={cn('ml-4 mt-1 text-[11px] font-medium text-[var(--danger)]')}>{errors.hasConsented.message}</p> : null}
    </div>
  );
}
