'use client';

import { useCallback, useEffect, useRef, useState, type RefObject } from 'react';
import {
 listMessages,
 type ChatMessageDto,
 type ConversationDto,
} from '@/features/chat/shared/actions';
import { appendUniqueMessage, mergeHistoryWithRealtimeMessages } from '@/features/chat/shared/mergeMessages';
import { useRuntimePolicies } from '@/shared/hooks/useRuntimePolicies';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';
import { createCancellableLoadTask } from '@/shared/utils/queryPolicy';

interface UseChatHistoryStateOptions {
 conversationId?: string | null;
 scrollToBottomRef: RefObject<(behavior?: ScrollBehavior) => void>;
}

export function useChatHistoryState({
 conversationId,
 scrollToBottomRef,
}: UseChatHistoryStateOptions) {
 const runtimePoliciesQuery = useRuntimePolicies();
 const chatPageSize = runtimePoliciesQuery.data?.chat.history.pageSize ?? RUNTIME_POLICY_FALLBACKS.chat.history.pageSize;
 const [messages, setMessages] = useState<ChatMessageDto[]>([]);
 const [conversation, setConversation] = useState<ConversationDto | null>(null);
 const [nextCursor, setNextCursor] = useState<string | null>(null);
 const [loading, setLoading] = useState(false);
 const [loadingMore, setLoadingMore] = useState(false);
 const [initializing, setInitializing] = useState(true);

 const hasLoadedInitialRef = useRef(false);
 const lastInitialLoadTimeRef = useRef(0);
 const loadInitialRef = useRef<(silent?: boolean) => Promise<void>>(async () => {});
 const initialLoadTaskRef = useRef(createCancellableLoadTask());
 const loadMoreTaskRef = useRef(createCancellableLoadTask());
 const initialScrollTimeoutRef = useRef<number | null>(null);

 const loadInitial = useCallback(async (silent = false) => {
  if (!conversationId) return;
  const requestId = initialLoadTaskRef.current.createToken();
  if (!silent) setLoading(true);
  try {
   const history = await listMessages(conversationId, { limit: chatPageSize });
   if (!initialLoadTaskRef.current.isCurrentToken(requestId)) {
    return;
   }

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
     if (initialScrollTimeoutRef.current !== null) {
      window.clearTimeout(initialScrollTimeoutRef.current);
     }
     initialScrollTimeoutRef.current = window.setTimeout(() => {
      scrollToBottomRef.current?.('auto');
      initialScrollTimeoutRef.current = null;
     }, 10);
    }
    return;
   }

   if (!silent) {
    setMessages([]);
    setConversation(null);
    setNextCursor(null);
   }
  } catch {
   if (!initialLoadTaskRef.current.isCurrentToken(requestId)) {
    return;
   }
   if (!silent) {
    setMessages([]);
    setConversation(null);
    setNextCursor(null);
   }
  } finally {
   if (!initialLoadTaskRef.current.isCurrentToken(requestId)) {
    return;
   }
   if (!silent) {
    setLoading(false);
    setInitializing(false);
    lastInitialLoadTimeRef.current = Date.now();
   }
  }
 }, [chatPageSize, conversationId, scrollToBottomRef]);

 useEffect(() => {
  loadInitialRef.current = loadInitial;
 }, [loadInitial]);

 const loadingMoreRef = useRef(false);

 const loadMore = useCallback(async () => {
  if (!conversationId || !nextCursor || loadingMoreRef.current) return;

  const requestId = loadMoreTaskRef.current.createToken();
  loadingMoreRef.current = true;
  setLoadingMore(true);

  try {
   const result = await listMessages(conversationId, {
    cursor: nextCursor,
    limit: chatPageSize,
   });
   if (!loadMoreTaskRef.current.isCurrentToken(requestId)) {
    return;
   }

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
  } finally {
   if (!loadMoreTaskRef.current.isCurrentToken(requestId)) {
    return;
   }
   setLoadingMore(false);
   loadingMoreRef.current = false;
  }
 }, [chatPageSize, conversationId, nextCursor]);

 const resetForConversation = useCallback((cachedConversation: ConversationDto | null) => {
  initialLoadTaskRef.current.cancel();
  loadMoreTaskRef.current.cancel();
  loadingMoreRef.current = false;
  if (initialScrollTimeoutRef.current !== null) {
   window.clearTimeout(initialScrollTimeoutRef.current);
   initialScrollTimeoutRef.current = null;
  }
  setMessages([]);
  setConversation(cachedConversation);
  setNextCursor(null);
  hasLoadedInitialRef.current = false;
  setInitializing(true);
 }, []);

 useEffect(() => {
  const initialLoadTask = initialLoadTaskRef.current;
  const loadMoreTask = loadMoreTaskRef.current;
  return () => {
   initialLoadTask.cancel();
   loadMoreTask.cancel();
   if (initialScrollTimeoutRef.current !== null) {
    window.clearTimeout(initialScrollTimeoutRef.current);
    initialScrollTimeoutRef.current = null;
   }
  };
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
