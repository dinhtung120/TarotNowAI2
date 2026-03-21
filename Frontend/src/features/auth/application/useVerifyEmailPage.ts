'use client';

import { useEffect, useMemo, useState } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import toast from 'react-hot-toast';
import { useTranslations } from 'next-intl';
import { resendVerificationEmailAction, verifyEmailAction } from '@/actions/authActions';
import {
 createVerifyEmailSchema,
 type VerifyEmailFormValues,
} from '@/features/auth/domain/schemas';

export function useVerifyEmailPage() {
 const t = useTranslations('Auth');
 const [errorMsg, setErrorMsg] = useState('');
 const [success, setSuccess] = useState(false);
 const [resendTimer, setResendTimer] = useState(0);
 const [isResending, setIsResending] = useState(false);

 const verifySchema = useMemo(() => createVerifyEmailSchema(t), [t]);

 const {
  register,
  handleSubmit,
  control,
  formState: { errors, isSubmitting },
 } = useForm<VerifyEmailFormValues>({
  resolver: zodResolver(verifySchema),
 });

 const emailWatch = useWatch({ control, name: 'email' });

 useEffect(() => {
  if (resendTimer <= 0) return;

  const timer = window.setInterval(() => {
   setResendTimer((prev) => prev - 1);
  }, 1000);

  return () => {
   window.clearInterval(timer);
  };
 }, [resendTimer]);

 const handleResendOtp = async () => {
  if (!emailWatch || !emailWatch.includes('@')) {
   toast.error(t('verify.toast_invalid_email'));
   return;
  }

  setIsResending(true);
  try {
   const result = await resendVerificationEmailAction(emailWatch);
   if (result.success) {
    toast.success(t('verify.toast_resent_success'));
    setResendTimer(60);
    return;
   }

   toast.error(result.error || t('verify.toast_resent_fail'));
  } catch {
   toast.error(t('verify.toast_network_error'));
  } finally {
   setIsResending(false);
  }
 };

 const onSubmit = async (data: VerifyEmailFormValues) => {
  setErrorMsg('');

  try {
   const result = await verifyEmailAction(data);
   if (result.error) {
    setErrorMsg(result.error);
    return;
   }

   if (result.success) {
    setSuccess(true);
   }
  } catch {
   setErrorMsg(t('verify.error_unexpected'));
  }
 };

 return {
  t,
  errorMsg,
  success,
  resendTimer,
  isResending,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  handleResendOtp,
  onSubmit,
 };
}
