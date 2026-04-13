import {
  DEFAULT_UPLOAD_RETRY_ATTEMPTS,
  DEFAULT_UPLOAD_RETRY_DELAY_MS,
} from '@/shared/media-upload/constants';
import { runWithRetry } from '@/shared/media-upload/retry';
import { uploadToPresignedUrlViaXhr } from '@/shared/media-upload/uploadViaXhr';

export interface UploadWithRetryParams {
  contentType: string;
  file: Blob;
  maxAttempts?: number;
  onProgress?: (percent: number) => void;
  signal?: AbortSignal;
  timeoutMs?: number;
  uploadUrl: string;
}

export async function uploadToR2WithRetry({
  contentType,
  file,
  maxAttempts = DEFAULT_UPLOAD_RETRY_ATTEMPTS,
  onProgress,
  signal,
  timeoutMs,
  uploadUrl,
}: UploadWithRetryParams): Promise<void> {
  await runWithRetry(
    async () => {
      await uploadToPresignedUrlViaXhr({
        contentType,
        file,
        onProgress,
        signal,
        timeoutMs,
        uploadUrl,
      });
    },
    {
      maxAttempts,
      baseDelayMs: DEFAULT_UPLOAD_RETRY_DELAY_MS,
      shouldRetry: shouldRetryUpload,
    },
  );
}

function shouldRetryUpload(error: unknown): boolean {
  if (!(error instanceof Error)) {
    return true;
  }

  if (error.message.includes('đã bị hủy')) {
    return false;
  }

  return true;
}
