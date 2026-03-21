'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

export async function getMfaStatus(): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
  const result = await serverHttpRequest<{ mfaEnabled?: boolean }>('/mfa/status', {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get MFA status',
  });
  if (!result.ok) {
   logger.error('[MFA] getMfaStatus', result.error, { status: result.status });
   return false;
  }
  return !!result.data.mfaEnabled;
 } catch (error) {
  logger.error('[MFA] getMfaStatus', error);
  return false;
 }
}
