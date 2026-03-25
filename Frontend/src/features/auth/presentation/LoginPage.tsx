'use client';

import { Mail, Lock } from 'lucide-react';
import { Link } from '@/i18n/routing';
import AuthLayout from '@/shared/components/layout/AuthLayout';
import { Input, Button } from '@/shared/components/ui';
import { useLoginPage } from '@/features/auth/application/useLoginPage';
import { AuthErrorBanner } from '@/features/auth/presentation/components/AuthErrorBanner';

export default function LoginPage() {
 const {
  t,
  errorMsg,
  isRedirecting,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  onSubmit,
 } = useLoginPage();

 return (
  <AuthLayout title={t('login.title')} subtitle={t('login.subtitle')}>
   <AuthErrorBanner message={errorMsg} />

   <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
    <Input
     label={t('login.email_or_username_label')}
     leftIcon={<Mail className="w-5 h-5" />}
     placeholder={t('login.email_or_username_placeholder')}
     error={errors.emailOrUsername?.message}
     {...register('emailOrUsername')}
    />

    <div className="space-y-1">
     <Input
      label={t('login.password_label')}
      type="password"
      leftIcon={<Lock className="w-5 h-5" />}
      placeholder={t('login.password_placeholder')}
      error={errors.password?.message}
      {...register('password')}
     />
     <div className="flex justify-end pt-1">
      <Link
       href="/forgot-password"
       className="inline-flex items-center min-h-11 px-1 text-[11px] font-bold text-[var(--purple-accent)] hover:tn-text-primary transition-colors uppercase tracking-widest"
      >
       {t('login.forgot_password')}
      </Link>
     </div>
    </div>

    <div className="flex items-center ml-1 py-1">
     <label className="flex items-center gap-3 min-h-11 cursor-pointer group">
      <div className="relative flex items-center justify-center w-11 h-11">
       <input
        type="checkbox"
        {...register('rememberMe')}
        className="peer absolute inset-0 w-11 h-11 opacity-0 cursor-pointer"
       />
       <span className="pointer-events-none w-5 h-5 border border-[var(--purple-accent)]/50 rounded tn-overlay-soft peer-checked:bg-[var(--purple-accent)] transition-all" />
       <svg
        className="absolute w-2.5 h-2.5 tn-text-ink pointer-events-none opacity-0 peer-checked:opacity-100 transition-opacity"
        viewBox="0 0 14 10"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
       >
        <path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round" />
       </svg>
      </div>
      <span className="text-sm font-medium tn-text-secondary group-hover:tn-text-secondary transition-colors select-none">
       {t('login.remember_me')}
      </span>
     </label>
    </div>

    <Button
     type="submit"
     variant="brand"
     size="lg"
     fullWidth
     isLoading={isSubmitting || isRedirecting}
     className="mt-2"
    >
     {t('login.cta')}
    </Button>
   </form>

   <p className="mt-8 text-center text-sm tn-text-muted font-medium">
    {t('login.footer_prompt')}{' '}
    <Link
     href="/register"
     className="inline-flex items-center min-h-11 px-1 text-[var(--purple-accent)] font-bold hover:tn-text-primary transition-colors"
    >
     {t('login.footer_link')}
    </Link>
   </p>
  </AuthLayout>
 );
}
