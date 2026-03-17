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
import { useLocale, useTranslations } from 'next-intl';

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
	const t = useTranslations('Chat');
	const locale = useLocale();
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
				hubConnection.invoke('LeaveConversation', conversationId).catch(() => { });
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
					<span className="px-4 py-1.5 rounded-full bg-white/[0.03] text-[10px] text-zinc-500 font-medium tracking-widest uppercase border border-white/5">
						{msg.content}
					</span>
				</div>
			);
		}

		if (msg.type.startsWith('payment_')) {
			return (
				<div key={msg.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'} py-2`}>
					<div className="max-w-[85%] p-4 rounded-2xl bg-[var(--warning-bg)] border border-[var(--warning)]/30 space-y-1.5 shadow-[0_4px_20px_rgba(245,158,11,0.1)]">
						<div className={`text-[9px] font-black uppercase tracking-widest ${msg.type === 'payment_offer' ? 'text-[var(--warning)]' :
								msg.type === 'payment_accept' ? 'text-[var(--success)]' : 'text-[var(--danger)]'
							}`}>
							{msg.type === 'payment_offer' ? t('room.payment_offer') :
								msg.type === 'payment_accept' ? t('room.payment_accept') : t('room.payment_reject')}
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
				<div className={`max-w-[85%] px-5 py-3.5 rounded-[1.5rem] relative ${isMe
						? 'bg-gradient-to-br from-[var(--purple-accent)]/80 to-[var(--purple-accent)] border border-[var(--purple-accent)]/40 text-white rounded-tr-sm shadow-[0_4px_20px_rgba(168,85,247,0.2)]'
						: 'bg-white/[0.04] border border-white/5 text-zinc-200 rounded-tl-sm hover:bg-white/[0.06] transition-colors'
					}`}>
					<p className="text-sm leading-relaxed break-words font-sans">{msg.content}</p>
					<div className={`text-[9px] mt-1.5 font-bold tracking-widest uppercase ${isMe ? 'text-white/60' : 'text-zinc-600'}`}>
						{new Date(msg.createdAt).toLocaleTimeString(locale, { hour: '2-digit', minute: '2-digit' })}
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
				<GlassCard className="flex flex-col flex-1 overflow-hidden !p-0 border-white/10 shadow-2xl relative">

					{/* Header */}
					<div className="flex items-center justify-between px-6 py-5 border-b border-white/10 bg-black/40 backdrop-blur-md z-10 shrink-0">
						<div className="flex items-center gap-4">
								<button
									onClick={() => router.push('/chat')}
									className="p-2.5 rounded-xl hover:bg-white/10 transition-colors bg-white/5 group"
								>
								<ArrowLeft className="w-4 h-4 text-[var(--text-secondary)] group-hover:text-white transition-colors" />
							</button>
							<div>
								<div className="text-base font-black text-white italic tracking-tighter">{t('room.title')}</div>
								<div className="flex items-center gap-2 text-[10px] font-bold uppercase tracking-widest mt-0.5">
									<div className={`w-2 h-2 rounded-full ${connected ? 'bg-[var(--success)] animate-pulse shadow-[0_0_10px_rgba(16,185,129,0.5)]' : 'bg-zinc-600'}`} />
									<span className={connected ? 'text-[var(--success)]' : 'text-zinc-600'}>
										{connected ? t('room.signal_ok') : t('room.syncing')}
									</span>
								</div>
							</div>
						</div>
						<button
							onClick={() => setShowReport(true)}
							className="p-2.5 rounded-xl hover:bg-[var(--danger-bg)] transition-colors text-[var(--text-tertiary)] hover:text-[var(--danger)] group border border-transparent hover:border-[var(--danger)]/30"
							title={t('room.report_title')}
						>
							<Flag className="w-4 h-4" />
						</button>
					</div>

					{/* Escrow Panel — Phase 2.3 */}
					<div className="shrink-0 bg-black/20 z-10">
						<EscrowPanel conversationId={conversationId} currentUserId={currentUserId} />
					</div>

					{/* Messages Area */}
					<div className="flex-1 overflow-y-auto px-6 py-6 space-y-2 relative scroll-smooth scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
						{loading && (
							<div className="absolute inset-0 flex flex-col items-center justify-center space-y-4 bg-black/40 backdrop-blur-sm z-20">
								<Loader2 className="w-10 h-10 text-[var(--purple-accent)] animate-spin" />
								<span className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">{t('room.loading')}</span>
							</div>
						)}

						{!loading && messages.length === 0 && (
							<div className="h-full flex flex-col items-center justify-center space-y-4 opacity-60">
								<div className="w-20 h-20 rounded-full border border-dashed border-white/20 flex items-center justify-center">
									<MessageCircle className="w-8 h-8 text-[var(--purple-accent)]/60" />
								</div>
								<p className="text-[var(--text-secondary)] text-sm font-medium">{t('room.empty')}</p>
							</div>
						)}

						{messages.map(renderMessage)}
						<div ref={messagesEndRef} className="h-4" />
					</div>

					{/* Input Area */}
					<div className="p-4 md:p-5 border-t border-white/10 bg-black/60 backdrop-blur-xl shrink-0">
						<div className="flex items-center gap-3">
							<input
								ref={inputRef}
								id="chat-input"
								type="text"
								value={newMessage}
								onChange={(e) => setNewMessage(e.target.value)}
								onKeyDown={handleKeyDown}
								placeholder={t('room.input_placeholder')}
								disabled={!connected}
								className="flex-1 bg-white/[0.03] border border-white/10 hover:border-white/20 rounded-[1.5rem] px-5 py-4 text-sm text-white placeholder:text-zinc-600 focus:outline-none focus:border-[var(--purple-accent)]/50 focus:bg-white/[0.05] transition-all disabled:opacity-50"
							/>
							<button
								id="chat-send-btn"
								onClick={handleSend}
								disabled={!newMessage.trim() || sending || !connected}
								className="p-4 bg-[var(--purple-accent)] hover:bg-[#9333ea] disabled:bg-white/5 rounded-[1.5rem] transition-all disabled:opacity-50 text-white disabled:text-zinc-600 active:scale-95 shadow-[0_0_20px_rgba(168,85,247,0.3)] disabled:shadow-none"
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
