'use client';

import React, { useEffect, useState, useRef, useCallback } from 'react';
import { listMessages, getSignalRToken, type ChatMessageDto } from '@/actions/chatActions';
import {
  Send, Loader2, ArrowLeft, Flag, MessageCircle
} from 'lucide-react';
import { useParams } from 'next/navigation';
import { useRouter } from '@/i18n/routing';
import * as signalR from '@microsoft/signalr';
import ReportModal from '@/components/chat/ReportModal';
import EscrowPanel from '@/components/chat/EscrowPanel';

/**
 * Chat Screen — tin nhắn realtime qua SignalR.
 *
 * Tính năng:
 * → Kết nối SignalR hub tại /api/v1/chat
 * → Gửi/nhận tin nhắn realtime
 * → Load lịch sử chat lần đầu (REST API)
 * → Auto-scroll khi có tin mới
 * → Mark read khi mở conversation
 * → Report modal
 *
 * Trạng thái kết nối: Connected → nhận tin qua "ReceiveMessage"
 * Nếu mất kết nối: reconnect tự động (SignalR built-in).
 */
export default function ChatPage() {
  const params = useParams();
  const router = useRouter();
  const conversationId = params.id as string;

  // State
  const [messages, setMessages] = useState<ChatMessageDto[]>([]);
  const [newMessage, setNewMessage] = useState('');
  const [loading, setLoading] = useState(true);
  const [sending, setSending] = useState(false);
  const [connected, setConnected] = useState(false);
  const [showReport, setShowReport] = useState(false);

  // Refs
  const connectionRef = useRef<signalR.HubConnection | null>(null);
  const messagesEndRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);

  // Lấy currentUserId từ cookie hoặc token
  // Đơn giản hóa: parse từ token payload tại client
  const [currentUserId, setCurrentUserId] = useState<string>('');

  /** Auto-scroll xuống cuối khi có tin mới */
  const scrollToBottom = useCallback(() => {
    messagesEndRef.current?.scrollIntoView({ behavior: 'smooth' });
  }, []);

  // ======================================================================
  // SignalR Connection
  // ======================================================================
  useEffect(() => {
    if (!conversationId) return;

    let hubConnection: signalR.HubConnection | null = null;

    const initConnection = async () => {
      // 1. Lấy token từ server action
      const token = await getSignalRToken();
      if (!token) {
        setLoading(false);
        return;
      }

      // Parse userId từ JWT (base64 decode payload)
      try {
        const payload = JSON.parse(atob(token.split('.')[1]));
        const userId = payload.sub || payload.nameid || payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
        if (userId) setCurrentUserId(userId);
      } catch { /* ignore parse errors */ }

      // 2. Tạo SignalR connection
      const apiUrl = process.env.NEXT_PUBLIC_API_URL?.replace('/api/v1', '') || 'http://localhost:5037';
      hubConnection = new signalR.HubConnectionBuilder()
        .withUrl(`${apiUrl}/api/v1/chat`, {
          accessTokenFactory: () => token,
        })
        .withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
        .configureLogging(signalR.LogLevel.Warning)
        .build();

      // 3. Đăng ký event handlers
      hubConnection.on('ReceiveMessage', (message: ChatMessageDto) => {
        setMessages(prev => [...prev, message]);
        scrollToBottom();
      });

      hubConnection.on('UserJoined', (data: { userId: string }) => {
        console.log('[Chat] User joined:', data.userId);
      });

      hubConnection.on('MessagesRead', (data: { userId: string }) => {
        console.log('[Chat] Messages read by:', data.userId);
      });

      hubConnection.on('Error', (error: string) => {
        console.error('[Chat] Hub error:', error);
      });

      hubConnection.onreconnecting(() => {
        setConnected(false);
        console.log('[Chat] Reconnecting...');
      });

      hubConnection.onreconnected(() => {
        setConnected(true);
        // Re-join group sau reconnect
        hubConnection?.invoke('JoinConversation', conversationId);
        console.log('[Chat] Reconnected');
      });

      hubConnection.onclose(() => {
        setConnected(false);
        console.log('[Chat] Connection closed');
      });

      // 4. Start connection
      try {
        await hubConnection.start();
        setConnected(true);
        connectionRef.current = hubConnection;

        // Join conversation group
        await hubConnection.invoke('JoinConversation', conversationId);

        // Mark messages as read
        await hubConnection.invoke('MarkRead', conversationId);
      } catch (error) {
        console.error('[Chat] Connection failed:', error);
      }

      // 5. Load lịch sử chat (REST)
      const history = await listMessages(conversationId);
      if (history) {
        // Reverse vì server trả DESC, UI cần ASC
        setMessages([...history.messages].reverse());
      }
      setLoading(false);
      setTimeout(scrollToBottom, 100);
    };

    initConnection();

    // Cleanup khi unmount
    return () => {
      if (hubConnection) {
        hubConnection.invoke('LeaveConversation', conversationId).catch(() => {});
        hubConnection.stop();
      }
    };
  }, [conversationId, scrollToBottom]);

  // ======================================================================
  // Gửi tin nhắn
  // ======================================================================
  const handleSend = async () => {
    if (!newMessage.trim() || !connectionRef.current || !connected) return;

    setSending(true);
    try {
      await connectionRef.current.invoke('SendMessage', conversationId, newMessage.trim(), 'text');
      setNewMessage('');
      inputRef.current?.focus();
    } catch (error) {
      console.error('[Chat] Send failed:', error);
    }
    setSending(false);
  };

  const handleKeyDown = (e: React.KeyboardEvent) => {
    if (e.key === 'Enter' && !e.shiftKey) {
      e.preventDefault();
      handleSend();
    }
  };

  // ======================================================================
  // Render helper cho message types
  // ======================================================================
  const renderMessage = (msg: ChatMessageDto) => {
    const isMe = msg.senderId === currentUserId;
    const isSystem = msg.type === 'system' || msg.type.startsWith('system_');

    // System messages — centered, no bubble
    if (isSystem) {
      return (
        <div key={msg.id} className="flex justify-center py-2">
          <span className="px-3 py-1 rounded-full bg-white/[0.03] text-[10px] text-zinc-600 font-medium">
            {msg.content}
          </span>
        </div>
      );
    }

    // Payment messages — special styling
    if (msg.type.startsWith('payment_')) {
      return (
        <div key={msg.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'} py-1`}>
          <div className="max-w-[80%] p-4 rounded-2xl bg-amber-500/10 border border-amber-500/20 space-y-1">
            <div className="text-[9px] font-black uppercase tracking-widest text-amber-400">
              {msg.type === 'payment_offer' ? '💎 Đề xuất thanh toán' :
               msg.type === 'payment_accept' ? '✅ Đã chấp nhận' : '❌ Đã từ chối'}
            </div>
            <p className="text-xs text-zinc-400">{msg.content}</p>
            {msg.paymentPayload && (
              <div className="text-sm font-bold text-amber-400">{msg.paymentPayload.amountDiamond} 💎</div>
            )}
          </div>
        </div>
      );
    }

    // Regular text messages — chat bubbles
    return (
      <div key={msg.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'} py-1`}>
        <div className={`max-w-[80%] px-4 py-3 rounded-2xl ${
          isMe
            ? 'bg-purple-600/30 border border-purple-500/20 text-white'
            : 'bg-white/[0.04] border border-white/5 text-zinc-300'
        }`}>
          <p className="text-sm leading-relaxed break-words">{msg.content}</p>
          <div className={`text-[9px] mt-1 ${isMe ? 'text-purple-400/60' : 'text-zinc-700'}`}>
            {new Date(msg.createdAt).toLocaleTimeString('vi-VN', { hour: '2-digit', minute: '2-digit' })}
          </div>
        </div>
      </div>
    );
  };

  // ======================================================================
  // Main Render
  // ======================================================================
  return (
    <div className="flex flex-col h-[calc(100vh-80px)] max-w-3xl mx-auto animate-in fade-in duration-500">
      {/* Header */}
      <div className="flex items-center justify-between px-6 py-4 border-b border-white/5">
        <div className="flex items-center gap-3">
          <button
            onClick={() => router.push('/chat' as any)}
            className="p-2 rounded-lg hover:bg-white/[0.04] transition-colors"
          >
            <ArrowLeft className="w-4 h-4 text-zinc-400" />
          </button>
          <div>
            <div className="text-sm font-black text-white">Cuộc trò chuyện</div>
            <div className="flex items-center gap-1.5 text-[10px]">
              <div className={`w-1.5 h-1.5 rounded-full ${connected ? 'bg-emerald-400 animate-pulse' : 'bg-zinc-600'}`} />
              <span className={connected ? 'text-emerald-400' : 'text-zinc-600'}>
                {connected ? 'Đã kết nối' : 'Đang kết nối...'}
              </span>
            </div>
          </div>
        </div>
        <button
          onClick={() => setShowReport(true)}
          className="p-2 rounded-lg hover:bg-white/[0.04] transition-colors text-zinc-600 hover:text-red-400"
          title="Báo cáo vi phạm"
        >
          <Flag className="w-4 h-4" />
        </button>
      </div>

      {/* Escrow Panel — Phase 2.3 */}
      <EscrowPanel conversationId={conversationId} currentUserId={currentUserId} />

      {/* Messages Area */}
      <div className="flex-1 overflow-y-auto px-6 py-4 space-y-1">
        {loading && (
          <div className="h-full flex flex-col items-center justify-center space-y-3">
            <Loader2 className="w-8 h-8 text-purple-500 animate-spin" />
            <span className="text-[10px] font-black uppercase tracking-widest text-zinc-600">Tải tin nhắn...</span>
          </div>
        )}

        {!loading && messages.length === 0 && (
          <div className="h-full flex flex-col items-center justify-center space-y-3">
            <MessageCircle className="w-12 h-12 text-zinc-800" />
            <p className="text-zinc-600 text-xs">Bắt đầu cuộc trò chuyện...</p>
          </div>
        )}

        {messages.map(renderMessage)}
        <div ref={messagesEndRef} />
      </div>

      {/* Input Area */}
      <div className="px-6 py-4 border-t border-white/5">
        <div className="flex items-center gap-3">
          <input
            ref={inputRef}
            id="chat-input"
            type="text"
            value={newMessage}
            onChange={(e) => setNewMessage(e.target.value)}
            onKeyDown={handleKeyDown}
            placeholder="Nhập tin nhắn..."
            disabled={!connected}
            className="flex-1 bg-white/[0.02] border border-white/10 rounded-xl px-4 py-3 text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-purple-500/30 transition-all disabled:opacity-50"
          />
          <button
            id="chat-send-btn"
            onClick={handleSend}
            disabled={!newMessage.trim() || sending || !connected}
            className="p-3 bg-purple-600 hover:bg-purple-500 disabled:bg-zinc-800 rounded-xl transition-all disabled:opacity-50"
          >
            {sending
              ? <Loader2 className="w-4 h-4 text-white animate-spin" />
              : <Send className="w-4 h-4 text-white" />}
          </button>
        </div>
      </div>

      {/* Report Modal */}
      {showReport && (
        <ReportModal
          conversationId={conversationId}
          onClose={() => setShowReport(false)}
        />
      )}
    </div>
  );
}
