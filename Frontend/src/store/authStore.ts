import { useSyncExternalStore } from 'react';
import type { UserProfile } from '@/features/auth/domain/types';

interface AuthState {
    user: UserProfile | null;
    token: string | null;
    isAuthenticated: boolean;
    _hasHydrated: boolean;
    setHasHydrated: (state: boolean) => void;
    setAuth: (user: UserProfile, token: string) => void;
    updateUser: (user: Partial<UserProfile>) => void;
    clearAuth: () => void;
    syncAuth: () => void;
}

type AuthStoreSelector<T> = (state: AuthState) => T;
type AuthStoreListener = () => void;
type AuthStateData = Pick<AuthState, 'user' | 'token' | 'isAuthenticated' | '_hasHydrated'>;
type AuthStoreActions = Pick<AuthState, 'setHasHydrated' | 'setAuth' | 'updateUser' | 'clearAuth' | 'syncAuth'>;

const AUTH_STORAGE_KEY = 'tarot-now-auth';

const authListeners = new Set<AuthStoreListener>();

let authData: AuthStateData = {
    user: null,
    token: null,
    isAuthenticated: false,
    _hasHydrated: false,
};

function notifyAuthListeners() {
    for (const listener of authListeners) listener();
}

function createAuthSnapshot(data: AuthStateData, actions: AuthStoreActions): AuthState {
    return { ...data, ...actions };
}

function persistAuth(data: AuthStateData) {
    if (typeof window === 'undefined') return;
    window.localStorage.setItem(
        AUTH_STORAGE_KEY,
        JSON.stringify({
            user: data.user,
            token: data.token,
        })
    );
}

function isSameAuthData(nextData: AuthStateData) {
    return (
        authData.user === nextData.user &&
        authData.token === nextData.token &&
        authData.isAuthenticated === nextData.isAuthenticated &&
        authData._hasHydrated === nextData._hasHydrated
    );
}

const authActions: AuthStoreActions = {
    setHasHydrated: (state) => {
        if (authData._hasHydrated === state) return;
        updateAuthData({ ...authData, _hasHydrated: state });
    },
    setAuth: (user, token) => {
        updateAuthData(
            {
                ...authData,
                user,
                token,
                isAuthenticated: true,
            },
            { persist: true }
        );
    },
    updateUser: (partialUser) => {
        if (!authData.user) return;
        updateAuthData(
            {
                ...authData,
                user: { ...authData.user, ...partialUser },
            },
            { persist: true }
        );
    },
    clearAuth: () => {
        updateAuthData(
            {
                ...authData,
                user: null,
                token: null,
                isAuthenticated: false,
            },
            { persist: true }
        );
    },
    syncAuth: () => {
        const shouldBeAuthenticated = Boolean(authData.user && authData.token);
        if (authData.isAuthenticated === shouldBeAuthenticated) return;
        updateAuthData({
            ...authData,
            isAuthenticated: shouldBeAuthenticated,
        });
    },
};

let authSnapshot = createAuthSnapshot(authData, authActions);

function updateAuthData(
    nextData: AuthStateData,
    options: {
        notify?: boolean;
        persist?: boolean;
    } = {}
) {
    if (isSameAuthData(nextData)) return;
    authData = nextData;
    authSnapshot = createAuthSnapshot(authData, authActions);
    if (options.persist) persistAuth(authData);
    if (options.notify ?? true) notifyAuthListeners();
}

function hydrateAuthFromStorage() {
    if (typeof window === 'undefined') return;

    try {
        const raw = window.localStorage.getItem(AUTH_STORAGE_KEY);
        if (!raw) {
            updateAuthData({ ...authData, _hasHydrated: true }, { notify: false });
            return;
        }

        const parsed = JSON.parse(raw) as { user?: UserProfile | null; token?: string | null };
        const user = parsed.user ?? null;
        const token = parsed.token ?? null;

        updateAuthData(
            {
                user,
                token,
                isAuthenticated: Boolean(user && token),
                _hasHydrated: true,
            },
            { notify: false }
        );
    } catch {
        updateAuthData(
            {
                user: null,
                token: null,
                isAuthenticated: false,
                _hasHydrated: true,
            },
            { notify: false }
        );
    }
}

function getAuthSnapshot() {
    return authSnapshot;
}

const serverAuthSnapshot = createAuthSnapshot(
    {
        user: null,
        token: null,
        isAuthenticated: false,
        _hasHydrated: false,
    },
    authActions
);

function getServerAuthSnapshot() {
    return serverAuthSnapshot;
}

function subscribeAuth(listener: AuthStoreListener) {
    authListeners.add(listener);
    return () => authListeners.delete(listener);
}

if (typeof window !== 'undefined') {
    hydrateAuthFromStorage();
}

type UseAuthStore = {
    (): AuthState;
    <T>(selector: AuthStoreSelector<T>): T;
    getState: () => AuthState;
};

export const useAuthStore = ((selector?: AuthStoreSelector<unknown>) => {
    const snapshot = useSyncExternalStore(subscribeAuth, getAuthSnapshot, getServerAuthSnapshot);
    return selector ? selector(snapshot) : snapshot;
}) as UseAuthStore;

useAuthStore.getState = getAuthSnapshot;
