import { useCallback } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { performClientLogoutCleanup } from '@/shared/infrastructure/auth/clientLogoutCleanup';

interface UseNavbarLogoutArgs {
  closeAvatarMenu: () => void;
  navigateToLogin: () => void;
  onLogout?: () => Promise<unknown> | unknown;
}

export function useNavbarLogout({ closeAvatarMenu, navigateToLogin, onLogout }: UseNavbarLogoutArgs) {
  const queryClient = useQueryClient();

  return useCallback(async () => {
    closeAvatarMenu();
    if (onLogout) await onLogout();
    performClientLogoutCleanup(queryClient);
    navigateToLogin();
  }, [closeAvatarMenu, navigateToLogin, onLogout, queryClient]);
}
