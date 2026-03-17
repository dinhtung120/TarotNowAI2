'use client';

import React, { useEffect, useState } from 'react';
import { getMfaStatus, setupMfa, verifyMfa, type MfaSetupResult } from '@/actions/mfaActions';
import { useQRCode } from 'next-qrcode';
import { Shield, ShieldAlert, ShieldCheck, Loader2, ArrowRight, CheckCircle2, AlertTriangle, KeyRound, Copy } from 'lucide-react';
import Link from 'next/link';

export default function MfaSetupPage() {
  const { Canvas } = useQRCode();
  const [mfaEnabled, setMfaEnabled] = useState<boolean | null>(null);
  
  // Setup state
  const [setupData, setSetupData] = useState<MfaSetupResult | null>(null);
  const [setupLoading, setSetupLoading] = useState(false);
  const [setupError, setSetupError] = useState('');
  
  // Verify state
  const [code, setCode] = useState('');
  const [verifyLoading, setVerifyLoading] = useState(false);
  const [verifyError, setVerifyError] = useState('');

  useEffect(() => {
    checkStatus();
  }, []);

  const checkStatus = async () => {
    const status = await getMfaStatus();
    setMfaEnabled(status);
  };

  const handleStartSetup = async () => {
    setSetupLoading(true);
    setSetupError('');
    const res = await setupMfa();
    if (res.success && res.data) {
      setSetupData(res.data);
    } else {
      setSetupError(res.error || 'Lỗi khởi tạo MFA');
    }
    setSetupLoading(false);
  };

  const handleVerify = async (e: React.FormEvent) => {
    e.preventDefault();
    if (code.length < 6) return;

    setVerifyLoading(true);
    setVerifyError('');
    const res = await verifyMfa(code);
    if (res.success) {
      setMfaEnabled(true);
      setSetupData(null);
    } else {
      setVerifyError(res.error || 'Mã không hợp lệ');
    }
    setVerifyLoading(false);
  };

  const copyToClipboard = (text: string) => {
    navigator.clipboard.writeText(text);
    // Có thể thêm toast thông báo ở đây
  };

  if (mfaEnabled === null) {
    return (
      <div className="flex justify-center py-20">
        <Loader2 className="w-8 h-8 text-emerald-500 animate-spin" />
      </div>
    );
  }

  return (
    <div className="max-w-2xl mx-auto px-6 py-16 space-y-12 animate-in fade-in slide-in-from-bottom-8 duration-1000">
      {/* Header */}
      <div className="space-y-4">
        <div className="inline-flex items-center gap-2 px-3 py-1 rounded-full bg-emerald-500/5 border border-emerald-500/10 text-[9px] uppercase tracking-[0.2em] font-black justify-center text-emerald-400 shadow-xl backdrop-blur-md">
          <Shield className="w-3 h-3" />
          Security
        </div>
        <h1 className="text-4xl md:text-5xl font-black tracking-tighter text-white uppercase italic">
          Bảo mật 2 lớp
        </h1>
        <p className="text-zinc-500 font-medium text-sm">
          Multi-Factor Authentication (MFA/TOTP) bảo vệ tài khoản của bạn khỏi truy cập trái phép. Bắt buộc để thực hiện Payout.
        </p>
      </div>

      {/* Tình trạng đã bật MFA */}
      {mfaEnabled && (
        <div className="p-8 border border-emerald-500/20 bg-emerald-500/5 rounded-3xl text-center space-y-4">
          <ShieldCheck className="w-16 h-16 text-emerald-400 mx-auto" />
          <h2 className="text-2xl font-black text-white">MFA Đã Kích Hoạt</h2>
          <p className="text-emerald-400/80 text-sm">Tài khoản của bạn đã được bảo vệ an toàn bằng MFA.</p>
          <div className="pt-4">
            <Link href="/wallet/withdraw" className="inline-flex items-center gap-2 px-6 py-3 bg-emerald-600 hover:bg-emerald-500 text-white text-xs font-black uppercase tracking-widest rounded-xl transition-all">
              Tới trang Rút Tiền <ArrowRight className="w-4 h-4" />
            </Link>
          </div>
        </div>
      )}

      {/* Tình trạng chưa bật, bắt đầu setup */}
      {!mfaEnabled && !setupData && (
        <div className="p-8 border border-amber-500/20 bg-amber-500/5 rounded-3xl text-center space-y-6">
          <ShieldAlert className="w-16 h-16 text-amber-400 mx-auto animate-pulse" />
          <div className="space-y-2">
            <h2 className="text-2xl font-black text-white">Bảo mật chưa hoàn thiện</h2>
            <p className="text-amber-400/80 text-sm">Bạn cần bật MFA để bảo vệ ví và thực hiện rút tiền.</p>
          </div>
          
          {setupError && (
            <div className="text-red-400 text-xs flex justify-center items-center gap-2">
              <AlertTriangle className="w-4 h-4" /> {setupError}
            </div>
          )}

          <button
            onClick={handleStartSetup}
            disabled={setupLoading}
            className="w-full flex items-center justify-center gap-2 px-6 py-4 bg-emerald-600 hover:bg-emerald-500 text-white text-sm font-black uppercase tracking-widest rounded-2xl transition-all"
          >
            {setupLoading ? <Loader2 className="w-5 h-5 animate-spin" /> : <KeyRound className="w-5 h-5" />}
            Bắt đầu thiết lập MFA ngay
          </button>
        </div>
      )}

      {/* Hiển thị QR và nhập mã xác nhận (Bước 2) */}
      {!mfaEnabled && setupData && (
        <div className="space-y-8">
          <div className="p-8 border border-white/10 bg-white/[0.02] rounded-3xl space-y-8">
            <div className="space-y-2 text-center">
              <h3 className="text-xl font-black text-white uppercase italic">1. Quét mã QR</h3>
              <p className="text-zinc-500 text-xs">Sử dụng Google Authenticator, Authy hoặc ứng dụng TOTP tương tự.</p>
            </div>
            
            <div className="flex justify-center bg-white p-4 rounded-2xl w-fit mx-auto">
              <Canvas
                text={setupData.qrCodeUri}
                options={{
                  errorCorrectionLevel: 'M',
                  margin: 2,
                  scale: 4,
                  width: 200,
                  color: { dark: '#000000', light: '#FFFFFF' },
                }}
              />
            </div>

            <div className="text-center space-y-2">
              <p className="text-[10px] text-zinc-500 uppercase font-black tracking-widest">Hoặc nhập mã thủ công</p>
              <div className="flex justify-center gap-2">
                <code className="px-4 py-2 bg-black/50 border border-white/10 rounded-xl text-emerald-400 font-mono tracking-widest">
                  {setupData.secretDisplay}
                </code>
                <button onClick={() => copyToClipboard(setupData.secretDisplay)} className="p-2 bg-white/5 border border-white/10 rounded-xl hover:bg-white/10 transition-colors">
                  <Copy className="w-4 h-4 text-zinc-400" />
                </button>
              </div>
            </div>
          </div>

          <div className="p-8 border border-rose-500/20 bg-rose-500/5 rounded-3xl space-y-6">
            <div className="space-y-2">
              <h3 className="text-xl font-black text-white uppercase italic text-center text-rose-400">2. Lưu Mã Dự Phòng (Backup Codes)</h3>
              <p className="text-rose-400/80 text-xs text-center">Nếu mất điện thoại, bạn PHẢI dùng các mã này để khôi phục. Mỗi mã dùng 1 lần.</p>
            </div>
            
            <div className="grid grid-cols-2 gap-3 max-w-sm mx-auto">
              {setupData.backupCodes.map((code, idx) => (
                <div key={idx} className="bg-black/50 border border-rose-500/20 p-3 rounded-xl text-center font-mono text-zinc-300 font-bold select-all cursor-text">
                  {code}
                </div>
              ))}
            </div>
          </div>

          <form onSubmit={handleVerify} className="p-8 border border-emerald-500/20 bg-emerald-500/5 rounded-3xl space-y-6">
            <div className="space-y-2 text-center">
              <h3 className="text-xl font-black text-white uppercase italic">3. Xác nhận mã</h3>
              <p className="text-emerald-400/80 text-xs">Nhập mã 6 số từ ứng dụng để hoàn tất thiết lập.</p>
            </div>

            <input
              type="text"
              value={code}
              onChange={e => setCode(e.target.value.replace(/\D/g, '').slice(0, 6))}
              placeholder="000000"
              className="w-full max-w-[200px] mx-auto block text-center text-3xl tracking-[0.5em] font-mono py-4 bg-black/50 border border-emerald-500/30 rounded-2xl text-emerald-400 focus:outline-none focus:border-emerald-500 transition-colors placeholder:text-zinc-800"
              autoFocus
            />

            {verifyError && (
              <div className="text-red-400 text-xs flex justify-center items-center gap-2">
                <AlertTriangle className="w-4 h-4" /> {verifyError}
              </div>
            )}

            <button
              type="submit"
              disabled={code.length !== 6 || verifyLoading}
              className="w-full max-w-[200px] mx-auto flex items-center justify-center gap-2 px-6 py-4 bg-emerald-600 hover:bg-emerald-500 disabled:bg-zinc-800 text-white text-sm font-black uppercase tracking-widest rounded-2xl transition-all disabled:opacity-50"
            >
              {verifyLoading ? <Loader2 className="w-5 h-5 animate-spin" /> : <CheckCircle2 className="w-5 h-5" />}
              Xác nhận & Bật
            </button>
          </form>
        </div>
      )}
    </div>
  );
}
