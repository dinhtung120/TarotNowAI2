'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

export async function updateReaderProfile(data: {
 bioVi?: string;
 bioEn?: string;
 bioZh?: string;
 diamondPerQuestion?: number;
 specialties?: string[];
}): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
  const result = await serverHttpRequest<unknown>('/reader/profile', {
   method: 'PATCH',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to update reader profile',
  });

  if (!result.ok) {
   logger.error('[ReaderAction] updateReaderProfile', result.error, { status: result.status });
   return false;
  }

  return true;
 } catch (error) {
  logger.error('[ReaderAction] updateReaderProfile', error);
  return false;
 }
}

export async function updateReaderStatus(status: string): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
  const result = await serverHttpRequest<unknown>('/reader/status', {
   method: 'PATCH',
   token: accessToken,
   json: { status },
   fallbackErrorMessage: 'Failed to update reader status',
  });

  if (!result.ok) {
   logger.error('[ReaderAction] updateReaderStatus', result.error, {
    status: result.status,
    readerStatus: status,
   });
   return false;
  }

  return true;
 } catch (error) {
  logger.error('[ReaderAction] updateReaderStatus', error, { readerStatus: status });
  return false;
 }
}
