'use client';

import { useEffect, useMemo, useState } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import { useTranslations } from 'next-intl';
import { useRouter } from '@/i18n/routing';
import { useAuthStore } from '@/store/authStore';
import { getProfileAction, updateProfileAction, type ProfileDto } from '@/features/profile/application/actions';
import { getMyReaderRequest, type MyReaderRequest } from '@/features/reader/public';
import { useAuthGuard } from '@/shared/application/hooks/useAuthGuard';
import { useForm } from 'react-hook-form';
import { zodResolver } from '@hookform/resolvers/zod';
import * as z from 'zod';
import { compressAvatarImage } from '@/features/profile/application/compressAvatarImage';
import { uploadAvatarAction } from '@/features/profile/application/actions/upload-avatar';

interface ProfileFormValues {
 displayName: string;
 dateOfBirth: string;
}

export function useProfilePage() {
 const t = useTranslations('Profile');
 const tCommon = useTranslations('Common');
 const router = useRouter();
 const queryClient = useQueryClient();
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const user = useAuthStore((state) => state.user);
 const isAdmin = user?.role === 'admin';
 const isTarotReader = user?.role === 'tarot_reader';

 const [successMsg, setSuccessMsg] = useState('');
 const [errorMsg, setErrorMsg] = useState('');
 const [avatarPreview, setAvatarPreview] = useState<string | null>(null);
 const [avatarUploading, setAvatarUploading] = useState(false);
 const profileQueryKey = ['profile', 'me'] as const;

 const profileSchema = useMemo(
  () =>
   z.object({
    displayName: z.string().min(2, t('validation.display_name_min')),
    dateOfBirth: z.string().refine((date) => !isNaN(Date.parse(date)), {
     message: t('validation.date_of_birth_invalid'),
    }),
   }),
  [t]
 );

 const {
  register,
  handleSubmit,
  setValue,
  formState: { errors, isSubmitting },
 } = useForm<ProfileFormValues>({
  resolver: zodResolver(profileSchema),
 });

 useAuthGuard(isAuthenticated);

 const profileQuery = useQuery<{ profile: ProfileDto | null; error: string }>({
  queryKey: profileQueryKey,
  enabled: isAuthenticated,
  queryFn: async () => {
   const result = await getProfileAction();
   if (!result.success) {
    return { profile: null, error: result.error };
   }
   return { profile: result.data ?? null, error: '' };
  },
 });

 useEffect(() => {
  const payload = profileQuery.data;
  if (!payload) return;

  if (payload.error) return;
  if (!payload.profile) return;

  const data = payload.profile;
  setValue('displayName', data.displayName);
  setAvatarPreview(data.avatarUrl || null);

  if (data.dateOfBirth) {
   const dateObj = new Date(data.dateOfBirth);
   const yyyy = dateObj.getFullYear();
   const mm = String(dateObj.getMonth() + 1).padStart(2, '0');
   const dd = String(dateObj.getDate()).padStart(2, '0');
   setValue('dateOfBirth', `${yyyy}-${mm}-${dd}`);
  }
 }, [profileQuery.data, setValue]);

 const shouldLoadReaderRequest =
  isAuthenticated && !!user && !isTarotReader && !isAdmin;
 const readerRequestQuery = useQuery<MyReaderRequest | null>({
  queryKey: ['reader', 'my-request'],
  enabled: shouldLoadReaderRequest,
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
   useAuthStore.getState().updateUser({ 
     displayName: data.displayName, 
     avatarUrl: avatarPreview || null 
   });
   await queryClient.invalidateQueries({ queryKey: profileQueryKey });
  } catch {
   setErrorMsg(t('errorSave'));
  }
 };

 const handleAvatarSelect = async (event: React.ChangeEvent<HTMLInputElement>) => {
  const file = event.target.files?.[0];
  event.target.value = '';
  if (!file) return;

  // Giới hạn 10MB file ảnh nguyên bản như BE cấu hình
  if (file.size > 10 * 1024 * 1024) {
    setErrorMsg(t('validation.avatar_file_too_large') || 'Ảnh quá lớn (tối đa 10MB)');
    return;
  }

  setAvatarUploading(true);
  setErrorMsg('');
  
  try {
    const compressedFile = await compressAvatarImage(file);
    const formData = new FormData();
    formData.append('file', compressedFile, compressedFile.name);

    const result = await uploadAvatarAction(formData);

    if (!result.success) {
      setErrorMsg(result.error);
      return;
    }

    setAvatarPreview(result.data?.avatarUrl || null);
    setSuccessMsg(t('avatar_upload_success') || 'Đã cập nhật ảnh đại diện');
    
    // Đồng bộ lập tức Avatar URL mới vào Global AuthStore để Navbar hiển thị
    useAuthStore.getState().updateUser({ avatarUrl: result.data?.avatarUrl || null });

    // Yêu cầu tải lại profile để đồng bộ toàn cục avatar
    await queryClient.invalidateQueries({ queryKey: profileQueryKey });
  } catch (error) {
    setErrorMsg(t('avatar_upload_error') || 'Không thể tải ảnh lên');
  } finally {
    setAvatarUploading(false);
  }
 };

 return {
  t,
  tCommon,
  router,
  user,
  profileData: profileQuery.data?.profile ?? null,
  loading: profileQuery.isLoading || profileQuery.isFetching,
  successMsg,
  errorMsg: errorMsg || profileQuery.data?.error || '',
  readerRequest: readerRequestQuery.data ?? null,
  readerRequestLoading: readerRequestQuery.isLoading || readerRequestQuery.isFetching,
  register,
  handleSubmit,
  errors,
  isSubmitting,
  onSubmit,
  avatarPreview,
  avatarUploading,
  handleAvatarSelect
 };
}
