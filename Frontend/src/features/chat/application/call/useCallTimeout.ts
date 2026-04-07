'use client';

import { useEffect, useRef } from 'react';
import { useCallStore } from './useCallStore';


export function useCallTimeout(endCallCallback: (sessionID: string, reason: string) => void) {
  const { uiState, session, isCaller, reset } = useCallStore();
  const timeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const timeoutSeconds = Number(process.env.NEXT_PUBLIC_CALL_RING_TIMEOUT_SECONDS ?? '60');
  const timeoutMs = Number.isFinite(timeoutSeconds) && timeoutSeconds > 0
    ? timeoutSeconds * 1000
    : 60_000;

  useEffect(() => {
    if (uiState === 'ringing' || uiState === 'incoming') {
      timeoutRef.current = setTimeout(() => {
        if (isCaller && session?.id) {
          endCallCallback(session.id, 'timeout');
        } else {
          reset();
        }
      }, timeoutMs);
    } else {
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
        timeoutRef.current = null;
      }
    }

    return () => {
      if (timeoutRef.current) {
        clearTimeout(timeoutRef.current);
      }
    };
  }, [uiState, isCaller, session, endCallCallback, reset, timeoutMs]);
}
