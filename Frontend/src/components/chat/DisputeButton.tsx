'use client';

import React, { useEffect, useState } from 'react';
import { openDispute } from '@/actions/escrowActions';
import { AlertTriangle, Loader2, CheckCircle2, X } from 'lucide-react';
import { useTranslations } from 'next-intl';

/*
 * ===================================================================
 * COMPONENT: DisputeButton
 * BỐI CẢNH (CONTEXT):
 *   Nút CTA (Call-to-Action) hiển thị trong khung Chat, cho phép User mở 
 *   tranh chấp (Dispute) nếu có vấn đề với câu trả lời của Reader.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Hiển thị khi trạng thái item là 'released' và vẫn còn trong thời gian 
 *     cho phép tranh chấp (disputeWindowEnd > now).
 *   - Có đồng hồ đếm ngược (Countdown) cập nhật tự động bằng `setInterval`.
 *   - Khi click, mở Modal để điền lý do vi phạm (tối thiểu 10 ký tự) và gọi 
 *     API `openDispute`.
 * ===================================================================
 */
interface DisputeButtonProps {
 itemId: string;
 disputeWindowEnd: string;
 onDisputeOpened?: () => void;
}

export default function DisputeButton({ itemId, disputeWindowEnd, onDisputeOpened }: DisputeButtonProps) {
 const [showModal, setShowModal] = useState(false);
 const [reason, setReason] = useState('');
 const [submitting, setSubmitting] = useState(false);
 const [success, setSuccess] = useState(false);
 const [error, setError] = useState('');
 const [timeLeft, setTimeLeft] = useState<number | null>(null);
 const t = useTranslations('Chat');
 const tCommon = useTranslations('Common');

 useEffect(() => {
 const updateTimeLeft = () => {
 setTimeLeft(new Date(disputeWindowEnd).getTime() - Date.now());
 };

 const bootstrapTimer = window.setTimeout(updateTimeLeft, 0);
 const intervalTimer = window.setInterval(updateTimeLeft, 60000);

 return () => {
 window.clearTimeout(bootstrapTimer);
 window.clearInterval(intervalTimer);
 };
 }, [disputeWindowEnd]);

 // Kiểm tra còn trong window không
 if (timeLeft === null || timeLeft <= 0) return null;

 const hrs = Math.floor(timeLeft / 3600000);
 const mins = Math.floor((timeLeft % 3600000) / 60000);

 const handleSubmit = async () => {
 if (reason.length < 10) {
 setError(t('dispute.validation_min'));
 return;
 }
 setSubmitting(true);
 setError('');
 const ok = await openDispute(itemId, reason);
 if (ok) {
 setSuccess(true);
 onDisputeOpened?.();
 } else {
 setError(t('dispute.error_failed'));
 }
 setSubmitting(false);
 };

 return (
 <>
 {/* CTA Button */}
	 <button
	 onClick={() => setShowModal(true)}
	 className="flex items-center gap-1 px-3 py-1.5 rounded-lg bg-[var(--danger)]/10 hover:bg-[var(--danger)]/20 border border-[var(--danger)]/20 text-[var(--danger)] text-[9px] font-bold uppercase tracking-wider transition-all"
	 >
	 <AlertTriangle className="w-3 h-3" />
	 {t('dispute.cta', { hours: hrs, minutes: mins })}
	 </button>

 {/* Modal */}
 {showModal && (
 <div className="fixed inset-0 z-50 flex items-center justify-center tn-overlay "
 onClick={() => setShowModal(false)}>
 <div className="tn-surface-strong rounded-3xl border tn-border p-8 max-w-md mx-4 w-full space-y-5"
 onClick={e => e.stopPropagation()}>

 {success ? (
	 <div className="text-center space-y-3">
	 <CheckCircle2 className="w-10 h-10 text-[var(--success)] mx-auto" />
	 <h3 className="text-sm font-black tn-text-primary uppercase">{t('dispute.success_title')}</h3>
	 <p className="text-[10px] tn-text-muted">{t('dispute.success_desc')}</p>
	 <button onClick={() => setShowModal(false)}
	 className="px-5 py-2 rounded-xl bg-[var(--purple-accent)] tn-text-primary text-xs font-bold uppercase">
	 {tCommon('close')}
	 </button>
	 </div>
 ) : (
 <>
 <div className="flex items-center justify-between">
	 <div className="flex items-center gap-2">
	 <AlertTriangle className="w-4 h-4 text-[var(--danger)]" />
	 <h3 className="text-sm font-black tn-text-primary uppercase tracking-widest">{t('dispute.title')}</h3>
	 </div>
 <button onClick={() => setShowModal(false)} className="p-1 hover:tn-surface-strong rounded-lg">
 <X className="w-4 h-4 tn-text-muted" />
 </button>
 </div>

	 <div className="p-3 rounded-lg bg-[var(--danger)]/5 border border-[var(--danger)]/10 text-[10px] text-[var(--danger)]">
	 {t('dispute.remaining', { hours: hrs, minutes: mins })}
	 </div>

	 <div className="space-y-2">
	 <label className="text-[10px] font-black uppercase tracking-widest tn-text-muted">{t('dispute.reason_label')}</label>
	 <textarea
	 value={reason}
	 onChange={e => setReason(e.target.value)}
	 placeholder={t('dispute.reason_placeholder')}
	 rows={3}
	 className="w-full tn-field rounded-xl p-3 text-sm tn-text-primary placeholder:tn-text-muted tn-field-danger transition-all resize-none"
	 />
	 </div>

 {error && <div className="text-xs text-[var(--danger)]">{error}</div>}

 <button
 onClick={handleSubmit}
 disabled={submitting || reason.length < 10}
 className="w-full flex items-center justify-center gap-2 p-3 rounded-xl bg-[var(--danger)]/20 hover:bg-[var(--danger)]/30 border border-[var(--danger)]/20 text-[var(--danger)] text-[10px] font-black uppercase tracking-widest disabled:opacity-50"
 >
	 {submitting ? <Loader2 className="w-3 h-3 animate-spin" /> : <AlertTriangle className="w-3 h-3" />}
	 {t('dispute.submit')}
	 </button>
 </>
 )}
 </div>
 </div>
 )}
 </>
 );
}
