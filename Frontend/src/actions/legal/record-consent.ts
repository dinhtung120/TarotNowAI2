'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

export async function recordConsent(documentType: string, version: string): Promise<boolean> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return false;

 try {
  const result = await serverHttpRequest<unknown>('/legal/consent', {
   method: 'POST',
   token: accessToken,
   json: { documentType, version },
   fallbackErrorMessage: 'Failed to record consent',
  });

  if (!result.ok) {
   logger.error('LegalAction.recordConsent', result.error, {
    status: result.status,
    documentType,
    version,
   });
   return false;
  }

  return true;
 } catch (error) {
  logger.error('LegalAction.recordConsent', error, { documentType, version });
  return false;
 }
}
