/*
 * ===================================================================
 * FILE: hooks.ts
 * NAMESPACE: features/checkin/application/hooks
 * ===================================================================
 */

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getStreakStatus, performDailyCheckIn, purchaseStreakFreeze } from './actions';
import { IPurchaseStreakFreezeCommand } from '../types/checkin.types';
import { useWalletStore } from '@/store/walletStore';
import { useAuthStore } from '@/store/authStore';

// Keys cho React Query cache
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
    // Chỉ kích hoạt query khi user đã đăng nhập. Tránh request lãng phí 
    // khi chưa có token hoặc đang ở trang public.
    enabled: enabled && isAuthenticated,
    
    // TRẢ LỜI CÂU HỎI USER: Tại sao bị gọi nhiều lần?
    // Mặc định React Query sẽ thử lại 3 lần nếu lỗi. Với lỗi 401/403 (Auth), 
    // thử lại là vô ích nên ta tắt retry ở đây để giảm số lượng request lỗi.
    retry: (failureCount, error) => {
      const message = error instanceof Error ? error.message : '';
      if (message.includes('401') || message.includes('Unauthorized')) {
        return false;
      }
      return failureCount < 2; // Vẫn cho phép thử lại 2 lần với các lỗi network khác.
    },

    // Giữ dữ liệu "tươi" trong 1 phút để tránh việc fetch lại quá thường xuyên
    // khi components re-render hoặc chuyển tab.
    staleTime: 60 * 1000,
    
    // Khoảng thời gian làm mới định kỳ (5 phút)
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
