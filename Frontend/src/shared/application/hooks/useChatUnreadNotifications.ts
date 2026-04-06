'use client';

import { useEffect, useRef } from 'react';
import { useQuery } from '@tanstack/react-query';
import { getUnreadConversationCount } from '@/features/chat/application/actions';
import { useAuthStore } from '@/store/authStore';

interface ChatUnreadResult {
 unreadCount: number;
 loading: boolean;
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

export function useChatUnreadNotifications(): ChatUnreadResult {
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const previousUnreadRef = useRef<number | null>(null);

 const { data, isLoading } = useQuery({
  queryKey: ['chat', 'unread-badge'],
  enabled: isAuthenticated,
  queryFn: async () => {
   const result = await getUnreadConversationCount();
   if (result.success && result.data) {
    return result.data.count;
   }
   return 0;
  },
  staleTime: Infinity,
  refetchOnWindowFocus: false,
  refetchOnReconnect: false,
  refetchOnMount: false,
 });

 const unreadCount = data ?? 0;

 useEffect(() => {
  const previous = previousUnreadRef.current;
  if (previous === null) {
   previousUnreadRef.current = unreadCount;
   return;
  }

  if (unreadCount > previous) {
   maybeNotifyBrowser(unreadCount - previous, unreadCount);
  }

  previousUnreadRef.current = unreadCount;
 }, [unreadCount]);

 return {
  unreadCount,
  loading: isLoading,
 };
}
