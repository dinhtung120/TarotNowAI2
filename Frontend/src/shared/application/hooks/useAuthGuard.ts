'use client';

import { useEffect } from 'react';
import { useRouter } from '@/i18n/routing';
import { useAuthStore } from '@/store/authStore';

export function useAuthGuard(isAuthenticated: boolean, redirectTo = '/login'): void {
 const router = useRouter();
 const hasHydrated = useAuthStore((state) => state._hasHydrated);

 useEffect(() => {
  if (hasHydrated && !isAuthenticated) {
   router.push(redirectTo);
  }
 }, [isAuthenticated, hasHydrated, redirectTo, router]);
}
