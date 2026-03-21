'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

export async function createPromotion(minAmountVnd: number, bonusDiamond: number): Promise<boolean> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return false;

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
   return false;
  }

  return true;
 } catch (error) {
  logger.error('PromotionAction.createPromotion', error, { minAmountVnd, bonusDiamond });
  return false;
 }
}

export async function updatePromotion(
 id: string,
 data: { minAmountVnd: number; bonusDiamond: number; isActive: boolean }
): Promise<boolean> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return false;

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
   return false;
  }

  return true;
 } catch (error) {
  logger.error('PromotionAction.updatePromotion', error, { promotionId: id });
  return false;
 }
}

export async function deletePromotion(id: string): Promise<boolean> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return false;

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
   return false;
  }

  return true;
 } catch (error) {
  logger.error('PromotionAction.deletePromotion', error, { promotionId: id });
  return false;
 }
}
