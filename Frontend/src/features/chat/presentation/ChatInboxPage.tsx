'use client';

import React from 'react';
import {
	Loader2, Sparkles, ChevronRight, Inbox
} from 'lucide-react';
import Image from 'next/image';
import { Link } from '@/i18n/routing';
import { SectionHeader, GlassCard } from '@/components/ui';
import { useTranslations } from 'next-intl';
import { useChatInboxPage } from '@/features/chat/application/useChatInboxPage';

/*
 * ===================================================================
 * FILE: (user)/chat/page.tsx (Hộp Thư - Inbox)
 * BỐI CẢNH (CONTEXT):
 *   Trang hộp thư chính của User, hiển thị danh sách các phiên Chat (Conversations).
 *   Hỗ trợ hiển thị tab 2 vai trò: "Người hỏi" (User) và "Reader".
 * 
 * KIẾN TRÚC & UI:
 *   - Client Component ('use client') fetch dữ liệu realtime thông qua Server Actions `listConversations`.
 *   - Có Unread Badge (số lượng tin nhắn chưa đọc) và trạng thái thẻ (Pending/Active/Completed).
 * ===================================================================
 */
export default function InboxPage() {
	const t = useTranslations('Chat');
	const { conversations, loading, currentUserId, nowTs } = useChatInboxPage();

	/**
	 * Helper: Tính thời gian relative (vd: "5 phút trước").
	 * Đơn giản hóa — không dùng thư viện bên ngoài.
	 */
	const timeAgo = (dateStr?: string | null) => {
		if (!dateStr) return '';
		if (!nowTs) return '';
		const diff = Math.max(0, nowTs - new Date(dateStr).getTime());
		const mins = Math.floor(diff / 60000);
		if (mins < 1) return t('time.just_now');
		if (mins < 60) return t('time.minutes_ago', { count: mins });
		const hrs = Math.floor(mins / 60);
		if (hrs < 24) return t('time.hours_ago', { count: hrs });
		const days = Math.floor(hrs / 24);
		return t('time.days_ago', { count: days });
	};

	/** Status badge */
	const getStatusLabel = (status: string) => {
		switch (status) {
			case 'active':
				return {
					text: t('inbox.status.active'),
					className: 'bg-[var(--success)]/10 text-[var(--success)] border-[var(--success)]/20'
				};
			case 'pending':
				return {
					text: t('inbox.status.pending'),
					className: 'bg-[var(--warning)]/10 text-[var(--warning)] border-[var(--warning)]/20'
				};
			case 'completed':
				return {
					text: t('inbox.status.completed'),
					className: 'bg-[var(--bg-surface-hover)] text-[var(--text-secondary)] border-[var(--border-subtle)]'
				};
			default:
				return {
					text: status,
					className: 'bg-[var(--bg-surface-hover)] text-[var(--text-secondary)] border-[var(--border-subtle)]'
				};
		}
	};

	/** Escrow Status Helper */
	const getEscrowStatusText = (status: string) => {
		switch (status?.toLowerCase()) {
			case 'accepted': 
			case 'active': 
				return t('escrow.active');
			case 'released': return t('escrow.status_released');
			case 'refunded': return t('escrow.status_refunded');
			case 'disputed': return t('escrow.status_disputed');
			default: return status;
		}
	};

	return (
			<div className="max-w-4xl mx-auto px-4 sm:px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
				{/* Header */}
				<SectionHeader
					tag={t('inbox.tag')}
					tagIcon={<Sparkles className="w-3 h-3 text-[var(--purple-accent)]" />}
					title={t('inbox.title')}
					subtitle={t('inbox.subtitle')}
				/>

				{/* Main Content Area */}
				<div className="grid grid-cols-1 gap-6">

					{/* Role Tabs đã bị gỡ bỏ theo yêu cầu gộp chung hộp thoại */}

					{/* Loading State */}
					{loading && (
						<GlassCard className="h-[40vh] flex flex-col items-center justify-center space-y-4">
							<Loader2 className="w-10 h-10 text-[var(--purple-accent)] animate-spin" />
							<span className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">
								{t('inbox.loading')}
							</span>
						</GlassCard>
					)}

					{/* Empty State */}
					{!loading && conversations.length === 0 && (
						<GlassCard className="h-[40vh] flex flex-col items-center justify-center space-y-4 border-dashed border-white/10">
							<div className="w-16 h-16 rounded-full bg-white/5 flex items-center justify-center mb-2">
								<Inbox className="w-8 h-8 text-[var(--text-tertiary)]" />
							</div>
							<p className="text-[var(--text-secondary)] text-sm font-medium">{t('inbox.empty_title')}</p>
							<p className="text-[10px] text-[var(--text-tertiary)] uppercase tracking-widest">{t('inbox.empty_subtitle')}</p>
						</GlassCard>
					)}

					{/* Conversation List */}
					{!loading && conversations.length > 0 && (
						<div className="space-y-4">
							{conversations.map((conv) => {
								// [YÊU CẦU MỚI]: Xác định dựa vào currentUserId để biết mình đang ở vai trò nào
								const isUser = currentUserId === conv.userId;
								const unread = isUser ? conv.unreadCountUser : conv.unreadCountReader;
								const statusInfo = getStatusLabel(conv.status);
								const otherUserId = isUser ? conv.readerId : conv.userId;
								
								// [YÊU CẦU MỚI]: Hiển thị Tên thay vì ID
								const otherName = isUser 
									? (conv.readerName || `${t('inbox.list_label_reader')} ${otherUserId.substring(0, 8)}`) 
									: (conv.userName || `${t('inbox.list_label_user')} ${otherUserId.substring(0, 8)}`);
								const otherAvatar = isUser ? conv.readerAvatar : conv.userAvatar;

								return (
										<Link
											key={conv.id}
											href={`/chat/${conv.id}`}
											className="w-full group block"
										>
										<GlassCard className="flex flex-col sm:flex-row sm:items-center gap-4 sm:gap-5 !p-4 sm:!p-5 hover:border-[var(--purple-accent)]/30 hover:bg-white/[0.03] transition-all duration-300 text-left">
											{/* Avatar */}
											<div className="relative flex-shrink-0">
												{otherAvatar ? (
													<Image
														src={otherAvatar}
														alt={otherName}
														width={56}
														height={56}
														unoptimized
														className="w-14 h-14 rounded-2xl object-cover"
													/>
												) : (
													<div className="w-14 h-14 rounded-2xl bg-gradient-to-br from-[var(--purple-accent)]/20 to-[var(--purple-accent)]/5 border border-[var(--purple-accent)]/20 flex items-center justify-center text-lg font-black text-[var(--purple-accent)] shadow-[0_0_15px_rgba(168,85,247,0.1)] group-hover:scale-105 transition-transform duration-300">
														{otherName.charAt(0).toUpperCase()}
													</div>
												)}
												{/* Unread Badge */}
												{unread > 0 && (
													<div className="absolute -top-1.5 -right-1.5 w-6 h-6 bg-[var(--danger)] rounded-full flex items-center justify-center text-[10px] font-black text-white shadow-lg border-2 border-[#020108] animate-bounce-subtle">
														{unread > 9 ? '9+' : unread}
													</div>
												)}
											</div>

											{/* Content */}
											<div className="flex-1 min-w-0 space-y-1.5">
												<div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-1.5">
													<div className="flex items-center gap-2">
														<span className="text-base font-black text-white truncate group-hover:text-[var(--purple-accent)] transition-colors">
															{otherName}
														</span>
														{/* Thêm Tag phân biệt vai trò đối phương nếu cần mường tượng (Tùy chọn) */}
														<span className="px-2 py-0.5 rounded uppercase tracking-widest text-[8px] font-bold bg-white/5 text-[var(--text-tertiary)] border border-white/10">
															{isUser ? t('inbox.role_reader') : t('inbox.role_user')}
														</span>
													</div>
													<span className="text-[10px] font-bold text-[var(--text-tertiary)] flex-shrink-0 uppercase tracking-widest">
														{timeAgo(conv.lastMessageAt)}
													</span>
												</div>
												
												<div className="flex items-center justify-between gap-2 mt-1">
													<div className="flex items-center gap-2">
														<span
															className={`px-2.5 py-0.5 rounded-full text-[9px] font-black uppercase tracking-widest border ${statusInfo.className}`}
														>
															{statusInfo.text}
														</span>
													</div>
													
													{/* [MỚI]: Hiển thị Ký Quỹ ở góc phải thẻ bên dưới */}
													{conv.escrowTotalFrozen != null && conv.escrowTotalFrozen > 0 && conv.escrowStatus ? (
														<div className="flex items-center gap-1.5 px-2 py-0.5 sm:px-2.5 sm:py-1 rounded-lg bg-[var(--warning)]/10 border border-[var(--warning)]/20 shadow-[0_4px_10px_rgba(245,158,11,0.05)] text-[9px] sm:text-[10px] font-bold text-[var(--warning)] tracking-widest uppercase">
															<span>{conv.escrowTotalFrozen} 💎</span>
															<span className="w-[1px] h-3 bg-[var(--warning)]/30 mx-0.5" />
															<span>{getEscrowStatusText(conv.escrowStatus)}</span>
														</div>
													) : null}
												</div>
											</div>

											<div className="hidden sm:flex w-10 h-10 rounded-full bg-white/5 items-center justify-center group-hover:bg-[var(--purple-accent)]/10 transition-colors shrink-0">
												<ChevronRight className="w-5 h-5 text-[var(--text-tertiary)] group-hover:text-[var(--purple-accent)] transition-colors" />
											</div>
										</GlassCard>
									</Link>
								);
							})}
						</div>
					)}
				</div>
			</div>
	);
}
