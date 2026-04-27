'use client';

import { useCallback, useEffect, useRef, useState, type Dispatch, type RefObject, type SetStateAction } from 'react';
import type { HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import {
 sendConversationMessage,
 type ChatMessageDto,
 type ListConversationsResult,
 type MediaPayloadDto,
} from '@/features/chat/application/actions';
import { appendUniqueMessage } from '@/features/chat/domain/mergeMessages';
import { logger } from '@/shared/application/gateways/logger';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';

interface UseChatSendActionsOptions {
 conversationId?: string | null;
 connected: boolean;
 connectionRef: RefObject<HubConnection | null>;
 inputRef: RefObject<HTMLInputElement | null>;
 setMessages: Dispatch<SetStateAction<ChatMessageDto[]>>;
}

export function useChatSendActions({
 conversationId,
 connected,
 connectionRef,
 inputRef,
 setMessages,
}: UseChatSendActionsOptions) {
 const queryClient = useQueryClient();
 const [newMessage, setNewMessage] = useState('');
 const [sending, setSending] = useState(false);
 const markReadRef = useRef<() => Promise<void>>(async () => {});

 const syncInboxUnreadAfterRead = useCallback(() => {
  if (!conversationId) return;

  queryClient.setQueriesData<ListConversationsResult>(
   { queryKey: userStateQueryKeys.chat.inboxRoot() },
   (previous) => {
    if (!previous?.conversations?.length || !previous.currentUserId) return previous;

    let changed = false;
    const conversations = previous.conversations.map((conversation) => {
      if (conversation.id !== conversationId) return conversation;

      const isUserRole = conversation.userId === previous.currentUserId;
      const unreadField = isUserRole ? 'unreadCountUser' : 'unreadCountReader';
      if (conversation[unreadField] === 0) return conversation;

      changed = true;
      return { ...conversation, [unreadField]: 0 };
    });

    return changed ? { ...previous, conversations } : previous;
   }
  );

  void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.unreadBadge() });
 }, [conversationId, queryClient]);

 const markRead = useCallback(async () => {
  if (!connectionRef.current || !conversationId) return;
  try {
   await connectionRef.current.invoke('MarkRead', conversationId);
   syncInboxUnreadAfterRead();
  } catch (error) {
   logger.warn('[Chat] markRead failed', error, { conversationId });
  }
 }, [connectionRef, conversationId, syncInboxUnreadAfterRead]);
 useEffect(() => {
  markReadRef.current = markRead;
 }, [markRead, markReadRef]);

 const sendTypedMessage = useCallback(async (content: string, type = 'text') => {
  const normalized = content.trim();
  if (!normalized || !conversationId) return false;

  if (connectionRef.current && connected) {
   try {
    await connectionRef.current.invoke('SendMessage', conversationId, normalized, type);
    return true;
   } catch (error) {
    logger.warn('[Chat] SendMessage via SignalR failed, fallback REST', error, { conversationId, type });
   }
  }

  try {
   const result = await sendConversationMessage(conversationId, { type, content: normalized });
   if (result.success && result.data) {
    setMessages((prev) => appendUniqueMessage(prev, result.data as ChatMessageDto));
    return true;
   }
   logger.warn('[Chat] REST SendMessage failed', result.error, { conversationId });
  } catch (error) {
   logger.warn('[Chat] REST SendMessage error', error, { conversationId });
  }
  return false;
 }, [connected, connectionRef, conversationId, setMessages]);

 const sendMediaMessage = useCallback(async (type: 'image' | 'voice', payload: MediaPayloadDto) => {
  if (!conversationId) return false;

  if (connectionRef.current && connected) {
   try {
    await connectionRef.current.invoke('SendMessage', conversationId, JSON.stringify(payload), type);
    return true;
   } catch (error) {
    logger.warn('[Chat] SendMediaMessage via SignalR failed, fallback REST', error, { conversationId, type });
   }
  }

  try {
   const result = await sendConversationMessage(conversationId, {
    type,
    content: JSON.stringify(payload),
    mediaPayload: payload,
   });
   if (result.success && result.data) {
    setMessages((prev) => appendUniqueMessage(prev, result.data as ChatMessageDto));
    return true;
   }
   logger.warn('[Chat] REST SendMediaMessage failed', result.error, { conversationId });
  } catch (error) {
   logger.warn('[Chat] REST SendMediaMessage error', error, { conversationId });
  }
  return false;
 }, [connected, connectionRef, conversationId, setMessages]);

 const handleSendTextMessage = useCallback(async () => {
  if (!newMessage.trim()) return false;
  setSending(true);
  const success = await sendTypedMessage(newMessage, 'text');
  setSending(false);
  if (!success) return false;
  setNewMessage('');
  inputRef.current?.focus();
  if (connectionRef.current && connected && conversationId) {
   void connectionRef.current.invoke('TypingStopped', conversationId).catch(() => undefined);
  }
  return true;
 }, [connected, connectionRef, conversationId, inputRef, newMessage, sendTypedMessage]);

 const notifyTyping = useCallback((typing: boolean) => {
  if (!connectionRef.current || !conversationId || !connected) return;
  const event = typing ? 'TypingStarted' : 'TypingStopped';
  void connectionRef.current.invoke(event, conversationId).catch(() => undefined);
 }, [connected, connectionRef, conversationId]);

 return {
  newMessage,
  setNewMessage,
  sending,
  sendTypedMessage,
  sendMediaMessage,
  handleSendTextMessage,
  notifyTyping,
  markRead,
  markReadRef,
 };
}
