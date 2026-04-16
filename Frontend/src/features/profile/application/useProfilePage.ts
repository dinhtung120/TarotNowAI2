'use client';

import { useEffect, useMemo, useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { zodResolver } from '@hookform/resolvers/zod';
import { useTranslations } from 'next-intl';
import { useForm } from 'react-hook-form';
import * as z from 'zod';
import { getProfileAction, updateProfileAction, type ProfileDto } from '@/features/profile/application/actions';
import { uploadProfileAvatar } from '@/features/profile/application/uploadProfileAvatar';
import { getMyReaderRequest, type MyReaderRequest } from '@/features/reader/public';
import { useRouter } from '@/i18n/routing';
import { useAuthGuard } from '@/shared/application/hooks/useAuthGuard';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { useAuthStore } from '@/store/authStore';
import { toast } from 'react-hot-toast';

interface ProfileFormValues {
  dateOfBirth: string;
  displayName: string;
}

export function useProfilePage() {
  const t = useTranslations('Profile');
  const tCommon = useTranslations('Common');
  const router = useRouter();
  const queryClient = useQueryClient();
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const user = useAuthStore((state) => state.user);
  const [successMsg, setSuccessMsg] = useState('');
  const [errorMsg, setErrorMsg] = useState('');
  const [avatarPreview, setAvatarPreview] = useState<string | null>(null);
  const [avatarUploadProgress, setAvatarUploadProgress] = useState(0);
  const [avatarUploading, setAvatarUploading] = useState(false);
  const profileQueryKey = userStateQueryKeys.profile.me();
  const isAdmin = user?.role === 'admin';
  const isTarotReader = user?.role === 'tarot_reader';

  const profileSchema = useMemo(() => z.object({
    displayName: z.string().min(2, t('validation.display_name_min')),
    dateOfBirth: z.string().refine((date) => !isNaN(Date.parse(date)), { message: t('validation.date_of_birth_invalid') }),
  }), [t]);

  const { register, handleSubmit, setValue, formState: { errors, isSubmitting } } = useForm<ProfileFormValues>({
    resolver: zodResolver(profileSchema),
  });

  useAuthGuard(isAuthenticated);

  const profileQuery = useQuery<{ profile: ProfileDto | null; error: string }>({
    queryKey: profileQueryKey,
    enabled: isAuthenticated,
    queryFn: async () => {
      const result = await getProfileAction();
      return result.success ? { profile: result.data ?? null, error: '' } : { profile: null, error: result.error };
    },
  });

  useEffect(() => {
    const payload = profileQuery.data;
    if (!payload || payload.error || !payload.profile) return;

    setValue('displayName', payload.profile.displayName);
    setAvatarPreview(payload.profile.avatarUrl || null);
    if (!payload.profile.dateOfBirth) return;

    const dateObj = new Date(payload.profile.dateOfBirth);
    const yyyy = dateObj.getFullYear();
    const mm = String(dateObj.getMonth() + 1).padStart(2, '0');
    const dd = String(dateObj.getDate()).padStart(2, '0');
    setValue('dateOfBirth', `${yyyy}-${mm}-${dd}`);
  }, [profileQuery.data, setValue]);

  const readerRequestQuery = useQuery<MyReaderRequest | null>({
    queryKey: userStateQueryKeys.reader.myRequest(),
    enabled: isAuthenticated && !!user && !isTarotReader && !isAdmin,
    queryFn: async () => {
      const result = await getMyReaderRequest();
      return result.success ? result.data ?? null : null;
    },
  });

  const onSubmit = async (data: ProfileFormValues) => {
    setSuccessMsg('');
    setErrorMsg('');

    try {
      const result = await updateProfileAction({
        displayName: data.displayName,
        avatarUrl: avatarPreview || null,
        dateOfBirth: new Date(data.dateOfBirth).toISOString(),
      });
      if (!result.success) {
        setErrorMsg(result.error);
        return;
      }

      setSuccessMsg(t('successMsg'));
      useAuthStore.getState().updateUser({ displayName: data.displayName, avatarUrl: avatarPreview || null });
      await queryClient.invalidateQueries({ queryKey: profileQueryKey });
    } catch {
      setErrorMsg(t('errorSave'));
    }
  };

  const handleAvatarSelect = async (event: React.ChangeEvent<HTMLInputElement>) => {
    const file = event.target.files?.[0];
    event.target.value = '';
    if (!file) return;

    const previousAvatarPreview = avatarPreview;
    const optimisticPreview = URL.createObjectURL(file);

    setAvatarPreview(optimisticPreview);
    setAvatarUploadProgress(0);
    setAvatarUploading(true);
    setErrorMsg('');
    try {
      const uploadResult = await uploadProfileAvatar({
        file,
        profileQueryKey,
        queryClient,
        t,
        onProgress: setAvatarUploadProgress,
      });

      if (!uploadResult.success) {
        const err = uploadResult.error || t('avatar_upload_error') || 'Không thể tải ảnh lên';
        setAvatarPreview(previousAvatarPreview);
        setErrorMsg(err);
        toast.error(err);
        revokeObjectUrlIfNeeded(optimisticPreview);
        return;
      }

      setAvatarPreview(uploadResult.avatarUrl);
      revokeObjectUrlIfNeeded(optimisticPreview);
      setSuccessMsg(uploadResult.message);
      toast.success(uploadResult.message);
      useAuthStore.getState().updateUser({ avatarUrl: uploadResult.avatarUrl });
    } catch {
      setAvatarPreview(previousAvatarPreview);
      const err = t('avatar_upload_error') || 'Không thể tải ảnh lên';
      setErrorMsg(err);
      toast.error(err);
      revokeObjectUrlIfNeeded(optimisticPreview);
    } finally {
      setAvatarUploading(false);
      setAvatarUploadProgress(0);
    }
  };

  return {
    t, tCommon, router, user,
    profileData: profileQuery.data?.profile ?? null,
    loading: profileQuery.isLoading || profileQuery.isFetching,
    successMsg,
    errorMsg: errorMsg || profileQuery.data?.error || '',
    readerRequest: readerRequestQuery.data ?? null,
    readerRequestLoading: readerRequestQuery.isLoading || readerRequestQuery.isFetching,
    register, handleSubmit, errors, isSubmitting, onSubmit,
    avatarPreview, avatarUploadProgress, avatarUploading, handleAvatarSelect,
  };
}

function revokeObjectUrlIfNeeded(value: string | null) {
  if (!value || !value.startsWith('blob:')) {
    return;
  }

  URL.revokeObjectURL(value);
}
