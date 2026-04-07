import type { MediaPayloadDto } from '@/features/chat/application/actions';

const MAX_MEDIA_PAYLOAD_BYTES = 5 * 1024 * 1024;
const MAX_RAW_IMAGE_BYTES = 20 * 1024 * 1024;

function readBlobAsDataUrl(blob: Blob): Promise<string> {
  return new Promise((resolve, reject) => {
    const reader = new FileReader();
    reader.onerror = () => reject(new Error('Không thể đọc file media.'));
    reader.onload = () => {
      const value = typeof reader.result === 'string' ? reader.result : '';
      if (!value.startsWith('data:')) {
        reject(new Error('Định dạng media không hợp lệ.'));
        return;
      }
      resolve(value);
    };
    reader.readAsDataURL(blob);
  });
}

function canvasToBlob(canvas: HTMLCanvasElement, mimeType: string, quality?: number) {
  return new Promise<Blob | null>((resolve) => {
    canvas.toBlob((blob) => resolve(blob), mimeType, quality);
  });
}

export function validateRawImageFile(file: File) {
  if (file.size > MAX_RAW_IMAGE_BYTES) {
    throw new Error('Ảnh gốc vượt quá 20MB.');
  }
}

export async function buildImageMediaPayload(file: File): Promise<MediaPayloadDto> {
  const objectUrl = URL.createObjectURL(file);
  const image = new window.Image();

  try {
    await new Promise<void>((resolve, reject) => {
      image.onload = () => resolve();
      image.onerror = () => reject(new Error('Không thể xử lý ảnh đã chọn.'));
      image.src = objectUrl;
    });

    const maxDimension = 2048;
    const ratio = Math.min(1, maxDimension / Math.max(image.width, image.height));
    const targetWidth = Math.max(1, Math.round(image.width * ratio));
    const targetHeight = Math.max(1, Math.round(image.height * ratio));

    const canvas = document.createElement('canvas');
    canvas.width = targetWidth;
    canvas.height = targetHeight;
    const context = canvas.getContext('2d');
    if (!context) throw new Error('Không khởi tạo được bộ xử lý ảnh.');

    context.drawImage(image, 0, 0, targetWidth, targetHeight);

    const candidates: Array<{ mimeType: string; quality: number; status: string }> = [
      { mimeType: 'image/avif', quality: 0.7, status: 'client_compressed_avif' },
      { mimeType: 'image/webp', quality: 0.85, status: 'client_compressed_webp' },
      { mimeType: 'image/jpeg', quality: 0.85, status: 'client_compressed_jpeg_fallback' },
    ];

    let blob: Blob | null = null;
    let selected = candidates[candidates.length - 1];

    for (const candidate of candidates) {
      blob = await canvasToBlob(canvas, candidate.mimeType, candidate.quality);
      if (blob) {
        selected = candidate;
        break;
      }
    }

    if (!blob) throw new Error('Không thể nén ảnh đã chọn.');
    if (blob.size > MAX_MEDIA_PAYLOAD_BYTES) {
      throw new Error('Ảnh sau nén vượt quá 5MB, vui lòng chọn ảnh nhỏ hơn.');
    }

    return {
      url: await readBlobAsDataUrl(blob),
      mimeType: selected.mimeType,
      sizeBytes: blob.size,
      width: targetWidth,
      height: targetHeight,
      description: file.name,
      processingStatus: selected.status,
    };
  } finally {
    URL.revokeObjectURL(objectUrl);
  }
}

export async function buildVoiceMediaPayloadFromBlob(blob: Blob, durationMs: number): Promise<MediaPayloadDto> {
  if (blob.size > MAX_MEDIA_PAYLOAD_BYTES) {
    throw new Error('Tin nhắn thoại vượt quá 5MB, vui lòng ghi ngắn hơn.');
  }

  return {
    url: await readBlobAsDataUrl(blob),
    mimeType: blob.type || 'audio/webm',
    sizeBytes: blob.size,
    durationMs,
    description: 'voice_recording',
    processingStatus: 'client_recorded_opus',
  };
}
