import { Lock } from 'lucide-react';
import { useTranslations } from 'next-intl';
import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import type { RegisterFormValues } from '@/features/auth/domain/schemas';
import { Input } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface RegisterPasswordFieldsProps {
  errors: FieldErrors<RegisterFormValues>;
  register: UseFormRegister<RegisterFormValues>;
}

export default function RegisterPasswordFields({ errors, register }: RegisterPasswordFieldsProps) {
  const t = useTranslations('Auth');

  return (
    <div className={cn('grid grid-cols-1 gap-4 md:grid-cols-2')}>
      <Input
        label={t('register.password_label')}
        type="password"
        leftIcon={<Lock className={cn('h-4 w-4')} />}
        placeholder={t('register.password_placeholder')}
        error={errors.password?.message}
        {...register('password')}
      />
      <Input
        label={t('register.confirm_password_label')}
        type="password"
        leftIcon={<Lock className={cn('h-4 w-4')} />}
        placeholder={t('register.confirm_password_placeholder')}
        error={errors.confirmPassword?.message}
        {...register('confirmPassword')}
      />
    </div>
  );
}
