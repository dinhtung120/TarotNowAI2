import imageCompression from 'browser-image-compression';
import {
  type ImageCompressionStep,
  IMAGE_COMPRESSION_BASE_OPTIONS,
  IMAGE_UPLOAD_CONTENT_TYPE,
  getImageCompressionSteps,
  getImageCompressionTargetBytes,
  getImageUploadMaxBytes,
} from '@/shared/media-upload/constants';

export class ImageUploadValidationError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'ImageUploadValidationError';
  }
}

export function validateImageForDirectUpload(file: File): void {
  const imageUploadMaxBytes = getImageUploadMaxBytes();
  if (!file.type.toLowerCase().startsWith('image/')) {
    throw new ImageUploadValidationError('Chỉ được chọn file ảnh.');
  }

  if (file.size <= 0) {
    throw new ImageUploadValidationError('File ảnh không hợp lệ.');
  }

  if (file.size > imageUploadMaxBytes) {
    throw new ImageUploadValidationError('Ảnh quá lớn (tối đa 10MB).');
  }
}

export async function compressImageForDirectUpload(file: File): Promise<File> {
  validateImageForDirectUpload(file);
  const compressionSteps = getImageCompressionSteps();
  const targetBytes = getImageCompressionTargetBytes();
  const imageUploadMaxBytes = getImageUploadMaxBytes();

  let currentFile = file;
  for (const step of compressionSteps) {
    currentFile = await compressByStep(currentFile, file.name, step);
    if (currentFile.size <= targetBytes) {
      break;
    }
  }

  if (currentFile.size <= 0 || currentFile.size > imageUploadMaxBytes) {
    throw new ImageUploadValidationError('Ảnh sau nén không hợp lệ (tối đa 10MB).');
  }

  return currentFile;
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

async function compressByStep(
  file: File,
  originalFileName: string,
  step: ImageCompressionStep,
): Promise<File> {
  const compressed = await imageCompression(file, {
    ...IMAGE_COMPRESSION_BASE_OPTIONS,
    ...step,
  });

  return new File([compressed], toWebpFileName(originalFileName), {
    type: IMAGE_UPLOAD_CONTENT_TYPE,
  });
}
