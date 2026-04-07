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
      <label className={cn('tn-consent-label group flex cursor-pointer flex-col gap-2 rounded-xl border border-transparent p-3 transition-all')}>
        <div className={cn('flex min-h-11 items-center gap-3')}>
          <div className={cn('relative flex h-11 w-11 items-center justify-center')}>
            <input type="checkbox" {...register('hasConsented')} className={cn('tn-consent-checkbox-input absolute inset-0 h-11 w-11 cursor-pointer opacity-0')} />
            <span className={cn('tn-consent-checkbox-box pointer-events-none h-5 w-5 rounded-md border tn-border-accent-50 bg-white/5 transition-all')} />
            <svg className={cn('tn-consent-checkbox-check pointer-events-none absolute h-3 w-3 text-white opacity-0 transition-opacity')} viewBox="0 0 14 10" fill="none" xmlns="http://www.w3.org/2000/svg">
              <path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round" />
            </svg>
          </div>
          <span className={cn('tn-consent-text tn-text-11 font-medium leading-snug tn-text-secondary transition-colors')}>
            {t('register.consent_prefix')}
          </span>
        </div>
        <div className={cn('ml-8 flex flex-wrap items-center gap-2')}>
          <Link href="/legal/tos" className={cn('inline-flex min-h-11 items-center border-b tn-border-accent-30 px-2 tn-text-accent tn-hover-underline')}>
            {t('register.consent_terms')}
          </Link>
          <span className={cn('tn-text-11 font-medium tn-text-secondary')}>{t('register.consent_and')}</span>
          <Link href="/legal/privacy" className={cn('inline-flex min-h-11 items-center border-b tn-border-accent-30 px-2 tn-text-accent tn-hover-underline')}>
            {t('register.consent_privacy')}
          </Link>
        </div>
      </label>
      {errors.hasConsented ? <p className={cn('ml-4 mt-1 tn-text-11 font-medium tn-text-danger')}>{errors.hasConsented.message}</p> : null}
    </div>
  );
}
