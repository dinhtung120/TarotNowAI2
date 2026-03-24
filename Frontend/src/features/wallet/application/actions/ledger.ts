'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { PaginatedList, WalletTransaction } from '@/types/wallet';

export async function getLedger(
 page = 1,
 limit = 20
): Promise<ActionResult<PaginatedList<WalletTransaction>>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<PaginatedList<WalletTransaction>>(
   `/Wallet/ledger?page=${page}&limit=${limit}`,
   {
    method: 'GET',
    token: accessToken,
    fallbackErrorMessage: 'Failed to get ledger',
   }
  );
  if (!result.ok) {
   logger.error('WalletAction.getLedger', result.error, { status: result.status, page, limit });
   return actionFail(result.error || 'Failed to get ledger');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('WalletAction.getLedger', error, { page, limit });
  return actionFail('Failed to get ledger');
 }
}
