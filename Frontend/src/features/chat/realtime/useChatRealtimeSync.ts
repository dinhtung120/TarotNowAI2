'use client';

import { useEffect, useRef } from 'react';
import { HubConnectionState, type HubConnection } from '@microsoft/signalr';
import { useQueryClient, type QueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/features/auth/session/authStore';
import type {
  ChatMessageDto,
  ListConversationsResult,
} from '@/features/chat/shared/actions';
import { logger } from '@/shared/gateways/logger';
import { userStateQueryKeys } from '@/shared/gateways/userStateQueryKeys';
import { getSignalRHubUrl } from '@/features/chat/shared/gateways/signalRUrl';
import { ensureRealtimeSession } from '@/features/chat/shared/gateways/realtimeSessionGuard';
import { useRuntimePolicies } from '@/shared/hooks/useRuntimePolicies';
import { useReconnectWakeup } from '@/features/chat/shared/hooks/useReconnectWakeup';
import {
  hasSameNumberArray,
  isUnauthorizedNegotiationError,
  shouldStopConnection,
  startConnectionWithTimeout,
  stopConnectionSafely,
} from '@/features/chat/shared/hooks/signalRConnectionUtils';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';

interface UseChatRealtimeSyncOptions {
  enabled?: boolean;
}

interface ConversationUpdatedPayload {
  conversationId?: string;
  type?: string;
  updatedAt?: string;
  status?: string;
}

interface MessageCreatedFastPayload extends ChatMessageDto {
  conversationId: string;
}

interface ConversationUnreadDeltaPayload {
  conversationId?: string;
  userId?: string;
  readerId?: string;
  recipientUserId?: string;
  clearForUserId?: string;
  unreadDelta?: number;
  lastMessagePreview?: string;
  lastMessageType?: string;
  updatedAt?: string;
}

const UNREAD_BADGE_REFRESH_EVENT_TYPES = new Set(['message_created', 'message_read', 'unread_changed']);
const CRITICAL_CHAT_CONVERSATION_EVENT_TYPES = new Set([
 'complete_requested',
 'complete_responded',
 'completed',
 'accepted',
 'rejected',
]);

function normalizeConversationEventType(value?: string) {
 return value?.trim().toLowerCase() ?? '';
}

function isCriticalConversationEventType(eventType: string) {
 return CRITICAL_CHAT_CONVERSATION_EVENT_TYPES.has(eventType);
}

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
  const currentUserId = useAuthStore((state) => state.user?.id ?? '');
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
    
    const invalidateInboxQueries = (force = false) => {
      if (!force && Date.now() - appStartTimeRef.current < appStartGuardMs) {
        return;
      }

      if (inboxInvalidateTimeout) clearTimeout(inboxInvalidateTimeout);
      inboxInvalidateTimeout = setTimeout(() => {
        void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.inboxRoot() });
      }, force ? 0 : invalidateDebounceMs);
    };

    const invalidateUnreadBadge = (force = false) => {
      if (!force && Date.now() - appStartTimeRef.current < appStartGuardMs) {
        return;
      }

      if (unreadInvalidateTimeout) clearTimeout(unreadInvalidateTimeout);
      unreadInvalidateTimeout = setTimeout(() => {
        void queryClient.invalidateQueries({ queryKey: userStateQueryKeys.chat.unreadBadge() });
      }, force ? 0 : invalidateDebounceMs);
    };

    const patchInboxWithMessage = (message: MessageCreatedFastPayload): boolean => {
      let patched = false;

      queryClient.setQueriesData<ListConversationsResult>(
        { queryKey: userStateQueryKeys.chat.inboxRoot() },
        (previous) => {
          if (!previous?.conversations?.length || !currentUserId) {
            return previous;
          }

          const index = previous.conversations.findIndex(
            (conversation) => conversation.id === message.conversationId,
          );
          if (index < 0) {
            return previous;
          }

          const conversation = previous.conversations[index];
          const isUserRole = conversation.userId === currentUserId;
          const isSenderCurrentUser = message.senderId === currentUserId;
          const unreadField = isUserRole ? 'unreadCountUser' : 'unreadCountReader';
          const updatedUnread = isSenderCurrentUser
            ? conversation[unreadField]
            : conversation[unreadField] + 1;

          const updatedConversation = {
            ...conversation,
            lastMessageAt: message.createdAt,
            updatedAt: message.createdAt,
            lastMessagePreview: resolveRealtimePreview(message),
            [unreadField]: updatedUnread,
          };

          const conversations = [...previous.conversations];
          conversations.splice(index, 1);
          conversations.unshift(updatedConversation);
          patched = true;

          return {
            ...previous,
            conversations,
          };
        },
      );

      return patched;
    };

    const patchInboxWithUnreadDelta = (payload: ConversationUnreadDeltaPayload): boolean => {
      if (!payload.conversationId || !currentUserId) {
        return false;
      }

      let patched = false;
      queryClient.setQueriesData<ListConversationsResult>(
        { queryKey: userStateQueryKeys.chat.inboxRoot() },
        (previous) => {
          if (!previous?.conversations?.length) {
            return previous;
          }

          const index = previous.conversations.findIndex(
            (conversation) => conversation.id === payload.conversationId,
          );
          if (index < 0) {
            return previous;
          }

          const conversation = previous.conversations[index];
          const isUserRole = conversation.userId === currentUserId;
          const unreadField = isUserRole ? 'unreadCountUser' : 'unreadCountReader';
          let nextUnread = conversation[unreadField];

          if (payload.unreadDelta === 0) {
            if (payload.clearForUserId !== currentUserId) {
              return previous;
            }
            nextUnread = 0;
          } else if (payload.recipientUserId && payload.recipientUserId === currentUserId) {
            nextUnread = Math.max(0, nextUnread + (payload.unreadDelta ?? 0));
          }

          const updatedConversation = {
            ...conversation,
            [unreadField]: nextUnread,
            updatedAt: payload.updatedAt ?? conversation.updatedAt,
            lastMessagePreview: payload.lastMessagePreview ?? conversation.lastMessagePreview,
          };
          const conversations = [...previous.conversations];
          const shouldPromote = payload.unreadDelta !== 0 || Boolean(payload.lastMessagePreview);
          if (shouldPromote) {
            conversations.splice(index, 1);
            conversations.unshift(updatedConversation);
          } else {
            conversations[index] = updatedConversation;
          }
          patched = true;

          return {
            ...previous,
            conversations,
          };
        },
      );

      return patched;
    };

    const patchUnreadBadge = (delta: number) => {
      queryClient.setQueryData<number>(userStateQueryKeys.chat.unreadBadge(), (previous) => {
        const base = previous ?? 0;
        return Math.max(0, base + delta);
      });
    };

    const init = async () => {
      const hasValidSession = await ensureRealtimeSession();
      if (!hasValidSession || cancelled) {
        return;
      }

      const signalR = await import('@microsoft/signalr');
      logger.info('[ChatRealtimeSync]', `Connecting to: ${getSignalRHubUrl('/api/v1/presence')}`);
	      hubConnection = new signalR.HubConnectionBuilder()
	        .withUrl(getSignalRHubUrl('/api/v1/presence'), { 
	          withCredentials: true,
	        })
	        .withAutomaticReconnect(schedule)
	        .configureLogging(process.env.NODE_ENV === 'development' ? signalR.LogLevel.Debug : signalR.LogLevel.Warning)
	        .build();

      hubConnection.serverTimeoutInMilliseconds = serverTimeoutMs;

      hubConnection.on('conversation.updated', (payload: ConversationUpdatedPayload) => {
        const eventType = normalizeConversationEventType(payload.type);
        const isCriticalEvent = isCriticalConversationEventType(eventType);
        invalidateInboxQueries(isCriticalEvent);
        // Giữ unread badge đồng bộ cho mọi sự kiện làm đổi trạng thái đọc/chưa đọc.
        if (isCriticalEvent || shouldRefreshUnreadBadge(payload)) {
          invalidateUnreadBadge(isCriticalEvent);
        }
      });
      const applyIncomingMessage = (message: MessageCreatedFastPayload) => {
        const patched = patchInboxWithMessage(message);
        if (!patched) {
          invalidateInboxQueries();
        }

        if (message.senderId !== currentUserId) {
          patchUnreadBadge(1);
        }
      };
      hubConnection.on('message.created.fast', applyIncomingMessage);
      hubConnection.on('message.created', applyIncomingMessage);
      hubConnection.on('chat.unread.delta', (payload: ConversationUnreadDeltaPayload) => {
        const patched = patchInboxWithUnreadDelta(payload);
        if (!patched) {
          invalidateInboxQueries();
        }

        if (payload.unreadDelta === 0) {
          invalidateUnreadBadge();
          return;
        }

        if (payload.recipientUserId && payload.recipientUserId === currentUserId) {
          patchUnreadBadge(payload.unreadDelta ?? 0);
        }
      });
      hubConnection.on('conversation.updated.delta', (payload: ConversationUpdatedPayload) => {
        const eventType = normalizeConversationEventType(payload.type);
        const isCriticalEvent = isCriticalConversationEventType(eventType);
        const patched = patchInboxConversationDelta(queryClient, payload);
        if (!patched) {
          invalidateInboxQueries(isCriticalEvent);
        }
        if (isCriticalEvent) {
          invalidateUnreadBadge(true);
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
  }, [cancelWakeup, currentUserId, enabled, isAuthenticated, queryClient, scheduleWakeup, wakeupVersion]);
}

function resolveRealtimePreview(message: MessageCreatedFastPayload): string {
  if (message.type === 'image') return '[image]';
  if (message.type === 'voice') return '[voice]';
  if (message.type === 'payment_offer') return 'Đề xuất cộng tiền';
  if (message.type === 'payment_accept') return 'Đã chấp nhận đề xuất cộng tiền';
  if (message.type === 'payment_reject') return 'Đã từ chối đề xuất cộng tiền';
  return message.content?.trim() || 'Tin nhắn mới';
}

function patchInboxConversationDelta(
  queryClient: QueryClient,
  payload: ConversationUpdatedPayload,
): boolean {
  if (!payload.conversationId) {
    return false;
  }

  let patched = false;
  queryClient.setQueriesData<ListConversationsResult>(
    { queryKey: userStateQueryKeys.chat.inboxRoot() },
    (previous) => {
      if (!previous?.conversations?.length) {
        return previous;
      }

      const index = previous.conversations.findIndex(
        (conversation) => conversation.id === payload.conversationId,
      );
      if (index < 0) {
        return previous;
      }

      const target = previous.conversations[index];
      const next = [...previous.conversations];
      next[index] = {
        ...target,
        status: (payload.status as typeof target.status | undefined) ?? target.status,
        updatedAt: payload.updatedAt ?? target.updatedAt,
      };
      patched = true;
      return {
        ...previous,
        conversations: next,
      };
    },
  );

  return patched;
}
