'use client';

import { useEffect, useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useSearchParams } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { loginAction } from '@/features/auth/application/actions';
import { useAuthStore } from '@/store/authStore';
import { useRouter } from '@/i18n/routing';
import { resolveLoginIdentityPrefill } from '@/features/auth/application/authFlowEmail';
import {
 getLocalStorageItem,
 removeLocalStorageItem,
 setLocalStorageItem,
} from '@/shared/infrastructure/storage/browserStorage';
import { createLoginSchema, type LoginFormValues } from '@/features/auth/domain/schemas';

export function useLoginPage() {
 const t = useTranslations('Auth');
 const router = useRouter();
 const searchParams = useSearchParams();
 const setAuth = useAuthStore((state) => state.setAuth);

 const [errorMsg, setErrorMsg] = useState('');
 const [isRedirecting, setIsRedirecting] = useState(false);
 const queryEmail = searchParams.get('email');
 const initialIdentity = useMemo(
  () => resolveLoginIdentityPrefill(queryEmail, null),
  [queryEmail]
 );

 const loginSchema = useMemo(() => createLoginSchema(t), [t]);

 const {
  register,
  handleSubmit,
  setValue,
  formState: { errors, isSubmitting },
 } = useForm<LoginFormValues>({
  resolver: zodResolver(loginSchema),
  defaultValues: {
   emailOrUsername: initialIdentity,
   rememberMe: false,
  },
 });

 useEffect(() => {
  const rememberedIdentity = getLocalStorageItem('tarot_remembered_email');
  const prefilledIdentity = resolveLoginIdentityPrefill(queryEmail, rememberedIdentity);
  if (!prefilledIdentity) return;

  setValue('emailOrUsername', prefilledIdentity);
  const shouldRemember = !initialIdentity && !!rememberedIdentity;
  setValue('rememberMe', shouldRemember);
 }, [queryEmail, initialIdentity, setValue]);

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
    setAuth(result.data.user, result.data.accessToken);
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
