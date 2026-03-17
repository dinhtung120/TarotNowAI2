'use client';

import React, { useEffect, useState } from 'react';
import {
  createWithdrawal, listMyWithdrawals,
  type WithdrawalResult
} from '@/actions/withdrawalActions';
import {
  Sparkles, Loader2, CheckCircle2, AlertTriangle,
  Diamond, Building2, CreditCard, ArrowRight, Clock
} from 'lucide-react';
import MfaChallengeModal from '@/components/auth/MfaChallengeModal';

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
      case 'pending': return { text: 'Chờ duyệt', color: 'amber' };
      case 'approved': return { text: 'Đã duyệt', color: 'emerald' };
      case 'rejected': return { text: 'Từ chối', color: 'red' };
      case 'paid': return { text: 'Đã thanh toán', color: 'blue' };
      default: return { text: status, color: 'zinc' };
    }
  };

  return (
    <div className="max-w-2xl mx-auto px-6 py-16 space-y-12 animate-in fade-in slide-in-from-bottom-8 duration-1000">
      {/* Header */}
      <div className="space-y-4">
        <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-emerald-500/5 border border-emerald-500/10 text-[9px] uppercase tracking-[0.2em] font-black text-emerald-400 shadow-xl backdrop-blur-md">
          <Sparkles className="w-3 h-3" />
          Rút tiền
        </div>
        <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic">
          Withdrawal
        </h1>
        <p className="text-zinc-500 font-medium text-sm">
          Chuyển Diamond thành VND. Phí 10%, tối thiểu 50💎, tối đa 1 lần/ngày.
        </p>
      </div>

      {/* Form */}
      <form onSubmit={handleSubmit} className="space-y-6">
        {/* Amount */}
        <div className="space-y-2">
          <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">
            Số lượng Diamond
          </label>
          <div className="relative">
            <Diamond className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-amber-400" />
            <input
              type="number"
              value={amount}
              onChange={e => setAmount(e.target.value)}
              placeholder="Tối thiểu 50"
              min={50}
              className="w-full pl-12 pr-4 py-4 bg-white/[0.02] border border-white/10 rounded-2xl text-white text-lg font-bold placeholder:text-zinc-700 focus:outline-none focus:border-emerald-500/30 transition-all"
            />
          </div>
        </div>

        {/* Fee Preview */}
        {amountNum >= 50 && (
          <div className="p-4 bg-emerald-500/5 border border-emerald-500/10 rounded-2xl space-y-2 animate-in fade-in duration-300">
            <div className="flex justify-between text-xs">
              <span className="text-zinc-500">Tổng VND</span>
              <span className="text-white font-bold">{grossVnd.toLocaleString('vi-VN')} ₫</span>
            </div>
            <div className="flex justify-between text-xs">
              <span className="text-zinc-500">Phí 10%</span>
              <span className="text-red-400 font-bold">-{feeVnd.toLocaleString('vi-VN')} ₫</span>
            </div>
            <div className="border-t border-white/5 pt-2 flex justify-between text-sm">
              <span className="text-zinc-400 font-bold">Thực nhận</span>
              <span className="text-emerald-400 font-black text-lg">{netVnd.toLocaleString('vi-VN')} ₫</span>
            </div>
          </div>
        )}

        {/* Bank Info */}
        <div className="space-y-4">
          <div className="space-y-2">
            <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Ngân hàng</label>
            <div className="relative">
              <Building2 className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-zinc-600" />
              <input
                type="text"
                value={bankName}
                onChange={e => setBankName(e.target.value)}
                placeholder="VD: Vietcombank"
                className="w-full pl-12 pr-4 py-3 bg-white/[0.02] border border-white/10 rounded-xl text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-white/20 transition-all"
              />
            </div>
          </div>

          <div className="space-y-2">
            <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Tên chủ tài khoản</label>
            <input
              type="text"
              value={accountName}
              onChange={e => setAccountName(e.target.value)}
              placeholder="NGUYEN VAN A"
              className="w-full px-4 py-3 bg-white/[0.02] border border-white/10 rounded-xl text-sm text-white placeholder:text-zinc-700 focus:outline-none focus:border-white/20 transition-all uppercase"
            />
          </div>

          <div className="space-y-2">
            <label className="text-[10px] font-black uppercase tracking-widest text-zinc-500">Số tài khoản</label>
            <div className="relative">
              <CreditCard className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 text-zinc-600" />
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

        {/* Error / Success */}
        {error && (
          <div className="flex items-center gap-2 p-3 rounded-xl bg-red-500/5 border border-red-500/10 text-xs text-red-400">
            <AlertTriangle className="w-3 h-3" /> {error}
          </div>
        )}
        {success && (
          <div className="flex items-center gap-2 p-3 rounded-xl bg-emerald-500/5 border border-emerald-500/10 text-xs text-emerald-400">
            <CheckCircle2 className="w-3 h-3" /> Đã gửi yêu cầu rút tiền thành công!
          </div>
        )}

        {/* Submit */}
        <button
          type="submit"
          disabled={submitting || amountNum < 50}
          className="w-full flex items-center justify-center gap-2 p-4 rounded-2xl bg-emerald-600 hover:bg-emerald-500 disabled:bg-zinc-800 text-white text-xs font-black uppercase tracking-widest transition-all disabled:opacity-50"
        >
          {submitting ? <Loader2 className="w-4 h-4 animate-spin" /> : <ArrowRight className="w-4 h-4" />}
          Gửi Yêu Cầu Rút Tiền
        </button>
      </form>

      {/* History */}
      <div className="space-y-4">
        <h2 className="text-sm font-black text-white uppercase tracking-widest">Lịch sử rút tiền</h2>

        {loadingHistory && (
          <div className="flex items-center justify-center py-8">
            <Loader2 className="w-6 h-6 text-zinc-600 animate-spin" />
          </div>
        )}

        {!loadingHistory && history.length === 0 && (
          <p className="text-zinc-700 text-xs text-center py-8">Chưa có yêu cầu rút tiền nào.</p>
        )}

        {history.map(item => {
          const badge = getStatusBadge(item.status);
          return (
            <div key={item.id} className="p-4 bg-white/[0.02] border border-white/5 rounded-2xl space-y-2">
              <div className="flex items-center justify-between">
                <div className="flex items-center gap-2">
                  <Diamond className="w-3 h-3 text-amber-400" />
                  <span className="text-sm font-bold text-white">{item.amountDiamond} 💎</span>
                  <ArrowRight className="w-3 h-3 text-zinc-600" />
                  <span className="text-sm font-bold text-emerald-400">{item.netAmountVnd?.toLocaleString('vi-VN')} ₫</span>
                </div>
                <span className={`px-2 py-0.5 rounded-full bg-${badge.color}-500/10 border border-${badge.color}-500/20 text-${badge.color}-400 text-[9px] font-bold uppercase`}>
                  {badge.text}
                </span>
              </div>
              <div className="flex items-center gap-3 text-[10px] text-zinc-600">
                <span>{item.bankName} • {item.bankAccountNumber}</span>
                <span className="flex items-center gap-1">
                  <Clock className="w-2.5 h-2.5" />
                  {new Date(item.createdAt).toLocaleDateString('vi-VN')}
                </span>
              </div>
              {item.adminNote && (
                <div className="text-[10px] text-zinc-500 italic">Admin: {item.adminNote}</div>
              )}
            </div>
          );
        })}
      </div>

      <MfaChallengeModal
        isOpen={showMfa}
        onClose={() => setShowMfa(false)}
        onSuccess={handleMfaSuccess}
        actionTitle="rút tiền"
        skipApiCall={true} // Bỏ qua call check API riêng lẻ, gửi code thẳng vào createWithdrawal
      />
    </div>
  );
}
