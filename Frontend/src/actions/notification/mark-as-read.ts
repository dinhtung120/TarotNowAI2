'use server';

import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';
import { logger } from '@/shared/infrastructure/logging/logger';

export async function markNotificationAsRead(
 notificationId: string
): Promise<{ success: boolean; error?: string }> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return { success: false, error: 'Unauthorized' };

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
   return { success: false, error: result.error };
  }

  return { success: true };
 } catch (error) {
  logger.error('NotificationAction.markNotificationAsRead', error, { notificationId });
  return { success: false, error: 'Network error' };
 }
}
