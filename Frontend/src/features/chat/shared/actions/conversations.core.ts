import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import type { ChatMessageDto, ConversationDto, InboxTab, ListConversationsResult, ListMessagesResult, MediaPayloadDto } from './conversations.types';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

function unauthorized<T>() { return actionFail(AUTH_ERROR.UNAUTHORIZED) as ActionResult<T>; }

export async function createConversation(readerId: string, slaHours?: number): Promise<ActionResult<ConversationDto>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const payload = slaHours ? { readerId, slaHours } : { readerId };
  const result = await serverHttpRequest<ConversationDto>('/conversations', { method: 'POST', token: accessToken, json: payload, fallbackErrorMessage: 'Failed to create conversation' });
  if (!result.ok) {
   logger.error('[ChatAction] createConversation', result.error, { status: result.status, readerId });
   return actionFail(result.error || 'Failed to create conversation');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] createConversation', error, { readerId });
  return actionFail('Failed to create conversation');
 }
}

export async function listConversations(tab: InboxTab = 'active', page = 1, pageSize = 20): Promise<ActionResult<ListConversationsResult>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const query = new URLSearchParams({ tab, page: page.toString(), pageSize: pageSize.toString() });
  const result = await serverHttpRequest<ListConversationsResult>(`/conversations?${query.toString()}`, { method: 'GET', token: accessToken, fallbackErrorMessage: 'Failed to list conversations' });
  if (!result.ok) {
   logger.error('[ChatAction] listConversations', result.error, { status: result.status, tab, page, pageSize });
   return actionFail(result.error || 'Failed to list conversations');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] listConversations', error, { tab, page, pageSize });
  return actionFail('Failed to list conversations');
 }
}

export async function getUnreadConversationCount(): Promise<ActionResult<{ count: number }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<{ count: number }>('/conversations/unread-total', { method: 'GET', token: accessToken, fallbackErrorMessage: 'Failed to get unread total' });
  if (!result.ok) {
   logger.error('[ChatAction] getUnreadConversationCount', result.error, { status: result.status });
   return actionFail(result.error || 'Failed to get unread total');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] getUnreadConversationCount', error);
  return actionFail('Failed to get unread total');
 }
}

export async function listMessages(conversationId: string, options?: { cursor?: string; limit?: number }): Promise<ActionResult<ListMessagesResult>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const query = new URLSearchParams();
  if (options?.cursor) query.set('cursor', options.cursor);
  if (options?.limit) query.set('limit', options.limit.toString());
  const suffix = query.toString();
  const route = suffix ? `/conversations/${conversationId}/messages?${suffix}` : `/conversations/${conversationId}/messages`;
  const result = await serverHttpRequest<ListMessagesResult>(route, { method: 'GET', token: accessToken, fallbackErrorMessage: 'Failed to list messages' });
  if (!result.ok) {
   logger.error('[ChatAction] listMessages', result.error, { status: result.status, conversationId, cursor: options?.cursor, limit: options?.limit });
   return actionFail(result.error || 'Failed to list messages');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] listMessages', error, { conversationId });
  return actionFail('Failed to list messages');
 }
}

export async function sendConversationMessage(
 conversationId: string,
 payload: { type?: string; content: string; clientMessageId?: string; mediaPayload?: MediaPayloadDto | null }
): Promise<ActionResult<ChatMessageDto>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<ChatMessageDto>(`/conversations/${conversationId}/messages`, { method: 'POST', token: accessToken, json: payload, fallbackErrorMessage: 'Failed to send message' });
  if (!result.ok) {
   logger.error('[ChatAction] sendConversationMessage', result.error, { status: result.status, conversationId });
   return actionFail(result.error || 'Failed to send message');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] sendConversationMessage', error, { conversationId });
  return actionFail('Failed to send message');
 }
}
