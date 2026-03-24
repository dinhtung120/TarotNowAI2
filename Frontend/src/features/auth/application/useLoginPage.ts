'use client';

import { useEffect, useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { loginAction } from '@/features/auth/application/actions';
import { useAuthStore } from '@/store/authStore';
import { useRouter } from '@/i18n/routing';
import {
 getLocalStorageItem,
 removeLocalStorageItem,
 setLocalStorageItem,
} from '@/shared/infrastructure/storage/browserStorage';
import { createLoginSchema, type LoginFormValues } from '@/features/auth/domain/schemas';

export function useLoginPage() {
 const t = useTranslations('Auth');
 const router = useRouter();
 const setAuth = useAuthStore((state) => state.setAuth);

 const [errorMsg, setErrorMsg] = useState('');
 const [isRedirecting, setIsRedirecting] = useState(false);

 const loginSchema = useMemo(() => createLoginSchema(t), [t]);

 const {
  register,
  handleSubmit,
  setValue,
  formState: { errors, isSubmitting },
 } = useForm<LoginFormValues>({
  resolver: zodResolver(loginSchema),
  defaultValues: {
   rememberMe: false,
  },
 });

 useEffect(() => {
  const savedEmail = getLocalStorageItem('tarot_remembered_email');
  if (!savedEmail) return;

  setValue('emailOrUsername', savedEmail);
  setValue('rememberMe', true);
 }, [setValue]);

 useEffect(() => {
  router.prefetch('/');
  router.prefetch('/reading');
 }, [router]);

 const onSubmit = async (data: LoginFormValues) => {
  setErrorMsg('');

  if (data.rememberMe) {
   setLocalStorageItem('tarot_remembered_email', data.emailOrUsername);
  } else {
   removeLocalStorageItem('tarot_remembered_email');
  }

  try {
   const result = await loginAction(data);
   if (!result.success) {
    setErrorMsg(result.error);
    return;
   }

   if (result.data) {
    setAuth(result.data.user);
    setIsRedirecting(true);
    router.replace('/');
   }
  } catch {
   setErrorMsg(t('login.error_unexpected'));
   setIsRedirecting(false);
  }
 };

 return {
  t,
  errorMsg,
  isRedirecting,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  onSubmit,
 };
}
