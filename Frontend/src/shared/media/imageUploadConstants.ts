/** MIME cho phép trước khi nén WebP ở FE (khớp kiểm tra BE image/*). */
export const USER_IMAGE_ALLOWED_MIME_TYPES = [
  'image/jpeg',
  'image/png',
  'image/webp',
  'image/gif',
  'image/heic',
  'image/heif',
] as const;

export type UserImageAllowedMime = (typeof USER_IMAGE_ALLOWED_MIME_TYPES)[number];

/** Giới hạn dung lượng file gốc trước khi nén (10MB, khớp RequestSizeLimit API). */
export const USER_IMAGE_MAX_BYTES = 10 * 1024 * 1024;
