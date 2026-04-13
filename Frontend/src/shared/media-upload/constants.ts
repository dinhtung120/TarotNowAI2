export const IMAGE_UPLOAD_CONTENT_TYPE = 'image/webp';
export const IMAGE_UPLOAD_MAX_BYTES = 10 * 1024 * 1024;
export const VOICE_UPLOAD_MAX_BYTES = 5 * 1024 * 1024;
export const VOICE_UPLOAD_MAX_DURATION_MS = 600_000;

export const IMAGE_COMPRESSION_DEFAULTS = {
  fileType: IMAGE_UPLOAD_CONTENT_TYPE,
  maxSizeMB: 1,
  maxWidthOrHeight: 2048,
  useWebWorker: true,
  // Tự host script worker để tránh fallback CDN (bị chặn bởi CSP production).
  libURL: '/vendor/browser-image-compression.js',
} as const;

export const DEFAULT_UPLOAD_RETRY_ATTEMPTS = 3;
export const DEFAULT_UPLOAD_RETRY_DELAY_MS = 350;
