'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import type { MfaSetupResult } from './types';

export async function setupMfa(): Promise<ActionResult<MfaSetupResult>> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(tApi('unauthorized'));

 try {
  const result = await serverHttpRequest<MfaSetupResult>('/mfa/setup', {
   method: 'POST',
   token: accessToken,
   fallbackErrorMessage: tApi('unknown_error'),
  });
  if (!result.ok) {
   if (result.status === 401) return actionFail(tApi('unauthorized'));
   logger.error('[MFA] setupMfa', result.error, { status: result.status });
   return actionFail(result.error || tApi('unknown_error'));
  }
  return actionOk(result.data);
 } catch (error) {
  logger.error('[MFA] setupMfa', error);
  return actionFail(tApi('network_error'));
 }
}
