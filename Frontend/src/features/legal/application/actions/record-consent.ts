'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

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
