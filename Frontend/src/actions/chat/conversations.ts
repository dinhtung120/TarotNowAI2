'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

export interface ConversationDto {
 id: string;
 userId: string;
 readerId: string;
 userName?: string | null;
 userAvatar?: string | null;
 readerName?: string;
 readerAvatar?: string;
 escrowTotalFrozen?: number;
 escrowStatus?: string;
 status: string;
 lastMessageAt?: string | null;
 unreadCountUser: number;
 unreadCountReader: number;
 createdAt: string;
 updatedAt?: string | null;
}

export interface ListConversationsResult {
 conversations: ConversationDto[];
 totalCount: number;
 currentUserId: string;
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
 } | null;
 isRead: boolean;
 createdAt: string;
}

export interface ListMessagesResult {
 messages: ChatMessageDto[];
 totalCount: number;
 conversation?: ConversationDto;
}

export async function createConversation(readerId: string): Promise<ConversationDto | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
  const result = await serverHttpRequest<ConversationDto>('/conversations', {
   method: 'POST',
   token: accessToken,
   json: { readerId },
   fallbackErrorMessage: 'Failed to create conversation',
  });

  if (!result.ok) {
   logger.error('[ChatAction] createConversation', result.error, { status: result.status, readerId });
   return null;
  }
  return result.data;
 } catch (error) {
  logger.error('[ChatAction] createConversation', error, { readerId });
  return null;
 }
}

export async function listConversations(
 role = 'user',
 page = 1,
 pageSize = 20
): Promise<ListConversationsResult | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
  const query = new URLSearchParams({
   role,
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
    role,
    page,
    pageSize,
   });
   return null;
  }

  return result.data;
 } catch (error) {
  logger.error('[ChatAction] listConversations', error, { role, page, pageSize });
  return null;
 }
}

export async function listMessages(
 conversationId: string,
 page = 1,
 pageSize = 50
): Promise<ListMessagesResult | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
  const query = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });

  const result = await serverHttpRequest<ListMessagesResult>(
   `/conversations/${conversationId}/messages?${query.toString()}`,
   {
    method: 'GET',
    token: accessToken,
    fallbackErrorMessage: 'Failed to list messages',
   }
  );

  if (!result.ok) {
   logger.error('[ChatAction] listMessages', result.error, {
    status: result.status,
    conversationId,
    page,
    pageSize,
   });
   return null;
  }

  return result.data;
 } catch (error) {
  logger.error('[ChatAction] listMessages', error, { conversationId, page, pageSize });
  return null;
 }
}

export async function getSignalRToken(): Promise<string | null> {
 const token = await getAccessToken();
 return token || null;
}
