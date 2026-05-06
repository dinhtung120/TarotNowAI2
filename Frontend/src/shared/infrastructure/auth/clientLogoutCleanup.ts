import type { QueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/features/auth/session/authStore';
import { useWalletStore } from '@/features/wallet/shared/walletStore';
import { invalidateClientSessionSnapshot } from '@/shared/infrastructure/auth/clientSessionSnapshot';

export function performClientLogoutCleanup(queryClient: QueryClient): void {
 invalidateClientSessionSnapshot();
 queryClient.clear();
 useWalletStore.getState().resetWallet();
 useAuthStore.getState().clearAuth();
}
