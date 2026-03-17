'use client';

import React, { useState } from 'react';
import { challengeMfa } from '@/actions/mfaActions';
import { ShieldAlert, Loader2, X } from 'lucide-react';

interface MfaChallengeModalProps {
 isOpen: boolean;
 onClose: () => void;
 onSuccess: (code: string) => void;
 actionTitle?: string;
 skipApiCall?: boolean;
}

/**
 * Modal hiển thị yêu cầu nhập mã MFA khi thực hiện hành động nhạy cảm (VD: rút tiền).
 */
export default function MfaChallengeModal({ isOpen, onClose, onSuccess, actionTitle = 'hành động này', skipApiCall = false }: MfaChallengeModalProps) {
 const [code, setCode] = useState('');
 const [loading, setLoading] = useState(false);
 const [error, setError] = useState('');

 if (!isOpen) return null;

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

 const res = await challengeMfa(code);
 if (res.success) {
 resetFormState();
 onSuccess(code);
 } else {
 setError(res.error || 'Xác thực thất bại');
 setLoading(false);
 }
 };

 return (
 <div className="fixed inset-0 z-50 flex items-center justify-center p-4 tn-overlay-strong animate-in fade-in duration-200">
 <div className="relative w-full max-w-sm tn-surface-strong border tn-border rounded-3xl p-6 shadow-2xl animate-in zoom-in-95 duration-200">
 {/* Close Button */}
 <button onClick={handleClose}
 className="absolute top-4 right-4 p-2 rounded-full hover:tn-surface-strong tn-text-muted hover:tn-text-primary transition-colors"
 >
 <X className="w-5 h-5" />
 </button>

 <div className="text-center space-y-6">
 <div className="mx-auto w-16 h-16 rounded-full bg-[var(--success)]/10 border border-[var(--success)]/20 flex items-center justify-center">
 <ShieldAlert className="w-8 h-8 text-[var(--success)]" />
 </div>

 <div className="space-y-2">
 <h3 className="text-xl font-black tn-text-primary">Xác thực 2 lớp (MFA)</h3>
 <p className="text-sm tn-text-secondary">
 Vui lòng nhập mã từ ứng dụng Authenticator để tiếp tục {actionTitle}.
 </p>
 </div>

 <form onSubmit={handleSubmit} className="space-y-4">
 <input
 type="text"
 value={code}
 onChange={(e) => setCode(e.target.value.replace(/\D/g, '').slice(0, 6))}
 placeholder="000000"
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
 {loading ? <Loader2 className="w-4 h-4 animate-spin" /> : 'Xác thực'}
 </button>
 </form>
 </div>
 </div>
 </div>
 );
}
