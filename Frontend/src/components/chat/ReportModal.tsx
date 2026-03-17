'use client';

import React, { useState } from 'react';
import { sendReport } from '@/actions/chatActions';
import { X, Flag, Loader2, CheckCircle2 } from 'lucide-react';

/**
 * Report Modal — báo cáo vi phạm trong cuộc trò chuyện.
 *
 * Hiển thị form:
 * → Chọn loại vi phạm (tin nhắn, cuộc trò chuyện, user).
 * → Nhập lý do chi tiết (tối thiểu 10 ký tự).
 * → Submit qua server action.
 * → Hiện success/error state.
 */
interface ReportModalProps {
  conversationId: string;
  onClose: () => void;
}

export default function ReportModal({ conversationId, onClose }: ReportModalProps) {
  const [targetType, setTargetType] = useState('conversation');
  const [reason, setReason] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState('');

  const handleSubmit = async () => {
    if (reason.length < 10) {
      setError('Lý do phải có ít nhất 10 ký tự.');
      return;
    }

    setSubmitting(true);
    setError('');

    const result = await sendReport({
      targetType,
      targetId: conversationId,
      conversationRef: conversationId,
      reason,
    });

    if (result) {
      setSuccess(true);
    } else {
      setError('Gửi báo cáo thất bại. Vui lòng thử lại.');
    }
    setSubmitting(false);
  };

  // Success state
  if (success) {
    return (
      <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm"
           onClick={onClose}>
        <div className="bg-zinc-900 rounded-3xl border border-white/10 p-10 max-w-md mx-4 text-center space-y-4"
             onClick={e => e.stopPropagation()}>
          <CheckCircle2 className="w-12 h-12 text-emerald-400 mx-auto" />
          <h3 className="text-lg font-black text-white uppercase italic">Đã gửi báo cáo</h3>
          <p className="text-xs text-zinc-500">Cảm ơn bạn. Admin sẽ xem xét và xử lý trong thời gian sớm nhất.</p>
          <button
            onClick={onClose}
            className="px-6 py-2 rounded-xl bg-purple-600 hover:bg-purple-500 text-white text-xs font-bold uppercase tracking-widest transition-all"
          >
            Đóng
          </button>
        </div>
      </div>
    );
  }

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center bg-black/60 backdrop-blur-sm"
         onClick={onClose}>
      <div className="bg-zinc-900 rounded-3xl border border-white/10 p-8 max-w-md mx-4 w-full space-y-6"
           onClick={e => e.stopPropagation()}>
        {/* Header */}
        <div className="flex items-center justify-between">
          <div className="flex items-center gap-2">
            <Flag className="w-4 h-4 text-red-400" />
            <h3 className="text-sm font-black text-white uppercase tracking-widest">Báo cáo vi phạm</h3>
          </div>
          <button onClick={onClose} className="p-1 hover:bg-white/[0.04] rounded-lg transition-colors">
            <X className="w-4 h-4 text-zinc-500" />
          </button>
        </div>

        {/* Target Type */}
        <div className="space-y-2">
          <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Loại vi phạm</label>
          <div className="flex gap-2">
            {[
              { value: 'conversation', label: 'Cuộc trò chuyện' },
              { value: 'user', label: 'Người dùng' },
              { value: 'message', label: 'Tin nhắn' },
            ].map(opt => (
              <button
                key={opt.value}
                onClick={() => setTargetType(opt.value)}
                className={`flex-1 px-3 py-2 rounded-xl text-[10px] font-bold uppercase tracking-wider transition-all border ${
                  targetType === opt.value
                    ? 'bg-red-500/10 border-red-500/30 text-red-400'
                    : 'bg-white/[0.02] border-white/5 text-zinc-600'
                }`}
              >
                {opt.label}
              </button>
            ))}
          </div>
        </div>

        {/* Reason */}
        <div className="space-y-2">
          <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Lý do *</label>
          <textarea
            value={reason}
            onChange={(e) => setReason(e.target.value)}
            placeholder="Mô tả chi tiết lý do báo cáo (tối thiểu 10 ký tự)..."
            rows={4}
            className="w-full bg-white/[0.02] border border-white/10 rounded-xl p-4 text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-red-500/30 transition-all resize-none"
          />
          <span className={`text-[10px] ${reason.length >= 10 ? 'text-zinc-700' : 'text-red-400'}`}>
            {reason.length}/500
          </span>
        </div>

        {/* Error */}
        {error && (
          <div className="p-3 rounded-lg bg-red-500/10 border border-red-500/20 text-xs text-red-400">
            {error}
          </div>
        )}

        {/* Submit */}
        <button
          onClick={handleSubmit}
          disabled={submitting || reason.length < 10}
          className="w-full flex items-center justify-center gap-2 p-3 rounded-xl bg-red-600/20 hover:bg-red-600/30 border border-red-500/20 text-red-400 text-[10px] font-black uppercase tracking-widest transition-all disabled:opacity-50"
        >
          {submitting ? <Loader2 className="w-3 h-3 animate-spin" /> : <Flag className="w-3 h-3" />}
          Gửi Báo Cáo
        </button>
      </div>
    </div>
  );
}
