'use client';

import { useEffect, useRef } from 'react';
import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import { logger } from '@/shared/infrastructure/logging/logger';
import { userStateQueryKeys } from '@/shared/infrastructure/query/userStateQueryKeys';
import { getSignalRHubUrl } from '@/shared/infrastructure/realtime/signalRUrl';
import { ensureRealtimeSession } from '@/shared/infrastructure/realtime/realtimeSessionGuard';

interface UseChatRealtimeSyncOptions {
  enabled?: boolean;
}

interface ConversationUpdatedPayload {
  conversationId?: string;
  type?: string;
}

const UNAUTHORIZED_COOLDOWN_MS = 60_000;
const NEGOTIATION_TIMEOUT_MS = 8_000;
const UNREAD_BADGE_REFRESH_EVENT_TYPES = new Set(['message_created', 'message_read', 'unread_changed']);
let unauthorizedRetryBlockedUntil = 0;

function shouldRefreshUnreadBadge(payload?: ConversationUpdatedPayload): boolean {
 const eventType = payload?.type?.trim().toLowerCase();
 if (!eventType) {
  return true;
 }

 return UNREAD_BADGE_REFRESH_EVENT_TYPES.has(eventType);
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
 return new Error(`Chat negotiation timeout after ${timeoutMs}ms.`);
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

export function useChatRealtimeSync(options: UseChatRealtimeSyncOptions = {}) {
  const enabled = options.enabled ?? true;
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const connectionRef = useRef<HubConnection | null>(null);
  const queryClient = useQueryClient();
  
  const appStartTimeRef = useRef(Date.now());

  useEffect(() => {
    if (!enabled || !isAuthenticated) {
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

    if (Date.now() < unauthorizedRetryBlockedUntil) {
      return;
    }

    let cancelled = false;
    let hubConnection: HubConnection | null = null;
    let inboxInvalidateTimeout: NodeJS.Timeout | null = null;
    let unreadInvalidateTimeout: NodeJS.Timeout | null = null;

    
    const invalidateInboxQueries = () => {
      if (Date.now() - appStartTimeRef.current < 3000) {
        return;
      }

      if (inboxInvalidateTimeout) clearTimeout(inboxInvalidateTimeout);
      inboxInvalidateTimeout = setTimeout(() => {
        void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.inboxRoot() });
      }, 1000); 
    };

    const invalidateUnreadBadge = () => {
      if (Date.now() - appStartTimeRef.current < 3000) {
        return;
      }

      if (unreadInvalidateTimeout) clearTimeout(unreadInvalidateTimeout);
      unreadInvalidateTimeout = setTimeout(() => {
        void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.unreadBadge() });
      }, 1000); 
    };

    const init = async () => {
      const hasValidSession = await ensureRealtimeSession();
      if (!hasValidSession || cancelled) {
        return;
      }

      const signalR = await import('@microsoft/signalr');
      logger.info('[ChatRealtimeSync]', `Connecting to: ${getSignalRHubUrl('/api/v1/chat')}`);
	      hubConnection = new signalR.HubConnectionBuilder()
	        .withUrl(getSignalRHubUrl('/api/v1/chat'), { 
	          withCredentials: true,
	        })
	        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
	        .configureLogging(process.env.NODE_ENV === 'development' ? signalR.LogLevel.Debug : signalR.LogLevel.Warning)
	        .build();

      hubConnection.serverTimeoutInMilliseconds = 120000;

      hubConnection.on('conversation.updated', (payload: ConversationUpdatedPayload) => {
        invalidateInboxQueries();
        // Giữ unread badge đồng bộ cho mọi sự kiện làm đổi trạng thái đọc/chưa đọc.
        if (shouldRefreshUnreadBadge(payload)) {
          invalidateUnreadBadge();
        }
      });
      hubConnection.on('Error', (error: string) => {
        logger.error('[ChatRealtimeSync] hub error', error);
      });
      hubConnection.onreconnected(() => {
        invalidateInboxQueries();
        invalidateUnreadBadge();
      });

      try {
        await startConnectionWithTimeout(hubConnection, NEGOTIATION_TIMEOUT_MS);
        if (cancelled) {
          if (hubConnection.state !== HubConnectionState.Disconnected) {
            await hubConnection.stop().catch(() => undefined);
          }
          return;
        }
        connectionRef.current = hubConnection;
      } catch (error) {
        if (isUnauthorizedNegotiationError(error)) {
          unauthorizedRetryBlockedUntil = Date.now() + UNAUTHORIZED_COOLDOWN_MS;
        } else if (error instanceof Error) {
          unauthorizedRetryBlockedUntil = Date.now() + Math.floor(UNAUTHORIZED_COOLDOWN_MS / 2);
        }
        if (hubConnection && hubConnection.state !== HubConnectionState.Disconnected) {
          await hubConnection.stop().catch(() => undefined);
        }
        logger.error('[ChatRealtimeSync] connect failed', error);
      }
    };

    void init();

    return () => {
      cancelled = true;
      if (inboxInvalidateTimeout) clearTimeout(inboxInvalidateTimeout);
      if (unreadInvalidateTimeout) clearTimeout(unreadInvalidateTimeout);
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
  }, [enabled, isAuthenticated, queryClient]);
}
