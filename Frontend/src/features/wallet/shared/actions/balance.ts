'use server';

import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import type { WalletBalance } from '@/features/wallet/shared/types';
import { AUTH_ERROR } from "@/shared/models/authErrors";

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
