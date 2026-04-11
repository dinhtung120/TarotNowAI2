'use server';

import type { IStreakStatusResult } from '@/features/checkin/types/checkin.types';
import type { NotificationListResponse } from '@/features/notifications/application/actions/types';
import { actionFail, actionOk, type ActionResult } from '@/shared/domain/actionResult';
import { getServerAccessToken } from '@/shared/infrastructure/auth/serverAuth';
import { serverHttpRequest } from '@/shared/infrastructure/http/serverHttpClient';

export interface NavbarSnapshotDto {
 unreadNotificationCount: number;
 unreadChatCount: number;
 streak: IStreakStatusResult;
 dropdownPreview: NotificationListResponse;
}

export async function getNavbarSnapshotAction(): Promise<ActionResult<NavbarSnapshotDto>> {
 const token = await getServerAccessToken();
 if (!token) {
  return actionFail('Unauthorized');
 }

 try {
  const result = await serverHttpRequest<NavbarSnapshotDto>('/me/navbar-snapshot', {
   method: 'GET',
   token,
   fallbackErrorMessage: 'Failed to load navbar snapshot',
  });

  if (!result.ok) {
   return actionFail(result.error || 'Failed to load navbar snapshot');
  }

  return actionOk(result.data);
 } catch {
  return actionFail('Navbar snapshot fetch error');
 }
}
