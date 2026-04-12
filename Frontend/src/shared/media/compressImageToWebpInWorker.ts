import { validateImageForUpload } from '@/shared/media/validateImageForUpload';

export type WebpCompressOptions = {
  maxEdge: number;
  quality: number;
};

async function compressImageToWebpMainThread(file: File, options: WebpCompressOptions): Promise<File> {
  const objectUrl = URL.createObjectURL(file);
  const image = new window.Image();

  try {
    await new Promise<void>((resolve, reject) => {
      image.onload = () => resolve();
      image.onerror = () => reject(new Error('Không thể đọc file ảnh.'));
      image.src = objectUrl;
    });

    const ratio = Math.min(1, options.maxEdge / Math.max(image.width, image.height));
    const targetWidth = Math.max(1, Math.round(image.width * ratio));
    const targetHeight = Math.max(1, Math.round(image.height * ratio));

    const canvas = document.createElement('canvas');
    canvas.width = targetWidth;
    canvas.height = targetHeight;
    const context = canvas.getContext('2d');
    if (!context) {
      throw new Error('Không khởi tạo được bộ nén ảnh.');
    }

    context.drawImage(image, 0, 0, targetWidth, targetHeight);
    const blob = await new Promise<Blob | null>((resolve) =>
      canvas.toBlob(resolve, 'image/webp', options.quality)
    );
    if (!blob) {
      throw new Error('Không nén được WebP.');
    }

    return new File([blob], `upload_${Date.now()}.webp`, { type: 'image/webp' });
  } finally {
    URL.revokeObjectURL(objectUrl);
  }
}

/**
 * Nén ảnh sang WebP trong Web Worker (fallback main thread nếu Worker lỗi).
 */
export async function compressImageToWebpInWorker(file: File, options: WebpCompressOptions): Promise<File> {
  validateImageForUpload(file);

  const WorkerCtor = typeof Worker !== 'undefined' ? Worker : undefined;
  if (!WorkerCtor) {
    return compressImageToWebpMainThread(file, options);
  }

  const id = crypto.randomUUID();

  try {
    const worker = new WorkerCtor(new URL('./workers/imageWebpCompress.worker.ts', import.meta.url));
    const buf = await file.arrayBuffer();

    const outBuffer = await new Promise<ArrayBuffer>((resolve, reject) => {
      const onMsg = (ev: MessageEvent) => {
        const d = ev.data as { id?: string; buffer?: ArrayBuffer; error?: string };
        if (d?.id !== id) {
          return;
        }

        worker.removeEventListener('message', onMsg);
        worker.terminate();
        if (d.error) {
          reject(new Error(d.error));
          return;
        }

        if (!d.buffer) {
          reject(new Error('Worker trả dữ liệu rỗng.'));
          return;
        }

        resolve(d.buffer);
      };

      worker.addEventListener('message', onMsg);
      worker.postMessage(
        {
          id,
          buffer: buf,
          mimeType: file.type,
          maxEdge: options.maxEdge,
          quality: options.quality,
        },
        [buf]
      );
    });

    return new File([outBuffer], `upload_${Date.now()}.webp`, { type: 'image/webp' });
  } catch {
    return compressImageToWebpMainThread(file, options);
  }
}
