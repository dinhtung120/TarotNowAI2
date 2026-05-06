'use server';

import { getServerAccessToken } from '@/shared/application/gateways/serverAuth';
import { serverHttpRequest } from '@/shared/application/gateways/serverHttpClient';
import { logger } from '@/shared/application/gateways/logger';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import type { NotificationListResponse } from './types';
import { AUTH_ERROR } from "@/shared/domain/authErrors";

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
