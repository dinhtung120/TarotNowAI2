'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';

export async function getMfaStatus(): Promise<ActionResult<boolean>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<{ mfaEnabled?: boolean }>('/mfa/status', {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get MFA status',
  });
  if (!result.ok) {
   logger.error('[MFA] getMfaStatus', result.error, { status: result.status });
   return actionFail(result.error || 'Failed to get MFA status');
  }
  return actionOk(!!result.data.mfaEnabled);
 } catch (error) {
  logger.error('[MFA] getMfaStatus', error);
  return actionFail('Failed to get MFA status');
 }
}
