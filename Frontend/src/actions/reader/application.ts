'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

export interface MyReaderRequest {
 hasRequest: boolean;
 status?: string;
 introText?: string;
 adminNote?: string;
 createdAt?: string;
 reviewedAt?: string;
}

export async function submitReaderApplication(
 introText: string,
 proofDocuments: string[] = []
): Promise<{ success: boolean; message: string }> {
 const t = await getTranslations('ReaderApply');
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, message: tApi('unauthorized') };

 try {
  const result = await serverHttpRequest<{ message?: string }>('/reader/apply', {
   method: 'POST',
   token: accessToken,
   json: { introText, proofDocuments },
   fallbackErrorMessage: t('errors.submit_failed'),
  });

  if (!result.ok) {
   logger.error('[ReaderAction] submitReaderApplication', result.error, { status: result.status });
   return { success: false, message: result.error || t('errors.submit_failed') };
  }

  return { success: true, message: result.data.message || t('success.submitted') };
 } catch (error) {
  logger.error('[ReaderAction] submitReaderApplication', error);
  return { success: false, message: tApi('network_error') };
 }
}

export async function getMyReaderRequest(): Promise<MyReaderRequest | null> {
 const accessToken = await getAccessToken();
 if (!accessToken) return null;

 try {
  const result = await serverHttpRequest<MyReaderRequest>('/reader/my-request', {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get reader request',
  });

  if (!result.ok) {
   logger.error('[ReaderAction] getMyReaderRequest', result.error, { status: result.status });
   return null;
  }

  return result.data;
 } catch (error) {
  logger.error('[ReaderAction] getMyReaderRequest', error);
  return null;
 }
}
