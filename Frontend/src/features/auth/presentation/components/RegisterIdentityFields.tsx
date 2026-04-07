import { AtSign, Calendar, Mail, User } from 'lucide-react';
import { useTranslations } from 'next-intl';
import type { FieldErrors, UseFormRegister } from 'react-hook-form';
import type { RegisterFormValues } from '@/features/auth/domain/schemas';
import { Input } from '@/shared/components/ui';
import { cn } from '@/lib/utils';

interface RegisterIdentityFieldsProps {
  errors: FieldErrors<RegisterFormValues>;
  register: UseFormRegister<RegisterFormValues>;
}

export default function RegisterIdentityFields({ errors, register }: RegisterIdentityFieldsProps) {
  const t = useTranslations('Auth');

  return (
    <>
      <div className={cn('tn-grid-1-2-md gap-4')}>
        <Input
          label={t('register.email_label')}
          type="email"
          leftIcon={<Mail className={cn('h-4 w-4')} />}
          placeholder={t('register.email_placeholder')}
          error={errors.email?.message}
          {...register('email')}
        />
        <Input
          label={t('register.username_label')}
          type="text"
          leftIcon={<AtSign className={cn('h-4 w-4')} />}
          placeholder={t('register.username_placeholder')}
          error={errors.username?.message}
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
          {...register('displayName')}
        />
        <Input
          label={t('register.dob_label')}
          type="date"
          leftIcon={<Calendar className={cn('z-10 h-4 w-4')} />}
          error={errors.dateOfBirth?.message}
          {...register('dateOfBirth')}
        />
      </div>
    </>
  );
}
