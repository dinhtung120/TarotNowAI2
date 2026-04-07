import { randomUUID } from 'node:crypto';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { ListAdminDisputesResult } from './conversations.types';

function unauthorized<T>() { return actionFail('Unauthorized') as ActionResult<T>; }

export async function requestConversationAddMoney(
 conversationId: string,
 data: { amountDiamond: number; description: string; idempotencyKey?: string },
): Promise<ActionResult<{ messageId: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<{ messageId: string }>(`/conversations/${conversationId}/add-money/request`, {
   method: 'POST',
   token: accessToken,
   json: { amountDiamond: data.amountDiamond, description: data.description, idempotencyKey: data.idempotencyKey ?? randomUUID() },
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
 data: { accept: boolean; offerMessageId: string; rejectReason?: string },
): Promise<ActionResult<{ accepted: boolean; messageId: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<{ accepted: boolean; messageId: string }>(`/conversations/${conversationId}/add-money/respond`, {
   method: 'POST',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to respond add money',
  });
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

export async function openConversationDispute(conversationId: string, data: { reason: string; itemId?: string }): Promise<ActionResult<{ status: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<{ status: string }>(`/conversations/${conversationId}/dispute`, {
   method: 'POST',
   token: accessToken,
   json: { reason: data.reason, itemId: data.itemId ?? null },
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

export async function listAdminDisputes(page = 1, pageSize = 20): Promise<ActionResult<ListAdminDisputesResult>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const query = new URLSearchParams({ page: page.toString(), pageSize: pageSize.toString() });
  const result = await serverHttpRequest<ListAdminDisputesResult>(`/admin/disputes?${query.toString()}`, { method: 'GET', token: accessToken, fallbackErrorMessage: 'Failed to list disputes' });
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
 data: { action: 'release' | 'refund' | 'split'; adminNote?: string; splitPercentToReader?: number },
): Promise<ActionResult<{ success: boolean; itemId: string; action: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await serverHttpRequest<{ success: boolean; itemId: string; action: string }>(`/admin/disputes/${itemId}/resolve`, {
   method: 'POST',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to resolve dispute',
  });
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
