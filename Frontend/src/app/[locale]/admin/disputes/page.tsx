'use client';

import React, { useEffect, useState } from 'react';
import { resolveDispute } from '@/actions/escrowActions';
import {
  AlertTriangle, Loader2, CheckCircle2, Sparkles,
  Shield, ArrowRight, Diamond
} from 'lucide-react';

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
      setResolved(prev => new Set(prev).add(itemId));
    }
    setProcessing(null);
    setNote('');
  };

  return (
    <div className="max-w-4xl mx-auto px-6 py-16 space-y-10 animate-in fade-in slide-in-from-bottom-8 duration-1000">
      {/* Header */}
      <div className="space-y-4">
        <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-red-500/5 border border-red-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-red-400 shadow-xl backdrop-blur-md">
          <Sparkles className="w-3 h-3" />
          Admin
        </div>
        <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic">
          Dispute Queue
        </h1>
        <p className="text-zinc-500 font-medium text-sm">
          Xử lý các tranh chấp escrow. Quyết định release cho reader hoặc refund cho user.
        </p>
      </div>

      {/* Info Card */}
      <div className="p-6 bg-white/[0.02] border border-white/5 rounded-2xl space-y-3">
        <div className="flex items-center gap-2">
          <AlertTriangle className="w-4 h-4 text-amber-400" />
          <span className="text-xs font-bold text-white">Hướng dẫn xử lý Dispute</span>
        </div>
        <ul className="text-[10px] text-zinc-500 space-y-1 ml-6 list-disc">
          <li><strong className="text-emerald-400">Release</strong>: Reader đã trả lời đúng → diamond chuyển cho reader (trừ 10% phí).</li>
          <li><strong className="text-blue-400">Refund</strong>: Reader trả lời sai/không trả lời → diamond hoàn cho user.</li>
          <li>Kiểm tra lịch sử chat trước khi quyết định.</li>
        </ul>
      </div>

      {/* Dispute Resolution Form */}
      <div className="p-6 bg-white/[0.02] border border-white/5 rounded-2xl space-y-4">
        <h3 className="text-sm font-black text-white uppercase tracking-widest">Xử lý nhanh</h3>

        <div className="space-y-3">
          <div className="space-y-1">
            <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Item ID (UUID)</label>
            <input
              id="dispute-item-id"
              type="text"
              placeholder="Nhập UUID của question item..."
              className="w-full bg-white/[0.02] border border-white/10 rounded-xl px-4 py-3 text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-red-500/30 transition-all"
            />
          </div>

          <div className="space-y-1">
            <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Ghi chú admin</label>
            <textarea
              value={note}
              onChange={e => setNote(e.target.value)}
              placeholder="Lý do quyết định..."
              rows={2}
              className="w-full bg-white/[0.02] border border-white/10 rounded-xl p-3 text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-white/20 transition-all resize-none"
            />
          </div>

          <div className="flex gap-3">
            <button
              onClick={() => {
                const input = document.getElementById('dispute-item-id') as HTMLInputElement;
                if (input?.value) handleResolve(input.value, 'release');
              }}
              disabled={processing !== null}
              className="flex-1 flex items-center justify-center gap-2 p-3 rounded-xl bg-emerald-600/20 hover:bg-emerald-600/30 border border-emerald-500/20 text-emerald-400 text-[10px] font-black uppercase tracking-widest disabled:opacity-50 transition-all"
            >
              {processing ? <Loader2 className="w-3 h-3 animate-spin" /> : <CheckCircle2 className="w-3 h-3" />}
              Release cho Reader
            </button>

            <button
              onClick={() => {
                const input = document.getElementById('dispute-item-id') as HTMLInputElement;
                if (input?.value) handleResolve(input.value, 'refund');
              }}
              disabled={processing !== null}
              className="flex-1 flex items-center justify-center gap-2 p-3 rounded-xl bg-blue-600/20 hover:bg-blue-600/30 border border-blue-500/20 text-blue-400 text-[10px] font-black uppercase tracking-widest disabled:opacity-50 transition-all"
            >
              {processing ? <Loader2 className="w-3 h-3 animate-spin" /> : <ArrowRight className="w-3 h-3" />}
              Refund cho User
            </button>
          </div>
        </div>
      </div>

      {/* Resolved list */}
      {resolved.size > 0 && (
        <div className="space-y-2">
          <h3 className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Đã xử lý</h3>
          {Array.from(resolved).map(id => (
            <div key={id} className="flex items-center gap-2 p-3 bg-emerald-500/5 border border-emerald-500/10 rounded-xl">
              <CheckCircle2 className="w-3 h-3 text-emerald-400" />
              <span className="text-xs text-emerald-400 font-mono">{id.substring(0, 8)}...</span>
              <span className="text-[9px] text-zinc-600">Đã resolve</span>
            </div>
          ))}
        </div>
      )}
    </div>
  );
}
