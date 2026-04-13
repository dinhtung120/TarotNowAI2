export interface UploadViaXhrParams {
  contentType: string;
  file: Blob;
  onProgress?: (percent: number) => void;
  signal?: AbortSignal;
  timeoutMs?: number;
  uploadUrl: string;
}

export async function uploadToPresignedUrlViaXhr({
  contentType,
  file,
  onProgress,
  signal,
  timeoutMs = 60_000,
  uploadUrl,
}: UploadViaXhrParams): Promise<void> {
  const isR2Upload = uploadUrl.includes('.r2.cloudflarestorage.com/');

  await new Promise<void>((resolve, reject) => {
    const xhr = new XMLHttpRequest();
    let completed = false;

    const finish = (callback: () => void) => {
      if (completed) {
        return;
      }

      completed = true;
      signal?.removeEventListener('abort', onAbortSignal);
      callback();
    };

    const fail = (message: string) => {
      finish(() => reject(new Error(message)));
    };

    const onAbortSignal = () => {
      xhr.abort();
    };

    xhr.open('PUT', uploadUrl, true);
    xhr.timeout = timeoutMs;
    xhr.setRequestHeader('Content-Type', contentType);

    xhr.upload.onprogress = (event) => {
      if (!onProgress || !event.lengthComputable) {
        return;
      }

      const percent = Math.max(0, Math.min(100, Math.round((event.loaded / event.total) * 100)));
      onProgress(percent);
    };

    xhr.onload = () => {
      const isSuccess = xhr.status >= 200 && xhr.status < 300;
      if (!isSuccess) {
        fail(`R2 upload failed (${xhr.status}).`);
        return;
      }

      finish(() => resolve());
    };

    xhr.onerror = () => {
      if (isR2Upload && xhr.status === 0) {
        fail('Upload bị chặn bởi CORS của R2. Vui lòng cấu hình Allowed Origins/Headers cho domain hiện tại.');
        return;
      }

      fail('Không thể upload media lên R2.');
    };
    xhr.ontimeout = () => fail('Upload media bị timeout.');
    xhr.onabort = () => fail('Upload media đã bị hủy.');

    signal?.addEventListener('abort', onAbortSignal, { once: true });
    xhr.send(file);
  });
}
