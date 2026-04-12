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
 * Nén ảnh sang WebP trong Web Worker (fallback main thread nếu Worker lỗi hoặc không được hỗ trợ).
 * Lý do chọn hướng tiếp cận này: xử lý nén ảnh là tác vụ tốn CPU, thực hiện trong Worker sẽ giải phóng luồng chính,
 * giúp giao diện (UI) vẫn mượt mà, không bị khựng (lag) khi đang xử lý ảnh lớn.
 */
export async function compressImageToWebpInWorker(file: File, options: WebpCompressOptions): Promise<File> {
  // Thực hiện validate cơ bản trước khi nén để tránh xử lý các file không phải ảnh hoặc quá lớn.
  validateImageForUpload(file);

  // Kiểm tra xem trình duyệt có hỗ trợ Web Worker không (SSR hoặc trình duyệt quá cũ).
  if (typeof Worker === 'undefined') {
    return compressImageToWebpMainThread(file, options);
  }

  const id = crypto.randomUUID();
  let worker: Worker | null = null;

  try {
    /**
     * KHỞI TẠO WORKER:
     * Sử dụng cú pháp `new Worker(new URL(...))` trực tiếp để Next.js/Webpack nhận diện chính xác.
     * Lưu ý: Không dùng biến trung gian cho hàm dựng Worker vì trình đóng gói sẽ không thể 'static analysis'
     * để biên dịch file .ts sang .js, dẫn đến lỗi 404 trong môi trường production.
     */
    worker = new Worker(new URL('./workers/imageWebpCompress.worker.ts', import.meta.url));
    const buf = await file.arrayBuffer();

    const outBuffer = await new Promise<ArrayBuffer>((resolve, reject) => {
      if (!worker) return reject(new Error('Sự cố khởi tạo bộ nén.'));

      // Xử lý khi nhận được kết quả nén từ Worker.
      const onMsg = (ev: MessageEvent) => {
        const d = ev.data as { id?: string; buffer?: ArrayBuffer; error?: string };
        if (d?.id !== id) return;

        cleanup();
        if (d.error) {
          reject(new Error(d.error));
        } else if (!d.buffer) {
          reject(new Error('Dữ liệu nén rỗng.'));
        } else {
          resolve(d.buffer);
        }
      };

      /**
       * XỬ LÝ LỖI WORKER (QUAN TRỌNG):
       * Nếu Worker không thể tải (ví dụ lỗi 404 mạng) hoặc gặp lỗi runtime nghiêm trọng, 
       * sự kiện 'error' sẽ kích hoạt. Chúng ta reject ngay để luồng chính thực hiện fallback,
       * tránh tình trạng giao diện bị treo vô hạn trong trạng thái "Đang tải lên".
       */
      const onErr = (err: ErrorEvent) => {
        cleanup();
        reject(err);
      };

      // Dọn dẹp tài nguyên sau khi hoàn tất hoặc gặp lỗi để tránh rò rỉ bộ nhớ (memory leak).
      const cleanup = () => {
        worker?.removeEventListener('message', onMsg);
        worker?.removeEventListener('error', onErr);
        worker?.terminate();
      };

      worker.addEventListener('message', onMsg);
      worker.addEventListener('error', onErr);

      // Gửi dữ liệu thô (ArrayBuffer) sang Worker theo dạng Transferable để tối ưu hiệu năng (nút copy/move pointer).
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
  } catch (error) {
    /**
     * CƠ CHẾ DỰ PHÒNG (FALLBACK):
     * Mọi lỗi xảy ra (Worker 404, lỗi logic trong Worker, trình duyệt chặn Worker...) đều được bắt tại đây.
     * Hệ thống sẽ tự động chuyển sang nén ảnh bằng luồng chính để đảm bảo người dùng vẫn upload được ảnh.
     */
    console.warn('WebWorker compression failed, falling back to main thread:', error);
    worker?.terminate();
    return compressImageToWebpMainThread(file, options);
  }
}
