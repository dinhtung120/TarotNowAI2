import type { QueryClient } from '@tanstack/react-query';
import type { NotificationListResponse } from '@/features/notifications/shared/actions';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';

interface ApplyNotificationReadPatchOptions {
 id: string;
 page?: number;
 unreadOnly?: boolean;
}

function patchNotificationList(
 previous: NotificationListResponse | null,
 options: {
  id: string;
  unreadOnly: boolean;
  onUnreadTarget: () => void;
 },
): NotificationListResponse | null {
 if (!previous) {
  return previous;
 }

 const target = previous.items.find((item) => item.id === options.id);
 if (!target) {
  return previous;
 }

 if (!target.isRead) {
  options.onUnreadTarget();
 }

 if (options.unreadOnly) {
  return {
   ...previous,
   items: previous.items.filter((item) => item.id !== options.id),
   totalCount: Math.max(0, previous.totalCount - 1),
  };
 }

 return {
  ...previous,
  items: previous.items.map((item) => (item.id === options.id ? { ...item, isRead: true } : item)),
 };
}

function areQueryKeyValuesEqual(left: unknown, right: unknown): boolean {
 if (Object.is(left, right)) return true;
 if (Array.isArray(left) && Array.isArray(right)) {
  return left.length === right.length && left.every((value, index) => areQueryKeyValuesEqual(value, right[index]));
 }
 return false;
}

function uniqueQueryKeys(queryKeys: ReadonlyArray<readonly unknown[]>) {
 return queryKeys.filter((queryKey, index) => {
  return queryKeys.findIndex((candidate) => areQueryKeyValuesEqual(candidate, queryKey)) === index;
 });
}

export function applyNotificationReadPatch(
 queryClient: QueryClient,
 options: ApplyNotificationReadPatchOptions,
): { wasUnread: boolean } {
 const { id, page, unreadOnly = false } = options;
 let wasUnread = false;
 const markUnreadTarget = () => {
  wasUnread = true;
 };

 const queryKeys = [
  userStateQueryKeys.notifications.dropdown(),
  userStateQueryKeys.notifications.list(1, false),
  userStateQueryKeys.notifications.list(1, true),
 ];

 if (typeof page === 'number') {
  queryKeys.push(userStateQueryKeys.notifications.list(page, unreadOnly));
 }

 for (const queryKey of uniqueQueryKeys(queryKeys)) {
 const isUnreadOnlyList = queryKey[queryKey.length - 1] === 'unread';
 queryClient.setQueryData<NotificationListResponse | null>(queryKey, (previous) =>
  patchNotificationList(previous ?? null, {
    id,
    unreadOnly: isUnreadOnlyList,
    onUnreadTarget: markUnreadTarget,
   }),
  );
 }

 if (wasUnread) {
  queryClient.setQueryData<number>(
   userStateQueryKeys.notifications.unreadCount(),
   (previous) => Math.max(0, (previous ?? 0) - 1),
  );
 }

 return { wasUnread };
}
