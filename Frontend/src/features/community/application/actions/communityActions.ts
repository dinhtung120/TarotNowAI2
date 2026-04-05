'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { logger } from '@/shared/infrastructure/logging/logger';
import { CommunityPost, CreatePostPayload, ReportPostPayload, ToggleReactionPayload } from '../../types';

interface FeedResponse {
  data: CommunityPost[];
  metadata: {
    totalCount: number;
    page: number;
    pageSize: number;
  };
}

export async function uploadPostImageAction(formData: FormData): Promise<ActionResult<{ url: string }>> {
  const token = await getServerAccessToken();
  if (!token) return actionFail('Unauthorized');

  try {
    const result = await serverHttpRequest<{ url: string }>('/community/images', {
      method: 'POST',
      token,
      formData,
      fallbackErrorMessage: 'Failed to upload image'
    });

    if (!result.ok) return actionFail(result.error || 'Failed to upload image');
    return actionOk(result.data as { url: string });
  } catch (error) {
    logger.error('uploadPostImageAction error', error);
    return actionFail('Failed to upload image');
  }
}

export async function getFeedAction(
  page = 1,
  pageSize = 10,
  visibility?: string
): Promise<ActionResult<FeedResponse>> {
  const token = await getServerAccessToken();
  if (!token) return actionFail('Unauthorized');

  try {
    const params = new URLSearchParams({
      page: page.toString(),
      pageSize: pageSize.toString(),
    });
    if (visibility) params.append('visibility', visibility);

    const result = await serverHttpRequest<FeedResponse>(`/community/posts?${params.toString()}`, {
      method: 'GET',
      token,
      fallbackErrorMessage: 'Failed to get feed'
    });

    if (!result.ok) {
      if (result.status === 404) return actionOk({ data: [], metadata: { totalCount: 0, page, pageSize } });
      return actionFail(result.error || 'Failed to get feed');
    }
    return actionOk(result.data as FeedResponse);
  } catch (error) {
    logger.error('getFeedAction error', error);
    return actionFail('Failed to get feed');
  }
}

export async function createPostAction(payload: CreatePostPayload): Promise<ActionResult<CommunityPost>> {
  const token = await getServerAccessToken();
  if (!token) return actionFail('Unauthorized');

  try {
    const result = await serverHttpRequest<CommunityPost>('/community/posts', {
      method: 'POST',
      token,
      json: payload,
      fallbackErrorMessage: 'Failed to create post'
    });

    if (!result.ok) return actionFail(result.error || 'Failed to create post');
    return actionOk(result.data as CommunityPost);
  } catch (error) {
    logger.error('createPostAction error', error);
    return actionFail('Failed to create post');
  }
}

export async function deletePostAction(postId: string): Promise<ActionResult<void>> {
  const token = await getServerAccessToken();
  if (!token) return actionFail('Unauthorized');

  try {
    const result = await serverHttpRequest<void>(`/community/posts/${postId}`, {
      method: 'DELETE',
      token,
      fallbackErrorMessage: 'Failed to delete post'
    });

    if (!result.ok) return actionFail(result.error || 'Failed to delete post');
    return actionOk(undefined as void);
  } catch (error) {
    logger.error('deletePostAction error', error);
    return actionFail('Failed to delete post');
  }
}

export async function toggleReactionAction(postId: string, payload: ToggleReactionPayload): Promise<ActionResult<{success: boolean}>> {
  const token = await getServerAccessToken();
  if (!token) return actionFail('Unauthorized');

  try {
    const result = await serverHttpRequest<{success: boolean}>(`/community/posts/${postId}/reactions`, {
      method: 'POST',
      token,
      json: payload,
      fallbackErrorMessage: 'Failed to toggle reaction'
    });

    if (!result.ok) return actionFail(result.error || 'Failed to toggle reaction');
    return actionOk(result.data as {success: boolean});
  } catch (error) {
    logger.error('toggleReactionAction error', error);
    return actionFail('Failed to toggle reaction');
  }
}

export async function reportPostAction(postId: string, payload: ReportPostPayload): Promise<ActionResult<{success: boolean; reportId: string}>> {
  const token = await getServerAccessToken();
  if (!token) return actionFail('Unauthorized');

  try {
    const result = await serverHttpRequest<{success: boolean; reportId: string}>(`/community/posts/${postId}/reports`, {
      method: 'POST',
      token,
      json: payload,
      fallbackErrorMessage: 'Failed to report post'
    });

    if (!result.ok) return actionFail(result.error || 'Failed to report post');
    return actionOk(result.data as {success: boolean; reportId: string});
  } catch (error) {
    logger.error('reportPostAction error', error);
    return actionFail('Failed to report post');
  }
}

export async function addCommentAction(postId: string, content: string): Promise<ActionResult<import('../../types').CommunityComment>> {
  const token = await getServerAccessToken();
  if (!token) return actionFail('Unauthorized');

  try {
    const result = await serverHttpRequest<import('../../types').CommunityComment>(`/community/posts/${postId}/comments`, {
      method: 'POST',
      token,
      json: { content },
      fallbackErrorMessage: 'Failed to add comment'
    });

    if (!result.ok) return actionFail(result.error || 'Failed to add comment');
    return actionOk(result.data as import('../../types').CommunityComment);
  } catch (error) {
    logger.error('addCommentAction error', error);
    return actionFail('Failed to add comment');
  }
}

export async function getCommentsAction(
  postId: string,
  page = 1,
  pageSize = 10
): Promise<ActionResult<{ items: import('../../types').CommunityComment[]; totalCount: number; hasNextPage: boolean }>> {
  const token = await getServerAccessToken();
  try {
    const result = await serverHttpRequest<{
      items: import('../../types').CommunityComment[];
      totalCount: number;
      page: number;
      pageSize: number;
      totalPages: number;
    }>(`/community/posts/${postId}/comments?page=${page}&pageSize=${pageSize}`, {
      method: 'GET',
      token: token || undefined,
      fallbackErrorMessage: 'Failed to fetch comments'
    });

    if (!result.ok) return actionFail(result.error || 'Failed to fetch comments');
    
    return actionOk({
      items: result.data?.items || [],
      totalCount: result.data?.totalCount || 0,
      hasNextPage: page < (result.data?.totalPages || 0)
    });
  } catch (error) {
    logger.error('getCommentsAction error', error);
    return actionFail('Failed to fetch comments');
  }
}
