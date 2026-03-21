'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { ConsentStatus } from './types';

export async function checkConsentStatus(
 documentType?: string,
 version?: string
): Promise<ConsentStatus[] | null> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return null;

 try {
  const params = new URLSearchParams();
  if (documentType) params.append('documentType', documentType);
  if (version) params.append('version', version);
  const queryString = params.toString() ? `?${params.toString()}` : '';

  const result = await serverHttpRequest<ConsentStatus[]>(`/legal/consent-status${queryString}`, {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to check consent status',
  });

  if (!result.ok) {
   logger.error('LegalAction.checkConsentStatus', result.error, {
    status: result.status,
    documentType,
    version,
   });
   return null;
  }

  return result.data;
 } catch (error) {
  logger.error('LegalAction.checkConsentStatus', error, { documentType, version });
  return null;
 }
}
