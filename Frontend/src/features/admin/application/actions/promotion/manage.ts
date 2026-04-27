'use server';

import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

export async function createPromotion(
 minAmountVnd: number,
 bonusGold: number
): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const result = await serverHttpRequest<unknown>('/admin/promotions', {
   method: 'POST',
   token: accessToken,
   json: { minAmountVnd, bonusGold },
   fallbackErrorMessage: 'Failed to create promotion',
  });

  if (!result.ok) {
   logger.error('PromotionAction.createPromotion', result.error, {
    status: result.status,
    minAmountVnd,
    bonusGold,
   });
   return actionFail(result.error || 'Failed to create promotion');
  }

 return actionOk();
 } catch (error) {
  logger.error('PromotionAction.createPromotion', error, { minAmountVnd, bonusGold });
  return actionFail('Failed to create promotion');
 }
}

export async function updatePromotion(
 id: string,
 data: { minAmountVnd: number; bonusGold: number; isActive: boolean }
): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const result = await serverHttpRequest<unknown>(`/admin/promotions/${id}`, {
   method: 'PUT',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to update promotion',
  });

  if (!result.ok) {
   logger.error('PromotionAction.updatePromotion', result.error, {
    status: result.status,
    promotionId: id,
   });
   return actionFail(result.error || 'Failed to update promotion');
  }

  return actionOk();
 } catch (error) {
  logger.error('PromotionAction.updatePromotion', error, { promotionId: id });
  return actionFail('Failed to update promotion');
 }
}

export async function deletePromotion(id: string): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const result = await serverHttpRequest<unknown>(`/admin/promotions/${id}`, {
   method: 'DELETE',
   token: accessToken,
   fallbackErrorMessage: 'Failed to delete promotion',
  });

  if (!result.ok) {
   logger.error('PromotionAction.deletePromotion', result.error, {
    status: result.status,
    promotionId: id,
   });
   return actionFail(result.error || 'Failed to delete promotion');
  }

  return actionOk();
 } catch (error) {
  logger.error('PromotionAction.deletePromotion', error, { promotionId: id });
  return actionFail('Failed to delete promotion');
 }
}
