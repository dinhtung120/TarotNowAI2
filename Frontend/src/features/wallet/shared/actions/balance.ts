'use server';

import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { WalletBalance } from '@/features/wallet/shared/types';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

export async function getWalletBalance(): Promise<ActionResult<WalletBalance>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

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
