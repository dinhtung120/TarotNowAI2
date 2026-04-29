'use client';

import type { QueryClient } from '@tanstack/react-query';
import type { ChangeEvent } from 'react';
import { toast } from 'react-hot-toast';
import { uploadProfileAvatar } from '@/features/profile/application/uploadProfileAvatar';
import { profileDetailQueryKey } from '@/features/profile/application/profileDetailQuery';
import { revokeObjectUrlIfNeeded } from '@/features/profile/application/useProfilePage.helpers';
import { useAuthStore } from '@/store/authStore';

interface HandleAvatarSelectParams {
 avatarPreview: string | null;
 event: ChangeEvent<HTMLInputElement>;
 queryClient: QueryClient;
 setAvatarPreview: (value: string | null) => void;
 setAvatarUploadProgress: (value: number) => void;
 setAvatarUploading: (value: boolean) => void;
 setErrorMsg: (value: string) => void;
 setSuccessMsg: (value: string) => void;
 t: (key: string) => string;
}

export async function handleProfileAvatarSelect(params: HandleAvatarSelectParams): Promise<void> {
 const {
  avatarPreview,
  event,
  queryClient,
  setAvatarPreview,
  setAvatarUploadProgress,
  setAvatarUploading,
  setErrorMsg,
  setSuccessMsg,
  t,
 } = params;

 const file = event.target.files?.[0];
 event.target.value = '';
 if (!file) {
  return;
 }

 const previousAvatarPreview = avatarPreview;
 const optimisticPreview = URL.createObjectURL(file);

 setAvatarPreview(optimisticPreview);
 setAvatarUploadProgress(0);
 setAvatarUploading(true);
 setErrorMsg('');
 try {
  const uploadResult = await uploadProfileAvatar({
   file,
   profileQueryKey: profileDetailQueryKey,
   queryClient,
   t,
   onProgress: setAvatarUploadProgress,
  });

  if (!uploadResult.success) {
   const errorMessage = uploadResult.error || t('avatar_upload_error') || 'Unable to upload avatar';
   setAvatarPreview(previousAvatarPreview);
   setErrorMsg(errorMessage);
   toast.error(errorMessage);
   revokeObjectUrlIfNeeded(optimisticPreview);
   return;
  }

  setAvatarPreview(uploadResult.avatarUrl);
  revokeObjectUrlIfNeeded(optimisticPreview);
  setSuccessMsg(uploadResult.message);
  toast.success(uploadResult.message);
  useAuthStore.getState().updateUser({ avatarUrl: uploadResult.avatarUrl });
 } catch {
  const errorMessage = t('avatar_upload_error') || 'Unable to upload avatar';
  setAvatarPreview(previousAvatarPreview);
  setErrorMsg(errorMessage);
  toast.error(errorMessage);
  revokeObjectUrlIfNeeded(optimisticPreview);
 } finally {
  setAvatarUploading(false);
  setAvatarUploadProgress(0);
 }
}
