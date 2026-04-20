'use server';

import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type {
 CreateDepositOrderResponse,
 DepositPackageResponse,
 MyDepositOrderResponse,
} from './types';

export async function listDepositPackages(): Promise<ActionResult<DepositPackageResponse[]>> {
 try {
  const result = await serverHttpRequest<DepositPackageResponse[]>('/deposits/packages', {
   method: 'GET',
   fallbackErrorMessage: 'Failed to list deposit packages',
  });

  if (!result.ok) {
   logger.error('DepositAction.listDepositPackages', result.error, { status: result.status });
   return actionFail(result.error || 'Failed to list deposit packages');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('DepositAction.listDepositPackages', error);
  return actionFail('Failed to list deposit packages');
 }
}

export async function createDepositOrder(
 packageCode: string,
 idempotencyKey: string,
): Promise<ActionResult<CreateDepositOrderResponse>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const result = await serverHttpRequest<CreateDepositOrderResponse>('/deposits/orders', {
   method: 'POST',
   token: accessToken,
   json: { packageCode, idempotencyKey },
   fallbackErrorMessage: 'Failed to create deposit order',
  });

  if (!result.ok) {
   logger.error('DepositAction.createDepositOrder', result.error, {
    status: result.status,
    packageCode,
   });
   return actionFail(result.error || 'Failed to create deposit order');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('DepositAction.createDepositOrder', error, { packageCode });
  return actionFail('Failed to create deposit order');
 }
}

export async function getMyDepositOrder(orderId: string): Promise<ActionResult<MyDepositOrderResponse>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const result = await serverHttpRequest<MyDepositOrderResponse>(`/deposits/orders/${orderId}`, {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get deposit order',
  });

  if (!result.ok) {
   logger.error('DepositAction.getMyDepositOrder', result.error, {
    status: result.status,
    orderId,
   });
   return actionFail(result.error || 'Failed to get deposit order');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('DepositAction.getMyDepositOrder', error, { orderId });
  return actionFail('Failed to get deposit order');
 }
}
