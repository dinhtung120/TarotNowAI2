'use client';
import { useEffect, useRef, useState } from 'react';
import { useAuthStore } from '@/store/authStore';
import { CallSessionDto } from '../../domain/callTypes';
import { useCallStore } from './useCallStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { HubConnectionState, type HubConnection, type ILogger, type LogLevel } from '@microsoft/signalr';
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
        })
        .withAutomaticReconnect([0, 2000, 5000])
        .configureLogging(customLogger)
        .build();
      hubConnection.serverTimeoutInMilliseconds = 120000;
      hubConnection.on('call.initiated', (initiatedSession: CallSessionDto) => {
        const store = useCallStore.getState();
        if (store.uiState === 'ringing') {
          useCallStore.setState({ session: initiatedSession });
        }
      });
      hubConnection.on('call.incoming', (incomingSession: CallSessionDto) => {
        const state = useCallStore.getState();
        if (state.uiState !== 'idle' && state.uiState !== 'ended') return;
        useCallStore.getState().setIncomingCall(incomingSession);
      });
      hubConnection.on('call.accepted', (acceptedSession: CallSessionDto) => {
        useCallStore.getState().setConnected(acceptedSession);
      });
      hubConnection.on('call.rejected', () => {
        useCallStore.getState().setEnded();
        setTimeout(() => useCallStore.getState().reset(), 3000);
      });
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
  }, [token]);
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
  const invoke = async (method: string, ...args: unknown[]) => {
    if (!connectionRef.current || !connected) return;
    await connectionRef.current.invoke(method, ...args);
  };
  const initiateCall = async (conversationId: string, type: string) => invoke('InitiateCall', conversationId, type);
  const respondCall = async (callSessionId: string, accept: boolean) => invoke('RespondCall', callSessionId, accept);
  const endCall = async (callSessionId: string, reason: string = 'normal') => invoke('EndCall', callSessionId, reason);
  const sendOffer = async (conversationId: string, sdpOffer: RTCSessionDescriptionInit) => invoke('SendOffer', conversationId, sdpOffer);
  const sendAnswer = async (conversationId: string, sdpAnswer: RTCSessionDescriptionInit) => invoke('SendAnswer', conversationId, sdpAnswer);
  const sendIceCandidate = async (conversationId: string, candidate: RTCIceCandidateInit) => invoke('SendIceCandidate', conversationId, candidate);
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
