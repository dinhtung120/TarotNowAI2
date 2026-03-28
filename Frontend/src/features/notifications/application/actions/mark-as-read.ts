'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';

export async function markNotificationAsRead(
 notificationId: string
): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<unknown>(`/Notification/${notificationId}/read`, {
   method: 'PATCH',
   token: accessToken,
   fallbackErrorMessage: 'Failed to mark as read',
  });

  if (!result.ok) {
   logger.error('NotificationAction.markNotificationAsRead', result.error, {
    status: result.status,
    notificationId,
   });
   return actionFail(result.error || 'Failed to mark as read');
  }

  return actionOk();
 } catch (error) {
  logger.error('NotificationAction.markNotificationAsRead', error, { notificationId });
  return actionFail('Network error');
 }
}

export async function markAllNotificationsAsRead(): Promise<ActionResult<undefined>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail('Unauthorized');

 try {
  const result = await serverHttpRequest<unknown>(`/Notification/read-all`, {
   method: 'PATCH',
   token: accessToken,
   fallbackErrorMessage: 'Failed to mark all as read',
  });

  if (!result.ok) {
   logger.error('NotificationAction.markAllNotificationsAsRead', result.error, {
    status: result.status,
   });
   return actionFail(result.error || 'Failed to mark all as read');
  }

  return actionOk();
 } catch (error) {
  logger.error('NotificationAction.markAllNotificationsAsRead', error);
  return actionFail('Network error');
 }
}
