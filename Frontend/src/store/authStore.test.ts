import { beforeEach, describe, expect, it } from 'vitest';
import { useAuthStore } from '@/store/authStore';
import type { UserProfile } from '@/features/auth/domain/types';

const TEST_USER: UserProfile = {
 id: 'user-1',
 email: 'user@example.com',
 username: 'user',
 displayName: 'User Test',
 avatarUrl: null,
 level: 1,
 exp: 0,
 role: 'User',
 status: 'Active',
};

describe('authStore snapshot stability', () => {
 beforeEach(() => {
  const state = useAuthStore.getState();
  state.clearAuth();
 });

 it('keeps a cached snapshot reference when state does not change', () => {
  const first = useAuthStore.getState();
  const second = useAuthStore.getState();
  expect(second).toBe(first);
 });

 it('replaces snapshot reference only when state changes', () => {
  const before = useAuthStore.getState();
  before.setAuth(TEST_USER);

  const after = useAuthStore.getState();
  expect(after).not.toBe(before);
  expect(after.isAuthenticated).toBe(true);
  expect(after.user?.id).toBe('user-1');

  const afterAgain = useAuthStore.getState();
  expect(afterAgain).toBe(after);
 });
});
