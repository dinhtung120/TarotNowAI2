'use client';

import { useEffect, useRef } from 'react';
import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import { logger } from '@/shared/application/gateways/logger';
import { getSignalRHubUrl } from '@/shared/application/gateways/signalRUrl';
import { ensureRealtimeSession } from '@/shared/application/gateways/realtimeSessionGuard';
import { registerPresenceConnectionHandlers } from './usePresenceConnection.registration';
import { useRuntimePolicies } from '@/shared/application/hooks/useRuntimePolicies';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';

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
  const connectionRef = useRef<HubConnection | null>(null);
  const reconnectBlockedUntilRef = useRef(0);
  const queryClient = useQueryClient();

  // Khởi tạo các ref để lưu trữ giá trị cấu hình nhằm giữ cho dependency array của useEffect chính ổn định.
  // Việc tạo array mới ([...]) trong mỗi lần render là nguyên nhân chính gây ra việc reconnect liên tục.
  const reconnectScheduleRef = useRef<number[]>([...RUNTIME_POLICY_FALLBACKS.realtime.reconnectScheduleMs]);
  const configRef = useRef({
    negotiationTimeoutMs: RUNTIME_POLICY_FALLBACKS.realtime.negotiationTimeoutMs as number,
    negotiationCooldownMs: RUNTIME_POLICY_FALLBACKS.realtime.presenceNegotiationCooldownMs as number,
    serverTimeoutMs: RUNTIME_POLICY_FALLBACKS.realtime.serverTimeoutMs as number,
  });

  // Cập nhật các giá trị cấu hình vào ref khi runtimePolicies thay đổi, nhưng không trigger re-render
  // và quan trọng nhất là không làm cho useEffect kết nối SignalR chạy lại.
  useEffect(() => {
    const currentSchedule = realtimePolicy?.reconnectScheduleMs ?? RUNTIME_POLICY_FALLBACKS.realtime.reconnectScheduleMs;
    // So sánh JSON để tránh cập nhật ref nếu giá trị thực tế không đổi (tránh clone array vô ích).
    if (JSON.stringify(reconnectScheduleRef.current) !== JSON.stringify(currentSchedule)) {
      reconnectScheduleRef.current = [...currentSchedule];
    }

    configRef.current = {
      negotiationTimeoutMs: realtimePolicy?.negotiationTimeoutMs ?? RUNTIME_POLICY_FALLBACKS.realtime.negotiationTimeoutMs,
      negotiationCooldownMs:
        realtimePolicy?.presenceNegotiationCooldownMs ?? RUNTIME_POLICY_FALLBACKS.realtime.presenceNegotiationCooldownMs,
      serverTimeoutMs: realtimePolicy?.serverTimeoutMs ?? RUNTIME_POLICY_FALLBACKS.realtime.serverTimeoutMs,
    };
  }, [realtimePolicy]);

  useEffect(() => {
    /*
     * Logic chính để thiết lập kết nối SignalR.
     * Effect này giờ đây chỉ phụ thuộc vào `enabled` và `isAuthenticated`, giúp kết nối
     * luôn ổn định trừ khi trạng thái đăng nhập hoặc yêu cầu bật/tắt realtime thay đổi.
     */
    if (!enabled || !isAuthenticated) {
      const existing = connectionRef.current;
      if (existing && shouldStopConnection(existing)) {
        void existing.stop().catch(() => undefined);
      }

      connectionRef.current = null;
      return;
    }

    if (Date.now() < reconnectBlockedUntilRef.current) {
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
        if (isUnauthorizedNegotiationError(error)) {
          reconnectBlockedUntilRef.current = Date.now() + negotiationCooldownMs;
        } else if (error instanceof Error) {
          reconnectBlockedUntilRef.current = Date.now() + Math.floor(negotiationCooldownMs / 2);
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
  }, [enabled, isAuthenticated, queryClient]);
}
