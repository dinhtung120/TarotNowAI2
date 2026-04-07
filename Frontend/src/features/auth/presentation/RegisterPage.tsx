'use client';

import { Mail, Sparkles } from 'lucide-react';
import { Link } from '@/i18n/routing';
import { useRegisterPage } from '@/features/auth/application/useRegisterPage';
import { AuthErrorBanner } from '@/features/auth/presentation/components/AuthErrorBanner';
import { AuthSuccessCard } from '@/features/auth/presentation/components/AuthSuccessCard';
import RegisterConsentField from '@/features/auth/presentation/components/RegisterConsentField';
import RegisterIdentityFields from '@/features/auth/presentation/components/RegisterIdentityFields';
import RegisterPasswordFields from '@/features/auth/presentation/components/RegisterPasswordFields';
import AuthLayout from '@/shared/components/layout/AuthLayout';
import { Button } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

export default function RegisterPage() {
  const { t, errorMsg, success, register, handleSubmit, errors, isSubmitting, onSubmit } = useRegisterPage();

  if (success) {
    return (
      <AuthSuccessCard
        icon={<Mail className={cn('h-10 w-10 text-[var(--success)]')} />}
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
      <form onSubmit={handleSubmit(onSubmit)} method="post" className={cn('space-y-4')}>
        <RegisterIdentityFields register={register} errors={errors} />
        <RegisterPasswordFields register={register} errors={errors} />
        <RegisterConsentField register={register} errors={errors} />
        <Button
          type="submit"
          variant="brand"
          size="lg"
          fullWidth
          isLoading={isSubmitting}
          className={cn('mt-6 font-bold tracking-wide')}
          rightIcon={!isSubmitting ? <Sparkles className={cn('ml-2 h-5 w-5')} /> : undefined}
        >
          {t('register.cta')}
        </Button>
      </form>
      <p className={cn('mt-8 text-center text-sm font-medium text-[var(--text-muted)]')}>
        {t('register.footer_prompt')}{' '}
        <Link href="/login" className={cn('inline-flex min-h-11 items-center px-1 font-bold text-[var(--purple-accent)] transition-colors hover:text-[var(--text-primary)]')}>
          {t('register.footer_link')}
        </Link>
      </p>
    </AuthLayout>
  );
}
