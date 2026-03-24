'use client';

import { useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { resetPasswordAction } from '@/features/auth/application/actions';
import {
 createResetPasswordSchema,
 type ResetPasswordFormValues,
} from '@/features/auth/domain/schemas';

export function useResetPasswordPage() {
 const t = useTranslations('Auth');
 const [errorMsg, setErrorMsg] = useState('');
 const [success, setSuccess] = useState(false);

 const resetPasswordSchema = useMemo(() => createResetPasswordSchema(t), [t]);

 const {
  register,
  handleSubmit,
  formState: { errors, isSubmitting },
 } = useForm<ResetPasswordFormValues>({
  resolver: zodResolver(resetPasswordSchema),
 });

 const onSubmit = async (data: ResetPasswordFormValues) => {
  setErrorMsg('');

  try {
   const result = await resetPasswordAction(data);
   if (result.error) {
    setErrorMsg(result.error);
    return;
   }

   setSuccess(true);
  } catch {
   setErrorMsg(t('reset.error_unexpected'));
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
