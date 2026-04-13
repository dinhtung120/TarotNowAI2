import imageCompression from 'browser-image-compression';
import {
  IMAGE_COMPRESSION_DEFAULTS,
  IMAGE_UPLOAD_CONTENT_TYPE,
  IMAGE_UPLOAD_MAX_BYTES,
} from '@/shared/media-upload/constants';

export class ImageUploadValidationError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'ImageUploadValidationError';
  }
}

export function validateImageForDirectUpload(file: File): void {
  if (!file.type.toLowerCase().startsWith('image/')) {
    throw new ImageUploadValidationError('Chỉ được chọn file ảnh.');
  }

  if (file.size <= 0) {
    throw new ImageUploadValidationError('File ảnh không hợp lệ.');
  }

  if (file.size > IMAGE_UPLOAD_MAX_BYTES) {
    throw new ImageUploadValidationError('Ảnh quá lớn (tối đa 10MB).');
  }
}

export async function compressImageForDirectUpload(file: File): Promise<File> {
  validateImageForDirectUpload(file);

  const compressed = await imageCompression(file, IMAGE_COMPRESSION_DEFAULTS);
  const outFile = new File([compressed], toWebpFileName(file.name), {
    type: IMAGE_UPLOAD_CONTENT_TYPE,
  });

  if (outFile.size <= 0 || outFile.size > IMAGE_UPLOAD_MAX_BYTES) {
    throw new ImageUploadValidationError('Ảnh sau nén không hợp lệ (tối đa 10MB).');
  }

  return outFile;
}

export async function getImageDimensions(file: Blob): Promise<{ height: number; width: number }> {
  const objectUrl = URL.createObjectURL(file);
  const image = new window.Image();

  try {
    await new Promise<void>((resolve, reject) => {
      image.onload = () => resolve();
      image.onerror = () => reject(new ImageUploadValidationError('Không thể đọc metadata ảnh.'));
      image.src = objectUrl;
    });

    return {
      width: image.naturalWidth || image.width,
      height: image.naturalHeight || image.height,
    };
  } finally {
    URL.revokeObjectURL(objectUrl);
  }
}

function toWebpFileName(fileName: string): string {
  const trimmed = fileName.trim();
  const withoutExtension = trimmed.replace(/\.[^.]+$/, '');
  const safeBaseName = withoutExtension || 'upload';
  return `${safeBaseName}.webp`;
}
