'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { CreateDepositOrderResponse } from './types';

export async function createDepositOrder(
 amountVnd: number
): Promise<ActionResult<CreateDepositOrderResponse>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

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
   return actionFail(result.error || 'Failed to create deposit order');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('DepositAction.createDepositOrder', error, { amountVnd });
  return actionFail('Failed to create deposit order');
 }
}
