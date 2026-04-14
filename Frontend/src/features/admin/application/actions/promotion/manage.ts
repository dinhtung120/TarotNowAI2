'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

export async function createPromotion(
 minAmountVnd: number,
 bonusDiamond: number
): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const result = await serverHttpRequest<unknown>('/admin/promotions', {
   method: 'POST',
   token: accessToken,
   json: { minAmountVnd, bonusDiamond },
   fallbackErrorMessage: 'Failed to create promotion',
  });

  if (!result.ok) {
   logger.error('PromotionAction.createPromotion', result.error, {
    status: result.status,
    minAmountVnd,
    bonusDiamond,
   });
   return actionFail(result.error || 'Failed to create promotion');
  }

  return actionOk();
 } catch (error) {
  logger.error('PromotionAction.createPromotion', error, { minAmountVnd, bonusDiamond });
  return actionFail('Failed to create promotion');
 }
}

export async function updatePromotion(
 id: string,
 data: { minAmountVnd: number; bonusDiamond: number; isActive: boolean }
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
