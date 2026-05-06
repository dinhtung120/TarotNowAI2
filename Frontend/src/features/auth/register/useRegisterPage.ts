'use client';

import { useState, useMemo } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { registerAction } from '@/features/auth/shared';
import { useOptimizedNavigation } from '@/shared/application/gateways/useOptimizedNavigation';
import { usePublicRuntimePolicies } from '@/shared/application/hooks/usePublicRuntimePolicies';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';
import {
  createRegisterSchema,
  type RegisterFormValues,
} from '@/features/auth/shared/validation/schemas';

/**
 * Hook quản lý logic cho trang Đăng ký
 * Sử dụng react-hook-form kết hợp với zod để validation
 */
export function useRegisterPage() {
  const t = useTranslations('Auth');
  const navigation = useOptimizedNavigation();
  const [errorMsg, setErrorMsg] = useState('');
  const publicRuntimePoliciesQuery = usePublicRuntimePolicies();
  const minimumAge = publicRuntimePoliciesQuery.data?.auth.minimumAge ?? RUNTIME_POLICY_FALLBACKS.auth.minimumAge;

  // Tạo schema validation dựa trên ngôn ngữ hiện tại
  const registerSchema = useMemo(() => createRegisterSchema(t, minimumAge), [minimumAge, t]);

  // Khởi tạo form với các phương thức cần thiết
  const methods = useForm<RegisterFormValues>({
    resolver: zodResolver(registerSchema),
    mode: 'onBlur',
    reValidateMode: 'onBlur',
  });

  const {
    handleSubmit,
    formState: { errors, isSubmitting },
  } = methods;

  /**
   * Xử lý gửi form đăng ký
   */
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
        const encodedEmail = encodeURIComponent(data.email.trim());
        navigation.replace(`/verify-email?email=${encodedEmail}`);
      }
    } catch {
      setErrorMsg(t('register.error_unexpected'));
    }
  };

  return {
    t,
    errorMsg,
    methods, // Trả về toàn bộ object của useForm để dùng với FormProvider
    handleSubmit,
    errors,
    isSubmitting,
    onSubmit,
  };
}
