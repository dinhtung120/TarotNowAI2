'use client';

import { useCallback, useEffect, useMemo, useState } from 'react';
import {
  getNotifications,
  markNotificationAsRead,
  type NotificationListResponse,
} from '@/actions/notificationActions';

export function useNotificationsPage() {
  const [data, setData] = useState<NotificationListResponse | null>(null);
  const [loading, setLoading] = useState(true);
  const [page, setPage] = useState(1);
  const [filterUnread, setFilterUnread] = useState(false);
  const pageSize = 20;

  const fetchData = useCallback(async () => {
    setLoading(true);
    const result = await getNotifications(
      page,
      pageSize,
      filterUnread ? false : undefined
    );
    setData(result);
    setLoading(false);
  }, [page, filterUnread]);

  useEffect(() => {
    const initialFetchTimer = window.setTimeout(() => {
      void fetchData();
    }, 0);

    return () => {
      window.clearTimeout(initialFetchTimer);
    };
  }, [fetchData]);

  const markAsRead = async (id: string) => {
    const result = await markNotificationAsRead(id);
    if (result.success) {
      setData((prev) => {
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
