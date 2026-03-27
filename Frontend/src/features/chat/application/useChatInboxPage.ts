'use client';

import { useEffect, useState } from 'react';
import { useQuery } from '@tanstack/react-query';
import { listConversations, type ConversationDto } from '@/features/chat/application/actions';

export function useChatInboxPage() {
  const [nowTs, setNowTs] = useState(0);

 const { data, isLoading, isFetching } = useQuery({
  queryKey: ['chat', 'inbox', 'all'],
  queryFn: async () => {
   const result = await listConversations('all');
   if (result.success && result.data) {
    return result.data;
   }
   return {
    conversations: [] as ConversationDto[],
    currentUserId: '',
    totalCount: 0,
   };
  },
 });

  useEffect(() => {
    const updateNow = () => setNowTs(Date.now());
    updateNow();
    const timer = window.setInterval(updateNow, 60000);
    return () => window.clearInterval(timer);
  }, []);

  return {
    conversations: data?.conversations ?? [],
    loading: isLoading || isFetching,
    currentUserId: data?.currentUserId ?? '',
    nowTs,
  };
}
