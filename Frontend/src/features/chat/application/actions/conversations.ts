'use server';

import {
 createConversation as createConversationCore,
 getUnreadConversationCount as getUnreadConversationCountCore,
 listConversations as listConversationsCore,
 listMessages as listMessagesCore,
 sendConversationMessage as sendConversationMessageCore,
} from './conversations.core';
import {
 acceptConversation as acceptConversationFlow,
 cancelPendingConversation as cancelPendingConversationFlow,
 rejectConversation as rejectConversationFlow,
 requestConversationComplete as requestConversationCompleteFlow,
 respondConversationComplete as respondConversationCompleteFlow,
 submitConversationReview as submitConversationReviewFlow,
} from './conversations.flow';
import {
 listAdminDisputes as listAdminDisputesFinance,
 openConversationDispute as openConversationDisputeFinance,
 requestConversationAddMoney as requestConversationAddMoneyFinance,
 resolveAdminDispute as resolveAdminDisputeFinance,
 respondConversationAddMoney as respondConversationAddMoneyFinance,
} from './conversations.finance';
import type {
 ChatMessageDto,
 ConversationDto,
 ListAdminDisputesResult,
 ListConversationsResult,
 ListMessagesResult,
 MediaPayloadDto,
} from './conversations.types';
import type { ActionResult } from '@/shared/domain/actionResult';

export async function createConversation(readerId: string, slaHours?: number): Promise<ActionResult<ConversationDto>> {
 return createConversationCore(readerId, slaHours);
}

export async function listConversations(tab: 'active' | 'pending' | 'completed' | 'all' = 'active', page = 1, pageSize = 20): Promise<ActionResult<ListConversationsResult>> {
 return listConversationsCore(tab, page, pageSize);
}

export async function getUnreadConversationCount(): Promise<ActionResult<{ count: number }>> {
 return getUnreadConversationCountCore();
}

export async function listMessages(conversationId: string, options?: { cursor?: string; limit?: number }): Promise<ActionResult<ListMessagesResult>> {
 return listMessagesCore(conversationId, options);
}

export async function sendConversationMessage(
 conversationId: string,
 payload: { type?: string; content: string; clientMessageId?: string; mediaPayload?: MediaPayloadDto | null },
): Promise<ActionResult<ChatMessageDto>> {
 return sendConversationMessageCore(conversationId, payload);
}

export async function acceptConversation(conversationId: string): Promise<ActionResult<{ status: string }>> {
 return acceptConversationFlow(conversationId);
}

export async function rejectConversation(
 conversationId: string,
 reason: string,
): Promise<ActionResult<{ status: string; reason?: string }>> {
 return rejectConversationFlow(conversationId, reason);
}

export async function cancelPendingConversation(conversationId: string): Promise<ActionResult<{ status: string }>> {
 return cancelPendingConversationFlow(conversationId);
}

export async function requestConversationComplete(conversationId: string): Promise<ActionResult<{ status: string }>> {
 return requestConversationCompleteFlow(conversationId);
}

export async function respondConversationComplete(
 conversationId: string,
 accept: boolean,
): Promise<ActionResult<{ status: string; accepted: boolean }>> {
 return respondConversationCompleteFlow(conversationId, accept);
}

export async function submitConversationReview(
 conversationId: string,
 payload: { rating: number; comment?: string },
): Promise<ActionResult<{ conversationId: string; readerId: string; rating: number; comment?: string; createdAt: string }>> {
 return submitConversationReviewFlow(conversationId, payload);
}

export async function requestConversationAddMoney(
 conversationId: string,
 data: { amountDiamond: number; description: string; idempotencyKey?: string },
): Promise<ActionResult<{ messageId: string }>> {
 return requestConversationAddMoneyFinance(conversationId, data);
}

export async function respondConversationAddMoney(
 conversationId: string,
 data: { accept: boolean; offerMessageId: string; rejectReason?: string },
): Promise<ActionResult<{ accepted: boolean; messageId: string }>> {
 return respondConversationAddMoneyFinance(conversationId, data);
}

export async function openConversationDispute(
 conversationId: string,
 data: { reason: string; itemId?: string },
): Promise<ActionResult<{ status: string }>> {
 return openConversationDisputeFinance(conversationId, data);
}

export async function listAdminDisputes(page = 1, pageSize = 20): Promise<ActionResult<ListAdminDisputesResult>> {
 return listAdminDisputesFinance(page, pageSize);
}

export async function resolveAdminDispute(
 itemId: string,
 data: { action: 'release' | 'refund' | 'split'; adminNote?: string; splitPercentToReader?: number },
): Promise<ActionResult<{ success: boolean; itemId: string; action: string }>> {
 return resolveAdminDisputeFinance(itemId, data);
}
