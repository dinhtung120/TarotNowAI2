'use client';
import { useEffect, useRef } from 'react';
import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import { useWalletStore } from '@/store/walletStore';
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
  let inboxInvalidateTimeout: NodeJS.Timeout | null = null;
  const init = async () => {
   const signalR = await import('@microsoft/signalr');
   const hubUrl = getSignalRHubUrl('/api/v1/presence');
   logger.info('[PresenceRealtimeSync]', `Connecting to: ${hubUrl}`);
   hubConnection = new signalR.HubConnectionBuilder()
    .withUrl(hubUrl, { 
			accessTokenFactory: () => useAuthStore.getState().token ?? '',
    })
    .withAutomaticReconnect([0, 2000, 5000, 10000, 30000]) 
    .configureLogging(signalR.LogLevel.Debug)
    .build();
   hubConnection.serverTimeoutInMilliseconds = 120000;
   hubConnection.on('UserStatusChanged', (userId: string) => {
    void queryClient.invalidateQueries({ queryKey: ['readers'] });
    void queryClient.invalidateQueries({ queryKey: ['reader', userId] });
   });
   hubConnection.on('notification.new', () => {
    void queryClient.invalidateQueries({ queryKey: ['notifications'] });
   });
   hubConnection.on('wallet.balance_changed', () => {
    logger.info('[PresenceRealtimeSync]', 'wallet.balance_changed received. Refetching balance...');
    void useWalletStore.getState().fetchBalance();
   });
   const invalidateInbox = () => {
    if (inboxInvalidateTimeout) clearTimeout(inboxInvalidateTimeout);
    inboxInvalidateTimeout = setTimeout(() => {
     void queryClient.invalidateQueries({ queryKey: ['chat', 'inbox'] });
    }, 1000);
   };
   hubConnection.on('conversation.updated', () => {
    logger.info('[PresenceRealtimeSync]', 'conversation.updated received. Invalidating inbox queries...');
    invalidateInbox();
   });
   
   hubConnection.on('gamification.quest_completed', () => {
    logger.info('[PresenceRealtimeSync]', 'gamification.quest_completed received');
    void queryClient.invalidateQueries({ queryKey: ['gamification', 'quests'] });
    void queryClient.invalidateQueries({ queryKey: ['wallet'] });
   });
   hubConnection.on('gamification.achievement_unlocked', () => {
    logger.info('[PresenceRealtimeSync]', 'gamification.achievement_unlocked received');
    void queryClient.invalidateQueries({ queryKey: ['gamification', 'achievements'] });
    void queryClient.invalidateQueries({ queryKey: ['gamification', 'titles'] });
   });
   hubConnection.on('gamification.card_level_up', () => {
    logger.info('[PresenceRealtimeSync]', 'gamification.card_level_up received');
    void queryClient.invalidateQueries({ queryKey: ['collection'] });
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
   if (inboxInvalidateTimeout) {
    clearTimeout(inboxInvalidateTimeout);
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
