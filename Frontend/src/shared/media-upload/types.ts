export type CommunityImageContextType = 'comment' | 'post';
export type ConversationMediaKind = 'image' | 'voice';

export interface PresignedUploadResponse {
  expiresAtUtc: string;
  objectKey: string;
  publicUrl: string;
  uploadToken: string;
  uploadUrl: string;
}

export interface AvatarConfirmResponse {
  avatarUrl: string;
  objectKey: string;
  success: boolean;
}

export interface CommunityImageConfirmResponse {
  contextDraftId: string;
  contextType: CommunityImageContextType;
  objectKey: string;
  publicUrl: string;
  success: boolean;
}
