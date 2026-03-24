'use client';

import { Mail, ArrowLeft, Send } from 'lucide-react';
import { Link } from '@/i18n/routing';
import AuthLayout from '@/shared/components/layout/AuthLayout';
import { Input, Button } from '@/shared/components/ui';
import { useForgotPasswordPage } from '@/features/auth/application/useForgotPasswordPage';
import { AuthErrorBanner } from '@/features/auth/presentation/components/AuthErrorBanner';
import { AuthSuccessCard } from '@/features/auth/presentation/components/AuthSuccessCard';

export default function ForgotPasswordPage() {
 const {
  t,
  errorMsg,
  success,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  onSubmit,
 } = useForgotPasswordPage();

 if (success) {
  return (
   <AuthSuccessCard
    icon={<Mail className="w-10 h-10 text-[var(--info)]" />}
    title={t('forgot.success_title')}
    description={t('forgot.success_desc')}
    ctaHref="/reset-password"
    ctaLabel={t('forgot.success_cta')}
    glowClass="bg-[var(--info-bg)]"
    iconWrapperClass="bg-[var(--info-bg)] shadow-[0_0_30px_var(--info)]"
   />
  );
 }

 return (
  <AuthLayout title={t('forgot.title')} subtitle={t('forgot.subtitle')}>
   <div className="mb-6 flex justify-center">
    <Link
     href="/login"
     className="inline-flex items-center text-xs font-bold tn-text-secondary hover:tn-text-primary transition-colors group uppercase tracking-widest min-h-11 px-2"
    >
     <ArrowLeft className="w-4 h-4 mr-1.5 transform group-hover:-translate-x-1 transition-transform" />
     {t('forgot.back_to_login')}
    </Link>
   </div>

   <AuthErrorBanner message={errorMsg} />

   <form onSubmit={handleSubmit(onSubmit)} className="space-y-6">
    <Input
     label={t('forgot.email_label')}
     type="email"
     leftIcon={<Mail className="w-5 h-5" />}
     placeholder={t('forgot.email_placeholder')}
     error={errors.email?.message}
     {...register('email')}
    />

    <Button
     type="submit"
     variant="brand"
     size="lg"
     fullWidth
     isLoading={isSubmitting}
     rightIcon={!isSubmitting && <Send className="w-5 h-5 ml-2" />}
    >
     {t('forgot.cta')}
    </Button>
   </form>
  </AuthLayout>
 );
}
