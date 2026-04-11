import { AtSign, Mail, User } from 'lucide-react';
import { useTranslations } from 'next-intl';
import { Controller, useFormContext } from 'react-hook-form';
import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import type { RegisterFormValues } from '@/features/auth/domain/schemas';
import { Input } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import RegisterBirthdayInput from './RegisterBirthdayInput';

interface RegisterIdentityFieldsProps {
  errors: FieldErrors<RegisterFormValues>;
  register: UseFormRegister<RegisterFormValues>;
}

export default function RegisterIdentityFields({ errors, register }: RegisterIdentityFieldsProps) {
  const t = useTranslations('Auth');
  const { control } = useFormContext<RegisterFormValues>();

  return (
    <>
      <div className={cn('tn-grid-1-2-md gap-4')}>
        <Input
          label={t('register.email_label')}
          type="email"
          leftIcon={<Mail className={cn('h-4 w-4')} />}
          placeholder={t('register.email_placeholder')}
          error={errors.email?.message}
          autoComplete="off"
          maxLength={100}
          {...register('email')}
        />
        <Input
          label={t('register.username_label')}
          type="text"
          leftIcon={<AtSign className={cn('h-4 w-4')} />}
          placeholder={t('register.username_placeholder')}
          error={errors.username?.message}
          autoComplete="off"
          maxLength={32}
          {...register('username')}
        />
      </div>
      <div className={cn('tn-grid-1-2-md gap-4')}>
        <Input
          label={t('register.display_name_label')}
          type="text"
          leftIcon={<User className={cn('h-4 w-4')} />}
          placeholder={t('register.display_name_placeholder')}
          error={errors.displayName?.message}
          autoComplete="off"
          maxLength={50}
          {...register('displayName')}
        />
        <Controller
          name="dateOfBirth"
          control={control}
          render={({ field }) => (
            <RegisterBirthdayInput
              label={t('register.dob_label')}
              error={errors.dateOfBirth?.message}
              value={field.value}
              onChange={field.onChange}
              onBlur={field.onBlur}
            />
          )}
        />
      </div>
    </>
  );
}
