'use client';

import { useEffect } from 'react';
import { getWalletBalance } from '@/features/wallet/application/actions';
import { setWalletBalanceFetcher } from '@/store/walletStore';

export function useWalletStoreBridge() {
  useEffect(() => {
    setWalletBalanceFetcher(getWalletBalance);
    return () => {
      setWalletBalanceFetcher();
    };
  }, []);
}
