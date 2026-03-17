'use client';

import React, { useEffect, useState } from 'react';
import {
  createWithdrawal, listMyWithdrawals,
  type WithdrawalResult
} from '@/actions/withdrawalActions';
import {
  Sparkles, Loader2, CheckCircle2, AlertTriangle,
  Diamond, Building2, CreditCard, ArrowRight, Clock, ArrowLeft
} from 'lucide-react';
import MfaChallengeModal from '@/components/auth/MfaChallengeModal';
import { useRouter } from '@/i18n/routing';
import UserLayout from '@/components/layout/UserLayout';
import { SectionHeader, Button, GlassCard } from '@/components/ui';

/**
 * Trang rút tiền cho Reader.
 *
 * Form gửi yêu cầu rút:
 * → Nhập số Diamond (min 50)
 * → Thông tin ngân hàng
 * → Preview phí 10% + net amount
 *
 * Lịch sử rút tiền ở phía dưới.
 */
export default function WithdrawPage() {
  const router = useRouter();
  
  // Form state
  const [amount, setAmount] = useState('');
  const [bankName, setBankName] = useState('');
  const [accountName, setAccountName] = useState('');
  const [accountNumber, setAccountNumber] = useState('');
  const [submitting, setSubmitting] = useState(false);
  const [success, setSuccess] = useState(false);
  const [error, setError] = useState('');
  
  // MFA
  const [showMfa, setShowMfa] = useState(false);

  // History
  const [history, setHistory] = useState<WithdrawalResult[]>([]);
  const [loadingHistory, setLoadingHistory] = useState(true);

  useEffect(() => {
    loadHistory();
  }, []);

  const loadHistory = async () => {
    const data = await listMyWithdrawals();
    setHistory(data);
    setLoadingHistory(false);
  };

  // Fee preview
  const amountNum = parseInt(amount) || 0;
  // 1 Diamond = 1000 VND
  const grossVnd = amountNum * 1000;
  const feeVnd = Math.ceil(grossVnd * 0.10);
  const netVnd = grossVnd - feeVnd;

  const handleSubmit = (e: React.FormEvent) => {
    e.preventDefault();
    setError('');
    setSuccess(false);

    if (amountNum < 50) { setError('Tối thiểu 50 Diamond.'); return; }
    if (!bankName.trim()) { setError('Vui lòng nhập tên ngân hàng.'); return; }
    if (!accountName.trim()) { setError('Vui lòng nhập tên chủ tài khoản.'); return; }
    if (!accountNumber.trim()) { setError('Vui lòng nhập số tài khoản.'); return; }

    // Mở modal MFA thay vì submit luôn
    setShowMfa(true);
  };

  const handleMfaSuccess = async (mfaCode: string) => {
    setShowMfa(false);
    setSubmitting(true);
    setError('');

    const result = await createWithdrawal({
      amountDiamond: amountNum,
      bankName: bankName.trim(),
      bankAccountName: accountName.trim(),
      bankAccountNumber: accountNumber.trim(),
      mfaCode
    });

    if (result.success) {
      setSuccess(true);
      setAmount('');
      await loadHistory();
    } else {
      setError(result.error || 'Không thể tạo yêu cầu.');
      // Nếu lỗi do MFA sai, gợi ý MFA
      if (result.error?.includes('MFA')) {
        setError(result.error);
      }
    }
    setSubmitting(false);
  };

  const getStatusBadge = (status: string) => {
    switch (status) {
      case 'pending': return { text: 'Chờ duyệt', color: 'var(--warning)', bg: 'var(--warning-bg)' };
      case 'approved': return { text: 'Đã duyệt', color: 'var(--success)', bg: 'var(--success-bg)' };
      case 'rejected': return { text: 'Từ chối', color: 'var(--danger)', bg: 'var(--danger-bg)' };
      case 'paid': return { text: 'Đã thanh toán', color: 'var(--info)', bg: 'var(--info-bg)' };
      default: return { text: status, color: 'var(--text-secondary)', bg: 'rgba(255,255,255,0.05)' };
    }
  };

  return (
    <UserLayout>
        <div className="max-w-3xl mx-auto px-6 pt-8 pb-32 space-y-12 w-full">
            
            <button 
                onClick={() => router.push('/wallet')}
                className="group flex items-center gap-2 text-[var(--text-secondary)] hover:text-white transition-colors text-[10px] font-black uppercase tracking-[0.2em] mb-8 w-fit"
            >
                <ArrowLeft className="w-3.5 h-3.5 transition-transform group-hover:-translate-x-1" />
                Quay lại kho báu
            </button>

            {/* Header */}
            <SectionHeader
                tag="Rút tiền"
                tagIcon={<Sparkles className="w-3 h-3 text-[var(--success)]" />}
                title="Withdrawal"
                subtitle="Chuyển Diamond thành VND. Phí 10%, tối thiểu 50💎, tối đa 1 lần/ngày."
                className="animate-in fade-in slide-in-from-bottom-4 duration-1000"
            />

            {/* Form */}
            <GlassCard className="animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-200">
                <form onSubmit={handleSubmit} className="space-y-6">
                    {/* Amount */}
                    <div className="space-y-4">
                        <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)] block">
                            Số lượng Diamond
                        </label>
                        <div className="relative">
                            <Diamond className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-[var(--warning)]" />
                            <input
                                type="number"
                                value={amount}
                                onChange={e => setAmount(e.target.value)}
                                placeholder="Tối thiểu 50"
                                min={50}
                                className="w-full pl-12 pr-4 py-4 bg-white/[0.02] border border-white/10 rounded-2xl text-white text-xl font-black italic placeholder:text-zinc-700 placeholder:not-italic focus:outline-none focus:border-[var(--success)]/40 focus:ring-1 focus:ring-[var(--success)]/20 transition-all font-sans"
                            />
                        </div>
                    </div>

                    {/* Fee Preview */}
                    {amountNum >= 50 && (
                        <div className="p-5 bg-[var(--success-bg)] border border-[var(--success)]/30 rounded-2xl space-y-3 animate-in fade-in duration-300">
                            <div className="flex justify-between items-center text-sm">
                                <span className="text-[var(--success)]/80 text-[10px] font-black uppercase tracking-widest">Tổng VND</span>
                                <span className="text-white font-bold">{grossVnd.toLocaleString('vi-VN')} ₫</span>
                            </div>
                            <div className="flex justify-between items-center text-sm">
                                <span className="text-[var(--danger)]/80 text-[10px] font-black uppercase tracking-widest">Phí 10%</span>
                                <span className="text-[var(--danger)] font-bold">-{feeVnd.toLocaleString('vi-VN')} ₫</span>
                            </div>
                            <div className="border-t border-[var(--success)]/20 pt-3 flex justify-between items-end">
                                <span className="text-[var(--success)] text-[11px] font-black uppercase tracking-widest">Thực nhận</span>
                                <span className="text-[var(--success)] font-black text-2xl italic">{netVnd.toLocaleString('vi-VN')} ₫</span>
                            </div>
                        </div>
                    )}

                    {/* Bank Info */}
                    <div className="space-y-5 pt-4">
                        <div className="space-y-3">
                            <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)] block">Ngân hàng</label>
                            <div className="relative">
                            <Building2 className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-zinc-500" />
                            <input
                                type="text"
                                value={bankName}
                                onChange={e => setBankName(e.target.value)}
                                placeholder="VD: Vietcombank, Techcombank..."
                                className="w-full pl-12 pr-4 py-3 bg-white/[0.02] border border-white/10 rounded-xl text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-white/20 transition-all"
                            />
                            </div>
                        </div>

                        <div className="space-y-3">
                            <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)] block">Tên chủ tài khoản</label>
                            <input
                            type="text"
                            value={accountName}
                            onChange={e => setAccountName(e.target.value)}
                            placeholder="NGUYEN VAN A"
                            className="w-full px-4 py-3 bg-white/[0.02] border border-white/10 rounded-xl text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-white/20 transition-all uppercase"
                            />
                        </div>

                        <div className="space-y-3">
                            <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)] block">Số tài khoản</label>
                            <div className="relative">
                            <CreditCard className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-zinc-500" />
                            <input
                                type="text"
                                value={accountNumber}
                                onChange={e => setAccountNumber(e.target.value)}
                                placeholder="0123456789"
                                className="w-full pl-12 pr-4 py-3 bg-white/[0.02] border border-white/10 rounded-xl text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-white/20 transition-all"
                            />
                            </div>
                        </div>
                    </div>

                    {/* Error / Success Messages */}
                    <div className="space-y-3">
                        {error && (
                            <div className="flex items-center gap-2 p-4 rounded-xl bg-[var(--danger-bg)] border border-[var(--danger)]/30 text-xs font-bold uppercase tracking-widest text-[var(--danger)] animate-in zoom-in-95">
                                <AlertTriangle className="w-4 h-4" /> {error}
                            </div>
                        )}
                        {success && (
                            <div className="flex items-center gap-2 p-4 rounded-xl bg-[var(--success-bg)] border border-[var(--success)]/30 text-xs font-bold uppercase tracking-widest text-[var(--success)] animate-in zoom-in-95">
                                <CheckCircle2 className="w-4 h-4" /> Đã gửi yêu cầu rút tiền thành công!
                            </div>
                        )}

                        {/* Submit Button */}
                        <Button
                            variant="primary"
                            type="submit"
                            disabled={submitting || amountNum < 50}
                            className={`w-full h-14 ${amountNum >= 50 ? '!bg-[var(--success)] !text-white hover:scale-[1.02]' : ''}`}
                        >
                            {submitting ? (
                                <>
                                    <Loader2 className="w-4 h-4 animate-spin mr-2" />
                                    Đang xử lý...
                                </>
                            ) : (
                                <>
                                    <ArrowRight className="w-4 h-4 mr-2" />
                                    Gửi Yêu Cầu Rút Tiền
                                </>
                            )}
                        </Button>
                    </div>
                </form>
            </GlassCard>

            {/* History List */}
            <div className="space-y-6 pt-8 animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-400">
                <h2 className="text-[11px] font-black text-white uppercase tracking-[0.3em] flex items-center gap-3">
                    <Clock className="w-4 h-4 text-[var(--text-secondary)]" /> 
                    Lịch sử rút tiền
                </h2>

                <GlassCard className="!p-0 overflow-hidden border-white/5">
                    {loadingHistory && (
                        <div className="flex items-center justify-center py-12">
                            <Loader2 className="w-6 h-6 text-zinc-600 animate-spin" />
                        </div>
                    )}

                    {!loadingHistory && history.length === 0 && (
                        <div className="text-[var(--text-secondary)] text-[11px] font-medium uppercase tracking-widest text-center py-12">
                            Chưa có yêu cầu rút tiền nào.
                        </div>
                    )}

                    <div className="divide-y divide-white/5">
                        {history.map(item => {
                            const badge = getStatusBadge(item.status);
                            return (
                                <div key={item.id} className="p-6 space-y-4 hover:bg-white/[0.02] transition-colors">
                                    <div className="flex items-start justify-between gap-4">
                                        <div className="space-y-1 w-full">
                                            <div className="flex items-center justify-between">
                                                <div className="flex items-center gap-3">
                                                    <div className="flex items-center gap-1.5 bg-[var(--warning)]/10 px-2 py-1 rounded text-[var(--warning)] font-black text-sm italic">
                                                        {item.amountDiamond} <Diamond className="w-3.5 h-3.5" />
                                                    </div>
                                                    <ArrowRight className="w-3.5 h-3.5 text-zinc-600" />
                                                    <div className="font-black text-[var(--success)] text-lg italic tracking-tighter">
                                                        {item.netAmountVnd?.toLocaleString('vi-VN')} <span className="text-[10px] not-italic text-[var(--success)]/60">₫</span>
                                                    </div>
                                                </div>
                                                <span 
                                                    className="px-2.5 py-1 rounded-full text-[8px] font-black uppercase tracking-widest border border-white/5"
                                                    style={{ 
                                                        color: badge.color, 
                                                        backgroundColor: badge.bg,
                                                        borderColor: `${badge.color}40`
                                                    }}
                                                >
                                                    {badge.text}
                                                </span>
                                            </div>
                                        </div>
                                    </div>

                                    <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-3 text-[10px] text-[var(--text-tertiary)] font-medium">
                                        <div className="flex items-center gap-1.5 uppercase tracking-widest">
                                            <Building2 className="w-3 h-3 text-zinc-600" />
                                            {item.bankName} • {item.bankAccountNumber}
                                        </div>
                                        <div className="flex items-center gap-1.5 font-bold text-zinc-500">
                                            <Clock className="w-3 h-3" />
                                            {new Date(item.createdAt).toLocaleDateString('vi-VN')}
                                        </div>
                                    </div>

                                    {item.adminNote && (
                                        <div className="text-[10px] text-[var(--warning)] italic bg-[var(--warning)]/5 p-3 rounded-xl border border-[var(--warning)]/10 mt-3 flex gap-2 w-full">
                                            <span className="font-bold opacity-70">ADMIN:</span>
                                            {item.adminNote}
                                        </div>
                                    )}
                                </div>
                            );
                        })}
                    </div>
                </GlassCard>
            </div>

            <MfaChallengeModal
                isOpen={showMfa}
                onClose={() => setShowMfa(false)}
                onSuccess={handleMfaSuccess}
                actionTitle="rút tiền"
                skipApiCall={true}
            />
        </div>
    </UserLayout>
  );
}
