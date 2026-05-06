'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';

export interface UserCollectionDto {
 cardId: number;
 level: number;
 copies: number;
 currentExp: number;
 expToNextLevel: number;
 baseAtk: number;
 baseDef: number;
 bonusAtkPercent: number;
 bonusDefPercent: number;
 totalAtk: number;
 totalDef: number;
 atk: number;
 def: number;
 expGained: number;
 lastDrawnAt: string;
}

export async function getUserCollection(): Promise<ActionResult<UserCollectionDto[]>> {
 const t = await getTranslations('ApiErrors');

 try {
  const token = await getServerAccessToken();
  if (!token) {
   return actionFail(t('unauthorized'));
  }

  const result = await serverHttpRequest<UserCollectionDto[]>('/reading/collection', {
   method: 'GET',
   token,
   next: { revalidate: 0 },
   fallbackErrorMessage: t('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return actionFail(t('unauthorized'));
   }

   logger.error('CollectionAction.getUserCollection', result.error, { status: result.status });
   return actionFail(result.error || t('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('CollectionAction.getUserCollection', error);
  return actionFail(t('network_error'));
 }
}
