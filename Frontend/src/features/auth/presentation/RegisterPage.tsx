'use client';

import { FormProvider } from 'react-hook-form';
import { Sparkles } from 'lucide-react';
import { Link } from '@/i18n/routing';
import { useRegisterPage } from '@/features/auth/application/useRegisterPage';
import { AuthErrorBanner } from '@/features/auth/presentation/components/AuthErrorBanner';
import RegisterConsentField from '@/features/auth/presentation/components/RegisterConsentField';
import RegisterIdentityFields from '@/features/auth/presentation/components/RegisterIdentityFields';
import RegisterPasswordFields from '@/features/auth/presentation/components/RegisterPasswordFields';
import AuthLayout from '@/shared/components/layout/AuthLayout';
import { Button } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

export default function RegisterPage() {
  const { t, errorMsg, methods, handleSubmit, errors, isSubmitting, onSubmit } = useRegisterPage();

  return (
    <AuthLayout title={t('register.title')} subtitle={t('register.subtitle')}>
      <AuthErrorBanner message={errorMsg} />
      <FormProvider {...methods}>
        <form onSubmit={handleSubmit(onSubmit)} method="post" className={cn('space-y-1')} autoComplete="off">
          <RegisterIdentityFields register={methods.register} errors={errors} />
          <RegisterPasswordFields register={methods.register} errors={errors} />
          <RegisterConsentField register={methods.register} errors={errors} />
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
      </FormProvider>
      <p className={cn('mt-8 text-center text-sm font-medium tn-text-muted')}>
        {t('register.footer_prompt')}{' '}
        <Link href="/login" className={cn('inline-flex min-h-11 items-center px-1 font-bold tn-text-accent tn-hover-text-primary transition-colors')}>
          {t('register.footer_link')}
        </Link>
      </p>
    </AuthLayout>
  );
}
