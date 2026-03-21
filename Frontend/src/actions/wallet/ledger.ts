'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { PaginatedList, WalletTransaction } from '@/types/wallet';

export async function getLedger(page = 1, limit = 20): Promise<PaginatedList<WalletTransaction> | null> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return null;

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
   return null;
  }
  return result.data;
 } catch (error) {
  logger.error('WalletAction.getLedger', error, { page, limit });
  return null;
 }
}
