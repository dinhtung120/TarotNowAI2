import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import type { HubConnection } from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import { registerPresenceConnectionHandlers } from '@/shared/application/hooks/usePresenceConnection.registration';
import { invalidateUserStateQueries } from '@/shared/application/gateways/invalidateUserStateQueries';
import { performClientLogoutCleanup } from '@/shared/application/gateways/clientLogoutCleanup';
import { useWalletStore } from '@/store/walletStore';
import { useAuthStore } from '@/store/authStore';

vi.mock('@/shared/application/gateways/logger', () => ({
 logger: {
  info: vi.fn(),
  warn: vi.fn(),
  error: vi.fn(),
 },
}));

vi.mock('@/shared/application/gateways/invalidateUserStateQueries', () => ({
 invalidateUserStateQueries: vi.fn().mockResolvedValue(undefined),
}));

vi.mock('@/shared/application/gateways/clientLogoutCleanup', () => ({
 performClientLogoutCleanup: vi.fn(),
}));

vi.mock('@/shared/application/gateways/gachaRealtimeDedup', () => ({
 shouldSkipRealtimeGachaInvalidation: vi.fn(() => false),
}));

vi.mock('@/shared/application/gateways/inventoryRealtimeDedup', () => ({
 shouldSkipRealtimeInventoryInvalidation: vi.fn(() => false),
}));

vi.mock('@/store/walletStore', () => ({
 useWalletStore: {
  getState: vi.fn(),
 },
}));

vi.mock('@/store/authStore', () => ({
 useAuthStore: {
  getState: vi.fn(),
 },
}));

vi.mock('@/i18n/routing', () => ({
 routing: {
  locales: ['vi', 'en', 'zh'],
  defaultLocale: 'vi',
 },
}));

interface FakeHub extends Partial<HubConnection> {
 emit: (eventName: string, ...args: unknown[]) => void;
 emitReconnected: (connectionId?: string) => void;
}

function createFakeHubConnection(): FakeHub {
 const handlers = new Map<string, Array<(...args: unknown[]) => void>>();
 let reconnectedHandler: ((connectionId?: string) => void) | null = null;

 return {
  state: 'Connected',
  invoke: vi.fn().mockResolvedValue(undefined),
  on: (eventName: string, callback: (...args: unknown[]) => void) => {
   const existing = handlers.get(eventName) ?? [];
   existing.push(callback);
   handlers.set(eventName, existing);
  },
  onclose: vi.fn(),
  onreconnected: (callback: (connectionId?: string) => void) => {
   reconnectedHandler = callback;
  },
  onreconnecting: vi.fn(),
  emit: (eventName: string, ...args: unknown[]) => {
   const eventHandlers = handlers.get(eventName) ?? [];
   for (const handler of eventHandlers) {
    handler(...args);
   }
  },
  emitReconnected: (connectionId?: string) => {
   reconnectedHandler?.(connectionId);
  },
 };
}

describe('registerPresenceConnectionHandlers', () => {
const mockedInvalidateUserStateQueries = vi.mocked(invalidateUserStateQueries);
const mockedPerformClientLogoutCleanup = vi.mocked(performClientLogoutCleanup);
const mockedWalletGetState = vi.mocked(useWalletStore.getState);
const mockedAuthGetState = vi.mocked(useAuthStore.getState);
 const queryClient: Pick<QueryClient, 'invalidateQueries'> = {
  invalidateQueries: vi.fn().mockResolvedValue(undefined),
 };

 beforeEach(() => {
  vi.useFakeTimers();
  vi.setSystemTime(new Date('2026-04-29T12:00:00.000Z'));
  mockedInvalidateUserStateQueries.mockClear();
  mockedPerformClientLogoutCleanup.mockClear();
  mockedWalletGetState.mockReturnValue({
   balance: null,
   setBalance: vi.fn(),
   fetchBalance: vi.fn().mockResolvedValue(undefined),
  } as never);
  mockedAuthGetState.mockReturnValue({
   isAuthenticated: true,
  } as never);
  vi.stubGlobal('fetch', vi.fn().mockResolvedValue({ ok: true }));
  window.history.pushState({}, '', '/vi/profile');
 });

 afterEach(() => {
  vi.unstubAllGlobals();
  vi.restoreAllMocks();
  vi.useRealTimers();
 });

 it('batches invalidations with cooldown to prevent duplicate query storms', async () => {
  const hub = createFakeHubConnection();
  registerPresenceConnectionHandlers(hub as HubConnection, queryClient);

  hub.emit('notification.new', { type: 'system_message' });
  hub.emit('notification.new', { type: 'system_message' });

  await vi.advanceTimersByTimeAsync(320);
  await Promise.resolve();
  expect(mockedInvalidateUserStateQueries).toHaveBeenCalledTimes(1);
  expect(mockedInvalidateUserStateQueries).toHaveBeenLastCalledWith(queryClient, ['notifications']);

  hub.emit('notification.new', { type: 'system_message' });
  await vi.advanceTimersByTimeAsync(320);
  await Promise.resolve();
  expect(mockedInvalidateUserStateQueries).toHaveBeenCalledTimes(1);

  await vi.advanceTimersByTimeAsync(480);
  await Promise.resolve();
  expect(mockedInvalidateUserStateQueries).toHaveBeenCalledTimes(2);
  expect(mockedInvalidateUserStateQueries).toHaveBeenLastCalledWith(queryClient, ['notifications']);
 });

 it('schedules wallet refresh fallback when realtime payload cannot be applied optimistically', async () => {
  const fetchBalance = vi.fn().mockResolvedValue(undefined);
  mockedWalletGetState.mockReturnValue({
   balance: null,
   setBalance: vi.fn(),
   fetchBalance,
  } as never);

  const hub = createFakeHubConnection();
  registerPresenceConnectionHandlers(hub as HubConnection, queryClient);

  hub.emit('wallet.balance_changed', { currency: 'gold', deltaAmount: 'invalid' });
  await vi.advanceTimersByTimeAsync(0);
  expect(fetchBalance).toHaveBeenCalledTimes(1);

  hub.emit('wallet.balance_changed', { currency: 'gold', deltaAmount: 'invalid' });
  await vi.advanceTimersByTimeAsync(1_999);
  expect(fetchBalance).toHaveBeenCalledTimes(1);

  await vi.advanceTimersByTimeAsync(1);
  expect(fetchBalance).toHaveBeenCalledTimes(2);
 });

 it('forces logout when role-changed notification is received', async () => {
  window.history.pushState({}, '', '/vi/login');
  const hub = createFakeHubConnection();
  registerPresenceConnectionHandlers(hub as HubConnection, queryClient);

  hub.emit('notification.new', { type: 'reader_request_approved' });
  await Promise.resolve();
  await Promise.resolve();

  expect(globalThis.fetch).toHaveBeenCalledWith('/api/auth/logout', {
   method: 'POST',
   credentials: 'include',
   cache: 'no-store',
  });
  expect(mockedPerformClientLogoutCleanup).toHaveBeenCalledWith(queryClient);
 });

 it('runs heartbeat and reconnect handlers through coordinator utilities', async () => {
  const fetchBalance = vi.fn().mockResolvedValue(undefined);
  mockedWalletGetState.mockReturnValue({
   balance: null,
   setBalance: vi.fn(),
   fetchBalance,
  } as never);

  const hub = createFakeHubConnection();
  const handlers = registerPresenceConnectionHandlers(hub as HubConnection, queryClient);
  const heartbeat = handlers.startHeartbeat();

  await vi.advanceTimersByTimeAsync(5 * 60 * 1000);
  expect(hub.invoke).toHaveBeenCalledWith('Heartbeat');

  hub.emit('conversation.updated', { type: 'message_created' });
  hub.emit('gamification.quest_completed');
  hub.emit('gamification.achievement_unlocked');
  hub.emit('gamification.card_level_up');
  hub.emit('reading.quota_changed');
  hub.emit('profile.changed');
  hub.emit('title.changed');
  hub.emit('gacha.result', { operationId: 'op-1' });
  hub.emit('inventory.changed', { itemCode: 'free_draw_ticket_daily', operationId: 'op-2' });
  hub.emitReconnected('connection-2');

  await vi.advanceTimersByTimeAsync(2_000);
  expect(fetchBalance).toHaveBeenCalled();

  clearInterval(heartbeat);
  handlers.dispose();
 });

 it('syncs observer groups for active reader queries and unsubscribes stale users', async () => {
  const hub = createFakeHubConnection();
  const activeDirectoryQueries = [
   {
    queryKey: ['readers', 1, 12, '', '', ''],
    state: { data: { readers: [{ userId: 'reader-1' }, { userId: 'reader-2' }] } },
    getObserversCount: () => 1,
   },
  ];
  const activeProfileQueries = [
   {
    queryKey: ['reader-profile', 'reader-3'],
    state: { data: null },
    getObserversCount: () => 1,
   },
  ];

  let directoryQueries = activeDirectoryQueries;
  let profileQueries = activeProfileQueries;

  const queryClientWithCache = {
   invalidateQueries: vi.fn().mockResolvedValue(undefined),
   getQueryCache: () => ({
    findAll: (filters?: { queryKey?: readonly unknown[] }) => {
     const key = filters?.queryKey?.[0];
     if (key === 'readers') {
      return directoryQueries;
     }

     if (key === 'reader-profile') {
      return profileQueries;
     }

     return [];
    },
    subscribe: (listener: () => void) => {
     return () => {
      listener();
     };
    },
   }),
  } as unknown as QueryClient;

  const handlers = registerPresenceConnectionHandlers(hub as HubConnection, queryClientWithCache);
  handlers.syncStatusObservers();
  await vi.advanceTimersByTimeAsync(0);
  await Promise.resolve();

  expect(hub.invoke).toHaveBeenCalledWith(
   'SubscribeUserStatusObservers',
   expect.arrayContaining(['reader-1', 'reader-2', 'reader-3']),
  );

  directoryQueries = [
   {
    queryKey: ['readers', 1, 12, '', '', ''],
    state: { data: { readers: [{ userId: 'reader-2' }] } },
    getObserversCount: () => 1,
   },
  ];
  profileQueries = [];
  handlers.syncStatusObservers();
  await vi.advanceTimersByTimeAsync(0);
  await Promise.resolve();

  expect(hub.invoke).toHaveBeenCalledWith(
   'UnsubscribeUserStatusObservers',
   expect.arrayContaining(['reader-1', 'reader-3']),
  );

  handlers.dispose();
 });
});
