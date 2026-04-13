export const IMAGE_UPLOAD_CONTENT_TYPE = 'image/webp';
export const IMAGE_UPLOAD_MAX_BYTES = 10 * 1024 * 1024;
export const VOICE_UPLOAD_MAX_BYTES = 5 * 1024 * 1024;
export const VOICE_UPLOAD_MAX_DURATION_MS = 600_000;

export const IMAGE_COMPRESSION_TARGET_BYTES = 80 * 1024;
export const IMAGE_COMPRESSION_STEPS = [
  { initialQuality: 0.68, maxSizeMB: 0.15, maxWidthOrHeight: 1440 },
  { initialQuality: 0.58, maxSizeMB: 0.1, maxWidthOrHeight: 1200 },
  { initialQuality: 0.48, maxSizeMB: 0.06, maxWidthOrHeight: 960 },
  { initialQuality: 0.38, maxSizeMB: 0.03, maxWidthOrHeight: 640 },
] as const;
export const IMAGE_COMPRESSION_BASE_OPTIONS = {
  fileType: IMAGE_UPLOAD_CONTENT_TYPE,
  useWebWorker: true,
  // Tự host script worker để tránh fallback CDN (bị chặn bởi CSP production).
  libURL: '/vendor/browser-image-compression.js',
} as const;

export const DEFAULT_UPLOAD_RETRY_ATTEMPTS = 3;
export const DEFAULT_UPLOAD_RETRY_DELAY_MS = 350;
