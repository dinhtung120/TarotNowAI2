'use client';

import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { parseApiError } from '@/shared/infrastructure/error/parseApiError';
import { fetchJsonOrThrow, fetchWithTimeout } from '@/shared/infrastructure/http/clientFetch';
import type { NotificationListResponse } from '@/features/notifications/application/actions';
import { useAuthStore } from '@/store/authStore';

interface UseNotificationDropdownOptions {
 enabled?: boolean;
}

const NOTIFICATION_API_ROUTES = {
 list: '/api/notifications',
 unreadCount: '/api/notifications/unread-count',
 markAllRead: '/api/notifications/read-all',
 markRead: (id: string) => `/api/notifications/${encodeURIComponent(id)}/read`,
} as const;

async function fetchNotifications(page = 1, pageSize = 10): Promise<NotificationListResponse | null> {
 try {
  return await fetchJsonOrThrow<NotificationListResponse>(
   `${NOTIFICATION_API_ROUTES.list}?page=${page}&pageSize=${pageSize}`,
   {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
   },
   'Failed to get notifications.',
   8_000,
  );
 } catch {
  return null;
 }
}

async function fetchUnreadNotificationCount(): Promise<number> {
 try {
  const response = await fetchJsonOrThrow<{ count?: number }>(
   NOTIFICATION_API_ROUTES.unreadCount,
   {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
   },
   'Failed to get unread notifications.',
   8_000,
  );
  return response.count ?? 0;
 } catch {
  return 0;
 }
}

async function sendNotificationPatch(path: string): Promise<void> {
 const response = await fetchWithTimeout(
  path,
  {
   method: 'PATCH',
   credentials: 'include',
   cache: 'no-store',
  },
  8_000,
 );

 if (!response.ok) {
  throw new Error(await parseApiError(response, 'Failed to update notification state.'));
 }
}

export function useNotificationDropdown(options: UseNotificationDropdownOptions = {}) {
 const enabled = options.enabled ?? true;
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const queryClient = useQueryClient();

 const queryKeyList = ['notifications', 'dropdown'] as const;
 const queryKeyCount = ['notifications', 'unread-count'] as const;

 const { data, isLoading } = useQuery<NotificationListResponse | null>({
  queryKey: queryKeyList,
  queryFn: () => fetchNotifications(1, 10),
  enabled: isAuthenticated && enabled,
  staleTime: 60_000,
 });

 const { data: unreadCount = 0 } = useQuery<number>({
  queryKey: queryKeyCount,
  queryFn: fetchUnreadNotificationCount,
  enabled: isAuthenticated && enabled,
  staleTime: Infinity,
  refetchOnWindowFocus: false,
  refetchOnMount: false,
 });

 const markReadMutation = useMutation({
  mutationFn: (id: string) => sendNotificationPatch(NOTIFICATION_API_ROUTES.markRead(id)),
 });

 const markAsRead = async (id: string) => {
  queryClient.setQueryData<NotificationListResponse | null>(queryKeyList, (previous) => {
   if (!previous) {
    return previous;
   }

   return {
    ...previous,
    items: previous.items.map((item) => (item.id === id ? { ...item, isRead: true } : item)),
   };
  });

  queryClient.setQueryData<number>(queryKeyCount, (previous) => Math.max(0, (previous ?? 0) - 1));

  queryClient.setQueryData<NotificationListResponse | null>(['notifications', 1, false], (previous) => {
   if (!previous) {
    return previous;
   }

   return {
    ...previous,
    items: previous.items.map((item) => (item.id === id ? { ...item, isRead: true } : item)),
   };
  });

  queryClient.setQueryData<NotificationListResponse | null>(['notifications', 1, true], (previous) => {
   if (!previous) {
    return previous;
   }

   return {
    ...previous,
    items: previous.items.filter((item) => item.id !== id),
    totalCount: Math.max(0, previous.totalCount - 1),
   };
  });

  try {
   await markReadMutation.mutateAsync(id);
  } catch {
   await queryClient.invalidateQueries({ queryKey: ['notifications'] });
  }
 };

 const markAllReadMutation = useMutation({
  mutationFn: () => sendNotificationPatch(NOTIFICATION_API_ROUTES.markAllRead),
 });

 const markAllAsRead = async () => {
  queryClient.setQueryData<NotificationListResponse | null>(queryKeyList, (previous) => {
   if (!previous) {
    return previous;
   }

   return {
    ...previous,
    items: previous.items.map((item) => ({ ...item, isRead: true })),
   };
  });
  queryClient.setQueryData<number>(queryKeyCount, 0);

  try {
   await markAllReadMutation.mutateAsync();
  } finally {
   await queryClient.invalidateQueries({ queryKey: ['notifications'] });
  }
 };

 return {
  notifications: data?.items ?? [],
  unreadCount,
  isLoading,
  markAsRead,
  markAllAsRead,
 };
}
