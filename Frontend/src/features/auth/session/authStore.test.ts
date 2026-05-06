import { afterEach, beforeEach, describe, expect, it } from 'vitest';
import { QueryClient } from '@tanstack/react-query';
import type { UserProfile } from '@/features/auth/session/types';
import { userStateQueryKeys } from '@/shared/query/userStateQueryKeys';
import { registerAuthQueryBridge, useAuthStore } from '@/features/auth/session/authStore';

const AUTH_SESSION_QUERY_KEY = userStateQueryKeys.auth.session();
const PROFILE_DETAIL_QUERY_KEY = userStateQueryKeys.profile.detail();

const SAMPLE_USER: UserProfile = {
 id: 'user-1',
 email: 'user@example.com',
 username: 'user',
 displayName: 'User',
 avatarUrl: null,
 level: 5,
 exp: 250,
 role: 'user',
 status: 'Active',
};

describe('authStore', () => {
 beforeEach(() => {
  registerAuthQueryBridge(null);
  useAuthStore.getState().clearAuth();
 });

 afterEach(() => {
  registerAuthQueryBridge(null);
  useAuthStore.getState().clearAuth();
 });

 it('sets and clears session state', () => {
  useAuthStore.getState().setSession(SAMPLE_USER, 120);

  const authenticatedState = useAuthStore.getState();
  expect(authenticatedState.user).toEqual(SAMPLE_USER);
  expect(authenticatedState.isAuthenticated).toBe(true);
  expect(authenticatedState.accessTokenExpiresAtMs).toBeTypeOf('number');

  useAuthStore.getState().clearAuth();
  const clearedState = useAuthStore.getState();
  expect(clearedState.user).toBeNull();
  expect(clearedState.isAuthenticated).toBe(false);
  expect(clearedState.accessTokenExpiresAtMs).toBeNull();
 });

 it('syncs auth state from React Query cache bridge', () => {
  const queryClient = new QueryClient();
  registerAuthQueryBridge(queryClient);

  queryClient.setQueryData(AUTH_SESSION_QUERY_KEY, SAMPLE_USER);
  expect(useAuthStore.getState().user).toEqual(SAMPLE_USER);
  expect(useAuthStore.getState().isAuthenticated).toBe(true);

  queryClient.setQueryData(AUTH_SESSION_QUERY_KEY, null);
  expect(useAuthStore.getState().user).toBeNull();
  expect(useAuthStore.getState().isAuthenticated).toBe(false);
 });

 it('persists user updates back to query cache when bridge is active', () => {
  const queryClient = new QueryClient();
  registerAuthQueryBridge(queryClient);
  useAuthStore.getState().setSession(SAMPLE_USER, 120);

  useAuthStore.getState().updateUser({ displayName: 'Updated Name' });
  const cachedUser = queryClient.getQueryData<UserProfile | null>(AUTH_SESSION_QUERY_KEY);

  expect(cachedUser?.displayName).toBe('Updated Name');
  expect(useAuthStore.getState().user?.displayName).toBe('Updated Name');
 });

 it('ignores non-auth query keys and invalid auth cache shapes', () => {
  const queryClient = new QueryClient();
  registerAuthQueryBridge(queryClient);
  useAuthStore.getState().setSession(SAMPLE_USER, 120);

  queryClient.setQueryData(PROFILE_DETAIL_QUERY_KEY, {
   profile: {
    ...SAMPLE_USER,
    dateOfBirth: '1990-01-01',
    zodiac: 'leo',
    numerology: 3,
    hasConsented: true,
   },
   error: '',
  });
  queryClient.setQueryData(AUTH_SESSION_QUERY_KEY, {
   profile: SAMPLE_USER,
   error: '',
  });

  expect(useAuthStore.getState().user).toEqual(SAMPLE_USER);
  expect(useAuthStore.getState().isAuthenticated).toBe(true);
 });

 it('supports setAuth without bridge and keeps session synced', () => {
  useAuthStore.getState().setAuth(SAMPLE_USER, 60);
  useAuthStore.getState().syncAuth();

  const state = useAuthStore.getState();
  expect(state.user).toEqual(SAMPLE_USER);
  expect(state.isAuthenticated).toBe(true);
  expect(state.accessTokenExpiresAtMs).toBeTypeOf('number');
 });
});
