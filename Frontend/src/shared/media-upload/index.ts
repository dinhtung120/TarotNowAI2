export {
  getDefaultUploadRetryAttempts,
  getDefaultUploadRetryDelayMs,
  IMAGE_UPLOAD_CONTENT_TYPE,
  getImageUploadMaxBytes,
  getVoiceUploadMaxBytes,
  getVoiceUploadMaxDurationMs,
} from '@/shared/media-upload/constants';
export {
  compressImageForDirectUpload,
  getImageDimensions,
  ImageUploadValidationError,
  validateImageForDirectUpload,
} from '@/shared/media-upload/imageCompression';
export {
  appendPlaceholderMarkdown,
  appendRealImageMarkdown,
  createMarkdownImagePlaceholder,
  removePlaceholderMarkdown,
  replacePlaceholderMarkdown,
} from '@/shared/media-upload/markdownPlaceholders';
export {
  confirmAvatarUpload,
  confirmCommunityImageUpload,
  presignAvatarUpload,
  presignCommunityImageUpload,
  presignConversationMediaUpload,
} from '@/shared/media-upload/presignedUploadApi';
export { uploadToPresignedUrlViaXhr } from '@/shared/media-upload/uploadViaXhr';
export { uploadToR2WithRetry } from '@/shared/media-upload/uploadWithRetry';
export type {
  AvatarConfirmResponse,
  CommunityImageConfirmResponse,
  CommunityImageContextType,
  ConversationMediaKind,
  PresignedUploadResponse,
} from '@/shared/media-upload/types';
