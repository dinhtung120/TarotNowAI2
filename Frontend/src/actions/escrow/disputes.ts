'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

export async function openDispute(itemId: string, reason: string): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
  const result = await serverHttpRequest<unknown>('/escrow/dispute', {
   method: 'POST',
   token: accessToken,
   json: { itemId, reason },
   fallbackErrorMessage: 'Failed to open dispute',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] openDispute', result.error, {
    status: result.status,
    itemId,
   });
   return false;
  }

  return true;
 } catch (error) {
  logger.error('[EscrowAction] openDispute', error, { itemId });
  return false;
 }
}

export async function resolveDispute(data: {
 itemId: string;
 action: 'release' | 'refund';
 adminNote?: string;
}): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
  const result = await serverHttpRequest<unknown>('/admin/escrow/resolve-dispute', {
   method: 'POST',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to resolve dispute',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] resolveDispute', result.error, { status: result.status });
   return false;
  }

  return true;
 } catch (error) {
  logger.error('[EscrowAction] resolveDispute', error);
  return false;
 }
}
