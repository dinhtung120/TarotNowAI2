'use server';

import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { WalletPaginatedList, WalletTransaction } from '@/features/wallet/domain/types';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

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
