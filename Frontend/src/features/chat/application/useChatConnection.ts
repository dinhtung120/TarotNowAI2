'use client';

import { useCallback, useEffect, useMemo, useRef, useState } from 'react';
import { HubConnectionState, type HubConnection, type ILogger, type LogLevel } from '@microsoft/signalr';
import { useQueryClient } from '@tanstack/react-query';
import { useAuthStore } from '@/store/authStore';
import {
 listMessages,
 sendConversationMessage,
 type ChatMessageDto,
 type ConversationDto,
 type MediaPayloadDto,
} from '@/features/chat/application/actions';
import {
 appendUniqueMessage,
 mergeHistoryWithRealtimeMessages,
} from '@/features/chat/domain/mergeMessages';
import { logger } from '@/shared/infrastructure/logging/logger';

interface UseChatConnectionOptions {
 conversationId?: string | null;
}

const PAGE_SIZE = 50;

export function useChatConnection({ conversationId }: UseChatConnectionOptions) {
 const authStore = useAuthStore();
 const authUserId = authStore.user?.id ?? '';
 const token = authStore.token;

 const queryClient = useQueryClient();

 const getCachedConversation = (id?: string | null): ConversationDto | null => {
  if (!id) return null;
  const cachedActive = queryClient.getQueryData<{ conversations: ConversationDto[] }>(['chat', 'inbox', 'active']);
  const activeMatch = cachedActive?.conversations?.find((c) => c.id === id);
  if (activeMatch) return activeMatch;
  const cachedPending = queryClient.getQueryData<{ conversations: ConversationDto[] }>(['chat', 'inbox', 'pending']);
  return cachedPending?.conversations?.find((c) => c.id === id) ?? null;
 };

 const [messages, setMessages] = useState<ChatMessageDto[]>([]);
 const [newMessage, setNewMessage] = useState('');
 const [loading, setLoading] = useState(false);
 const [loadingMore, setLoadingMore] = useState(false);
 const [sending, setSending] = useState(false);
 const [connected, setConnected] = useState(false);
 const [conversation, setConversation] = useState<ConversationDto | null>(null);
 const [nextCursor, setNextCursor] = useState<string | null>(null);
 const [typingUserId, setTypingUserId] = useState<string | null>(null);

 /**
  * initializing: Trạng thái khởi tạo ban đầu.
  *
  * Khi lần đầu truy cập trực tiếp URL /chat/[id], TanStack Query cache
  * trống nên conversation = null. Nếu không phân biệt "đang load" vs
  * "thực sự không có data", UI sẽ hiện read-only hint gây nhầm lẫn.
  *
  * - initializing = true: Đang connect SignalR + load messages lần đầu
  *   → UI nên hiện input bar (disabled vì connected = false)
  * - initializing = false: Đã load xong → quyết định dựa trên conversation.status
  */
 const [initializing, setInitializing] = useState(true);

 const connectionRef = useRef<HubConnection | null>(null);
 const typingTimeoutRef = useRef<ReturnType<typeof setTimeout> | null>(null);
 const messagesEndRef = useRef<HTMLDivElement>(null);
 const inputRef = useRef<HTMLInputElement>(null);
 const hasLoadedInitialRef = useRef(false);

 /* ========================================================================
  * Ref ổn định cho các callback dùng bên trong useEffect chính.
  *
  * Vấn đề gốc (BUG): useEffect khởi tạo SignalR connection có dependency
  * array chứa loadInitial, markRead, scrollToBottom. Ba hàm này là
  * useCallback phụ thuộc vào conversationId / scrollToBottom → mỗi khi
  * reference thay đổi, useEffect chạy lại → teardown connection cũ →
  * tạo connection mới → setConversation(null) (line reset) →
  * canShowInput = false → UI read-only tạm thời.
  *
  * Triệu chứng: Lần đầu load trang chat qua client-side navigation,
  * input/ảnh/ghi âm bị disabled (read-only). F5 lại thì hoạt động bình
  * thường vì lúc đó effect chỉ chạy 1 lần.
  *
  * Cách sửa: Dùng ref để giữ reference ổn định cho các callback.
  * useEffect chính CHỈ phụ thuộc vào conversationId, currentUserId, token
  * (những giá trị thực sự cần tạo lại connection khi thay đổi).
  * Bên trong effect, gọi qua ref.current() để luôn dùng phiên bản
  * mới nhất mà không trigger re-run effect.
  * ======================================================================== */
 const loadInitialRef = useRef<(silent?: boolean) => Promise<void>>(async () => {});
 const markReadRef = useRef<() => Promise<void>>(async () => {});
 const scrollToBottomRef = useRef<(behavior?: ScrollBehavior) => void>(() => {});

 const currentUserId = authUserId;
 const isUserRole = useMemo(() => {
  if (!conversation || !authUserId) return null;
  return conversation.userId === authUserId;
 }, [authUserId, conversation]);
 const otherName = useMemo(() => {
  if (!conversation || isUserRole === null) return '';
  return isUserRole ? conversation.readerName ?? '' : conversation.userName ?? '';
 }, [conversation, isUserRole]);
 const otherAvatar = useMemo(() => {
  if (!conversation || isUserRole === null) return null;
  return isUserRole ? conversation.readerAvatar ?? null : conversation.userAvatar ?? null;
 }, [conversation, isUserRole]);
 const remoteTyping = typingUserId !== null && typingUserId !== currentUserId;
 const hasMore = Boolean(nextCursor);

 /* scrollToBottom: stable callback (deps rỗng) → ít khi thay đổi reference,
  * nhưng vẫn cập nhật ref để đảm bảo an toàn. */
 const scrollToBottom = useCallback((behavior: ScrollBehavior = 'smooth') => {
  messagesEndRef.current?.scrollIntoView({ behavior });
 }, []);
 scrollToBottomRef.current = scrollToBottom;

 /* markRead: phụ thuộc conversationId → mỗi khi đổi phòng chat, reference
  * mới. Cập nhật ref để useEffect luôn gọi đúng phiên bản. */
 const markRead = useCallback(async () => {
  if (!connectionRef.current || !conversationId) return;
  try {
   await connectionRef.current.invoke('MarkRead', conversationId);
  } catch (error) {
   logger.warn('[Chat] markRead failed', error, { conversationId });
  }
 }, [conversationId]);
 markReadRef.current = markRead;

 /**
  * Gửi tin nhắn text qua SignalR hoặc REST API fallback.
  *
  * TRƯỚC ĐÂY: Chỉ gửi qua SignalR → nếu connected === false thì không gửi được.
  * Trên mobile, SignalR connection bị chậm/fail → conversation stuck ở pending.
  *
  * GIẢI PHÁP: Khi SignalR chưa connected, dùng REST API `sendConversationMessage`
  * như fallback. Tin nhắn vẫn được gửi tới server, và khi SignalR reconnect,
  * `loadInitial(true)` sẽ sync lại message list.
  */
 const sendTypedMessage = useCallback(
  async (content: string, messageType = 'text') => {
   const normalizedContent = content.trim();
   if (!normalizedContent || !conversationId) {
    return false;
   }

   /* Ưu tiên SignalR nếu đã connected (real-time) */
   if (connectionRef.current && connected) {
    try {
     await connectionRef.current.invoke('SendMessage', conversationId, normalizedContent, messageType);
     return true;
    } catch (error) {
     logger.warn('[Chat] SendMessage via SignalR failed, trying REST fallback', error, {
      conversationId,
      messageType,
     });
    }
   }

   /* Fallback: gửi qua REST API khi SignalR không khả dụng */
   try {
    const result = await sendConversationMessage(conversationId, {
     type: messageType,
     content: normalizedContent,
    });
    if (result.success && result.data) {
     /* Thêm message vào list ngay lập tức (optimistic update) */
     setMessages((prev) => appendUniqueMessage(prev, result.data!));
     return true;
    }
    logger.warn('[Chat] REST SendMessage failed', result.error, { conversationId });
    return false;
   } catch (error) {
    logger.warn('[Chat] REST SendMessage error', error, { conversationId });
    return false;
   }
  },
  [connected, conversationId]
 );

 const sendMediaMessage = useCallback(
  async (type: 'image' | 'voice', payload: MediaPayloadDto) => {
   if (!conversationId) {
    return false;
   }

   if (connectionRef.current && connected) {
    try {
     await connectionRef.current.invoke('SendMessage', conversationId, JSON.stringify(payload), type);
     return true;
    } catch (error) {
     logger.warn('[Chat] SendMediaMessage via SignalR failed, trying REST fallback', error, {
      conversationId,
      type,
     });
    }
   }

   /* Fallback: gửi qua REST API khi SignalR không khả dụng */
   try {
    const result = await sendConversationMessage(conversationId, {
     type,
     content: JSON.stringify(payload),
     mediaPayload: payload,
    });
    if (result.success && result.data) {
     /* Thêm message vào list ngay lập tức (optimistic update) */
     setMessages((prev) => appendUniqueMessage(prev, result.data!));
     return true;
    }
    logger.warn('[Chat] REST SendMediaMessage failed', result.error, { conversationId });
    return false;
   } catch (error) {
    logger.warn('[Chat] REST SendMediaMessage error', error, { conversationId });
    return false;
   }
  },
  [connected, conversationId]
 );

 const handleSendTextMessage = useCallback(async () => {
  if (!newMessage.trim()) return false;
  setSending(true);
  const success = await sendTypedMessage(newMessage, 'text');
  setSending(false);
  if (success) {
   setNewMessage('');
   inputRef.current?.focus();
   /* Chỉ gửi TypingStopped nếu SignalR đang connected */
   if (connectionRef.current && connected) {
    void connectionRef.current.invoke('TypingStopped', conversationId).catch(() => undefined);
   }
  }
  return success;
 }, [conversationId, connected, newMessage, sendTypedMessage]);

 const notifyTyping = useCallback(
  (typing: boolean) => {
   if (!connectionRef.current || !conversationId || !connected) return;
   const event = typing ? 'TypingStarted' : 'TypingStopped';
   void connectionRef.current.invoke(event, conversationId).catch(() => undefined);
  },
  [connected, conversationId]
 );

 /* loadInitial: phụ thuộc conversationId → reference thay đổi khi đổi phòng.
  * Trước đây còn phụ thuộc scrollToBottom → gây thêm lần thay đổi reference
  * không cần thiết. Giờ dùng scrollToBottomRef bên trong thay vì closure. */
 const loadInitial = useCallback(async (silent = false) => {
  if (!conversationId) return;
  if (!silent) {
   setLoading(true);
  }
  const history = await listMessages(conversationId, { limit: PAGE_SIZE });
  if (history.success && history.data) {
   const payload = history.data;
   setMessages((prev) => {
    if (silent && hasLoadedInitialRef.current) {
     return mergeHistoryWithRealtimeMessages(prev, payload.messages);
    }

    return mergeHistoryWithRealtimeMessages([], payload.messages);
   });
   setConversation(payload.conversation ?? null);
   setNextCursor(payload.nextCursor ?? null);
   if (!hasLoadedInitialRef.current) {
    hasLoadedInitialRef.current = true;
    /* Dùng ref thay vì closure trực tiếp để tránh dependency cycle */
    setTimeout(() => scrollToBottomRef.current('auto'), 10);
   }
  } else {
   if (!silent) {
    setMessages([]);
    setConversation(null);
    setNextCursor(null);
   }
  }

  if (!silent) {
   setLoading(false);
   /* Đánh dấu đã hoàn tất khởi tạo – UI sẽ chuyển sang dựa vào
    * conversation.status thực tế thay vì giả định đang load */
   setInitializing(false);
  }
 }, [conversationId]);
 loadInitialRef.current = loadInitial;

 const loadMore = useCallback(async () => {
  if (!conversationId || !nextCursor || loadingMore) return;
  setLoadingMore(true);
  const result = await listMessages(conversationId, { cursor: nextCursor, limit: PAGE_SIZE });
  if (result.success && result.data) {
   const payload = result.data;
   const older = [...payload.messages].reverse();
   setMessages((prev) => {
    const combined = [...older, ...prev];
    const ids = new Set<string>();
    return combined.filter((item) => {
      if (ids.has(item.id)) return false;
      ids.add(item.id);
      return true;
    });
   });
   setNextCursor(payload.nextCursor ?? null);
  }
  setLoadingMore(false);
 }, [conversationId, loadingMore, nextCursor]);

 /* ========================================================================
  * useEffect chính: khởi tạo SignalR hub connection
  *
  * Dependency array CHỈ chứa 3 giá trị thực sự cần tạo lại connection:
  * - conversationId: đổi phòng chat → cần connection mới
  * - currentUserId: đổi user đăng nhập → cần connection mới
  * - token: token mới (refresh) → cần connection mới
  *
  * Các callback (loadInitial, markRead, scrollToBottom) được truy cập
  * qua ref để tránh dependency thay đổi gây re-initialization vô hạn.
  * Đây chính là fix cho bug "read-only khi lần đầu navigate vào chat room".
  * ======================================================================== */
 useEffect(() => {
  if (!conversationId || !token) {
   setMessages([]);
   setConversation(null);
   setConnected(false);
   setLoading(false);
   setNextCursor(null);
   hasLoadedInitialRef.current = false;
   return;
  }

  let cancelled = false;
  let hubConnection: HubConnection | null = null;

  const initConnection = async () => {
   const signalR = await import('@microsoft/signalr');
   const { getSignalRHubUrl } = await import('@/shared/infrastructure/realtime/signalRUrl');
    const customLogger: ILogger = {
     log: (logLevel: LogLevel, message: string) => {
      if (logLevel === signalR.LogLevel.Error || logLevel === signalR.LogLevel.Critical) {
       console.warn(`[SignalR Error] ${message}`);
      } else if (logLevel === signalR.LogLevel.Warning) {
       console.warn(`[SignalR Warn] ${message}`);
      } else if (logLevel === signalR.LogLevel.Information) {
       console.info(`[SignalR Info] ${message}`);
      } else {
       console.debug(`[SignalR Debug] ${message}`);
      }
     }
    };

    hubConnection = new signalR.HubConnectionBuilder()
     .withUrl(getSignalRHubUrl('/api/v1/chat'), {
      /**
				 * Dùng useAuthStore.getState().token thay vì closure variable `token`
				 * để luôn đọc token mới nhất từ Zustand store.
				 */
				accessTokenFactory: () => useAuthStore.getState().token ?? '',
     })
     .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
     .configureLogging(customLogger)
    .build();

   hubConnection.serverTimeoutInMilliseconds = 120000; // 2 minutes server timeout

   hubConnection.on('message.created', (message: ChatMessageDto) => {
    if (message.conversationId !== conversationId) return;
    setMessages((prev) => appendUniqueMessage(prev, message));
   });

   hubConnection.on('message.read', (payload: { userId: string; conversationId: string }) => {
    if (payload.conversationId !== conversationId) return;
    if (payload.userId === currentUserId) return;
    setMessages((prev) =>
     prev.map((item) =>
      item.senderId === currentUserId
       ? { ...item, isRead: true }
       : item
     )
    );
   });

   hubConnection.on('typing.started', (payload: { userId: string; conversationId: string }) => {
    if (payload.conversationId !== conversationId || payload.userId === currentUserId) return;
    setTypingUserId(payload.userId);
    if (typingTimeoutRef.current) clearTimeout(typingTimeoutRef.current);
    typingTimeoutRef.current = setTimeout(() => setTypingUserId(null), 2500);
   });

   hubConnection.on('typing.stopped', (payload: { userId: string; conversationId: string }) => {
    if (payload.conversationId !== conversationId || payload.userId === currentUserId) return;
    setTypingUserId(null);
   });

   /* Khi conversation cập nhật (status đổi, escrow đổi...),
    * ta chỉ cần fetch 1 tin nhắn ảo (limit: 1) để lấy lại metadata
    * của conversation, KHÔNG gọi lại toàn bộ 50 tin nhắn làm giật lag.
    * Các tin nhắn mới thực sự đã được update qua event 'message.created'. */
   hubConnection.on('conversation.updated', (payload: { conversationId: string; type?: string }) => {
    if (payload.conversationId !== conversationId) return;
    if (payload.type === 'message_created') return;
    void listMessages(conversationId, { limit: 1 }).then((res) => {
     if (res.success && res.data?.conversation) {
      setConversation(res.data.conversation);
     }
    });
   });

   hubConnection.on('Error', (error: string) => {
    logger.warn('[Chat] Hub error', error, { conversationId });
   });

   hubConnection.onreconnecting(() => setConnected(false));
   hubConnection.onreconnected(() => {
    if (cancelled) return;
    setConnected(true);
    void hubConnection?.invoke('JoinConversation', conversationId);
    void markReadRef.current();
   });
   hubConnection.onclose(() => setConnected(false));

   try {
    await hubConnection.start();
    if (cancelled) {
     if (hubConnection.state !== HubConnectionState.Disconnected) {
      await hubConnection.stop().catch(() => undefined);
     }
     return;
    }
    connectionRef.current = hubConnection;
    setConnected(true);

    await hubConnection.invoke('JoinConversation', conversationId);
    await loadInitialRef.current(false);
    await markReadRef.current();
   } catch (error) {
    /* Đổi sang logger.warn thay cho error để tránh Next.js dev overlay đỏ màn hình
     * khi rớt mạng hoặc 401 do token hết hạn */
    logger.warn('[Chat] Connection failed', error, { conversationId });
    await loadInitialRef.current(false);
   }
  };

  setMessages([]);
  setConversation(getCachedConversation(conversationId));
  setNextCursor(null);
  setTypingUserId(null);
  hasLoadedInitialRef.current = false;
  /* Reset initializing khi đổi phòng chat – đảm bảo UI hiện input bar
   * (disabled) thay vì read-only hint trong lúc chờ load conversation mới */
  setInitializing(true);
  void initConnection();

  return () => {
   cancelled = true;
   if (typingTimeoutRef.current) clearTimeout(typingTimeoutRef.current);
   if (hubConnection) {
    if (hubConnection.state === HubConnectionState.Connected) {
     void hubConnection.invoke('LeaveConversation', conversationId).catch(() => undefined);
    }
    if (hubConnection.state === HubConnectionState.Connected
      || hubConnection.state === HubConnectionState.Reconnecting
      || hubConnection.state === HubConnectionState.Disconnecting) {
     void hubConnection.stop().catch(() => undefined);
    }
   }
   connectionRef.current = null;
  };
 // eslint-disable-next-line react-hooks/exhaustive-deps
 }, [conversationId, currentUserId, token]);

 return {
  messages,
  setMessages,
  newMessage,
  setNewMessage,
  loading,
  loadingMore,
  hasMore,
  loadMore,
  sending,
  connected,
  currentUserId,
  conversation,
  otherName,
  otherAvatar,
  isUserRole,
  remoteTyping,
  messagesEndRef,
  inputRef,
  sendTypedMessage,
  sendMediaMessage,
  handleSendTextMessage,
  notifyTyping,
  markRead,
  /** Đang khởi tạo connection + load dữ liệu lần đầu */
  initializing,
 };
}
