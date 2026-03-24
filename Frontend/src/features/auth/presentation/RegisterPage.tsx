'use client';

import { Mail, Lock, User, AtSign, Calendar, Sparkles } from 'lucide-react';
import { Link } from '@/i18n/routing';
import AuthLayout from '@/shared/components/layout/AuthLayout';
import { Input, Button } from '@/shared/components/ui';
import { useRegisterPage } from '@/features/auth/application/useRegisterPage';
import { AuthErrorBanner } from '@/features/auth/presentation/components/AuthErrorBanner';
import { AuthSuccessCard } from '@/features/auth/presentation/components/AuthSuccessCard';

export default function RegisterPage() {
 const {
  t,
  errorMsg,
  success,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  onSubmit,
 } = useRegisterPage();

 if (success) {
  return (
   <AuthSuccessCard
    icon={<Mail className="w-10 h-10 text-[var(--success)]" />}
    title={t('register.success_title')}
    description={t('register.success_desc')}
    ctaHref="/verify-email"
    ctaLabel={t('register.success_cta')}
    glowClass="bg-[var(--success-bg)]"
    iconWrapperClass="bg-[var(--success-bg)] shadow-[0_0_30px_var(--success)]"
   />
  );
 }

 return (
  <AuthLayout title={t('register.title')} subtitle={t('register.subtitle')}>
   <AuthErrorBanner message={errorMsg} />

   <form onSubmit={handleSubmit(onSubmit)} className="space-y-4">
    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
     <Input
      label={t('register.email_label')}
      type="email"
      leftIcon={<Mail className="w-4 h-4" />}
      placeholder={t('register.email_placeholder')}
      error={errors.email?.message}
      {...register('email')}
     />
     <Input
      label={t('register.username_label')}
      type="text"
      leftIcon={<AtSign className="w-4 h-4" />}
      placeholder={t('register.username_placeholder')}
      error={errors.username?.message}
      {...register('username')}
     />
    </div>

    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
     <Input
      label={t('register.display_name_label')}
      type="text"
      leftIcon={<User className="w-4 h-4" />}
      placeholder={t('register.display_name_placeholder')}
      error={errors.displayName?.message}
      {...register('displayName')}
     />
     <Input
      label={t('register.dob_label')}
      type="date"
      leftIcon={<Calendar className="w-4 h-4 z-10" />}
      error={errors.dateOfBirth?.message}
      {...register('dateOfBirth')}
     />
    </div>

    <div className="grid grid-cols-1 md:grid-cols-2 gap-4">
     <Input
      label={t('register.password_label')}
      type="password"
      leftIcon={<Lock className="w-4 h-4" />}
      placeholder={t('register.password_placeholder')}
      error={errors.password?.message}
      {...register('password')}
     />
     <Input
      label={t('register.confirm_password_label')}
      type="password"
      leftIcon={<Lock className="w-4 h-4" />}
      placeholder={t('register.confirm_password_placeholder')}
      error={errors.confirmPassword?.message}
      {...register('confirmPassword')}
     />
    </div>

    <div className="pt-2">
     <label className="flex items-center gap-3 cursor-pointer group p-3 rounded-xl hover:tn-surface border border-transparent hover:tn-border-soft transition-all">
      <div className="relative flex items-center justify-center mt-0.5">
       <input
        type="checkbox"
        {...register('hasConsented')}
        className="peer appearance-none w-5 h-5 border border-[var(--purple-accent)]/50 rounded-md tn-overlay checked:bg-[var(--purple-accent)] transition-all cursor-pointer"
       />
       <svg
        className="absolute w-3 h-3 tn-text-ink pointer-events-none opacity-0 peer-checked:opacity-100 transition-opacity"
        viewBox="0 0 14 10"
        fill="none"
        xmlns="http://www.w3.org/2000/svg"
       >
        <path d="M1 5L5 9L13 1" stroke="currentColor" strokeWidth="2.5" strokeLinecap="round" strokeLinejoin="round" />
       </svg>
      </div>
      <span className="text-[11px] font-medium tn-text-secondary group-hover:tn-text-secondary transition-colors leading-none whitespace-nowrap">
       {t('register.consent_prefix')}{' '}
       <Link
        href="/legal/tos"
        className="text-[var(--purple-accent)] hover:underline border-b border-[var(--purple-accent)]/30 pb-0.5 align-middle"
       >
        {t('register.consent_terms')}
       </Link>{' '}
       {t('register.consent_and')}{' '}
       <Link
        href="/legal/privacy"
        className="text-[var(--purple-accent)] hover:underline border-b border-[var(--purple-accent)]/30 pb-0.5 align-middle"
       >
        {t('register.consent_privacy')}
       </Link>
      </span>
     </label>
     {errors.hasConsented && (
      <p className="text-[11px] text-[var(--danger)] font-medium mt-1 ml-4">{errors.hasConsented.message}</p>
     )}
    </div>

    <div className="pt-2">
     <Button
      type="submit"
      variant="brand"
      size="lg"
      fullWidth
      isLoading={isSubmitting}
      rightIcon={!isSubmitting && <Sparkles className="w-5 h-5 ml-2" />}
     >
      {t('register.cta')}
     </Button>
    </div>
   </form>

   <p className="mt-8 text-center text-sm tn-text-muted font-medium">
    {t('register.footer_prompt')}{' '}
    <Link
     href="/login"
     className="inline-flex items-center min-h-11 px-1 text-[var(--purple-accent)] font-bold hover:tn-text-primary transition-colors"
    >
     {t('register.footer_link')}
    </Link>
   </p>
  </AuthLayout>
 );
}
