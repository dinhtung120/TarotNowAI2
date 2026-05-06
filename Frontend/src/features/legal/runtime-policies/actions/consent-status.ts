'use server';

import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import type { ConsentStatus } from './types';
import { AUTH_ERROR } from "@/shared/models/authErrors";

export async function checkConsentStatus(
 documentType?: string,
 version?: string
): Promise<ActionResult<ConsentStatus[]>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

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
   return actionFail(result.error || 'Failed to check consent status');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('LegalAction.checkConsentStatus', error, { documentType, version });
  return actionFail('Failed to check consent status');
 }
}
