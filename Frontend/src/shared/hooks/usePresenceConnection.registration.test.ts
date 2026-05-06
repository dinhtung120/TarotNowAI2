import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';
import type { HubConnection } from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import { registerPresenceConnectionHandlers } from '@/shared/hooks/usePresenceConnection.registration';
import { invalidateUserStateQueries } from '@/shared/gateways/invalidateUserStateQueries';
import { performClientLogoutCleanup } from '@/shared/gateways/clientLogoutCleanup';
import { useWalletStore } from '@/features/wallet/shared/walletStore';
import { useAuthStore } from '@/features/auth/session/authStore';

vi.mock('@/shared/gateways/logger', () => ({
 logger: {
  info: vi.fn(),
  warn: vi.fn(),
  error: vi.fn(),
 },
}));

vi.mock('@/shared/gateways/invalidateUserStateQueries', () => ({
 invalidateUserStateQueries: vi.fn().mockResolvedValue(undefined),
}));

vi.mock('@/shared/gateways/clientLogoutCleanup', () => ({
 performClientLogoutCleanup: vi.fn(),
}));

vi.mock('@/shared/gateways/gachaRealtimeDedup', () => ({
 shouldSkipRealtimeGachaInvalidation: vi.fn(() => false),
}));

vi.mock('@/shared/gateways/inventoryRealtimeDedup', () => ({
 shouldSkipRealtimeInventoryInvalidation: vi.fn(() => false),
}));

vi.mock('@/features/wallet/shared/walletStore', () => ({
 useWalletStore: {
  getState: vi.fn(),
 },
}));

vi.mock('@/features/auth/session/authStore', () => ({
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
 (queryClient.invalidateQueries as ReturnType<typeof vi.fn>).mockClear();
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

 it('chunks observer subscriptions into 200-user batches without truncating observed users', async () => {
  const hub = createFakeHubConnection();
  const observedUsers = Array.from({ length: 401 }, (_, index) => ({ userId: `reader-${index + 1}` }));
  const queryClientWithCache = {
   invalidateQueries: vi.fn().mockResolvedValue(undefined),
   getQueryCache: () => ({
    findAll: (filters?: { queryKey?: readonly unknown[] }) => {
     const key = filters?.queryKey?.[0];
     if (key === 'readers') {
      return [
       {
        queryKey: ['readers', 1, 12, '', '', ''],
        state: { data: { readers: observedUsers } },
        getObserversCount: () => 1,
       },
      ];
     }

     if (key === 'reader-profile') {
      return [];
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

  const subscribeCalls = (hub.invoke as ReturnType<typeof vi.fn>).mock.calls
   .filter(([methodName]) => methodName === 'SubscribeUserStatusObservers')
   .map(([, userIds]) => userIds as string[]);

  expect(subscribeCalls).toHaveLength(3);
  expect(subscribeCalls[0]).toHaveLength(200);
  expect(subscribeCalls[1]).toHaveLength(200);
  expect(subscribeCalls[2]).toHaveLength(1);
  const flattenedUserIds = subscribeCalls.flat();
  expect(new Set(flattenedUserIds).size).toBe(401);
  expect(flattenedUserIds).toContain('reader-1');
  expect(flattenedUserIds).toContain('reader-401');

  handlers.dispose();
 });

 it('ignores stale user status events based on event timestamp', async () => {
  const hub = createFakeHubConnection();
  const handlers = registerPresenceConnectionHandlers(hub as HubConnection, queryClient);

  hub.emit('user.status_changed', 'reader-9', 'online', '2026-05-02T12:00:00.000Z');
  hub.emit('user.status_changed', 'reader-9', 'offline', '2026-05-02T11:59:00.000Z');
  await Promise.resolve();
  await Promise.resolve();

  const invalidateCalls = (queryClient.invalidateQueries as ReturnType<typeof vi.fn>).mock.calls;
  expect(invalidateCalls).toHaveLength(2);
  expect(invalidateCalls[0]?.[0]).toEqual({ queryKey: ['readers'] });
  expect(invalidateCalls[1]?.[0]).toEqual({ queryKey: ['reader-profile', 'reader-9'] });

  handlers.dispose();
 });
});
