import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import { useWalletStore } from '@/store/walletStore';
import { logger } from '@/shared/infrastructure/logging/logger';

const HEARTBEAT_INTERVAL_MS = 5 * 60 * 1000;

export function registerPresenceConnectionHandlers(hubConnection: HubConnection, queryClient: QueryClient) {
  let inboxInvalidateTimeout: NodeJS.Timeout | null = null;

  const invalidateInbox = () => {
    if (inboxInvalidateTimeout) {
      clearTimeout(inboxInvalidateTimeout);
    }

    inboxInvalidateTimeout = setTimeout(() => {
      void queryClient.invalidateQueries({ queryKey: ['chat', 'inbox'] });
    }, 1000);
  };

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

  hubConnection.on('conversation.updated', () => {
    logger.info('[PresenceRealtimeSync]', 'conversation.updated received. Invalidating inbox queries...');
    invalidateInbox();
  });

  hubConnection.on('gamification.quest_completed', () => {
    void queryClient.invalidateQueries({ queryKey: ['gamification', 'quests'] });
    void queryClient.invalidateQueries({ queryKey: ['wallet'] });
  });

  hubConnection.on('gamification.achievement_unlocked', () => {
    void queryClient.invalidateQueries({ queryKey: ['gamification', 'achievements'] });
    void queryClient.invalidateQueries({ queryKey: ['gamification', 'titles'] });
  });

  hubConnection.on('gamification.card_level_up', () => {
    void queryClient.invalidateQueries({ queryKey: ['collection'] });
  });

  hubConnection.on('gacha.result', () => {
    void queryClient.invalidateQueries({ queryKey: ['gacha', 'history'] });
    void queryClient.invalidateQueries({ queryKey: ['collection'] });
    void useWalletStore.getState().fetchBalance();
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

  return {
    dispose: () => {
      if (inboxInvalidateTimeout) {
        clearTimeout(inboxInvalidateTimeout);
      }
    },
    startHeartbeat: () => {
      return setInterval(() => {
        if (hubConnection.state === HubConnectionState.Connected) {
          hubConnection.invoke('Heartbeat').catch((err) => {
            logger.error('[PresenceRealtimeSync] heartbeat error', err);
          });
        }
      }, HEARTBEAT_INTERVAL_MS);
    },
  };
}
