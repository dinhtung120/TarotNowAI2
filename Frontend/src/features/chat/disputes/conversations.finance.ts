import { randomUUID } from 'node:crypto';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import type { ListAdminDisputesResult } from '@/features/chat/shared/actions/conversations.types';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { invokeDomainCommand } from '@/shared/application/gateways/domainCommandRegistry';

function unauthorized<T>() { return actionFail(AUTH_ERROR.UNAUTHORIZED) as ActionResult<T>; }

export async function requestConversationAddMoney(
 conversationId: string,
 data: { amountDiamond: number; description: string; idempotencyKey?: string },
): Promise<ActionResult<{ messageId: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 const idempotencyKey = data.idempotencyKey ?? randomUUID();

 logger.info('[ChatAction] requestConversationAddMoney.start', {
  conversationId,
  amountDiamond: data.amountDiamond,
  idempotencyKey,
 });

 try {
  const result = await invokeDomainCommand<{ messageId: string }>('chat.add-money.request', {
   path: `/conversations/${conversationId}/add-money/request`,
   token: accessToken,
   json: {
    amountDiamond: data.amountDiamond,
    description: data.description,
    idempotencyKey,
   },
   fallbackErrorMessage: 'Failed to request add money',
  });
  if (!result.ok) {
   logger.error('[ChatAction] requestConversationAddMoney.failed', result.error, {
    status: result.status,
    conversationId,
    idempotencyKey,
    failureClass: result.status === 503 || result.status === 504
     ? 'transport'
     : result.status === 401
      ? 'auth'
      : 'business-or-http',
   });
   return actionFail(result.error || 'Failed to request add money', { status: result.status, errorCode: result.error });
  }

  logger.info('[ChatAction] requestConversationAddMoney.succeeded', {
   conversationId,
   idempotencyKey,
   messageId: result.data?.messageId,
  });

  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] requestConversationAddMoney.error', error, { conversationId, idempotencyKey });
  return actionFail('Failed to request add money');
 }
}

export async function respondConversationAddMoney(
 conversationId: string,
 data: { accept: boolean; offerMessageId: string; rejectReason?: string },
): Promise<ActionResult<{ accepted: boolean; messageId: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();

 logger.info('[ChatAction] respondConversationAddMoney.start', {
  conversationId,
  offerMessageId: data.offerMessageId,
  accept: data.accept,
 });

 try {
  const result = await invokeDomainCommand<{ accepted: boolean; messageId: string }>('chat.add-money.respond', {
   path: `/conversations/${conversationId}/add-money/respond`,
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to respond add money',
  });
  if (!result.ok) {
   logger.error('[ChatAction] respondConversationAddMoney.failed', result.error, {
    status: result.status,
    conversationId,
    offerMessageId: data.offerMessageId,
    accept: data.accept,
    failureClass: result.status === 503 || result.status === 504
     ? 'transport'
     : result.status === 401
      ? 'auth'
      : 'business-or-http',
   });
   return actionFail(result.error || 'Failed to respond add money', { status: result.status, errorCode: result.error });
  }

  logger.info('[ChatAction] respondConversationAddMoney.succeeded', {
   conversationId,
   offerMessageId: data.offerMessageId,
   accept: data.accept,
   messageId: result.data?.messageId,
  });

  return actionOk(result.data);
 } catch (error) {
  logger.error('[ChatAction] respondConversationAddMoney.error', error, {
   conversationId,
   offerMessageId: data.offerMessageId,
   accept: data.accept,
  });
  return actionFail('Failed to respond add money');
 }
}

export async function openConversationDispute(conversationId: string, data: { reason: string; itemId?: string }): Promise<ActionResult<{ status: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return unauthorized();
 try {
  const result = await invokeDomainCommand<{ status: string }>('chat.dispute.open', {
   path: `/conversations/${conversationId}/dispute`,
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
  const result = await invokeDomainCommand<{ success: boolean; itemId: string; action: string }>('chat.dispute.resolve', {
   path: `/admin/disputes/${itemId}/resolve`,
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
