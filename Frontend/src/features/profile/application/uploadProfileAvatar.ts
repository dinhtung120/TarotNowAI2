import type { QueryClient } from '@tanstack/react-query';
import { compressAvatarImage } from '@/features/profile/application/compressAvatarImage';
import { uploadAvatarAction } from '@/features/profile/application/actions/upload-avatar';

interface UploadProfileAvatarArgs {
  file: File;
  profileQueryKey: readonly string[];
  queryClient: QueryClient;
  t: (key: string) => string;
}

interface UploadProfileAvatarResult {
  avatarUrl: string | null;
  error: string;
  message: string;
  success: boolean;
}

export async function uploadProfileAvatar({ file, profileQueryKey, queryClient, t }: UploadProfileAvatarArgs): Promise<UploadProfileAvatarResult> {
  const compressedFile = await compressAvatarImage(file);
  const formData = new FormData();
  formData.append('file', compressedFile, compressedFile.name);

  const result = await uploadAvatarAction(formData);
  if (!result.success) {
    return {
      avatarUrl: null,
      error: result.error,
      message: '',
      success: false,
    };
  }

  const avatarUrl = result.data?.avatarUrl || null;
  await queryClient.invalidateQueries({ queryKey: profileQueryKey });

  return {
    avatarUrl,
    error: '',
    message: t('avatar_upload_success') || 'Đã cập nhật ảnh đại diện',
    success: true,
  };
}
