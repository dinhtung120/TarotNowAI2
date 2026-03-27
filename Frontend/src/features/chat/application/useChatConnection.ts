'use client';

import { useCallback, useEffect, useRef, useState } from 'react';
import type { HubConnection } from '@microsoft/signalr';
import { listMessages, type ChatMessageDto } from '@/features/chat/application/actions';
import { logger } from '@/shared/infrastructure/logging/logger';
import { useTranslations } from 'next-intl';
import { useAuthStore } from '@/store/authStore';
import {
 appendUniqueMessage,
 mergeHistoryWithRealtimeMessages,
} from '@/features/chat/domain/mergeMessages';

export function useChatConnection(conversationId: string) {
 const t = useTranslations('Chat');
 const authUserId = useAuthStore((state) => state.user?.id ?? '');

 const [messages, setMessages] = useState<ChatMessageDto[]>([]);
 const [newMessage, setNewMessage] = useState('');
 const [loading, setLoading] = useState(true);
 const [sending, setSending] = useState(false);
 const [connected, setConnected] = useState(false);
 const [otherName, setOtherName] = useState('');
 const [otherAvatar, setOtherAvatar] = useState<string | null>(null);
 const [isUserRole, setIsUserRole] = useState<boolean | null>(null);
 const [currentUserId, setCurrentUserId] = useState('');

 const connectionRef = useRef<HubConnection | null>(null);
 const messagesEndRef = useRef<HTMLDivElement>(null);
 const inputRef = useRef<HTMLInputElement>(null);

 const scrollToBottom = useCallback(() => {
  messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
 }, []);

 const sendTypedMessage = useCallback(
  async (content: string, messageType: string) => {
   const normalizedContent = content.trim();
   if (!normalizedContent || !connectionRef.current || !connected) {
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

 const handleSendTextMessage = useCallback(async () => {
  if (!newMessage.trim()) return;

  setSending(true);
  const success = await sendTypedMessage(newMessage, 'text');
  if (success) {
   setNewMessage('');
   inputRef.current?.focus();
  }
  setSending(false);
 }, [newMessage, sendTypedMessage]);

 useEffect(() => {
  setCurrentUserId(authUserId);
 }, [authUserId]);

 useEffect(() => {
  if (!conversationId) return;

  let isCancelled = false;
  let hubConnection: HubConnection | null = null;

  const initConnection = async () => {
   const signalR = await import('@microsoft/signalr');
   hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('/api/v1/chat', {
     withCredentials: true,
    })
    .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
    .configureLogging(signalR.LogLevel.Warning)
    .build();

   hubConnection.on('ReceiveMessage', (message: ChatMessageDto) => {
    setMessages((prev) => appendUniqueMessage(prev, message));
    scrollToBottom();
   });

   hubConnection.on('UserJoined', (payload: { userId: string }) => {
    void payload.userId;
   });

   hubConnection.on('MessagesRead', (payload: { userId: string }) => {
    void payload.userId;
   });

   hubConnection.on('Error', (error: string) => {
    logger.error('[Chat] Hub error', error, { conversationId });
   });

   hubConnection.onreconnecting(() => {
    setConnected(false);
   });

   hubConnection.onreconnected(() => {
    setConnected(true);
    void hubConnection?.invoke('JoinConversation', conversationId);
   });

   hubConnection.onclose(() => {
    setConnected(false);
   });

   try {
    await hubConnection.start();
    if (isCancelled) return;

    setConnected(true);
    connectionRef.current = hubConnection;

    await hubConnection.invoke('JoinConversation', conversationId);
    await hubConnection.invoke('MarkRead', conversationId);
   } catch (error) {
    logger.error('[Chat] Connection failed', error, { conversationId });
   }

   const historyResult = await listMessages(conversationId);
   if (isCancelled) return;
   const history = historyResult.success ? historyResult.data : undefined;

   if (history?.conversation && authUserId) {
    const isUser = authUserId === history.conversation.userId;
    const name = isUser
     ? history.conversation.readerName || `${t('inbox.list_label_reader')} ${history.conversation.readerId.substring(0, 8)}`
     : history.conversation.userName || `${t('inbox.list_label_user')} ${history.conversation.userId.substring(0, 8)}`;

    setOtherName(name);
    setOtherAvatar(isUser ? history.conversation.readerAvatar ?? null : history.conversation.userAvatar ?? null);
    setIsUserRole(isUser);
   }

   if (history?.messages) {
    setMessages((prev) => mergeHistoryWithRealtimeMessages(prev, history.messages));
   }

   setLoading(false);
   window.setTimeout(scrollToBottom, 500);
  };

  void initConnection();

  return () => {
   isCancelled = true;

   if (hubConnection) {
    void hubConnection.invoke('LeaveConversation', conversationId).catch(() => undefined);
    void hubConnection.stop();
   }

   connectionRef.current = null;
  };
 }, [authUserId, conversationId, scrollToBottom, t]);

 return {
  messages,
  setMessages,
  newMessage,
  setNewMessage,
  loading,
  sending,
  connected,
  currentUserId,
  otherName,
  otherAvatar,
  isUserRole,
  messagesEndRef,
  inputRef,
  sendTypedMessage,
  handleSendTextMessage,
 };
}
