'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';


export async function openDispute(itemId: string, reason: string): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

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
   return actionFail(result.error || 'Failed to open dispute');
  }

  return actionOk();
 } catch (error) {
  logger.error('[EscrowAction] openDispute', error, { itemId });
  return actionFail('Failed to open dispute');
 }
}

export async function resolveDispute(data: {
 itemId: string;
 action: 'release' | 'refund';
 adminNote?: string;
}): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<unknown>('/admin/escrow/resolve-dispute', {
   method: 'POST',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to resolve dispute',
  });

  if (!result.ok) {
   logger.error('[EscrowAction] resolveDispute', result.error, { status: result.status });
   return actionFail(result.error || 'Failed to resolve dispute');
  }

  return actionOk();
 } catch (error) {
  logger.error('[EscrowAction] resolveDispute', error);
  return actionFail('Failed to resolve dispute');
 }
}
