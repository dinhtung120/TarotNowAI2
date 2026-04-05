'use client';

import { useEffect, useRef, useState } from 'react';
import { useAuthStore } from '@/store/authStore';
import { CallSessionDto } from '../../domain/callTypes';
import { useCallStore } from './useCallStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { HubConnectionState, type HubConnection, type ILogger, type LogLevel } from '@microsoft/signalr';

/**
 * Hook SignalR quản lý kết nối CallHub và nhận sự kiện từ Backend.
 * 
 * FIX #5: Không destructure state từ useCallStore ở component level
 * để tránh stale closure và re-render loop. Sử dụng useCallStore.getState()
 * bên trong callbacks thay vì closure.
 */
export function useCallSignaling() {
  const token = useAuthStore(s => s.token);
  const [connected, setConnected] = useState(false);
  const connectionRef = useRef<HubConnection | null>(null);

  useEffect(() => {
    if (!token) return;

    let cancelled = false;
    let hubConnection: HubConnection | null = null;

    const initConn = async () => {
      const signalR = await import('@microsoft/signalr');
      const { getSignalRHubUrl } = await import('@/shared/infrastructure/realtime/signalRUrl');
      
      const customLogger: ILogger = {
        log: (logLevel: LogLevel, message: string) => {
          if (logLevel === signalR.LogLevel.Error || logLevel === signalR.LogLevel.Critical) {
            console.warn(`[SignalR Call] ${message}`);
          }
        }
      };

      hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(getSignalRHubUrl('/api/v1/call'), {
          accessTokenFactory: () => useAuthStore.getState().token ?? '',
          /* FIX #23: Giữ negotiate flow bình thường (giống ChatHub).
           * Lỗi "connection not found" THỰC SỰ là do Backend thiếu JWT auth 
           * cho CallHub endpoint (đã sửa ở DependencyInjection.Auth.cs).
           * Không cần skipNegotiation — negotiate flow hoạt động tốt khi auth đúng. */
        })
        .withAutomaticReconnect([0, 2000, 5000])
        .configureLogging(customLogger)
        .build();

      hubConnection.serverTimeoutInMilliseconds = 120000;

      // FIX #4 (frontend): Caller nhận lại session từ Backend sau InitiateCall.
      hubConnection.on('call.initiated', (initiatedSession: CallSessionDto) => {
        const store = useCallStore.getState();
        if (store.uiState === 'ringing') {
          useCallStore.setState({ session: initiatedSession });
        }
      });

      // Xử lý Sự kiện Nhận cuộc gọi
      hubConnection.on('call.incoming', (incomingSession: CallSessionDto) => {
        const state = useCallStore.getState();
        if (state.uiState !== 'idle' && state.uiState !== 'ended') return;
        useCallStore.getState().setIncomingCall(incomingSession);
      });

      // Đối phương đồng ý
      hubConnection.on('call.accepted', (acceptedSession: CallSessionDto) => {
        useCallStore.getState().setConnected(acceptedSession);
      });

      // Đối phương từ chối
      hubConnection.on('call.rejected', () => {
        useCallStore.getState().setEnded();
        setTimeout(() => useCallStore.getState().reset(), 3000);
      });

      // Cuộc gọi kết thúc / Cancel / Timeout / Disconnect
      hubConnection.on('call.ended', () => {
        useCallStore.getState().setEnded();
        setTimeout(() => useCallStore.getState().reset(), 3000);
      });

      hubConnection.on('call.error', (error: unknown) => {
        logger.warn('[CallHub] Call Error', error);
        useCallStore.getState().setEnded();
        setTimeout(() => useCallStore.getState().reset(), 3000);
      });

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
      } catch (err) {
        logger.warn('[CallHub] Connection failed', err);
      }
    };

    void initConn();

    return () => {
      cancelled = true;
      if (hubConnection) {
        if (hubConnection.state === HubConnectionState.Connected
         || hubConnection.state === HubConnectionState.Reconnecting) {
          hubConnection.stop().catch(() => undefined);
        }
      }
      connectionRef.current = null;
    };
  // eslint-disable-next-line react-hooks/exhaustive-deps
  }, [token]);

  // Subscribe webRTC relays
  // FIX #7 (partial): Vẫn dùng CustomEvent nhưng đăng ký listener
  // bên trong useEffect của connection, đảm bảo timing đúng.
  useEffect(() => {
    const handleOffer = (offer: RTCSessionDescriptionInit) => {
      window.dispatchEvent(new CustomEvent('webrtc.offer', { detail: offer }));
    };
    const handleAnswer = (answer: RTCSessionDescriptionInit) => {
      window.dispatchEvent(new CustomEvent('webrtc.answer', { detail: answer }));
    };
    const handleCandidate = (candidate: RTCIceCandidateInit) => {
      window.dispatchEvent(new CustomEvent('webrtc.ice-candidate', { detail: candidate }));
    };

    if (connectionRef.current) {
      connectionRef.current.on('webrtc.offer', handleOffer);
      connectionRef.current.on('webrtc.answer', handleAnswer);
      connectionRef.current.on('webrtc.ice-candidate', handleCandidate);
    }

    return () => {
      if (connectionRef.current) {
        connectionRef.current.off('webrtc.offer', handleOffer);
        connectionRef.current.off('webrtc.answer', handleAnswer);
        connectionRef.current.off('webrtc.ice-candidate', handleCandidate);
      }
    };
  }, [connected]);

  // Hành động gọi - dùng inline function ref stable
  const initiateCall = async (conversationId: string, type: string) => {
    if (connectionRef.current && connected) {
      await connectionRef.current.invoke('InitiateCall', conversationId, type);
    }
  };

  const respondCall = async (callSessionId: string, accept: boolean) => {
    if (connectionRef.current && connected) {
      await connectionRef.current.invoke('RespondCall', callSessionId, accept);
    }
  };

  const endCall = async (callSessionId: string, reason: string = 'normal') => {
    if (connectionRef.current && connected) {
      await connectionRef.current.invoke('EndCall', callSessionId, reason);
    }
  };

  const sendOffer = async (conversationId: string, sdpOffer: RTCSessionDescriptionInit) => {
    if (connectionRef.current && connected) {
      await connectionRef.current.invoke('SendOffer', conversationId, sdpOffer);
    }
  };

  const sendAnswer = async (conversationId: string, sdpAnswer: RTCSessionDescriptionInit) => {
    if (connectionRef.current && connected) {
      await connectionRef.current.invoke('SendAnswer', conversationId, sdpAnswer);
    }
  };

  const sendIceCandidate = async (conversationId: string, candidate: RTCIceCandidateInit) => {
    if (connectionRef.current && connected) {
      await connectionRef.current.invoke('SendIceCandidate', conversationId, candidate);
    }
  };

  return {
    connected,
    initiateCall,
    respondCall,
    endCall,
    sendOffer,
    sendAnswer,
    sendIceCandidate
  };
}
