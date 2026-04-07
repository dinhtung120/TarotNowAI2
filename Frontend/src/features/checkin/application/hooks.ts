

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getStreakStatus, performDailyCheckIn, purchaseStreakFreeze } from './actions';
import { IPurchaseStreakFreezeCommand } from '../types/checkin.types';
import { useWalletStore } from '@/store/walletStore';
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

    staleTime: 60 * 1000,
    
    refetchInterval: 5 * 60 * 1000, 
  });
};

export const useDailyCheckIn = () => {
  const queryClient = useQueryClient();
  const fetchBalance = useWalletStore((state) => state.fetchBalance);

  return useMutation({
    mutationFn: async () => {
      const result = await performDailyCheckIn();
      if (!result.success || !result.data) {
        throw new Error(result.error || 'Check-in failed');
      }
      return result.data;
    },
    onSuccess: (data) => {
      if (!data.isAlreadyCheckedIn) {
        fetchBalance();
        queryClient.invalidateQueries({ queryKey: CHECKIN_QUERY_KEYS.streakStatus });
      }
    },
  });
};

export const usePurchaseFreeze = () => {
  const queryClient = useQueryClient();
  const fetchBalance = useWalletStore((state) => state.fetchBalance);

  return useMutation({
    mutationFn: async (payload: IPurchaseStreakFreezeCommand) => {
      const result = await purchaseStreakFreeze(payload);
      if (!result.success || !result.data) {
        throw new Error(result.error || 'Freeze purchase failed');
      }
      return result.data;
    },
    onSuccess: (data) => {
      if (data.success) {
        fetchBalance();
        queryClient.invalidateQueries({ queryKey: CHECKIN_QUERY_KEYS.streakStatus });
      }
    },
  });
};
