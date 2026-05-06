'use client';

import { act } from 'react';
import { createRoot, type Root } from 'react-dom/client';
import { afterEach, beforeEach, describe, expect, it, vi } from 'vitest';

const scheduleWakeup = vi.fn();
const cancelWakeup = vi.fn();
const ensureRealtimeSession = vi.fn();
const getCachedConversation = vi.fn();
const listMessages = vi.fn();

const fakeConnection = {
 state: 'Disconnected',
 start: vi.fn(),
 stop: vi.fn().mockResolvedValue(undefined),
 invoke: vi.fn().mockResolvedValue(undefined),
 on: vi.fn(),
 onclose: vi.fn(),
 onreconnecting: vi.fn(),
 onreconnected: vi.fn(),
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
  Error: 1,
  Critical: 2,
  Warning: 3,
  Information: 4,
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

vi.mock('@/features/chat/shared/actions', () => ({
 listMessages: (...args: unknown[]) => listMessages(...args),
}));

vi.mock('@/shared/application/gateways/realtimeSessionGuard', () => ({
 ensureRealtimeSession: () => ensureRealtimeSession(),
}));

vi.mock('@/shared/application/gateways/logger', () => ({
 logger: {
  warn: vi.fn(),
 },
}));

vi.mock('@/shared/application/gateways/signalRUrl', () => ({
 getSignalRHubUrl: vi.fn(() => 'http://localhost/api/v1/chat'),
}));

vi.mock('@/shared/application/hooks/useReconnectWakeup', () => ({
 useReconnectWakeup: () => ({
  wakeupVersion: wakeupState.wakeupVersion,
  scheduleWakeup,
  cancelWakeup,
 }),
}));

vi.mock('@/shared/application/hooks/useRuntimePolicies', () => ({
 useRuntimePolicies: () => ({
  data: {
   realtime: {
    reconnectScheduleMs: [0, 20],
    serverTimeoutMs: 1_000,
    chat: {
     typingClearMs: 100,
     invalidateDebounceMs: 100,
     initialLoadGuardMs: 100,
     appStartGuardMs: 100,
    },
   },
  },
 }),
}));

vi.mock('@/features/chat/room/hooks/utils', () => ({
 createSignalRLogger: vi.fn(() => ({ log: vi.fn() })),
 getCachedConversation: (...args: unknown[]) => getCachedConversation(...args),
}));

let wakeupState = { wakeupVersion: 0 };

import { useChatSignalRLifecycle } from '@/features/chat/room/hooks/useChatSignalRLifecycle';

interface HarnessProps {
 conversationId: string | null;
 loadInitial: ReturnType<typeof vi.fn>;
 markRead: ReturnType<typeof vi.fn>;
 resetForConversation: ReturnType<typeof vi.fn>;
 setConnected?: ReturnType<typeof vi.fn>;
 setLoading?: ReturnType<typeof vi.fn>;
 setTypingUserId?: ReturnType<typeof vi.fn>;
 setMessages?: ReturnType<typeof vi.fn>;
 setConversation?: ReturnType<typeof vi.fn>;
 appendMessage?: ReturnType<typeof vi.fn>;
}

function Harness({
 conversationId,
 loadInitial,
 markRead,
 resetForConversation,
 setConnected = vi.fn(),
 setLoading = vi.fn(),
 setTypingUserId = vi.fn(),
 setMessages = vi.fn(),
 setConversation = vi.fn(),
 appendMessage = vi.fn(),
}: HarnessProps) {
 useChatSignalRLifecycle({
  conversationId,
  currentUserId: 'user-1',
  queryClient: {} as never,
  connectionRef: { current: null },
  typingTimeoutRef: { current: null },
  lastInitialLoadTimeRef: { current: 0 },
  loadInitialRef: { current: loadInitial },
  markReadRef: { current: markRead },
  setConnected,
  setLoading,
  setTypingUserId,
  setMessages,
  setConversation,
  resetForConversation,
  appendMessage,
 });

 return null;
}

describe('useChatSignalRLifecycle', () => {
 let container: HTMLDivElement;
 let root: Root;

 beforeEach(() => {
  wakeupState = { wakeupVersion: 0 };
  container = document.createElement('div');
  document.body.appendChild(container);
  root = createRoot(container);
  ensureRealtimeSession.mockResolvedValue(true);
  getCachedConversation.mockReturnValue(null);
  listMessages.mockResolvedValue({ success: true, data: { conversation: null } });
  fakeConnection.state = 'Disconnected';
  fakeConnection.start.mockReset();
  fakeConnection.stop.mockClear();
  fakeConnection.invoke.mockClear();
  fakeConnection.on.mockClear();
  fakeConnection.onclose.mockClear();
  fakeConnection.onreconnecting.mockClear();
  fakeConnection.onreconnected.mockClear();
  scheduleWakeup.mockClear();
  cancelWakeup.mockClear();
 });

 afterEach(() => {
  act(() => root.unmount());
  container.remove();
  vi.useRealTimers();
  vi.clearAllMocks();
 });

 it('falls back to REST load and schedules a retry when the initial realtime connect fails', async () => {
  const loadInitial = vi.fn().mockResolvedValue(undefined);
  const markRead = vi.fn().mockResolvedValue(undefined);
  const resetForConversation = vi.fn();

  fakeConnection.start.mockRejectedValue(new Error('dial failed'));

  act(() => {
   root.render(
    <Harness
     conversationId="conversation-1"
     loadInitial={loadInitial}
     markRead={markRead}
     resetForConversation={resetForConversation}
    />,
   );
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  expect(loadInitial).toHaveBeenCalledWith(false);
  expect(loadInitial).toHaveBeenCalledTimes(1);
  expect(scheduleWakeup).toHaveBeenCalled();
  expect(resetForConversation).toHaveBeenCalledWith(null);
 });

 it('cancels the pending retry and resets cached state when the conversation changes', async () => {
  const loadInitial = vi.fn().mockResolvedValue(undefined);
  const markRead = vi.fn().mockResolvedValue(undefined);
  const resetForConversation = vi.fn();

  fakeConnection.start.mockRejectedValue(new Error('dial failed'));
  getCachedConversation
   .mockReturnValueOnce({ id: 'conversation-1' })
   .mockReturnValueOnce({ id: 'conversation-2' });

  act(() => {
   root.render(
    <Harness
     conversationId="conversation-1"
     loadInitial={loadInitial}
     markRead={markRead}
     resetForConversation={resetForConversation}
    />,
   );
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  act(() => {
   root.render(
    <Harness
     conversationId="conversation-2"
     loadInitial={loadInitial}
     markRead={markRead}
     resetForConversation={resetForConversation}
    />,
   );
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  expect(cancelWakeup).toHaveBeenCalled();
  expect(resetForConversation).toHaveBeenCalledWith({ id: 'conversation-2' });
 });

 it('leaves the conversation and stops the hub on unmount after a successful connect', async () => {
  const loadInitial = vi.fn().mockResolvedValue(undefined);
  const markRead = vi.fn().mockResolvedValue(undefined);
  const resetForConversation = vi.fn();

  fakeConnection.start.mockImplementation(async () => {
   fakeConnection.state = 'Connected';
  });

  act(() => {
   root.render(
    <Harness
     conversationId="conversation-1"
     loadInitial={loadInitial}
     markRead={markRead}
     resetForConversation={resetForConversation}
    />,
   );
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  act(() => {
   root.unmount();
  });

  expect(fakeConnection.invoke).toHaveBeenCalledWith('LeaveConversation', 'conversation-1');
  expect(fakeConnection.stop).toHaveBeenCalled();
 });

 it('runs registered realtime callbacks after reconnecting', async () => {
  const loadInitial = vi.fn().mockResolvedValue(undefined);
  const markRead = vi.fn().mockResolvedValue(undefined);
  const resetForConversation = vi.fn();
  const setConnected = vi.fn();
  const setTypingUserId = vi.fn();
  const setMessages = vi.fn();
  const appendMessage = vi.fn();

  fakeConnection.start.mockImplementation(async () => {
   fakeConnection.state = 'Connected';
  });

  function ConnectedHarness() {
   useChatSignalRLifecycle({
    conversationId: 'conversation-1',
    currentUserId: 'user-1',
    queryClient: {} as never,
    connectionRef: { current: null },
    typingTimeoutRef: { current: null },
    lastInitialLoadTimeRef: { current: 0 },
    loadInitialRef: { current: loadInitial },
    markReadRef: { current: markRead },
    setConnected,
    setLoading: vi.fn(),
    setTypingUserId,
    setMessages,
    setConversation: vi.fn(),
    resetForConversation,
    appendMessage,
   });

   return null;
  }

  act(() => {
   root.render(<ConnectedHarness />);
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  const onMessageCreated = fakeConnection.on.mock.calls.find(([name]) => name === 'message.created')?.[1];
  const onTypingStarted = fakeConnection.on.mock.calls.find(([name]) => name === 'typing.started')?.[1];
  const onTypingStopped = fakeConnection.on.mock.calls.find(([name]) => name === 'typing.stopped')?.[1];
  const onReconnecting = fakeConnection.onreconnecting.mock.calls[0]?.[0];
  const onReconnected = fakeConnection.onreconnected.mock.calls[0]?.[0];
  const onClose = fakeConnection.onclose.mock.calls[0]?.[0];

  await act(async () => {
   onMessageCreated?.({ conversationId: 'conversation-1', senderId: 'user-2' });
   onTypingStarted?.({ conversationId: 'conversation-1', userId: 'user-2' });
   onTypingStopped?.({ conversationId: 'conversation-1', userId: 'user-2' });
   onReconnecting?.();
   await onReconnected?.();
   onClose?.();
  });

  expect(appendMessage).toHaveBeenCalled();
 expect(setTypingUserId).toHaveBeenCalledWith('user-2');
 expect(setMessages).not.toHaveBeenCalled();
 expect(setConnected).toHaveBeenCalled();
 });

 it('handles read receipts, visibility refreshes, conversation updates, and retry cleanup branches', async () => {
  vi.useFakeTimers();
  const loadInitial = vi.fn().mockResolvedValue(undefined);
  const markRead = vi.fn().mockResolvedValue(undefined);
  const resetForConversation = vi.fn();
  const setConnected = vi.fn();
  const setLoading = vi.fn();
  const setTypingUserId = vi.fn();
  const setConversation = vi.fn();
  const appendMessage = vi.fn();
  let latestMessages: Array<{ senderId: string; isRead: boolean }> = [
   { senderId: 'user-1', isRead: false },
   { senderId: 'user-2', isRead: false },
  ];
  const setMessages = vi.fn((updater: (value: typeof latestMessages) => typeof latestMessages) => {
   latestMessages = updater(latestMessages);
  });

  fakeConnection.start.mockImplementation(async () => {
   fakeConnection.state = 'Connected';
  });
  fakeConnection.invoke
   .mockResolvedValueOnce(undefined)
   .mockRejectedValueOnce(new Error('leave failed'));
  fakeConnection.stop.mockRejectedValueOnce(new Error('stop failed'));
  listMessages.mockResolvedValue({
   success: true,
   data: {
    conversation: { id: 'conversation-1', title: 'Updated' },
   },
  });

  Object.defineProperty(document, 'visibilityState', {
   configurable: true,
   value: 'visible',
  });

  function ConnectedHarness() {
   useChatSignalRLifecycle({
    conversationId: 'conversation-1',
    currentUserId: 'user-1',
    queryClient: {} as never,
    connectionRef: { current: null },
    typingTimeoutRef: { current: null },
    lastInitialLoadTimeRef: { current: 0 },
    loadInitialRef: { current: loadInitial },
    markReadRef: { current: markRead },
    setConnected,
    setLoading,
    setTypingUserId,
    setMessages,
    setConversation,
    resetForConversation,
    appendMessage,
   });

   return null;
  }

  act(() => {
   root.render(<ConnectedHarness />);
  });

  await act(async () => {
   await Promise.resolve();
   await Promise.resolve();
  });

  const onMessageRead = fakeConnection.on.mock.calls.find(([name]) => name === 'message.read')?.[1];
  const onTypingStarted = fakeConnection.on.mock.calls.find(([name]) => name === 'typing.started')?.[1];
  const onConversationUpdated = fakeConnection.on.mock.calls.find(([name]) => name === 'conversation.updated')?.[1];
  const onError = fakeConnection.on.mock.calls.find(([name]) => name === 'Error')?.[1];

  await act(async () => {
   onMessageRead?.({ conversationId: 'conversation-1', userId: 'user-2' });
   onTypingStarted?.({ conversationId: 'conversation-1', userId: 'user-2' });
   document.dispatchEvent(new Event('visibilitychange'));
   onConversationUpdated?.({ conversationId: 'conversation-1', type: 'status_changed' });
   onConversationUpdated?.({ conversationId: 'conversation-2', type: 'status_changed' });
   onConversationUpdated?.({ conversationId: 'conversation-1', type: 'message_created' });
   onError?.('server warning');
   await vi.advanceTimersByTimeAsync(101);
   await Promise.resolve();
  });

  expect(latestMessages[0]?.isRead).toBe(true);
  expect(latestMessages[1]?.isRead).toBe(false);
  expect(markRead).toHaveBeenCalled();
  expect(setTypingUserId).toHaveBeenCalledWith(null);
  expect(setConversation).toHaveBeenCalledWith({ id: 'conversation-1', title: 'Updated' });

  act(() => {
   root.render(
    <Harness
     conversationId={null}
     loadInitial={loadInitial}
     markRead={markRead}
     resetForConversation={resetForConversation}
     setConnected={setConnected}
     setLoading={setLoading}
     setTypingUserId={setTypingUserId}
     setMessages={setMessages}
     setConversation={setConversation}
     appendMessage={appendMessage}
    />,
   );
  });

  expect(cancelWakeup).toHaveBeenCalled();
  expect(resetForConversation).toHaveBeenCalledWith(null);
  expect(setConnected).toHaveBeenCalledWith(false);
  expect(setLoading).toHaveBeenCalledWith(false);
 });
});
