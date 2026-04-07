'use client';

import { useEffect, type Dispatch, type MutableRefObject, type SetStateAction } from 'react';
import {
 HubConnectionState,
 type HubConnection,
 type LogLevel,
} from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import type { ChatMessageDto, ConversationDto } from '@/features/chat/application/actions';
import { listMessages } from '@/features/chat/application/actions';
import { logger } from '@/shared/infrastructure/logging/logger';
import { useAuthStore } from '@/store/authStore';
import { createSignalRLogger, getCachedConversation } from './utils';

interface UseChatSignalRLifecycleOptions {
 conversationId?: string | null;
 token?: string | null;
 currentUserId: string;
 queryClient: QueryClient;
 connectionRef: MutableRefObject<HubConnection | null>;
 typingTimeoutRef: MutableRefObject<ReturnType<typeof setTimeout> | null>;
 lastInitialLoadTimeRef: MutableRefObject<number>;
 loadInitialRef: MutableRefObject<(silent?: boolean) => Promise<void>>;
 markReadRef: MutableRefObject<() => Promise<void>>;
 setConnected: Dispatch<SetStateAction<boolean>>;
 setLoading: Dispatch<SetStateAction<boolean>>;
 setTypingUserId: Dispatch<SetStateAction<string | null>>;
 setMessages: Dispatch<SetStateAction<ChatMessageDto[]>>;
 setConversation: Dispatch<SetStateAction<ConversationDto | null>>;
 resetForConversation: (cachedConversation: ConversationDto | null) => void;
 appendMessage: (message: ChatMessageDto) => void;
}

async function createHubConnection() {
 const signalR = await import('@microsoft/signalr');
 const { getSignalRHubUrl } = await import('@/shared/infrastructure/realtime/signalRUrl');
 const customLogger = createSignalRLogger({
  error: signalR.LogLevel.Error as LogLevel,
  critical: signalR.LogLevel.Critical as LogLevel,
  warning: signalR.LogLevel.Warning as LogLevel,
  information: signalR.LogLevel.Information as LogLevel,
 });
 return new signalR.HubConnectionBuilder()
  .withUrl(getSignalRHubUrl('/api/v1/chat'), {
   accessTokenFactory: () => useAuthStore.getState().token ?? '',
  })
  .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
  .configureLogging(customLogger)
  .build();
}

export function useChatSignalRLifecycle(options: UseChatSignalRLifecycleOptions) {
 const {
  conversationId, token, currentUserId, queryClient, connectionRef, typingTimeoutRef,
  lastInitialLoadTimeRef, loadInitialRef, markReadRef, setConnected, setLoading,
  setTypingUserId, setMessages, setConversation, resetForConversation, appendMessage,
 } = options;

 useEffect(() => {
  if (!conversationId || !token) {
   resetForConversation(null);
   setConnected(false);
   setLoading(false);
   return;
  }

  let cancelled = false;
  let hubConnection: HubConnection | null = null;
  resetForConversation(getCachedConversation(queryClient, conversationId));
  setTypingUserId(null);

  const init = async () => {
   hubConnection = await createHubConnection();
   hubConnection.serverTimeoutInMilliseconds = 120000;
   hubConnection.on('message.created', (message: ChatMessageDto) => {
    if (message.conversationId === conversationId) appendMessage(message);
   });
   hubConnection.on('message.read', (payload: { userId: string; conversationId: string }) => {
    if (payload.conversationId !== conversationId || payload.userId === currentUserId) return;
    setMessages((prev) =>
     prev.map((item) =>
      item.senderId === currentUserId ? { ...item, isRead: true } : item
     )
    );
   });
   hubConnection.on('typing.started', (payload: { userId: string; conversationId: string }) => {
    if (payload.conversationId !== conversationId || payload.userId === currentUserId) return;
    setTypingUserId(payload.userId);
    if (typingTimeoutRef.current) clearTimeout(typingTimeoutRef.current);
    typingTimeoutRef.current = setTimeout(() => setTypingUserId(null), 2500);
   });
   hubConnection.on('typing.stopped', (payload: { userId: string; conversationId: string }) => {
    if (payload.conversationId !== conversationId || payload.userId === currentUserId) return;
    setTypingUserId(null);
   });
   hubConnection.on('conversation.updated', (payload: { conversationId: string; type?: string }) => {
    if (payload.conversationId !== conversationId || payload.type === 'message_created') return;
    if (Date.now() - lastInitialLoadTimeRef.current < 2000) return;
    void listMessages(conversationId, { limit: 1 }).then((res) => {
     if (res.success && res.data?.conversation) setConversation(res.data.conversation);
    });
   });
   hubConnection.on('Error', (error: string) => logger.warn('[Chat] Hub error', error, { conversationId }));
   hubConnection.onreconnecting(() => setConnected(false));
   hubConnection.onreconnected(() => {
    if (cancelled) return;
    setConnected(true);
    void hubConnection?.invoke('JoinConversation', conversationId);
    void markReadRef.current();
   });
   hubConnection.onclose(() => setConnected(false));

   try {
    await hubConnection.start();
    if (cancelled) return;
    connectionRef.current = hubConnection;
    setConnected(true);
    await hubConnection.invoke('JoinConversation', conversationId);
    await loadInitialRef.current(false);
    await markReadRef.current();
   } catch (error) {
    logger.warn('[Chat] Connection failed', error, { conversationId });
    await loadInitialRef.current(false);
   }
  };

  void init();
  return () => {
   cancelled = true;
   if (typingTimeoutRef.current) clearTimeout(typingTimeoutRef.current);
   if (!hubConnection) return;
   if (hubConnection.state === HubConnectionState.Connected) {
    void hubConnection.invoke('LeaveConversation', conversationId).catch(() => undefined);
   }
   if ([HubConnectionState.Connected, HubConnectionState.Reconnecting, HubConnectionState.Disconnecting].includes(hubConnection.state)) {
    void hubConnection.stop().catch(() => undefined);
   }
   connectionRef.current = null;
  };
 }, [appendMessage, connectionRef, conversationId, currentUserId, lastInitialLoadTimeRef, loadInitialRef, markReadRef, queryClient, resetForConversation, setConnected, setConversation, setLoading, setMessages, setTypingUserId, token, typingTimeoutRef]);
}
