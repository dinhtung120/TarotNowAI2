'use client';

import { useEffect, useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useSearchParams } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { resolveLoginIdentityPrefill } from '@/features/auth/application/authFlowEmail';
import { useOptimizedNavigation } from '@/shared/application/gateways/useOptimizedNavigation';
import { useAuth } from '@/shared/hooks/useAuth';
import {
 getLocalStorageItem,
 removeLocalStorageItem,
 setLocalStorageItem,
} from '@/shared/application/gateways/browserStorage';
import { createLoginSchema, type LoginFormValues } from '@/features/auth/application/validation/schemas';

export function useLoginPage() {
 const t = useTranslations('Auth');
 const navigation = useOptimizedNavigation();
 const searchParams = useSearchParams();
 const { login } = useAuth();

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
   const result = await login(data);
   if (!result.success) {
    setErrorMsg(result.error);
    return;
   }

   if (result.data) {
    setIsRedirecting(true);
    navigation.replace('/');
    window.setTimeout(() => {
     navigation.refresh();
    }, 0);
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
