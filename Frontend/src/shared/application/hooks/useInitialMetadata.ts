'use client';

import { useLocale } from 'next-intl';
import { useQueryClient } from '@tanstack/react-query';
import { useEffect, useRef } from 'react';
import { useAuthStore } from '@/store/authStore';
import { useWalletStore } from '@/store/walletStore';
import { checkinQueryKeys } from '@/features/checkin/domain/checkinQueryKeys';
import type { ActionResult } from '@/shared/domain/actionResult';
import type { UserMetadataDto } from '@/shared/application/actions/metadata';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';

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

    const existingStreak = queryClient.getQueryData(checkinQueryKeys.streakStatus);
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

          queryClient.setQueryData(checkinQueryKeys.streakStatus, streak);

          queryClient.setQueryData(userStateQueryKeys.notifications.unreadCount(), unreadNotificationCount);
          queryClient.setQueryData(userStateQueryKeys.notifications.dropdown(), recentNotifications);

          queryClient.setQueryData(userStateQueryKeys.chat.unreadBadge(), unreadChatCount);
          queryClient.setQueryData(userStateQueryKeys.chat.inboxActive(), activeConversations);
          
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
  }, [isAuthenticated, locale, queryClient, setBalance]);
}
