'use client';
import { useCallback, useEffect, useRef } from 'react';
import toast from 'react-hot-toast';
import { useTranslations } from 'next-intl';
import { usePathname } from '@/i18n/routing';
import { useAuthStore } from '@/store/authStore';
import type { ActionResult } from '@/shared/domain/actionResult';
import { isTerminalAuthError } from '@/shared/domain/authErrors';
import type { UserProfile } from '@/features/auth/domain/types';
import { getClientSessionSnapshot, invalidateClientSessionSnapshot } from '@/shared/infrastructure/auth/clientSessionSnapshot';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';
import {
 isAuthlessPath,
 isLegalPath,
 normalizePathname,
} from '@/shared/infrastructure/navigation/normalizePathname';
const REFRESH_INTERVAL_MS = 40 * 60 * 1000;
const MIN_REFRESH_THROTTLE_MS = 20 * 60 * 1000;
let globalLastRefreshAt = 0;

interface AuthSessionManagerProps {
    logout: () => Promise<unknown> | unknown;
    refreshAccessToken: () => Promise<ActionResult<{ user: UserProfile; expiresInSeconds: number }>>;
}
export default function AuthSessionManager({
    logout,
    refreshAccessToken,
}: AuthSessionManagerProps) {
    const navigation = useOptimizedNavigation();
    const pathname = usePathname();
    const tApi = useTranslations('ApiErrors');
    const isAuthenticated = useAuthStore((s) => s.isAuthenticated);
    const clearAuth = useAuthStore((s) => s.clearAuth);
    const setSession = useAuthStore((s) => s.setSession);
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
                invalidateClientSessionSnapshot();
                clearAuth();
                logoutInProgressRef.current = false;
            }
            if (showToast) {
                toast.error(tApiRef.current('unauthorized'));
            }
            const normalizedPath = normalizePathname(pathnameRef.current);
            if (!normalizedPath.includes('/login')) {
                navigation.push('/login');
            }
        },
        [clearAuth, logout, navigation]
    );
    const tryRefresh = useCallback(
        async (showToastOnFailure = false) => {
            if (refreshInProgressRef.current || logoutInProgressRef.current) return;
            const now = Date.now();
            if (now - globalLastRefreshAt < MIN_REFRESH_THROTTLE_MS) return;
            refreshInProgressRef.current = true;
            globalLastRefreshAt = Date.now();
            try {
                const result = await refreshAccessToken();
                if (!result.success) {
                    if (isTerminalAuthError(result.error)) {
                        await runLogout(showToastOnFailure);
                    }
                } else if (result.data?.user) {
                    invalidateClientSessionSnapshot();
                    setSession(result.data.user);
                }
            } catch {
                // Lỗi mạng/tạm thời khi refresh không nên cưỡng bức logout ngay.
            } finally {
                refreshInProgressRef.current = false;
            }
        },
        [refreshAccessToken, runLogout, setSession]
    );
    useEffect(() => {
        let cancelled = false;
        const bootstrapSession = async () => {
            if (useAuthStore.getState().isAuthenticated) {
                return;
            }

            const currentPath = normalizePathname(pathnameRef.current);
            const shouldSkipBootstrap = !useAuthStore.getState().isAuthenticated
             && (isAuthlessPath(currentPath) || isLegalPath(currentPath));
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
                // Tránh focus-trigger refresh ngay sau bootstrap (request thừa lúc first paint).
                globalLastRefreshAt = Date.now();
                return;
            }

            if (sessionState.terminalFailure && useAuthStore.getState().isAuthenticated) {
                clearAuth();
            }
        };

        const bootstrapTimerId = window.setTimeout(() => {
            void bootstrapSession();
        }, 120);
        return () => {
            cancelled = true;
            window.clearTimeout(bootstrapTimerId);
        };
    }, [clearAuth, setSession]);
    useEffect(() => {
        if (!isAuthenticated) return;
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
