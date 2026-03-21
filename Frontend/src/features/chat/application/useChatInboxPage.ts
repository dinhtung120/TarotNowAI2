'use client';

import { useEffect, useState } from 'react';
import { listConversations, type ConversationDto } from '@/actions/chatActions';

export function useChatInboxPage() {
  const [conversations, setConversations] = useState<ConversationDto[]>([]);
  const [loading, setLoading] = useState(true);
  const [currentUserId, setCurrentUserId] = useState('');
  const [nowTs, setNowTs] = useState(0);

  useEffect(() => {
    const fetchConversations = async () => {
      setLoading(true);
      const result = await listConversations('all');
      if (result) {
        setConversations(result.conversations);
        setCurrentUserId(result.currentUserId);
      }
      setLoading(false);
    };

    void fetchConversations();
  }, []);

  useEffect(() => {
    const updateNow = () => setNowTs(Date.now());
    updateNow();
    const timer = window.setInterval(updateNow, 60000);
    return () => window.clearInterval(timer);
  }, []);

  return {
    conversations,
    loading,
    currentUserId,
    nowTs,
  };
}
