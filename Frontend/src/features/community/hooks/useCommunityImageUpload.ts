import { useCallback, useState } from 'react';
import {
  appendPlaceholderMarkdown,
  compressImageForDirectUpload,
  confirmCommunityImageUpload,
  createMarkdownImagePlaceholder,
  ImageUploadValidationError,
  presignCommunityImageUpload,
  removePlaceholderMarkdown,
  replacePlaceholderMarkdown,
  uploadToR2WithRetry,
  type CommunityImageContextType,
} from '@/shared/media-upload';

interface UseCommunityImageUploadParams {
  contextType: CommunityImageContextType;
  getContent: () => string;
  setContent: (nextValue: string) => void;
}

interface UploadImageResult {
  error: string;
  success: boolean;
}

export function useCommunityImageUpload({
  contextType,
  getContent,
  setContent,
}: UseCommunityImageUploadParams) {
  const [contextDraftId, setContextDraftId] = useState(generateDraftId());
  const [isUploading, setIsUploading] = useState(false);
  const [uploadProgress, setUploadProgress] = useState(0);

  const uploadImage = useCallback(async (file: File): Promise<UploadImageResult> => {
    if (isUploading) {
      return { success: false, error: 'Đang tải ảnh lên. Vui lòng đợi hoàn tất.' };
    }

    let compressedFile: File;
    try {
      compressedFile = await compressImageForDirectUpload(file);
    } catch (error) {
      const message = error instanceof ImageUploadValidationError ? error.message : 'Không thể xử lý ảnh trước khi upload.';
      return { success: false, error: message };
    }

    const placeholder = createMarkdownImagePlaceholder('uploading-image');
    setContent(appendPlaceholderMarkdown(getContent(), placeholder));
    setIsUploading(true);
    setUploadProgress(0);

    try {
      const presigned = await presignCommunityImageUpload({
        contextType,
        contextDraftId,
        sizeBytes: compressedFile.size,
      });
      if (!presigned.ok) {
        rollbackPlaceholder(getContent, placeholder, setContent);
        return { success: false, error: presigned.error };
      }

      await uploadToR2WithRetry({
        uploadUrl: presigned.data.uploadUrl,
        contentType: compressedFile.type,
        file: compressedFile,
        onProgress: setUploadProgress,
      });

      const confirmed = await confirmCommunityImageUpload({
        contextType,
        contextDraftId,
        objectKey: presigned.data.objectKey,
        publicUrl: presigned.data.publicUrl,
        uploadToken: presigned.data.uploadToken,
      });
      if (!confirmed.ok) {
        rollbackPlaceholder(getContent, placeholder, setContent);
        return { success: false, error: confirmed.error };
      }

      setContent(replacePlaceholderMarkdown(getContent(), placeholder, confirmed.data.publicUrl));
      setUploadProgress(100);
      return { success: true, error: '' };
    } catch (error) {
      rollbackPlaceholder(getContent, placeholder, setContent);
      return { success: false, error: error instanceof Error ? error.message : 'Không thể upload ảnh.' };
    } finally {
      setIsUploading(false);
      setTimeout(() => setUploadProgress(0), 200);
    }
  }, [contextDraftId, contextType, getContent, isUploading, setContent]);

  return {
    contextDraftId,
    isUploading,
    resetContextDraftId: () => setContextDraftId(generateDraftId()),
    uploadProgress,
    uploadImage,
  };
}

function rollbackPlaceholder(
  getContent: () => string,
  placeholder: ReturnType<typeof createMarkdownImagePlaceholder>,
  setContent: (nextValue: string) => void,
) {
  setContent(removePlaceholderMarkdown(getContent(), placeholder));
}

function generateDraftId(): string {
  if (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function') {
    return crypto.randomUUID();
  }

  return `${Date.now()}-${Math.random().toString(16).slice(2)}`;
}
