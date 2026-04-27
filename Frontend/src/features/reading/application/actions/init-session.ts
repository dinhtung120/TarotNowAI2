'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { EVENT_CONTRACTS } from '@/shared/domain/eventContracts';
import type { InitReadingRequest, InitReadingResponse } from './types';

export async function initReadingSession(data: InitReadingRequest): Promise<ActionResult<InitReadingResponse>> {
 const t = await getTranslations('ApiErrors');

 try {
  const token = await getServerAccessToken();

  if (!token) {
   return actionFail(t('unauthorized'));
  }

  const result = await serverHttpRequest<InitReadingResponse>('/reading/init', {
   method: 'POST',
   token,
   expectedDomainEvents: EVENT_CONTRACTS.readingInit,
   json: data,
   fallbackErrorMessage: t('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return actionFail(t('unauthorized'));
   }
   logger.error('ReadingAction.initReadingSession', result.error, { status: result.status });
   return actionFail(result.error || t('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('ReadingAction.initReadingSession', error);
  return actionFail(t('network_error'));
 }
}
