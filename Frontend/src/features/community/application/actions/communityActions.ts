'use server';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import {
 CommunityPost,
 CreatePostPayload,
 ToggleReactionPayload,
 type CommunityComment,
 type CommunityFeedResponse,
} from '../../types';
interface CommentsResponse { items: CommunityComment[]; totalCount: number; page: number; pageSize: number; totalPages: number }

const UNAUTHORIZED = 'Unauthorized';
const FAIL_UPLOAD_IMAGE = 'Failed to upload image';
const FAIL_GET_FEED = 'Failed to get feed';
const FAIL_CREATE_POST = 'Failed to create post';
const FAIL_TOGGLE_REACTION = 'Failed to toggle reaction';
const FAIL_ADD_COMMENT = 'Failed to add comment';
const FAIL_GET_COMMENTS = 'Failed to fetch comments';

async function withToken<T>(work: (token: string) => Promise<ActionResult<T>>): Promise<ActionResult<T>> {
 const token = await getServerAccessToken();
 if (!token) return actionFail(UNAUTHORIZED);
 return work(token);
}

export async function uploadPostImageAction(formData: FormData): Promise<ActionResult<{ url: string }>> {
 return withToken(async (token) => {
  try {
   const result = await serverHttpRequest<{ url: string }>('/community/images', { method: 'POST', token, formData, fallbackErrorMessage: FAIL_UPLOAD_IMAGE });
   if (!result.ok) return actionFail(result.error || FAIL_UPLOAD_IMAGE);
   return actionOk(result.data);
  } catch (error) {
   logger.error('uploadPostImageAction error', error);
   return actionFail(FAIL_UPLOAD_IMAGE);
  }
 });
}

export async function getFeedAction(
 page = 1,
 pageSize = 10,
 visibility?: string
): Promise<ActionResult<CommunityFeedResponse>> {
 return withToken(async (token) => {
  try {
   const params = new URLSearchParams({ page: page.toString(), pageSize: pageSize.toString() });
   if (visibility) params.append('visibility', visibility);
   const result = await serverHttpRequest<CommunityFeedResponse>(
    `/community/posts?${params.toString()}`,
    { method: 'GET', token, fallbackErrorMessage: FAIL_GET_FEED }
   );
   if (!result.ok) return result.status === 404 ? actionOk({ data: [], metadata: { totalCount: 0, page, pageSize } }) : actionFail(result.error || FAIL_GET_FEED);
   return actionOk(result.data);
  } catch (error) {
   logger.error('getFeedAction error', error);
   return actionFail(FAIL_GET_FEED);
  }
 });
}

export async function createPostAction(payload: CreatePostPayload): Promise<ActionResult<CommunityPost>> {
 return withToken(async (token) => {
  try {
   const result = await serverHttpRequest<CommunityPost>('/community/posts', { method: 'POST', token, json: payload, fallbackErrorMessage: FAIL_CREATE_POST });
   if (!result.ok) return actionFail(result.error || FAIL_CREATE_POST);
   return actionOk(result.data);
  } catch (error) {
   logger.error('createPostAction error', error);
   return actionFail(FAIL_CREATE_POST);
  }
 });
}

export async function toggleReactionAction(postId: string, payload: ToggleReactionPayload): Promise<ActionResult<{ success: boolean }>> {
 return withToken(async (token) => {
  try {
   const result = await serverHttpRequest<{ success: boolean }>(`/community/posts/${postId}/reactions`, { method: 'POST', token, json: payload, fallbackErrorMessage: FAIL_TOGGLE_REACTION });
   if (!result.ok) return actionFail(result.error || FAIL_TOGGLE_REACTION);
   return actionOk(result.data);
  } catch (error) {
   logger.error('toggleReactionAction error', error);
   return actionFail(FAIL_TOGGLE_REACTION);
  }
 });
}

export async function addCommentAction(postId: string, content: string): Promise<ActionResult<CommunityComment>> {
 return withToken(async (token) => {
  try {
   const result = await serverHttpRequest<CommunityComment>(`/community/posts/${postId}/comments`, { method: 'POST', token, json: { content }, fallbackErrorMessage: FAIL_ADD_COMMENT });
   if (!result.ok) return actionFail(result.error || FAIL_ADD_COMMENT);
   return actionOk(result.data);
  } catch (error) {
   logger.error('addCommentAction error', error);
   return actionFail(FAIL_ADD_COMMENT);
  }
 });
}

export async function getCommentsAction(postId: string, page = 1, pageSize = 10): Promise<ActionResult<{ items: CommunityComment[]; totalCount: number; hasNextPage: boolean }>> {
 const token = await getServerAccessToken();
 try {
  const result = await serverHttpRequest<CommentsResponse>(`/community/posts/${postId}/comments?page=${page}&pageSize=${pageSize}`, { method: 'GET', token: token || undefined, fallbackErrorMessage: FAIL_GET_COMMENTS });
  if (!result.ok) return actionFail(result.error || FAIL_GET_COMMENTS);
  return actionOk({ items: result.data?.items || [], totalCount: result.data?.totalCount || 0, hasNextPage: page < (result.data?.totalPages || 0) });
 } catch (error) {
  logger.error('getCommentsAction error', error);
  return actionFail(FAIL_GET_COMMENTS);
 }
}
