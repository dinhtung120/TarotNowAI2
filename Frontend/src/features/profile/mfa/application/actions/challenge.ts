'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';

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

export async function challengeMfa(code: string): Promise<ActionResult<undefined>> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(tApi('unauthorized'));

 try {
  const result = await serverHttpRequest<unknown>('/mfa/challenge', {
   method: 'POST',
   token: accessToken,
   json: { code },
   fallbackErrorMessage: tApi('unknown_error'),
  });
  if (!result.ok) {
   if (result.status === 401) return actionFail(tApi('unauthorized'));
   if (result.status === 400) return actionFail(tApi('invalid_code'));
   logger.error('[MFA] challengeMfa', result.error, { status: result.status });
   return actionFail(result.error || tApi('unknown_error'));
  }
  return actionOk();
 } catch (error) {
  logger.error('[MFA] challengeMfa', error);
  return actionFail(tApi('network_error'));
 }
}
