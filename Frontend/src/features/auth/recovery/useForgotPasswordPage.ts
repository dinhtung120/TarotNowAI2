'use client';

import { useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { forgotPasswordAction } from '@/features/auth/shared';
import {
 createForgotPasswordSchema,
 type ForgotPasswordFormValues,
} from '@/features/auth/shared/validation/schemas';

export function useForgotPasswordPage() {
 const t = useTranslations('Auth');
 const [errorMsg, setErrorMsg] = useState('');
 const [success, setSuccess] = useState(false);

 const forgotPasswordSchema = useMemo(() => createForgotPasswordSchema(t), [t]);

 const {
  register,
  handleSubmit,
  getValues,
  formState: { errors, isSubmitting },
 } = useForm<ForgotPasswordFormValues>({
  resolver: zodResolver(forgotPasswordSchema),
  mode: 'onBlur',
  reValidateMode: 'onBlur',
 });

 const onSubmit = async (data: ForgotPasswordFormValues) => {
  setErrorMsg('');

  try {
   const result = await forgotPasswordAction(data);
   if (result.error) {
    setErrorMsg(result.error);
    return;
   }

   setSuccess(true);
  } catch {
   setErrorMsg(t('forgot.error_unexpected'));
  }
 };

 return {
  t,
  errorMsg,
  success,
  register,
  handleSubmit,
  getValues,
  errors,
  isSubmitting,
  onSubmit,
 };
}
