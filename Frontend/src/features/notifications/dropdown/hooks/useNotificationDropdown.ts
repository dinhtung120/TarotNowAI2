'use client';

import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { applyNotificationReadPatch } from '@/features/notifications/shared/notificationCache';
import { parseApiError } from '@/shared/gateways/parseApiError';
import { fetchJsonOrThrow, fetchWithTimeout } from '@/shared/gateways/clientFetch';
import type { NotificationListResponse } from '@/features/notifications/shared/actions';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';
import { useAuthStore } from '@/features/auth/public';

interface UseNotificationDropdownOptions {
 enabled?: boolean;
 open?: boolean;
}

const NOTIFICATION_API_ROUTES = {
 list: '/api/notifications',
 unreadCount: '/api/notifications/unread-count',
 markAllRead: '/api/notifications/read-all',
 markRead: (id: string) => `/api/notifications/${encodeURIComponent(id)}/read`,
} as const;

async function fetchNotifications(page = 1, pageSize = 10): Promise<NotificationListResponse> {
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
}

async function fetchUnreadNotificationCount(): Promise<number> {
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
 const open = options.open ?? enabled;
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const queryClient = useQueryClient();

 const queryKeyList = userStateQueryKeys.notifications.dropdown();
 const queryKeyCount = userStateQueryKeys.notifications.unreadCount();

 const dropdownQuery = useQuery<NotificationListResponse>({
  queryKey: queryKeyList,
  queryFn: () => fetchNotifications(1, 10),
  enabled: isAuthenticated && enabled && open,
  staleTime: 120_000,
  refetchOnWindowFocus: false,
  refetchOnMount: false,
  refetchOnReconnect: false,
 });

 const unreadCountQuery = useQuery<number>({
  queryKey: queryKeyCount,
  queryFn: fetchUnreadNotificationCount,
  enabled: isAuthenticated && enabled,
  staleTime: 90_000,
  refetchOnWindowFocus: false,
  refetchOnMount: false,
  refetchOnReconnect: false,
 });

 const markReadMutation = useMutation({
  mutationFn: (id: string) => sendNotificationPatch(NOTIFICATION_API_ROUTES.markRead(id)),
 });

 const markAsRead = async (id: string) => {
  applyNotificationReadPatch(queryClient, { id });

  try {
   await markReadMutation.mutateAsync(id);
  } catch {
   await Promise.all([
    queryClient.invalidateQueries({ queryKey: queryKeyList, exact: true }),
    queryClient.invalidateQueries({ queryKey: queryKeyCount, exact: true }),
   ]);
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
   await Promise.all([
    queryClient.invalidateQueries({ queryKey: queryKeyList, exact: true }),
    queryClient.invalidateQueries({ queryKey: queryKeyCount, exact: true }),
   ]);
  }
 };

 return {
  notifications: dropdownQuery.data?.items ?? [],
  unreadCount: unreadCountQuery.data ?? 0,
  isLoading: dropdownQuery.isLoading,
  hasLoadError: dropdownQuery.isError || unreadCountQuery.isError,
  loadErrorMessage: dropdownQuery.error instanceof Error
   ? dropdownQuery.error.message
   : unreadCountQuery.error instanceof Error
    ? unreadCountQuery.error.message
    : '',
  markAsRead,
  markAllAsRead,
  refreshDropdown: dropdownQuery.refetch,
  refreshUnreadCount: unreadCountQuery.refetch,
 };
}
