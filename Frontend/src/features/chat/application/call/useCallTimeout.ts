'use client';

import { useEffect, useRef } from 'react';
import { useCallStore } from './useCallStore';

/**
 * Đếm thời gian tự động huỷ cuộc gọi nếu chuông kêu quá timeout (60 giây)
 */
export function useCallTimeout(endCallCallback: (sessionID: string, reason: string) => void) {
  const { uiState, session, isCaller, reset } = useCallStore();
  const timeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);
  const timeoutSeconds = Number(process.env.NEXT_PUBLIC_CALL_RING_TIMEOUT_SECONDS ?? '60');
  const timeoutMs = Number.isFinite(timeoutSeconds) && timeoutSeconds > 0
    ? timeoutSeconds * 1000
    : 60_000;

  useEffect(() => {
    if (uiState === 'ringing' || uiState === 'incoming') {
      // Timeout chờ chuông (mặc định 60s, có thể override bằng NEXT_PUBLIC_CALL_RING_TIMEOUT_SECONDS)
      timeoutRef.current = setTimeout(() => {
        if (isCaller && session?.id) {
          // Người gọi chủ động báo huỷ do timeout
          endCallCallback(session.id, 'timeout');
        } else {
          // Người nhận tự tắt giao diện
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
