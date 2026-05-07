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

 it('leaves caches and unread badge unchanged when the notification is not cached', () => {
  const queryClient = new QueryClient();
  const dropdown = createNotificationList([createItem('n-2')]);
  const allList = createNotificationList([createItem('n-3', true)]);
  queryClient.setQueryData(userStateQueryKeys.notifications.dropdown(), dropdown);
  queryClient.setQueryData(userStateQueryKeys.notifications.list(1, false), allList);
  queryClient.setQueryData(userStateQueryKeys.notifications.unreadCount(), 4);

  const result = applyNotificationReadPatch(queryClient, { id: 'n-missing' });

  expect(result).toEqual({ wasUnread: false });
  expect(queryClient.getQueryData(userStateQueryKeys.notifications.dropdown())).toBe(dropdown);
  expect(queryClient.getQueryData(userStateQueryKeys.notifications.list(1, false))).toBe(allList);
  expect(queryClient.getQueryData(userStateQueryKeys.notifications.unreadCount())).toBe(4);
 });

 it('patches an explicit unread page without duplicating the default unread list update', () => {
  const queryClient = new QueryClient();
  queryClient.setQueryData(
   userStateQueryKeys.notifications.list(1, true),
   { ...createNotificationList([createItem('n-1')]), totalCount: 0 },
  );
  queryClient.setQueryData(userStateQueryKeys.notifications.unreadCount(), 0);

  const result = applyNotificationReadPatch(queryClient, { id: 'n-1', page: 1, unreadOnly: true });

  expect(result).toEqual({ wasUnread: true });
  expect(queryClient.getQueryData(userStateQueryKeys.notifications.list(1, true))).toMatchObject({
   items: [],
   totalCount: 0,
  });
  expect(queryClient.getQueryData(userStateQueryKeys.notifications.unreadCount())).toBe(0);
 });

 it('patches an explicit unread page and initializes a missing unread badge to zero', () => {
  const queryClient = new QueryClient();
  queryClient.setQueryData(
   userStateQueryKeys.notifications.list(2, true),
   createNotificationList([createItem('n-4')]),
  );

  const result = applyNotificationReadPatch(queryClient, { id: 'n-4', page: 2, unreadOnly: true });

  expect(result).toEqual({ wasUnread: true });
  expect(queryClient.getQueryData(userStateQueryKeys.notifications.list(2, true))).toMatchObject({
   items: [],
   totalCount: 0,
  });
  expect(queryClient.getQueryData(userStateQueryKeys.notifications.unreadCount())).toBe(0);
 });
});
