'use client';

import { useCallback, useEffect, useRef, useState } from 'react';
import { useQueryClient } from '@tanstack/react-query';
import { checkinQueryKeys } from '@/features/checkin/streak/checkinQueryKeys';
import { fetchJsonOrThrow } from '@/shared/application/gateways/clientFetch';
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

 const createStreamTicket = useCallback(async (followupQuestion: string) => {
  const response = await fetchJsonOrThrow<{ streamToken: string }>(
   `/${locale}/api/reading/sessions/${sessionId}/stream-ticket`,
   {
    method: 'POST',
    credentials: 'include',
    cache: 'no-store',
    headers: {
     'Content-Type': 'application/json',
    },
    body: JSON.stringify({
     followupQuestion,
     language: locale,
     idempotencyKey: buildFollowupIdempotencyKey(),
    }),
   },
   errorStreamMessage,
   8_000,
  );

  return response.streamToken;
 }, [errorStreamMessage, locale, sessionId]);

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

 const startStream = useCallback(async (customPrompt?: string) => {
  setIsStreaming(true);
  setError(null);
  const isFollowup = Boolean(customPrompt);
  const messageId = `${Date.now()}-ai`;

  try {
   const baseUrl = `/${locale}/api/reading/sessions/${sessionId}/stream?language=${encodeURIComponent(locale)}`;
   const finalUrl = customPrompt
    ? `${baseUrl}&streamToken=${encodeURIComponent(await createStreamTicket(customPrompt))}`
    : baseUrl;

   setMessages((prev) =>
    isFollowup
     ? [...prev, { id: messageId, role: 'ai', content: '', isStreaming: true }]
     : [{ id: messageId, role: 'ai', content: '', isStreaming: true }],
   );

   eventSourceRef.current = new EventSource(finalUrl);
   let handledStreamError = false;

   eventSourceRef.current.addEventListener('stream_error', (event) => {
    handledStreamError = true;
    stopStream();
    setError(extractStreamErrorDetail((event as MessageEvent<string>).data, errorStreamMessage));
    setIsSendingFollowup(false);
    setMessages((prev) => prev.filter((message) => message.id !== messageId));
   });

   eventSourceRef.current.onerror = () => {
    if (handledStreamError) {
     return;
    }

    stopStream();
    setError(errorStreamMessage);
    setIsSendingFollowup(false);
    setMessages((prev) => prev.filter((message) => message.id !== messageId));
   };

   eventSourceRef.current.onmessage = (event) => {
    if (event.data === '[DONE]') {
     stopStream();
     if (!isFollowup) setIsComplete(true);
     setIsSendingFollowup(false);
     setMessages((prev) => {
      const next = [...prev];
      const lastIndex = next.length - 1;
      const last = next[lastIndex];
      if (last && last.role === 'ai') {
       next[lastIndex] = { ...last, isStreaming: false };
      }
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
  } catch {
   stopStream();
   setError(errorStreamMessage);
   setIsSendingFollowup(false);
   setMessages((prev) => prev.filter((message) => message.id !== messageId));
  }
 }, [createStreamTicket, errorStreamMessage, flushPendingChunk, locale, onComplete, queryClient, sessionId, stopStream]);

 const handleFollowupSubmit = useCallback(({ followupText: submittedFollowupText }: { followupText: string }) => {
  if (!submittedFollowupText.trim() || isStreaming || isSendingFollowup) return;
  const question = submittedFollowupText.trim();
  setFollowupText('');
  setIsSendingFollowup(true);
  setMessages((prev) => [...prev, { id: `${Date.now()}-user`, role: 'user', content: question }]);
  startStream(question);
 }, [isSendingFollowup, isStreaming, startStream]);

 useEffect(() => {
  const timerId = window.setTimeout(() => {
   if (!eventSourceRef.current) {
    void startStream();
   }
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

function buildFollowupIdempotencyKey(): string {
 return typeof crypto !== 'undefined' && typeof crypto.randomUUID === 'function'
  ? `reading_followup_${crypto.randomUUID()}`
  : `reading_followup_${Date.now()}`;
}

function extractStreamErrorDetail(payload: string, fallback: string): string {
 if (!payload) {
  return fallback;
 }

 try {
  const parsed = JSON.parse(payload) as { detail?: string };
  const detail = parsed.detail?.trim();
  return detail || fallback;
 } catch {
  return payload.trim() || fallback;
 }
}
