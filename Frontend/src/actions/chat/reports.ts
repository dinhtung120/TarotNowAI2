'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

const getAccessToken = getServerAccessToken;

export async function sendReport(data: {
 targetType: string;
 targetId: string;
 conversationRef?: string;
 reason: string;
}): Promise<boolean> {
 const accessToken = await getAccessToken();
 if (!accessToken) return false;

 try {
  const result = await serverHttpRequest<unknown>('/reports', {
   method: 'POST',
   token: accessToken,
   json: data,
   fallbackErrorMessage: 'Failed to send report',
  });

  if (!result.ok) {
   logger.error('[ChatAction] sendReport', result.error, {
    status: result.status,
    targetType: data.targetType,
    targetId: data.targetId,
   });
   return false;
  }

  return true;
 } catch (error) {
  logger.error('[ChatAction] sendReport', error, {
   targetType: data.targetType,
   targetId: data.targetId,
  });
  return false;
 }
}
