

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
