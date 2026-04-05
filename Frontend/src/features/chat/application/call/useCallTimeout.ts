'use client';

import { useEffect, useRef } from 'react';
import { useCallStore } from './useCallStore';

/**
 * Đếm thời gian tự động huỷ cuộc gọi nếu chuông kêu quá timeout (60 giây)
 */
export function useCallTimeout(endCallCallback: (sessionID: string, reason: string) => void) {
  const { uiState, session, isCaller, reset } = useCallStore();
  const timeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);

  useEffect(() => {
    if (uiState === 'ringing' || uiState === 'incoming') {
      // 60 giây chờ chuông
      timeoutRef.current = setTimeout(() => {
        if (isCaller && session?.id) {
          // Người gọi chủ động báo huỷ do timeout
          endCallCallback(session.id, 'timeout');
        } else {
          // Người nhận tự tắt giao diện
          reset();
        }
      }, 60000);
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
  }, [uiState, isCaller, session, endCallCallback, reset]);
}
