import { useCallback, useEffect, useRef, useState } from 'react';
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
  messages?: Partial<UseCommunityImageUploadMessages>;
}

interface UploadImageResult {
  error: string;
  success: boolean;
}

interface UseCommunityImageUploadMessages {
  alreadyUploading: string;
  prepareFailed: string;
  uploadFailed: string;
}

const DEFAULT_MESSAGES: UseCommunityImageUploadMessages = {
  alreadyUploading: 'An upload is already in progress. Please wait.',
  prepareFailed: 'Unable to prepare the selected image.',
  uploadFailed: 'Unable to upload image.',
};

export function useCommunityImageUpload({
  contextType,
  getContent,
  setContent,
  messages,
}: UseCommunityImageUploadParams) {
  const [contextDraftId, setContextDraftId] = useState(generateDraftId());
  const [isUploading, setIsUploading] = useState(false);
  const [uploadProgress, setUploadProgress] = useState(0);
  const inFlightRef = useRef(false);
  const progressResetTimerRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const abortControllerRef = useRef<AbortController | null>(null);
  const mountedRef = useRef(true);
  const resolvedMessages: UseCommunityImageUploadMessages = {
    ...DEFAULT_MESSAGES,
    ...messages,
  };

  const uploadImage = useCallback(async (file: File): Promise<UploadImageResult> => {
    if (inFlightRef.current) {
      return { success: false, error: resolvedMessages.alreadyUploading };
    }

    let compressedFile: File;
    try {
      compressedFile = await compressImageForDirectUpload(file);
    } catch (error) {
      const message = error instanceof ImageUploadValidationError ? error.message : resolvedMessages.prepareFailed;
      return { success: false, error: message };
    }

    const placeholder = createMarkdownImagePlaceholder('uploading-image');
    if (mountedRef.current) {
      setContent(appendPlaceholderMarkdown(getContent(), placeholder));
    }
    inFlightRef.current = true;
    if (mountedRef.current) {
      setIsUploading(true);
      setUploadProgress(0);
    }
    const controller = new AbortController();
    abortControllerRef.current = controller;

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
        signal: controller.signal,
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

      if (mountedRef.current) {
        setContent(replacePlaceholderMarkdown(getContent(), placeholder, confirmed.data.publicUrl));
        setUploadProgress(100);
      }
      return { success: true, error: '' };
    } catch (error) {
      if (mountedRef.current) {
        rollbackPlaceholder(getContent, placeholder, setContent);
      }
      if (controller.signal.aborted) {
        return { success: false, error: resolvedMessages.uploadFailed };
      }

      return { success: false, error: error instanceof Error ? error.message : resolvedMessages.uploadFailed };
    } finally {
      inFlightRef.current = false;
      abortControllerRef.current = null;
      if (mountedRef.current) {
        setIsUploading(false);
      }
      if (progressResetTimerRef.current) {
        clearTimeout(progressResetTimerRef.current);
      }
      progressResetTimerRef.current = setTimeout(() => {
        if (mountedRef.current) {
          setUploadProgress(0);
        }
      }, 200);
    }
  }, [contextDraftId, contextType, getContent, resolvedMessages.alreadyUploading, resolvedMessages.prepareFailed, resolvedMessages.uploadFailed, setContent]);

  useEffect(() => () => {
    mountedRef.current = false;
    if (abortControllerRef.current) {
      abortControllerRef.current.abort();
      abortControllerRef.current = null;
    }
    if (progressResetTimerRef.current) {
      clearTimeout(progressResetTimerRef.current);
      progressResetTimerRef.current = null;
    }
  }, []);

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
