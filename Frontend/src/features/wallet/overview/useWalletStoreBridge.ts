'use client';

import { useEffect } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { getWalletBalanceClient } from '@/features/wallet/overview/getWalletBalanceClient';
import { registerWalletQueryBridge, setWalletBalanceFetcher } from '@/features/wallet/shared/walletStore';

export function useWalletStoreBridge() {
  const queryClient = useQueryClient();

  useEffect(() => {
    setWalletBalanceFetcher(getWalletBalanceClient);
    registerWalletQueryBridge(queryClient);
    return () => {
      registerWalletQueryBridge(null);
      setWalletBalanceFetcher();
    };
  }, [queryClient]);
}
