'use client';

import { useEffect, useRef } from 'react';
import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { getSignalRHubUrl } from '@/shared/infrastructure/realtime/signalRUrl';
import { ensureRealtimeSession } from '@/shared/infrastructure/realtime/realtimeSessionGuard';
import { registerPresenceConnectionHandlers } from './usePresenceConnection.registration';

const reconnectSchedule = [0, 2000, 5000, 10000, 30000];

function shouldStopConnection(connection: HubConnection | null) {
  return !!connection && (
    connection.state === HubConnectionState.Connected
    || connection.state === HubConnectionState.Reconnecting
    || connection.state === HubConnectionState.Disconnecting
  );
}

interface UsePresenceConnectionOptions {
  enabled?: boolean;
}

export function usePresenceConnection(options: UsePresenceConnectionOptions = {}) {
  const enabled = options.enabled ?? true;
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const connectionRef = useRef<HubConnection | null>(null);
  const queryClient = useQueryClient();

  useEffect(() => {
    if (!enabled || !isAuthenticated) {
      const existing = connectionRef.current;
      if (existing && shouldStopConnection(existing)) {
        void existing.stop().catch(() => undefined);
      }

      connectionRef.current = null;
      return;
    }

    let cancelled = false;
    let hubConnection: HubConnection | null = null;
    let heartbeatInterval: NodeJS.Timeout | null = null;
    let disposeRegistration: () => void = () => {};

    const init = async () => {
      const hasValidSession = await ensureRealtimeSession();
      if (!hasValidSession || cancelled) {
        return;
      }

      const signalR = await import('@microsoft/signalr');
      const hubUrl = getSignalRHubUrl('/api/v1/presence');
      logger.info('[PresenceRealtimeSync]', `Connecting to: ${hubUrl}`);

	      hubConnection = new signalR.HubConnectionBuilder()
	        .withUrl(hubUrl, { withCredentials: true })
	        .withAutomaticReconnect(reconnectSchedule)
	        .configureLogging(process.env.NODE_ENV === 'development' ? signalR.LogLevel.Debug : signalR.LogLevel.Warning)
	        .build();

      hubConnection.serverTimeoutInMilliseconds = 120000;
      const registration = registerPresenceConnectionHandlers(hubConnection, queryClient);
      disposeRegistration = registration.dispose;

      try {
        await hubConnection.start();
        if (cancelled) {
          if (hubConnection.state !== HubConnectionState.Disconnected) {
            await hubConnection.stop().catch(() => undefined);
          }

          return;
        }

        connectionRef.current = hubConnection;
        heartbeatInterval = registration.startHeartbeat();
      } catch (error) {
        logger.error('[PresenceRealtimeSync] connect failed', error);
      }
    };

    void init();

    return () => {
      cancelled = true;
      if (heartbeatInterval) clearInterval(heartbeatInterval);
      disposeRegistration();

      if (hubConnection && shouldStopConnection(hubConnection)) {
        void hubConnection.stop().catch(() => undefined);
      }

      connectionRef.current = null;
    };
  }, [enabled, isAuthenticated, queryClient]);
}
