import { USER_IMAGE_ALLOWED_MIME_TYPES, USER_IMAGE_MAX_BYTES } from '@/shared/media/imageUploadConstants';

export class UserImageValidationError extends Error {
  constructor(message: string) {
    super(message);
    this.name = 'UserImageValidationError';
  }
}

/**
 * Kiểm tra MIME + kích thước trước khi đưa vào worker nén.
 */
export function validateImageForUpload(file: File): void {
  const mime = (file.type || '').toLowerCase();
  if (!mime.startsWith('image/')) {
    throw new UserImageValidationError('Chỉ được chọn file ảnh.');
  }

  const allowed = USER_IMAGE_ALLOWED_MIME_TYPES as readonly string[];
  if (!allowed.includes(mime)) {
    throw new UserImageValidationError('Định dạng ảnh không được hỗ trợ (JPEG, PNG, WebP, GIF).');
  }

  if (file.size <= 0) {
    throw new UserImageValidationError('File ảnh không hợp lệ.');
  }

  if (file.size > USER_IMAGE_MAX_BYTES) {
    throw new UserImageValidationError(`Ảnh quá lớn (tối đa ${Math.round(USER_IMAGE_MAX_BYTES / (1024 * 1024))}MB).`);
  }
}
