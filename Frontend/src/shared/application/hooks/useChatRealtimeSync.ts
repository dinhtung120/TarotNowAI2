'use client';

import { useEffect, useRef } from 'react';
import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import { logger } from '@/shared/infrastructure/logging/logger';

export function useChatRealtimeSync() {
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const connectionRef = useRef<HubConnection | null>(null);
 const queryClient = useQueryClient();

 useEffect(() => {
  if (!isAuthenticated) {
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

  const invalidateChatQueries = () => {
   void queryClient.invalidateQueries({ queryKey: ['chat', 'inbox'] });
   void queryClient.invalidateQueries({ queryKey: ['chat', 'unread-badge'] });
  };

  const init = async () => {
   const signalR = await import('@microsoft/signalr');
   hubConnection = new signalR.HubConnectionBuilder()
    .withUrl('/api/v1/chat', { withCredentials: true })
    .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
    .configureLogging(signalR.LogLevel.Warning)
    .build();

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
 }, [isAuthenticated, queryClient]);
}
