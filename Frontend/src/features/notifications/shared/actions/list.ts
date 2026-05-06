'use server';

import { getServerAccessToken } from '@/shared/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/gateways/serverHttpClient';
import { logger } from '@/shared/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/models/actionResult';
import type { NotificationListResponse } from './types';
import { AUTH_ERROR } from "@/shared/models/authErrors";

export async function getNotifications(
 page = 1,
 pageSize = 20,
 isRead?: boolean
): Promise<ActionResult<NotificationListResponse>> {
 const accessToken = await getServerAccessToken();
 if (!accessToken) return actionFail(AUTH_ERROR.UNAUTHORIZED);

 try {
  const params = new URLSearchParams({
   page: page.toString(),
   pageSize: pageSize.toString(),
  });
  if (isRead !== undefined) {
   params.set('isRead', isRead.toString());
  }

  const result = await serverHttpRequest<NotificationListResponse>(
   `/Notification?${params.toString()}`,
   {
    method: 'GET',
    token: accessToken,
    fallbackErrorMessage: 'Failed to get notifications',
   }
  );

  if (!result.ok) {
   logger.error('NotificationAction.getNotifications', result.error, {
    status: result.status,
    page,
    pageSize,
    isRead,
   });
   return actionFail(result.error || 'Failed to get notifications');
  }

  return actionOk(result.data);
 } catch (error) {
  logger.error('NotificationAction.getNotifications', error, { page, pageSize, isRead });
  return actionFail('Failed to get notifications');
 }
}
