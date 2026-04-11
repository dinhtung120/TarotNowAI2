'use client';

import { useEffect, useRef } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { checkinQueryKeys } from '@/features/checkin/domain/checkinQueryKeys';
import type { UserMetadataDto } from '@/shared/application/actions/metadata';
import { useInitialMetadata } from '@/shared/application/hooks/useInitialMetadata';
import { useWalletStore } from '@/store/walletStore';

interface UseMetadataInitialLoaderArgs {
  initialMetadata?: UserMetadataDto | null;
}

export function useMetadataInitialLoader({ initialMetadata }: UseMetadataInitialLoaderArgs) {
  const queryClient = useQueryClient();
  const setBalance = useWalletStore((state) => state.setBalance);
  const isHydrated = useRef(false);

  useEffect(() => {
    if (!initialMetadata || isHydrated.current) return;

    const {
      wallet,
      streak,
      unreadNotificationCount,
      recentNotifications,
      unreadChatCount,
      activeConversations,
    } = initialMetadata;

    setBalance(wallet);
    queryClient.setQueryData(checkinQueryKeys.streakStatus, streak);
    queryClient.setQueryData(['notifications', 'unread-count'], unreadNotificationCount);
    queryClient.setQueryData(['notifications', 'dropdown'], recentNotifications);
    queryClient.setQueryData(['chat', 'unread-badge'], unreadChatCount);
    queryClient.setQueryData(['chat', 'inbox', 'active'], activeConversations);
    isHydrated.current = true;
  }, [initialMetadata, queryClient, setBalance]);

  useInitialMetadata();
}
