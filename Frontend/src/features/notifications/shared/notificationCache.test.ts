import { describe, expect, it } from 'vitest';
import { QueryClient } from '@tanstack/react-query';
import { applyNotificationReadPatch } from '@/features/notifications/shared/notificationCache';
import type { NotificationListResponse } from '@/features/notifications/shared/actions';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';

function createNotificationList(items: NotificationListResponse['items']): NotificationListResponse {
 return {
  items,
  totalCount: items.length,
  page: 1,
  pageSize: 20,
 };
}

function createItem(id: string, isRead = false) {
 return {
  id,
  userId: 'user-1',
  titleVi: `Thong bao ${id}`,
  titleEn: `Notification ${id}`,
  bodyVi: 'Chi tiet',
  bodyEn: 'Details',
  type: 'system',
  isRead,
  createdAt: '2026-04-28T00:00:00.000Z',
 };
}

describe('applyNotificationReadPatch', () => {
 it('keeps dropdown, page list, unread list, and badge in sync for an unread item', () => {
  const queryClient = new QueryClient();
  queryClient.setQueryData(
   userStateQueryKeys.notifications.dropdown(),
   createNotificationList([createItem('n-1'), createItem('n-2', true)]),
  );
  queryClient.setQueryData(
   userStateQueryKeys.notifications.list(1, false),
   createNotificationList([createItem('n-1'), createItem('n-2', true)]),
  );
  queryClient.setQueryData(
   userStateQueryKeys.notifications.list(1, true),
   createNotificationList([createItem('n-1')]),
  );
  queryClient.setQueryData(userStateQueryKeys.notifications.unreadCount(), 3);

  const result = applyNotificationReadPatch(queryClient, { id: 'n-1', page: 1, unreadOnly: true });

  expect(result).toEqual({ wasUnread: true });
  expect(queryClient.getQueryData(userStateQueryKeys.notifications.unreadCount())).toBe(2);
  expect(
   queryClient.getQueryData<NotificationListResponse>(userStateQueryKeys.notifications.dropdown())?.items[0]?.isRead,
  ).toBe(true);
  expect(
   queryClient.getQueryData<NotificationListResponse>(userStateQueryKeys.notifications.list(1, false))?.items[0]?.isRead,
  ).toBe(true);
  expect(
   queryClient.getQueryData<NotificationListResponse>(userStateQueryKeys.notifications.list(1, true)),
  ).toMatchObject({
   items: [],
   totalCount: 0,
  });
 });

 it('does not decrement unread badge again for an item that was already read', () => {
  const queryClient = new QueryClient();
  queryClient.setQueryData(
   userStateQueryKeys.notifications.dropdown(),
   createNotificationList([createItem('n-2', true)]),
  );
  queryClient.setQueryData(userStateQueryKeys.notifications.unreadCount(), 1);

  const result = applyNotificationReadPatch(queryClient, { id: 'n-2' });

  expect(result).toEqual({ wasUnread: false });
  expect(queryClient.getQueryData(userStateQueryKeys.notifications.unreadCount())).toBe(1);
 });
});
