'use client';
import { useCallback, useEffect, useRef } from 'react';
import toast from 'react-hot-toast';
import { useTranslations } from 'next-intl';
import { usePathname } from '@/i18n/routing';
import { useAuthStore } from '@/store/authStore';
import type { ActionResult } from '@/shared/domain/actionResult';
import { isTerminalAuthError } from '@/shared/domain/authErrors';
import type { UserProfile } from '@/features/auth/domain/types';
import { useOptimizedNavigation } from '@/shared/infrastructure/navigation/useOptimizedNavigation';
const REFRESH_INTERVAL_MS = 40 * 60 * 1000;
const MIN_REFRESH_THROTTLE_MS = 20 * 60 * 1000;
let globalLastRefreshAt = 0;

interface ServerSessionState {
 authenticated: boolean;
 user: UserProfile | null;
 terminalFailure: boolean;
}

async function getServerSessionState(): Promise<ServerSessionState> {
    try {
        const response = await fetch('/api/auth/session', {
            method: 'GET',
            credentials: 'include',
            cache: 'no-store',
        });
        let payload: { authenticated?: boolean; user?: UserProfile; error?: string } | null = null;
        try {
            payload = (await response.json()) as { authenticated?: boolean; user?: UserProfile; error?: string };
        } catch {
            payload = null;
        }

        if (!response.ok) {
            return {
                authenticated: false,
                user: null,
                terminalFailure: isTerminalAuthError(payload?.error) || response.status === 401 || response.status === 403,
            };
        }

        return {
            authenticated: Boolean(payload?.authenticated),
            user: payload?.user ?? null,
            terminalFailure: false,
        };
    } catch {
        return {
            authenticated: false,
            user: null,
            terminalFailure: false,
        };
    }
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
                clearAuth();
                logoutInProgressRef.current = false;
            }
            if (showToast) {
                toast.error(tApiRef.current('unauthorized'));
            }
            if (!pathnameRef.current.includes('/login')) {
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
            const sessionState = await getServerSessionState();
            if (cancelled) return;
            if (sessionState.authenticated && sessionState.user) {
                setSession(sessionState.user);
                return;
            }

            if (sessionState.terminalFailure && useAuthStore.getState().isAuthenticated) {
                clearAuth();
            }
        };

        void bootstrapSession();
        return () => {
            cancelled = true;
        };
    }, [clearAuth, setSession]);
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
