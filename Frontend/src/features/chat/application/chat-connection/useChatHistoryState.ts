'use client';

import { useCallback, useEffect, useRef, useState, type RefObject } from 'react';
import {
 listMessages,
 type ChatMessageDto,
 type ConversationDto,
} from '@/features/chat/application/actions';
import { appendUniqueMessage, mergeHistoryWithRealtimeMessages } from '@/features/chat/domain/mergeMessages';
import { CHAT_PAGE_SIZE } from './utils';

interface UseChatHistoryStateOptions {
 conversationId?: string | null;
 scrollToBottomRef: RefObject<(behavior?: ScrollBehavior) => void>;
}

export function useChatHistoryState({
 conversationId,
 scrollToBottomRef,
}: UseChatHistoryStateOptions) {
 const [messages, setMessages] = useState<ChatMessageDto[]>([]);
 const [conversation, setConversation] = useState<ConversationDto | null>(null);
 const [nextCursor, setNextCursor] = useState<string | null>(null);
 const [loading, setLoading] = useState(false);
 const [loadingMore, setLoadingMore] = useState(false);
 const [initializing, setInitializing] = useState(true);

 const hasLoadedInitialRef = useRef(false);
 const lastInitialLoadTimeRef = useRef(0);
 const loadInitialRef = useRef<(silent?: boolean) => Promise<void>>(async () => {});

 const loadInitial = useCallback(async (silent = false) => {
  if (!conversationId) return;
  if (!silent) setLoading(true);

  const history = await listMessages(conversationId, { limit: CHAT_PAGE_SIZE });
  if (history.success && history.data) {
   const payload = history.data;
   setMessages((prev) =>
    silent && hasLoadedInitialRef.current
     ? mergeHistoryWithRealtimeMessages(prev, payload.messages)
     : mergeHistoryWithRealtimeMessages([], payload.messages)
   );
   setConversation(payload.conversation ?? null);
   setNextCursor(payload.nextCursor ?? null);
   if (!hasLoadedInitialRef.current) {
    hasLoadedInitialRef.current = true;
    setTimeout(() => scrollToBottomRef.current?.('auto'), 10);
   }
  } else if (!silent) {
   setMessages([]);
   setConversation(null);
   setNextCursor(null);
  }

  if (!silent) {
   setLoading(false);
   setInitializing(false);
   lastInitialLoadTimeRef.current = Date.now();
  }
 }, [conversationId, scrollToBottomRef]);

 useEffect(() => {
  loadInitialRef.current = loadInitial;
 }, [loadInitial]);

 const loadMore = useCallback(async () => {
  if (!conversationId || !nextCursor || loadingMore) return;
  setLoadingMore(true);
  const result = await listMessages(conversationId, {
   cursor: nextCursor,
   limit: CHAT_PAGE_SIZE,
  });
  if (result.success && result.data) {
   const older = [...result.data.messages].reverse();
   setMessages((prev) => {
    const combined = [...older, ...prev];
    const ids = new Set<string>();
    return combined.filter((item) => {
     if (ids.has(item.id)) return false;
     ids.add(item.id);
     return true;
    });
   });
   setNextCursor(result.data.nextCursor ?? null);
  }
  setLoadingMore(false);
 }, [conversationId, loadingMore, nextCursor]);

 const resetForConversation = useCallback((cachedConversation: ConversationDto | null) => {
  setMessages([]);
  setConversation(cachedConversation);
  setNextCursor(null);
  hasLoadedInitialRef.current = false;
  setInitializing(true);
 }, []);

 const appendMessage = useCallback((message: ChatMessageDto) => {
  setMessages((prev) => appendUniqueMessage(prev, message));
 }, []);

 return {
  messages,
  setMessages,
  conversation,
  setConversation,
  nextCursor,
  hasMore: Boolean(nextCursor),
  loading,
  loadingMore,
  initializing,
  setLoading,
  setInitializing,
  loadInitial,
  loadInitialRef,
  loadMore,
  resetForConversation,
  appendMessage,
  hasLoadedInitialRef,
  lastInitialLoadTimeRef,
 };
}
