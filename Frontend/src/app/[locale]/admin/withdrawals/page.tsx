'use client';

import React, { useEffect, useState } from 'react';
import {
  listWithdrawalQueue, processWithdrawal,
  type WithdrawalResult
} from '@/actions/withdrawalActions';
import {
  Sparkles, Loader2, CheckCircle2, XCircle,
  Diamond, Building2, Clock, User, ArrowRight
} from 'lucide-react';
import MfaChallengeModal from '@/components/auth/MfaChallengeModal';
import { SectionHeader, GlassCard, Button, Input } from '@/components/ui';
import toast from 'react-hot-toast';

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
      toast.success('Xử lý yêu cầu thành công');
      setQueue(prev => prev.filter(r => r.id !== id));
    } else {
      toast.error(res.error || 'Xử lý thất bại');
    }
    setProcessing(null);
    setPendingAction(null);
  };

  return (
    <div className="space-y-8 pb-20 animate-in fade-in duration-700 max-w-5xl mx-auto">
      {/* Header */}
      <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
        <SectionHeader
            tag="Admin"
            tagIcon={<Sparkles className="w-3 h-3 text-[var(--success)]" />}
            title="Payout Queue"
            subtitle="Duyệt hoặc từ chối yêu cầu rút tiền của Reader. Thứ tự FIFO."
            className="mb-0 text-left items-start"
        />
      </div>

      {/* Loading */}
      {loading && (
        <div className="flex items-center justify-center py-20">
          <div className="flex flex-col items-center justify-center space-y-4">
              <Loader2 className="w-8 h-8 animate-spin text-[var(--success)]" />
              <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">Đang tải danh sách...</span>
          </div>
        </div>
      )}

      {/* Empty */}
      {!loading && queue.length === 0 && (
        <GlassCard className="flex flex-col items-center justify-center py-20 text-center">
          <div className="w-16 h-16 rounded-full bg-[var(--success)]/10 border border-[var(--success)]/20 flex items-center justify-center mb-6 shadow-inner">
              <CheckCircle2 className="w-8 h-8 text-[var(--success)]" />
          </div>
          <p className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">Không có yêu cầu nào đang chờ duyệt.</p>
        </GlassCard>
      )}

      {/* Queue */}
      {!loading && queue.length > 0 && (
        <div className="space-y-6">
          {queue.map(item => (
            <GlassCard key={item.id} className="space-y-6 group hover:border-white/10 transition-all">
              {/* Row 1: Amount + User */}
              <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
                <div className="flex items-center gap-4">
                  <div className="w-12 h-12 rounded-[1rem] bg-[var(--warning)]/10 border border-[var(--warning)]/20 flex items-center justify-center shadow-inner group-hover:scale-110 transition-transform">
                    <Diamond className="w-6 h-6 text-[var(--warning)]" />
                  </div>
                  <div>
                    <div className="text-2xl font-black text-white italic tracking-tighter drop-shadow-sm">{item.amountDiamond} <span className="text-xl text-[var(--warning)]">💎</span></div>
                    <div className="text-[10px] font-bold text-[var(--text-secondary)] mt-1 flex gap-2 items-center">
                      <span>Gross: {item.amountVnd?.toLocaleString('vi-VN')} ₫</span>
                      <span className="opacity-50">|</span> 
                      <span>Fee: {item.feeVnd?.toLocaleString('vi-VN')} ₫</span>
                      <span className="opacity-50">|</span> 
                      <span className="text-[var(--success)] drop-shadow-sm">Net: {item.netAmountVnd?.toLocaleString('vi-VN')} ₫</span>
                    </div>
                  </div>
                </div>
                
                <div className="flex flex-col items-start md:items-end gap-2 text-left md:text-right bg-white/[0.02] p-3 rounded-2xl border border-white/5 shadow-inner shrink-0">
                  <div className="flex items-center gap-2 text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-tighter">
                    <Clock className="w-3.5 h-3.5" />
                    {new Date(item.createdAt).toLocaleString('vi-VN')}
                  </div>
                  <div className="flex items-center gap-2 text-[10px] font-bold text-[var(--text-tertiary)] italic">
                    <User className="w-3.5 h-3.5" />
                    ID: {item.userId.substring(0, 8)}...
                  </div>
                </div>
              </div>

              {/* Row 2: Bank Info */}
              <div className="flex items-center gap-4 p-4 bg-white/[0.02] border border-white/5 rounded-2xl shadow-inner">
                <div className="w-8 h-8 rounded-full bg-white/5 flex items-center justify-center shrink-0">
                    <Building2 className="w-4 h-4 text-[var(--text-secondary)]" />
                </div>
                <div className="text-xs text-[var(--text-tertiary)] flex flex-wrap items-center gap-2">
                  <span className="font-black text-white uppercase tracking-tighter drop-shadow-sm">{item.bankName}</span>
                  <span className="text-white/20">&bull;</span>
                  <span className="font-bold">{item.bankAccountName}</span>
                  <span className="text-white/20">&bull;</span>
                  <span className="font-mono text-[var(--text-secondary)] bg-white/5 px-2 py-0.5 rounded-md border border-white/10">{item.bankAccountNumber}</span>
                </div>
              </div>

              {/* Row 3: Admin Note + Actions */}
              <div className="flex flex-col md:flex-row items-center gap-4 pt-2 border-t border-white/5">
                <Input
                  leftIcon={<User className="w-4 h-4" />}
                  placeholder="GHI CHÚ ADMIN..."
                  value={notes[item.id] || ''}
                  onChange={e => setNotes(prev => ({ ...prev, [item.id]: e.target.value }))}
                  className="flex-1 w-full text-xs font-black uppercase tracking-widest text-white shadow-inner bg-white/[0.02] border-white/5"
                />
                
                <div className="flex items-center gap-3 w-full md:w-auto shrink-0">
                    <Button
                        variant="primary"
                        onClick={() => handleProcess(item.id, 'approve')}
                        disabled={processing === item.id}
                        className="flex-1 md:flex-none py-3 shadow-md bg-[var(--success)] text-white hover:bg-[var(--success)] hover:brightness-110"
                    >
                        {processing === item.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <CheckCircle2 className="w-4 h-4" />}
                        Approve
                    </Button>
                    <Button
                        variant="danger"
                        onClick={() => handleProcess(item.id, 'reject')}
                        disabled={processing === item.id}
                        className="flex-1 md:flex-none py-3 shadow-md"
                    >
                        {processing === item.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <XCircle className="w-4 h-4" />}
                        Reject
                    </Button>
                </div>
              </div>
            </GlassCard>
          ))}
        </div>
      )}

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
