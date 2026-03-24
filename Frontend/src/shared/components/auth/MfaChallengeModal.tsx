'use client';

import React, { useState } from 'react';
import { ShieldAlert, Loader2, X } from 'lucide-react';
import { useTranslations } from 'next-intl';

type MfaChallengeResult = {
 success: boolean;
 error?: string;
};

interface MfaChallengeModalProps {
 isOpen: boolean;
 onClose: () => void;
 onSuccess: (code: string) => void;
 actionTitle?: string;
 skipApiCall?: boolean;
 onChallenge?: (code: string) => Promise<MfaChallengeResult>;
}

/*
 * ===================================================================
 * COMPONENT: MfaChallengeModal
 * BỐI CẢNH (CONTEXT):
 *   Modal bảo mật (Popup) yêu cầu User nhập mã OTP từ ứng dụng Authenticator 
 *   trước khi thực hiện hành động nhạy cảm (Ví dụ: Thêm thẻ ngân hàng, Rút tiền).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Nhập OTP 6 số và gọi `challengeMfa` (nếu không bật `skipApiCall`).
 *   - Sau khi verify thành công, gọi callback `onSuccess(code)` để truyền 
 *     MFA token về parent component để tiếp tục luồng giao dịch.
 * ===================================================================
 */
export default function MfaChallengeModal({
 isOpen,
 onClose,
 onSuccess,
 actionTitle,
 skipApiCall = false,
 onChallenge,
}: MfaChallengeModalProps) {
 const t = useTranslations("Auth");
 const tCommon = useTranslations("Common");
 const [code, setCode] = useState('');
 const [loading, setLoading] = useState(false);
 const [error, setError] = useState('');

 if (!isOpen) return null;

 const actionTitleText = actionTitle ?? t("mfa.action_default");

 const resetFormState = () => {
 setCode('');
 setError('');
 setLoading(false);
 };

 const handleClose = () => {
 resetFormState();
 onClose();
 };

 const handleSubmit = async (e: React.FormEvent) => {
 e.preventDefault();
 if (code.length < 6) return;

 setLoading(true);
 setError('');

 if (skipApiCall) {
 resetFormState();
 onSuccess(code);
 return;
 }

 if (!onChallenge) {
 setError(t("mfa.error_generic"));
 setLoading(false);
 return;
 }

 const res = await onChallenge(code);
 if (res.success) {
 resetFormState();
 onSuccess(code);
 } else {
 setError(res.error || t("mfa.error_generic"));
 setLoading(false);
 }
 };

 return (
 <div className="fixed inset-0 z-50 flex items-center justify-center p-4 tn-overlay-strong animate-in fade-in duration-200">
 <div className="relative w-full max-w-sm tn-surface-strong border tn-border rounded-3xl p-6 shadow-2xl animate-in zoom-in-95 duration-200">
 {/* Close Button */}
 <button onClick={handleClose}
 aria-label={tCommon("close")}
 className="absolute top-4 right-4 p-2 rounded-full hover:tn-surface-strong tn-text-muted hover:tn-text-primary transition-colors"
 >
 <X className="w-5 h-5" />
 </button>

 <div className="text-center space-y-6">
 <div className="mx-auto w-16 h-16 rounded-full bg-[var(--success)]/10 border border-[var(--success)]/20 flex items-center justify-center">
 <ShieldAlert className="w-8 h-8 text-[var(--success)]" />
 </div>

 <div className="space-y-2">
 <h3 className="text-xl font-black tn-text-primary">{t("mfa.title")}</h3>
 <p className="text-sm tn-text-secondary">
 {t("mfa.desc", { actionTitle: actionTitleText })}
 </p>
 </div>

 <form onSubmit={handleSubmit} className="space-y-4">
 <input
 type="text"
 value={code}
 onChange={(e) => setCode(e.target.value.replace(/\D/g, '').slice(0, 6))}
 placeholder={t("mfa.placeholder")}
 className="w-full text-center text-3xl tracking-[0.5em] font-mono py-4 tn-field tn-field-success rounded-2xl text-[var(--success)] transition-colors placeholder:tn-text-muted"
 autoFocus
 />

 {error && (
 <p className="text-xs text-[var(--danger)]">{error}</p>
 )}

 <button
 type="submit"
 disabled={code.length !== 6 || loading}
 className="w-full h-12 flex items-center justify-center gap-2 bg-[var(--success)] hover:bg-[var(--success)] disabled:tn-surface-strong tn-text-primary font-black uppercase tracking-widest text-xs rounded-xl transition-all disabled:opacity-50"
 >
 {loading ? <Loader2 className="w-4 h-4 animate-spin" /> : t("mfa.submit")}
 </button>
 </form>
 </div>
 </div>
 </div>
 );
}
