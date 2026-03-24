'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { RevealReadingRequest, RevealReadingResponse } from './types';

export async function revealReadingSession(data: RevealReadingRequest): Promise<ActionResult<RevealReadingResponse>> {
 const t = await getTranslations('ApiErrors');

 try {
  const token = await getServerAccessToken();

  if (!token) {
   return actionFail(t('unauthorized'));
  }

  const result = await serverHttpRequest<RevealReadingResponse>('/reading/reveal', {
   method: 'POST',
   token,
   json: data,
   fallbackErrorMessage: t('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return actionFail(t('unauthorized'));
   }
   logger.error('ReadingAction.revealReadingSession', result.error, { status: result.status });
   return actionFail(result.error || t('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('ReadingAction.revealReadingSession', error);
  return actionFail(t('network_error'));
 }
}
