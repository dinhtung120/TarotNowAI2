'use client';

import { useCallback, useEffect } from 'react';
import { useQuery, useQueryClient } from '@tanstack/react-query';
import type { UserProfile } from '@/features/auth/domain/types';
import {
 loginAction,
 logoutAction,
 refreshAccessTokenAction,
} from '@/features/auth/application/actions';
import type { ActionResult } from '@/shared/domain/actionResult';
import {
 getClientSessionSnapshot,
 invalidateClientSessionSnapshot,
} from '@/shared/application/gateways/clientSessionSnapshot';
import { performClientLogoutCleanup } from '@/shared/application/gateways/clientLogoutCleanup';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { useAuthStore } from '@/store/authStore';

interface LoginPayload {
 emailOrUsername: string;
 password: string;
 rememberMe?: boolean;
}

interface AuthSessionPayload {
 user: UserProfile;
 expiresInSeconds: number;
}

const AUTH_SESSION_QUERY_KEY = userStateQueryKeys.auth.session();

async function fetchSessionUser(): Promise<UserProfile | null> {
 const snapshot = await getClientSessionSnapshot({ mode: 'full' });
 return snapshot.authenticated ? snapshot.user : null;
}

function syncAuthStoreFromSession(
 user: UserProfile | null,
 setSession: (nextUser: UserProfile | null) => void,
 clearAuth: () => void,
): void {
 if (user) {
  setSession(user);
  return;
 }

 clearAuth();
}

export function useAuth() {
 const queryClient = useQueryClient();
 const setAuth = useAuthStore((state) => state.setAuth);
 const setSession = useAuthStore((state) => state.setSession);
 const clearAuth = useAuthStore((state) => state.clearAuth);
 const user = useAuthStore((state) => state.user);
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

 const sessionQuery = useQuery({
  queryKey: AUTH_SESSION_QUERY_KEY,
  queryFn: fetchSessionUser,
  staleTime: 10_000,
  gcTime: 5 * 60 * 1000,
  retry: false,
  refetchOnWindowFocus: false,
  refetchOnMount: false,
  initialData: user,
 });

 useEffect(() => {
  syncAuthStoreFromSession(sessionQuery.data ?? null, setSession, clearAuth);
 }, [clearAuth, sessionQuery.data, setSession]);

 const login = useCallback(async (payload: LoginPayload): Promise<ActionResult<AuthSessionPayload>> => {
  const result = await loginAction(payload);
  if (!result.success || !result.data) {
   return result;
  }

  invalidateClientSessionSnapshot();
  setAuth(result.data.user, result.data.expiresInSeconds);
  queryClient.setQueryData(AUTH_SESSION_QUERY_KEY, result.data.user);
  return result;
 }, [queryClient, setAuth]);

 const refresh = useCallback(async (): Promise<ActionResult<AuthSessionPayload>> => {
  const result = await refreshAccessTokenAction();
  if (!result.success || !result.data) {
   return result;
  }

  invalidateClientSessionSnapshot();
  setSession(result.data.user, result.data.expiresInSeconds);
  queryClient.setQueryData(AUTH_SESSION_QUERY_KEY, result.data.user);
  return result;
 }, [queryClient, setSession]);

 const logout = useCallback(async (): Promise<ActionResult<undefined>> => {
  const result = await logoutAction();
  if (!result.success) {
   return result;
  }

  invalidateClientSessionSnapshot();
  performClientLogoutCleanup(queryClient);
  queryClient.setQueryData(AUTH_SESSION_QUERY_KEY, null);
  return result;
 }, [queryClient]);

 const syncSession = useCallback(async (): Promise<UserProfile | null> => {
  const snapshot = await getClientSessionSnapshot({ force: true, mode: 'full' });
  const nextUser = snapshot.authenticated ? snapshot.user : null;
  syncAuthStoreFromSession(nextUser, setSession, clearAuth);
  queryClient.setQueryData(AUTH_SESSION_QUERY_KEY, nextUser);
  return nextUser;
 }, [clearAuth, queryClient, setSession]);

 return {
  user,
  isAuthenticated,
  isSessionLoading: sessionQuery.isLoading,
  login,
  refresh,
  logout,
  syncSession,
 };
}
