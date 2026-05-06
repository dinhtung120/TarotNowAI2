'use client';

import { useQuery } from '@tanstack/react-query';
import { getWalletBalanceClient } from '@/features/wallet/overview/getWalletBalanceClient';
import type { WalletBalance } from '@/features/wallet/shared/types';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { queryFnOrThrow } from '@/shared/application/utils/queryPolicy';

export function useWalletBalanceQuery() {
 return useQuery<WalletBalance>({
  queryKey: userStateQueryKeys.wallet.balance(),
  queryFn: async () => {
   const result = await getWalletBalanceClient();
   return queryFnOrThrow(result, 'Failed to get wallet balance.');
  },
 });
}
