/*
 * ===================================================================
 * FILE: types.ts
 * ===================================================================
 * MỤC ĐÍCH:
 *   Định nghĩa các interface cho dữ liệu gửi trả về từ Backend đối với 
 *   tính năng Cộng Đồng (Community).
 * ===================================================================
 */

export type PostVisibility = 'public' | 'private';

export type ReactionType = 'like' | 'love' | 'insightful' | 'haha' | 'sad';

export interface CommunityPost {
  id: string;
  authorId: string;
  authorDisplayName: string;
  authorAvatarUrl: string | null;
  content: string;
  visibility: PostVisibility;
  reactionsCount: Record<string, number>;
  totalReactions: number;
  commentsCount: number;
  isDeleted: boolean;
  createdAt: string;
  updatedAt: string | null;
  // Cờ báo hiệu viewer hiện tại đã react loại gì chưa (nếu có null là chưa)
  viewerReaction: ReactionType | null;
}

export interface CommunityComment {
  id: string;
  postId: string;
  authorId: string;
  authorDisplayName: string;
  authorAvatarUrl: string | null;
  content: string;
  createdAt: string;
}

export interface CreatePostPayload {
  content: string;
  visibility: PostVisibility;
}

export interface ToggleReactionPayload {
  type: ReactionType;
}

export interface ReportPostPayload {
  reasonCode: string;
  description: string;
}
