import { useCallback } from 'react';
import { useAuthStore } from '@/store/authStore';
import { useWalletStore } from '@/store/walletStore';

interface UseNavbarLogoutArgs {
  closeAvatarMenu: () => void;
  navigateToLogin: () => void;
  onLogout?: () => Promise<unknown> | unknown;
}

export function useNavbarLogout({ closeAvatarMenu, navigateToLogin, onLogout }: UseNavbarLogoutArgs) {
  return useCallback(async () => {
    closeAvatarMenu();
    if (onLogout) await onLogout();
    useWalletStore.getState().resetWallet();
    useAuthStore.getState().clearAuth();
    navigateToLogin();
  }, [closeAvatarMenu, navigateToLogin, onLogout]);
}
