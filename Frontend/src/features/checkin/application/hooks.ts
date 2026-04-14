'use client';

import { useQuery } from '@tanstack/react-query';
import { getStreakStatus } from './actions';
import { checkinQueryKeys } from '@/features/checkin/domain/checkinQueryKeys';
import { useAuthStore } from '@/store/authStore';
import { AUTH_ERROR } from '@/shared/domain/authErrors';

export const useStreakStatus = (enabled: boolean = true) => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

  return useQuery({
    queryKey: checkinQueryKeys.streakStatus,
    queryFn: async () => {
      const result = await getStreakStatus();
      if (!result.success || !result.data) {
        throw new Error(result.error || 'Failed to get streak status');
      }
      return result.data;
    },
    enabled: enabled && isAuthenticated,

    retry: (failureCount, error) => {
      const message = error instanceof Error ? error.message : '';
      if (message.includes('401') || message.includes(AUTH_ERROR.UNAUTHORIZED)) {
        return false;
      }
      return failureCount < 2;
    },

    staleTime: Infinity,

    refetchOnWindowFocus: false,
  });
};
