'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

export async function getHistorySessionsAction(
 page: number = 1,
 pageSize: number = 10,
 spreadType?: string,
 date?: string
) {
 const tApi = await getTranslations('ApiErrors');

 try {
  const token = await getAccessToken();
  if (!token) {
   return { error: 'unauthorized' };
  }

  let query = `page=${page}&pageSize=${pageSize}`;
  if (spreadType && spreadType !== 'all') query += `&spreadType=${encodeURIComponent(spreadType)}`;
  if (date) query += `&date=${encodeURIComponent(date)}`;

  const result = await serverHttpRequest<unknown>(`/history/sessions?${query}`, {
   method: 'GET',
   token,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return { error: 'unauthorized' };
   }
   logger.error('HistoryAction.getHistorySessionsAction', result.error, {
    status: result.status,
    page,
    pageSize,
    spreadType,
    date,
   });
   return { error: result.error || tApi('unknown_error') };
  }

  return { success: true, data: result.data };
 } catch (error) {
  logger.error('HistoryAction.getHistorySessionsAction', error, {
   page,
   pageSize,
   spreadType,
   date,
  });
  return { error: tApi('network_error') };
 }
}

export async function getHistoryDetailAction(sessionId: string) {
 const tApi = await getTranslations('ApiErrors');

 try {
  const token = await getAccessToken();
  if (!token) {
   return { error: 'unauthorized' };
  }

  const result = await serverHttpRequest<unknown>(`/history/sessions/${sessionId}`, {
   method: 'GET',
   token,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   if (result.status === 401) {
    return { error: 'unauthorized' };
   }
   if (result.status === 404) {
    return { error: tApi('not_found') };
   }
   logger.error('HistoryAction.getHistoryDetailAction', result.error, {
    status: result.status,
    sessionId,
   });
   return { error: result.error || tApi('unknown_error') };
  }

  return { success: true, data: result.data };
 } catch (error) {
  logger.error('HistoryAction.getHistoryDetailAction', error, { sessionId });
  return { error: tApi('network_error') };
 }
}
