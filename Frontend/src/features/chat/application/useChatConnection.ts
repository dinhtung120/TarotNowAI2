'use client';

import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { useAuthStore } from '@/store/authStore';
import {
 listMessages,
 type ChatMessageDto,
 type ConversationDto,
 type MediaPayloadDto,
} from '@/features/chat/application/actions';
import {
 appendUniqueMessage,
 mergeHistoryWithRealtimeMessages,
} from '@/features/chat/domain/mergeMessages';
import { logger } from '@/shared/infrastructure/logging/logger';

interface UseChatConnectionOptions {
 conversationId?: string | null;
}

const PAGE_SIZE = 50;

export function useChatConnection({ conversationId }: UseChatConnectionOptions) {
 const authStore = useAuthStore();
 const authUserId = authStore.user?.id ?? '';
 const token = authStore.token;

 const [messages, setMessages] = useState<ChatMessageDto[]>([]);
 const [newMessage, setNewMessage] = useState('');
 const [loading, setLoading] = useState(false);
 const [loadingMore, setLoadingMore] = useState(false);
 const [sending, setSending] = useState(false);
 const [connected, setConnected] = useState(false);
 const [conversation, setConversation] = useState<ConversationDto | null>(null);
 const [nextCursor, setNextCursor] = useState<string | null>(null);
 const [typingUserId, setTypingUserId] = useState<string | null>(null);

 const connectionRef = useRef<HubConnection | null>(null);
 const typingTimeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);
 const messagesEndRef = useRef<HTMLDivElement>(null);
 const inputRef = useRef<HTMLInputElement>(null);
 const hasLoadedInitialRef = useRef(false);

 const currentUserId = authUserId;
 const isUserRole = useMemo(() => {
  if (!conversation || !authUserId) return null;
  return conversation.userId === authUserId;
 }, [authUserId, conversation]);
 const otherName = useMemo(() => {
  if (!conversation || isUserRole === null) return '';
  return isUserRole ? conversation.readerName ?? '' : conversation.userName ?? '';
 }, [conversation, isUserRole]);
 const otherAvatar = useMemo(() => {
  if (!conversation || isUserRole === null) return null;
  return isUserRole ? conversation.readerAvatar ?? null : conversation.userAvatar ?? null;
 }, [conversation, isUserRole]);
 const remoteTyping = typingUserId !== null && typingUserId !== currentUserId;
 const hasMore = Boolean(nextCursor);

 const scrollToBottom = useCallback((behavior: ScrollBehavior = 'smooth') => {
  messagesEndRef.current?.scrollIntoView({ behavior });
 }, []);

 const markRead = useCallback(async () => {
  if (!connectionRef.current || !conversationId) return;
  try {
   await connectionRef.current.invoke('MarkRead', conversationId);
  } catch (error) {
   logger.error('[Chat] markRead failed', error, { conversationId });
  }
 }, [conversationId]);

 const sendTypedMessage = useCallback(
  async (content: string, messageType = 'text') => {
   const normalizedContent = content.trim();
   if (!normalizedContent || !connectionRef.current || !connected || !conversationId) {
    return false;
   }

   try {
    await connectionRef.current.invoke('SendMessage', conversationId, normalizedContent, messageType);
    return true;
   } catch (error) {
    logger.error('[Chat] SendMessage failed', error, {
     conversationId,
     messageType,
    });
    return false;
   }
  },
  [connected, conversationId]
 );

 const sendMediaMessage = useCallback(
  async (type: 'image' | 'voice', payload: MediaPayloadDto) => {
   if (!connectionRef.current || !connected || !conversationId) {
    return false;
   }

   try {
    await connectionRef.current.invoke('SendMessage', conversationId, JSON.stringify(payload), type);
    return true;
   } catch (error) {
    logger.error('[Chat] SendMediaMessage failed', error, {
     conversationId,
     type,
    });
    return false;
   }
  },
  [connected, conversationId]
 );

 const handleSendTextMessage = useCallback(async () => {
  if (!newMessage.trim()) return false;
  setSending(true);
  const success = await sendTypedMessage(newMessage, 'text');
  setSending(false);
  if (success) {
   setNewMessage('');
   inputRef.current?.focus();
   void connectionRef.current?.invoke('TypingStopped', conversationId);
  }
  return success;
 }, [conversationId, newMessage, sendTypedMessage]);

 const notifyTyping = useCallback(
  (typing: boolean) => {
   if (!connectionRef.current || !conversationId || !connected) return;
   const event = typing ? 'TypingStarted' : 'TypingStopped';
   void connectionRef.current.invoke(event, conversationId).catch(() => undefined);
  },
  [connected, conversationId]
 );

 const loadInitial = useCallback(async (silent = false) => {
  if (!conversationId) return;
  if (!silent) {
   setLoading(true);
  }
  const history = await listMessages(conversationId, { limit: PAGE_SIZE });
  if (history.success && history.data) {
   const payload = history.data;
   setMessages((prev) => {
    if (silent && hasLoadedInitialRef.current) {
     return mergeHistoryWithRealtimeMessages(prev, payload.messages);
    }

    return mergeHistoryWithRealtimeMessages([], payload.messages);
   });
   setConversation(payload.conversation ?? null);
   setNextCursor(payload.nextCursor ?? null);
   if (!hasLoadedInitialRef.current) {
    hasLoadedInitialRef.current = true;
    setTimeout(() => scrollToBottom('auto'), 10);
   }
  } else {
   if (!silent) {
    setMessages([]);
    setConversation(null);
    setNextCursor(null);
   }
  }

  if (!silent) {
   setLoading(false);
  }
 }, [conversationId, scrollToBottom]);

 const loadMore = useCallback(async () => {
  if (!conversationId || !nextCursor || loadingMore) return;
  setLoadingMore(true);
  const result = await listMessages(conversationId, { cursor: nextCursor, limit: PAGE_SIZE });
  if (result.success && result.data) {
   const payload = result.data;
   const older = [...payload.messages].reverse();
   setMessages((prev) => {
    const combined = [...older, ...prev];
    const ids = new Set<string>();
    return combined.filter((item) => {
      if (ids.has(item.id)) return false;
      ids.add(item.id);
      return true;
    });
   });
   setNextCursor(payload.nextCursor ?? null);
  }
  setLoadingMore(false);
 }, [conversationId, loadingMore, nextCursor]);

 useEffect(() => {
  if (!conversationId) {
   setMessages([]);
   setConversation(null);
   setConnected(false);
   setLoading(false);
   setNextCursor(null);
   hasLoadedInitialRef.current = false;
   return;
  }

  let cancelled = false;
  let hubConnection: HubConnection | null = null;

  const initConnection = async () => {
   const signalR = await import('@microsoft/signalr');
   const { getSignalRHubUrl } = await import('@/shared/infrastructure/realtime/signalRUrl');
   hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(getSignalRHubUrl('/api/v1/chat'), {
     accessTokenFactory: () => token ?? '',
    })
    .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
    .configureLogging(signalR.LogLevel.Warning)
    .build();

   hubConnection.on('message.created', (message: ChatMessageDto) => {
    if (message.conversationId !== conversationId) return;
    setMessages((prev) => appendUniqueMessage(prev, message));
    setTimeout(() => scrollToBottom(), 0);
   });

   hubConnection.on('message.read', (payload: { userId: string; conversationId: string }) => {
    if (payload.conversationId !== conversationId) return;
    if (payload.userId === currentUserId) return;
    setMessages((prev) =>
     prev.map((item) =>
      item.senderId === currentUserId
       ? { ...item, isRead: true }
       : item
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
    if (payload.conversationId !== conversationId) return;
    if (payload.type === 'message_created') return;
    void loadInitial(true);
   });

   hubConnection.on('Error', (error: string) => {
    logger.error('[Chat] Hub error', error, { conversationId });
   });

   hubConnection.onreconnecting(() => setConnected(false));
   hubConnection.onreconnected(() => {
    if (cancelled) return;
    setConnected(true);
    void hubConnection?.invoke('JoinConversation', conversationId);
    void markRead();
   });
   hubConnection.onclose(() => setConnected(false));

   try {
    await hubConnection.start();
    if (cancelled) {
     if (hubConnection.state !== HubConnectionState.Disconnected) {
      await hubConnection.stop().catch(() => undefined);
     }
     return;
    }
    connectionRef.current = hubConnection;
    setConnected(true);

    await hubConnection.invoke('JoinConversation', conversationId);
    await loadInitial(false);
    await markRead();
   } catch (error) {
    logger.error('[Chat] Connection failed', error, { conversationId });
    await loadInitial(false);
   }
  };

  setMessages([]);
  setConversation(null);
  setNextCursor(null);
  setTypingUserId(null);
  hasLoadedInitialRef.current = false;
  void initConnection();

  return () => {
   cancelled = true;
   if (typingTimeoutRef.current) clearTimeout(typingTimeoutRef.current);
   if (hubConnection) {
    if (hubConnection.state === HubConnectionState.Connected) {
     void hubConnection.invoke('LeaveConversation', conversationId).catch(() => undefined);
    }
    if (hubConnection.state === HubConnectionState.Connected
      || hubConnection.state === HubConnectionState.Reconnecting
      || hubConnection.state === HubConnectionState.Disconnecting) {
     void hubConnection.stop().catch(() => undefined);
    }
   }
   connectionRef.current = null;
  };
 }, [conversationId, currentUserId, token, loadInitial, markRead, scrollToBottom]);

 return {
  messages,
  setMessages,
  newMessage,
  setNewMessage,
  loading,
  loadingMore,
  hasMore,
  loadMore,
  sending,
  connected,
  currentUserId,
  conversation,
  otherName,
  otherAvatar,
  isUserRole,
  remoteTyping,
  messagesEndRef,
  inputRef,
  sendTypedMessage,
  sendMediaMessage,
  handleSendTextMessage,
  notifyTyping,
  markRead,
 };
}
