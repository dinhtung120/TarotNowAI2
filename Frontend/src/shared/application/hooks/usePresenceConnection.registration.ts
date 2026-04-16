import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import { useWalletStore } from '@/store/walletStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { invalidateUserStateQueries } from '@/shared/infrastructure/query/invalidateUserStateQueries';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';

const HEARTBEAT_INTERVAL_MS = 5 * 60 * 1000;

export function registerPresenceConnectionHandlers(hubConnection: HubConnection, queryClient: QueryClient) {
  let inboxInvalidateTimeout: NodeJS.Timeout | null = null;

  const invalidateDomains = (domains: Parameters<typeof invalidateUserStateQueries>[1]) => {
    void invalidateUserStateQueries(queryClient, domains);
  };

  const invalidateInbox = () => {
    if (inboxInvalidateTimeout) {
      clearTimeout(inboxInvalidateTimeout);
    }

    inboxInvalidateTimeout = setTimeout(() => {
      void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.inboxRoot() });
    }, 1000);
  };

  hubConnection.on('UserStatusChanged', (userId: string) => {
    void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reader.directoryRoot() });
    if (userId) {
      void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.reader.profile(userId) });
    }
  });

  hubConnection.on('notification.new', () => {
    invalidateDomains(['notifications']);
  });

  hubConnection.on('wallet.balance_changed', () => {
    logger.info('[PresenceRealtimeSync]', 'wallet.balance_changed received. Refetching balance...');
    invalidateDomains(['wallet']);
    void useWalletStore.getState().fetchBalance();
  });

  hubConnection.on('conversation.updated', () => {
    logger.info('[PresenceRealtimeSync]', 'conversation.updated received. Invalidating inbox queries...');
    invalidateInbox();
  });

  hubConnection.on('gamification.quest_completed', () => {
    invalidateDomains(['gamification', 'wallet', 'notifications']);
  });

  hubConnection.on('gamification.achievement_unlocked', () => {
    invalidateDomains(['gamification', 'profile', 'notifications']);
  });

  hubConnection.on('gamification.card_level_up', () => {
    invalidateDomains(['collection', 'notifications']);
  });

  hubConnection.on('gacha.result', () => {
    invalidateDomains([
      'gacha',
      'wallet',
      'inventory',
      'collection',
      'readingSetup',
      'gamification',
      'profile',
      'notifications',
    ]);
    void useWalletStore.getState().fetchBalance();
  });

  hubConnection.on('inventory.changed', () => {
    invalidateDomains([
      'inventory',
      'collection',
      'readingSetup',
      'wallet',
      'gamification',
      'profile',
      'notifications',
    ]);
    void useWalletStore.getState().fetchBalance();
  });

  hubConnection.on('reading.quota_changed', () => {
    invalidateDomains(['readingSetup', 'inventory', 'notifications']);
  });

  hubConnection.on('profile.changed', () => {
    invalidateDomains(['profile', 'gamification']);
  });

  hubConnection.on('title.changed', () => {
    invalidateDomains(['profile', 'gamification', 'notifications']);
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
