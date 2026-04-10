'use client';

import { useLocale } from 'next-intl';
import { useQueryClient } from '@tanstack/react-query';
import { useEffect, useRef } from 'react';
import { useAuthStore } from '@/store/authStore';
import { useWalletStore } from '@/store/walletStore';
import { CHECKIN_QUERY_KEYS } from '@/features/checkin/application/hooks';
import type { ActionResult } from '@/shared/domain/actionResult';
import type { UserMetadataDto } from '@/shared/application/actions/metadata';

let metadataFetchInFlight = false;
let metadataFetchCompleted = false;

async function getInitialMetadataClient(locale: string): Promise<ActionResult<UserMetadataDto>> {
  const url = `/${locale}/api/user-context/metadata`;
  const response = await fetch(url, {
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
  });
  return (await response.json()) as ActionResult<UserMetadataDto>;
}

export function useInitialMetadata() {
  const locale = useLocale();
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const queryClient = useQueryClient();
  const setBalance = useWalletStore((state) => state.setBalance);
  const hasFetchedRef = useRef(false);

  useEffect(() => {
    if (!isAuthenticated) {
      hasFetchedRef.current = false;
      metadataFetchInFlight = false;
      metadataFetchCompleted = false;
      return;
    }

    if (hasFetchedRef.current || metadataFetchInFlight || metadataFetchCompleted) return;

    const existingStreak = queryClient.getQueryData(CHECKIN_QUERY_KEYS.streakStatus);
    if (existingStreak) {
      hasFetchedRef.current = true;
      metadataFetchCompleted = true;
      return;
    }

    async function fetchMetadata() {
      metadataFetchInFlight = true;
      try {
        const result = await getInitialMetadataClient(locale);
        
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
          metadataFetchCompleted = true;
          return;
        }
      } catch (error) {
        console.error('[useInitialMetadata] Batch fetch error', error);
      } finally {
        metadataFetchInFlight = false;
      }
    }

    void fetchMetadata();
  }, [isAuthenticated, queryClient, setBalance]);
}
