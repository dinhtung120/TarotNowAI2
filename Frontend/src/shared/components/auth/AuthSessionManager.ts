'use client';
import { useCallback, useEffect, useRef } from 'react';
import toast from 'react-hot-toast';
import { useTranslations } from 'next-intl';
import { useQueryClient } from '@tanstack/react-query';
import { usePathname } from '@/i18n/routing';
import { useAuthStore } from '@/store/authStore';
import type { ActionResult } from '@/shared/domain/actionResult';
import { isTerminalAuthError } from '@/shared/domain/authErrors';
import type { UserProfile } from '@/features/auth/domain/types';
import { getClientSessionSnapshot, invalidateClientSessionSnapshot } from '@/shared/infrastructure/auth/clientSessionSnapshot';
import { performClientLogoutCleanup } from '@/shared/infrastructure/auth/clientLogoutCleanup';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';
import {
 isAuthlessPath,
 isLegalPath,
 normalizePathname,
} from '@/shared/infrastructure/navigation/normalizePathname';
import { PROTECTED_PREFIXES } from '@/shared/config/authRoutes';

const DEFAULT_REFRESH_INTERVAL_MS = 5 * 60 * 1000;
const BOOTSTRAP_REFRESH_DELAY_MS = 90_000;
const MIN_REFRESH_INTERVAL_MS = 30_000;
const PREEMPTIVE_REFRESH_WINDOW_MS = 90_000;
const MAX_REFRESH_BACKOFF_MS = 5 * 60 * 1000;

function isProtectedPath(pathname: string): boolean {
 return PROTECTED_PREFIXES.some((prefix) => pathname === prefix || pathname.startsWith(`${prefix}/`));
}

function resolveRefreshDelay(expiresInSeconds: number | undefined): number {
 if (typeof expiresInSeconds !== 'number' || !Number.isFinite(expiresInSeconds) || expiresInSeconds <= 0) {
  return DEFAULT_REFRESH_INTERVAL_MS;
 }

 const ttlMs = Math.floor(expiresInSeconds * 1000);
 return Math.max(MIN_REFRESH_INTERVAL_MS, ttlMs - PREEMPTIVE_REFRESH_WINDOW_MS);
}

interface AuthSessionManagerProps {
 logout: () => Promise<unknown> | unknown;
 refreshAccessToken: () => Promise<ActionResult<{ user: UserProfile; expiresInSeconds: number }>>;
}

export default function AuthSessionManager({
 logout,
 refreshAccessToken,
}: AuthSessionManagerProps) {
 const navigation = useOptimizedNavigation();
 const queryClient = useQueryClient();
 const pathname = usePathname();
 const tApi = useTranslations('ApiErrors');
 const isAuthenticated = useAuthStore((s) => s.isAuthenticated);
 const accessTokenExpiresAtMs = useAuthStore((s) => s.accessTokenExpiresAtMs);
 const setSession = useAuthStore((s) => s.setSession);

 const logoutInProgressRef = useRef(false);
 const refreshInProgressRef = useRef(false);
 const refreshTimerRef = useRef<ReturnType<typeof setTimeout> | null>(null);
 const consecutiveFailureCountRef = useRef(0);
 const nextRefreshAtRef = useRef(0);
 const tryRefreshRef = useRef<(showToastOnFailure?: boolean) => Promise<void>>(async () => {});

 const pathnameRef = useRef(pathname);
 const tApiRef = useRef(tApi);

 const clearRefreshTimer = useCallback(() => {
  if (refreshTimerRef.current) {
   clearTimeout(refreshTimerRef.current);
   refreshTimerRef.current = null;
  }
 }, []);

 const scheduleRefreshAt = useCallback((timestampMs: number) => {
  clearRefreshTimer();
  const safeTimestamp = Math.max(Date.now() + MIN_REFRESH_INTERVAL_MS, timestampMs);
  nextRefreshAtRef.current = safeTimestamp;
  const delayMs = Math.max(MIN_REFRESH_INTERVAL_MS, safeTimestamp - Date.now());
  refreshTimerRef.current = setTimeout(() => {
   refreshTimerRef.current = null;
   void tryRefreshRef.current(false);
  }, delayMs);
 }, [clearRefreshTimer]);

 const scheduleRefreshWithTtl = useCallback((expiresInSeconds: number | undefined) => {
  scheduleRefreshAt(Date.now() + resolveRefreshDelay(expiresInSeconds));
 }, [scheduleRefreshAt]);

 const runLogout = useCallback(
  async (showToast: boolean) => {
   if (logoutInProgressRef.current) return;
   logoutInProgressRef.current = true;
   clearRefreshTimer();

   try {
    await logout();
   } catch {
   } finally {
    performClientLogoutCleanup(queryClient);
    logoutInProgressRef.current = false;
    refreshInProgressRef.current = false;
    consecutiveFailureCountRef.current = 0;
    nextRefreshAtRef.current = 0;
   }

   if (showToast) {
    toast.error(tApiRef.current('unauthorized'));
   }

   const normalizedPath = normalizePathname(pathnameRef.current);
   if (!normalizedPath.includes('/login')) {
    navigation.push('/login');
   }
  },
  [clearRefreshTimer, logout, navigation, queryClient],
 );

 const scheduleRetryWithBackoff = useCallback(() => {
  const failures = Math.max(1, consecutiveFailureCountRef.current);
  const backoffMs = Math.min(MAX_REFRESH_BACKOFF_MS, 30_000 * (2 ** (failures - 1)));
  scheduleRefreshAt(Date.now() + backoffMs);
 }, [scheduleRefreshAt]);

 const tryRefresh = useCallback(
  async (showToastOnFailure = false) => {
   if (refreshInProgressRef.current || logoutInProgressRef.current) return;

   if (nextRefreshAtRef.current > 0 && Date.now() + 1_000 < nextRefreshAtRef.current) {
    return;
   }

   refreshInProgressRef.current = true;

   try {
    const result = await refreshAccessToken();
    if (!result.success) {
     if (isTerminalAuthError(result.error)) {
      await runLogout(showToastOnFailure);
      return;
     }

     consecutiveFailureCountRef.current += 1;
     scheduleRetryWithBackoff();
     return;
    }

    consecutiveFailureCountRef.current = 0;
    if (result.data?.user) {
     invalidateClientSessionSnapshot();
     setSession(result.data.user, result.data.expiresInSeconds);
    }

    scheduleRefreshWithTtl(result.data?.expiresInSeconds);
   } catch {
    consecutiveFailureCountRef.current += 1;
    scheduleRetryWithBackoff();
   } finally {
    refreshInProgressRef.current = false;
   }
  },
  [refreshAccessToken, runLogout, scheduleRefreshWithTtl, scheduleRetryWithBackoff, setSession],
 );

 useEffect(() => {
  tryRefreshRef.current = tryRefresh;
 }, [tryRefresh]);

 useEffect(() => {
  pathnameRef.current = pathname;
 }, [pathname]);

 useEffect(() => {
  tApiRef.current = tApi;
 }, [tApi]);

 useEffect(() => {
  let cancelled = false;

  const bootstrapSession = async () => {
   if (useAuthStore.getState().isAuthenticated) {
    return;
   }

   const currentPath = normalizePathname(pathnameRef.current);
   const shouldBootstrapOnPath = isProtectedPath(currentPath) || currentPath === '/';
   const shouldSkipBootstrap = !useAuthStore.getState().isAuthenticated
    && (!shouldBootstrapOnPath || isAuthlessPath(currentPath) || isLegalPath(currentPath));
   if (shouldSkipBootstrap) {
    return;
   }

   const sessionState = await getClientSessionSnapshot({
    maxAgeMs: 10_000,
    mode: 'full',
   });

   if (cancelled) return;
   if (sessionState.authenticated && sessionState.user) {
    setSession(sessionState.user);
    scheduleRefreshAt(Date.now() + BOOTSTRAP_REFRESH_DELAY_MS);
    return;
   }

   if (sessionState.terminalFailure && useAuthStore.getState().isAuthenticated) {
    performClientLogoutCleanup(queryClient);
   }
  };

  const bootstrapTimerId = window.setTimeout(() => {
   void bootstrapSession();
  }, 120);

  return () => {
   cancelled = true;
   window.clearTimeout(bootstrapTimerId);
  };
 }, [queryClient, scheduleRefreshAt, setSession]);

 useEffect(() => {
  if (!isAuthenticated) {
   clearRefreshTimer();
   consecutiveFailureCountRef.current = 0;
   nextRefreshAtRef.current = 0;
   return;
  }

  if (accessTokenExpiresAtMs && accessTokenExpiresAtMs > Date.now()) {
   const delay = Math.max(
    MIN_REFRESH_INTERVAL_MS,
    accessTokenExpiresAtMs - Date.now() - PREEMPTIVE_REFRESH_WINDOW_MS,
   );
   scheduleRefreshAt(Date.now() + delay);
   return;
  }

  scheduleRefreshAt(Date.now() + DEFAULT_REFRESH_INTERVAL_MS);
 }, [accessTokenExpiresAtMs, clearRefreshTimer, isAuthenticated, scheduleRefreshAt]);

 useEffect(() => {
  const refreshWhenActive = () => {
   if (!useAuthStore.getState().isAuthenticated) return;
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

 useEffect(() => () => {
  clearRefreshTimer();
 }, [clearRefreshTimer]);

 return null;
}
