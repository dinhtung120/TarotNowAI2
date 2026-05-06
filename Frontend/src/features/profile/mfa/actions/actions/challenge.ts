'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';

export async function verifyMfa(code: string): Promise<ActionResult<undefined>> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(tApi('unauthorized'));

 try {
  const result = await serverHttpRequest<unknown>('/mfa/verify', {
   method: 'POST',
   token: accessToken,
   json: { code },
   fallbackErrorMessage: tApi('unknown_error'),
  });
  if (!result.ok) {
   if (result.status === 401) return actionFail(tApi('unauthorized'));
   if (result.status === 400) return actionFail(tApi('invalid_code'));
   logger.error('[MFA] verifyMfa', result.error, { status: result.status });
   return actionFail(result.error || tApi('unknown_error'));
  }
  return actionOk();
 } catch (error) {
  logger.error('[MFA] verifyMfa', error);
  return actionFail(tApi('network_error'));
 }
}
