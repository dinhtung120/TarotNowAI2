import { useWalletStore } from '@/features/wallet/shared/walletStore';

const WALLET_REFRESH_MIN_INTERVAL_MS = 2_000;

interface WalletBalanceChangedPayload {
 currency?: string;
 deltaAmount?: number | string;
}

function toNumber(value: number | string | undefined): number | null {
 if (typeof value === 'number' && Number.isFinite(value)) {
  return value;
 }

 if (typeof value === 'string') {
  const parsed = Number(value);
  return Number.isFinite(parsed) ? parsed : null;
 }

 return null;
}

export function applyWalletDelta(payload?: WalletBalanceChangedPayload): boolean {
 const deltaAmount = toNumber(payload?.deltaAmount);
 if (deltaAmount === null || deltaAmount === 0) {
  return false;
 }

 const normalizedCurrency = payload?.currency?.trim().toLowerCase();
 if (!normalizedCurrency) {
  return false;
 }

 const store = useWalletStore.getState();
 if (!store.balance) {
  return false;
 }

 if (normalizedCurrency === 'gold') {
  store.setBalance({
   ...store.balance,
   goldBalance: Math.max(0, store.balance.goldBalance + deltaAmount),
  });
  return true;
 }

 if (normalizedCurrency === 'diamond') {
  store.setBalance({
   ...store.balance,
   diamondBalance: Math.max(0, store.balance.diamondBalance + deltaAmount),
  });
  return true;
 }

 return false;
}

export interface PresenceWalletRefreshScheduler {
 scheduleWalletRefresh: () => void;
 dispose: () => void;
}

export function createPresenceWalletRefreshScheduler(): PresenceWalletRefreshScheduler {
 let walletRefreshTimeout: NodeJS.Timeout | null = null;
 let lastWalletRefreshAt = 0;

 return {
  scheduleWalletRefresh: () => {
   const elapsedMs = Date.now() - lastWalletRefreshAt;
   const remainingMs = Math.max(0, WALLET_REFRESH_MIN_INTERVAL_MS - elapsedMs);
   if (walletRefreshTimeout) {
    return;
   }

   walletRefreshTimeout = setTimeout(() => {
    walletRefreshTimeout = null;
    lastWalletRefreshAt = Date.now();
    void useWalletStore.getState().fetchBalance();
   }, remainingMs);
  },
  dispose: () => {
   if (walletRefreshTimeout) {
    clearTimeout(walletRefreshTimeout);
   }
  },
 };
}
