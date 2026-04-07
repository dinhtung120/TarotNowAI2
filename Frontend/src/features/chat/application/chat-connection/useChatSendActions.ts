'use client';

import { useCallback, useEffect, useRef, useState, type Dispatch, type RefObject, type SetStateAction } from 'react';
import type { HubConnection } from '@microsoft/signalr';
import {
 sendConversationMessage,
 type ChatMessageDto,
 type MediaPayloadDto,
} from '@/features/chat/application/actions';
import { appendUniqueMessage } from '@/features/chat/domain/mergeMessages';
import { logger } from '@/shared/infrastructure/logging/logger';

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
 const [newMessage, setNewMessage] = useState('');
 const [sending, setSending] = useState(false);
 const markReadRef = useRef<() => Promise<void>>(async () => {});

 const markRead = useCallback(async () => {
  if (!connectionRef.current || !conversationId) return;
  try {
   await connectionRef.current.invoke('MarkRead', conversationId);
  } catch (error) {
   logger.warn('[Chat] markRead failed', error, { conversationId });
  }
 }, [connectionRef, conversationId]);
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
