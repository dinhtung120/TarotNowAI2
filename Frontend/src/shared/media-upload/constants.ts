import { getRuntimePolicyStoreSnapshot } from '@/shared/config/runtimePolicyStore';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';

export const IMAGE_UPLOAD_CONTENT_TYPE = 'image/webp';

export interface ImageCompressionStep {
  initialQuality: number;
  maxSizeMb: number;
  maxWidthOrHeight: number;
}

export const IMAGE_COMPRESSION_BASE_OPTIONS = {
  fileType: IMAGE_UPLOAD_CONTENT_TYPE,
  useWebWorker: true,
  // Tự host script worker để tránh fallback CDN (bị chặn bởi CSP production).
  libURL: '/vendor/browser-image-compression.js',
} as const;

export function getImageUploadMaxBytes(): number {
  return getRuntimePolicyStoreSnapshot().media.upload.maxImageBytes
    || RUNTIME_POLICY_FALLBACKS.media.upload.maxImageBytes;
}

export function getVoiceUploadMaxBytes(): number {
  return getRuntimePolicyStoreSnapshot().media.upload.maxVoiceBytes
    || RUNTIME_POLICY_FALLBACKS.media.upload.maxVoiceBytes;
}

export function getVoiceUploadMaxDurationMs(): number {
  return getRuntimePolicyStoreSnapshot().media.upload.maxVoiceDurationMs
    || RUNTIME_POLICY_FALLBACKS.media.upload.maxVoiceDurationMs;
}

export function getImageCompressionTargetBytes(): number {
  return getRuntimePolicyStoreSnapshot().media.upload.imageCompressionTargetBytes
    || RUNTIME_POLICY_FALLBACKS.media.upload.imageCompressionTargetBytes;
}

export function getImageCompressionSteps(): readonly ImageCompressionStep[] {
  const runtimeSteps = getRuntimePolicyStoreSnapshot().media.upload.imageCompressionSteps;
  if (runtimeSteps.length > 0) {
    return runtimeSteps;
  }

  return RUNTIME_POLICY_FALLBACKS.media.upload.imageCompressionSteps;
}

export function getDefaultUploadRetryAttempts(): number {
  return getRuntimePolicyStoreSnapshot().media.upload.retryAttempts
    ?? RUNTIME_POLICY_FALLBACKS.media.upload.retryAttempts;
}

export function getDefaultUploadRetryDelayMs(): number {
  return getRuntimePolicyStoreSnapshot().media.upload.retryDelayMs
    ?? RUNTIME_POLICY_FALLBACKS.media.upload.retryDelayMs;
}
