'use client';

import { useEffect, useRef, type Dispatch, type MutableRefObject, type SetStateAction } from 'react';
import {
 HubConnectionState,
 type HubConnection,
 type LogLevel,
} from '@microsoft/signalr';
import type { QueryClient } from '@tanstack/react-query';
import type { ChatMessageDto, ConversationDto } from '@/features/chat/application/actions';
import { listMessages } from '@/features/chat/application/actions';
import { logger } from '@/shared/infrastructure/logging/logger';
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

async function createHubConnection() {
 const signalR = await import('@microsoft/signalr');
 const { getSignalRHubUrl } = await import('@/shared/infrastructure/realtime/signalRUrl');
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
  .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
  .configureLogging(customLogger)
  .build();
}

export function useChatSignalRLifecycle(options: UseChatSignalRLifecycleOptions) {
 const {
  conversationId, currentUserId, queryClient, connectionRef, typingTimeoutRef,
  lastInitialLoadTimeRef, loadInitialRef, markReadRef, setConnected, setLoading,
  setTypingUserId, setMessages, setConversation, resetForConversation, appendMessage,
 } = options;

 const initializingRef = useRef(false);

 useEffect(() => {
  // Chỉ khởi tạo khi có đủ thông tin và không đang trong quá trình khởi tạo khác cho cùng conversation
  if (!conversationId || initializingRef.current) {
   if (!conversationId) {
    resetForConversation(null);
    setConnected(false);
    setLoading(false);
   }
   return;
  }

  let cancelled = false;
  let hubConnection: HubConnection | null = null;
  initializingRef.current = true;
  
  resetForConversation(getCachedConversation(queryClient, conversationId));
  setTypingUserId(null);

  const onVisibilityChange = () => {
   if (document.visibilityState === 'visible' && conversationId) {
    void markReadRef.current();
   }
  };

  const init = async () => {
   try {
    hubConnection = await createHubConnection();
    // Cấu hình timeout dài hơn để tránh bị ngắt kết nối do mạng chập chờn
    hubConnection.serverTimeoutInMilliseconds = 120000;
    
    // Đăng ký các sự kiện nhận tin nhắn và trạng thái realtime
    hubConnection.on('message.created', (message: ChatMessageDto) => {
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
      if (document.visibilityState === 'visible' && message.senderId !== currentUserId) {
       void markReadRef.current();
      }
     }
    });
    
    hubConnection.on('message.read', (payload: { userId: string; conversationId: string }) => {
     if (payload.conversationId !== conversationId || payload.userId === currentUserId) return;
     setMessages((prev) =>
      prev.map((item) =>
       item.senderId === currentUserId ? { ...item, isRead: true } : item
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

    hubConnection.on('conversation.updated', (payload: { conversationId: string; type?: string }) => {
     if (payload.conversationId !== conversationId) return;
     // Các sự kiện này không làm thay đổi trạng thái room cần refetch lại conversation.
     if (payload.type === 'message_created' || payload.type === 'member_joined' || payload.type === 'message_read') return;
     // Debounce việc fetch lại dữ liệu khi có cập nhật status conversation
     if (Date.now() - lastInitialLoadTimeRef.current < 2000) return;
     void listMessages(conversationId, { limit: 1 }).then((res) => {
      if (res.success && res.data?.conversation) setConversation(res.data.conversation);
     });
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
     void hubConnection?.invoke('JoinConversation', conversationId);
     void markReadRef.current();
    });
    hubConnection.onclose(() => setConnected(false));

    await hubConnection.start();
    
    if (cancelled) return;
    
    connectionRef.current = hubConnection;
    setConnected(true);
    
    // Join group và đồng bộ dữ liệu ban đầu
    await hubConnection.invoke('JoinConversation', conversationId);
    await loadInitialRef.current(false);
    await markReadRef.current();
   } catch (error) {
    if (!cancelled) {
     logger.warn('[Chat] Connection failed', error, { conversationId });
     // Fallback tải dữ liệu qua REST nếu SignalR thất bại
     await loadInitialRef.current(false);
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
  connectionRef,
  conversationId,
  currentUserId,
  lastInitialLoadTimeRef,
  loadInitialRef,
  markReadRef,
  queryClient,
  resetForConversation,
  setConnected,
  setConversation,
  setLoading,
  setMessages,
  setTypingUserId,
  typingTimeoutRef,
 ]);
}
