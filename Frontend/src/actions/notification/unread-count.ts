'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

export async function getUnreadNotificationCount(): Promise<number> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return 0;

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
   return 0;
  }

  return result.data.count ?? 0;
 } catch (error) {
  logger.error('NotificationAction.getUnreadNotificationCount', error);
  return 0;
 }
}
