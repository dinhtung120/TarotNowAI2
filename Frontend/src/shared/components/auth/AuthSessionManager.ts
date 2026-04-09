'use client';
import { useCallback, useEffect, useRef } from 'react';
import toast from 'react-hot-toast';
import { useTranslations } from 'next-intl';
import { usePathname, useRouter } from '@/i18n/routing';
import { useAuthStore } from '@/store/authStore';
import type { ActionResult } from '@/shared/domain/actionResult';
const REFRESH_INTERVAL_MS = 40 * 60 * 1000;
const MIN_REFRESH_THROTTLE_MS = 20 * 60 * 1000;
let globalLastRefreshAt = 0;
function isTokenExpiringSoon(token: string | null, thresholdSeconds: number = 600): boolean {
    if (!token) return true;
    try {
        const payloadBase64 = token.split('.')[1];
        if (!payloadBase64) return true;
        const decodedJson = atob(payloadBase64.replace(/-/g, '+').replace(/_/g, '/'));
        const payload = JSON.parse(decodedJson);
        const exp = payload.exp;
        if (!exp) return true;
        const nowInSeconds = Math.floor(Date.now() / 1000);
        return exp - nowInSeconds < thresholdSeconds;
    } catch {
        return true;
    }
}
interface AuthSessionManagerProps {
    logout: () => Promise<unknown> | unknown;
    refreshAccessToken: () => Promise<ActionResult<{ accessToken: string }>>;
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
            const now = Date.now();
            if (now - globalLastRefreshAt < MIN_REFRESH_THROTTLE_MS) return;
            const currentToken = useAuthStore.getState().token;
            if (!isTokenExpiringSoon(currentToken, 600) && !showToastOnFailure) {
              return;
            }
            refreshInProgressRef.current = true;
            globalLastRefreshAt = Date.now();
            try {
                const result = await refreshAccessToken();
                if (!result.success) {
                    await runLogout(showToastOnFailure);
                } else if (result.data?.accessToken) {
                    const currentUser = useAuthStore.getState().user;
                    if (currentUser) {
                        useAuthStore.getState().setAuth(currentUser, result.data.accessToken);
                    }
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
