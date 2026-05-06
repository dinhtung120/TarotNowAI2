import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { AUTH_ERROR } from "@/shared/models/authErrors";

function unauthorized<T>() { return actionFail(AUTH_ERROR.UNAUTHORIZED) as ActionResult<T>; }

export async function acceptConversation(conversationId: string): Promise<ActionResult<{ status: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<{ status: string }>(`/conversations/${conversationId}/accept`, { method: 'POST', token: accessToken, fallbackErrorMessage: 'Failed to accept conversation' });
  if (!result.ok) {
   logger.error('[ChatAction] acceptConversation', result.error, { status: result.status, conversationId });
   return actionFail(result.error || 'Failed to accept conversation');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] acceptConversation', error, { conversationId });
  return actionFail('Failed to accept conversation');
 }
}

export async function rejectConversation(conversationId: string, reason: string): Promise<ActionResult<{ status: string; reason?: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<{ status: string; reason?: string }>(`/conversations/${conversationId}/reject`, { method: 'POST', token: accessToken, json: { reason }, fallbackErrorMessage: 'Failed to reject conversation' });
  if (!result.ok) {
   logger.error('[ChatAction] rejectConversation', result.error, { status: result.status, conversationId });
   return actionFail(result.error || 'Failed to reject conversation');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] rejectConversation', error, { conversationId });
  return actionFail('Failed to reject conversation');
 }
}

export async function cancelPendingConversation(conversationId: string): Promise<ActionResult<{ status: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<{ status: string }>(`/conversations/${conversationId}/cancel`, { method: 'POST', token: accessToken, fallbackErrorMessage: 'Failed to cancel pending conversation' });
  if (!result.ok) {
   logger.error('[ChatAction] cancelPendingConversation', result.error, { status: result.status, conversationId });
   return actionFail(result.error || 'Failed to cancel pending conversation');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] cancelPendingConversation', error, { conversationId });
  return actionFail('Failed to cancel pending conversation');
 }
}

export async function requestConversationComplete(conversationId: string): Promise<ActionResult<{ status: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<{ status: string }>(`/conversations/${conversationId}/complete/request`, { method: 'POST', token: accessToken, fallbackErrorMessage: 'Failed to request completion' });
  if (!result.ok) {
   logger.error('[ChatAction] requestConversationComplete', result.error, { status: result.status, conversationId });
   return actionFail(result.error || 'Failed to request completion');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] requestConversationComplete', error, { conversationId });
  return actionFail('Failed to request completion');
 }
}

export async function respondConversationComplete(conversationId: string, accept: boolean): Promise<ActionResult<{ status: string; accepted: boolean }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<{ status: string; accepted: boolean }>(`/conversations/${conversationId}/complete/respond`, { method: 'POST', token: accessToken, json: { accept }, fallbackErrorMessage: 'Failed to respond completion' });
  if (!result.ok) {
   logger.error('[ChatAction] respondConversationComplete', result.error, { status: result.status, conversationId });
   return actionFail(result.error || 'Failed to respond completion');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] respondConversationComplete', error, { conversationId });
  return actionFail('Failed to respond completion');
 }
}

export async function submitConversationReview(
 conversationId: string,
 payload: { rating: number; comment?: string },
): Promise<ActionResult<{ conversationId: string; readerId: string; rating: number; comment?: string; createdAt: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<{ conversationId: string; readerId: string; rating: number; comment?: string; createdAt: string }>(
    `/conversations/${conversationId}/review`,
    {
      method: 'POST',
      token: accessToken,
      json: payload,
      fallbackErrorMessage: 'Failed to submit review',
    },
  );
  if (!result.ok) {
   logger.error('[ChatAction] submitConversationReview', result.error, { status: result.status, conversationId });
   return actionFail(result.error || 'Failed to submit review');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] submitConversationReview', error, { conversationId });
  return actionFail('Failed to submit review');
 }
}
