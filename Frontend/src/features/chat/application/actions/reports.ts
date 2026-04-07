'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';

export async function sendReport(data: {
 targetType: string;
 targetId: string;
 conversationRef?: string;
 reason: string;
}): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

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
   return actionFail(result.error || 'Failed to send report');
  }

  return actionOk();
 } catch (error) {
  logger.error('[ChatAction] sendReport', error, {
   targetType: data.targetType,
   targetId: data.targetId,
  });
  return actionFail('Failed to send report');
 }
}
