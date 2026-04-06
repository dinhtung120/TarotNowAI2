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

/**
 * Component nạp Metadata Batch (Phase 4).
 * TỐI ƯU HÓA: Hydra dữ liệu từ Server ngay lập tức trong render phase.
 * Ngăn chặn bão request bằng cách nạp sẵn cả Count và List thông báo.
 */
export default function MetadataInitialLoader({ initialMetadata }: MetadataInitialLoaderProps) {
  const queryClient = useQueryClient();
  const setBalance = useWalletStore((state) => state.setBalance);
  const isHydrated = useRef(false);

  // 1. Hydra khi có dữ liệu từ Server (dùng useEffect để tránh cảnh báo React render phase update)
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

      // Nạp toàn bộ vào Cache Đã có sẵn trên Client an toàn sau khi render xong
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

  // 2. Fallback cho Client-side navigation hoặc token refresh
  useInitialMetadata();

  return null;
}
