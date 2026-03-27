'use client';

import { useEffect, useRef } from 'react';
import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { getSignalRHubUrl } from '@/shared/infrastructure/realtime/signalRUrl';

export function usePresenceConnection() {
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const token = useAuthStore((state) => state.token);
 const connectionRef = useRef<HubConnection | null>(null);
 const queryClient = useQueryClient();

 useEffect(() => {
  if (!isAuthenticated || !token) {
   const existing = connectionRef.current;
   if (
    existing &&
    (existing.state === HubConnectionState.Connected
     || existing.state === HubConnectionState.Reconnecting
     || existing.state === HubConnectionState.Disconnecting)
   ) {
    void existing.stop().catch(() => undefined);
   }
   connectionRef.current = null;
   return;
  }

  let cancelled = false;
  let hubConnection: HubConnection | null = null;
  let heartbeatInterval: NodeJS.Timeout | null = null;

  const init = async () => {
   const signalR = await import('@microsoft/signalr');
   // Kết nối TRỰC TIẾP tới Backend, không qua proxy Next.js
   // Lý do: Next.js rewrites() không hỗ trợ WebSocket upgrade,
   // khiến SignalR không thể thiết lập kết nối thời gian thực.
   const hubUrl = getSignalRHubUrl('/api/v1/presence');
   logger.info('[PresenceRealtimeSync]', `Connecting to: ${hubUrl}`);
   hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl, { 
     accessTokenFactory: () => token ?? '',
    })
    .withAutomaticReconnect([0, 2000, 5000, 10000, 30000]) // Auto reconnect như ChatHub
    .configureLogging(signalR.LogLevel.Warning)
    .build();

   // Tăng timeout lên 60s (mặc định 30s) để tránh bị ngắt kết nối do delay qua proxy Next.js
   hubConnection.serverTimeoutInMilliseconds = 60000;

   // Khi có một user thay đổi trạng thái (timeout hoặc log in)
   hubConnection.on('UserStatusChanged', (userId: string, status: string) => {
    // Invalidate readers directory & profile queries để UI tự fetch lại data mới nhất
    void queryClient.invalidateQueries({ queryKey: ['readers'] });
    void queryClient.invalidateQueries({ queryKey: ['reader', userId] });
    void queryClient.invalidateQueries({ queryKey: ['chat', 'inbox'] });
   });

   hubConnection.on('Error', (error: string) => {
    logger.error('[PresenceRealtimeSync] hub error', error);
   });

   hubConnection.onreconnecting((error) => {
    logger.warn('[PresenceRealtimeSync] reconnecting due to error', error?.message || 'Unknown error');
   });

   hubConnection.onreconnected((connectionId) => {
    logger.info('[PresenceRealtimeSync] reconnected. new id:', connectionId || 'unknown');
   });

   hubConnection.onclose((error) => {
    if (error) {
     logger.error('[PresenceRealtimeSync] connection closed with error', error.message);
    }
   });

   try {
    logger.info('[PresenceRealtimeSync]', 'Starting connection...');
    await hubConnection.start();
    logger.info('[PresenceRealtimeSync]', `Connected! State: ${hubConnection.state}`);
    if (cancelled) {
     if (hubConnection.state !== HubConnectionState.Disconnected) {
      await hubConnection.stop().catch(() => undefined);
     }
     return;
    }
    connectionRef.current = hubConnection;
    
    // Bắt đầu gửi heartbeat định kỳ mỗi 5 phút (300,000 ms)
    heartbeatInterval = setInterval(() => {
     if (hubConnection?.state === HubConnectionState.Connected) {
      hubConnection.invoke('Heartbeat').catch(err => 
       logger.error('[PresenceRealtimeSync] heartbeat error', err)
      );
     }
    }, 5 * 60 * 1000);
    
   } catch (error) {
    logger.error('[PresenceRealtimeSync] connect failed', error);
   }
  };

  void init();

  return () => {
   cancelled = true;
   if (heartbeatInterval) {
    clearInterval(heartbeatInterval);
   }
   if (
    hubConnection &&
    (hubConnection.state === HubConnectionState.Connected
     || hubConnection.state === HubConnectionState.Reconnecting
     || hubConnection.state === HubConnectionState.Disconnecting)
   ) {
    void hubConnection.stop().catch(() => undefined);
   }
   connectionRef.current = null;
  };
 }, [isAuthenticated, token, queryClient]);
}
