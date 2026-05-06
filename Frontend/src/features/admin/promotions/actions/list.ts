'use server';

import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import type { DepositPromotion } from './types';
import { AUTH_ERROR } from "@/shared/models/authErrors";

export async function listPromotions(onlyActive = false): Promise<ActionResult<DepositPromotion[]>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const result = await serverHttpRequest<DepositPromotion[]>(`/admin/promotions?onlyActive=${onlyActive}`, {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to list promotions',
  });

  if (!result.ok) {
   logger.error('PromotionAction.listPromotions', result.error, {
    status: result.status,
    onlyActive,
   });
   return actionFail(result.error || 'Failed to list promotions');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('PromotionAction.listPromotions', error, { onlyActive });
  return actionFail('Failed to list promotions');
 }
}
