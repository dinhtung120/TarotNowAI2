import { useCallback } from 'react';
import type { AppRouterInstance } from 'next/dist/shared/lib/app-router-context.shared-runtime';
import { useAuthStore } from '@/store/authStore';
import { useWalletStore } from '@/store/walletStore';

interface UseNavbarLogoutArgs {
  closeAvatarMenu: () => void;
  onLogout?: () => Promise<unknown> | unknown;
  router: AppRouterInstance;
}

export function useNavbarLogout({ closeAvatarMenu, onLogout, router }: UseNavbarLogoutArgs) {
  return useCallback(async () => {
    closeAvatarMenu();
    if (onLogout) await onLogout();
    useWalletStore.getState().resetWallet();
    useAuthStore.getState().clearAuth();
    router.push('/login');
  }, [closeAvatarMenu, onLogout, router]);
}
