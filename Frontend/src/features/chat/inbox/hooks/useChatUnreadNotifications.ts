'use client';

import { useQuery } from '@tanstack/react-query';
import { fetchJsonOrThrow } from '@/shared/application/gateways/clientFetch';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { useAuthStore } from '@/features/auth/session/authStore';

interface ChatUnreadResult {
 unreadCount: number;
 loading: boolean;
}

interface UseChatUnreadNotificationsOptions {
 enabled?: boolean;
}

export function useChatUnreadNotifications(options: UseChatUnreadNotificationsOptions = {}): ChatUnreadResult {
 const enabled = options.enabled ?? true;
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

 const { data, isLoading } = useQuery({
  queryKey: userStateQueryKeys.chat.unreadBadge(),
  enabled: isAuthenticated && enabled,
  queryFn: async () => {
   const response = await fetchJsonOrThrow<{ count: number }>(
    '/api/chat/unread-count',
    {
     method: 'GET',
     credentials: 'include',
     cache: 'no-store',
    },
    'Failed to get unread chat count.',
    8_000,
   );
   return response.count ?? 0;
  },
  staleTime: 60_000,
  // Realtime event sẽ invalidate query key, tránh refetch mỗi lần focus/mount.
  refetchOnWindowFocus: false,
  refetchOnReconnect: false,
  refetchOnMount: false,
 });

 const unreadCount = data ?? 0;

 return {
  unreadCount,
  loading: isLoading,
 };
}
