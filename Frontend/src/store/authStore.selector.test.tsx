'use client';

import { act, useEffect } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it } from 'vitest';
import type { UserProfile } from '@/features/auth/domain/types';
import { registerAuthQueryBridge, useAuthStore } from '@/store/authStore';

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

function AuthSelectorHarness({ onChange }: { onChange: (value: boolean) => void }) {
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);

 useEffect(() => {
  onChange(isAuthenticated);
 }, [isAuthenticated, onChange]);

 return null;
}

describe('useAuthStore selector hook', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  registerAuthQueryBridge(null);
  useAuthStore.getState().clearAuth();
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);
 });

 afterEach(() => {
  act(() => root.unmount());
  container.remove();
  registerAuthQueryBridge(null);
  useAuthStore.getState().clearAuth();
 });

 it('subscribes React components through selector updates', () => {
  const history: boolean[] = [];

  act(() => {
   root.render(<AuthSelectorHarness onChange={(value) => {
    history.push(value);
   }} />);
  });
  expect(history).toEqual([false]);

  act(() => {
   useAuthStore.getState().setSession(SAMPLE_USER, 60);
  });
  expect(history.at(-1)).toBe(true);

  act(() => {
   useAuthStore.getState().clearAuth();
  });
  expect(history.at(-1)).toBe(false);
 });
});
