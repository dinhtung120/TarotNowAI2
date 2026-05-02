'use client';

import { useEffect, useRef, type Dispatch, type MutableRefObject, type SetStateAction } from 'react';
import {
 HubConnectionState,
 type HubConnection,
 type LogLevel,
} from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import type { ChatMessageDto, ConversationDto } from '@/features/chat/application/actions';
import { isSameParticipantId } from '@/features/chat/domain/participantId';
import { listMessages } from '@/features/chat/application/actions';
import { ensureRealtimeSession } from '@/shared/application/gateways/realtimeSessionGuard';
import { logger } from '@/shared/application/gateways/logger';
import { useReconnectWakeup } from '@/shared/application/hooks/useReconnectWakeup';
import { useRuntimePolicies } from '@/shared/application/hooks/useRuntimePolicies';
import { RUNTIME_POLICY_FALLBACKS } from '@/shared/config/runtimePolicyFallbacks';
import { createSignalRLogger, getCachedConversation } from './utils';

interface UseChatSignalRLifecycleOptions {
 conversationId?: string | null;
 currentUserId: string;
 queryClient: QueryClient;
 connectionRef: MutableRefObject<HubConnection | null>;
 typingTimeoutRef: MutableRefObject<ReturnType<typeof setTimeout> | null>;
 lastInitialLoadTimeRef: MutableRefObject<number>;
 loadInitialRef: MutableRefObject<(silent?: boolean) => Promise<void>>;
 markReadRef: MutableRefObject<() => Promise<void>>;
 setConnected: Dispatch<SetStateAction<boolean>>;
 setLoading: Dispatch<SetStateAction<boolean>>;
 setTypingUserId: Dispatch<SetStateAction<string | null>>;
 setMessages: Dispatch<SetStateAction<ChatMessageDto[]>>;
 setConversation: Dispatch<SetStateAction<ConversationDto | null>>;
 resetForConversation: (cachedConversation: ConversationDto | null) => void;
 appendMessage: (message: ChatMessageDto) => void;
}

interface ConversationDeltaPayload {
 conversationId: string;
 status?: ConversationDto['status'];
 type?: string;
 updatedAt?: string;
 confirm?: ConversationDto['confirm'];
 canSubmitReview?: boolean;
 hasSubmittedReview?: boolean;
 reviewedAt?: string | null;
}

const NON_CRITICAL_CONVERSATION_EVENT_TYPES = new Set(['message_created', 'member_joined', 'message_read']);
const CRITICAL_CONVERSATION_EVENT_TYPES = new Set([
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
 return CRITICAL_CONVERSATION_EVENT_TYPES.has(eventType);
}

async function createHubConnection(reconnectSchedule: number[]) {
 const signalR = await import('@microsoft/signalr');
 const { getSignalRHubUrl } = await import('@/shared/application/gateways/signalRUrl');
 const customLogger = createSignalRLogger({
  error: signalR.LogLevel.Error as LogLevel,
  critical: signalR.LogLevel.Critical as LogLevel,
  warning: signalR.LogLevel.Warning as LogLevel,
  information: signalR.LogLevel.Information as LogLevel,
 });
 return new signalR.HubConnectionBuilder()
  .withUrl(getSignalRHubUrl('/api/v1/chat'), {
   withCredentials: true,
  })
  .withAutomaticReconnect(reconnectSchedule)
  .configureLogging(customLogger)
  .build();
}

export function useChatSignalRLifecycle(options: UseChatSignalRLifecycleOptions) {
 const {
  conversationId, currentUserId, queryClient, connectionRef, typingTimeoutRef,
 lastInitialLoadTimeRef, loadInitialRef, markReadRef, setConnected, setLoading,
 setTypingUserId, setMessages, setConversation, resetForConversation, appendMessage,
 } = options;
 const runtimePoliciesQuery = useRuntimePolicies();
 const realtimePolicy = runtimePoliciesQuery.data?.realtime;
 const { wakeupVersion, scheduleWakeup, cancelWakeup } = useReconnectWakeup();
  // Sử dụng ref để giữ cấu hình ổn định, tránh việc array [0, 2000, ...] tạo mới mỗi render phá vỡ effect.
  const reconnectScheduleRef = useRef<number[]>([...RUNTIME_POLICY_FALLBACKS.realtime.reconnectScheduleMs]);
  const configRef = useRef({
    serverTimeoutMs: RUNTIME_POLICY_FALLBACKS.realtime.serverTimeoutMs as number,
    typingClearMs: RUNTIME_POLICY_FALLBACKS.realtime.chat.typingClearMs as number,
    invalidateDebounceMs: RUNTIME_POLICY_FALLBACKS.realtime.chat.invalidateDebounceMs as number,
    initialLoadGuardMs: RUNTIME_POLICY_FALLBACKS.realtime.chat.initialLoadGuardMs as number,
  });

  // Đồng bộ cấu hình từ runtimePolicy vào ref.
  useEffect(() => {
    const currentSchedule = realtimePolicy?.reconnectScheduleMs ?? RUNTIME_POLICY_FALLBACKS.realtime.reconnectScheduleMs;
    if (JSON.stringify(reconnectScheduleRef.current) !== JSON.stringify(currentSchedule)) {
      reconnectScheduleRef.current = [...currentSchedule];
    }

    configRef.current = {
      serverTimeoutMs: realtimePolicy?.serverTimeoutMs ?? RUNTIME_POLICY_FALLBACKS.realtime.serverTimeoutMs,
      typingClearMs: realtimePolicy?.chat.typingClearMs ?? RUNTIME_POLICY_FALLBACKS.realtime.chat.typingClearMs,
      invalidateDebounceMs:
        realtimePolicy?.chat.invalidateDebounceMs ?? RUNTIME_POLICY_FALLBACKS.realtime.chat.invalidateDebounceMs,
      initialLoadGuardMs:
        realtimePolicy?.chat.initialLoadGuardMs ?? RUNTIME_POLICY_FALLBACKS.realtime.chat.initialLoadGuardMs,
    };
 }, [realtimePolicy]);

 const initializingRef = useRef(false);
 const initialConnectAttemptRef = useRef(0);
 const lastConversationIdRef = useRef<string | null>(null);

 useEffect(() => {
  if (lastConversationIdRef.current !== (conversationId ?? null)) {
   lastConversationIdRef.current = conversationId ?? null;
   initialConnectAttemptRef.current = 0;
   cancelWakeup();
  }

  // Chỉ khởi tạo khi có đủ thông tin và không đang trong quá trình khởi tạo khác cho cùng conversation
  if (!conversationId || initializingRef.current) {
   if (!conversationId) {
    initialConnectAttemptRef.current = 0;
    cancelWakeup();
    resetForConversation(null);
    setConnected(false);
    setLoading(false);
   }
   return;
  }

  let cancelled = false;
  let hubConnection: HubConnection | null = null;
  let conversationInvalidateTimeout: ReturnType<typeof setTimeout> | null = null;
  initializingRef.current = true;

  if (initialConnectAttemptRef.current === 0) {
   resetForConversation(getCachedConversation(queryClient, conversationId));
   setTypingUserId(null);
  }

  // Lấy cấu hình ổn định từ ref.
  const { serverTimeoutMs, typingClearMs, invalidateDebounceMs, initialLoadGuardMs } = configRef.current;
  const schedule = reconnectScheduleRef.current;

  const onVisibilityChange = () => {
   if (document.visibilityState === 'visible' && conversationId) {
    void markReadRef.current();
   }
  };

  const init = async () => {
   try {
    const hasValidSession = await ensureRealtimeSession();
    if (!hasValidSession || cancelled) {
      return;
    }

     hubConnection = await createHubConnection(schedule);
    // Cấu hình timeout dài hơn để tránh bị ngắt kết nối do mạng chập chờn
    hubConnection.serverTimeoutInMilliseconds = serverTimeoutMs;
    
    // Đăng ký các sự kiện nhận tin nhắn và trạng thái realtime
    const handleIncomingMessage = (message: ChatMessageDto) => {
     /*
      * Step: Xử lý khi có tin nhắn mới đến.
      * Logic: 
      * 1. Thêm tin nhắn vào danh sách hiện tại để hiển thị ngay lập tức (Realtime UI).
      * 2. Nếu tab trình duyệt đang hiển thị (Visible/Focused), tự động đánh dấu đã đọc (Mark Read) 
      *    để Server reset bộ đếm tin nhắn chưa đọc và các thiết bị khác đồng bộ theo.
      */
     if (message.conversationId === conversationId) {
      appendMessage(message);
      
      // Nếu người dùng đang nhìn màn hình chat và message đến từ đối phương, đánh dấu đã đọc ngay.
      if (document.visibilityState === 'visible' && !isSameParticipantId(message.senderId, currentUserId)) {
       void markReadRef.current();
      }
     }
    };
    hubConnection.on('message.created', handleIncomingMessage);
    hubConnection.on('message.created.fast', handleIncomingMessage);
    
    hubConnection.on('message.read', (payload: { userId: string; conversationId: string }) => {
     if (payload.conversationId !== conversationId || isSameParticipantId(payload.userId, currentUserId)) return;
     setMessages((prev) =>
      prev.map((item) =>
       isSameParticipantId(item.senderId, currentUserId) ? { ...item, isRead: true } : item
      )
     );
    });

    hubConnection.on('typing.started', (payload: { userId: string; conversationId: string }) => {
     if (payload.conversationId !== conversationId || isSameParticipantId(payload.userId, currentUserId)) return;
     setTypingUserId(payload.userId);
     if (typingTimeoutRef.current) clearTimeout(typingTimeoutRef.current);
     typingTimeoutRef.current = setTimeout(() => setTypingUserId(null), typingClearMs);
    });

    hubConnection.on('typing.stopped', (payload: { userId: string; conversationId: string }) => {
     if (payload.conversationId !== conversationId || isSameParticipantId(payload.userId, currentUserId)) return;
     setTypingUserId(null);
    });

    const reloadConversationSnapshot = () => {
     void loadInitialRef.current(true);
    };

    hubConnection.on('conversation.updated', (payload: { conversationId: string; type?: string }) => {
     if (payload.conversationId !== conversationId) return;
     const eventType = normalizeConversationEventType(payload.type);
     if (NON_CRITICAL_CONVERSATION_EVENT_TYPES.has(eventType)) return;
     const isCriticalEvent = isCriticalConversationEventType(eventType);
     // Với event thường, giữ guard để tránh refetch dồn dập lúc mới mount.
     if (!isCriticalEvent && Date.now() - lastInitialLoadTimeRef.current < initialLoadGuardMs) return;
     if (conversationInvalidateTimeout) {
      clearTimeout(conversationInvalidateTimeout);
     }
     if (isCriticalEvent) {
      reloadConversationSnapshot();
      return;
     }

     conversationInvalidateTimeout = setTimeout(() => {
      void listMessages(conversationId, { limit: 1 }).then((res) => {
       if (res.success && res.data?.conversation) setConversation(res.data.conversation);
      });
     }, invalidateDebounceMs);
    });

    hubConnection.on('conversation.updated.delta', (payload: ConversationDeltaPayload) => {
     if (payload.conversationId !== conversationId) return;
     const eventType = normalizeConversationEventType(payload.type);
      setConversation((prev) => {
       if (!prev) return prev;

       return {
        ...prev,
        status: payload.status ?? prev.status,
        updatedAt: payload.updatedAt ?? prev.updatedAt,
        confirm: payload.confirm ?? prev.confirm,
        canSubmitReview: payload.canSubmitReview ?? prev.canSubmitReview,
        hasSubmittedReview: payload.hasSubmittedReview ?? prev.hasSubmittedReview,
        reviewedAt: payload.reviewedAt ?? prev.reviewedAt,
       };
      });
     if (isCriticalConversationEventType(eventType)) {
      reloadConversationSnapshot();
     }
    });

    hubConnection.on('message.read.delta', (payload: { userId: string; conversationId: string }) => {
     if (payload.conversationId !== conversationId || isSameParticipantId(payload.userId, currentUserId)) return;
     setMessages((prev) =>
      prev.map((item) =>
       isSameParticipantId(item.senderId, currentUserId) ? { ...item, isRead: true } : item
      )
     );
    });

    /*
     * Step: Xử lý sự kiện thay đổi trạng thái hiển thị của tab trình duyệt.
     * Logic: Khi người dùng chuyển sang tab khác rồi quay lại tab này (trạng thái 'visible'),
     * chúng ta cần thực hiện đánh dấu toàn bộ tin nhắn là đã đọc vì người dùng đã quay trở lại phòng chat.
     */
    document.addEventListener('visibilitychange', onVisibilityChange);

    hubConnection.on('Error', (error: string) => logger.warn('[Chat] Hub error', error, { conversationId }));
    
    hubConnection.onreconnecting(() => setConnected(false));
    hubConnection.onreconnected(() => {
     if (cancelled) return;
     setConnected(true);
     void (async () => {
      await hubConnection?.invoke('JoinConversation', conversationId);
      await loadInitialRef.current(true);
      await markReadRef.current();
     })();
    });
    hubConnection.onclose(() => setConnected(false));

    await hubConnection.start();
    
    if (cancelled) return;
    
    connectionRef.current = hubConnection;
    setConnected(true);
    initialConnectAttemptRef.current = 0;
    cancelWakeup();
    
    // Join group và đồng bộ dữ liệu ban đầu
    await hubConnection.invoke('JoinConversation', conversationId);
    await loadInitialRef.current(false);
    await markReadRef.current();
   } catch (error) {
    if (!cancelled) {
     logger.warn('[Chat] Connection failed', error, { conversationId });
     // Fallback tải dữ liệu qua REST nếu SignalR thất bại
     await loadInitialRef.current(false);
     const retryIndex = Math.min(
      initialConnectAttemptRef.current + 1,
      Math.max(0, schedule.length - 1),
     );
     initialConnectAttemptRef.current = retryIndex;
     scheduleWakeup(
      schedule[retryIndex]
      ?? RUNTIME_POLICY_FALLBACKS.realtime.reconnectScheduleMs[1]
      ?? 2_000,
     );
    }
   } finally {
    if (!cancelled) {
     initializingRef.current = false;
    }
   }
  };

  void init();

  return () => {
   cancelled = true;
   initializingRef.current = false;
   
   // Gỡ bỏ trình lắng nghe sự kiện visibilitychange để tránh rò rỉ bộ nhớ (memory leak).
   document.removeEventListener('visibilitychange', onVisibilityChange);
   if (conversationInvalidateTimeout) clearTimeout(conversationInvalidateTimeout);

   if (typingTimeoutRef.current) clearTimeout(typingTimeoutRef.current);
   if (!hubConnection) return;
   
   if (hubConnection.state === HubConnectionState.Connected) {
    void hubConnection.invoke('LeaveConversation', conversationId).catch(() => undefined);
   }
   
   if ([HubConnectionState.Connected, HubConnectionState.Reconnecting, HubConnectionState.Disconnecting].includes(hubConnection.state)) {
    void hubConnection.stop().catch(() => undefined);
   }
   connectionRef.current = null;
  };
 }, [
  appendMessage,
  cancelWakeup,
  connectionRef,
  conversationId,
  currentUserId,
  lastInitialLoadTimeRef,
  loadInitialRef,
  markReadRef,
  queryClient,
  resetForConversation,
  scheduleWakeup,
  setConnected,
  setConversation,
  setLoading,
  setMessages,
  setTypingUserId,
  typingTimeoutRef,
  wakeupVersion,
 ]);
}
