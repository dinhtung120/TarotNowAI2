

import { useQuery, useMutation, useQueryClient } from '@tanstack/react-query';
import { getSubscriptionPlansAction, getMyEntitlementsAction, subscribeToPlanAction } from '../application/actions';
import { useWalletStore } from '@/store/walletStore';

const subscriptionKeys = {
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
    staleTime: 5 * 60 * 1000, 
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

  
  const fetchWalletBalance = useWalletStore((state) => state.fetchBalance);

  return useMutation({
    mutationFn: async (req: import('../types').SubscribeRequest) => {
      const res = await subscribeToPlanAction(req);
      if (!res.success) throw new Error(res.error);
      return res.data;
    },
    onSuccess: () => {
      
      queryClient.invalidateQueries({ queryKey: subscriptionKeys.entitlements() });
      
      fetchWalletBalance();
    },
  });
};
