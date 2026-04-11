'use client';

import { useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { useSearchParams } from 'next/navigation';
import { resetPasswordAction } from '@/features/auth/application/actions';
import {
 createResetPasswordSchema,
 type ResetPasswordFormValues,
} from '@/features/auth/domain/schemas';

/**
 * Hook quản lý nghiệp vụ cho trang Đặt lại mật khẩu (Reset Password).
 * Tự động lấy email từ URL để điền sẵn cho người dùng.
 */
export function useResetPasswordPage() {
 const t = useTranslations('Auth');
 const searchParams = useSearchParams();
 const [errorMsg, setErrorMsg] = useState('');
 const [success, setSuccess] = useState(false);

 // Lấy email từ query parameter để tự động điền form
 const initialEmail = searchParams.get('email') || '';

 const resetPasswordSchema = useMemo(() => createResetPasswordSchema(t), [t]);

 const {
  register,
  handleSubmit,
  formState: { errors, isSubmitting },
 } = useForm<ResetPasswordFormValues>({
  resolver: zodResolver(resetPasswordSchema),
  defaultValues: {
    email: initialEmail,
  },
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
