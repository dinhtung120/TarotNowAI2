'use client';

import { useEffect, useRef } from 'react';
import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import { logger } from '@/shared/application/gateways/logger';
import { userStateQueryKeys } from '@/shared/application/gateways/userStateQueryKeys';
import { getSignalRHubUrl } from '@/shared/application/gateways/signalRUrl';
import { ensureRealtimeSession } from '@/shared/application/gateways/realtimeSessionGuard';
import { useRuntimePolicies } from '@/shared/application/hooks/useRuntimePolicies';
import { useReconnectWakeup } from '@/shared/application/hooks/useReconnectWakeup';
import {
  hasSameNumberArray,
  isUnauthorizedNegotiationError,
  shouldStopConnection,
  startConnectionWithTimeout,
  stopConnectionSafely,
} from '@/shared/application/hooks/signalRConnectionUtils';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';

interface UseChatRealtimeSyncOptions {
  enabled?: boolean;
}

interface ConversationUpdatedPayload {
  conversationId?: string;
  type?: string;
}

const UNREAD_BADGE_REFRESH_EVENT_TYPES = new Set(['message_created', 'message_read', 'unread_changed']);

function shouldRefreshUnreadBadge(payload?: ConversationUpdatedPayload): boolean {
 const eventType = payload?.type?.trim().toLowerCase();
 if (!eventType) {
  return true;
 }

 return UNREAD_BADGE_REFRESH_EVENT_TYPES.has(eventType);
}

export function useChatRealtimeSync(options: UseChatRealtimeSyncOptions = {}) {
  const enabled = options.enabled ?? true;
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const runtimePoliciesQuery = useRuntimePolicies(enabled && isAuthenticated);
  const realtimePolicy = runtimePoliciesQuery.data?.realtime;
  const connectionRef = useRef<HubConnection | null>(null);
  const unauthorizedRetryBlockedUntilRef = useRef(0);
  const queryClient = useQueryClient();
  const { wakeupVersion, scheduleWakeup, cancelWakeup } = useReconnectWakeup();
  
  const appStartTimeRef = useRef(Date.now());

  // Ổn định cấu hình bằng useRef để tránh phá vỡ kết nối SignalR khi component re-render.
  const reconnectScheduleRef = useRef<number[]>([...RUNTIME_POLICY_FALLBACKS.realtime.reconnectScheduleMs]);
  const configRef = useRef({
    negotiationTimeoutMs: RUNTIME_POLICY_FALLBACKS.realtime.negotiationTimeoutMs as number,
    unauthorizedCooldownMs: RUNTIME_POLICY_FALLBACKS.realtime.chatUnauthorizedCooldownMs as number,
    serverTimeoutMs: RUNTIME_POLICY_FALLBACKS.realtime.serverTimeoutMs as number,
    invalidateDebounceMs: RUNTIME_POLICY_FALLBACKS.realtime.chat.invalidateDebounceMs as number,
    appStartGuardMs: RUNTIME_POLICY_FALLBACKS.realtime.chat.appStartGuardMs as number,
  });

  // Đồng bộ giá trị từ runtimePolicy vào ref mà không gây ảnh hưởng đến useEffect chính.
  useEffect(() => {
    const currentSchedule = realtimePolicy?.reconnectScheduleMs ?? RUNTIME_POLICY_FALLBACKS.realtime.reconnectScheduleMs;
    if (!hasSameNumberArray(reconnectScheduleRef.current, currentSchedule)) {
      reconnectScheduleRef.current = [...currentSchedule];
    }

    configRef.current = {
      negotiationTimeoutMs: realtimePolicy?.negotiationTimeoutMs ?? RUNTIME_POLICY_FALLBACKS.realtime.negotiationTimeoutMs,
      unauthorizedCooldownMs:
        realtimePolicy?.chatUnauthorizedCooldownMs ?? RUNTIME_POLICY_FALLBACKS.realtime.chatUnauthorizedCooldownMs,
      serverTimeoutMs: realtimePolicy?.serverTimeoutMs ?? RUNTIME_POLICY_FALLBACKS.realtime.serverTimeoutMs,
      invalidateDebounceMs:
        realtimePolicy?.chat.invalidateDebounceMs ?? RUNTIME_POLICY_FALLBACKS.realtime.chat.invalidateDebounceMs,
      appStartGuardMs: realtimePolicy?.chat.appStartGuardMs ?? RUNTIME_POLICY_FALLBACKS.realtime.chat.appStartGuardMs,
    };
  }, [realtimePolicy]);

  useEffect(() => {
    /*
     * Quản lý vòng đời kết nối Chat SignalR.
     * Chỉ kết nối lại khi trạng thái enabled hoặc isAuthenticated thay đổi.
     */
    if (!enabled || !isAuthenticated) {
      cancelWakeup();
      const existing = connectionRef.current;
      if (shouldStopConnection(existing)) {
        void stopConnectionSafely(existing);
      }
      connectionRef.current = null;
      return;
    }

    if (Date.now() < unauthorizedRetryBlockedUntilRef.current) {
      scheduleWakeup(unauthorizedRetryBlockedUntilRef.current - Date.now());
      return;
    }

    let cancelled = false;
    let hubConnection: HubConnection | null = null;
    let inboxInvalidateTimeout: NodeJS.Timeout | null = null;
    let unreadInvalidateTimeout: NodeJS.Timeout | null = null;

    // Lấy cấu hình từ ref để sử dụng trong closures của effect.
    const { appStartGuardMs, invalidateDebounceMs, serverTimeoutMs, negotiationTimeoutMs, unauthorizedCooldownMs } = configRef.current;
    const schedule = reconnectScheduleRef.current;
    
    const invalidateInboxQueries = () => {
      if (Date.now() - appStartTimeRef.current < appStartGuardMs) {
        return;
      }

      if (inboxInvalidateTimeout) clearTimeout(inboxInvalidateTimeout);
      inboxInvalidateTimeout = setTimeout(() => {
        void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.inboxRoot() });
      }, invalidateDebounceMs);
    };

    const invalidateUnreadBadge = () => {
      if (Date.now() - appStartTimeRef.current < appStartGuardMs) {
        return;
      }

      if (unreadInvalidateTimeout) clearTimeout(unreadInvalidateTimeout);
      unreadInvalidateTimeout = setTimeout(() => {
        void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.unreadBadge() });
      }, invalidateDebounceMs);
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
	        .withAutomaticReconnect(schedule)
	        .configureLogging(process.env.NODE_ENV === 'development' ? signalR.LogLevel.Debug : signalR.LogLevel.Warning)
	        .build();

      hubConnection.serverTimeoutInMilliseconds = serverTimeoutMs;

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
        await startConnectionWithTimeout(hubConnection, negotiationTimeoutMs, 'Chat');
        if (cancelled) {
          if (hubConnection.state !== HubConnectionState.Disconnected) {
            await stopConnectionSafely(hubConnection);
          }
          return;
        }
        connectionRef.current = hubConnection;
        unauthorizedRetryBlockedUntilRef.current = 0;
        cancelWakeup();
      } catch (error) {
        if (isUnauthorizedNegotiationError(error)) {
          unauthorizedRetryBlockedUntilRef.current = Date.now() + unauthorizedCooldownMs;
        } else if (error instanceof Error) {
          unauthorizedRetryBlockedUntilRef.current = Date.now() + Math.floor(unauthorizedCooldownMs / 2);
        }
        scheduleWakeup(unauthorizedRetryBlockedUntilRef.current - Date.now());
        if (hubConnection && hubConnection.state !== HubConnectionState.Disconnected) {
          await stopConnectionSafely(hubConnection);
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
        void stopConnectionSafely(hubConnection);
      }
      connectionRef.current = null;
    };
  }, [cancelWakeup, enabled, isAuthenticated, queryClient, scheduleWakeup, wakeupVersion]);
}
