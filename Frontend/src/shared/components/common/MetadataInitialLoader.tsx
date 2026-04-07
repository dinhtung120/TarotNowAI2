'use client';

import { useQueryClient } from '@tanstack/react-query';
import { useRef, useEffect } from 'react';
import { useWalletStore } from '@/store/walletStore';
import { CHECKIN_QUERY_KEYS } from '@/features/checkin/application/hooks';
import type { UserMetadataDto } from '@/shared/application/actions/metadata';
import { useInitialMetadata } from '@/shared/application/hooks/useInitialMetadata';

interface MetadataInitialLoaderProps {
  initialMetadata?: UserMetadataDto | null;
}

export default function MetadataInitialLoader({ initialMetadata }: MetadataInitialLoaderProps) {
  const queryClient = useQueryClient();
  const setBalance = useWalletStore((state) => state.setBalance);
  const isHydrated = useRef(false);

  useEffect(() => {
    if (initialMetadata && !isHydrated.current) {
      console.log('[MetadataInitialLoader] SSR Hydration starting (Phase 4)...');
      
      const { 
        wallet, 
        streak, 
        unreadNotificationCount, 
        recentNotifications,
        unreadChatCount, 
        activeConversations 
      } = initialMetadata;

      setBalance(wallet);
      queryClient.setQueryData(CHECKIN_QUERY_KEYS.streakStatus, streak);
      queryClient.setQueryData(['notifications', 'unread-count'], unreadNotificationCount);
      queryClient.setQueryData(['notifications', 'dropdown'], recentNotifications);
      queryClient.setQueryData(['chat', 'unread-badge'], unreadChatCount);
      queryClient.setQueryData(['chat', 'inbox', 'active'], activeConversations);
      
      isHydrated.current = true;
      console.log('[MetadataInitialLoader] SSR Hydration completed in useEffect.');
    }
  }, [initialMetadata, queryClient, setBalance]);

  useInitialMetadata();

  return null;
}
