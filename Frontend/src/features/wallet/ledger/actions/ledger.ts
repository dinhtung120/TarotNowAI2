'use server';

import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import type { WalletPaginatedList, WalletTransaction } from '@/features/wallet/shared/types';
import { AUTH_ERROR } from "@/shared/models/authErrors";

export async function getLedger(
 page = 1,
 limit = 20
): Promise<ActionResult<WalletPaginatedList<WalletTransaction>>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const result = await serverHttpRequest<WalletPaginatedList<WalletTransaction>>(
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
