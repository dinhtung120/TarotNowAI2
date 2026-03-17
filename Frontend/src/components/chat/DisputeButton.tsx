'use client';

import React, { useState } from 'react';
import { openDispute } from '@/actions/escrowActions';
import { AlertTriangle, Loader2, CheckCircle2, X } from 'lucide-react';

/**
 * DisputeButton — CTA mở tranh chấp + window countdown.
 *
 * Hiển thị khi:
 * → Item status = 'released'
 * → Dispute window chưa hết (disputeWindowEnd > now)
 *
 * Click → modal nhập lý do → submit.
 */
interface DisputeButtonProps {
  itemId: string;
  disputeWindowEnd: string;
  onDisputeOpened?: () => void;
}

export default function DisputeButton({ itemId, disputeWindowEnd, onDisputeOpened }: DisputeButtonProps) {
  const [showModal, setShowModal] = useState(false);
  const [reason, setReason] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState('');

  // Kiểm tra còn trong window không
  const timeLeft = new Date(disputeWindowEnd).getTime() - Date.now();
  if (timeLeft <= 0) return null;

  const hrs = Math.floor(timeLeft / 3600000);
  const mins = Math.floor((timeLeft % 3600000) / 60000);

  const handleSubmit = async () => {
    if (reason.length < 10) {
      setError('Lý do phải có ít nhất 10 ký tự.');
      return;
    }
    setSubmitting(true);
    setError('');
    const ok = await openDispute(itemId, reason);
    if (ok) {
      setSuccess(true);
      onDisputeOpened?.();
    } else {
      setError('Gửi tranh chấp thất bại.');
    }
    setSubmitting(false);
  };

  return (
    <>
      {/* CTA Button */}
      <button
        onClick={() => setShowModal(true)}
        className="flex items-center gap-1 px-3 py-1.5 rounded-lg bg-red-500/10 hover:bg-red-500/20 border border-red-500/20 text-red-400 text-[9px] font-bold uppercase tracking-wider transition-all"
      >
        <AlertTriangle className="w-3 h-3" />
        Tranh chấp ({hrs}h {mins}m)
      </button>

      {/* Modal */}
      {showModal && (
        <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm"
             onClick={() => setShowModal(false)}>
          <div className="bg-zinc-900 rounded-3xl border border-white/10 p-8 max-w-md mx-4 w-full space-y-5"
               onClick={e => e.stopPropagation()}>

            {success ? (
              <div className="text-center space-y-3">
                <CheckCircle2 className="w-10 h-10 text-emerald-400 mx-auto" />
                <h3 className="text-sm font-black text-white uppercase">Đã gửi tranh chấp</h3>
                <p className="text-[10px] text-zinc-500">Admin sẽ xem xét và xử lý.</p>
                <button onClick={() => setShowModal(false)}
                  className="px-5 py-2 rounded-xl bg-purple-600 text-white text-xs font-bold uppercase">
                  Đóng
                </button>
              </div>
            ) : (
              <>
                <div className="flex items-center justify-between">
                  <div className="flex items-center gap-2">
                    <AlertTriangle className="w-4 h-4 text-red-400" />
                    <h3 className="text-sm font-black text-white uppercase tracking-widest">Mở tranh chấp</h3>
                  </div>
                  <button onClick={() => setShowModal(false)} className="p-1 hover:bg-white/[0.04] rounded-lg">
                    <X className="w-4 h-4 text-zinc-500" />
                  </button>
                </div>

                <div className="p-3 rounded-lg bg-red-500/5 border border-red-500/10 text-[10px] text-red-400">
                  ⏰ Còn lại: {hrs} giờ {mins} phút để mở tranh chấp
                </div>

                <div className="space-y-2">
                  <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Lý do *</label>
                  <textarea
                    value={reason}
                    onChange={e => setReason(e.target.value)}
                    placeholder="Mô tả chi tiết lý do (tối thiểu 10 ký tự)..."
                    rows={3}
                    className="w-full bg-white/[0.02] border border-white/10 rounded-xl p-3 text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-red-500/30 transition-all resize-none"
                  />
                </div>

                {error && <div className="text-xs text-red-400">{error}</div>}

                <button
                  onClick={handleSubmit}
                  disabled={submitting || reason.length < 10}
                  className="w-full flex items-center justify-center gap-2 p-3 rounded-xl bg-red-600/20 hover:bg-red-600/30 border border-red-500/20 text-red-400 text-[10px] font-black uppercase tracking-widest disabled:opacity-50"
                >
                  {submitting ? <Loader2 className="w-3 h-3 animate-spin" /> : <AlertTriangle className="w-3 h-3" />}
                  Gửi Tranh Chấp
                </button>
              </>
            )}
          </div>
        </div>
      )}
    </>
  );
}
