import { Eye, EyeOff, Lock } from 'lucide-react';
import { useState } from 'react';
import { useTranslations } from 'next-intl';
import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import type { RegisterFormValues } from '@/features/auth/shared/schemas';
import { Input } from '@/shared/ui';
import { cn } from '@/lib/utils';

interface RegisterPasswordFieldsProps {
  errors: FieldErrors<RegisterFormValues>;
  register: UseFormRegister<RegisterFormValues>;
}

export default function RegisterPasswordFields({ errors, register }: RegisterPasswordFieldsProps) {
  const t = useTranslations('Auth');
  const [showPassword, setShowPassword] = useState(false);
  const [showConfirmPassword, setShowConfirmPassword] = useState(false);

  return (
    <div className={cn('tn-grid-1-2-md gap-4')}>
      <Input
        label={t('register.password_label')}
        type={showPassword ? 'text' : 'password'}
        leftIcon={<Lock className={cn('h-4 w-4')} />}
        rightElement={
          <button
            type="button"
            onClick={() => setShowPassword(!showPassword)}
            className={cn('tn-text-secondary hover:tn-text-primary transition-colors')}
            aria-label={showPassword ? 'Hide password' : 'Show password'}
          >
            {showPassword ? <EyeOff className={cn('h-4 w-4')} /> : <Eye className={cn('h-4 w-4')} />}
          </button>
        }
        error={errors.password?.message}
        autoComplete="new-password"
        maxLength={100}
        {...register('password')}
      />
      <Input
        label={t('register.confirm_password_label')}
        type={showConfirmPassword ? 'text' : 'password'}
        leftIcon={<Lock className={cn('h-4 w-4')} />}
        rightElement={
          <button
            type="button"
            onClick={() => setShowConfirmPassword(!showConfirmPassword)}
            className={cn('tn-text-secondary hover:tn-text-primary transition-colors')}
            aria-label={showConfirmPassword ? 'Hide password' : 'Show password'}
          >
            {showConfirmPassword ? <EyeOff className={cn('h-4 w-4')} /> : <Eye className={cn('h-4 w-4')} />}
          </button>
        }
        error={errors.confirmPassword?.message}
        autoComplete="new-password"
        maxLength={100}
        {...register('confirmPassword')}
      />
    </div>
  );
}
