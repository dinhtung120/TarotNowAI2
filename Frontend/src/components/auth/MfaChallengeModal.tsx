'use client';

import React, { useState, useEffect } from 'react';
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

  // Reset khi mở lại
  useEffect(() => {
    if (isOpen) {
      setCode('');
      setError('');
      setLoading(false);
    }
  }, [isOpen]);

  if (!isOpen) return null;

  const handleSubmit = async (e: React.FormEvent) => {
    e.preventDefault();
    if (code.length < 6) return;

    setLoading(true);
    setError('');

    if (skipApiCall) {
      onSuccess(code);
      setLoading(false);
      return;
    }

    const res = await challengeMfa(code);
    if (res.success) {
      onSuccess(code);
    } else {
      setError(res.error || 'Xác thực thất bại');
      setLoading(false);
    }
  };

  return (
    <div className="fixed inset-0 z-50 flex items-center justify-center p-4 bg-black/80 backdrop-blur-sm animate-in fade-in duration-200">
      <div className="relative w-full max-w-sm bg-zinc-900 border border-white/10 rounded-3xl p-6 shadow-2xl animate-in zoom-in-95 duration-200">
        
        {/* Close Button */}
        <button 
          onClick={onClose}
          className="absolute top-4 right-4 p-2 rounded-full hover:bg-white/10 text-zinc-500 hover:text-white transition-colors"
        >
          <X className="w-5 h-5" />
        </button>

        <div className="text-center space-y-6">
          <div className="mx-auto w-16 h-16 rounded-full bg-emerald-500/10 border border-emerald-500/20 flex items-center justify-center">
            <ShieldAlert className="w-8 h-8 text-emerald-400" />
          </div>

          <div className="space-y-2">
            <h3 className="text-xl font-black text-white">Xác thực 2 lớp (MFA)</h3>
            <p className="text-sm text-zinc-400">
              Vui lòng nhập mã từ ứng dụng Authenticator để tiếp tục {actionTitle}.
            </p>
          </div>

          <form onSubmit={handleSubmit} className="space-y-4">
            <input
              type="text"
              value={code}
              onChange={(e) => setCode(e.target.value.replace(/\D/g, '').slice(0, 6))}
              placeholder="000000"
              className="w-full text-center text-3xl tracking-[0.5em] font-mono py-4 bg-black/50 border border-white/10 focus:border-emerald-500 rounded-2xl text-emerald-400 focus:outline-none transition-colors placeholder:text-zinc-800"
              autoFocus
            />

            {error && (
              <p className="text-xs text-red-500">{error}</p>
            )}

            <button
              type="submit"
              disabled={code.length !== 6 || loading}
              className="w-full h-12 flex items-center justify-center gap-2 bg-emerald-600 hover:bg-emerald-500 disabled:bg-zinc-800 text-white font-black uppercase tracking-widest text-xs rounded-xl transition-all disabled:opacity-50"
            >
              {loading ? <Loader2 className="w-4 h-4 animate-spin" /> : 'Xác thực'}
            </button>
          </form>
        </div>
      </div>
    </div>
  );
}
