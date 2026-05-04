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
 currentUserId: string;
 connected: boolean;
 connectionRef: RefObject<HubConnection | null>;
 inputRef: RefObject<HTMLInputElement | null>;
 setMessages: Dispatch<SetStateAction<ChatMessageDto[]>>;
}

function createClientMessageId() {
 if (typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function') {
  return `cmsg_${crypto.randomUUID()}`;
 }

 return `cmsg_${Date.now()}_${Math.random().toString(36).slice(2, 10)}`;
}

function normalizeClientMessageId(value: string | null | undefined) {
 return value?.trim() || '';
}

function resolveMediaFallbackContent(type: 'image' | 'voice', payload: MediaPayloadDto) {
 if (payload.description && payload.description.trim()) {
  return payload.description.trim();
 }

 return type === 'voice' ? '[voice]' : '[image]';
}

function buildOptimisticMessage(params: {
 conversationId: string;
 currentUserId: string;
 type: string;
 content: string;
 clientMessageId: string;
 mediaPayload?: MediaPayloadDto | null;
}): ChatMessageDto {
 return {
  id: `tmp:${params.clientMessageId}`,
  conversationId: params.conversationId,
  senderId: params.currentUserId,
  type: params.type,
  content: params.content,
  clientMessageId: params.clientMessageId,
  mediaPayload: params.mediaPayload ?? null,
  isRead: false,
  createdAt: new Date().toISOString(),
  localStatus: 'sending',
 };
}

export function useChatSendActions({
 conversationId,
 currentUserId,
 connected,
 connectionRef,
 inputRef,
 setMessages,
}: UseChatSendActionsOptions) {
 const queryClient = useQueryClient();
 const [newMessage, setNewMessage] = useState('');
 const [sending, setSending] = useState(false);
 const sendInFlightRef = useRef(false);
 const markReadRef = useRef<() => Promise<void>>(async () => {});

 const focusComposerInput = useCallback(() => {
  if (typeof window === 'undefined') return;
  const focusInput = () => {
   inputRef.current?.focus({ preventScroll: true });
  };
  focusInput();
  window.requestAnimationFrame(focusInput);
  window.setTimeout(focusInput, 0);
 }, [inputRef]);

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

 const markOptimisticMessageFailed = useCallback((clientMessageId: string) => {
  const normalized = normalizeClientMessageId(clientMessageId);
  if (!normalized) return;

  setMessages((prev) =>
   prev.map((message) =>
    normalizeClientMessageId(message.clientMessageId) === normalized
     ? { ...message, localStatus: 'failed' }
     : message
   )
  );
 }, [setMessages]);

 const sendTypedMessage = useCallback(async (content: string, type = 'text') => {
  const normalized = content.trim();
  if (!normalized || !conversationId || !currentUserId) return false;
  const clientMessageId = createClientMessageId();
  const optimisticMessage = buildOptimisticMessage({
   conversationId,
   currentUserId,
   type,
   content: normalized,
   clientMessageId,
  });
  setMessages((prev) => appendUniqueMessage(prev, optimisticMessage));

  if (connectionRef.current && connected) {
   try {
    await connectionRef.current.invoke('SendMessage', conversationId, normalized, type, clientMessageId);
    return true;
   } catch (error) {
    logger.warn('[Chat] SendMessage via SignalR failed, fallback REST', error, { conversationId, type });
   }
  }

  try {
   const result = await sendConversationMessage(conversationId, {
    type,
    content: normalized,
    clientMessageId,
   });
   if (result.success && result.data) {
    setMessages((prev) => appendUniqueMessage(prev, result.data as ChatMessageDto));
    return true;
   }
   logger.warn('[Chat] REST SendMessage failed', result.error, { conversationId });
  } catch (error) {
   logger.warn('[Chat] REST SendMessage error', error, { conversationId });
  }

  markOptimisticMessageFailed(clientMessageId);
  return false;
 }, [connected, connectionRef, conversationId, currentUserId, markOptimisticMessageFailed, setMessages]);

 const sendMediaMessage = useCallback(async (type: 'image' | 'voice', payload: MediaPayloadDto) => {
  if (!conversationId || !currentUserId) return false;
  const clientMessageId = createClientMessageId();
  const optimisticMessage = buildOptimisticMessage({
   conversationId,
   currentUserId,
   type,
   content: resolveMediaFallbackContent(type, payload),
   clientMessageId,
   mediaPayload: payload,
  });
  setMessages((prev) => appendUniqueMessage(prev, optimisticMessage));

  if (connectionRef.current && connected) {
   try {
    await connectionRef.current.invoke(
     'SendMessage',
     conversationId,
     JSON.stringify(payload),
     type,
     clientMessageId
    );
    return true;
   } catch (error) {
    logger.warn('[Chat] SendMediaMessage via SignalR failed, fallback REST', error, { conversationId, type });
   }
  }

  try {
   const result = await sendConversationMessage(conversationId, {
    type,
    content: JSON.stringify(payload),
    clientMessageId,
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

  markOptimisticMessageFailed(clientMessageId);
  return false;
 }, [connected, connectionRef, conversationId, currentUserId, markOptimisticMessageFailed, setMessages]);

 const handleSendTextMessage = useCallback(async () => {
  const messageContent = newMessage.trim();
  if (!messageContent || sendInFlightRef.current) return false;
  sendInFlightRef.current = true;
  setSending(true);
  setNewMessage('');

  try {
   const success = await sendTypedMessage(messageContent, 'text');
   if (!success) {
    setNewMessage(messageContent);
    return false;
   }
   if (connectionRef.current && connected && conversationId) {
    void connectionRef.current.invoke('TypingStopped', conversationId).catch(() => undefined);
   }
   return true;
  } catch {
   setNewMessage(messageContent);
   return false;
  } finally {
   setSending(false);
   sendInFlightRef.current = false;
   focusComposerInput();
  }
 }, [connected, connectionRef, conversationId, focusComposerInput, newMessage, sendTypedMessage]);

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
