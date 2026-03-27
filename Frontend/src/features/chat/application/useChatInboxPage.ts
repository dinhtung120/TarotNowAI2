'use client';

import { useEffect, useMemo, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import {
 listConversations,
 type ConversationDto,
 type InboxTab,
} from '@/features/chat/application/actions';

export function useChatInboxPage(initialTab: InboxTab = 'active') {
 const [tab, setTab] = useState<InboxTab>(initialTab);
 const [nowTs, setNowTs] = useState(0);

 const { data, isLoading, refetch } = useQuery({
  queryKey: ['chat', 'inbox', tab],
  queryFn: async () => {
   const result = await listConversations(tab, 1, 100);
   if (result.success && result.data) {
    return result.data;
   }
   return {
    conversations: [] as ConversationDto[],
    currentUserId: '',
   totalCount: 0,
   };
  },
  staleTime: Infinity,
  refetchOnWindowFocus: false,
  refetchOnReconnect: false,
  refetchOnMount: false,
 });
 const conversations = useMemo(() => data?.conversations ?? [], [data?.conversations]);

 useEffect(() => {
  const updateNow = () => setNowTs(Date.now());
  updateNow();
  const timer = window.setInterval(updateNow, 60000);
  return () => window.clearInterval(timer);
 }, []);

 return {
  tab,
  setTab,
  conversations,
  loading: isLoading,
  currentUserId: data?.currentUserId ?? '',
  nowTs,
  refetch,
 };
}
