'use client';

import { useEffect, useRef } from 'react';
import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { getSignalRHubUrl } from '@/shared/infrastructure/realtime/signalRUrl';

export function useChatRealtimeSync() {
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const token = useAuthStore((state) => state.token);
  const connectionRef = useRef<HubConnection | null>(null);
  const queryClient = useQueryClient();
  
  const appStartTimeRef = useRef(Date.now());

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
    let invalidateTimeout: NodeJS.Timeout | null = null;

    
    const invalidateChatQueries = () => {
      if (Date.now() - appStartTimeRef.current < 3000) {
        return;
      }

      if (invalidateTimeout) clearTimeout(invalidateTimeout);
      invalidateTimeout = setTimeout(() => {
        void queryClient.invalidateQueries({ queryKey: ['chat', 'inbox'] });
        void queryClient.invalidateQueries({ queryKey: ['chat', 'unread-badge'] });
      }, 1000); 
    };

    const init = async () => {
      const signalR = await import('@microsoft/signalr');
      hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(getSignalRHubUrl('/api/v1/chat'), { 
          accessTokenFactory: () => useAuthStore.getState().token ?? '',
        })
        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
        .configureLogging(signalR.LogLevel.Warning)
        .build();

      hubConnection.serverTimeoutInMilliseconds = 120000;

      hubConnection.on('conversation.updated', () => invalidateChatQueries());
      hubConnection.on('message.read', () => invalidateChatQueries());
      hubConnection.on('Error', (error: string) => {
        logger.error('[ChatRealtimeSync] hub error', error);
      });

      try {
        await hubConnection.start();
        if (cancelled) {
          if (hubConnection.state !== HubConnectionState.Disconnected) {
            await hubConnection.stop().catch(() => undefined);
          }
          return;
        }
        connectionRef.current = hubConnection;
      } catch (error) {
        logger.error('[ChatRealtimeSync] connect failed', error);
      }
    };

    void init();

    return () => {
      cancelled = true;
      if (invalidateTimeout) clearTimeout(invalidateTimeout);
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
  }, [isAuthenticated, queryClient, token]);
}
