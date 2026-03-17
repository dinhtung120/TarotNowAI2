'use client';

import React, { useEffect, useState } from 'react';
import { resolveDispute } from '@/actions/escrowActions';
import {
  AlertTriangle, Loader2, CheckCircle2, Sparkles,
  Shield, ArrowRight, Diamond
} from 'lucide-react';
import { SectionHeader, GlassCard, Button, Input } from '@/components/ui';
import toast from 'react-hot-toast';

/**
 * Admin Dispute Queue — danh sách disputes cần xử lý.
 *
 * Hiện tại sử dụng escrow status API.
 * Admin resolve: release cho reader hoặc refund cho user.
 */

// Simplified: dùng list từ chat_question_items trực tiếp
// Trong production, tạo riêng admin query.
interface DisputeItem {
  id: string;
  type: string;
  amountDiamond: number;
  status: string;
}

export default function AdminDisputesPage() {
  const [processing, setProcessing] = useState<string | null>(null);
  const [resolved, setResolved] = useState<Set<string>>(new Set());
  const [note, setNote] = useState('');

  const handleResolve = async (itemId: string, action: 'release' | 'refund') => {
    setProcessing(itemId);
    const ok = await resolveDispute({ itemId, action, adminNote: note || undefined });
    if (ok) {
      toast.success(action === 'release' ? 'Đã giải ngân cho Reader thành công' : 'Đã hoàn tiền cho User thành công');
      setResolved(prev => new Set(prev).add(itemId));
    } else {
      toast.error('Xử lý tranh chấp thất bại. Vui lòng kiểm tra lại.');
    }
    setProcessing(null);
    setNote('');
  };

  return (
    <div className="max-w-4xl mx-auto px-6 py-16 space-y-10 animate-in fade-in duration-700">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
        <SectionHeader
            tag="Escrow"
            tagIcon={<Sparkles className="w-3 h-3 text-[var(--danger)]" />}
            title="Dispute Queue"
            subtitle="Xử lý các tranh chấp escrow. Quyết định release cho reader hoặc refund cho user."
            className="mb-0 text-left items-start"
        />
      </div>

      {/* Info Card */}
      <GlassCard className="space-y-4 shadow-inner !p-6">
        <div className="flex items-center gap-3">
          <div className="p-2 rounded-xl bg-[var(--warning)]/10 border border-[var(--warning)]/20 shadow-inner">
            <AlertTriangle className="w-5 h-5 text-[var(--warning)]" />
          </div>
          <span className="text-sm font-black text-white uppercase tracking-widest drop-shadow-sm">Hướng dẫn xử lý Dispute</span>
        </div>
        <ul className="text-xs text-[var(--text-secondary)] space-y-2 ml-10 list-disc font-medium leading-relaxed">
          <li><strong className="text-[var(--success)] uppercase tracking-tighter">Release</strong>: Reader đã trả lời đúng → diamond chuyển cho reader (trừ 10% phí).</li>
          <li><strong className="text-[var(--accent)] uppercase tracking-tighter">Refund</strong>: Reader trả lời sai/không trả lời → diamond hoàn cho user.</li>
          <li>Kiểm tra lịch sử chat trước khi quyết định.</li>
        </ul>
      </GlassCard>

      {/* Dispute Resolution Form */}
      <GlassCard className="space-y-6 !p-8">
        <h3 className="text-sm font-black text-white uppercase tracking-widest flex items-center gap-2">
            <Shield className="w-4 h-4 text-[var(--purple-accent)]" />
            Xử lý nhanh
        </h3>

        <div className="space-y-4">
          <div className="space-y-2">
            <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Item ID (UUID)</label>
            <Input
              id="dispute-item-id"
              placeholder="NHẬP UUID CỦA QUESTION ITEM..."
              className="w-full text-xs font-black tracking-widest text-white shadow-inner"
            />
          </div>

          <div className="space-y-2">
            <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Ghi chú admin</label>
            <textarea
              value={note}
              onChange={e => setNote(e.target.value)}
              placeholder="Lý do quyết định..."
              rows={3}
              className="w-full bg-white/[0.02] border border-white/10 rounded-2xl p-4 text-xs font-bold text-white placeholder:text-zinc-700 focus:outline-none focus:border-[var(--purple-accent)]/30 transition-all resize-none shadow-inner"
            />
          </div>

          <div className="flex flex-col sm:flex-row gap-4 pt-2">
            <Button
              variant="primary"
              onClick={() => {
                const input = document.getElementById('dispute-item-id') as HTMLInputElement;
                if (input?.value) handleResolve(input.value, 'release');
                else toast.error('Vui lòng nhập Item ID');
              }}
              disabled={processing !== null}
              className="flex-1 py-4 bg-[var(--success)] hover:bg-[var(--success)]/90 text-white shadow-[0_0_20px_rgba(16,185,129,0.2)]"
            >
              {processing && document.getElementById('dispute-item-id') && (document.getElementById('dispute-item-id') as HTMLInputElement).value ? <Loader2 className="w-4 h-4 animate-spin" /> : <CheckCircle2 className="w-4 h-4" />}
              Release cho Reader
            </Button>

            <Button
              variant="secondary"
              onClick={() => {
                const input = document.getElementById('dispute-item-id') as HTMLInputElement;
                if (input?.value) handleResolve(input.value, 'refund');
                else toast.error('Vui lòng nhập Item ID');
              }}
              disabled={processing !== null}
              className="flex-1 py-4 border-[var(--accent)]/30 text-[var(--accent)] hover:bg-[var(--accent)]/10"
            >
              {processing && document.getElementById('dispute-item-id') && (document.getElementById('dispute-item-id') as HTMLInputElement).value ? <Loader2 className="w-4 h-4 animate-spin" /> : <ArrowRight className="w-4 h-4" />}
              Refund cho User
            </Button>
          </div>
        </div>
      </GlassCard>

      {/* Resolved list */}
      {resolved.size > 0 && (
        <div className="space-y-4 animate-in fade-in slide-in-from-bottom-4">
          <h3 className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] flex items-center gap-2">
            <CheckCircle2 className="w-3 h-3 text-[var(--success)]" />
            Đã xử lý trong phiên
          </h3>
          <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-3">
            {Array.from(resolved).map(id => (
                <div key={id} className="flex flex-col gap-1 p-4 bg-[var(--success)]/5 border border-[var(--success)]/10 rounded-2xl shadow-inner">
                    <div className="flex items-center gap-2">
                        <CheckCircle2 className="w-4 h-4 text-[var(--success)] drop-shadow-sm" />
                        <span className="text-xs font-black text-[var(--success)] tracking-tighter uppercase drop-shadow-sm">Đã Resolve</span>
                    </div>
                <span className="text-[10px] text-[var(--text-secondary)] font-mono truncate">{id}</span>
                </div>
            ))}
          </div>
        </div>
      )}
    </div>
  );
}
