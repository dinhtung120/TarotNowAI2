'use client';

import { useEffect } from 'react';
import { getClientSessionSnapshot } from '@/shared/infrastructure/auth/clientSessionSnapshot';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';

export function useAuthGuard(isAuthenticated: boolean, redirectTo = '/login'): void {
 const navigation = useOptimizedNavigation();

 useEffect(() => {
  if (isAuthenticated) {
   return;
  }

  let cancelled = false;
  const verifySession = async () => {
   const sessionSnapshot = await getClientSessionSnapshot({ maxAgeMs: 2_000 });
   const shouldRedirect = !sessionSnapshot.authenticated;
   if (!cancelled && shouldRedirect) {
    navigation.push(redirectTo);
   }
  };

  void verifySession();
  return () => {
   cancelled = true;
  };
 }, [isAuthenticated, navigation, redirectTo]);
}
