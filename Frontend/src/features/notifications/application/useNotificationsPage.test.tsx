'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import { useNotificationsPage } from '@/features/notifications/application/useNotificationsPage';
import { applyNotificationReadPatch } from '@/features/notifications/application/notificationCache';
import {
 getNotifications,
 markNotificationAsRead,
} from '@/features/notifications/application/actions';
import {
 useMutation,
 useQuery,
 useQueryClient,
} from '@tanstack/react-query';

vi.mock('@tanstack/react-query', () => ({
 useMutation: vi.fn(),
 useQuery: vi.fn(),
 useQueryClient: vi.fn(),
}));

vi.mock('@/features/notifications/application/notificationCache', () => ({
 applyNotificationReadPatch: vi.fn(),
}));

vi.mock('@/features/notifications/application/actions', () => ({
 getNotifications: vi.fn(),
 markNotificationAsRead: vi.fn(),
}));

const mockedUseQuery = vi.mocked(useQuery);
const mockedUseMutation = vi.mocked(useMutation);
const mockedUseQueryClient = vi.mocked(useQueryClient);
const mockedApplyNotificationReadPatch = vi.mocked(applyNotificationReadPatch);
const mockedGetNotifications = vi.mocked(getNotifications);
const mockedMarkNotificationAsRead = vi.mocked(markNotificationAsRead);

function Harness({ onChange }: { onChange: (value: ReturnType<typeof useNotificationsPage>) => void }) {
 const value = useNotificationsPage();
 onChange(value);
 return null;
}

describe('useNotificationsPage', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);
  mockedUseQueryClient.mockReturnValue({} as never);
 });

 afterEach(() => {
  act(() => root.unmount());
  container.remove();
  vi.clearAllMocks();
 });

 it('patches unread-only cache entries with the current page state when mark-as-read succeeds', async () => {
  mockedUseQuery.mockReturnValue({
   data: {
    items: [{ id: 'n1', isRead: false }],
    page: 1,
    pageSize: 20,
    totalCount: 1,
   },
   isLoading: false,
   isFetching: false,
  } as never);
  mockedUseMutation.mockReturnValue({
   mutateAsync: vi.fn().mockResolvedValue({ success: true }),
  } as never);

  let latest: ReturnType<typeof useNotificationsPage> | null = null;
  act(() => {
   root.render(<Harness onChange={(value) => {
    latest = value;
   }} />);
  });

  act(() => {
   latest?.setFilterUnread(true);
  });

  await act(async () => {
   await latest?.markAsRead('n1');
  });

  expect(mockedApplyNotificationReadPatch).toHaveBeenCalledWith(expect.anything(), {
   id: 'n1',
   page: 1,
   unreadOnly: true,
  });
 });

 it('does not patch caches when the mark-as-read action fails', async () => {
  mockedUseQuery.mockReturnValue({
   data: {
    items: [{ id: 'n2', isRead: false }],
    page: 1,
    pageSize: 20,
    totalCount: 1,
   },
   isLoading: false,
   isFetching: false,
  } as never);
  mockedUseMutation.mockReturnValue({
   mutateAsync: vi.fn().mockResolvedValue({ success: false }),
  } as never);

  let latest: ReturnType<typeof useNotificationsPage> | null = null;
  act(() => {
   root.render(<Harness onChange={(value) => {
    latest = value;
   }} />);
  });

  await act(async () => {
   await latest?.markAsRead('n2');
  });

 expect(mockedApplyNotificationReadPatch).toHaveBeenCalledTimes(0);
 });

 it('executes the page query and mutation contracts directly', async () => {
  mockedUseQuery.mockReturnValue({
   data: {
    items: [{ id: 'n3', isRead: false }],
    page: 1,
    pageSize: 20,
    totalCount: 41,
   },
   isLoading: false,
   isFetching: false,
  } as never);
  mockedUseMutation.mockReturnValue({
   mutateAsync: vi.fn(),
  } as never);
  mockedGetNotifications.mockResolvedValue({
   success: true,
   data: {
    items: [{ id: 'n3', isRead: false }],
    page: 2,
    pageSize: 20,
    totalCount: 41,
   },
  } as never);
  mockedMarkNotificationAsRead.mockResolvedValue({ success: true } as never);

  let latest: ReturnType<typeof useNotificationsPage> | null = null;
  act(() => {
   root.render(<Harness onChange={(value) => {
    latest = value;
   }} />);
  });

  expect(latest?.totalPages).toBe(3);

  const queryOptions = mockedUseQuery.mock.calls[0]?.[0];
  const mutationOptions = mockedUseMutation.mock.calls[0]?.[0];

  await expect(queryOptions?.queryFn()).resolves.toMatchObject({
   totalCount: 41,
   page: 2,
  });
  await expect(mutationOptions?.mutationFn('n3')).resolves.toMatchObject({ success: true });
 });
});
