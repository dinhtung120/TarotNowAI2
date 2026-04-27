'use server';

import { getTranslations } from 'next-intl/server';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import {
 actionFail,
 actionOk,
 type ActionResult,
} from '@/shared/domain/actionResult';

export async function registerAction(data: { email: string; username: string; password: string; displayName: string; dateOfBirth: string; hasConsented: boolean }): Promise<ActionResult<Record<string, unknown>>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<Record<string, unknown>>('/auth/register', {
   method: 'POST',
   json: data,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[AuthAction] registerAction', error);
  return actionFail(tApi('network_error'));
 }
}

export async function verifyEmailAction(
 data: { email: string; otpCode: string }
): Promise<ActionResult<Record<string, unknown>>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<Record<string, unknown>>('/auth/verify-email', {
   method: 'POST',
   json: data,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[AuthAction] verifyEmailAction', error, { email: data.email });
  return actionFail(tApi('network_error'));
 }
}

export async function resendVerificationEmailAction(email: string): Promise<ActionResult<undefined>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<unknown>('/auth/send-verification-email', {
   method: 'POST',
   json: { email },
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk();
 } catch (error) {
  logger.error('[AuthAction] resendVerificationEmailAction', error, { email });
  return actionFail(tApi('network_error'));
 }
}
