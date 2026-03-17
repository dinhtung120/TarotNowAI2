'use client';

import React, { useEffect, useState, useCallback } from 'react';
import {
  getEscrowStatus, confirmRelease, readerReply,
  type EscrowStatusResult, type QuestionItemResult
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

export default function EscrowPanel({ conversationId, currentUserId }: EscrowPanelProps) {
  const [escrow, setEscrow] = useState<EscrowStatusResult | null>(null);
  const [loading, setLoading] = useState(true);
  const [expanded, setExpanded] = useState(false);
  const [actionLoading, setActionLoading] = useState<string | null>(null);

  // Fetch escrow status
  const fetchStatus = useCallback(async () => {
    const result = await getEscrowStatus(conversationId);
    setEscrow(result);
    setLoading(false);
  }, [conversationId]);

  useEffect(() => {
    fetchStatus();
    // Poll mỗi 30s để cập nhật timer
    const interval = setInterval(fetchStatus, 30000);
    return () => clearInterval(interval);
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
    const diff = new Date(dateStr).getTime() - Date.now();
    if (diff <= 0) return 'Hết hạn';
    const hrs = Math.floor(diff / 3600000);
    const mins = Math.floor((diff % 3600000) / 60000);
    return `${hrs}h ${mins}m`;
  };

  /** Status badge */
  const getStatusBadge = (status: string) => {
    switch (status) {
      case 'accepted': return { text: 'Đang escrow', color: 'amber', icon: <Shield className="w-3 h-3" /> };
      case 'released': return { text: 'Đã release', color: 'emerald', icon: <CheckCircle2 className="w-3 h-3" /> };
      case 'refunded': return { text: 'Đã refund', color: 'blue', icon: <CheckCircle2 className="w-3 h-3" /> };
      case 'disputed': return { text: 'Tranh chấp', color: 'red', icon: <AlertTriangle className="w-3 h-3" /> };
      default: return { text: status, color: 'zinc', icon: <Clock className="w-3 h-3" /> };
    }
  };

  const hasActiveItems = escrow.items.some(i => i.status === 'accepted');

  return (
    <div className="mx-6 mb-2">
      {/* Header — luôn hiện */}
      <button
        onClick={() => setExpanded(!expanded)}
        className="w-full flex items-center justify-between px-4 py-3 bg-amber-500/5 hover:bg-amber-500/10 border border-amber-500/10 rounded-xl transition-all"
      >
        <div className="flex items-center gap-2">
          <Shield className="w-4 h-4 text-amber-400" />
          <span className="text-[10px] font-black uppercase tracking-widest text-amber-400">
            Escrow
          </span>
          <span className="text-xs font-bold text-white">
            {escrow.totalFrozen} 💎
          </span>
          {hasActiveItems && (
            <span className="px-1.5 py-0.5 rounded-full bg-amber-500/20 text-[8px] font-bold text-amber-400 animate-pulse">
              ACTIVE
            </span>
          )}
        </div>
        {expanded ? <ChevronUp className="w-3 h-3 text-zinc-600" /> : <ChevronDown className="w-3 h-3 text-zinc-600" />}
      </button>

      {/* Expanded content */}
      {expanded && (
        <div className="mt-2 space-y-2 animate-in fade-in slide-in-from-top-2 duration-300">
          {escrow.items.map((item) => {
            const badge = getStatusBadge(item.status);
            const isPayer = true; // simplify — detect from context
            const isAccepted = item.status === 'accepted';

            return (
              <div
                key={item.id}
                className="p-4 bg-white/[0.02] border border-white/5 rounded-xl space-y-3"
              >
                {/* Item header */}
                <div className="flex items-center justify-between">
                  <div className="flex items-center gap-2">
                    <Diamond className="w-3 h-3 text-amber-400" />
                    <span className="text-xs font-bold text-white">{item.amountDiamond} 💎</span>
                    <span className="text-[9px] text-zinc-600 uppercase">
                      {item.type === 'main_question' ? 'Chính' : 'Bổ sung'}
                    </span>
                  </div>
                  <span className={`flex items-center gap-1 px-2 py-0.5 rounded-full bg-${badge.color}-500/10 text-${badge.color}-400 text-[9px] font-bold uppercase border border-${badge.color}-500/20`}>
                    {badge.icon}
                    {badge.text}
                  </span>
                </div>

                {/* Timers */}
                {isAccepted && (
                  <div className="flex gap-3 text-[10px]">
                    {item.autoRefundAt && !item.repliedAt && (
                      <div className="flex items-center gap-1 text-red-400">
                        <Clock className="w-3 h-3" />
                        <span>Hạn reply: {getCountdown(item.autoRefundAt)}</span>
                      </div>
                    )}
                    {item.autoReleaseAt && item.repliedAt && (
                      <div className="flex items-center gap-1 text-emerald-400">
                        <Clock className="w-3 h-3" />
                        <span>Auto-release: {getCountdown(item.autoReleaseAt)}</span>
                      </div>
                    )}
                  </div>
                )}

                {/* Dispute window */}
                {item.disputeWindowEnd && item.status === 'released' && (
                  <div className="flex items-center gap-1 text-[10px] text-orange-400">
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
                        className="flex-1 flex items-center justify-center gap-1 px-3 py-2 rounded-lg bg-emerald-600/20 hover:bg-emerald-600/30 border border-emerald-500/20 text-emerald-400 text-[10px] font-bold uppercase tracking-wider transition-all disabled:opacity-50"
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
                        className="flex-1 flex items-center justify-center gap-1 px-3 py-2 rounded-lg bg-purple-600/20 hover:bg-purple-600/30 border border-purple-500/20 text-purple-400 text-[10px] font-bold uppercase tracking-wider transition-all disabled:opacity-50"
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
