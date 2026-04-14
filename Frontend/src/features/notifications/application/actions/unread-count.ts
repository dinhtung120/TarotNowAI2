'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

export async function getUnreadNotificationCount(): Promise<ActionResult<number>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const result = await serverHttpRequest<{ count?: number }>('/Notification/unread-count', {
   method: 'GET',
   token: accessToken,
   fallbackErrorMessage: 'Failed to get unread notification count',
  });

  if (!result.ok) {
   logger.error('NotificationAction.getUnreadNotificationCount', result.error, {
    status: result.status,
   });
   return actionFail(result.error || 'Failed to get unread notification count');
  }

  return actionOk(result.data.count ?? 0);
 } catch (error) {
  logger.error('NotificationAction.getUnreadNotificationCount', error);
  return actionFail('Failed to get unread notification count');
 }
}
