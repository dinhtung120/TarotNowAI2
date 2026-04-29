'use client';

import type { FieldErrors, UseFormHandleSubmit, UseFormRegister } from 'react-hook-form';
import { Mail } from 'lucide-react';
import { OptimizedLink as Link } from '@/shared/infrastructure/navigation/useOptimizedLink';
import { AuthErrorBanner } from '@/features/auth/presentation/components/AuthErrorBanner';
import { LoginPasswordField } from '@/features/auth/presentation/components/LoginPasswordField';
import { LoginRememberField } from '@/features/auth/presentation/components/LoginRememberField';
import type { LoginFormValues } from '@/features/auth/application/validation/schemas';
import { Button, Input } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface LoginFormProps {
 t: (key: string) => string;
 errorMsg: string;
 isSubmitting: boolean;
 isRedirecting: boolean;
 register: UseFormRegister<LoginFormValues>;
 handleSubmit: UseFormHandleSubmit<LoginFormValues>;
 errors: FieldErrors<LoginFormValues>;
 onSubmit: (data: LoginFormValues) => Promise<void>;
}

export default function LoginForm({
 t,
 errorMsg,
 isSubmitting,
 isRedirecting,
 register,
 handleSubmit,
 errors,
 onSubmit,
}: LoginFormProps) {
 return (
  <>
   <AuthErrorBanner message={errorMsg} />
   <form onSubmit={handleSubmit(onSubmit)} method="post" className={cn('space-y-1')}>
    <Input
     label={t('login.email_or_username_label')}
     leftIcon={<Mail className={cn('h-5', 'w-5')} />}
     placeholder={t('login.email_or_username_placeholder')}
     error={errors.emailOrUsername?.message}
     {...register('emailOrUsername')}
    />
    <div className={cn('space-y-1')}>
     <LoginPasswordField
      label={t('login.password_label')}
      placeholder={t('login.password_placeholder')}
      error={errors.password?.message}
      registerField={register('password')}
     />
     <div className={cn('flex', 'items-center', 'justify-between')}>
      <LoginRememberField label={t('login.remember_me')} register={register('rememberMe')} />
      <Link href="/forgot-password" className={cn('inline-flex', 'items-center', 'px-1', 'tn-text-11', 'font-bold', 'uppercase', 'tracking-widest', 'text-violet-400', 'transition-colors')}>
       {t('login.forgot_password')}
      </Link>
     </div>
    </div>

    <Button type="submit" variant="brand" size="lg" fullWidth isLoading={isSubmitting || isRedirecting} className={cn('mt-1')}>
     {t('login.cta')}
    </Button>
   </form>
   <p className={cn('mt-4', 'text-center', 'text-sm', 'font-medium', 'tn-text-muted')}>
    {t('login.footer_prompt')}{' '}
    <Link href="/register" className={cn('inline-flex', 'min-h-11', 'items-center', 'px-1', 'font-bold', 'text-violet-400', 'transition-colors')}>
     {t('login.footer_link')}
    </Link>
   </p>
  </>
 );
}
