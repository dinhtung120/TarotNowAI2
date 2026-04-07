'use client';

import { useQueryClient } from '@tanstack/react-query';
import { useEffect, useRef } from 'react';
import { useAuthStore } from '@/store/authStore';
import { useWalletStore } from '@/store/walletStore';
import { getInitialMetadata } from '@/shared/application/actions/metadata';
import { CHECKIN_QUERY_KEYS } from '@/features/checkin/application/hooks';

export function useInitialMetadata() {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const queryClient = useQueryClient();
  const setBalance = useWalletStore((state) => state.setBalance);
  const hasFetchedRef = useRef(false);

  useEffect(() => {
    if (!isAuthenticated || hasFetchedRef.current) return;

    const existingStreak = queryClient.getQueryData(CHECKIN_QUERY_KEYS.streakStatus);
    if (existingStreak) {
      console.log('[useInitialMetadata] Cache already primed. Skipping fetch.');
      hasFetchedRef.current = true;
      return;
    }

    async function fetchMetadata() {
      try {
        console.log('[useInitialMetadata] Fetching aggregate metadata (Client Fallback)...');
        const result = await getInitialMetadata();
        
        if (result.success && result.data) {
          const { 
            wallet, 
            streak, 
            unreadNotificationCount, 
            recentNotifications,
            unreadChatCount, 
            activeConversations 
          } = result.data;

          setBalance(wallet);

          queryClient.setQueryData(CHECKIN_QUERY_KEYS.streakStatus, streak);

          queryClient.setQueryData(['notifications', 'unread-count'], unreadNotificationCount);
          queryClient.setQueryData(['notifications', 'dropdown'], recentNotifications);

          queryClient.setQueryData(['chat', 'unread-badge'], unreadChatCount);
          queryClient.setQueryData(['chat', 'inbox', 'active'], activeConversations);
          
          hasFetchedRef.current = true;
        }
      } catch (error) {
        console.error('[useInitialMetadata] Batch fetch error', error);
      }
    }

    void fetchMetadata();
  }, [isAuthenticated, queryClient, setBalance]);
}
