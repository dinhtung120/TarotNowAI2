'use client';

import { Mail, CheckCircle2, KeyRound, RefreshCcw } from 'lucide-react';
import AuthLayout from '@/components/layout/AuthLayout';
import { Input, Button } from '@/components/ui';
import { useVerifyEmailPage } from '@/features/auth/application/useVerifyEmailPage';
import { AuthErrorBanner } from '@/features/auth/presentation/components/AuthErrorBanner';
import { AuthSuccessCard } from '@/features/auth/presentation/components/AuthSuccessCard';

export default function VerifyEmailPage() {
 const {
  t,
  errorMsg,
  success,
  resendTimer,
  isResending,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  handleResendOtp,
  onSubmit,
 } = useVerifyEmailPage();

 if (success) {
  return (
   <AuthSuccessCard
    icon={<CheckCircle2 className="w-10 h-10 text-[var(--success)]" />}
    title={t('verify.success_title')}
    description={t('verify.success_desc')}
    ctaHref="/login"
    ctaLabel={t('verify.success_cta')}
    glowClass="bg-[var(--success-bg)]"
    iconWrapperClass="bg-[var(--success-bg)] shadow-[0_0_30px_var(--success)]"
   />
  );
 }

 return (
  <AuthLayout title={t('verify.title')} subtitle={t('verify.subtitle')}>
   <AuthErrorBanner message={errorMsg} />

   <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
    <Input
     label={t('verify.email_label')}
     type="email"
     leftIcon={<Mail className="w-5 h-5" />}
     placeholder={t('verify.email_placeholder')}
     error={errors.email?.message}
     {...register('email')}
    />

    <Input
     label={t('verify.otp_label')}
     type="text"
     leftIcon={<KeyRound className="w-5 h-5" />}
     placeholder={t('verify.otp_placeholder')}
     maxLength={6}
     error={errors.otpCode?.message}
     {...register('otpCode')}
     className="text-center font-bold tracking-widest text-xl"
    />

    <div className="pt-2">
     <Button
      type="submit"
      variant="brand"
      size="lg"
      fullWidth
      isLoading={isSubmitting}
      rightIcon={!isSubmitting && <CheckCircle2 className="w-5 h-5 ml-2" />}
     >
      {t('verify.cta')}
     </Button>
    </div>
   </form>

   <div className="text-center mt-6">
    <button
     onClick={handleResendOtp}
     disabled={resendTimer > 0 || isResending}
     className="text-[10px] font-black uppercase tracking-widest tn-text-muted hover:tn-text-primary transition-all flex items-center justify-center gap-2 mx-auto disabled:opacity-50 disabled:cursor-not-allowed group min-h-11 px-3 rounded-xl hover:tn-surface-soft"
    >
     <RefreshCcw
      className={`w-3.5 h-3.5 ${isResending ? 'animate-spin' : 'group-hover:rotate-180 transition-transform duration-500'}`}
     />
     {resendTimer > 0 ? t('verify.resend_with_timer', { seconds: resendTimer }) : t('verify.resend')}
    </button>
   </div>
  </AuthLayout>
 );
}
