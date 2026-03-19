'use client';

import React, { useEffect, useState, useCallback } from 'react';
import {
 getEscrowStatus, confirmRelease, readerReply,
 type EscrowStatusResult
} from '@/actions/escrowActions';
import {
 Shield, Loader2, CheckCircle2, Clock, AlertTriangle,
 Diamond, ChevronDown, ChevronUp
} from 'lucide-react';
import { useTranslations } from 'next-intl';

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
}

export default function EscrowPanel({ conversationId, currentUserId: _currentUserId }: EscrowPanelProps) {
 void _currentUserId;
 const t = useTranslations("Chat");
 const [escrow, setEscrow] = useState<EscrowStatusResult | null>(null);
 const [loading, setLoading] = useState(true);
 const [expanded, setExpanded] = useState(false);
 const [actionLoading, setActionLoading] = useState<string | null>(null);
 const [nowTs, setNowTs] = useState<number>(0);

 // Fetch escrow status
 const fetchStatus = useCallback(async () => {
 const result = await getEscrowStatus(conversationId);
 setEscrow(result);
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
 const ok = await confirmRelease(itemId);
 if (ok) await fetchStatus();
 setActionLoading(null);
 };

 /** Xử lý reader reply */
 const handleReaderReply = async (itemId: string) => {
 setActionLoading(itemId);
 const ok = await readerReply(itemId);
 if (ok) await fetchStatus();
 setActionLoading(null);
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
 <div className="mx-3 sm:mx-6 mb-2">
 {/* Header — luôn hiện */}
 <button
 onClick={() => setExpanded(!expanded)}
 className="w-full flex items-center justify-between gap-3 px-4 py-3 bg-[var(--warning)]/5 hover:bg-[var(--warning)]/10 border border-[var(--warning)]/10 rounded-xl transition-all text-left min-h-11"
 >
 <div className="flex items-center gap-2 min-w-0">
 <Shield className="w-4 h-4 text-[var(--warning)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--warning)] truncate">
 {t("escrow.title")}
 </span>
 <span className="text-xs font-bold tn-text-primary">
 {escrow.totalFrozen} 💎
 </span>
 {hasActiveItems && (
 <span className="px-1.5 py-0.5 rounded-full bg-[var(--warning)]/20 text-[8px] font-bold text-[var(--warning)] animate-pulse">
 {t("escrow.active")}
 </span>
 )}
 </div>
 {expanded ? <ChevronUp className="w-3 h-3 tn-text-muted" /> : <ChevronDown className="w-3 h-3 tn-text-muted" />}
 </button>

 {/* Expanded content */}
 {expanded && (
 <div className="mt-2 space-y-2 animate-in fade-in slide-in-from-top-2 duration-300">
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
 {/* User: Confirm Release — chỉ khi reader đã reply */}
 {item.repliedAt && (
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

 {/* Reader: Reply — khi chưa reply */}
 {!item.repliedAt && (
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
 </div>
 )}
 </div>
 );
}
