'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { EscrowStatusResult } from './types';


export async function acceptOffer(data: {
 readerId: string;
 conversationRef: string;
 amountDiamond: number;
 proposalMessageRef?: string;
 idempotencyKey: string;
}): Promise<ActionResult<{ itemId?: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<{ success: boolean; itemId?: string }>('/escrow/accept', {
   method: 'POST',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to accept offer',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] acceptOffer', result.error, { status: result.status });
   return actionFail(result.error || 'Failed to accept offer');
  }

  if (!result.data.success) {
   return actionFail('Failed to accept offer');
  }

  return actionOk({ itemId: result.data.itemId });
 } catch (error) {
  logger.error('[EscrowAction] acceptOffer', error);
  return actionFail('Failed to accept offer');
 }
}

export async function readerReply(itemId: string): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<unknown>('/escrow/reply', {
   method: 'POST',
   token: accessToken,
   json: { itemId },
   fallbackErrorMessage: 'Failed to send reader reply',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] readerReply', result.error, { status: result.status, itemId });
   return actionFail(result.error || 'Failed to send reader reply');
  }

  return actionOk();
 } catch (error) {
  logger.error('[EscrowAction] readerReply', error, { itemId });
  return actionFail('Failed to send reader reply');
 }
}

export async function confirmRelease(itemId: string): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<unknown>('/escrow/confirm', {
   method: 'POST',
   token: accessToken,
   json: { itemId },
   fallbackErrorMessage: 'Failed to confirm release',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] confirmRelease', result.error, { status: result.status, itemId });
   return actionFail(result.error || 'Failed to confirm release');
  }

  return actionOk();
 } catch (error) {
  logger.error('[EscrowAction] confirmRelease', error, { itemId });
  return actionFail('Failed to confirm release');
 }
}

export async function addQuestion(data: {
 conversationRef: string;
 amountDiamond: number;
 proposalMessageRef?: string;
 idempotencyKey: string;
}): Promise<ActionResult<{ itemId?: string }>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<{ success: boolean; itemId?: string }>('/escrow/add-question', {
   method: 'POST',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to add question',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] addQuestion', result.error, { status: result.status });
   return actionFail(result.error || 'Failed to add question');
  }

  if (!result.data.success) {
   return actionFail('Failed to add question');
  }

  return actionOk({ itemId: result.data.itemId });
 } catch (error) {
  logger.error('[EscrowAction] addQuestion', error);
  return actionFail('Failed to add question');
 }
}

export async function getEscrowStatus(conversationId: string): Promise<ActionResult<EscrowStatusResult>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<EscrowStatusResult>(`/escrow/${conversationId}`, {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get escrow status',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] getEscrowStatus', result.error, {
    status: result.status,
    conversationId,
   });
   return actionFail(result.error || 'Failed to get escrow status');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[EscrowAction] getEscrowStatus', error, { conversationId });
  return actionFail('Failed to get escrow status');
 }
}
