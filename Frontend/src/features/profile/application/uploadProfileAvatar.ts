import type { QueryClient } from '@tanstack/react-query';
import { compressAvatarImage } from '@/features/profile/application/compressAvatarImage';
import { UserImageValidationError } from '@/shared/media/validateImageForUpload';
import { postFormDataToApiV1 } from '@/shared/infrastructure/http/clientMultipartUpload';

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

  const failMsg = t('avatar_upload_error') || 'Không thể tải ảnh lên';
  const uploaded = await postFormDataToApiV1<{ avatarUrl?: string; AvatarUrl?: string }>('/profile/avatar', formData, {
    fallbackErrorMessage: failMsg,
    unauthorizedMessage: t('avatar_upload_error') || failMsg,
  });

  if (!uploaded.ok) {
    return {
      avatarUrl: null,
      error: uploaded.error,
      message: '',
      success: false,
    };
  }

  const raw = uploaded.data;
  const avatarUrl = (typeof raw.avatarUrl === 'string' ? raw.avatarUrl : null) ?? (typeof raw.AvatarUrl === 'string' ? raw.AvatarUrl : null);
  await queryClient.invalidateQueries({ queryKey: profileQueryKey });

  return {
    avatarUrl,
    error: '',
    message: t('avatar_upload_success') || 'Đã cập nhật ảnh đại diện',
    success: true,
  };
}
