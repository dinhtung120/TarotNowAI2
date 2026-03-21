/*
 * ===================================================================
 * FILE: page.tsx (Admin Withdrawals)
 * BỐI CẢNH (CONTEXT):
 *   Giao diện duyệt lệnh rút tiền (Payout) dành cho Readers.
 * 
 * BẢO MẬT:
 *   Yêu cầu bắt buộc xác thực MFA bằng OTP trước khi Approve một lệnh rút tiền bất kỳ, tránh rủi ro
 *   bị lộ phiên đăng nhập Admin gây thất thoát luồng tiền hệ thống.
 * ===================================================================
 */
'use client';

import React from 'react';
import {
 Sparkles, Loader2, CheckCircle2, XCircle,
 Diamond, Building2, Clock, User
} from 'lucide-react';
import MfaChallengeModal from '@/components/auth/MfaChallengeModal';
import { SectionHeader, GlassCard, Button, Input } from '@/components/ui';
import { useLocale, useTranslations } from "next-intl";
import { useAdminWithdrawals } from '@/features/admin/withdrawals/application/useAdminWithdrawals';

/**
 * Admin Withdrawal Queue — danh sách yêu cầu rút tiền pending.
 * Admin approve hoặc reject.
 */
export default function AdminWithdrawalsPage() {
 const t = useTranslations("Admin");
 const locale = useLocale();
 const {
 queue,
 loading,
 processing,
 notes,
 setNotes,
 showMfa,
 setShowMfa,
 pendingAction,
 handleProcess,
 handleMfaSuccess,
 formatVnd,
 } = useAdminWithdrawals(t, locale);

 return (
 <div className="space-y-8 pb-20 animate-in fade-in duration-700 max-w-5xl mx-auto">
 {/* Header */}
 <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
 <SectionHeader
 tag={t("withdrawals.header.tag")}
 tagIcon={<Sparkles className="w-3 h-3 text-[var(--success)]" />}
 title={t("withdrawals.header.title")}
 subtitle={t("withdrawals.header.subtitle")}
 className="mb-0 text-left items-start"
 />
 </div>

 {/* Loading */}
 {loading && (
 <div className="flex items-center justify-center py-20">
 <div className="flex flex-col items-center justify-center space-y-4">
 <Loader2 className="w-8 h-8 animate-spin text-[var(--success)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("withdrawals.states.loading")}</span>
 </div>
 </div>
 )}

 {/* Empty */}
 {!loading && queue.length === 0 && (
 <GlassCard className="flex flex-col items-center justify-center py-20 text-center">
 <div className="w-16 h-16 rounded-full bg-[var(--success)]/10 border border-[var(--success)]/20 flex items-center justify-center mb-6 shadow-inner">
 <CheckCircle2 className="w-8 h-8 text-[var(--success)]" />
 </div>
 <p className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)]">{t("withdrawals.states.empty")}</p>
 </GlassCard>
 )}

 {/* Queue */}
 {!loading && queue.length > 0 && (
 <div className="space-y-6">
 {queue.map(item => (
 <GlassCard key={item.id} className="space-y-6 group hover:tn-border transition-all">
 {/* Row 1: Amount + User */}
 <div className="flex flex-col md:flex-row md:items-center justify-between gap-4">
 <div className="flex items-center gap-4">
 <div className="w-12 h-12 rounded-[1rem] bg-[var(--warning)]/10 border border-[var(--warning)]/20 flex items-center justify-center shadow-inner group-hover:scale-110 transition-transform">
 <Diamond className="w-6 h-6 text-[var(--warning)]" />
 </div>
 <div>
 <div className="text-2xl font-black tn-text-primary italic tracking-tighter drop-shadow-sm">{item.amountDiamond} <span className="text-xl text-[var(--warning)]">💎</span></div>
 <div className="text-[10px] font-bold text-[var(--text-secondary)] mt-1 flex gap-2 items-center">
 <span>{t("withdrawals.row.gross")}: {formatVnd(item.amountVnd)}</span>
 <span className="opacity-50">|</span> <span>{t("withdrawals.row.fee")}: {formatVnd(item.feeVnd)}</span>
 <span className="opacity-50">|</span> <span className="text-[var(--success)] drop-shadow-sm">{t("withdrawals.row.net")}: {formatVnd(item.netAmountVnd)}</span>
 </div>
 </div>
 </div>
 <div className="flex flex-col items-start md:items-end gap-2 text-left md:text-right tn-surface p-3 rounded-2xl border tn-border-soft shadow-inner shrink-0">
 <div className="flex items-center gap-2 text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-tighter">
 <Clock className="w-3.5 h-3.5" />
 {new Date(item.createdAt).toLocaleString(locale)}
 </div>
 <div className="flex items-center gap-2 text-[10px] font-bold text-[var(--text-tertiary)] italic">
 <User className="w-3.5 h-3.5" />
 {t("withdrawals.row.id_prefix", { id: item.userId.substring(0, 8) })}
 </div>
 </div>
 </div>

 {/* Row 2: Bank Info */}
 <div className="flex items-center gap-4 p-4 tn-panel-soft rounded-2xl shadow-inner">
 <div className="w-8 h-8 rounded-full tn-surface flex items-center justify-center shrink-0">
 <Building2 className="w-4 h-4 text-[var(--text-secondary)]" />
 </div>
 <div className="text-xs text-[var(--text-tertiary)] flex flex-wrap items-center gap-2">
 <span className="font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm">{item.bankName}</span>
 <span className="text-[color:var(--c-61-49-80-20)]">&bull;</span>
 <span className="font-bold">{item.bankAccountName}</span>
 <span className="text-[color:var(--c-61-49-80-20)]">&bull;</span>
 <span className="font-mono text-[var(--text-secondary)] tn-surface px-2 py-0.5 rounded-md border tn-border">{item.bankAccountNumber}</span>
 </div>
 </div>

 {/* Row 3: Admin Note + Actions */}
 <div className="flex flex-col md:flex-row items-center gap-4 pt-2 border-t tn-border-soft">
 <Input
 leftIcon={<User className="w-4 h-4" />}
 placeholder={t("withdrawals.input.admin_note_placeholder")}
 value={notes[item.id] || ''}
 onChange={e => setNotes(prev => ({ ...prev, [item.id]: e.target.value }))}
 className="flex-1 w-full text-xs font-black uppercase tracking-widest tn-text-primary shadow-inner tn-panel-soft"
 />
 <div className="flex items-center gap-3 w-full md:w-auto shrink-0">
 <Button
 variant="primary"
 onClick={() => handleProcess(item.id, 'approve')}
 disabled={processing === item.id}
 className="flex-1 md:flex-none py-3 shadow-md bg-[var(--success)] tn-text-primary hover:bg-[var(--success)] hover:brightness-110"
 >
 {processing === item.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <CheckCircle2 className="w-4 h-4" />}
 {t("withdrawals.actions.approve")}
 </Button>
 <Button
 variant="danger"
 onClick={() => handleProcess(item.id, 'reject')}
 disabled={processing === item.id}
 className="flex-1 md:flex-none py-3 shadow-md"
 >
 {processing === item.id ? <Loader2 className="w-4 h-4 animate-spin" /> : <XCircle className="w-4 h-4" />}
 {t("withdrawals.actions.reject")}
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
 actionTitle={pendingAction?.action === 'approve' ? t("withdrawals.mfa.action_approve") : t("withdrawals.mfa.action_reject")}
 skipApiCall={true}
 />
 </div>
 );
}
