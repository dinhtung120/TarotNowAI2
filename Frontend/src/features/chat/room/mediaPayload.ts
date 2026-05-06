import type { MediaPayloadDto } from '@/features/chat/shared/actions';
import {
  compressImageForDirectUpload,
  getVoiceUploadMaxBytes,
  getVoiceUploadMaxDurationMs,
  getImageDimensions,
  presignConversationMediaUpload,
  uploadToR2WithRetry,
  validateImageForDirectUpload,
} from '@/shared/media-upload';

const FALLBACK_VOICE_MIME = 'audio/webm';
const ALLOWED_VOICE_MIME_TYPES = new Set([
  'audio/webm',
  'audio/ogg',
  'audio/opus',
  'audio/mp4',
  'audio/x-m4a',
  'audio/m4a',
  'audio/mpeg',
  'audio/wav',
]);

interface BuildImagePayloadParams {
  conversationId: string;
  file: File;
  onProgress?: (percent: number) => void;
}

interface BuildVoicePayloadParams {
  blob: Blob;
  conversationId: string;
  durationMs: number;
  onProgress?: (percent: number) => void;
}

export function validateRawImageFile(file: File) {
  validateImageForDirectUpload(file);
}

export async function buildImageMediaPayload({
  conversationId,
  file,
  onProgress,
}: BuildImagePayloadParams): Promise<MediaPayloadDto> {
  const compressedFile = await compressImageForDirectUpload(file);
  const dimensions = await getImageDimensions(compressedFile);

  const presigned = await presignConversationMediaUpload({
    conversationId,
    mediaKind: 'image',
    contentType: compressedFile.type,
    sizeBytes: compressedFile.size,
  });
  if (!presigned.ok) {
    throw new Error(presigned.error || 'Không thể tạo URL upload ảnh chat.');
  }

  await uploadToR2WithRetry({
    uploadUrl: presigned.data.uploadUrl,
    contentType: compressedFile.type,
    file: compressedFile,
    onProgress,
  });

  return {
    url: presigned.data.publicUrl,
    objectKey: presigned.data.objectKey,
    uploadToken: presigned.data.uploadToken,
    mimeType: compressedFile.type,
    sizeBytes: compressedFile.size,
    width: dimensions.width,
    height: dimensions.height,
    description: file.name,
    processingStatus: 'uploaded',
  };
}

export async function buildVoiceMediaPayloadFromBlob({
  blob,
  conversationId,
  durationMs,
  onProgress,
}: BuildVoicePayloadParams): Promise<MediaPayloadDto> {
  if (blob.size <= 0 || blob.size > getVoiceUploadMaxBytes()) {
    throw new Error('Tin nhắn thoại vượt quá 5MB, vui lòng ghi ngắn hơn.');
  }

  if (durationMs <= 0 || durationMs > getVoiceUploadMaxDurationMs()) {
    throw new Error('Tin nhắn thoại vượt quá giới hạn 10 phút.');
  }

  const normalizedMimeType = normalizeVoiceMimeType(blob.type);
  const presigned = await presignConversationMediaUpload({
    conversationId,
    mediaKind: 'voice',
    contentType: normalizedMimeType,
    sizeBytes: blob.size,
    durationMs,
  });
  if (!presigned.ok) {
    throw new Error(presigned.error || 'Không thể tạo URL upload voice chat.');
  }

  await uploadToR2WithRetry({
    uploadUrl: presigned.data.uploadUrl,
    contentType: normalizedMimeType,
    file: blob,
    onProgress,
  });

  return {
    url: presigned.data.publicUrl,
    objectKey: presigned.data.objectKey,
    uploadToken: presigned.data.uploadToken,
    mimeType: normalizedMimeType,
    sizeBytes: blob.size,
    durationMs,
    description: 'voice_recording',
    processingStatus: 'uploaded',
  };
}

function normalizeVoiceMimeType(rawMimeType: string): string {
  const normalized = rawMimeType.trim().toLowerCase();
  if (!normalized) {
    return FALLBACK_VOICE_MIME;
  }

  const mimeWithoutCodec = normalized.split(';')[0]?.trim() || normalized;
  const canonicalMimeType = mimeWithoutCodec === 'audio/x-m4a' || mimeWithoutCodec === 'audio/m4a'
    ? 'audio/mp4'
    : mimeWithoutCodec;

  if (!ALLOWED_VOICE_MIME_TYPES.has(canonicalMimeType)) {
    throw new Error('Định dạng âm thanh chưa được hỗ trợ.');
  }

  return canonicalMimeType;
}
