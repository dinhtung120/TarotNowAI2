

import { useQuery } from '@tanstack/react-query';
import { getStreakStatus } from './actions';
import { useAuthStore } from '@/store/authStore';

export const CHECKIN_QUERY_KEYS = {
  streakStatus: ['streakStatus'] as const,
};

export const useStreakStatus = (enabled: boolean = true) => {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

  return useQuery({
    queryKey: CHECKIN_QUERY_KEYS.streakStatus,
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
      if (message.includes('401') || message.includes('Unauthorized')) {
        return false;
      }
      return failureCount < 2;
    },

    staleTime: Infinity,

    refetchOnWindowFocus: false,
  });
};
