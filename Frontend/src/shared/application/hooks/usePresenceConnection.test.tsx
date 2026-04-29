'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';

const scheduleWakeup = vi.fn();
const cancelWakeup = vi.fn();
const ensureRealtimeSession = vi.fn();
const registerPresenceConnectionHandlers = vi.fn();
const useAuthStore = vi.fn();
const useRuntimePolicies = vi.fn();
const useQueryClient = vi.fn();

const fakeConnection = {
 state: 'Disconnected',
 start: vi.fn(),
 stop: vi.fn().mockResolvedValue(undefined),
 serverTimeoutInMilliseconds: 0,
};

vi.mock('@microsoft/signalr', () => ({
 HubConnectionState: {
  Connected: 'Connected',
  Reconnecting: 'Reconnecting',
  Disconnecting: 'Disconnecting',
  Disconnected: 'Disconnected',
 },
 LogLevel: {
  Debug: 0,
  Warning: 1,
 },
 HubConnectionBuilder: class {
  withUrl() {
   return this;
  }

  withAutomaticReconnect() {
   return this;
  }

  configureLogging() {
   return this;
  }

  build() {
   return fakeConnection;
  }
 },
}));

vi.mock('@tanstack/react-query', () => ({
 useQueryClient: () => useQueryClient(),
}));

vi.mock('@/store/authStore', () => ({
 useAuthStore: (selector: (state: { isAuthenticated: boolean }) => unknown) => useAuthStore(selector),
}));

vi.mock('@/shared/application/gateways/logger', () => ({
 logger: {
  info: vi.fn(),
  error: vi.fn(),
 },
}));

vi.mock('@/shared/application/gateways/signalRUrl', () => ({
 getSignalRHubUrl: vi.fn(() => 'http://localhost/api/v1/presence'),
}));

vi.mock('@/shared/application/gateways/realtimeSessionGuard', () => ({
 ensureRealtimeSession: () => ensureRealtimeSession(),
}));

vi.mock('@/shared/application/hooks/useReconnectWakeup', () => ({
 useReconnectWakeup: () => ({
  wakeupVersion: wakeupState.wakeupVersion,
  scheduleWakeup,
  cancelWakeup,
 }),
}));

vi.mock('@/shared/application/hooks/useRuntimePolicies', () => ({
 useRuntimePolicies: () => useRuntimePolicies(),
}));

vi.mock('@/shared/application/hooks/usePresenceConnection.registration', () => ({
 registerPresenceConnectionHandlers: (...args: unknown[]) => registerPresenceConnectionHandlers(...args),
}));

let wakeupState = { wakeupVersion: 0 };
let authState = { isAuthenticated: true };

import { usePresenceConnection } from '@/shared/application/hooks/usePresenceConnection';

function Harness({ enabled = true }: { enabled?: boolean }) {
 usePresenceConnection({ enabled });
 return null;
}

describe('usePresenceConnection', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  vi.useFakeTimers();
  vi.setSystemTime(new Date('2026-04-28T08:00:00.000Z'));
  wakeupState = { wakeupVersion: 0 };
  authState = { isAuthenticated: true };
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);
  useQueryClient.mockReturnValue({});
  useAuthStore.mockImplementation((selector: (state: { isAuthenticated: boolean }) => unknown) =>
   selector(authState),
  );
  useRuntimePolicies.mockReturnValue({
   data: {
    realtime: {
     reconnectScheduleMs: [0, 20],
     negotiationTimeoutMs: 10,
     presenceNegotiationCooldownMs: 50,
     serverTimeoutMs: 1_000,
     chat: {
      typingClearMs: 100,
      invalidateDebounceMs: 100,
      initialLoadGuardMs: 100,
      appStartGuardMs: 100,
     },
    },
   },
  });
  ensureRealtimeSession.mockResolvedValue(true);
  registerPresenceConnectionHandlers.mockReturnValue({
   dispose: vi.fn(),
   startHeartbeat: vi.fn(() => setInterval(() => undefined, 1_000)),
  });
  fakeConnection.state = 'Disconnected';
  fakeConnection.start.mockReset();
  fakeConnection.stop.mockClear();
  scheduleWakeup.mockClear();
  cancelWakeup.mockClear();
 });

 afterEach(() => {
  act(() => root.unmount());
  container.remove();
  vi.useRealTimers();
  vi.clearAllMocks();
 });

 it('schedules a cooldown wake-up after unauthorized negotiation errors and retries after expiry', async () => {
  fakeConnection.start
   .mockRejectedValueOnce(new Error('401 Unauthorized'))
   .mockImplementationOnce(async () => {
    fakeConnection.state = 'Connected';
   });

  act(() => {
   root.render(<Harness />);
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  expect(scheduleWakeup).toHaveBeenCalled();

  wakeupState.wakeupVersion = 1;
  vi.setSystemTime(new Date('2026-04-28T08:00:00.100Z'));
  act(() => {
   root.render(<Harness />);
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

 expect(fakeConnection.start).toHaveBeenCalledTimes(2);
 expect(cancelWakeup).toHaveBeenCalled();
 });

 it('waits for the cooldown expiry before reconnecting again', async () => {
  fakeConnection.start.mockRejectedValueOnce(new Error('401 Unauthorized'));

  act(() => {
   root.render(<Harness />);
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  expect(fakeConnection.start).toHaveBeenCalledTimes(1);

  wakeupState.wakeupVersion = 1;
  vi.setSystemTime(new Date('2026-04-28T08:00:00.020Z'));
  act(() => {
   root.render(<Harness />);
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  expect(fakeConnection.start).toHaveBeenCalledTimes(1);
  expect(scheduleWakeup).toHaveBeenCalledTimes(2);
 });

 it('times out stalled negotiations and schedules a shorter retry cooldown for generic errors', async () => {
  fakeConnection.start.mockImplementationOnce(() => {
   fakeConnection.state = 'Connected';
   return new Promise<void>(() => undefined);
  });

  act(() => {
   root.render(<Harness />);
  });

  await act(async () => {
   await vi.advanceTimersByTimeAsync(11);
   await Promise.resolve();
  });

  expect(scheduleWakeup).toHaveBeenCalled();
  expect(fakeConnection.stop).toHaveBeenCalled();
 });

 it('stops the existing connection when realtime is disabled and swallows stop errors', async () => {
  fakeConnection.start.mockImplementation(async () => {
   fakeConnection.state = 'Connected';
  });

  act(() => {
   root.render(<Harness />);
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  fakeConnection.stop.mockRejectedValueOnce(new Error('stop failed'));
  act(() => {
   root.render(<Harness enabled={false} />);
  });

  await act(async () => {
   await Promise.resolve();
  });

  expect(cancelWakeup).toHaveBeenCalled();
  expect(fakeConnection.stop).toHaveBeenCalled();
 });

 it('stops the connection if unmounted right before negotiation completes', async () => {
  let resolveStart: (() => void) | null = null;
  fakeConnection.start.mockImplementationOnce(() =>
   new Promise<void>((resolve) => {
    resolveStart = () => {
     fakeConnection.state = 'Connected';
     resolve();
    };
   }),
  );

  act(() => {
   root.render(<Harness />);
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  act(() => {
   root.unmount();
  });

  await act(async () => {
   resolveStart?.();
   await Promise.resolve();
  });

  expect(fakeConnection.stop).toHaveBeenCalled();
 });

 it('cleans up safely when the realtime session is unavailable before connection setup', async () => {
  ensureRealtimeSession.mockResolvedValue(false);

  act(() => {
   root.render(<Harness enabled={false} />);
  });

  await act(async () => {
   await Promise.resolve();
  });

  act(() => {
   root.render(<Harness />);
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  act(() => {
   root.unmount();
  });

  expect(fakeConnection.start).not.toHaveBeenCalled();
 });

 it('stops the active connection and disposes registration on unmount', async () => {
  const dispose = vi.fn();
  registerPresenceConnectionHandlers.mockReturnValue({
   dispose,
   startHeartbeat: vi.fn(() => setInterval(() => undefined, 1_000)),
  });
  fakeConnection.start.mockImplementation(async () => {
   fakeConnection.state = 'Connected';
  });

  act(() => {
   root.render(<Harness />);
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  act(() => {
   root.unmount();
  });

  expect(dispose).toHaveBeenCalled();
  expect(fakeConnection.stop).toHaveBeenCalled();
 });
});
