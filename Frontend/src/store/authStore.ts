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

const AUTH_STORAGE_KEY = 'tarot-now-auth';

const authListeners = new Set<AuthStoreListener>();

const authData: Pick<AuthState, 'user' | 'token' | 'isAuthenticated' | '_hasHydrated'> = {
 user: null,
 token: null,
 isAuthenticated: false,
 _hasHydrated: false,
};

function notifyAuthListeners() {
 for (const listener of authListeners) listener();
}

function persistAuth() {
 if (typeof window === 'undefined') return;
 window.localStorage.setItem(
  AUTH_STORAGE_KEY,
  JSON.stringify({
   user: authData.user,
   token: authData.token,
  })
 );
}

function hydrateAuthFromStorage() {
 if (typeof window === 'undefined') return;
 try {
  const raw = window.localStorage.getItem(AUTH_STORAGE_KEY);
  if (raw) {
   const parsed = JSON.parse(raw) as { user?: UserProfile | null; token?: string | null };
   authData.user = parsed.user ?? null;
   authData.token = parsed.token ?? null;
   authData.isAuthenticated = Boolean(authData.user && authData.token);
  }
 } catch {
  authData.user = null;
  authData.token = null;
  authData.isAuthenticated = false;
 }
 authData._hasHydrated = true;
}

function getAuthState(): AuthState {
 return {
  ...authData,
  setHasHydrated: (state) => {
   if (authData._hasHydrated === state) return;
   authData._hasHydrated = state;
   notifyAuthListeners();
  },
  setAuth: (user, token) => {
   authData.user = user;
   authData.token = token;
   authData.isAuthenticated = true;
   persistAuth();
   notifyAuthListeners();
  },
  updateUser: (partialUser) => {
   if (!authData.user) return;
   authData.user = { ...authData.user, ...partialUser };
   persistAuth();
   notifyAuthListeners();
  },
  clearAuth: () => {
   authData.user = null;
   authData.token = null;
   authData.isAuthenticated = false;
   persistAuth();
   notifyAuthListeners();
  },
  syncAuth: () => {
   const shouldBeAuthenticated = Boolean(authData.user && authData.token);
   if (authData.isAuthenticated === shouldBeAuthenticated) return;
   authData.isAuthenticated = shouldBeAuthenticated;
   notifyAuthListeners();
  },
 };
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

const identityAuthSelector = (state: AuthState) => state;

export const useAuthStore = ((selector?: AuthStoreSelector<unknown>) =>
 useSyncExternalStore(
  subscribeAuth,
  () => (selector ? selector(getAuthState()) : identityAuthSelector(getAuthState())),
  () => (selector ? selector(getAuthState()) : identityAuthSelector(getAuthState()))
 )) as UseAuthStore;

useAuthStore.getState = getAuthState;
