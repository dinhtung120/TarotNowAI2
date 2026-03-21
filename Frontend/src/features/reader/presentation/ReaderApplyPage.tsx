'use client';

import React from 'react';
import {
 FileText, Send, Clock, CheckCircle2, XCircle, Loader2, Sparkles, Star, ScrollText
} from 'lucide-react';
import { useLocale, useTranslations } from 'next-intl';
import { useReaderApplyPage } from '@/features/reader/application/useReaderApplyPage';

/*
 * ===================================================================
 * FILE: (user)/reader/apply/page.tsx (Đăng ký Reader)
 * BỐI CẢNH (CONTEXT):
 *   Trang cho phép User (Người dùng thường) nộp đơn ứng tuyển làm Reader (Người đọc Tarot).
 * 
 * LUỒNG HOẠT ĐỘNG:
 *   - Fetch trạng thái đơn (Pending, Approved, Rejected) ngay khi load.
 *   - Nếu Pending/Approved: Hiện thi thông báo trạng thái.
 *   - Nếu Chưa có hoặc Rejected: Hiện Form điền giới thiệu bản thân (Tối thiểu 20 ký tự).
 *   - Submit qua Server Action `submitReaderApplication`.
 * ===================================================================
 */
export default function ReaderApplyPage() {
 const t = useTranslations("ReaderApply");
 const locale = useLocale();
 const {
 introText,
 setIntroText,
 submitting,
 message,
 messageType,
 existingRequest,
 loading,
 handleSubmit,
 } = useReaderApplyPage(t);

 // Loading state
 if (loading) {
 return (
 <div className="min-h-[60vh] flex flex-col items-center justify-center space-y-4">
 <Loader2 className="w-10 h-10 text-[var(--purple-accent)] animate-spin" />
 <span className="text-[10px] font-black uppercase tracking-[0.3em] tn-text-muted">
 {t("loading")}
 </span>
 </div>
 );
 }

 // Nếu đã có đơn pending → hiển thị status card
 if (existingRequest?.hasRequest && existingRequest.status === 'pending') {
 return (
 <div className="max-w-2xl mx-auto px-4 sm:px-6 py-20 animate-in fade-in slide-in-from-bottom-8 duration-1000">
 <div className="relative overflow-hidden bg-gradient-to-br from-[var(--warning)]/10 to-transparent rounded-[3rem] border border-[var(--warning)]/20 p-12 shadow-2xl">
 <div className="absolute top-0 right-0 p-10 opacity-10 pointer-events-none">
 <Clock size={180} className="text-[var(--warning)]" />
 </div>
 <div className="relative z-10 space-y-6 text-center">
 <div className="w-16 h-16 mx-auto rounded-2xl bg-[var(--warning)]/20 flex items-center justify-center border border-[var(--warning)]/30">
 <Clock className="w-8 h-8 text-[var(--warning)]" />
 </div>
 <h1 className="text-3xl font-black tn-text-primary uppercase italic tracking-tighter">
 {t("pending.title")}
 </h1>
 <p className="tn-text-secondary text-sm leading-relaxed max-w-md mx-auto">
 {t("pending.desc")}
 </p>
 <div className="p-6 rounded-2xl tn-panel-overlay-soft text-left space-y-2">
 <div className="text-[10px] font-black uppercase tracking-widest text-[var(--warning)]">{t("pending.intro_label")}</div>
 <p className="text-xs tn-text-secondary leading-relaxed">{existingRequest.introText}</p>
 </div>
 <div className="text-[10px] font-black uppercase tracking-[0.2em] tn-text-muted">
 {t("pending.sent_at", { date: new Date(existingRequest.createdAt || '').toLocaleString(locale) })}
 </div>
 </div>
 </div>
 </div>
 );
 }

 // Nếu đã được approved
 if (existingRequest?.hasRequest && existingRequest.status === 'approved') {
 return (
 <div className="max-w-2xl mx-auto px-4 sm:px-6 py-20 animate-in fade-in slide-in-from-bottom-8 duration-1000">
 <div className="relative overflow-hidden bg-gradient-to-br from-[var(--success)]/10 to-transparent rounded-[3rem] border border-[var(--success)]/20 p-12 shadow-2xl">
 <div className="absolute top-0 right-0 p-10 opacity-10 pointer-events-none">
 <CheckCircle2 size={180} className="text-[var(--success)]" />
 </div>
 <div className="relative z-10 space-y-6 text-center">
 <div className="w-16 h-16 mx-auto rounded-2xl bg-[var(--success)]/20 flex items-center justify-center border border-[var(--success)]/30">
 <CheckCircle2 className="w-8 h-8 text-[var(--success)]" />
 </div>
 <h1 className="text-3xl font-black tn-text-primary uppercase italic tracking-tighter">
 {t("approved.title")}
 </h1>
 <p className="tn-text-secondary text-sm leading-relaxed max-w-md mx-auto">
 {t("approved.desc")}
 </p>
 </div>
 </div>
 </div>
 );
 }

 // Nếu bị rejected → cho phép submit lại
 const wasRejected = existingRequest?.hasRequest && existingRequest.status === 'rejected';

 // Form đăng ký
 const iconClassByColor: Record<"purple" | "amber" | "emerald", string> = {
  purple: "text-[var(--purple-accent)]",
  amber: "text-[var(--warning)]",
  emerald: "text-[var(--success)]",
 };

 return (
 <div className="max-w-2xl mx-auto px-4 sm:px-6 py-20 animate-in fade-in slide-in-from-bottom-8 duration-1000">
 <div className="space-y-10">
 {/* Header */}
 <div className="text-center space-y-4">
 <div className="inline-flex items-center gap-2 px-4 py-1.5 rounded-full bg-[var(--purple-accent)]/5 border border-[var(--purple-accent)]/10 text-[9px] uppercase tracking-[0.2em] font-black text-[var(--purple-accent)] shadow-xl ">
 <Sparkles className="w-3 h-3 text-[var(--purple-accent)]" />
 {t("header.tag")}
 </div>
 <h1 className="text-4xl md:text-5xl font-black tracking-tighter tn-text-primary uppercase italic">
 {t("header.title")}
 </h1>
 <p className="tn-text-muted font-medium max-w-lg mx-auto text-sm leading-relaxed">
 {t("header.subtitle")}
 </p>
 </div>

 {/* Rejected Notice */}
 {wasRejected && (
 <div className="p-6 rounded-2xl bg-[var(--danger)]/5 border border-[var(--danger)]/20 space-y-2">
 <div className="flex items-center gap-2">
 <XCircle className="w-4 h-4 text-[var(--danger)]" />
 <span className="text-[10px] font-black uppercase tracking-widest text-[var(--danger)]">{t("rejected.title")}</span>
 </div>
 {existingRequest?.adminNote && (
 <p className="text-xs tn-text-secondary leading-relaxed">{t("rejected.reason", { note: existingRequest.adminNote })}</p>
 )}
 <p className="text-xs tn-text-muted">{t("rejected.can_resubmit")}</p>
 </div>
 )}

 {/* Form */}
 <form onSubmit={handleSubmit} className="space-y-8">
 {/* Info Cards */}
 <div className="grid grid-cols-1 md:grid-cols-3 gap-4">
 {[
 { icon: ScrollText, title: t("steps.intro.title"), desc: t("steps.intro.desc"), color: 'purple' as const },
 { icon: Clock, title: t("steps.review.title"), desc: t("steps.review.desc"), color: 'amber' as const },
 { icon: Star, title: t("steps.start.title"), desc: t("steps.start.desc"), color: 'emerald' as const }
 ].map((step) => (
 <div key={step.title} className="p-5 rounded-2xl tn-panel-soft text-center space-y-2">
 <step.icon className={`w-6 h-6 mx-auto ${iconClassByColor[step.color]}`} />
 <div className="text-[10px] font-black uppercase tracking-widest tn-text-secondary">{step.title}</div>
 <p className="text-[10px] tn-text-muted">{step.desc}</p>
 </div>
 ))}
 </div>

 {/* Textarea */}
 <div className="space-y-3">
 <label className="flex items-center gap-2 text-[10px] font-black uppercase tracking-widest tn-text-secondary">
 <FileText className="w-3 h-3" />
 {t("form.intro_label")}
 </label>
 <textarea
 id="reader-intro-text"
 value={introText}
 onChange={(e) => setIntroText(e.target.value)}
 placeholder={t("form.intro_placeholder")}
 rows={6}
 className="w-full tn-field rounded-2xl p-6 text-sm tn-text-primary placeholder:tn-text-muted tn-field-accent transition-all resize-none"
 />
 <div className="flex justify-between">
 <span className={`text-[10px] ${introText.length >= 20 ? 'tn-text-muted' : 'text-[var(--danger)]'}`}>
 {t("form.min_chars")}
 </span>
 <span className="text-[10px] tn-text-muted">{introText.length}/2000</span>
 </div>
 </div>

 {/* Message */}
 {message && (
 <div className={`p-4 rounded-xl text-sm ${
 messageType === 'success' ? 'bg-[var(--success)]/10 border border-[var(--success)]/20 text-[var(--success)]' : 'bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-[var(--danger)]'
 }`}>
 {message}
 </div>
 )}

 {/* Submit Button */}
 <button
 id="reader-submit-btn"
 type="submit"
 disabled={submitting || introText.length < 20}
 className="w-full group relative overflow-hidden bg-gradient-to-r from-[var(--purple-accent)] to-[var(--purple-accent)] hover:from-[var(--purple-accent)] hover:to-[var(--purple-accent)] disabled:from-[var(--text-disabled)] disabled:to-[var(--text-muted)] rounded-2xl p-5 text-center font-black uppercase tracking-widest text-sm tn-text-primary transition-all duration-300 shadow-xl disabled:cursor-not-allowed"
 >
 {submitting ? (
 <span className="flex items-center justify-center gap-3">
 <Loader2 className="w-4 h-4 animate-spin" />
 {t("form.submitting")}
 </span>
 ) : (
 <span className="flex items-center justify-center gap-3">
 <Send className="w-4 h-4" />
 {t("form.submit")}
 </span>
 )}
 </button>
 </form>
 </div>
 </div>
 );
}
