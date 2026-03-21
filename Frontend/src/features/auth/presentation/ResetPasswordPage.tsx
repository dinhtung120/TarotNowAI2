'use client';

import { Mail, Lock, KeyRound, CheckCircle2 } from 'lucide-react';
import AuthLayout from '@/components/layout/AuthLayout';
import { Input, Button } from '@/components/ui';
import { useResetPasswordPage } from '@/features/auth/application/useResetPasswordPage';
import { AuthErrorBanner } from '@/features/auth/presentation/components/AuthErrorBanner';
import { AuthSuccessCard } from '@/features/auth/presentation/components/AuthSuccessCard';

export default function ResetPasswordPage() {
 const {
  t,
  errorMsg,
  success,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  onSubmit,
 } = useResetPasswordPage();

 if (success) {
  return (
   <AuthSuccessCard
    icon={<CheckCircle2 className="w-10 h-10 text-[var(--success)]" />}
    title={t('reset.success_title')}
    description={t('reset.success_desc')}
    ctaHref="/login"
    ctaLabel={t('reset.success_cta')}
    glowClass="bg-[var(--success-bg)]"
    iconWrapperClass="bg-[var(--success-bg)] shadow-[0_0_30px_var(--success)]"
   />
  );
 }

 return (
  <AuthLayout title={t('reset.title')} subtitle={t('reset.subtitle')}>
   <AuthErrorBanner message={errorMsg} />

   <form onSubmit={handleSubmit(onSubmit)} className="space-y-5">
    <Input
     label={t('reset.email_label')}
     type="email"
     leftIcon={<Mail className="w-5 h-5" />}
     placeholder={t('reset.email_placeholder')}
     error={errors.email?.message}
     {...register('email')}
    />

    <Input
     label={t('reset.otp_label')}
     type="text"
     leftIcon={<KeyRound className="w-5 h-5" />}
     placeholder={t('reset.otp_placeholder')}
     maxLength={6}
     error={errors.otpCode?.message}
     {...register('otpCode')}
     className="text-center font-bold tracking-widest text-lg"
    />

    <Input
     label={t('reset.password_label')}
     type="password"
     leftIcon={<Lock className="w-5 h-5" />}
     placeholder={t('reset.password_placeholder')}
     error={errors.newPassword?.message}
     {...register('newPassword')}
    />

    <div className="pt-2">
     <Button
      type="submit"
      variant="brand"
      size="lg"
      fullWidth
      isLoading={isSubmitting}
      rightIcon={!isSubmitting && <Lock className="w-4 h-4 ml-2" />}
     >
      {t('reset.cta')}
     </Button>
    </div>
   </form>
  </AuthLayout>
 );
}
