'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { randomUUID } from 'node:crypto';

export type InboxTab = 'active' | 'pending' | 'completed' | 'all';
export type CompletionStatus =
 | 'pending'
 | 'awaiting_acceptance'
 | 'ongoing'
 | 'completed'
 | 'cancelled'
 | 'expired'
 | 'disputed';

export interface ConversationConfirmDto {
 userAt?: string | null;
 readerAt?: string | null;
 requestedBy?: string | null;
 requestedAt?: string | null;
 autoResolveAt?: string | null;
}

export interface ConversationDto {
 id: string;
 userId: string;
 readerId: string;
 userName?: string | null;
 userAvatar?: string | null;
 readerName?: string | null;
 readerAvatar?: string | null;
 readerStatus?: string | null;
 escrowTotalFrozen?: number;
 escrowStatus?: string | null;
 status: CompletionStatus;
 lastMessageAt?: string | null;
 lastMessagePreview?: string | null;
 offerExpiresAt?: string | null;
 slaHours?: number;
 confirm?: ConversationConfirmDto | null;
 unreadCountUser: number;
 unreadCountReader: number;
 createdAt: string;
 updatedAt?: string | null;
}

export interface MediaPayloadDto {
 url: string;
 mimeType?: string | null;
 sizeBytes?: number | null;
 durationMs?: number | null;
 width?: number | null;
 height?: number | null;
 thumbnailUrl?: string | null;
 description?: string | null;
 processingStatus?: string | null;
}

export interface ChatMessageDto {
 id: string;
 conversationId: string;
 senderId: string;
 type: string;
 content: string;
 paymentPayload?: {
  amountDiamond: number;
  proposalId?: string;
  expiresAt?: string;
  description?: string;
 } | null;
 mediaPayload?: MediaPayloadDto | null;
 isRead: boolean;
 createdAt: string;
}

export interface ListConversationsResult {
 conversations: ConversationDto[];
 totalCount: number;
 currentUserId: string;
}

export interface ListMessagesResult {
 messages: ChatMessageDto[];
 nextCursor?: string | null;
 conversation?: ConversationDto;
}

export interface AdminDisputeItemDto {
 id: string;
 financeSessionId: string;
 payerId: string;
 receiverId: string;
 amountDiamond: number;
 status: string;
 createdAt: string;
 updatedAt?: string | null;
}

export interface ListAdminDisputesResult {
 items: AdminDisputeItemDto[];
 totalCount: number;
 page: number;
 pageSize: number;
}

function unauthorized<T>() {
 return actionFail('Unauthorized') as ActionResult<T>;
}

export async function createConversation(
 readerId: string,
 slaHours = 12
): Promise<ActionResult<ConversationDto>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const result = await serverHttpRequest<ConversationDto>('/conversations', {
   method: 'POST',
   token: accessToken,
   json: { readerId, slaHours },
   fallbackErrorMessage: 'Failed to create conversation',
  });

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

export async function listConversations(
 tab: InboxTab = 'active',
 page = 1,
 pageSize = 20
): Promise<ActionResult<ListConversationsResult>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const query = new URLSearchParams({
   tab,
   page: page.toString(),
   pageSize: pageSize.toString(),
  });

  const result = await serverHttpRequest<ListConversationsResult>(`/conversations?${query.toString()}`, {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to list conversations',
  });

  if (!result.ok) {
   logger.error('[ChatAction] listConversations', result.error, {
    status: result.status,
    tab,
    page,
    pageSize,
   });
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
  const result = await serverHttpRequest<{ count: number }>('/conversations/unread-total', {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get unread total',
  });

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

export async function listMessages(
 conversationId: string,
 options?: { cursor?: string; limit?: number }
): Promise<ActionResult<ListMessagesResult>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const query = new URLSearchParams();
  if (options?.cursor) query.set('cursor', options.cursor);
  if (options?.limit) query.set('limit', options.limit.toString());

  const suffix = query.toString();
  const route = suffix
   ? `/conversations/${conversationId}/messages?${suffix}`
   : `/conversations/${conversationId}/messages`;

  const result = await serverHttpRequest<ListMessagesResult>(route, {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to list messages',
  });

  if (!result.ok) {
   logger.error('[ChatAction] listMessages', result.error, {
    status: result.status,
    conversationId,
    cursor: options?.cursor,
    limit: options?.limit,
   });
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
 payload: {
  type?: string;
  content: string;
  mediaPayload?: MediaPayloadDto | null;
 }
): Promise<ActionResult<ChatMessageDto>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const result = await serverHttpRequest<ChatMessageDto>(`/conversations/${conversationId}/messages`, {
   method: 'POST',
   token: accessToken,
   json: payload,
   fallbackErrorMessage: 'Failed to send message',
  });

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

export async function acceptConversation(conversationId: string): Promise<ActionResult<{ status: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const result = await serverHttpRequest<{ status: string }>(`/conversations/${conversationId}/accept`, {
   method: 'POST',
   token: accessToken,
   fallbackErrorMessage: 'Failed to accept conversation',
  });

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

export async function rejectConversation(
 conversationId: string,
 reason: string
): Promise<ActionResult<{ status: string; reason?: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const result = await serverHttpRequest<{ status: string; reason?: string }>(
   `/conversations/${conversationId}/reject`,
   {
    method: 'POST',
    token: accessToken,
    json: { reason },
    fallbackErrorMessage: 'Failed to reject conversation',
   }
  );

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

export async function cancelPendingConversation(
 conversationId: string
): Promise<ActionResult<{ status: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const result = await serverHttpRequest<{ status: string }>(`/conversations/${conversationId}/cancel`, {
   method: 'POST',
   token: accessToken,
   fallbackErrorMessage: 'Failed to cancel pending conversation',
  });

  if (!result.ok) {
   logger.error('[ChatAction] cancelPendingConversation', result.error, {
    status: result.status,
    conversationId,
   });
   return actionFail(result.error || 'Failed to cancel pending conversation');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] cancelPendingConversation', error, { conversationId });
  return actionFail('Failed to cancel pending conversation');
 }
}

export async function requestConversationComplete(
 conversationId: string
): Promise<ActionResult<{ status: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const result = await serverHttpRequest<{ status: string }>(`/conversations/${conversationId}/complete/request`, {
   method: 'POST',
   token: accessToken,
   fallbackErrorMessage: 'Failed to request completion',
  });

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

export async function respondConversationComplete(
 conversationId: string,
 accept: boolean
): Promise<ActionResult<{ status: string; accepted: boolean }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const result = await serverHttpRequest<{ status: string; accepted: boolean }>(
   `/conversations/${conversationId}/complete/respond`,
   {
    method: 'POST',
    token: accessToken,
    json: { accept },
    fallbackErrorMessage: 'Failed to respond completion',
   }
  );

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

export async function requestConversationAddMoney(
 conversationId: string,
 data: { amountDiamond: number; description: string; idempotencyKey?: string }
): Promise<ActionResult<{ messageId: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const result = await serverHttpRequest<{ messageId: string }>(`/conversations/${conversationId}/add-money/request`, {
   method: 'POST',
   token: accessToken,
    json: {
    amountDiamond: data.amountDiamond,
    description: data.description,
    idempotencyKey: data.idempotencyKey ?? randomUUID(),
   },
   fallbackErrorMessage: 'Failed to request add money',
  });

  if (!result.ok) {
   logger.error('[ChatAction] requestConversationAddMoney', result.error, { status: result.status, conversationId });
   return actionFail(result.error || 'Failed to request add money');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] requestConversationAddMoney', error, { conversationId });
  return actionFail('Failed to request add money');
 }
}

export async function respondConversationAddMoney(
 conversationId: string,
 data: { accept: boolean; offerMessageId: string; rejectReason?: string }
): Promise<ActionResult<{ accepted: boolean; messageId: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const result = await serverHttpRequest<{ accepted: boolean; messageId: string }>(
   `/conversations/${conversationId}/add-money/respond`,
   {
    method: 'POST',
    token: accessToken,
    json: data,
    fallbackErrorMessage: 'Failed to respond add money',
   }
  );

  if (!result.ok) {
   logger.error('[ChatAction] respondConversationAddMoney', result.error, { status: result.status, conversationId });
   return actionFail(result.error || 'Failed to respond add money');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] respondConversationAddMoney', error, { conversationId });
  return actionFail('Failed to respond add money');
 }
}

export async function openConversationDispute(
 conversationId: string,
 data: { reason: string; itemId?: string }
): Promise<ActionResult<{ status: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const result = await serverHttpRequest<{ status: string }>(`/conversations/${conversationId}/dispute`, {
   method: 'POST',
   token: accessToken,
   json: {
    reason: data.reason,
    itemId: data.itemId ?? null,
   },
   fallbackErrorMessage: 'Failed to open dispute',
  });

  if (!result.ok) {
   logger.error('[ChatAction] openConversationDispute', result.error, { status: result.status, conversationId });
   return actionFail(result.error || 'Failed to open dispute');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] openConversationDispute', error, { conversationId });
  return actionFail('Failed to open dispute');
 }
}

export async function listAdminDisputes(
 page = 1,
 pageSize = 20
): Promise<ActionResult<ListAdminDisputesResult>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const query = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });

  const result = await serverHttpRequest<ListAdminDisputesResult>(`/admin/disputes?${query.toString()}`, {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to list disputes',
  });

  if (!result.ok) {
   logger.error('[ChatAction] listAdminDisputes', result.error, { status: result.status, page, pageSize });
   return actionFail(result.error || 'Failed to list disputes');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] listAdminDisputes', error, { page, pageSize });
  return actionFail('Failed to list disputes');
 }
}

export async function resolveAdminDispute(
 itemId: string,
 data: { action: 'release' | 'refund' | 'split'; adminNote?: string; splitPercentToReader?: number }
): Promise<ActionResult<{ success: boolean; itemId: string; action: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 try {
  const result = await serverHttpRequest<{ success: boolean; itemId: string; action: string }>(
   `/admin/disputes/${itemId}/resolve`,
   {
    method: 'POST',
    token: accessToken,
    json: data,
    fallbackErrorMessage: 'Failed to resolve dispute',
   }
  );

  if (!result.ok) {
   logger.error('[ChatAction] resolveAdminDispute', result.error, { status: result.status, itemId });
   return actionFail(result.error || 'Failed to resolve dispute');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] resolveAdminDispute', error, { itemId });
  return actionFail('Failed to resolve dispute');
 }
}
