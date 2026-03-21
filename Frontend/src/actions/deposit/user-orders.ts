'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { CreateDepositOrderResponse } from './types';

export async function createDepositOrder(amountVnd: number): Promise<CreateDepositOrderResponse | null> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return null;

 try {
  const result = await serverHttpRequest<CreateDepositOrderResponse>('/deposits/orders', {
   method: 'POST',
   token: accessToken,
   json: { amountVnd },
   fallbackErrorMessage: 'Failed to create deposit order',
  });

  if (!result.ok) {
   logger.error('DepositAction.createDepositOrder', result.error, {
    status: result.status,
    amountVnd,
   });
   return null;
  }

  return result.data;
 } catch (error) {
  logger.error('DepositAction.createDepositOrder', error, { amountVnd });
  return null;
 }
}
