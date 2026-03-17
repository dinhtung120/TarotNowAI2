'use client';

import React, { useEffect, useState } from 'react';
import {
  listWithdrawalQueue, processWithdrawal,
  type WithdrawalResult
} from '@/actions/withdrawalActions';
import {
  Sparkles, Loader2, CheckCircle2, XCircle,
  Diamond, Building2, ArrowRight, Clock, User
} from 'lucide-react';
import MfaChallengeModal from '@/components/auth/MfaChallengeModal';

/**
 * Admin Withdrawal Queue — danh sách yêu cầu rút tiền pending.
 * Admin approve hoặc reject.
 */
export default function AdminWithdrawalsPage() {
  const [queue, setQueue] = useState<WithdrawalResult[]>([]);
  const [loading, setLoading] = useState(true);
  const [processing, setProcessing] = useState<string | null>(null);
  const [notes, setNotes] = useState<Record<string, string>>({});
  
  // MFA
  const [showMfa, setShowMfa] = useState(false);
  const [pendingAction, setPendingAction] = useState<{ id: string, action: 'approve' | 'reject' } | null>(null);

  useEffect(() => {
    loadQueue();
  }, []);

  const loadQueue = async () => {
    const data = await listWithdrawalQueue();
    setQueue(data);
    setLoading(false);
  };

  const handleProcess = (id: string, action: 'approve' | 'reject') => {
    setPendingAction({ id, action });
    setShowMfa(true);
  };

  const handleMfaSuccess = async (mfaCode: string) => {
    if (!pendingAction) return;
    const { id, action } = pendingAction;
    
    setShowMfa(false);
    setProcessing(id);
    
    const res = await processWithdrawal({
      withdrawalId: id,
      action,
      adminNote: notes[id] || undefined,
      mfaCode
    });
    
    if (res.success) {
      setQueue(prev => prev.filter(r => r.id !== id));
    } else {
      alert(res.error || 'Xử lý thất bại');
    }
    setProcessing(null);
    setPendingAction(null);
  };

  return (
    <div className="max-w-5xl mx-auto px-6 py-16 space-y-10 animate-in fade-in slide-in-from-bottom-8 duration-1000">
      {/* Header */}
      <div className="space-y-4">
        <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-emerald-500/5 border border-emerald-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-emerald-400 shadow-xl backdrop-blur-md">
          <Sparkles className="w-3 h-3" />
          Admin
        </div>
        <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic">
          Payout Queue
        </h1>
        <p className="text-zinc-500 font-medium text-sm">
          Duyệt hoặc từ chối yêu cầu rút tiền của Reader. FIFO.
        </p>
      </div>

      {/* Loading */}
      {loading && (
        <div className="flex items-center justify-center py-16">
          <Loader2 className="w-8 h-8 text-emerald-500 animate-spin" />
        </div>
      )}

      {/* Empty */}
      {!loading && queue.length === 0 && (
        <div className="text-center py-16">
          <CheckCircle2 className="w-12 h-12 text-emerald-500/30 mx-auto mb-3" />
          <p className="text-zinc-600 text-sm">Không có yêu cầu nào đang chờ duyệt.</p>
        </div>
      )}

      {/* Queue */}
      <div className="space-y-4">
        {queue.map(item => (
          <div key={item.id} className="p-6 bg-white/[0.02] border border-white/5 rounded-2xl space-y-4">
            {/* Row 1: Amount + User */}
            <div className="flex items-center justify-between">
              <div className="flex items-center gap-3">
                <div className="w-10 h-10 rounded-full bg-amber-500/10 flex items-center justify-center">
                  <Diamond className="w-5 h-5 text-amber-400" />
                </div>
                <div>
                  <div className="text-lg font-black text-white">{item.amountDiamond} 💎</div>
                  <div className="text-[10px] text-zinc-600">
                    Gross: {item.amountVnd?.toLocaleString('vi-VN')} ₫ | Fee: {item.feeVnd?.toLocaleString('vi-VN')} ₫ |{' '}
                    <span className="text-emerald-400 font-bold">Net: {item.netAmountVnd?.toLocaleString('vi-VN')} ₫</span>
                  </div>
                </div>
              </div>
              <div className="text-right">
                <div className="flex items-center gap-1 text-[10px] text-zinc-600">
                  <Clock className="w-3 h-3" />
                  {new Date(item.createdAt).toLocaleString('vi-VN')}
                </div>
                <div className="flex items-center gap-1 text-[10px] text-zinc-700 mt-1">
                  <User className="w-3 h-3" />
                  {item.userId.substring(0, 8)}...
                </div>
              </div>
            </div>

            {/* Row 2: Bank Info */}
            <div className="flex items-center gap-4 p-3 bg-white/[0.02] border border-white/5 rounded-xl">
              <Building2 className="w-4 h-4 text-zinc-600" />
              <div className="text-xs text-zinc-400">
                <span className="font-bold text-white">{item.bankName}</span>
                {' '}&bull;{' '}
                {item.bankAccountName}
                {' '}&bull;{' '}
                <span className="font-mono">{item.bankAccountNumber}</span>
              </div>
            </div>

            {/* Row 3: Admin Note + Actions */}
            <div className="flex items-center gap-3">
              <input
                type="text"
                placeholder="Ghi chú admin..."
                value={notes[item.id] || ''}
                onChange={e => setNotes(prev => ({ ...prev, [item.id]: e.target.value }))}
                className="flex-1 bg-white/[0.02] border border-white/10 rounded-xl px-3 py-2 text-xs text-white placeholder:text-zinc-700 focus:outline-none focus:border-white/20 transition-all"
              />
              <button
                onClick={() => handleProcess(item.id, 'approve')}
                disabled={processing === item.id}
                className="flex items-center gap-1 px-4 py-2 rounded-xl bg-emerald-600/20 hover:bg-emerald-600/30 border border-emerald-500/20 text-emerald-400 text-[10px] font-black uppercase tracking-wider disabled:opacity-50 transition-all"
              >
                {processing === item.id ? <Loader2 className="w-3 h-3 animate-spin" /> : <CheckCircle2 className="w-3 h-3" />}
                Approve
              </button>
              <button
                onClick={() => handleProcess(item.id, 'reject')}
                disabled={processing === item.id}
                className="flex items-center gap-1 px-4 py-2 rounded-xl bg-red-600/20 hover:bg-red-600/30 border border-red-500/20 text-red-400 text-[10px] font-black uppercase tracking-wider disabled:opacity-50 transition-all"
              >
                {processing === item.id ? <Loader2 className="w-3 h-3 animate-spin" /> : <XCircle className="w-3 h-3" />}
                Reject
              </button>
            </div>
          </div>
        ))}
      </div>

      <MfaChallengeModal
        isOpen={showMfa}
        onClose={() => setShowMfa(false)}
        onSuccess={handleMfaSuccess}
        actionTitle={pendingAction?.action === 'approve' ? 'duyệt yêu cầu rút tiền' : 'từ chối yêu cầu rút tiền'}
        skipApiCall={true}
      />
    </div>
  );
}
