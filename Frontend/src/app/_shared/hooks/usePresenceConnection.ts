'use client';

import { useEffect, useRef } from 'react';
import type { HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/features/auth/session/authStore';
import { logger } from '@/shared/gateways/logger';
import { getSignalRHubUrl } from '@/features/chat/shared/gateways/signalRUrl';
import { ensureRealtimeSession } from '@/features/chat/shared/gateways/realtimeSessionGuard';
import { useReconnectWakeup } from '@/features/chat/shared/hooks/useReconnectWakeup';
import { registerPresenceConnectionHandlers } from './usePresenceConnection.registration';
import {
  isUnauthorizedNegotiationError,
  shouldStopConnection,
  startConnectionWithTimeout,
  stopConnectionSafely,
} from '@/features/chat/shared/hooks/signalRConnectionUtils';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';

interface UsePresenceConnectionOptions {
  enabled?: boolean;
}

export function usePresenceConnection(options: UsePresenceConnectionOptions = {}) {
  const enabled = options.enabled ?? true;
  const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
  const connectionRef = useRef<HubConnection | null>(null);
  const reconnectBlockedUntilRef = useRef(0);
  const queryClient = useQueryClient();
  const { wakeupVersion, scheduleWakeup, cancelWakeup } = useReconnectWakeup();

  // Khởi tạo các ref để lưu trữ giá trị cấu hình nhằm giữ cho dependency array của useEffect chính ổn định.
  // Việc tạo array mới ([...]) trong mỗi lần render là nguyên nhân chính gây ra việc reconnect liên tục.
  const reconnectScheduleRef = useRef<number[]>([...RUNTIME_POLICY_FALLBACKS.realtime.reconnectScheduleMs]);
  const configRef = useRef({
    negotiationTimeoutMs: RUNTIME_POLICY_FALLBACKS.realtime.negotiationTimeoutMs as number,
    negotiationCooldownMs: RUNTIME_POLICY_FALLBACKS.realtime.presenceNegotiationCooldownMs as number,
    serverTimeoutMs: RUNTIME_POLICY_FALLBACKS.realtime.serverTimeoutMs as number,
  });

  useEffect(() => {
    /*
     * Logic chính để thiết lập kết nối SignalR.
     * Effect này giờ đây chỉ phụ thuộc vào `enabled` và `isAuthenticated`, giúp kết nối
     * luôn ổn định trừ khi trạng thái đăng nhập hoặc yêu cầu bật/tắt realtime thay đổi.
     */
    if (!enabled || !isAuthenticated) {
      cancelWakeup();
      const existing = connectionRef.current;
      if (existing && shouldStopConnection(existing)) {
        void stopConnectionSafely(existing);
      }

      connectionRef.current = null;
      return;
    }

    if (Date.now() < reconnectBlockedUntilRef.current) {
      scheduleWakeup(reconnectBlockedUntilRef.current - Date.now());
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

      // Sử dụng giá trị từ ref để đảm bảo tính ổn định của effect.
      const schedule = reconnectScheduleRef.current;
      const { negotiationTimeoutMs, serverTimeoutMs, negotiationCooldownMs } = configRef.current;

      hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(hubUrl, { withCredentials: true })
        .withAutomaticReconnect(schedule)
        .configureLogging(process.env.NODE_ENV === 'development' ? signalR.LogLevel.Debug : signalR.LogLevel.Warning)
        .build();

      hubConnection.serverTimeoutInMilliseconds = serverTimeoutMs;
      const registration = registerPresenceConnectionHandlers(hubConnection, queryClient);
      disposeRegistration = registration.dispose;

      try {
        await startConnectionWithTimeout(hubConnection, negotiationTimeoutMs, 'Presence');
        if (cancelled) {
          if (shouldStopConnection(hubConnection)) {
            await stopConnectionSafely(hubConnection);
          }

          return;
        }

        connectionRef.current = hubConnection;
        reconnectBlockedUntilRef.current = 0;
        cancelWakeup();
        registration.syncStatusObservers();
        heartbeatInterval = registration.startHeartbeat();
      } catch (error) {
        if (isUnauthorizedNegotiationError(error)) {
          reconnectBlockedUntilRef.current = Date.now() + negotiationCooldownMs;
        } else if (error instanceof Error) {
          reconnectBlockedUntilRef.current = Date.now() + Math.floor(negotiationCooldownMs / 2);
        }

        scheduleWakeup(reconnectBlockedUntilRef.current - Date.now());

        if (shouldStopConnection(hubConnection)) {
          await stopConnectionSafely(hubConnection);
        }
        logger.error('[PresenceRealtimeSync] connect failed', error);
      }
    };

    const connectTimer = window.setTimeout(() => {
      void init();
    }, 3_000);

    return () => {
      cancelled = true;
      window.clearTimeout(connectTimer);
      if (heartbeatInterval) clearInterval(heartbeatInterval);
      disposeRegistration();

      if (hubConnection && shouldStopConnection(hubConnection)) {
        void stopConnectionSafely(hubConnection);
      }

      connectionRef.current = null;
    };
  }, [cancelWakeup, enabled, isAuthenticated, queryClient, scheduleWakeup, wakeupVersion]);
}
