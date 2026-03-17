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

/**
 * EscrowPanel — In-chat escrow UI.
 *
 * Hiển thị:
 * → Trạng thái tổng (total frozen, session status).
 * → Danh sách question items + timer countdown.
 * → Actions: Confirm Release, Reader Reply.
 * → Collapsible — không chiếm quá nhiều không gian.
 */
interface EscrowPanelProps {
 conversationId: string;
 currentUserId: string;
}

export default function EscrowPanel({ conversationId, currentUserId: _currentUserId }: EscrowPanelProps) {
 void _currentUserId;
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
 const getCountdown = (dateStr?: string | null) => {
 if (!dateStr) return null;
 if (!nowTs) return null;
 const diff = new Date(dateStr).getTime() - nowTs;
 if (diff <= 0) return 'Hết hạn';
 const hrs = Math.floor(diff / 3600000);
 const mins = Math.floor((diff % 3600000) / 60000);
 return `${hrs}h ${mins}m`;
 };

 /** Status badge */
 const getStatusBadge = (status: string) => {
 switch (status) {
 case 'accepted':
 return {
 text: 'Đang escrow',
 className: 'bg-[var(--warning)]/10 text-[var(--warning)] border-[var(--warning)]/20',
 icon: <Shield className="w-3 h-3" />
 };
 case 'released':
 return {
 text: 'Đã release',
 className: 'bg-[var(--success)]/10 text-[var(--success)] border-[var(--success)]/20',
 icon: <CheckCircle2 className="w-3 h-3" />
 };
 case 'refunded':
 return {
 text: 'Đã refund',
 className: 'bg-[var(--info)]/10 text-[var(--info)] border-[var(--info)]/20',
 icon: <CheckCircle2 className="w-3 h-3" />
 };
 case 'disputed':
 return {
 text: 'Tranh chấp',
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
 <div className="mx-6 mb-2">
 {/* Header — luôn hiện */}
 <button
 onClick={() => setExpanded(!expanded)}
 className="w-full flex items-center justify-between px-4 py-3 bg-[var(--warning)]/5 hover:bg-[var(--warning)]/10 border border-[var(--warning)]/10 rounded-xl transition-all"
 >
 <div className="flex items-center gap-2">
 <Shield className="w-4 h-4 text-[var(--warning)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--warning)]">
 Escrow
 </span>
 <span className="text-xs font-bold tn-text-primary">
 {escrow.totalFrozen} 💎
 </span>
 {hasActiveItems && (
 <span className="px-1.5 py-0.5 rounded-full bg-[var(--warning)]/20 text-[8px] font-bold text-[var(--warning)] animate-pulse">
 ACTIVE
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
 {item.type === 'main_question' ? 'Chính' : 'Bổ sung'}
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
 <span>Hạn reply: {getCountdown(item.autoRefundAt)}</span>
 </div>
 )}
 {item.autoReleaseAt && item.repliedAt && (
 <div className="flex items-center gap-1 text-[var(--success)]">
 <Clock className="w-3 h-3" />
 <span>Auto-release: {getCountdown(item.autoReleaseAt)}</span>
 </div>
 )}
 </div>
 )}

 {/* Dispute window */}
 {item.disputeWindowEnd && item.status === 'released' && (
 <div className="flex items-center gap-1 text-[10px] text-[var(--warning)]">
 <AlertTriangle className="w-3 h-3" />
 <span>Dispute window: {getCountdown(item.disputeWindowEnd)}</span>
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
 className="flex-1 flex items-center justify-center gap-1 px-3 py-2 rounded-lg bg-[var(--success)]/20 hover:bg-[var(--success)]/30 border border-[var(--success)]/20 text-[var(--success)] text-[10px] font-bold uppercase tracking-wider transition-all disabled:opacity-50"
 >
 {actionLoading === item.id
 ? <Loader2 className="w-3 h-3 animate-spin" />
 : <CheckCircle2 className="w-3 h-3" />}
 Xác nhận Release
 </button>
 )}

 {/* Reader: Reply — khi chưa reply */}
 {!item.repliedAt && (
 <button
 onClick={() => handleReaderReply(item.id)}
 disabled={actionLoading === item.id}
 className="flex-1 flex items-center justify-center gap-1 px-3 py-2 rounded-lg bg-[var(--purple-accent)]/20 hover:bg-[var(--purple-accent)]/30 border border-[var(--purple-accent)]/20 text-[var(--purple-accent)] text-[10px] font-bold uppercase tracking-wider transition-all disabled:opacity-50"
 >
 {actionLoading === item.id
 ? <Loader2 className="w-3 h-3 animate-spin" />
 : <CheckCircle2 className="w-3 h-3" />}
 Đã trả lời
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
