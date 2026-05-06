'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import type { InitReadingRequest, InitReadingResponse } from './types';
import { invokeDomainCommand } from '@/shared/gateways/domainCommandRegistry';

export async function initReadingSession(data: InitReadingRequest): Promise<ActionResult<InitReadingResponse>> {
 const t = await getTranslations('ApiErrors');

 try {
  const token = await getServerAccessToken();

  if (!token) {
   return actionFail(t('unauthorized'));
  }

  const result = await invokeDomainCommand<InitReadingResponse>('reading.session.init', {
   path: '/reading/init',
   token,
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
