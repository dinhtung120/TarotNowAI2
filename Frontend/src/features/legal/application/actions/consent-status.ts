'use server';

import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { ConsentStatus } from './types';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

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
