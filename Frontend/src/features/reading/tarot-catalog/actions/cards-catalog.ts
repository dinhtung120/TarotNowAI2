'use server';

import { getTranslations } from 'next-intl/server';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';

export interface CardCatalogItemDto {
 id: number;
 code: string;
 nameVi: string;
 nameEn: string;
 nameZh: string;
 arcana: string;
 suit?: string | null;
 element: string;
 number: number;
 imageUrl?: string | null;
 uprightKeywords: string[];
 uprightDescription: string;
 reversedKeywords: string[];
 reversedDescription: string;
}

export async function getCardsCatalogAction(): Promise<ActionResult<CardCatalogItemDto[]>> {
 const t = await getTranslations('ApiErrors');

  try {
  const result = await serverHttpRequest<CardCatalogItemDto[]>('/reading/cards-catalog', {
   method: 'GET',
   fallbackErrorMessage: t('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return actionFail(t('unauthorized'));
   }
   logger.error('ReadingAction.getCardsCatalogAction', result.error, { status: result.status });
   return actionFail(result.error || t('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('ReadingAction.getCardsCatalogAction', error);
  return actionFail(t('network_error'));
 }
}
