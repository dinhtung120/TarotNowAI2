import { useSyncExternalStore } from 'react';
import type { WalletBalance } from '@/features/wallet/domain/types';
import type { ActionResult } from '@/shared/domain/actionResult';

type WalletBalanceFetcher = () => Promise<ActionResult<WalletBalance>>;
type WalletStoreListener = () => void;
type WalletStoreSelector<T> = (state: WalletState) => T;
type WalletStateData = Pick<WalletState, 'balance' | 'isLoading' | 'error'>;
type WalletStoreActions = Pick<WalletState, 'resetWallet' | 'fetchBalance' | 'setBalance'>;

let walletBalanceFetcher: WalletBalanceFetcher | null = null;

export function setWalletBalanceFetcher(fetcher?: WalletBalanceFetcher) {
 walletBalanceFetcher = fetcher ?? null;
}

interface WalletState {
 balance: WalletBalance | null;
 isLoading: boolean;
 error: string | null;
 resetWallet: () => void;
 fetchBalance: () => Promise<WalletBalance | null>;
 setBalance: (balance: WalletBalance) => void;
}

const walletListeners = new Set<WalletStoreListener>();

let walletData: WalletStateData = {
 balance: null,
 isLoading: false,
 error: null,
};

function notifyWalletListeners() {
 for (const listener of walletListeners) listener();
}

function createWalletSnapshot(data: WalletStateData, actions: WalletStoreActions): WalletState {
 return { ...data, ...actions };
}

function isSameWalletData(nextData: WalletStateData) {
 return (
  walletData.balance === nextData.balance &&
  walletData.isLoading === nextData.isLoading &&
  walletData.error === nextData.error
 );
}

const walletActions: WalletStoreActions = {
 resetWallet: () => {
  updateWalletData({
   balance: null,
   isLoading: false,
   error: null,
  });
 },
 fetchBalance: async () => {
  if (walletData.isLoading) return walletData.balance;

  updateWalletData({
   ...walletData,
   isLoading: true,
   error: null,
  });

  try {
   const fetcher = walletBalanceFetcher;
   if (!fetcher) {
    updateWalletData({
     ...walletData,
     isLoading: false,
     error: 'Wallet balance fetcher is not configured',
    });
    return null;
   }

   const result = await fetcher();
   if (result.success && result.data) {
     updateWalletData({
      balance: result.data,
      isLoading: false,
      error: null,
     });
    return result.data;
   }

   updateWalletData({
    balance: null,
    isLoading: false,
    error: result.error ?? 'Unknown error',
   });
   return null;
  } catch (error: unknown) {
   updateWalletData({
    balance: null,
    isLoading: false,
    error: error instanceof Error ? error.message : 'Unknown error',
   });
   return null;
  }
 },
 setBalance: (balance) => {
  updateWalletData({
   balance,
   isLoading: false,
   error: null,
  });
 },
};

let walletSnapshot = createWalletSnapshot(walletData, walletActions);

function updateWalletData(nextData: WalletStateData) {
 if (isSameWalletData(nextData)) return;
 walletData = nextData;
 walletSnapshot = createWalletSnapshot(walletData, walletActions);
 notifyWalletListeners();
}

function getWalletSnapshot() {
 return walletSnapshot;
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

export const useWalletStore = ((selector?: WalletStoreSelector<unknown>) =>
 {
  const snapshot = useSyncExternalStore(subscribeWallet, getWalletSnapshot, getWalletSnapshot);
  return selector ? selector(snapshot) : snapshot;
 }) as UseWalletStore;

useWalletStore.getState = getWalletSnapshot;
