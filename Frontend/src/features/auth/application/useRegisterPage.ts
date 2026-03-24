'use client';

import { useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { registerAction, resendVerificationEmailAction } from '@/features/auth/application/actions';
import { logger } from '@/shared/infrastructure/logging/logger';
import {
 createRegisterSchema,
 type RegisterFormValues,
} from '@/features/auth/domain/schemas';

export function useRegisterPage() {
 const t = useTranslations('Auth');
 const [errorMsg, setErrorMsg] = useState('');
 const [success, setSuccess] = useState(false);

 const registerSchema = useMemo(() => createRegisterSchema(t), [t]);

 const {
  register,
  handleSubmit,
  formState: { errors, isSubmitting },
 } = useForm<RegisterFormValues>({
  resolver: zodResolver(registerSchema),
 });

 const onSubmit = async (data: RegisterFormValues) => {
  setErrorMsg('');

  try {
   const submitData = {
    email: data.email,
    username: data.username,
    password: data.password,
    displayName: data.displayName,
    dateOfBirth: data.dateOfBirth,
    hasConsented: data.hasConsented,
   };

   const result = await registerAction(submitData);
   if (result.error) {
    setErrorMsg(result.error);
    return;
   }

   if (result.success) {
    setSuccess(true);
    void resendVerificationEmailAction(data.email).catch((error) => {
     logger.error('[Auth] resendVerificationEmailAction', error, { email: data.email });
    });
   }
  } catch {
   setErrorMsg(t('register.error_unexpected'));
  }
 };

 return {
  t,
  errorMsg,
  success,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  onSubmit,
 };
}
