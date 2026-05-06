'use server';

import { getTranslations } from 'next-intl/server';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import {
 actionFail,
 actionOk,
 type ActionResult,
} from '@/shared/models/actionResult';

export async function forgotPasswordAction(
 data: { email: string }
): Promise<ActionResult<Record<string, unknown>>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<Record<string, unknown>>('/auth/forgot-password', {
   method: 'POST',
   json: data,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[AuthAction] forgotPasswordAction', error, { email: data.email });
  return actionFail(tApi('network_error'));
 }
}

export async function resetPasswordAction(
 data: { email: string; otpCode: string; newPassword: string }
): Promise<ActionResult<Record<string, unknown>>> {
 const tApi = await getTranslations('ApiErrors');

 try {
  const result = await serverHttpRequest<Record<string, unknown>>('/auth/reset-password', {
   method: 'POST',
   json: data,
   fallbackErrorMessage: tApi('unknown_error'),
  });

  if (!result.ok) {
   return actionFail(result.error || tApi('unknown_error'));
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('[AuthAction] resetPasswordAction', error);
  return actionFail(tApi('network_error'));
 }
}
