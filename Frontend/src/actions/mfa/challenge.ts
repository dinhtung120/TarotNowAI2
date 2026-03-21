'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

export async function verifyMfa(code: string): Promise<{ success: boolean; error?: string }> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, error: tApi('unauthorized') };

 try {
  const result = await serverHttpRequest<unknown>('/mfa/verify', {
   method: 'POST',
   token: accessToken,
   json: { code },
   fallbackErrorMessage: tApi('unknown_error'),
  });
  if (!result.ok) {
   if (result.status === 401) return { success: false, error: tApi('unauthorized') };
   if (result.status === 400) return { success: false, error: tApi('invalid_code') };
   logger.error('[MFA] verifyMfa', result.error, { status: result.status });
   return { success: false, error: result.error || tApi('unknown_error') };
  }
  return { success: true };
 } catch (error) {
  logger.error('[MFA] verifyMfa', error);
  return { success: false, error: tApi('network_error') };
 }
}

export async function challengeMfa(code: string): Promise<{ success: boolean; error?: string }> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, error: tApi('unauthorized') };

 try {
  const result = await serverHttpRequest<unknown>('/mfa/challenge', {
   method: 'POST',
   token: accessToken,
   json: { code },
   fallbackErrorMessage: tApi('unknown_error'),
  });
  if (!result.ok) {
   if (result.status === 401) return { success: false, error: tApi('unauthorized') };
   if (result.status === 400) return { success: false, error: tApi('invalid_code') };
   logger.error('[MFA] challengeMfa', result.error, { status: result.status });
   return { success: false, error: result.error || tApi('unknown_error') };
  }
  return { success: true };
 } catch (error) {
  logger.error('[MFA] challengeMfa', error);
  return { success: false, error: tApi('network_error') };
 }
}
