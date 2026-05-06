import { useSyncExternalStoreWithSelector } from 'use-sync-external-store/shim/with-selector';
import type { QueryClient } from '@tanstack/react-query';
import type { WalletBalance } from '@/features/wallet/shared/types';
import type { ActionResult } from '@/shared/models/actionResult';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';

type WalletBalanceFetcher = () => Promise<ActionResult<WalletBalance>>;
type WalletStoreListener = () => void;
type WalletStoreSelector<T> = (state: WalletState) => T;
type WalletStateData = Pick<WalletState, 'balance' | 'isLoading' | 'error'>;
type WalletStoreActions = Pick<WalletState, 'resetWallet' | 'fetchBalance' | 'setBalance'>;

const WALLET_BALANCE_QUERY_KEY = userStateQueryKeys.wallet.balance();

let walletBalanceFetcher: WalletBalanceFetcher | null = null;
let walletQueryClient: QueryClient | null = null;
let detachWalletQueryBridge: (() => void) | null = null;

export function setWalletBalanceFetcher(fetcher?: WalletBalanceFetcher) {
 walletBalanceFetcher = fetcher ?? null;
}

export function registerWalletQueryBridge(queryClient?: QueryClient | null) {
 if (detachWalletQueryBridge) {
  detachWalletQueryBridge();
  detachWalletQueryBridge = null;
 }

 walletQueryClient = queryClient ?? null;
 if (!walletQueryClient) {
  return;
 }

 syncWalletFromQueryCache();
 detachWalletQueryBridge = walletQueryClient.getQueryCache().subscribe((event) => {
  const query = event?.query;
  if (!query) {
   return;
  }

  const queryKey = query.queryKey as readonly unknown[];
  if (!isWalletBalanceQueryKey(queryKey)) {
   return;
  }

  syncWalletFromQueryCache();
 });
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
  walletQueryClient?.setQueryData(WALLET_BALANCE_QUERY_KEY, null);
  updateWalletData({
   balance: null,
   isLoading: false,
   error: null,
  });
 },
 fetchBalance: async () => {
  if (walletData.isLoading) {
   return walletData.balance;
  }

  const fetcher = walletBalanceFetcher;
  if (!fetcher) {
   updateWalletData({
    ...walletData,
    isLoading: false,
    error: 'Wallet balance fetcher is not configured',
   });
   return null;
  }

  const previousBalance = walletData.balance;
  updateWalletData({
   ...walletData,
   isLoading: true,
   error: null,
  });

  try {
   const resolvedBalance = walletQueryClient
    ? await walletQueryClient.fetchQuery({
      queryKey: WALLET_BALANCE_QUERY_KEY,
      queryFn: async () => {
       const result = await fetcher();
       if (!result.success || !result.data) {
        throw new Error(result.error ?? 'Unknown error');
       }

       return result.data;
      },
     })
    : await fetchWalletBalanceDirect(fetcher);

   updateWalletData({
    balance: resolvedBalance,
    isLoading: false,
    error: null,
   });
   return resolvedBalance;
  } catch (error: unknown) {
   updateWalletData({
    balance: previousBalance,
    isLoading: false,
    error: error instanceof Error ? error.message : 'Unknown error',
   });
   return null;
  }
 },
 setBalance: (balance) => {
  walletQueryClient?.setQueryData(WALLET_BALANCE_QUERY_KEY, balance);
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

const serverWalletSnapshot = createWalletSnapshot(
 {
  balance: null,
  isLoading: false,
  error: null,
 },
 walletActions,
);

function getServerWalletSnapshot() {
 return serverWalletSnapshot;
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
  const resolvedSelector = (selector ?? identityWalletSelector) as WalletStoreSelector<unknown>;
  return useSyncExternalStoreWithSelector(
   subscribeWallet,
   getWalletSnapshot,
   getServerWalletSnapshot,
   resolvedSelector,
   Object.is,
  );
 }) as UseWalletStore;

useWalletStore.getState = getWalletSnapshot;

const identityWalletSelector = (state: WalletState): WalletState => state;

async function fetchWalletBalanceDirect(fetcher: WalletBalanceFetcher): Promise<WalletBalance> {
 const result = await fetcher();
 if (!result.success || !result.data) {
  throw new Error(result.error ?? 'Unknown error');
 }

 if (walletQueryClient) {
  walletQueryClient.setQueryData(WALLET_BALANCE_QUERY_KEY, result.data);
 }
 return result.data;
}

function isWalletBalanceQueryKey(queryKey: readonly unknown[]): boolean {
 if (queryKey.length !== WALLET_BALANCE_QUERY_KEY.length) {
  return false;
 }

 return WALLET_BALANCE_QUERY_KEY.every((segment, index) => queryKey[index] === segment);
}

function syncWalletFromQueryCache() {
 if (!walletQueryClient) {
  return;
 }

 const cachedBalance = walletQueryClient.getQueryData<WalletBalance | null>(WALLET_BALANCE_QUERY_KEY) ?? null;
 updateWalletData({
  ...walletData,
  balance: cachedBalance,
 });
}
