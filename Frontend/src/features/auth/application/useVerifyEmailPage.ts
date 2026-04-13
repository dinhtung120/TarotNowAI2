'use client';

import { useEffect, useMemo, useState } from 'react';
import { useForm, useWatch } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import toast from 'react-hot-toast';
import { useSearchParams } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useRouter } from '@/i18n/routing';
import { resendVerificationEmailAction, verifyEmailAction } from '@/features/auth/application/actions';
import { resolveVerifyEmailPrefill, isValidAuthFlowEmail } from '@/features/auth/application/authFlowEmail';
import {
 createVerifyEmailSchema,
 type VerifyEmailFormValues,
} from '@/features/auth/domain/schemas';

export function useVerifyEmailPage() {
 const t = useTranslations('Auth');
 const router = useRouter();
 const searchParams = useSearchParams();
 const [errorMsg, setErrorMsg] = useState('');
 const [resendTimer, setResendTimer] = useState(0);
 const [isResending, setIsResending] = useState(false);

 const prefill = useMemo(
  () => resolveVerifyEmailPrefill(searchParams.get('email')),
  [searchParams]
 );
 const verifySchema = useMemo(() => createVerifyEmailSchema(t), [t]);

 const {
  register,
  handleSubmit,
  control,
  setValue,
  formState: { errors, isSubmitting },
 } = useForm<VerifyEmailFormValues>({
  resolver: zodResolver(verifySchema),
  defaultValues: {
   email: prefill.email,
  },
 });

 const emailWatch = useWatch({ control, name: 'email' });

 useEffect(() => {
  setValue('email', prefill.email);
 }, [prefill.email, setValue]);

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
  const normalizedEmail = emailWatch?.trim() ?? '';
  if (!isValidAuthFlowEmail(normalizedEmail)) {
   toast.error(t('verify.toast_invalid_email'));
   return;
  }

  setIsResending(true);
  try {
   const result = await resendVerificationEmailAction(normalizedEmail);
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
    const encodedEmail = encodeURIComponent(data.email.trim());
    router.replace(`/login?email=${encodedEmail}`);
   }
  } catch {
   setErrorMsg(t('verify.error_unexpected'));
  }
 };

 return {
  t,
  errorMsg,
  isEmailReadonly: prefill.isReadonly,
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
