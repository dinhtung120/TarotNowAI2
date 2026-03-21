'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { WalletBalance } from '@/types/wallet';

export async function getWalletBalance(): Promise<WalletBalance | null> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return null;

 try {
  const result = await serverHttpRequest<WalletBalance>('/Wallet/balance', {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get wallet balance',
  });
  if (!result.ok) {
   logger.error('WalletAction.getWalletBalance', result.error, { status: result.status });
   return null;
  }
  return result.data;
 } catch (error) {
  logger.error('WalletAction.getWalletBalance', error);
  return null;
 }
}
