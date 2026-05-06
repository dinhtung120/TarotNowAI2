import { postJsonToApiV1, type ClientJsonResult } from '@/shared/http/clientJsonRequest';
import type {
  AvatarConfirmResponse,
  CommunityImageConfirmResponse,
  CommunityImageContextType,
  ConversationMediaKind,
  PresignedUploadResponse,
} from '@/shared/media-upload/types';

export async function presignAvatarUpload(sizeBytes: number): Promise<ClientJsonResult<PresignedUploadResponse>> {
  return postJsonToApiV1<PresignedUploadResponse, { contentType: string; sizeBytes: number }>(
    '/profile/avatar/presign',
    {
      contentType: 'image/webp',
      sizeBytes,
    },
    {
      fallbackErrorMessage: 'Không thể tạo URL upload avatar.',
      unauthorizedMessage: 'Vui lòng đăng nhập để đổi avatar.',
    },
  );
}

export async function confirmAvatarUpload(payload: {
  objectKey: string;
  publicUrl: string;
  uploadToken: string;
}): Promise<ClientJsonResult<AvatarConfirmResponse>> {
  return postJsonToApiV1<AvatarConfirmResponse, { objectKey: string; publicUrl: string; uploadToken: string }>(
    '/profile/avatar/confirm',
    payload,
    {
      fallbackErrorMessage: 'Không thể xác nhận avatar sau khi upload.',
      unauthorizedMessage: 'Vui lòng đăng nhập để đổi avatar.',
    },
  );
}

export async function presignCommunityImageUpload(payload: {
  contextDraftId: string;
  contextType: CommunityImageContextType;
  sizeBytes: number;
}): Promise<ClientJsonResult<PresignedUploadResponse>> {
  return postJsonToApiV1<
    PresignedUploadResponse,
    { contentType: string; contextDraftId: string; contextType: CommunityImageContextType; sizeBytes: number }
  >(
    '/community/image/presign',
    {
      contentType: 'image/webp',
      contextDraftId: payload.contextDraftId,
      contextType: payload.contextType,
      sizeBytes: payload.sizeBytes,
    },
    {
      fallbackErrorMessage: 'Không thể tạo URL upload ảnh.',
      unauthorizedMessage: 'Vui lòng đăng nhập để upload ảnh.',
    },
  );
}

export async function confirmCommunityImageUpload(payload: {
  contextDraftId: string;
  contextType: CommunityImageContextType;
  objectKey: string;
  publicUrl: string;
  uploadToken: string;
}): Promise<ClientJsonResult<CommunityImageConfirmResponse>> {
  return postJsonToApiV1<
    CommunityImageConfirmResponse,
    {
      contextDraftId: string;
      contextType: CommunityImageContextType;
      objectKey: string;
      publicUrl: string;
      uploadToken: string;
    }
  >('/community/image/confirm', payload, {
    fallbackErrorMessage: 'Không thể xác nhận ảnh cộng đồng.',
    unauthorizedMessage: 'Vui lòng đăng nhập để upload ảnh.',
  });
}

export async function presignConversationMediaUpload(payload: {
  contentType: string;
  conversationId: string;
  durationMs?: number;
  mediaKind: ConversationMediaKind;
  sizeBytes: number;
}): Promise<ClientJsonResult<PresignedUploadResponse>> {
  return postJsonToApiV1<
    PresignedUploadResponse,
    { contentType: string; durationMs?: number; mediaKind: ConversationMediaKind; sizeBytes: number }
  >(
    `/conversations/${payload.conversationId}/media/presign`,
    {
      contentType: payload.contentType,
      durationMs: payload.durationMs,
      mediaKind: payload.mediaKind,
      sizeBytes: payload.sizeBytes,
    },
    {
      fallbackErrorMessage: 'Không thể tạo URL upload media chat.',
      unauthorizedMessage: 'Vui lòng đăng nhập để gửi media.',
    },
  );
}
