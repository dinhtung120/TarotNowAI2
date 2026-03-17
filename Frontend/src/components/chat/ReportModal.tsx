'use client';

import React, { useState } from 'react';
import { sendReport } from '@/actions/chatActions';
import { X, Flag, Loader2, CheckCircle2 } from 'lucide-react';
import { useTranslations } from 'next-intl';

/**
 * Report Modal — báo cáo vi phạm trong cuộc trò chuyện.
 *
 * Hiển thị form:
 * → Chọn loại vi phạm (tin nhắn, cuộc trò chuyện, user).
 * → Nhập lý do chi tiết (tối thiểu 10 ký tự).
 * → Submit qua server action.
 * → Hiện success/error state.
 */
interface ReportModalProps {
 conversationId: string;
 onClose: () => void;
}

export default function ReportModal({ conversationId, onClose }: ReportModalProps) {
 const [targetType, setTargetType] = useState('conversation');
 const [reason, setReason] = useState('');
 const [submitting, setSubmitting] = useState(false);
 const [success, setSuccess] = useState(false);
 const [error, setError] = useState('');
 const t = useTranslations('Chat');
 const tCommon = useTranslations('Common');

 const handleSubmit = async () => {
 if (reason.length < 10) {
 setError(t('report.validation_min'));
 return;
 }

 setSubmitting(true);
 setError('');

 const result = await sendReport({
 targetType,
 targetId: conversationId,
 conversationRef: conversationId,
 reason,
 });

 if (result) {
 setSuccess(true);
 } else {
 setError(t('report.error_failed'));
 }
 setSubmitting(false);
 };

 // Success state
 if (success) {
 return (
 <div className="fixed inset-0 z-50 flex items-center justify-center tn-overlay "
 onClick={onClose}>
	 <div className="tn-surface-strong rounded-3xl border tn-border p-10 max-w-md mx-4 text-center space-y-4"
	 onClick={e => e.stopPropagation()}>
	 <CheckCircle2 className="w-12 h-12 text-[var(--success)] mx-auto" />
	 <h3 className="text-lg font-black tn-text-primary uppercase italic">{t('report.success_title')}</h3>
	 <p className="text-xs tn-text-muted">{t('report.success_desc')}</p>
	 <button
	 onClick={onClose}
	 className="px-6 py-2 rounded-xl bg-[var(--purple-accent)] hover:bg-[var(--purple-accent)] tn-text-primary text-xs font-bold uppercase tracking-widest transition-all"
	 >
	 {tCommon('close')}
	 </button>
	 </div>
 </div>
 );
 }

 return (
 <div className="fixed inset-0 z-50 flex items-center justify-center tn-overlay "
 onClick={onClose}>
 <div className="tn-surface-strong rounded-3xl border tn-border p-8 max-w-md mx-4 w-full space-y-6"
 onClick={e => e.stopPropagation()}>
 {/* Header */}
 <div className="flex items-center justify-between">
	 <div className="flex items-center gap-2">
	 <Flag className="w-4 h-4 text-[var(--danger)]" />
	 <h3 className="text-sm font-black tn-text-primary uppercase tracking-widest">{t('report.title')}</h3>
	 </div>
 <button onClick={onClose} className="p-1 hover:tn-surface-strong rounded-lg transition-colors">
 <X className="w-4 h-4 tn-text-muted" />
 </button>
 </div>

	 {/* Target Type */}
	 <div className="space-y-2">
	 <label className="text-[10px] font-black uppercase tracking-widest tn-text-muted">{t('report.target_type_label')}</label>
	 <div className="flex gap-2">
	 {[
	 { value: 'conversation', label: t('report.target_conversation') },
	 { value: 'user', label: t('report.target_user') },
	 { value: 'message', label: t('report.target_message') },
	 ].map(opt => (
 <button
 key={opt.value}
 onClick={() => setTargetType(opt.value)}
 className={`flex-1 px-3 py-2 rounded-xl text-[10px] font-bold uppercase tracking-wider transition-all border ${
 targetType === opt.value
 ? 'bg-[var(--danger)]/10 border-[var(--danger)]/30 text-[var(--danger)]'
 : 'tn-panel-soft tn-text-muted'
 }`}
 >
 {opt.label}
 </button>
 ))}
 </div>
 </div>

	 {/* Reason */}
	 <div className="space-y-2">
	 <label className="text-[10px] font-black uppercase tracking-widest tn-text-muted">{t('report.reason_label')}</label>
	 <textarea
	 value={reason}
	 onChange={(e) => setReason(e.target.value)}
	 placeholder={t('report.reason_placeholder')}
	 rows={4}
	 className="w-full tn-field rounded-xl p-4 text-sm tn-text-primary placeholder:tn-text-muted tn-field-danger transition-all resize-none"
	 />
 <span className={`text-[10px] ${reason.length >= 10 ? 'tn-text-muted' : 'text-[var(--danger)]'}`}>
 {reason.length}/500
 </span>
 </div>

 {/* Error */}
 {error && (
 <div className="p-3 rounded-lg bg-[var(--danger)]/10 border border-[var(--danger)]/20 text-xs text-[var(--danger)]">
 {error}
 </div>
 )}

 {/* Submit */}
 <button
 onClick={handleSubmit}
 disabled={submitting || reason.length < 10}
 className="w-full flex items-center justify-center gap-2 p-3 rounded-xl bg-[var(--danger)]/20 hover:bg-[var(--danger)]/30 border border-[var(--danger)]/20 text-[var(--danger)] text-[10px] font-black uppercase tracking-widest transition-all disabled:opacity-50"
 >
	 {submitting ? <Loader2 className="w-3 h-3 animate-spin" /> : <Flag className="w-3 h-3" />}
	 {t('report.submit')}
	 </button>
 </div>
 </div>
 );
}
