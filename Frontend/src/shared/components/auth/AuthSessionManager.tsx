'use client';

import { useCallback, useEffect, useRef } from 'react';
import toast from 'react-hot-toast';
import { useTranslations } from 'next-intl';
import { usePathname, useRouter } from '@/i18n/routing';
import { useAuthStore } from '@/store/authStore';
import type { ActionResult } from '@/shared/domain/actionResult';

const REFRESH_INTERVAL_MS = 9 * 60 * 1000;
const FOCUS_REFRESH_THROTTLE_MS = 30 * 1000;

export interface AuthSessionManagerProps {
 logout: () => Promise<unknown> | unknown;
 refreshAccessToken: () => Promise<ActionResult<undefined>>;
}

export default function AuthSessionManager({
 logout,
 refreshAccessToken,
}: AuthSessionManagerProps) {
 const router = useRouter();
 const pathname = usePathname();
 const tApi = useTranslations('ApiErrors');

 const isAuthenticated = useAuthStore((s) => s.isAuthenticated);
 const clearAuth = useAuthStore((s) => s.clearAuth);
 const syncAuth = useAuthStore((s) => s.syncAuth);

 const logoutInProgressRef = useRef(false);
 const refreshInProgressRef = useRef(false);
 const lastRefreshAtRef = useRef(0);
 const pathnameRef = useRef(pathname);
 const tApiRef = useRef(tApi);

 useEffect(() => {
  pathnameRef.current = pathname;
 }, [pathname]);

 useEffect(() => {
  tApiRef.current = tApi;
 }, [tApi]);

 const runLogout = useCallback(
  async (showToast: boolean) => {
   if (logoutInProgressRef.current) return;
   logoutInProgressRef.current = true;

   try {
    await logout();
   } catch {
    // Best-effort server logout; local state is still cleared.
   } finally {
    clearAuth();
    logoutInProgressRef.current = false;
   }

	   if (showToast) {
	    toast.error(tApiRef.current('unauthorized'));
	   }

	   if (!pathnameRef.current.includes('/login')) {
	    router.push('/login');
	   }
	  },
	  [clearAuth, logout, router]
	 );

 const tryRefresh = useCallback(
  async (showToastOnFailure = false) => {
   if (refreshInProgressRef.current || logoutInProgressRef.current) return;
   refreshInProgressRef.current = true;

   try {
    const result = await refreshAccessToken();
    if (!result.success) {
     await runLogout(showToastOnFailure);
    }
   } catch {
    await runLogout(showToastOnFailure);
   } finally {
    refreshInProgressRef.current = false;
   }
  },
  [refreshAccessToken, runLogout]
 );

 useEffect(() => {
  syncAuth();

  const handleStorage = (event: StorageEvent) => {
   if (event.key === 'tarot-now-auth') {
    syncAuth();
   }
  };

  window.addEventListener('storage', handleStorage);
  return () => {
   window.removeEventListener('storage', handleStorage);
  };
 }, [syncAuth]);

 useEffect(() => {
  if (!isAuthenticated) return;

  void tryRefresh(false);

  const intervalId = window.setInterval(() => {
   void tryRefresh(false);
  }, REFRESH_INTERVAL_MS);

  return () => {
   window.clearInterval(intervalId);
  };
 }, [isAuthenticated, tryRefresh]);

 useEffect(() => {
  const refreshWhenActive = () => {
   if (!useAuthStore.getState().isAuthenticated) return;

   const now = Date.now();
   if (now - lastRefreshAtRef.current < FOCUS_REFRESH_THROTTLE_MS) return;

   lastRefreshAtRef.current = now;
   void tryRefresh(false);
  };

  const onVisibilityChange = () => {
   if (document.visibilityState === 'visible') {
    refreshWhenActive();
   }
  };

  window.addEventListener('focus', refreshWhenActive);
  document.addEventListener('visibilitychange', onVisibilityChange);

  return () => {
   window.removeEventListener('focus', refreshWhenActive);
   document.removeEventListener('visibilitychange', onVisibilityChange);
  };
 }, [tryRefresh]);

 return null;
}
