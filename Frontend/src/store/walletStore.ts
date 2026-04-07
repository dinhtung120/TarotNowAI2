import { useSyncExternalStore } from 'react';
import type { WalletBalance } from '@/features/wallet/domain/types';
import type { ActionResult } from '@/shared/domain/actionResult';

type WalletBalanceFetcher = () => Promise<ActionResult<WalletBalance>>;
type WalletStoreListener = () => void;
type WalletStoreSelector<T> = (state: WalletState) => T;

let walletBalanceFetcher: WalletBalanceFetcher | null = null;

export function setWalletBalanceFetcher(fetcher?: WalletBalanceFetcher) {
 walletBalanceFetcher = fetcher ?? null;
}

interface WalletState {
 balance: WalletBalance | null;
 isLoading: boolean;
 error: string | null;
 resetWallet: () => void;
 fetchBalance: () => Promise<void>;
 setBalance: (balance: WalletBalance) => void;
}

const walletListeners = new Set<WalletStoreListener>();

const walletData: Pick<WalletState, 'balance' | 'isLoading' | 'error'> = {
 balance: null,
 isLoading: false,
 error: null,
};

function notifyWalletListeners() {
 for (const listener of walletListeners) listener();
}

function getWalletState(): WalletState {
 return {
  ...walletData,
  resetWallet: () => {
   walletData.balance = null;
   walletData.isLoading = false;
   walletData.error = null;
   notifyWalletListeners();
  },
  fetchBalance: async () => {
   if (walletData.isLoading) return;

   walletData.isLoading = true;
   walletData.error = null;
   notifyWalletListeners();

   try {
    const fetcher = walletBalanceFetcher;
    if (!fetcher) {
     walletData.error = 'Wallet balance fetcher is not configured';
     walletData.isLoading = false;
     notifyWalletListeners();
     return;
    }

    const result = await fetcher();
    if (result.success && result.data) {
     walletData.balance = result.data;
     walletData.error = null;
    } else {
     walletData.balance = null;
     walletData.error = result.error ?? 'Unknown error';
    }
   } catch (error: unknown) {
    walletData.balance = null;
    walletData.error = error instanceof Error ? error.message : 'Unknown error';
   } finally {
    walletData.isLoading = false;
    notifyWalletListeners();
   }
  },
  setBalance: (balance) => {
   walletData.balance = balance;
   walletData.isLoading = false;
   walletData.error = null;
   notifyWalletListeners();
  },
 };
}

function subscribeWallet(listener: WalletStoreListener) {
 walletListeners.add(listener);
 return () => walletListeners.delete(listener);
}

type UseWalletStore = {
 (): WalletState;
 <T>(selector: WalletStoreSelector<T>): T;
 getState: () => WalletState;
};

const identityWalletSelector = (state: WalletState) => state;

export const useWalletStore = ((selector?: WalletStoreSelector<unknown>) =>
 useSyncExternalStore(
  subscribeWallet,
  () => (selector ? selector(getWalletState()) : identityWalletSelector(getWalletState())),
  () => (selector ? selector(getWalletState()) : identityWalletSelector(getWalletState()))
 )) as UseWalletStore;

useWalletStore.getState = getWalletState;
