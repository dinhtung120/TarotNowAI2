'use server';

import { getTranslations } from 'next-intl/server';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import type { MfaSetupResult } from './types';

const getAccessToken = getServerAccessToken;

export async function setupMfa(): Promise<{ success: boolean; data?: MfaSetupResult; error?: string }> {
 const tApi = await getTranslations('ApiErrors');
 const accessToken = await getAccessToken();
 if (!accessToken) return { success: false, error: tApi('unauthorized') };

 try {
  const result = await serverHttpRequest<MfaSetupResult>('/mfa/setup', {
   method: 'POST',
   token: accessToken,
   fallbackErrorMessage: tApi('unknown_error'),
  });
  if (!result.ok) {
   if (result.status === 401) return { success: false, error: tApi('unauthorized') };
   logger.error('[MFA] setupMfa', result.error, { status: result.status });
   return { success: false, error: result.error || tApi('unknown_error') };
  }
  return { success: true, data: result.data };
 } catch (error) {
  logger.error('[MFA] setupMfa', error);
  return { success: false, error: tApi('network_error') };
 }
}
