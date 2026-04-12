import type { QueryClient } from '@tanstack/react-query';
import { compressAvatarImage } from '@/features/profile/application/compressAvatarImage';
import { UserImageValidationError } from '@/shared/media/validateImageForUpload';
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
  let compressedFile: File;
  try {
    compressedFile = await compressAvatarImage(file);
  } catch (err) {
    const message =
      err instanceof UserImageValidationError
        ? err.message
        : t('avatar_upload_error') || 'Không thể xử lý ảnh.';
    return {
      avatarUrl: null,
      error: message,
      message: '',
      success: false,
    };
  }
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
