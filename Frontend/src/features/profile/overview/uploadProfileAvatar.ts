import type { QueryClient } from '@tanstack/react-query';
import {
  compressImageForDirectUpload,
  confirmAvatarUpload,
  ImageUploadValidationError,
  presignAvatarUpload,
  uploadToR2WithRetry,
} from '@/shared/media-upload';

interface UploadProfileAvatarArgs {
  file: File;
  onProgress?: (percent: number) => void;
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

export async function uploadProfileAvatar({
  file,
  onProgress,
  profileQueryKey,
  queryClient,
  t,
}: UploadProfileAvatarArgs): Promise<UploadProfileAvatarResult> {
  const failMsg = t('avatar_upload_error') || 'Không thể tải ảnh lên';
  let compressedFile: File;

  try {
    compressedFile = await compressImageForDirectUpload(file);
  } catch (err) {
    const message =
      err instanceof ImageUploadValidationError
        ? err.message
        : t('avatar_upload_error') || 'Không thể xử lý ảnh.';
    return {
      avatarUrl: null,
      error: message,
      message: '',
      success: false,
    };
  }

  const presigned = await presignAvatarUpload(compressedFile.size);
  if (!presigned.ok) {
    return {
      avatarUrl: null,
      error: presigned.error || failMsg,
      message: '',
      success: false,
    };
  }

  onProgress?.(5);

  try {
    await uploadToR2WithRetry({
      uploadUrl: presigned.data.uploadUrl,
      file: compressedFile,
      contentType: compressedFile.type,
      onProgress,
    });
  } catch (error) {
    return {
      avatarUrl: null,
      error: error instanceof Error ? error.message : failMsg,
      message: '',
      success: false,
    };
  }

  const confirmed = await confirmAvatarUpload({
    objectKey: presigned.data.objectKey,
    publicUrl: presigned.data.publicUrl,
    uploadToken: presigned.data.uploadToken,
  });

  if (!confirmed.ok) {
    return {
      avatarUrl: null,
      error: confirmed.error || failMsg,
      message: '',
      success: false,
    };
  }

  await queryClient.invalidateQueries({ queryKey: profileQueryKey });
  onProgress?.(100);

  return {
    avatarUrl: confirmed.data.avatarUrl,
    error: '',
    message: t('avatar_upload_success') || 'Đã cập nhật ảnh đại diện',
    success: true,
  };
}
