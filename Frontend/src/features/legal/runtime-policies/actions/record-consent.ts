'use server';

import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import { AUTH_ERROR } from "@/shared/models/authErrors";

export async function recordConsent(
 documentType: string,
 version: string
): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

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
   return actionFail(result.error || 'Failed to record consent');
  }

  return actionOk();
 } catch (error) {
  logger.error('LegalAction.recordConsent', error, { documentType, version });
  return actionFail('Failed to record consent');
 }
}
