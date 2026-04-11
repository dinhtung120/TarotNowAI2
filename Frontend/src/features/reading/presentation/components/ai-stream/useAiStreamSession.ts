'use client';

import { useCallback, useEffect, useRef, useState, type FormEvent } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { checkinQueryKeys } from '@/features/checkin/domain/checkinQueryKeys';
import type { StreamMessage } from './types';

interface UseAiStreamSessionOptions {
 sessionId: string;
 locale: string;
 errorStreamMessage: string;
 onComplete?: () => void;
}

export function useAiStreamSession({
 sessionId,
 locale,
 errorStreamMessage,
 onComplete,
}: UseAiStreamSessionOptions) {
 const queryClient = useQueryClient();
 const [messages, setMessages] = useState<StreamMessage[]>([]);
 const [isStreaming, setIsStreaming] = useState(false);
 const [error, setError] = useState<string | null>(null);
 const [isComplete, setIsComplete] = useState(false);
 const [followupText, setFollowupText] = useState('');
 const [isSendingFollowup, setIsSendingFollowup] = useState(false);

 const eventSourceRef = useRef<EventSource | null>(null);
 const bottomRef = useRef<HTMLDivElement>(null);
 const pendingChunkRef = useRef('');
 const flushTimerRef = useRef<number | null>(null);

 const flushPendingChunk = useCallback(() => {
  if (!pendingChunkRef.current) return;
  const chunk = pendingChunkRef.current;
  pendingChunkRef.current = '';
  setMessages((prev) => {
   const next = [...prev];
   const lastIndex = next.length - 1;
   const last = next[lastIndex];
   if (!last || last.role !== 'ai') return prev;
   next[lastIndex] = { ...last, content: last.content + chunk };
   return next;
  });
 }, []);

 const stopStream = useCallback((updateState = true) => {
  if (flushTimerRef.current !== null) {
   window.clearTimeout(flushTimerRef.current);
   flushTimerRef.current = null;
  }
  flushPendingChunk();
  if (eventSourceRef.current) {
   eventSourceRef.current.close();
   eventSourceRef.current = null;
  }
  if (updateState) setIsStreaming(false);
 }, [flushPendingChunk]);

 const startStream = useCallback((customPrompt?: string) => {
  setIsStreaming(true);
  setError(null);
  const isFollowup = Boolean(customPrompt);
  const messageId = `${Date.now()}-ai`;

  setMessages((prev) =>
   isFollowup ? [...prev, { id: messageId, role: 'ai', content: '', isStreaming: true }] : [{ id: messageId, role: 'ai', content: '', isStreaming: true }]
  );

  const baseUrl = `/${locale}/api/reading/sessions/${sessionId}/stream?language=${encodeURIComponent(locale)}`;
  const finalUrl = customPrompt ? `${baseUrl}&followupQuestion=${encodeURIComponent(customPrompt)}` : baseUrl;
  eventSourceRef.current = new EventSource(finalUrl);

  eventSourceRef.current.onmessage = (event) => {
   if (event.data === '[DONE]') {
    stopStream();
    if (!isFollowup) setIsComplete(true);
    setIsSendingFollowup(false);
    setMessages((prev) => {
     const next = [...prev];
     const last = next[next.length - 1];
     if (last && last.role === 'ai') last.isStreaming = false;
     return next;
    });
    queryClient.invalidateQueries({ queryKey: checkinQueryKeys.streakStatus });
    if (onComplete && !isFollowup) onComplete();
    return;
   }

   pendingChunkRef.current += event.data.replace(/\\n/g, '\n');
   if (flushTimerRef.current === null) {
    flushTimerRef.current = window.setTimeout(() => {
     flushPendingChunk();
     flushTimerRef.current = null;
    }, 48);
   }
  };

  eventSourceRef.current.onerror = () => {
   stopStream();
   setError(errorStreamMessage);
   setIsSendingFollowup(false);
  };
 }, [errorStreamMessage, flushPendingChunk, locale, onComplete, queryClient, sessionId, stopStream]);

 const handleFollowupSubmit = useCallback((event: FormEvent<HTMLFormElement>) => {
  event.preventDefault();
  if (!followupText.trim() || isStreaming || isSendingFollowup) return;
  const question = followupText.trim();
  setFollowupText('');
  setIsSendingFollowup(true);
  setMessages((prev) => [...prev, { id: `${Date.now()}-user`, role: 'user', content: question }]);
  startStream(question);
 }, [followupText, isSendingFollowup, isStreaming, startStream]);

 useEffect(() => {
  const timerId = window.setTimeout(() => {
   if (!eventSourceRef.current) startStream();
  }, 100);
  return () => {
   window.clearTimeout(timerId);
   stopStream(false);
  };
 }, [sessionId, startStream, stopStream]);

 return {
  messages,
  error,
  isStreaming,
  isComplete,
  followupText,
  isSendingFollowup,
  bottomRef,
  setFollowupText,
  startStream,
  handleFollowupSubmit,
 };
}
