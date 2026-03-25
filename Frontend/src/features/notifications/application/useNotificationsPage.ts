'use client';

import { useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import {
  getNotifications,
  markNotificationAsRead,
  type NotificationListResponse,
} from '@/features/notifications/application/actions';

export function useNotificationsPage() {
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);
  const [filterUnread, setFilterUnread] = useState(false);
  const pageSize = 20;

  const queryKey = ['notifications', page, filterUnread] as const;

  const { data, isLoading, isFetching } = useQuery<NotificationListResponse | null>({
    queryKey,
    queryFn: async () => {
      const result = await getNotifications(
        page,
        pageSize,
        filterUnread ? false : undefined
      );
      return result.success ? result.data ?? null : null;
    },
  });

  const markReadMutation = useMutation({
    mutationFn: async (id: string) => markNotificationAsRead(id),
  });

  const loading = isLoading || isFetching;

  const markAsRead = async (id: string) => {
    const result = await markReadMutation.mutateAsync(id);
    if (result.success) {
      queryClient.setQueryData<NotificationListResponse | null>(queryKey, (prev) => {
        if (!prev) return prev;
        return {
          ...prev,
          items: prev.items.map((item) =>
            item.id === id ? { ...item, isRead: true } : item
          ),
        };
      });
    }

    return result;
  };

  const totalPages = useMemo(
    () => (data ? Math.ceil(data.totalCount / pageSize) : 0),
    [data, pageSize]
  );

  return {
    data,
    loading,
    page,
    setPage,
    filterUnread,
    setFilterUnread,
    totalPages,
    markAsRead,
  };
}
