'use client';

import { useQuery } from '@tanstack/react-query';
import type { WalletBalance } from '@/features/wallet/domain/types';
import { getWalletBalanceClient } from '@/features/wallet/application/getWalletBalanceClient';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';

export function useWalletBalanceQuery() {
 return useQuery<WalletBalance | null>({
  queryKey: userStateQueryKeys.wallet.balance(),
  queryFn: async () => {
   const result = await getWalletBalanceClient();
   return result.success && result.data ? result.data : null;
  },
 });
}
