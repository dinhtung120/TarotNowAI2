'use client';

import { useEffect, useMemo, useState } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useSearchParams } from 'next/navigation';
import { useTranslations } from 'next-intl';
import { useQueryClient } from '@tanstack/react-query';
import { loginAction } from '@/features/auth/application/actions';
import { useAuthStore } from '@/store/authStore';
import { resolveLoginIdentityPrefill } from '@/features/auth/application/authFlowEmail';
import { useOptimizedNavigation } from '@/shared/application/gateways/useOptimizedNavigation';
import { invalidateClientSessionSnapshot } from '@/shared/application/gateways/clientSessionSnapshot';
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
 const queryClient = useQueryClient();
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
    /* 
     * Bước 1: Cập nhật trạng thái người dùng trong store client-side.
     * Điều này giúp UI client phản ứng ngay lập tức (ví dụ: đổi Navigation bar).
     */
    setAuth(result.data.user, result.data.expiresInSeconds);
    
    /* 
     * Bước 2: Xóa snapshot session client-side. 
     * Đảm bảo các lần check session tiếp theo sẽ fetch dữ liệu mới từ backend.
     */
    invalidateClientSessionSnapshot();

    // Reset cache user-state cũ để tránh hiển thị lệch danh tính ngay sau login.
    queryClient.clear();

    setIsRedirecting(true);
    navigation.replace('/');
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
