'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { useNotificationDropdown } from '@/features/notifications/application/useNotificationDropdown';
import { applyNotificationReadPatch } from '@/features/notifications/application/notificationCache';
import { useAuthStore } from '@/store/authStore';
import {
 useMutation,
 useQuery,
 useQueryClient,
} from '@tanstack/react-query';
import {
 fetchJsonOrThrow,
 fetchWithTimeout,
} from '@/shared/application/gateways/clientFetch';

vi.mock('@tanstack/react-query', () => ({
 useMutation: vi.fn(),
 useQuery: vi.fn(),
 useQueryClient: vi.fn(),
}));

vi.mock('@/features/notifications/application/notificationCache', () => ({
 applyNotificationReadPatch: vi.fn(),
}));

vi.mock('@/store/authStore', () => ({
 useAuthStore: vi.fn(),
}));

vi.mock('@/shared/application/gateways/clientFetch', () => ({
 fetchJsonOrThrow: vi.fn(),
 fetchWithTimeout: vi.fn(),
}));

const mockedUseQuery = vi.mocked(useQuery);
const mockedUseMutation = vi.mocked(useMutation);
const mockedUseQueryClient = vi.mocked(useQueryClient);
const mockedApplyNotificationReadPatch = vi.mocked(applyNotificationReadPatch);
const mockedUseAuthStore = vi.mocked(useAuthStore);
const mockedFetchJsonOrThrow = vi.mocked(fetchJsonOrThrow);
const mockedFetchWithTimeout = vi.mocked(fetchWithTimeout);

function Harness({ onChange }: { onChange: (value: ReturnType<typeof useNotificationDropdown>) => void }) {
 const value = useNotificationDropdown();
 onChange(value);
 return null;
}

describe('useNotificationDropdown', () => {
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
  vi.clearAllMocks();
 });

 it('rolls back by invalidating notifications when optimistic mark-as-read fails', async () => {
  const invalidateQueries = vi.fn().mockResolvedValue(undefined);
  const setQueryData = vi.fn();
  const markReadMutateAsync = vi.fn().mockRejectedValue(new Error('network'));

  mockedUseQueryClient.mockReturnValue({ invalidateQueries, setQueryData } as never);
  mockedUseQuery
   .mockReturnValueOnce({
    data: { items: [{ id: 'n1', isRead: false }], page: 1, pageSize: 10, totalCount: 1 },
    isLoading: false,
    refetch: vi.fn(),
   } as never)
   .mockReturnValueOnce({
    data: 3,
    isLoading: false,
    refetch: vi.fn(),
   } as never);
  mockedUseMutation
   .mockReturnValueOnce({ mutateAsync: markReadMutateAsync } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn() } as never);

  let latest: ReturnType<typeof useNotificationDropdown> | null = null;
  act(() => {
   root.render(<Harness onChange={(value) => {
    latest = value;
   }} />);
  });

  await act(async () => {
   await latest?.markAsRead('n1');
  });

  expect(mockedApplyNotificationReadPatch).toHaveBeenCalledWith(expect.anything(), { id: 'n1' });
  expect(invalidateQueries).toHaveBeenNthCalledWith(1, {
   queryKey: ['notifications', 'dropdown'],
   exact: true,
  });
  expect(invalidateQueries).toHaveBeenNthCalledWith(2, {
   queryKey: ['notifications', 'unread-count'],
   exact: true,
  });
 });

 it('always invalidates notification caches after mark-all, even when the request fails', async () => {
  const invalidateQueries = vi.fn().mockResolvedValue(undefined);
  const setQueryData = vi.fn();
  const markAllMutateAsync = vi.fn().mockRejectedValue(new Error('boom'));

  mockedUseQueryClient.mockReturnValue({ invalidateQueries, setQueryData } as never);
  mockedUseQuery
   .mockReturnValueOnce({
    data: { items: [{ id: 'n1', isRead: false }], page: 1, pageSize: 10, totalCount: 1 },
    isLoading: false,
    refetch: vi.fn(),
   } as never)
   .mockReturnValueOnce({
    data: undefined,
    isLoading: false,
    refetch: vi.fn(),
   } as never);
  mockedUseMutation
   .mockReturnValueOnce({ mutateAsync: vi.fn() } as never)
   .mockReturnValueOnce({ mutateAsync: markAllMutateAsync } as never);

  let latest: ReturnType<typeof useNotificationDropdown> | null = null;
  act(() => {
   root.render(<Harness onChange={(value) => {
    latest = value;
   }} />);
  });

  await act(async () => {
   await latest?.markAllAsRead().catch(() => undefined);
  });

  expect(setQueryData).toHaveBeenCalledWith(['notifications', 'dropdown'], expect.any(Function));
  expect(setQueryData).toHaveBeenCalledWith(['notifications', 'unread-count'], 0);
  expect(invalidateQueries).toHaveBeenNthCalledWith(1, {
   queryKey: ['notifications', 'dropdown'],
   exact: true,
  });
  expect(invalidateQueries).toHaveBeenNthCalledWith(2, {
   queryKey: ['notifications', 'unread-count'],
   exact: true,
  });
  expect(latest?.unreadCount).toBe(0);
 });

 it('executes dropdown fetch and patch query functions through the React Query contracts', async () => {
  const refetch = vi.fn();
  const invalidateQueries = vi.fn().mockResolvedValue(undefined);
  mockedUseQueryClient.mockReturnValue({ invalidateQueries, setQueryData: vi.fn() } as never);
  mockedUseQuery
   .mockReturnValueOnce({ data: null, isLoading: false, refetch } as never)
   .mockReturnValueOnce({ data: 0, isLoading: false, refetch } as never);
  mockedUseMutation
   .mockReturnValueOnce({ mutateAsync: vi.fn() } as never)
   .mockReturnValueOnce({ mutateAsync: vi.fn() } as never);
  mockedFetchJsonOrThrow
   .mockResolvedValueOnce({ items: [], page: 1, pageSize: 10, totalCount: 0 } as never)
   .mockResolvedValueOnce({ count: 4 } as never);
  mockedFetchWithTimeout.mockResolvedValue(new Response(null, { status: 204 }));

  act(() => {
   root.render(<Harness onChange={() => undefined} />);
  });

  const listQuery = mockedUseQuery.mock.calls[0]?.[0];
  const unreadQuery = mockedUseQuery.mock.calls[1]?.[0];
  const markReadMutation = mockedUseMutation.mock.calls[0]?.[0];
  const markAllMutation = mockedUseMutation.mock.calls[1]?.[0];

  await expect(listQuery?.queryFn()).resolves.toMatchObject({ totalCount: 0 });
  await expect(unreadQuery?.queryFn()).resolves.toBe(4);
  await expect(markReadMutation?.mutationFn('n1')).resolves.toBeUndefined();
  await expect(markAllMutation?.mutationFn()).resolves.toBeUndefined();
 });
});
