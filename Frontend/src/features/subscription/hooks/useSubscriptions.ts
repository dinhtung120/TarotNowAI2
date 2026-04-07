

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getSubscriptionPlansAction, getMyEntitlementsAction, subscribeToPlanAction } from '../application/actions';
import { useWalletStore } from '@/store/walletStore';

export const subscriptionKeys = {
  all: ['subscriptions'] as const,
  plans: () => [...subscriptionKeys.all, 'plans'] as const,
  entitlements: () => [...subscriptionKeys.all, 'entitlements'] as const,
};

export const useSubscriptionPlans = () => {
  return useQuery({
    queryKey: subscriptionKeys.plans(),
    queryFn: async () => {
      const res = await getSubscriptionPlansAction();
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
    staleTime: 5 * 60 * 1000, // 5 phút cache
  });
};

export const useMyEntitlements = () => {
  return useQuery({
    queryKey: subscriptionKeys.entitlements(),
    queryFn: async () => {
      const res = await getMyEntitlementsAction();
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
  });
};

export const useSubscribe = () => {
  const queryClient = useQueryClient();

  /* Lấy hàm fetchBalance từ Zustand store để refetch wallet sau khi mua */
  const fetchWalletBalance = useWalletStore((state) => state.fetchBalance);

  return useMutation({
    mutationFn: async (req: import('../types').SubscribeRequest) => {
      const res = await subscribeToPlanAction(req);
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
    onSuccess: () => {
      /* Invalidate cache entitlements để UI cập nhật ngay quyền lợi mới mua */
      queryClient.invalidateQueries({ queryKey: subscriptionKeys.entitlements() });
      /* Refetch wallet vì đã trừ Diamond — Navbar cần hiển thị số dư mới */
      fetchWalletBalance();
    },
  });
};
