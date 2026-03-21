'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { InitReadingRequest, InitReadingResponse } from './types';

export async function initReadingSession(data: InitReadingRequest) {
 const t = await getTranslations('ApiErrors');

 try {
  const token = await getServerAccessToken();

  if (!token) {
   return { success: false, error: t('unauthorized') };
  }

  const result = await serverHttpRequest<InitReadingResponse>('/reading/init', {
   method: 'POST',
   token,
   json: data,
   fallbackErrorMessage: t('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return { success: false, error: t('unauthorized') };
   }
   logger.error('ReadingAction.initReadingSession', result.error, { status: result.status });
   return { success: false, error: result.error || t('unknown_error') };
  }

  return { success: true, data: result.data };
 } catch (error) {
  logger.error('ReadingAction.initReadingSession', error);
  return { success: false, error: t('network_error') };
 }
}
