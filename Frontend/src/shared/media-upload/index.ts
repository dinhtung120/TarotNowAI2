export {
  DEFAULT_UPLOAD_RETRY_ATTEMPTS,
  IMAGE_UPLOAD_CONTENT_TYPE,
  IMAGE_UPLOAD_MAX_BYTES,
  VOICE_UPLOAD_MAX_BYTES,
  VOICE_UPLOAD_MAX_DURATION_MS,
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
