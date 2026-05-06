import { useSyncExternalStoreWithSelector } from 'use-sync-external-store/shim/with-selector';
import type { QueryClient } from '@tanstack/react-query';
import type { UserProfile } from '@/features/auth/session/types';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';

interface AuthState {
 user: UserProfile | null;
 isAuthenticated: boolean;
 accessTokenExpiresAtMs: number | null;
 setAuth: (user: UserProfile, expiresInSeconds?: number) => void;
 setSession: (user: UserProfile | null, expiresInSeconds?: number) => void;
 updateUser: (user: Partial<UserProfile>) => void;
 clearAuth: () => void;
 syncAuth: () => void;
}

type AuthStoreSelector<T> = (state: AuthState) => T;
type AuthStoreListener = () => void;
type AuthStateData = Pick<AuthState, 'user' | 'isAuthenticated' | 'accessTokenExpiresAtMs'>;
type AuthStoreActions = Pick<AuthState, 'setAuth' | 'setSession' | 'updateUser' | 'clearAuth' | 'syncAuth'>;

const AUTH_SESSION_QUERY_KEY = userStateQueryKeys.auth.session();

const authListeners = new Set<AuthStoreListener>();

let authData: AuthStateData = {
 user: null,
 isAuthenticated: false,
 accessTokenExpiresAtMs: null,
};

let authQueryClient: QueryClient | null = null;
let detachAuthQueryBridge: (() => void) | null = null;

export function registerAuthQueryBridge(queryClient?: QueryClient | null) {
 if (detachAuthQueryBridge) {
  detachAuthQueryBridge();
  detachAuthQueryBridge = null;
 }

 authQueryClient = queryClient ?? null;
 if (!authQueryClient) {
  return;
 }

 syncAuthFromQueryCache();
 detachAuthQueryBridge = authQueryClient.getQueryCache().subscribe((event) => {
  const query = event?.query;
  if (!query) {
   return;
  }

  const queryKey = query.queryKey as readonly unknown[];
  if (!isProfileMeQueryKey(queryKey)) {
   return;
  }

  syncAuthFromQueryCache();
 });
}

function notifyAuthListeners() {
 for (const listener of authListeners) {
  listener();
 }
}

function createAuthSnapshot(data: AuthStateData, actions: AuthStoreActions): AuthState {
 return { ...data, ...actions };
}

function isSameAuthData(nextData: AuthStateData): boolean {
 return authData.user === nextData.user
  && authData.isAuthenticated === nextData.isAuthenticated
  && authData.accessTokenExpiresAtMs === nextData.accessTokenExpiresAtMs;
}

function resolveExpiryTimestamp(expiresInSeconds: number | undefined): number | null {
 if (typeof expiresInSeconds !== 'number' || !Number.isFinite(expiresInSeconds) || expiresInSeconds <= 0) {
  return null;
 }

 return Date.now() + Math.floor(expiresInSeconds * 1000);
}

function isUserProfile(value: unknown): value is UserProfile {
 if (!value || typeof value !== 'object') {
  return false;
 }

 const candidate = value as Partial<UserProfile>;
 return typeof candidate.id === 'string'
  && typeof candidate.email === 'string'
  && typeof candidate.username === 'string'
  && typeof candidate.displayName === 'string'
  && typeof candidate.level === 'number'
  && typeof candidate.exp === 'number'
  && typeof candidate.role === 'string'
  && typeof candidate.status === 'string'
  && (candidate.avatarUrl === null || typeof candidate.avatarUrl === 'string');
}

const authActions: AuthStoreActions = {
 setAuth: (user, expiresInSeconds) => {
  persistAuthUserToQuery(user);
  updateAuthData({
   user,
   isAuthenticated: true,
   accessTokenExpiresAtMs: resolveExpiryTimestamp(expiresInSeconds),
  });
 },
 setSession: (user, expiresInSeconds) => {
  persistAuthUserToQuery(user);
  updateAuthData({
   user,
   isAuthenticated: Boolean(user),
   accessTokenExpiresAtMs: user ? resolveExpiryTimestamp(expiresInSeconds) : null,
  });
 },
 updateUser: (partialUser) => {
  if (!authData.user) {
   return;
  }

  const nextUser = { ...authData.user, ...partialUser };
  persistAuthUserToQuery(nextUser);
  updateAuthData({
   user: nextUser,
   isAuthenticated: true,
   accessTokenExpiresAtMs: authData.accessTokenExpiresAtMs,
  });
 },
 clearAuth: () => {
  persistAuthUserToQuery(null);
  updateAuthData({
   user: null,
   isAuthenticated: false,
   accessTokenExpiresAtMs: null,
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
  accessTokenExpiresAtMs: null,
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
 const resolvedSelector = (selector ?? identityAuthSelector) as AuthStoreSelector<unknown>;
 return useSyncExternalStoreWithSelector(
  subscribeAuth,
  getAuthSnapshot,
  getServerAuthSnapshot,
  resolvedSelector,
  Object.is,
 );
}) as UseAuthStore;

const identityAuthSelector = (state: AuthState): AuthState => state;

useAuthStore.getState = getAuthSnapshot;

function persistAuthUserToQuery(user: UserProfile | null) {
 if (!authQueryClient) {
  return;
 }

 authQueryClient.setQueryData(AUTH_SESSION_QUERY_KEY, user);
}

function isProfileMeQueryKey(queryKey: readonly unknown[]): boolean {
 if (queryKey.length !== AUTH_SESSION_QUERY_KEY.length) {
  return false;
 }

 return AUTH_SESSION_QUERY_KEY.every((segment, index) => queryKey[index] === segment);
}

function syncAuthFromQueryCache() {
 if (!authQueryClient) {
  return;
 }

 const cachedValue = authQueryClient.getQueryData<unknown>(AUTH_SESSION_QUERY_KEY);
 const cachedUser = cachedValue === null || cachedValue === undefined
  ? null
  : isUserProfile(cachedValue)
   ? cachedValue
   : authData.user;
 updateAuthData({
  user: cachedUser,
  isAuthenticated: Boolean(cachedUser),
  accessTokenExpiresAtMs: cachedUser ? authData.accessTokenExpiresAtMs : null,
 });
}
