'use client';

import React, { useEffect, useState, useRef, useCallback } from 'react';
import { listMessages, getSignalRToken, type ChatMessageDto } from '@/actions/chatActions';
import {
	Send, Loader2, ArrowLeft, Flag, MessageCircle, Coins, Check, X
} from 'lucide-react';
import { useParams } from 'next/navigation';
import Image from 'next/image';
import { useRouter } from '@/i18n/routing';
import type { HubConnection } from '@microsoft/signalr';
import ReportModal from '@/components/chat/ReportModal';
import EscrowPanel from '@/components/chat/EscrowPanel';
import PaymentOfferModal from '@/components/chat/PaymentOfferModal';
import { GlassCard } from '@/components/ui';
import { useLocale, useTranslations } from 'next-intl';
import toast from 'react-hot-toast';
import { acceptOffer } from '@/actions/escrowActions';
import { API_ORIGIN } from '@/lib/api';

/*
 * ===================================================================
 * FILE: chat/[id]/page.tsx (Phòng Chat Realtime)
 * BỐI CẢNH (CONTEXT):
 *   Giao diện nhắn tin trực tiếp giữa User và Reader.
 * 
 * KIẾN TRÚC & GIAO TIẾP:
 *   - Sử dụng WebSockets qua thư viện `@microsoft/signalr` kết nối tới backend `.NET`.
 *   - Authenticate SignalR bằng token sinh ra từ Server Action `getSignalRToken` 
 *     để không lộ JWT Cookie ra phía Client JavaScript.
 *   - Fetch lịch sử tin nhắn ban đầu qua REST (`listMessages`), sau đó nghe event `ReceiveMessage`.
 *   - Tích hợp tính năng thanh toán trong Chat (EscrowPanel).
 * ===================================================================
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
	const [showPaymentOffer, setShowPaymentOffer] = useState(false); // [State] Modal Đề Xuất Thanh Toán
	const [processingOfferId, setProcessingOfferId] = useState<string | null>(null); // State đang xử lý nút Accept
	
	// [YÊU CẦU MỚI]: Thông tin Header Navbar
	const [otherName, setOtherName] = useState<string>('');
	const [otherAvatar, setOtherAvatar] = useState<string | null>(null);
	const [isUserRole, setIsUserRole] = useState<boolean | null>(null);

	// Refs
	const connectionRef = useRef<HubConnection | null>(null);
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

		let hubConnection: HubConnection | null = null;

		const initConnection = async () => {
			// 1. Lấy token từ server action
			const token = await getSignalRToken();
			if (!token) {
				setLoading(false);
				return;
			}

			let parsedUserId = '';
			// Parse userId từ JWT (base64 decode payload)
			try {
				const payload = JSON.parse(atob(token.split('.')[1]));
				const userId = payload.sub || payload.nameid || payload['http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier'];
				if (userId) {
					parsedUserId = userId;
					setCurrentUserId(userId);
				}
			} catch { /* ignore parse errors */ }

			// 2. Tạo SignalR connection
			const signalR = await import('@microsoft/signalr');
			const apiUrl = API_ORIGIN;
			hubConnection = new signalR.HubConnectionBuilder()
				.withUrl(`${apiUrl}/api/v1/chat`, {
					accessTokenFactory: () => token,
				})
				.withAutomaticReconnect([0, 2000, 5000, 10000, 30000])
				.configureLogging(signalR.LogLevel.Warning)
				.build();

			// 3. Đăng ký event handlers
			hubConnection.on('ReceiveMessage', (message: ChatMessageDto) => {
				setMessages(prev => {
					// Ngăn chặn lỗi trùng lặp React Key (Encountered two children with the same key) bằng cách 
					// kiểm tra xem tin nhắn đã tồn tại trong mảng state cục bộ hay chưa.
					// Có thể xảy ra do độ trễ mạng hoặc SignalR đẩy tin nhắn cũ xuống lại.
					if (prev.some(m => m.id === message.id)) return prev;
					return [...prev, message];
				});
				scrollToBottom();
			});

			hubConnection.on('UserJoined', (data: { userId: string }) => {
				void data.userId;
			});

			hubConnection.on('MessagesRead', (data: { userId: string }) => {
				void data.userId;
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
				// Cập nhật Header Tên/Avatar
				if (history.conversation && parsedUserId) {
					const isUser = parsedUserId === history.conversation.userId;
					const name = isUser 
						? (history.conversation.readerName || `${t('inbox.list_label_reader')} ${history.conversation.readerId.substring(0, 8)}`)
						: (history.conversation.userName || `${t('inbox.list_label_user')} ${history.conversation.userId.substring(0, 8)}`);
					const avatar = isUser ? history.conversation.readerAvatar : history.conversation.userAvatar;
					
					setOtherName(name);
					if (avatar) setOtherAvatar(avatar);
					setIsUserRole(isUser); // [Thayđổi]: Lưu lại role của cửa sổ hiện tại để gửi xuống cho panel tiền tệ
				}

				// Cần hợp nhất với tin nhắn có khả năng vừa được nhét vào từ WebSocket (tránh mất tin hoặc trùng lặp)
				// Sử dụng callback của setMessages để đọc state mới nhất.
				setMessages(prev => {
					const fetchedMessages = [...history.messages].reverse();
					const fetchedIds = new Set(fetchedMessages.map(m => m.id));
					
					// Giữ lại những tin nhắn mới đến từ Socket mà chưa có trong lịch sử REST
					const socketMessages = prev.filter(m => !fetchedIds.has(m.id));
					
					return [...fetchedMessages, ...socketMessages];
				});
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
	}, [conversationId, scrollToBottom, t]);

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

	// Xử lý Gửi Payment Offer (Chỉ dành cho Reader)
	const handleSendPaymentOffer = async (amount: number, note: string) => {
		if (!connectionRef.current || !connected) return;
		try {
			// Đóng gói thông tin Payload theo chuẩn của Backend
			const payload = { amountDiamond: amount, description: note };
			const content = JSON.stringify(payload);
			
			// Bắn lệnh qua WebSocket với thẻ "payment_offer"
			await connectionRef.current.invoke('SendMessage', conversationId, content, 'payment_offer');
		} catch (error) {
			console.error('[Chat] Gửi Payment Offer thất bại:', error);
		}
	};

	// Xử lý User Bấm Chấp Nhận (Chỉ dành cho User)
	const handleAcceptOffer = async (msg: ChatMessageDto) => {
		if (!msg.paymentPayload || !connected || !connectionRef.current) return;
		setProcessingOfferId(msg.id);
		
		try {
			// Gọi REST API Trừ Tiền Ký Quỹ
			const payload = {
				readerId: msg.senderId as string,
				conversationRef: conversationId,
				amountDiamond: msg.paymentPayload.amountDiamond,
				proposalMessageRef: msg.id,
				idempotencyKey: crypto.randomUUID()
			};
			
			const res = await acceptOffer(payload);
			if (res && res.success) {
				// Thông báo cho Reader biết mình đã thanh toán bằng cách ném lên 1 cục SignalR "payment_accept"
				const acceptPayload = JSON.stringify({ offerId: msg.id });
				await connectionRef.current.invoke('SendMessage', conversationId, acceptPayload, 'payment_accept');
				
				// Optional: Tự update UI hoặc refetch 
			} else {
				toast.error('Không đủ số dư Kim Cương hoặc đã có lỗi xảy ra.');
			}
		} catch (error) {
			console.error('[Chat] Chấp nhận Payment Offer thất bại:', error);
			toast.error('Không thể thực hiện thanh toán.');
		} finally {
			setProcessingOfferId(null);
		}
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
			// Kiểm tra xem message này ĐÃ bị thanh toán (có message accept nào chứa offerId này đi sau không).
			const isAccepted = messages.some(m => m.type === 'payment_accept' && m.content.includes(msg.id));
			const isProcessing = processingOfferId === msg.id;

			// Xoay xở nội dung đẹp mắt cho từng type (thay vì hiện nguyên cục JSON)
			let displayContent = msg.content;
			let displayDiamond = msg.paymentPayload?.amountDiamond;
			
			// payment_offer: Backend giờ lưu content = description (Lời nhắn thuần văn bản)
			// Không cần parse JSON nữa — content chính là lời nhắn gốc.
			if (msg.type === 'payment_offer') {
				displayContent = msg.content;
				if (!displayDiamond && msg.paymentPayload?.amountDiamond) displayDiamond = msg.paymentPayload.amountDiamond;
			}
			
			// payment_accept / payment_reject: content là JSON {"offerId":"..."}
			// Tìm ngược lại tin nhắn gốc để lấy nội dung Lời Nhắn và Số Kim Cương
			if (msg.type === 'payment_accept' || msg.type === 'payment_reject') {
				const fallbackText = msg.type === 'payment_accept' ? 'Xác nhận thanh toán thành công' : 'Đã từ chối đề xuất';
				try {
					const refData = JSON.parse(msg.content);
					if (refData.offerId) {
						const originalOffer = messages.find(m => m.id === refData.offerId);
						if (originalOffer) {
							if (originalOffer.paymentPayload) displayDiamond = originalOffer.paymentPayload.amountDiamond;
							displayContent = originalOffer.content || fallbackText;
						} else {
							displayContent = fallbackText;
						}
					}
				} catch {
					// format cũ hoặc lỗi JSON
				}
			}

			// Tùy biến màu sắc theo từng loại
			const isAcceptMsg = msg.type === 'payment_accept';
			const isRejectMsg = msg.type === 'payment_reject';
			const containerBg = isAcceptMsg 
				? 'bg-[var(--success)]/10 border-[var(--success)]/30 shadow-[0_4px_20px_rgba(16,185,129,0.05)]' 
				: isRejectMsg 
					? 'bg-[var(--danger)]/10 border-[var(--danger)]/30 shadow-[0_4px_20px_rgba(239,68,68,0.05)]'
					: 'bg-[var(--warning-bg)] border-[var(--warning)]/30 shadow-[0_4px_20px_rgba(245,158,11,0.1)]';
			const textColor = isAcceptMsg ? 'text-[var(--success)]' : isRejectMsg ? 'text-[var(--danger)]' : 'text-[var(--warning)]';

			return (
				<div key={msg.id} className={`flex ${isMe ? 'justify-end' : 'justify-start'} py-2`}>
					<div className={`max-w-[85%] min-w-[200px] p-4 rounded-2xl border space-y-2 ${containerBg}`}>
						{/* Nhãn loại tin nhắn (Tag nhỏ) */}
						<div className={`text-[9px] font-black uppercase tracking-widest ${isAcceptMsg ? 'text-[var(--success)]' : msg.type === 'payment_offer' ? 'text-[var(--warning)]' : 'text-[var(--danger)]'}`}>
							{msg.type === 'payment_offer' ? t('room.payment_offer') :
								msg.type === 'payment_accept' ? t('room.payment_accept') : t('room.payment_reject')}
						</div>
						
						{/* Nội dung Lời Nhắn — chỉ hiện nếu có ghi chú */}
						{displayContent && displayContent.trim() !== '' && (
							<p className="text-sm text-white font-medium leading-relaxed">{displayContent}</p>
						)}
						
						{displayDiamond && (
							<div className="flex items-center justify-between mt-2 pt-2 border-t border-white/5">
								<div className={`text-sm font-black tracking-wider ${textColor}`}>
									{displayDiamond} 💎
								</div>
								
								{/* [Tạo Mới]: NÚT CHẤP NHẬN CHO KHÁCH */}
								{isUserRole === true && msg.type === 'payment_offer' && (
									<div className="flex items-center gap-2">
										<button 
											onClick={() => handleAcceptOffer(msg)}
											disabled={isProcessing || isAccepted}
											className="flex items-center justify-center gap-1 bg-[var(--success)]/20 hover:bg-[var(--success)]/30 border border-[var(--success)]/30 text-[var(--success)] font-bold text-[10px] uppercase tracking-widest px-3 py-1.5 rounded-lg transition-all active:scale-95 disabled:opacity-50"
										>
											{isProcessing ? <Loader2 className="w-3 h-3 animate-spin" /> : <Check className="w-3 h-3" />}
											{isAccepted ? 'Đã Nhận' : 'Chấp Nhận'}
										</button>
										{!isAccepted && (
											<button 
												onClick={async () => {
													if (!connectionRef.current || !connected) return;
													try {
														const rejectPayload = JSON.stringify({ offerId: msg.id });
														await connectionRef.current.invoke('SendMessage', conversationId, rejectPayload, 'payment_reject');
													} catch (err) {
														console.error('[Chat] Reject failed:', err);
													}
												}}
												className="flex items-center justify-center gap-1 px-3 py-1.5 bg-[var(--danger)]/10 hover:bg-[var(--danger)]/20 text-[var(--danger)] rounded-lg transition-all active:scale-95 border border-[var(--danger)]/20 text-[10px] font-bold uppercase tracking-widest"
											>
												<X className="w-3 h-3" />
												Từ Chối
											</button>
										)}
									</div>
								)}
							</div>
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
			<div className="max-w-2xl mx-auto px-3 sm:px-4 md:px-0 pt-4 md:pt-6 pb-4 md:pb-6 h-[calc(100vh-6rem)] min-h-[32rem] flex flex-col w-full animate-in fade-in ease-out duration-700">
				<GlassCard className="flex flex-col flex-1 overflow-hidden !p-0 border-white/10 shadow-2xl relative">

					{/* Header */}
					<div className="flex items-center justify-between px-4 sm:px-6 py-3 sm:py-4 border-b border-white/10 bg-black/40 backdrop-blur-md z-30 shrink-0 relative">
						<div className="flex items-center gap-3 sm:gap-4">
							<button
								onClick={() => router.push('/chat')}
								className="p-2 sm:p-3 min-h-10 min-w-10 sm:min-h-11 sm:min-w-11 rounded-xl hover:bg-white/10 transition-colors bg-white/5 group"
							>
								<ArrowLeft className="w-4 h-4 text-[var(--text-secondary)] group-hover:text-white transition-colors" />
							</button>
							<div>
								<div className="flex items-center gap-2 sm:gap-3">
									{/* Avatar nhỏ nhắn trên Header */}
									{otherAvatar ? (
										<Image
											src={otherAvatar}
											alt={otherName}
											width={32}
											height={32}
											unoptimized
											className="w-8 h-8 rounded-full object-cover border border-white/10"
										/>
									) : (
										<div className="w-8 h-8 rounded-full bg-[var(--purple-accent)]/20 border border-[var(--purple-accent)]/30 flex items-center justify-center text-xs font-black text-[var(--purple-accent)]">
											{otherName ? otherName.charAt(0).toUpperCase() : '?'}
										</div>
									)}
									<div className="text-sm sm:text-base font-black text-white italic tracking-tighter">
										{otherName || t('room.title')}
									</div>
								</div>
								<div className="flex items-center gap-1.5 sm:gap-2 text-[9px] sm:text-[10px] font-bold uppercase tracking-widest mt-0.5 ml-10 sm:ml-11">
									<div className={`w-1.5 h-1.5 sm:w-2 sm:h-2 rounded-full ${connected ? 'bg-[var(--success)] animate-pulse shadow-[0_0_10px_rgba(16,185,129,0.5)]' : 'bg-zinc-600'}`} />
									<span className={connected ? 'text-[var(--success)]' : 'text-zinc-600'}>
										{connected ? t('room.signal_ok') : t('room.syncing')}
									</span>
								</div>
							</div>
						</div>
						
						{/* CỤM NÚT TRÊN HEADER (BÊN PHẢI) */}
						<div className="flex items-center gap-2 sm:gap-3">
							
							{/* Bảng Ký quỹ Gọn Trên Header (Dropdown) */}
							<EscrowPanel conversationId={conversationId} currentUserId={currentUserId} isUser={isUserRole} />
							
							<button
								onClick={() => setShowReport(true)}
								className="p-2.5 sm:p-3 min-h-10 min-w-10 sm:min-h-11 sm:min-w-11 flex items-center justify-center rounded-xl hover:bg-[var(--danger-bg)] transition-colors text-[var(--text-tertiary)] hover:text-[var(--danger)] group border border-transparent hover:border-[var(--danger)]/30"
								title={t('room.report_title')}
							>
								<Flag className="w-4 h-4 sm:w-4 sm:h-4" />
							</button>
						</div>
					</div>

					{/* Messages Area */}
					<div className="flex-1 overflow-y-auto px-3 sm:px-6 py-4 sm:py-6 space-y-2 relative scroll-smooth scrollbar-thin scrollbar-thumb-white/10 scrollbar-track-transparent">
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
							{/* Nút Đề xuât thanh toán — Chỉ hiện nếu là Reader (isUserRole = false) */}
							{isUserRole === false && (
								<button
									onClick={() => setShowPaymentOffer(true)}
									disabled={!connected}
									title={t('room.payment_offer')}
									className="p-4 min-w-14 bg-[var(--warning)]/20 hover:bg-[var(--warning)]/30 disabled:bg-white/5 rounded-[1.5rem] transition-all disabled:opacity-50 text-[var(--warning)] disabled:text-zinc-600 active:scale-95"
								>
									<Coins className="w-5 h-5 mx-auto" />
								</button>
							)}
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

				{/* Payment Offer Modal (Chỉ xuất hiện khi Reader thao tác gửi đề nghị) */}
				{showPaymentOffer && (
					<PaymentOfferModal
						onClose={() => setShowPaymentOffer(false)}
						onSubmit={handleSendPaymentOffer}
					/>
				)}
			</div>
	);
}
