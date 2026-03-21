'use client';

import { useEffect } from 'react';
import { useRouter } from '@/i18n/routing';

export function useAuthGuard(isAuthenticated: boolean, redirectTo = '/login'): void {
 const router = useRouter();

 useEffect(() => {
  if (!isAuthenticated) {
   router.push(redirectTo);
  }
 }, [isAuthenticated, redirectTo, router]);
}
