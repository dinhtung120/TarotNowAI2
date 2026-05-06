'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { useQuery } from '@tanstack/react-query';
import { useChatUnreadNotifications } from '@/features/chat/inbox/hooks/useChatUnreadNotifications';
import { fetchJsonOrThrow } from '@/shared/application/gateways/clientFetch';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { useAuthStore } from '@/features/auth/session/authStore';

vi.mock('@tanstack/react-query', () => ({
 useQuery: vi.fn(),
}));

vi.mock('@/features/auth/session/authStore', () => ({
 useAuthStore: vi.fn(),
}));

vi.mock('@/shared/application/gateways/clientFetch', () => ({
 fetchJsonOrThrow: vi.fn(),
}));

const mockedUseQuery = vi.mocked(useQuery);
const mockedUseAuthStore = vi.mocked(useAuthStore);
const mockedFetchJsonOrThrow = vi.mocked(fetchJsonOrThrow);

function Harness({
 enabled,
 onChange,
}: {
 enabled?: boolean;
 onChange: (value: ReturnType<typeof useChatUnreadNotifications>) => void;
}) {
 const value = useChatUnreadNotifications({ enabled });
 onChange(value);
 return null;
}

describe('useChatUnreadNotifications', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);
  mockedUseAuthStore.mockImplementation((selector) => selector({ isAuthenticated: true } as never));
 });

 afterEach(() => {
  act(() => root.unmount());
  container.remove();
  vi.restoreAllMocks();
  vi.clearAllMocks();
 });

 it('returns unread badge state from query result and keeps chat unread query key', () => {
  mockedUseQuery.mockReturnValue({
   data: 7,
   isLoading: false,
  } as never);

  let latest: ReturnType<typeof useChatUnreadNotifications> | null = null;
  act(() => {
   root.render(<Harness onChange={(value) => { latest = value; }} />);
  });

  expect(latest).toEqual({ unreadCount: 7, loading: false });
  expect(mockedUseQuery).toHaveBeenCalledWith(expect.objectContaining({
   queryKey: userStateQueryKeys.chat.unreadBadge(),
   enabled: true,
  }));
 });

 it('disables unread query when user is unauthenticated', () => {
  mockedUseAuthStore.mockImplementation((selector) => selector({ isAuthenticated: false } as never));
  mockedUseQuery.mockReturnValue({
   data: 0,
   isLoading: false,
  } as never);

  act(() => {
   root.render(<Harness onChange={() => undefined} />);
  });

  expect(mockedUseQuery).toHaveBeenCalledWith(expect.objectContaining({
   enabled: false,
  }));
 });

 it('does not call browser Notification API when unread count changes', () => {
  const requestPermission = vi.fn();
  const originalNotification = (window as Window & { Notification?: unknown }).Notification;

  class MockNotification {
   static permission = 'default';
   static requestPermission = requestPermission;
  }

  (window as Window & { Notification?: unknown }).Notification = MockNotification;

  mockedUseQuery
   .mockReturnValueOnce({ data: 1, isLoading: false } as never)
   .mockReturnValueOnce({ data: 3, isLoading: false } as never);

  act(() => {
   root.render(<Harness onChange={() => undefined} />);
  });
  act(() => {
   root.render(<Harness onChange={() => undefined} />);
  });

  expect(requestPermission).not.toHaveBeenCalled();

  (window as Window & { Notification?: unknown }).Notification = originalNotification;
 });

 it('uses unread-count API query function and returns zero fallback', async () => {
  mockedUseQuery.mockReturnValue({
   data: 0,
   isLoading: false,
  } as never);
  mockedFetchJsonOrThrow.mockResolvedValue({} as never);

  act(() => {
   root.render(<Harness onChange={() => undefined} />);
  });

  const unreadQuery = mockedUseQuery.mock.calls[0]?.[0];
  await expect(unreadQuery?.queryFn()).resolves.toBe(0);
  expect(mockedFetchJsonOrThrow).toHaveBeenCalledWith(
   '/api/chat/unread-count',
   expect.objectContaining({
    method: 'GET',
    credentials: 'include',
    cache: 'no-store',
   }),
   'Failed to get unread chat count.',
   8_000,
  );
 });
});
