'use client';

import { useMemo, useState } from 'react';
import { useMutation, useQuery, useQueryClient } from '@tanstack/react-query';
import { applyNotificationReadPatch } from '@/features/notifications/shared/notificationCache';
import {
  getNotifications,
  markNotificationAsRead,
  type NotificationListResponse,
} from '@/features/notifications/shared/actions';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { queryFnOrThrow } from '@/shared/application/utils/queryPolicy';

export function useNotificationsPage() {
  const queryClient = useQueryClient();
  const [page, setPage] = useState(1);
  const [filterUnread, setFilterUnread] = useState(false);
  const pageSize = 20;

  const queryKey = userStateQueryKeys.notifications.list(page, filterUnread);

  const { data, isLoading, isFetching } = useQuery<NotificationListResponse | null>({
    queryKey,
    queryFn: async () => {
      const result = await getNotifications(
        page,
        pageSize,
        filterUnread ? false : undefined
      );
      return queryFnOrThrow(result, 'Failed to get notifications');
    },
    staleTime: 90_000,
    refetchOnWindowFocus: false,
    refetchOnReconnect: false,
    retry: 1,
  });

  const markReadMutation = useMutation({
    mutationFn: async (id: string) => markNotificationAsRead(id),
  });

  const loading = isLoading || isFetching;

  const markAsRead = async (id: string) => {
    const result = await markReadMutation.mutateAsync(id);
    if (result.success) {
      applyNotificationReadPatch(queryClient, { id, page, unreadOnly: filterUnread });
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
