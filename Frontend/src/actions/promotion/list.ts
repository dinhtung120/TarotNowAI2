'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { DepositPromotion } from './types';

export async function listPromotions(onlyActive = false): Promise<DepositPromotion[] | null> {
 const accessToken = await getServerAccessToken();

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
   return null;
  }

  return result.data;
 } catch (error) {
  logger.error('PromotionAction.listPromotions', error, { onlyActive });
  return null;
 }
}
