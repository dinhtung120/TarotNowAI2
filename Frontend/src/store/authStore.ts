import { useSyncExternalStore } from 'react';
import type { UserProfile } from '@/features/auth/domain/types';

interface AuthState {
 user: UserProfile | null;
 isAuthenticated: boolean;
 setAuth: (user: UserProfile) => void;
 setSession: (user: UserProfile | null) => void;
 updateUser: (user: Partial<UserProfile>) => void;
 clearAuth: () => void;
 syncAuth: () => void;
}

type AuthStoreSelector<T> = (state: AuthState) => T;
type AuthStoreListener = () => void;
type AuthStateData = Pick<AuthState, 'user' | 'isAuthenticated'>;
type AuthStoreActions = Pick<AuthState, 'setAuth' | 'setSession' | 'updateUser' | 'clearAuth' | 'syncAuth'>;

const authListeners = new Set<AuthStoreListener>();

let authData: AuthStateData = {
 user: null,
 isAuthenticated: false,
};

function notifyAuthListeners() {
 for (const listener of authListeners) {
  listener();
 }
}

function createAuthSnapshot(data: AuthStateData, actions: AuthStoreActions): AuthState {
 return { ...data, ...actions };
}

function isSameAuthData(nextData: AuthStateData): boolean {
 return authData.user === nextData.user && authData.isAuthenticated === nextData.isAuthenticated;
}

const authActions: AuthStoreActions = {
 setAuth: (user) => {
  updateAuthData({
   user,
   isAuthenticated: true,
  });
 },
 setSession: (user) => {
  updateAuthData({
   user,
   isAuthenticated: Boolean(user),
  });
 },
 updateUser: (partialUser) => {
  if (!authData.user) {
   return;
  }

  updateAuthData({
   user: { ...authData.user, ...partialUser },
   isAuthenticated: true,
  });
 },
 clearAuth: () => {
  updateAuthData({
   user: null,
   isAuthenticated: false,
  });
 },
 syncAuth: () => {
  const shouldBeAuthenticated = Boolean(authData.user);
  if (authData.isAuthenticated === shouldBeAuthenticated) {
   return;
  }

  updateAuthData({
   ...authData,
   isAuthenticated: shouldBeAuthenticated,
  });
 },
};

let authSnapshot = createAuthSnapshot(authData, authActions);

function updateAuthData(nextData: AuthStateData) {
 if (isSameAuthData(nextData)) {
  return;
 }

 authData = nextData;
 authSnapshot = createAuthSnapshot(authData, authActions);
 notifyAuthListeners();
}

function getAuthSnapshot() {
 return authSnapshot;
}

const serverAuthSnapshot = createAuthSnapshot(
 {
  user: null,
  isAuthenticated: false,
 },
 authActions,
);

function getServerAuthSnapshot() {
 return serverAuthSnapshot;
}

function subscribeAuth(listener: AuthStoreListener) {
 authListeners.add(listener);
 return () => authListeners.delete(listener);
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
