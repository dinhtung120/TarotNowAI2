'use client';

import { useEffect } from 'react';
import { AUTH_ERROR } from '@/shared/domain/authErrors';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';

export function useAuthGuard(isAuthenticated: boolean, redirectTo = '/login'): void {
 const navigation = useOptimizedNavigation();

 useEffect(() => {
  if (isAuthenticated) {
   return;
  }

  let cancelled = false;
  const verifySession = async () => {
   try {
    const response = await fetch('/api/auth/session', {
     method: 'GET',
     credentials: 'include',
     cache: 'no-store',
    });

    if (!response.ok) {
     if (!cancelled) navigation.push(redirectTo);
     return;
    }

    const payload = (await response.json()) as { authenticated?: boolean; error?: string };
    const shouldRedirect = !payload.authenticated || payload.error === AUTH_ERROR.UNAUTHORIZED;
    if (!cancelled && shouldRedirect) {
     navigation.push(redirectTo);
    }
   } catch {
    if (!cancelled) navigation.push(redirectTo);
   }
  };

  void verifySession();
  return () => {
   cancelled = true;
  };
 }, [isAuthenticated, navigation, redirectTo]);
}
