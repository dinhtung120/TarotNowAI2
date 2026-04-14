'use client';

import { useEffect } from 'react';
import { useRouter } from '@/i18n/routing';
import { AUTH_ERROR } from '@/shared/domain/authErrors';

export function useAuthGuard(isAuthenticated: boolean, redirectTo = '/login'): void {
 const router = useRouter();

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
     if (!cancelled) router.push(redirectTo);
     return;
    }

    const payload = (await response.json()) as { authenticated?: boolean; error?: string };
    const shouldRedirect = !payload.authenticated || payload.error === AUTH_ERROR.UNAUTHORIZED;
    if (!cancelled && shouldRedirect) {
     router.push(redirectTo);
    }
   } catch {
    if (!cancelled) router.push(redirectTo);
   }
  };

  void verifySession();
  return () => {
   cancelled = true;
  };
 }, [isAuthenticated, redirectTo, router]);
}
