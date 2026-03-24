'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { WalletBalance } from '@/types/wallet';

export async function getWalletBalance(): Promise<ActionResult<WalletBalance>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<WalletBalance>('/Wallet/balance', {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get wallet balance',
  });
  if (!result.ok) {
   logger.error('WalletAction.getWalletBalance', result.error, { status: result.status });
   return actionFail(result.error || 'Failed to get wallet balance');
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('WalletAction.getWalletBalance', error);
  return actionFail('Failed to get wallet balance');
 }
}
