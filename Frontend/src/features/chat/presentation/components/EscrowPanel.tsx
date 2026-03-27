'use client';

import React, { useEffect, useState, useCallback } from 'react';
import {
 getEscrowStatus, confirmRelease, readerReply,
 type EscrowStatusResult
} from '@/features/chat/application/actions/escrow';
import {
 Shield, Loader2, CheckCircle2, Clock, AlertTriangle,
 Diamond
} from 'lucide-react';
import { useTranslations } from 'next-intl';
import toast from 'react-hot-toast';

/*
 * ===================================================================
 * COMPONENT: EscrowPanel
 * BỐI CẢNH (CONTEXT):
 *   Panel (Bảng điều khiển) tích hợp ngay bên trong giao diện Chat, giúp User 
 *   và Reader theo dõi tiến trình giao dịch trung gian (Escrow) của một phiên xem Tarot.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Fetch trạng thái Escrow định kỳ 30s (Polling) qua `getEscrowStatus`.
 *   - Hiển thị tổng số Diamond đang bị Đóng băng (Frozen).
 *   - Hiển thị danh sách câu hỏi (Main/Add-on) cùng trạng thái (Accepted, Released, Refunded...).
 *   - Cung cấp nút thao tác: "Reader Reply" (Dành cho Reader) và "Confirm Release" (Dành cho User).
 *   - Timer đếm ngược (Countdown) cho các deadline Auto-refund hoặc Auto-release.
 * ===================================================================
 */
interface EscrowPanelProps {
 conversationId: string;
 currentUserId: string;
 isUser: boolean | null;
}

export default function EscrowPanel({ conversationId, isUser, currentUserId: _currentUserId }: EscrowPanelProps) {
 void _currentUserId;
 const t = useTranslations("Chat");
 const [escrow, setEscrow] = useState<EscrowStatusResult | null>(null);
 const [loading, setLoading] = useState(true);
 const [expanded, setExpanded] = useState(false);
 const [actionLoading, setActionLoading] = useState<string | null>(null);
 const [confirmingCompleteAll, setConfirmingCompleteAll] = useState(false);
 const [nowTs, setNowTs] = useState<number>(0);

 // Fetch escrow status
 const fetchStatus = useCallback(async () => {
 const result = await getEscrowStatus(conversationId);
 setEscrow(result.success && result.data ? result.data : null);
 setNowTs(Date.now());
 setLoading(false);
 }, [conversationId]);

 useEffect(() => {
 const initialFetchTimer = window.setTimeout(() => {
 void fetchStatus();
 }, 0);
 // Poll mỗi 30s để cập nhật timer
 const interval = window.setInterval(() => {
 void fetchStatus();
 }, 30000);
 return () => {
 window.clearTimeout(initialFetchTimer);
 window.clearInterval(interval);
 };
 }, [fetchStatus]);

 // Không có escrow → ẩn panel
 if (loading) return null;
 if (!escrow) return null;

 /** Xử lý confirm release */
 const handleConfirmRelease = async (itemId: string) => {
 setActionLoading(itemId);
 const result = await confirmRelease(itemId);
 if (result.success) await fetchStatus();
 setActionLoading(null);
 };

 /** Xử lý reader reply */
		const handleReaderReply = async (itemId: string) => {
			setActionLoading(itemId);
			try {
				const result = await readerReply(itemId);
				if (result.success) {
					fetchStatus();
				}
			} finally {
			setActionLoading(null);
		}
	};

	const handleCompleteAll = async () => {
		if (!escrow || !escrow.items) return;
		const activeItems = escrow.items.filter(i => i.status === 'accepted');
		if (activeItems.length === 0) return;

		setActionLoading('ALL');
			try {
				await Promise.all(activeItems.map(item => confirmRelease(item.id)));
				fetchStatus();
				setExpanded(false);
				toast.success(t('escrow.complete_all_success'));
			} catch (error) {
				console.error("Failed to release all items:", error);
				toast.error(t('escrow.error_release_all'));
			} finally {
			setActionLoading(null);
			setConfirmingCompleteAll(false);
		}
	};

 /** Countdown timer helper */
 const getCountdown = (dateStr?: string | null): string => {
 if (!dateStr) return t("escrow.expired");
 if (!nowTs) return t("escrow.countdown", { hours: 0, minutes: 0 });
 const diff = new Date(dateStr).getTime() - nowTs;
 if (diff <= 0) return t("escrow.expired");
 const hrs = Math.floor(diff / 3600000);
 const mins = Math.floor((diff % 3600000) / 60000);
 return t("escrow.countdown", { hours: hrs, minutes: mins });
 };

 /** Status badge */
 const getStatusBadge = (status: string) => {
 switch (status) {
 case 'accepted':
 return {
 text: t("escrow.status_accepted"),
 className: 'bg-[var(--warning)]/10 text-[var(--warning)] border-[var(--warning)]/20',
 icon: <Shield className="w-3 h-3" />
 };
 case 'released':
 return {
 text: t("escrow.status_released"),
 className: 'bg-[var(--success)]/10 text-[var(--success)] border-[var(--success)]/20',
 icon: <CheckCircle2 className="w-3 h-3" />
 };
 case 'refunded':
 return {
 text: t("escrow.status_refunded"),
 className: 'bg-[var(--info)]/10 text-[var(--info)] border-[var(--info)]/20',
 icon: <CheckCircle2 className="w-3 h-3" />
 };
 case 'disputed':
 return {
 text: t("escrow.status_disputed"),
 className: 'bg-[var(--danger)]/10 text-[var(--danger)] border-[var(--danger)]/20',
 icon: <AlertTriangle className="w-3 h-3" />
 };
 default:
 return {
 text: status,
 className: 'tn-panel-soft tn-text-muted tn-border-soft',
 icon: <Clock className="w-3 h-3" />
 };
 }
 };

 const hasActiveItems = escrow.items.some(i => i.status === 'accepted');

 	return (
		<div className="relative">
			{/* Header Badge — luôn hiện */}
			<button
				onClick={() => setExpanded(!expanded)}
				className="flex items-center gap-2 px-3 py-2 sm:px-4 sm:py-2.5 bg-[var(--warning)]/10 hover:bg-[var(--warning)]/20 border border-[var(--warning)]/20 rounded-xl transition-all shadow-[0_4px_10px_rgba(245,158,11,0.05)] active:scale-95"
				title={t("escrow.title")}
			>
				<Diamond className="w-3.5 h-3.5 sm:w-4 sm:h-4 text-[var(--warning)]" />
				<span className="text-xs sm:text-sm font-black text-white tracking-widest">{escrow.totalFrozen}</span>
				
				{hasActiveItems && (
					<div className="flex items-center gap-1.5 ml-1 pl-2.5 sm:ml-2 sm:pl-3 border-l border-[var(--warning)]/20">
						<Clock className="w-3 h-3 text-[var(--warning)] animate-pulse" />
						<span className="text-[10px] font-bold text-[var(--warning)] tracking-widest uppercase">
							{t("escrow.active")}
						</span>
					</div>
				)}
			</button>

			{/* Expanded Dropdown content */}
			{expanded && (
				<div className="absolute right-0 top-full mt-3 w-[280px] sm:w-[320px] bg-[#1A1A1A] border border-white/10 rounded-2xl shadow-2xl p-4 space-y-3 z-50 animate-in fade-in slide-in-from-top-2 duration-200">
					{/* Tiêu đề Dropdown */}
					<div className="flex items-center justify-between pb-3 border-b border-white/5">
						<div className="flex items-center gap-2 text-[var(--warning)]">
							<Shield className="w-4 h-4" />
							<span className="text-[10px] font-black uppercase tracking-widest">{t("escrow.title")}</span>
						</div>
						<span className="text-xs font-bold text-white uppercase tracking-widest">{t("escrow.total_frozen")}: {escrow.totalFrozen} 💎</span>
					</div>
 {escrow.items.map((item) => {
 const badge = getStatusBadge(item.status);
 const isAccepted = item.status === 'accepted';

 return (
 <div
 key={item.id}
 className="p-4 tn-panel-soft rounded-xl space-y-3"
 >
 {/* Item header */}
 <div className="flex items-center justify-between">
 <div className="flex items-center gap-2">
 <Diamond className="w-3 h-3 text-[var(--warning)]" />
 <span className="text-xs font-bold tn-text-primary">{item.amountDiamond} 💎</span>
 <span className="text-[9px] tn-text-muted uppercase">
 {item.type === 'main_question' ? t("escrow.type_main") : t("escrow.type_addon")}
 </span>
 </div>
 <span className={`flex items-center gap-1 px-2 py-0.5 rounded-full text-[9px] font-bold uppercase border ${badge.className}`}>
 {badge.icon}
 {badge.text}
 </span>
 </div>

 {/* Timers */}
 {isAccepted && (
 <div className="flex gap-3 text-[10px]">
 {item.autoRefundAt && !item.repliedAt && (
 <div className="flex items-center gap-1 text-[var(--danger)]">
 <Clock className="w-3 h-3" />
 <span>{t("escrow.deadline_reply", { time: getCountdown(item.autoRefundAt) })}</span>
 </div>
 )}
 {item.autoReleaseAt && item.repliedAt && (
 <div className="flex items-center gap-1 text-[var(--success)]">
 <Clock className="w-3 h-3" />
 <span>{t("escrow.auto_release", { time: getCountdown(item.autoReleaseAt) })}</span>
 </div>
 )}
 </div>
 )}

 {/* Dispute window */}
 {item.disputeWindowEnd && item.status === 'released' && (
 <div className="flex items-center gap-1 text-[10px] text-[var(--warning)]">
 <AlertTriangle className="w-3 h-3" />
 <span>{t("escrow.dispute_window", { time: getCountdown(item.disputeWindowEnd) })}</span>
 </div>
 )}

 {/* Action buttons */}
 {isAccepted && (
 <div className="flex gap-2">
 {/* User: Confirm Release — hiện mọi lúc khi isUser = true */}
 {isUser && (
 <button
 onClick={() => handleConfirmRelease(item.id)}
 disabled={actionLoading === item.id}
 className="flex-1 flex items-center justify-center gap-1 px-3 py-2.5 rounded-lg bg-[var(--success)]/20 hover:bg-[var(--success)]/30 border border-[var(--success)]/20 text-[var(--success)] text-[10px] font-bold uppercase tracking-wider transition-all disabled:opacity-50 min-h-11"
 >
 {actionLoading === item.id
 ? <Loader2 className="w-3 h-3 animate-spin" />
 : <CheckCircle2 className="w-3 h-3" />}
 {t("escrow.action_confirm_release")}
 </button>
 )}

 {/* Reader: Reply — cho reader khi chưa reply */}
 {isUser === false && !item.repliedAt && (
 <button
 onClick={() => handleReaderReply(item.id)}
 disabled={actionLoading === item.id}
 className="flex-1 flex items-center justify-center gap-1 px-3 py-2.5 rounded-lg bg-[var(--purple-accent)]/20 hover:bg-[var(--purple-accent)]/30 border border-[var(--purple-accent)]/20 text-[var(--purple-accent)] text-[10px] font-bold uppercase tracking-wider transition-all disabled:opacity-50 min-h-11"
 >
 {actionLoading === item.id
 ? <Loader2 className="w-3 h-3 animate-spin" />
 : <CheckCircle2 className="w-3 h-3" />}
 {t("escrow.action_replied")}
 </button>
 )}
 </div>
 )}
 </div>
 );
 })}

						{/* NÚT Hoàn Thành Cuộc Trò Chuyện (Release All) Dành Cho User */}
						{isUser && hasActiveItems && (
							<div className="pt-2 border-t border-white/10 mt-2">
								<button
									onClick={() => {
										if (confirmingCompleteAll) {
											void handleCompleteAll();
											return;
										}
										setConfirmingCompleteAll(true);
									}}
									disabled={actionLoading === 'ALL'}
									className="w-full flex items-center justify-center gap-2 px-4 py-3 bg-[var(--success)] shadow-[0_0_20px_var(--c-16-185-129-20)] hover:bg-emerald-400 text-black rounded-xl font-bold uppercase tracking-widest text-xs transition-all active:scale-95 disabled:opacity-50"
								>
									{actionLoading === 'ALL' ? (
										<Loader2 className="w-4 h-4 animate-spin" />
									) : (
										<CheckCircle2 className="w-4 h-4" />
									)}
										{confirmingCompleteAll
											? t('escrow.confirm_release_all_cta')
											: t('escrow.action_complete_all')}
									</button>
								{confirmingCompleteAll && actionLoading !== 'ALL' && (
									<div className="mt-2 flex items-center gap-2">
											<button
												onClick={() => setConfirmingCompleteAll(false)}
												className="flex-1 min-h-11 px-3 py-2 rounded-lg tn-panel-soft text-[10px] font-bold uppercase tracking-widest text-[var(--text-secondary)] hover:tn-text-primary transition-colors"
											>
												{t('escrow.confirm_cancel')}
											</button>
											<button
												onClick={() => void handleCompleteAll()}
												className="flex-1 min-h-11 px-3 py-2 rounded-lg bg-[var(--success)]/20 border border-[var(--success)]/20 text-[var(--success)] text-[10px] font-bold uppercase tracking-widest hover:bg-[var(--success)]/30 transition-colors"
											>
												{t('escrow.confirm_ok')}
											</button>
										</div>
									)}
									<p className="text-[9px] text-center text-[var(--success)] mt-2 italic px-2">
										{t('escrow.complete_all_hint')}
									</p>
							</div>
						)}
					</div>
				)}
		</div>
	);
}
