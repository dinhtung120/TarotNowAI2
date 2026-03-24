/*
 * ===================================================================
 * FILE: wallet/withdraw/page.tsx (Rút Tiền)
 * BỐI CẢNH (CONTEXT):
 *   Trang cho phép User (thường là Reader/Admin có thu nhập) rút Kim Cương (Diamond) 
 *   về tài khoản ngân hàng VND.
 * 
 * BẢO MẬT & LUỒNG HOẠT ĐỘNG:
 *   - Tính toán phí rút (10%) và hiển thị trực quan (Gross/Net).
 *   - Yêu cầu xác thực MFA (MfaChallengeModal) trước khi submit.
 *   - Lưu yêu cầu rút tiền qua API `createWithdrawal`.
 *   - Liệt kê lịch sử rút tiền ở nửa dưới trang (`listMyWithdrawals`).
 * ===================================================================
 */
"use client";

import React from "react";
import {
  Sparkles,
  Loader2,
  CheckCircle2,
  AlertTriangle,
  Diamond,
  Building2,
  CreditCard,
  ArrowRight,
  Clock,
  ArrowLeft,
} from "lucide-react";
import MfaChallengeModal from "@/shared/components/auth/MfaChallengeModal";
import { useRouter } from "@/i18n/routing";
import { Button, GlassCard, SectionHeader } from "@/shared/components/ui";
import { useWithdrawPage } from "@/features/wallet/application/useWithdrawPage";

export default function WithdrawPage() {
  const router = useRouter();
  const {
    t,
    locale,
    amount,
    setAmount,
    bankName,
    setBankName,
    accountName,
    setAccountName,
    accountNumber,
    setAccountNumber,
    submitting,
    success,
    error,
    showMfa,
    setShowMfa,
    history,
    loadingHistory,
    amountNum,
    grossVnd,
    feeVnd,
    netVnd,
    getStatusBadge,
    handleSubmit,
    handleMfaSuccess,
  } = useWithdrawPage();

  return (
      <div className="max-w-3xl mx-auto px-4 sm:px-6 pt-8 pb-32 space-y-12 w-full">
        <button
          onClick={() => router.push("/wallet")}
          className="group flex items-center gap-2 text-[var(--text-secondary)] hover:tn-text-primary transition-colors text-[10px] font-black uppercase tracking-[0.2em] mb-8 w-fit min-h-11 px-2 rounded-xl hover:tn-surface-soft"
        >
          <ArrowLeft className="w-3.5 h-3.5 transition-transform group-hover:-translate-x-1" />
          {t("withdraw.back_to_wallet")}
        </button>

        <SectionHeader
          tag={t("withdraw.tag")}
          tagIcon={<Sparkles className="w-3 h-3 text-[var(--success)]" />}
          title={t("withdraw.title")}
          subtitle={t("withdraw.subtitle")}
          className="animate-in fade-in slide-in-from-bottom-4 duration-1000"
        />

        <GlassCard className="animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-200">
          <form onSubmit={handleSubmit} className="space-y-6">
            <div className="space-y-4">
              <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)] block">
                {t("withdraw.amount_label")}
              </label>
              <div className="relative">
                <Diamond className="absolute left-4 top-1/2 -translate-y-1/2 w-5 h-5 text-[var(--warning)]" />
                <input
                  type="number"
                  value={amount}
                  onChange={(e) => setAmount(e.target.value)}
                  placeholder={t("withdraw.amount_placeholder")}
                  min={50}
                  className="w-full pl-12 pr-4 py-4 tn-field rounded-2xl tn-text-primary text-xl font-black italic placeholder:tn-text-muted placeholder:not-italic tn-field-success transition-all font-sans"
                />
              </div>
            </div>

            {amountNum >= 50 && (
              <div className="p-5 bg-[var(--success)]/5 border border-[var(--success)]/20 rounded-2xl space-y-3 animate-in fade-in duration-300">
                <div className="flex justify-between items-center text-sm">
                  <span className="text-[var(--success)]/80 text-[10px] font-black uppercase tracking-widest">
                    {t("withdraw.gross_label")}
                  </span>
                  <span className="tn-text-primary font-bold">
                    {grossVnd.toLocaleString(locale)} ₫
                  </span>
                </div>
                <div className="flex justify-between items-center text-sm">
                  <span className="text-[var(--danger)]/80 text-[10px] font-black uppercase tracking-widest">
                    {t("withdraw.fee_label")}
                  </span>
                  <span className="text-[var(--danger)] font-bold">
                    -{feeVnd.toLocaleString(locale)} ₫
                  </span>
                </div>
                <div className="border-t border-[var(--success)]/15 pt-3 flex justify-between items-end">
                  <span className="text-[var(--success)] text-[11px] font-black uppercase tracking-widest">
                    {t("withdraw.net_label")}
                  </span>
                  <span className="text-[var(--success)] font-black text-2xl italic">
                    {netVnd.toLocaleString(locale)} ₫
                  </span>
                </div>
              </div>
            )}

            <div className="space-y-5 pt-4">
              <div className="space-y-3">
                <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)] block">
                  {t("withdraw.bank_label")}
                </label>
                <div className="relative">
                  <Building2 className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 tn-text-muted" />
                  <input
                    type="text"
                    value={bankName}
                    onChange={(e) => setBankName(e.target.value)}
                    placeholder={t("withdraw.bank_placeholder")}
                    className="w-full pl-12 pr-4 py-3 tn-field rounded-xl text-sm tn-text-primary placeholder:tn-text-muted tn-field-accent transition-all"
                  />
                </div>
              </div>

              <div className="space-y-3">
                <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)] block">
                  {t("withdraw.account_name_label")}
                </label>
                <input
                  type="text"
                  value={accountName}
                  onChange={(e) => setAccountName(e.target.value)}
                  placeholder={t("withdraw.account_name_placeholder")}
                  className="w-full px-4 py-3 tn-field rounded-xl text-sm tn-text-primary placeholder:tn-text-muted tn-field-accent transition-all uppercase"
                />
              </div>

              <div className="space-y-3">
                <label className="text-[10px] font-black uppercase tracking-[0.2em] text-[var(--text-secondary)] block">
                  {t("withdraw.account_number_label")}
                </label>
                <div className="relative">
                  <CreditCard className="absolute left-4 top-1/2 -translate-y-1/2 w-4 h-4 tn-text-muted" />
                  <input
                    type="text"
                    value={accountNumber}
                    onChange={(e) => setAccountNumber(e.target.value)}
                    placeholder={t("withdraw.account_number_placeholder")}
                    className="w-full pl-12 pr-4 py-3 tn-field rounded-xl text-sm tn-text-primary placeholder:tn-text-muted tn-field-accent transition-all"
                  />
                </div>
              </div>
            </div>

            <div className="space-y-3">
              {error && (
                <div className="flex items-center gap-2 p-4 rounded-xl bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-xs font-bold uppercase tracking-widest text-[var(--danger)] animate-in zoom-in-95">
                  <AlertTriangle className="w-4 h-4" /> {error}
                </div>
              )}

              {success && (
                <div className="flex items-center gap-2 p-4 rounded-xl bg-[var(--success)]/10 border border-[var(--success)]/20 text-xs font-bold uppercase tracking-widest text-[var(--success)] animate-in zoom-in-95">
                  <CheckCircle2 className="w-4 h-4" />{" "}
                  {t("withdraw.success_message")}
                </div>
              )}

              <Button
                variant="primary"
                type="submit"
                disabled={submitting || amountNum < 50}
                className="w-full h-14"
              >
                {submitting ? (
                  <>
                    <Loader2 className="w-4 h-4 animate-spin mr-2" />
                    {t("withdraw.submitting")}
                  </>
                ) : (
                  <>
                    <ArrowRight className="w-4 h-4 mr-2" />
                    {t("withdraw.submit")}
                  </>
                )}
              </Button>
            </div>
          </form>
        </GlassCard>

        <div className="space-y-6 pt-8 animate-in fade-in slide-in-from-bottom-8 duration-1000 delay-400">
          <h2 className="text-[11px] font-black tn-text-primary uppercase tracking-[0.3em] flex items-center gap-3">
            <Clock className="w-4 h-4 text-[var(--text-secondary)]" />{" "}
            {t("withdraw.history_title")}
          </h2>

          <GlassCard className="!p-0 overflow-hidden tn-border-soft">
            {loadingHistory && (
              <div className="flex items-center justify-center py-12">
                <Loader2 className="w-6 h-6 tn-text-muted animate-spin" />
              </div>
            )}

            {!loadingHistory && history.length === 0 && (
              <div className="text-[var(--text-secondary)] text-[11px] font-medium uppercase tracking-widest text-center py-12">
                {t("withdraw.history_empty")}
              </div>
            )}

            <div className="divide-y divide-white/5">
              {history.map((item) => {
                const badge = getStatusBadge(item.status);
                return (
                  <div
                    key={item.id}
                    className="p-4 sm:p-6 space-y-4 hover:tn-surface transition-colors"
                  >
                    <div className="flex items-start justify-between gap-4">
                      <div className="space-y-1 w-full">
                        <div className="flex flex-col sm:flex-row sm:items-center sm:justify-between gap-3">
                          <div className="flex flex-wrap items-center gap-2 sm:gap-3">
                            <div className="flex items-center gap-1.5 bg-[var(--warning)]/10 px-2 py-1 rounded text-[var(--warning)] font-black text-sm italic">
                              {item.amountDiamond}{" "}
                              <Diamond className="w-3.5 h-3.5" />
                            </div>
                            <ArrowRight className="w-3.5 h-3.5 tn-text-muted" />
                            <div className="font-black text-[var(--success)] text-lg italic tracking-tighter">
                              {item.netAmountVnd?.toLocaleString(locale)}{" "}
                              <span className="text-[10px] not-italic text-[var(--success)]/60">
                                ₫
                              </span>
                            </div>
                          </div>
                          <span
                            className={[
                              "w-fit px-2.5 py-1 rounded-full text-[8px] font-black uppercase tracking-widest border",
                              badge.className,
                            ].join(" ")}
                          >
                            {badge.text}
                          </span>
                        </div>
                      </div>
                    </div>

                    <div className="flex flex-col sm:flex-row sm:items-center justify-between gap-3 text-[10px] text-[var(--text-tertiary)] font-medium">
                      <div className="flex items-center gap-1.5 uppercase tracking-widest break-all">
                        <Building2 className="w-3 h-3 tn-text-muted" />
                        {item.bankName} • {item.bankAccountNumber}
                      </div>
                      <div className="flex items-center gap-1.5 font-bold tn-text-muted">
                        <Clock className="w-3 h-3" />
                        {new Date(item.createdAt).toLocaleDateString(locale)}
                      </div>
                    </div>

                    {item.adminNote && (
                      <div className="text-[10px] text-[var(--warning)] italic bg-[var(--warning)]/5 p-3 rounded-xl border border-[var(--warning)]/10 mt-3 flex gap-2 w-full">
                        <span className="font-bold opacity-70">
                          {t("withdraw.admin_note_prefix")}
                        </span>
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
          actionTitle={t("withdraw.action_title")}
          skipApiCall={true}
        />
      </div>
  );
}
