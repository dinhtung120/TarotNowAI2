'use client';

import { useEffect, useRef } from 'react';
import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { getSignalRHubUrl } from '@/shared/infrastructure/realtime/signalRUrl';
import { ensureRealtimeSession } from '@/shared/infrastructure/realtime/realtimeSessionGuard';
import { registerPresenceConnectionHandlers } from './usePresenceConnection.registration';
import { useRuntimePolicies } from '@/shared/application/hooks/useRuntimePolicies';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';

let reconnectBlockedUntil = 0;

function shouldStopConnection(connection: HubConnection | null) {
  return !!connection && (
    connection.state === HubConnectionState.Connected
    || connection.state === HubConnectionState.Reconnecting
    || connection.state === HubConnectionState.Disconnecting
  );
}

function isUnauthorizedNegotiationError(error: unknown): boolean {
 if (!error) {
  return false;
 }

 const text = typeof error === 'string'
  ? error
  : error instanceof Error
   ? error.message
   : JSON.stringify(error);
 return text.includes('401') || /unauthorized/i.test(text);
}

function createTimeoutError(timeoutMs: number): Error {
 return new Error(`Presence negotiation timeout after ${timeoutMs}ms.`);
}

async function startConnectionWithTimeout(
 connection: HubConnection,
 timeoutMs: number,
): Promise<void> {
 let timeoutId: NodeJS.Timeout | null = null;
 try {
  await Promise.race([
   connection.start(),
   new Promise<never>((_, reject) => {
    timeoutId = setTimeout(() => {
     reject(createTimeoutError(timeoutMs));
    }, timeoutMs);
   }),
  ]);
 } finally {
  if (timeoutId) {
   clearTimeout(timeoutId);
  }
 }
}

interface UsePresenceConnectionOptions {
  enabled?: boolean;
}

export function usePresenceConnection(options: UsePresenceConnectionOptions = {}) {
  const enabled = options.enabled ?? true;
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const runtimePoliciesQuery = useRuntimePolicies();
  const realtimePolicy = runtimePoliciesQuery.data?.realtime;
  const reconnectSchedule = [...(realtimePolicy?.reconnectScheduleMs ?? RUNTIME_POLICY_FALLBACKS.realtime.reconnectScheduleMs)];
  const negotiationTimeoutMs = realtimePolicy?.negotiationTimeoutMs ?? RUNTIME_POLICY_FALLBACKS.realtime.negotiationTimeoutMs;
  const negotiationCooldownMs =
    realtimePolicy?.presenceNegotiationCooldownMs ?? RUNTIME_POLICY_FALLBACKS.realtime.presenceNegotiationCooldownMs;
  const serverTimeoutMs = realtimePolicy?.serverTimeoutMs ?? RUNTIME_POLICY_FALLBACKS.realtime.serverTimeoutMs;
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

    if (Date.now() < reconnectBlockedUntil) {
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

      hubConnection.serverTimeoutInMilliseconds = serverTimeoutMs;
      const registration = registerPresenceConnectionHandlers(hubConnection, queryClient);
      disposeRegistration = registration.dispose;

      try {
        await startConnectionWithTimeout(hubConnection, negotiationTimeoutMs);
        if (cancelled) {
          if (hubConnection.state !== HubConnectionState.Disconnected) {
            await hubConnection.stop().catch(() => undefined);
          }

          return;
        }

        connectionRef.current = hubConnection;
        heartbeatInterval = registration.startHeartbeat();
      } catch (error) {
        if (isUnauthorizedNegotiationError(error) || error instanceof Error) {
          reconnectBlockedUntil = Date.now() + negotiationCooldownMs;
        }

        if (hubConnection && hubConnection.state !== HubConnectionState.Disconnected) {
          await hubConnection.stop().catch(() => undefined);
        }
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
  }, [enabled, isAuthenticated, negotiationCooldownMs, negotiationTimeoutMs, queryClient, reconnectSchedule, serverTimeoutMs]);
}
