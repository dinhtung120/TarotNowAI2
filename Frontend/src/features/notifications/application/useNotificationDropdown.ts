'use client';

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import {
  getNotifications,
  markNotificationAsRead,
  markAllNotificationsAsRead,
  type NotificationListResponse,
} from '@/features/notifications/application/actions';
import { getUnreadNotificationCount } from '@/features/notifications/application/actions/unread-count';

export function useNotificationDropdown() {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const queryClient = useQueryClient();

  const queryKeyList = ['notifications', 'dropdown'];
  const queryKeyCount = ['notifications', 'unread-count'];

  const { data, isLoading } = useQuery<NotificationListResponse | null>({
    queryKey: queryKeyList,
    queryFn: async () => {
      const result = await getNotifications(1, 10);
      return result.success ? result.data ?? null : null;
    },
    enabled: isAuthenticated,
    staleTime: 1000 * 60, 
  });

  const { data: unreadCount = 0 } = useQuery<number>({
    queryKey: queryKeyCount,
    queryFn: async () => {
      const result = await getUnreadNotificationCount();
      return result.success ? result.data ?? 0 : 0;
    },
    enabled: isAuthenticated,
    staleTime: 1000 * 30, 
  });

  const markReadMutation = useMutation({
    mutationFn: async (id: string) => markNotificationAsRead(id),
  });

  const markAsRead = async (id: string) => {
    queryClient.setQueryData<NotificationListResponse | null>(queryKeyList, (prev) => {
      if (!prev) return prev;
      return {
        ...prev,
        items: prev.items.map((item) =>
          item.id === id ? { ...item, isRead: true } : item
        ),
      };
    });

    queryClient.setQueryData<number>(queryKeyCount, (prev) => Math.max(0, (prev ?? 0) - 1));

    queryClient.setQueryData<NotificationListResponse | null>(['notifications', 1, false], (prev) => {
        if (!prev) return prev;
        return {
          ...prev,
          items: prev.items.map((item) =>
            item.id === id ? { ...item, isRead: true } : item
          ),
        };
      });
      queryClient.setQueryData<NotificationListResponse | null>(['notifications', 1, true], (prev) => {
        if (!prev) return prev;
        return {
          ...prev,
          items: prev.items.filter((item) => item.id !== id),
          totalCount: Math.max(0, prev.totalCount - 1)
        };
      });

    const result = await markReadMutation.mutateAsync(id);
    if (!result.success) {
      queryClient.invalidateQueries({ queryKey: ['notifications'] });
    }
    return result;
  };

  const markAllReadMutation = useMutation({
    mutationFn: async () => markAllNotificationsAsRead(),
  });

  const markAllAsRead = async () => {
    queryClient.setQueryData<NotificationListResponse | null>(queryKeyList, (prev) => {
      if (!prev) return prev;
      return {
        ...prev,
        items: prev.items.map((item) => ({ ...item, isRead: true })),
      };
    });

    queryClient.setQueryData<number>(queryKeyCount, 0);

    const result = await markAllReadMutation.mutateAsync();
    queryClient.invalidateQueries({ queryKey: ['notifications'] });
    return result;
  };

  return {
    notifications: data?.items ?? [],
    unreadCount,
    isLoading,
    markAsRead,
    markAllAsRead,
  };
}
