/*
 * ===================================================================
 * FILE: (user)/profile/mfa/page.tsx (Cài đặt Bảo Mật 2 Lớp)
 * BỐI CẢNH (CONTEXT):
 *   Trang quản lý tính năng xác thực 2 bước (MFA/2FA) bằng Authenticator App.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Kiểm tra trạng thái MFA (Đã bật/Chưa bật).
 *   - Bước 1: Fetch secret key & URI (`setupMfa`) -> vẽ QR Code bằng `next-qrcode`.
 *   - Bước 2: Hiển thị Backup Codes để User lưu trữ phòng hờ.
 *   - Bước 3: User nhập mã OTP 6 số để xác nhận (`verifyMfa`).
 * ===================================================================
 */
'use client';

import React, { useEffect, useMemo, useState } from 'react';
import { getMfaStatus, setupMfa, verifyMfa, type MfaSetupResult } from '@/actions/mfaActions';
import { useQRCode } from 'next-qrcode';
import { Shield, ShieldAlert, ShieldCheck, Loader2, ArrowRight, CheckCircle2, AlertTriangle, KeyRound, Copy } from 'lucide-react';
import { Link } from '@/i18n/routing';
import { useTranslations } from 'next-intl';
import { SectionHeader, GlassCard, Button } from '@/components/ui';

export default function MfaSetupPage() {
 const t = useTranslations("Profile");
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
 const qrColors = useMemo(() => {
   if (typeof window === 'undefined') {
     return { dark: '', light: '' };
   }

   const root = getComputedStyle(document.documentElement);
   const dark = root.getPropertyValue('--qr-code-dark').trim() || root.getPropertyValue('--text-ink').trim();
   const light = root.getPropertyValue('--qr-code-light').trim() || root.getPropertyValue('--bg-elevated').trim();
   return { dark, light };
 }, []);
 const qrColorOptions = qrColors.dark && qrColors.light ? { color: qrColors } : {};

 useEffect(() => {
 const loadStatus = async () => {
 const status = await getMfaStatus();
 setMfaEnabled(status);
 };

 void loadStatus();
 }, []);

 const handleStartSetup = async () => {
 setSetupLoading(true);
 setSetupError('');
 const res = await setupMfa();
 if (res.success && res.data) {
 setSetupData(res.data);
 } else {
 setSetupError(res.error || t("mfa.setup_error_generic"));
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
 setVerifyError(res.error || t("mfa.verify_error_invalid"));
 }
 setVerifyLoading(false);
 };

 const copyToClipboard = (text: string) => {
 navigator.clipboard.writeText(text);
 };

 if (mfaEnabled === null) {
 return (
 <div className="h-[60vh] flex flex-col items-center justify-center space-y-6">
 <div className="relative group">
 <div className="absolute inset-x-0 top-0 h-40 w-40 bg-[var(--success)]/20 blur-[60px] rounded-full animate-pulse" />
 <Loader2 className="w-12 h-12 animate-spin text-[var(--success)] relative z-10" />
 </div>
 <div className="text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]">{t("mfa.status_checking")}</div>
 </div>
 );
 }

 return (
 <div className="max-w-2xl mx-auto px-4 sm:px-6 pt-8 pb-32 space-y-10 w-full animate-in fade-in slide-in-from-bottom-8 duration-1000">
 {/* Header */}
 <SectionHeader
 tag={t("mfa.tag")}
 tagIcon={<Shield className="w-3 h-3 text-[var(--success)]" />}
 title={t("mfa.title")}
 subtitle={t("mfa.subtitle")}
 />

 {/* Tình trạng đã bật MFA */}
 {mfaEnabled && (
 <GlassCard className="!p-8 text-center space-y-4">
 <div className="mx-auto w-24 h-24 bg-[var(--success-bg)] border border-[var(--success)]/30 rounded-full flex items-center justify-center shadow-[0_0_30px_var(--c-16-185-129-20)] mb-6">
 <ShieldCheck className="w-12 h-12 text-[var(--success)]" />
 </div>
 <h2 className="text-2xl font-black tn-text-primary uppercase italic tracking-tight">{t("mfa.enabled_title")}</h2>
 <p className="text-[var(--text-secondary)] text-sm font-medium">{t("mfa.enabled_desc")}</p>
 <div className="pt-6">
 <Link href="/wallet/withdraw">
 <Button variant="primary" className="!bg-[var(--success)] hover:!bg-[var(--success)]/80 tn-text-primary border-none shadow-[0_0_20px_var(--c-16-185-129-30)] hover:shadow-[0_0_30px_var(--c-16-185-129-50)]">
 {t("mfa.enabled_cta")} <ArrowRight className="w-4 h-4 ml-2" />
 </Button>
 </Link>
 </div>
 </GlassCard>
 )}

 {/* Tình trạng chưa bật, bắt đầu setup */}
 {!mfaEnabled && !setupData && (
 <GlassCard className="!p-8 text-center space-y-6">
 <div className="mx-auto w-24 h-24 bg-[var(--warning)]/10 border border-[var(--warning)]/30 rounded-full flex items-center justify-center shadow-[0_0_30px_var(--c-245-158-11-20)] animate-pulse mb-6">
 <ShieldAlert className="w-12 h-12 text-[var(--warning)]" />
 </div>
 <div className="space-y-3">
 <h2 className="text-2xl font-black tn-text-primary uppercase italic tracking-tight">{t("mfa.disabled_title")}</h2>
 <p className="text-[var(--warning)] text-sm font-medium">{t("mfa.disabled_desc")}</p>
 </div>
 {setupError && (
 <div className="text-[var(--danger)] text-xs font-bold uppercase tracking-widest flex justify-center items-center gap-2 bg-[var(--danger-bg)] border border-[var(--danger)]/30 p-4 rounded-xl">
 <AlertTriangle className="w-4 h-4" /> {setupError}
 </div>
 )}

 <div className="pt-4">
 <Button
 variant="primary"
 onClick={handleStartSetup}
 disabled={setupLoading}
 className="w-full h-14 !bg-[var(--warning)]/20 hover:!bg-[var(--warning)]/30 text-[var(--warning)] border-[var(--warning)]/30"
 >
 {setupLoading ? <Loader2 className="w-5 h-5 animate-spin mr-2" /> : <KeyRound className="w-5 h-5 mr-2" />}
 {t("mfa.start_setup")}
 </Button>
 </div>
 </GlassCard>
 )}

 {/* Hiển thị QR và nhập mã xác nhận (Bước 2) */}
 {!mfaEnabled && setupData && (
 <div className="space-y-8 animate-in slide-in-from-bottom-4 duration-700">
 <GlassCard className="!p-8 space-y-8">
 <div className="space-y-3 text-center">
 <h3 className="text-xl font-black tn-text-primary uppercase italic tracking-tight">{t("mfa.step1_title")}</h3>
 <p className="text-[var(--text-secondary)] text-sm font-medium">{t("mfa.step1_desc")}</p>
 </div>
                <div className="flex justify-center tn-surface-strong p-5 rounded-2xl w-fit mx-auto shadow-2xl">
                  <Canvas
                    text={setupData.qrCodeUri}
                    options={{
                      errorCorrectionLevel: 'M',
                      margin: 2,
                      scale: 4,
                      width: 200,
                      ...qrColorOptions,
                    }}
                  />
                </div>

 <div className="text-center space-y-4 pt-4 border-t tn-border">
 <p className="text-[10px] text-[var(--text-tertiary)] uppercase font-black tracking-widest">{t("mfa.manual_code_label")}</p>
 <div className="flex justify-center items-center gap-3">
 <code className="px-5 py-3 tn-panel rounded-xl text-[var(--success)] font-mono tracking-widest font-bold shadow-inner">
 {setupData.secretDisplay}
 </code>
 <button onClick={() => copyToClipboard(setupData.secretDisplay)} aria-label={t("mfa.copy")} className="p-3 tn-panel rounded-xl hover:tn-surface-strong transition-all group">
 <Copy className="w-4 h-4 text-[var(--text-secondary)] group-hover:tn-text-primary transition-colors" />
 </button>
 </div>
 </div>
 </GlassCard>

 <GlassCard className="!p-8 !bg-[var(--danger)]/5 border-[var(--danger)]/20 shadow-[0_0_40px_var(--c-239-68-68-05)] text-center space-y-8">
 <div className="space-y-3">
 <h3 className="text-xl font-black uppercase italic tracking-tight text-[var(--danger)] flex items-center justify-center gap-2">
 <AlertTriangle className="w-5 h-5" />
 {t("mfa.step2_title")}
 </h3>
 <p className="text-[var(--danger)]/80 text-sm font-medium">{t("mfa.step2_desc")}</p>
 </div>
 <div className="grid grid-cols-2 gap-4 max-w-sm mx-auto">
 {setupData.backupCodes.map((code, idx) => (
 <div key={idx} className="tn-surface border border-[var(--danger)]/20 py-3 rounded-xl text-center font-mono tn-text-secondary font-bold select-all cursor-text shadow-inner">
 {code}
 </div>
 ))}
 </div>
 </GlassCard>

 <GlassCard className="!p-8 text-center space-y-8">
 <div className="space-y-3">
 <h3 className="text-xl font-black tn-text-primary uppercase italic tracking-tight">{t("mfa.step3_title")}</h3>
 <p className="text-[var(--text-secondary)] text-sm font-medium">{t("mfa.step3_desc")}</p>
 </div>

 <form onSubmit={handleVerify} className="space-y-8">
 <input
 type="text"
 value={code}
 onChange={e => setCode(e.target.value.replace(/\D/g, '').slice(0, 6))}
 placeholder={t("mfa.code_placeholder")}
 className="w-full max-w-[240px] mx-auto block text-center text-4xl tracking-widest font-mono py-6 tn-field rounded-2xl tn-text-primary tn-field-success transition-all placeholder:text-[color:var(--c-154-144-171-58)] shadow-inner"
 autoFocus
 />

 {verifyError && (
 <div className="text-[var(--danger)] text-xs font-bold uppercase tracking-widest flex justify-center items-center gap-2 bg-[var(--danger-bg)] border border-[var(--danger)]/30 p-4 rounded-xl max-w-xs mx-auto">
 <AlertTriangle className="w-4 h-4" /> {verifyError}
 </div>
 )}

 <Button
 variant="primary"
 type="submit"
 disabled={code.length !== 6 || verifyLoading}
 className="w-full max-w-[240px] mx-auto h-14 !bg-[var(--success)] hover:!bg-[var(--success)]/80 tn-text-primary border-none shadow-[0_0_20px_var(--c-16-185-129-30)] disabled:opacity-50 disabled:shadow-none"
 >
 {verifyLoading ? <Loader2 className="w-5 h-5 animate-spin mr-2" /> : <CheckCircle2 className="w-5 h-5 mr-2" />}
 {t("mfa.verify_cta")}
 </Button>
 </form>
 </GlassCard>
 </div>
 )}
 </div>
 );
}
