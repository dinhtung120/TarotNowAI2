'use client';

import React, { createContext, useContext, useCallback, useMemo } from 'react';
import { useCallSignaling } from '../../application/call/useCallSignaling';
import { useCallTimeout } from '../../application/call/useCallTimeout';
import { useLiveKitRoom } from '../../application/call/useLiveKitRoom';
import { useCallStore } from '../../application/call/useCallStore';
import { CallApiError } from '../../application/call/callApi';
import type { CallType } from '../../domain/callTypes';
import { logger } from '@/shared/infrastructure/logging/logger';

const CallContext = createContext<{
  initiateCall: (conversationId: string, type: CallType) => Promise<void>;
  respondCall: (callSessionId: string, accept: boolean) => Promise<void>;
  endCall: (callSessionId: string, reason?: string) => Promise<void>;
  connected: boolean;
} | null>(null);

export const CallProvider = ({ children }: { children: React.ReactNode }) => {
  const { startCall, acceptCall, issueToken, endCall, connected } = useCallSignaling();
  useLiveKitRoom({ issueToken });

  /*
   * Hàm khởi tạo cuộc gọi, bao gồm kiểm tra quyền truy cập Media (Preflight).
   * Được wrap trong useCallback để tối ưu hóa hiệu năng render cho các component con.
   */
  const initiateCall = useCallback(async (conversationId: string, type: CallType) => {
    const resolvedType = await preflightType(type);
    const ticket = await startCall(conversationId, resolvedType);
    useCallStore.getState().setOutgoingCall(ticket);
  }, [startCall]);

  /*
   * Hàm phản hồi cuộc gọi (Chấp nhận/Từ chối).
   */
  const respondCall = useCallback(async (callSessionId: string, accept: boolean) => {
    if (!accept) {
      await endCall(callSessionId, 'rejected');
      return;
    }

    const ticket = await acceptCall(callSessionId);
    useCallStore.getState().setJoinTicket(ticket);
  }, [acceptCall, endCall]);

  useCallTimeout(endCall);

  /*
   * Memoize giá trị Context để ngăn chặn các component đang lắng nghe (useCallContext)
   * bị re-render không cần thiết khi CallProvider render lại.
   */
  const contextValue = useMemo(() => ({
    initiateCall,
    respondCall,
    endCall,
    connected
  }), [initiateCall, respondCall, endCall, connected]);

  return (
    <CallContext.Provider value={contextValue}>
      {children}
    </CallContext.Provider>
  );
};

export const useCallContext = () => {
  const context = useContext(CallContext);
  if (!context) {
    throw new Error('useCallContext must be used within CallProvider');
  }
  return context;
};

async function preflightType(type: CallType): Promise<CallType> {
  if (type === 'audio') {
    await ensureMediaPermission({ audio: true, video: false });
    return 'audio';
  }

  try {
    await ensureMediaPermission({ audio: true, video: true });
    return 'video';
  } catch (videoError) {
    logger.warn('Call.Media', 'Video preflight failed, fallback to audio.', { error: videoError });
    await ensureMediaPermission({ audio: true, video: false });
    return 'audio';
  }
}

async function ensureMediaPermission(constraints: MediaStreamConstraints) {
  const supportIssue = resolveMediaSupportIssue();
  if (supportIssue) {
    throw new CallApiError('MEDIA_PERMISSION_DENIED', supportIssue.message, {
      cause: supportIssue,
    });
  }

  try {
    const stream = await navigator.mediaDevices.getUserMedia(constraints);
    stream.getTracks().forEach(track => track.stop());
  } catch (error) {
    if (error instanceof CallApiError) throw error;
    throw new CallApiError(
      'MEDIA_PERMISSION_DENIED',
      mapMediaPermissionError(error, constraints),
      { cause: error }
    );
  }
}

function resolveMediaSupportIssue(): { code: string; message: string } | null {
  if (typeof window === 'undefined') {
    return { code: 'NO_WINDOW', message: 'Không thể truy cập media trên ngữ cảnh hiện tại.' };
  }
  if (typeof navigator === 'undefined') return { code: 'NO_NAVIGATOR', message: 'Trình duyệt không hỗ trợ gọi thoại/video.' };

  const host = window.location.hostname;
  const isLocalhost = host === 'localhost' || host === '127.0.0.1';
  if (!window.isSecureContext && !isLocalhost) {
    return {
      code: 'INSECURE_CONTEXT',
      message: `Trang hiện chạy trên HTTP (${window.location.origin}). Hãy dùng HTTPS hoặc mở bằng localhost để gọi.`,
    };
  }

  if (!navigator.mediaDevices) {
    return {
      code: 'NO_MEDIA_DEVICES',
      message: 'Trình duyệt hoặc WebView hiện tại không cung cấp mediaDevices cho microphone/camera.',
    };
  }

  if (typeof navigator.mediaDevices.getUserMedia !== 'function') {
    return {
      code: 'NO_GET_USER_MEDIA',
      message: 'Trình duyệt không hỗ trợ API getUserMedia.',
    };
  }

  return null;
}

function mapMediaPermissionError(error: unknown, constraints: MediaStreamConstraints): string {
  const needVideo = !!constraints.video;
  const target = needVideo ? 'microphone/camera' : 'microphone';

  if (typeof window !== 'undefined') {
    const host = window.location.hostname;
    const isLocalhost = host === 'localhost' || host === '127.0.0.1';
    if (!window.isSecureContext && !isLocalhost) {
      return 'Trình duyệt chỉ cho phép gọi trên HTTPS (hoặc localhost).';
    }
  }

  if (error instanceof DOMException) {
    if (error.name === 'NotAllowedError' || error.name === 'SecurityError') {
      return `Bạn đã chặn quyền ${target}. Hãy bật lại quyền trong trình duyệt rồi thử lại.`;
    }

    if (error.name === 'NotFoundError' || error.name === 'DevicesNotFoundError') {
      return needVideo
        ? 'Không tìm thấy microphone hoặc camera trên thiết bị.'
        : 'Không tìm thấy microphone trên thiết bị.';
    }

    if (error.name === 'NotReadableError' || error.name === 'TrackStartError') {
      return 'Microphone/camera đang được ứng dụng khác sử dụng. Hãy đóng ứng dụng đó rồi thử lại.';
    }
  }

  if (error instanceof Error && error.message) {
    return error.message;
  }

  return `Bạn cần cấp quyền ${target} để gọi.`;
}
