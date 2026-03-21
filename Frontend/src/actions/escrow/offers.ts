'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { EscrowStatusResult } from './types';

const getAccessToken = getServerAccessToken;

export async function acceptOffer(data: {
 readerId: string;
 conversationRef: string;
 amountDiamond: number;
 proposalMessageRef?: string;
 idempotencyKey: string;
}): Promise<{ success: boolean; itemId?: string }> {
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false };

 try {
  const result = await serverHttpRequest<{ success: boolean; itemId?: string }>('/escrow/accept', {
   method: 'POST',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to accept offer',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] acceptOffer', result.error, { status: result.status });
   return { success: false };
  }

  return result.data;
 } catch (error) {
  logger.error('[EscrowAction] acceptOffer', error);
  return { success: false };
 }
}

export async function readerReply(itemId: string): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
  const result = await serverHttpRequest<unknown>('/escrow/reply', {
   method: 'POST',
   token: accessToken,
   json: { itemId },
   fallbackErrorMessage: 'Failed to send reader reply',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] readerReply', result.error, { status: result.status, itemId });
   return false;
  }

  return true;
 } catch (error) {
  logger.error('[EscrowAction] readerReply', error, { itemId });
  return false;
 }
}

export async function confirmRelease(itemId: string): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
  const result = await serverHttpRequest<unknown>('/escrow/confirm', {
   method: 'POST',
   token: accessToken,
   json: { itemId },
   fallbackErrorMessage: 'Failed to confirm release',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] confirmRelease', result.error, { status: result.status, itemId });
   return false;
  }

  return true;
 } catch (error) {
  logger.error('[EscrowAction] confirmRelease', error, { itemId });
  return false;
 }
}

export async function addQuestion(data: {
 conversationRef: string;
 amountDiamond: number;
 proposalMessageRef?: string;
 idempotencyKey: string;
}): Promise<{ success: boolean; itemId?: string }> {
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false };

 try {
  const result = await serverHttpRequest<{ success: boolean; itemId?: string }>('/escrow/add-question', {
   method: 'POST',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to add question',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] addQuestion', result.error, { status: result.status });
   return { success: false };
  }

  return result.data;
 } catch (error) {
  logger.error('[EscrowAction] addQuestion', error);
  return { success: false };
 }
}

export async function getEscrowStatus(conversationId: string): Promise<EscrowStatusResult | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

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
   return null;
  }

  return result.data;
 } catch (error) {
  logger.error('[EscrowAction] getEscrowStatus', error, { conversationId });
  return null;
 }
}
