'use client';

import { useEffect, useRef } from 'react';
import { useCallStore } from './useCallStore';


export function useCallTimeout(endCallCallback: (sessionID: string, reason: string) => void) {
  const { uiState, session, isCaller, reset, ringTimeoutSeconds, joinTimeoutSeconds } = useCallStore();
  const timeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const ringSeconds = Number(ringTimeoutSeconds ?? process.env.NEXT_PUBLIC_CALL_RING_TIMEOUT_SECONDS ?? '60');
  const joinSeconds = Number(joinTimeoutSeconds ?? process.env.NEXT_PUBLIC_CALL_JOIN_TIMEOUT_SECONDS ?? '45');
  const ringTimeoutMs = Number.isFinite(ringSeconds) && ringSeconds > 0 ? ringSeconds * 1000 : 60_000;
  const joinTimeoutMs = Number.isFinite(joinSeconds) && joinSeconds > 0 ? joinSeconds * 1000 : 45_000;

  useEffect(() => {
    if (uiState === 'requested' || uiState === 'incoming') {
      timeoutRef.current = setTimeout(() => {
        if (session?.id) {
          endCallCallback(session.id, 'ring_timeout');
        } else {
          reset();
        }
      }, ringTimeoutMs);
    } else if (uiState === 'accepted' || uiState === 'joining') {
      timeoutRef.current = setTimeout(() => {
        if (session?.id) {
          endCallCallback(session.id, 'join_timeout');
        } else if (isCaller) {
          reset();
        } else {
          useCallStore.getState().setFailed('join_timeout', 'JOIN_TIMEOUT');
        }
      }, joinTimeoutMs);
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
  }, [uiState, isCaller, session, endCallCallback, reset, ringTimeoutMs, joinTimeoutMs]);
}
