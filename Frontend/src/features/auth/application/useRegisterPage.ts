'use client';

import { useState, useMemo } from 'react';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { registerAction, resendVerificationEmailAction } from '@/features/auth/application/actions';
import { logger } from '@/shared/infrastructure/logging/logger';
import {
  createRegisterSchema,
  type RegisterFormValues,
} from '@/features/auth/domain/schemas';

/**
 * Hook quản lý logic cho trang Đăng ký
 * Sử dụng react-hook-form kết hợp với zod để validation
 */
export function useRegisterPage() {
  const t = useTranslations('Auth');
  const [errorMsg, setErrorMsg] = useState('');
  const [success, setSuccess] = useState(false);

  // Tạo schema validation dựa trên ngôn ngữ hiện tại
  const registerSchema = useMemo(() => createRegisterSchema(t), [t]);

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
        setSuccess(true);
        // Tự động gửi email xác thực sau khi đăng ký thành công
        void resendVerificationEmailAction(data.email).catch((error) => {
          logger.error('[Auth] resendVerificationEmailAction', error, { email: data.email });
        });
      }
    } catch {
      setErrorMsg(t('register.error_unexpected'));
    }
  };

  return {
    t,
    errorMsg,
    success,
    methods, // Trả về toàn bộ object của useForm để dùng với FormProvider
    handleSubmit,
    errors,
    isSubmitting,
    onSubmit,
  };
}
