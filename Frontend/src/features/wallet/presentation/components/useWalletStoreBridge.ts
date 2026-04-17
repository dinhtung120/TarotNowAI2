'use client';

import { useEffect } from 'react';
import { getWalletBalanceClient } from '@/features/wallet/application/getWalletBalanceClient';
import { setWalletBalanceFetcher } from '@/store/walletStore';

export function useWalletStoreBridge() {
  useEffect(() => {
    setWalletBalanceFetcher(getWalletBalanceClient);
    return () => {
      setWalletBalanceFetcher();
    };
  }, []);
}
