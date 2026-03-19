/*
 * ===================================================================
 * FILE: page.tsx (Admin Disputes)
 * BỐI CẢNH (CONTEXT):
 *   Giao diện xử lý Khiếu nại (Dispute) - Khi User và Reader có tranh chấp.
 *   Admin quyết định 1 trong 2 hành động tối thượng:
 *   - RELEASE: Chuyển tiền đóng băng cho Reader.
 *   - REFUND: Hoàn tiền lại cho User (Không tính phí).
 * ===================================================================
 */
'use client';

import React, { useState } from 'react';
import { resolveDispute } from '@/actions/escrowActions';
import {
 AlertTriangle, Loader2, CheckCircle2, Sparkles,
 Shield, ArrowRight
} from 'lucide-react';
import { SectionHeader, GlassCard, Button, Input } from '@/components/ui';
import toast from 'react-hot-toast';
import { useTranslations } from "next-intl";

/**
 * Admin Dispute Queue — danh sách disputes cần xử lý.
 *
 * Hiện tại sử dụng escrow status API.
 * Admin resolve: release cho reader hoặc refund cho user.
 */

export default function AdminDisputesPage() {
 const t = useTranslations("Admin");
 const [processing, setProcessing] = useState<string | null>(null);
 const [resolved, setResolved] = useState<Set<string>>(new Set());
 const [itemId, setItemId] = useState('');
 const [note, setNote] = useState('');

 const handleResolve = async (itemId: string, action: 'release' | 'refund') => {
 setProcessing(itemId);
 const ok = await resolveDispute({ itemId, action, adminNote: note || undefined });
 if (ok) {
 toast.success(action === 'release' ? t("disputes.toast.release_success") : t("disputes.toast.refund_success"));
 setResolved(prev => new Set(prev).add(itemId));
 } else {
 toast.error(t("disputes.toast.failed"));
 }
 setProcessing(null);
 setNote('');
 };

 return (
 <div className="max-w-4xl mx-auto px-4 sm:px-6 py-16 space-y-10 animate-in fade-in duration-700">
 {/* Header */}
 <div className="flex flex-col md:flex-row md:items-end justify-between gap-6">
 <SectionHeader
 tag={t("disputes.header.tag")}
 tagIcon={<Sparkles className="w-3 h-3 text-[var(--danger)]" />}
 title={t("disputes.header.title")}
 subtitle={t("disputes.header.subtitle")}
 className="mb-0 text-left items-start"
 />
 </div>

 {/* Info Card */}
 <GlassCard className="space-y-4 shadow-inner !p-6">
 <div className="flex items-center gap-3">
 <div className="p-2 rounded-xl bg-[var(--warning)]/10 border border-[var(--warning)]/20 shadow-inner">
 <AlertTriangle className="w-5 h-5 text-[var(--warning)]" />
 </div>
 <span className="text-sm font-black tn-text-primary uppercase tracking-widest drop-shadow-sm">{t("disputes.guide.title")}</span>
 </div>
 <ul className="text-xs text-[var(--text-secondary)] space-y-2 ml-10 list-disc font-medium leading-relaxed">
 <li><strong className="text-[var(--success)] uppercase tracking-tighter">{t("disputes.guide.release_label")}</strong>: {t("disputes.guide.release_desc")}</li>
 <li><strong className="text-[var(--accent)] uppercase tracking-tighter">{t("disputes.guide.refund_label")}</strong>: {t("disputes.guide.refund_desc")}</li>
 <li>{t("disputes.guide.check_history")}</li>
 </ul>
 </GlassCard>

 {/* Dispute Resolution Form */}
 <GlassCard className="space-y-6 !p-8">
 <h3 className="text-sm font-black tn-text-primary uppercase tracking-widest flex items-center gap-2">
 <Shield className="w-4 h-4 text-[var(--purple-accent)]" />
 {t("disputes.form.quick_title")}
 </h3>

 <div className="space-y-4">
 <div className="space-y-2">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("disputes.form.item_id_label")}</label>
 <Input
 id="dispute-item-id"
 placeholder={t("disputes.form.item_id_placeholder")}
 value={itemId}
 onChange={e => setItemId(e.target.value)}
 className="w-full text-xs font-black tracking-widest tn-text-primary shadow-inner"
 />
 </div>

 <div className="space-y-2">
 <label className="text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)]">{t("disputes.form.note_label")}</label>
 <textarea
 value={note}
 onChange={e => setNote(e.target.value)}
 placeholder={t("disputes.form.note_placeholder")}
 rows={3}
 className="w-full tn-field rounded-2xl p-4 text-xs font-bold tn-text-primary placeholder:tn-text-muted tn-field-accent transition-all resize-none shadow-inner"
 />
 </div>

 <div className="flex flex-col sm:flex-row gap-4 pt-2">
 <Button
 variant="primary"
 onClick={() => {
 const trimmed = itemId.trim();
 if (trimmed) void handleResolve(trimmed, 'release');
 else toast.error(t("disputes.toast.missing_item_id"));
 }}
 disabled={processing !== null}
 className="flex-1 py-4 bg-[var(--success)] hover:bg-[var(--success)]/90 tn-text-primary shadow-[0_0_20px_var(--c-16-185-129-20)]"
 >
 {processing === itemId.trim() ? <Loader2 className="w-4 h-4 animate-spin" /> : <CheckCircle2 className="w-4 h-4" />}
 {t("disputes.form.release_button")}
 </Button>

 <Button
 variant="secondary"
 onClick={() => {
 const trimmed = itemId.trim();
 if (trimmed) void handleResolve(trimmed, 'refund');
 else toast.error(t("disputes.toast.missing_item_id"));
 }}
 disabled={processing !== null}
 className="flex-1 py-4 border-[var(--accent)]/30 text-[var(--accent)] hover:bg-[var(--accent)]/10"
 >
 {processing === itemId.trim() ? <Loader2 className="w-4 h-4 animate-spin" /> : <ArrowRight className="w-4 h-4" />}
 {t("disputes.form.refund_button")}
 </Button>
 </div>
 </div>
 </GlassCard>

 {/* Resolved list */}
 {resolved.size > 0 && (
 <div className="space-y-4 animate-in fade-in slide-in-from-bottom-4">
 <h3 className="text-[10px] font-black uppercase tracking-widest text-[var(--text-tertiary)] flex items-center gap-2">
 <CheckCircle2 className="w-3 h-3 text-[var(--success)]" />
 {t("disputes.resolved.title")}
 </h3>
 <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-3">
 {Array.from(resolved).map(id => (
 <div key={id} className="flex flex-col gap-1 p-4 bg-[var(--success)]/5 border border-[var(--success)]/10 rounded-2xl shadow-inner">
 <div className="flex items-center gap-2">
 <CheckCircle2 className="w-4 h-4 text-[var(--success)] drop-shadow-sm" />
 <span className="text-xs font-black text-[var(--success)] tracking-tighter uppercase drop-shadow-sm">{t("disputes.resolved.badge")}</span>
 </div>
 <span className="text-[10px] text-[var(--text-secondary)] font-mono truncate">{id}</span>
 </div>
 ))}
 </div>
 </div>
 )}
 </div>
 );
}
