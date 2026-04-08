'use client';

import { useCallback, useEffect, useRef, useState } from 'react';
import { HubConnectionState, type HubConnection, type ILogger, type LogLevel } from '@microsoft/signalr';
import { useAuthStore } from '@/store/authStore';
import { useCallStore } from './useCallStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { acceptCallRequest, endCallRequest, issueCallTokenRequest, startCallRequest } from './callApi';
import type { CallJoinTicketDto, CallSessionDto, CallTimeoutsDto, CallType } from '@/features/chat/domain/callTypes';

interface CallRealtimeEventPayload {
 sessionId: string;
 conversationId: string;
 status: string;
 reason?: string | null;
 session?: CallSessionDto;
 timeouts?: CallTimeoutsDto;
}

interface CallRealtimeErrorPayload {
 errorKey?: string;
 message?: string;
}

export function useCallSignaling() {
 const token = useAuthStore(s => s.token);
 const [connected, setConnected] = useState(false);
 const connectionRef = useRef<HubConnection | null>(null);

 useEffect(() => {
  if (!token) return;
  let cancelled = false;
  let hubConnection: HubConnection | null = null;

  const initConnection = async () => {
   const signalR = await import('@microsoft/signalr');
   const { getSignalRHubUrl } = await import('@/shared/infrastructure/realtime/signalRUrl');
   const customLogger: ILogger = {
    log: (logLevel: LogLevel, message: string) => {
     if (logLevel === signalR.LogLevel.Error || logLevel === signalR.LogLevel.Critical) {
      console.warn('[SignalR Call] ' + message);
     }
    },
   };

   hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(getSignalRHubUrl('/api/v1/call'), {
     accessTokenFactory: () => useAuthStore.getState().token ?? '',
     skipNegotiation: true, // Bỏ qua bước thương lượng để kết nối thẳng qua WebSocket
     transport: signalR.HttpTransportType.WebSockets, // Ép sử dụng WebSocket
    })
    .withAutomaticReconnect([0, 2000, 5000])
    .configureLogging(customLogger)
    .build();

   hubConnection.serverTimeoutInMilliseconds = 120000;
   registerCallEvents(hubConnection);
   hubConnection.onclose(() => setConnected(false));
   hubConnection.onreconnecting(() => setConnected(false));
   hubConnection.onreconnected(() => setConnected(true));

   try {
    await hubConnection.start();
    if (cancelled) {
      if (hubConnection.state !== HubConnectionState.Disconnected) {
       await hubConnection.stop().catch(() => undefined);
      }
      return;
    }

    connectionRef.current = hubConnection;
    setConnected(true);
   } catch (error) {
    logger.warn('[CallHub] Connection failed', error);
   }
  };

  void initConnection();
  return () => {
   cancelled = true;
   if (!hubConnection) return;

   if (hubConnection.state === HubConnectionState.Connected || hubConnection.state === HubConnectionState.Reconnecting) {
    hubConnection.stop().catch(() => undefined);
   }

   connectionRef.current = null;
  };
 }, [token]);

  /*
   * Khởi tạo cuộc gọi (Caller / Người gọi)
   * Sử dụng useCallback để giữ reference cố định giữa các lần render,
   * tránh kích hoạt lại các useEffect phụ thuộc vào hàm này.
   */
  const startCall = useCallback(async (conversationId: string, type: CallType): Promise<CallJoinTicketDto> => {
    const authToken = useAuthStore.getState().token;
    if (!authToken) throw new Error('Unauthorized');
    return startCallRequest(authToken, conversationId, type);
  }, []);

  /*
   * Chấp nhận cuộc gọi (Callee / Người nghe)
   * Giữ reference cố định để tối ưu hóa hiệu năng render.
   */
  const acceptCall = useCallback(async (callSessionId: string): Promise<CallJoinTicketDto> => {
    const authToken = useAuthStore.getState().token;
    if (!authToken) throw new Error('Unauthorized');
    return acceptCallRequest(authToken, callSessionId);
  }, []);

  /*
   * Cấp lại token LiveKit khi cần (Refresh Token)
   * Đây là hàm quan trọng được truyền vào useLiveKitRoom.
   * Việc wrap trong useCallback là bắt buộc để tránh ngắt kết nối Room do re-render.
   */
  const issueToken = useCallback(async (callSessionId: string): Promise<CallJoinTicketDto | null> => {
    const authToken = useAuthStore.getState().token;
    if (!authToken) return null;

    try {
      return await issueCallTokenRequest(authToken, callSessionId);
    } catch (error) {
      logger.warn('[CallAPI] Token refresh failed', error);
      return null;
    }
  }, []);

  /*
   * Kết thúc cuộc gọi
   * Reference được giữ nguyên để đảm bảo logic dọn dẹp (cleanup) chạy ổn định.
   */
  const endCall = useCallback(async (callSessionId: string, reason: string = 'normal') => {
    const authToken = useAuthStore.getState().token;
    if (!authToken) return;

    try {
      await endCallRequest(authToken, callSessionId, reason);
    } catch (error) {
      logger.warn('[CallAPI] End call failed', error);
    }
  }, []);

 return { connected, startCall, acceptCall, issueToken, endCall };
}

function registerCallEvents(hubConnection: HubConnection) {
 hubConnection.on('call.incoming', (payload: CallRealtimeEventPayload) => {
  const state = useCallStore.getState();
  if (state.uiState !== 'idle' && state.uiState !== 'ended' && state.uiState !== 'failed') return;
  if (!payload?.session) return;
  useCallStore.getState().setIncomingCall(payload.session, payload.timeouts);
 });

 hubConnection.on('call.accepted', (payload: CallRealtimeEventPayload) => {
  const store = useCallStore.getState();
  if (payload?.session) {
   store.setAccepted(payload.session);
  } else {
   store.setAccepted();
  }

  if (store.joinTicket) {
   useCallStore.getState().setJoining(payload?.session);
  }
 });

 hubConnection.on('call.ended', (payload: CallRealtimeEventPayload) => {
  if (payload?.status === 'failed') {
   useCallStore.getState().setFailed(payload.reason ?? 'call_failed');
  } else {
   useCallStore.getState().setEnded(payload?.reason ?? null);
  }

  setTimeout(() => useCallStore.getState().reset(), 1000);
 });

 hubConnection.on('call.error', (payload: CallRealtimeErrorPayload | string | null) => {
  const errorKey = typeof payload === 'string' ? payload : payload?.errorKey ?? null;
  const message = typeof payload === 'string' ? payload : payload?.message ?? 'Call error';
  logger.warn('[CallHub] Call Error', message, { errorKey });
  useCallStore.getState().setFailed(message, errorKey);
  setTimeout(() => useCallStore.getState().reset(), 1000);
 });
}
