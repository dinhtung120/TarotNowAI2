'use client';

import { useEffect, useRef } from 'react';
import { useQuery } from '@tanstack/react-query';
import { usePathname } from '@/i18n/routing';
import { getUnreadConversationCount } from '@/features/chat/application/actions';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { useAuthStore } from '@/store/authStore';

interface ChatUnreadResult {
 unreadCount: number;
 loading: boolean;
}

interface UseChatUnreadNotificationsOptions {
 enabled?: boolean;
}

function maybeNotifyBrowser(diff: number, unreadCount: number) {
 if (typeof window === 'undefined' || typeof Notification === 'undefined') {
  return;
 }

 if (document.visibilityState === 'visible') {
  return;
 }

 const title = 'TarotNow Chat';
 const body = diff > 1
  ? `Bạn có thêm ${diff} tin nhắn mới (${unreadCount} chưa đọc).`
  : `Bạn có tin nhắn chat mới (${unreadCount} chưa đọc).`;

 const notify = () => {
  const notification = new Notification(title, { body });
  notification.onclick = () => {
   const segments = window.location.pathname.split('/').filter(Boolean);
   const locale = segments[0] ?? 'vi';
   window.focus();
   window.location.href = `/${locale}/chat`;
   notification.close();
  };
 };

 if (Notification.permission === 'granted') {
  notify();
  return;
 }

 if (Notification.permission === 'default') {
  void Notification.requestPermission().then((permission) => {
   if (permission === 'granted') {
    notify();
   }
  });
 }
}

export function useChatUnreadNotifications(options: UseChatUnreadNotificationsOptions = {}): ChatUnreadResult {
 const enabled = options.enabled ?? true;
 const pathname = usePathname();
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const previousUnreadRef = useRef<number | null>(null);
 const isChatRoute = /(^|\/)chat(\/|$)/.test(pathname);

 const { data, isLoading } = useQuery({
  queryKey: userStateQueryKeys.chat.unreadBadge(),
  enabled: isAuthenticated && enabled,
  queryFn: async () => {
   const result = await getUnreadConversationCount();
   if (result.success && result.data) {
    return result.data.count;
   }
   return 0;
  },
  staleTime: 15_000,
  refetchOnWindowFocus: true,
  refetchOnReconnect: true,
  refetchOnMount: true,
 });

 const unreadCount = data ?? 0;

 useEffect(() => {
  if (!enabled || isChatRoute) {
   previousUnreadRef.current = unreadCount;
   return;
  }

  const previous = previousUnreadRef.current;
  if (previous === null) {
   previousUnreadRef.current = unreadCount;
   return;
  }

  if (unreadCount > previous) {
   maybeNotifyBrowser(unreadCount - previous, unreadCount);
  }

  previousUnreadRef.current = unreadCount;
 }, [enabled, isChatRoute, unreadCount]);

 return {
  unreadCount,
  loading: isLoading,
 };
}
