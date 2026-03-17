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
import UserLayout from '@/components/layout/UserLayout';
import { GlassCard } from '@/components/ui';

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

 hubConnection.on('Error', (error: string) => {
 console.error('[Chat] Hub error:', error);
 });

 hubConnection.onreconnecting(() => {
 setConnected(false);
 });

 hubConnection.onreconnected(() => {
 setConnected(true);
 hubConnection?.invoke('JoinConversation', conversationId);
 });

 hubConnection.onclose(() => {
 setConnected(false);
 });

 // 4. Start connection
 try {
 await hubConnection.start();
 setConnected(true);
 connectionRef.current = hubConnection;

 await hubConnection.invoke('JoinConversation', conversationId);
 await hubConnection.invoke('MarkRead', conversationId);
 } catch (error) {
 console.error('[Chat] Connection failed:', error);
 }

 // 5. Load lịch sử chat (REST)
 const history = await listMessages(conversationId);
 if (history) {
 setMessages([...history.messages].reverse());
 }
 setLoading(false);
 setTimeout(scrollToBottom, 500);
 };

 initConnection();

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

 if (isSystem) {
 return (
 <div key={msg.id} className="flex justify-center py-3">
 <span className="px-4 py-1.5 rounded-full tn-surface text-[10px] tn-text-muted font-medium tracking-widest uppercase border tn-border-soft">
 {msg.content}
 </span>
 </div>
 );
 }

 if (msg.type.startsWith('payment_')) {
 return (
 <div key={msg.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'} py-2`}>
 <div className="max-w-[85%] p-4 rounded-2xl bg-[var(--warning-bg)] border border-[var(--warning)]/30 space-y-1.5 shadow-[0_4px_20px_var(--c-245-158-11-10)]">
 <div className={`text-[9px] font-black uppercase tracking-widest ${
 msg.type === 'payment_offer' ? 'text-[var(--warning)]' :
 msg.type === 'payment_accept' ? 'text-[var(--success)]' : 'text-[var(--danger)]'
 }`}>
 {msg.type === 'payment_offer' ? '💎 Đề xuất thanh toán' :
 msg.type === 'payment_accept' ? '✅ Đã chấp nhận' : '❌ Đã từ chối'}
 </div>
 <p className="text-xs text-[var(--text-secondary)]">{msg.content}</p>
 {msg.paymentPayload && (
 <div className="text-sm font-bold text-[var(--warning)]">{msg.paymentPayload.amountDiamond} 💎</div>
 )}
 </div>
 </div>
 );
 }

 return (
 <div key={msg.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'} py-2 group`}>
 <div className={`max-w-[85%] px-5 py-3.5 rounded-[1.5rem] relative ${
 isMe
 ? 'bg-gradient-to-br from-[var(--purple-accent)]/80 to-[var(--purple-accent)] border border-[var(--purple-accent)]/40 tn-text-primary rounded-tr-sm shadow-[0_4px_20px_var(--c-168-85-247-20)]'
 : 'tn-surface-strong border tn-border-soft tn-text-secondary rounded-tl-sm hover:tn-surface-strong transition-colors'
 }`}>
 <p className="text-sm leading-relaxed break-words font-sans">{msg.content}</p>
 <div className={`text-[9px] mt-1.5 font-bold tracking-widest uppercase ${isMe ? 'text-[color:var(--c-61-49-80-60)]' : 'tn-text-muted'}`}>
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
 <UserLayout>
 <div className="max-w-4xl mx-auto px-4 md:px-6 pt-6 pb-24 h-[calc(100vh-80px)] md:h-[calc(100vh)] flex flex-col w-full animate-in fade-in ease-out duration-700">
 <GlassCard className="flex flex-col flex-1 overflow-hidden !p-0 tn-border shadow-2xl relative">
 {/* Header */}
 <div className="flex items-center justify-between px-6 py-5 border-b tn-border tn-overlay z-10 shrink-0">
 <div className="flex items-center gap-4">
 <button
 onClick={() => router.push('/chat')}
 className="p-2.5 rounded-xl hover:tn-surface-strong transition-colors tn-surface group"
 >
 <ArrowLeft className="w-4 h-4 text-[var(--text-secondary)] group-hover:tn-text-primary transition-colors" />
 </button>
 <div>
 <div className="text-base font-black tn-text-primary italic tracking-tighter">Phòng Kết Nối Tâm Linh</div>
 <div className="flex items-center gap-2 text-[10px] font-bold uppercase tracking-widest mt-0.5">
 <div className={`w-2 h-2 rounded-full ${connected ? 'bg-[var(--success)] animate-pulse shadow-[0_0_10px_var(--c-16-185-129-50)]' : 'bg-[var(--text-muted)]'}`} />
 <span className={connected ? 'text-[var(--success)]' : 'tn-text-muted'}>
 {connected ? 'Tín hiệu ổn định' : 'Đang đồng bộ...'}
 </span>
 </div>
 </div>
 </div>
 <button
 onClick={() => setShowReport(true)}
 className="p-2.5 rounded-xl hover:bg-[var(--danger-bg)] transition-colors text-[var(--text-tertiary)] hover:text-[var(--danger)] group border border-transparent hover:border-[var(--danger)]/30"
 title="Báo cáo vi phạm"
 >
 <Flag className="w-4 h-4" />
 </button>
 </div>

 {/* Escrow Panel — Phase 2.3 */}
 <div className="shrink-0 tn-overlay-soft z-10">
 <EscrowPanel conversationId={conversationId} currentUserId={currentUserId} />
 </div>

 {/* Messages Area */}
 <div className="flex-1 overflow-y-auto px-6 py-6 space-y-2 relative scroll-smooth scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
 {loading && (
 <div className="absolute inset-0 flex flex-col items-center justify-center space-y-4 tn-overlay z-20">
 <Loader2 className="w-10 h-10 text-[var(--purple-accent)] animate-spin" />
 <span className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">Đồng bộ tần số...</span>
 </div>
 )}

 {!loading && messages.length === 0 && (
 <div className="h-full flex flex-col items-center justify-center space-y-4 opacity-60">
 <div className="w-20 h-20 rounded-full border border-dashed tn-border-strong flex items-center justify-center">
 <MessageCircle className="w-8 h-8 text-[var(--purple-accent)]/60" />
 </div>
 <p className="text-[var(--text-secondary)] text-sm font-medium">Kết nối đã mở. Hãy gửi thông điệp đầu tiên.</p>
 </div>
 )}

 {messages.map(renderMessage)}
 <div ref={messagesEndRef} className="h-4" />
 </div>

 {/* Input Area */}
 <div className="p-4 md:p-5 border-t tn-border tn-overlay shrink-0">
 <div className="flex items-center gap-3">
 <input
 ref={inputRef}
 id="chat-input"
 type="text"
 value={newMessage}
 onChange={(e) => setNewMessage(e.target.value)}
 onKeyDown={handleKeyDown}
 placeholder="Truyền tải thông điệp của bạn..."
 disabled={!connected}
 className="flex-1 tn-field hover:tn-border-strong rounded-[1.5rem] px-5 py-4 text-sm tn-text-primary placeholder:tn-text-muted tn-field-accent transition-all disabled:opacity-50"
 />
 <button
 id="chat-send-btn"
 onClick={handleSend}
 disabled={!newMessage.trim() || sending || !connected}
 className="p-4 bg-[var(--purple-accent)] hover:bg-[var(--purple-muted)] disabled:tn-surface rounded-[1.5rem] transition-all disabled:opacity-50 tn-text-primary disabled:tn-text-muted active:scale-95 shadow-[0_0_20px_var(--c-168-85-247-30)] disabled:shadow-none"
 >
 {sending
 ? <Loader2 className="w-5 h-5 animate-spin" />
 : <Send className="w-5 h-5 ml-0.5" />}
 </button>
 </div>
 </div>
 </GlassCard>

 {/* Report Modal */}
 {showReport && (
 <ReportModal
 conversationId={conversationId}
 onClose={() => setShowReport(false)}
 />
 )}
 </div>
 </UserLayout>
 );
}
