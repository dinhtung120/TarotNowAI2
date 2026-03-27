/*
 * ===================================================================
 * COMPONENT: PaymentOfferModal
 * ===================================================================
 * MỤC ĐÍCH: Hộp thoại Modal cho phép Thầy Bói (Reader) tạo MỘT GÓI ĐỀ XUẤT 
 * YÊU CẦU TIỀN TỆ (Payment Offer / Diamond) gửi cho Khách Hàng.
 * Nằm trên cổng SignalR "payment_offer".
 * ===================================================================
 */

import React, { useState } from 'react';
import { useTranslations } from 'next-intl';
import { X, Loader2, Coins } from 'lucide-react';

interface PaymentOfferModalProps {
	onClose: () => void;
	/** Callback trả về số Diamond yêu cầu và Đoạn Ghi Chú kèm theo */
	onSubmit: (amount: number, note: string) => Promise<void>;
}

export default function PaymentOfferModal({ onClose, onSubmit }: PaymentOfferModalProps) {
	const t = useTranslations('Chat');
	const [amount, setAmount] = useState<number>(10);
	const [note, setNote] = useState('');
	const [submitting, setSubmitting] = useState(false);

	/**
	 * Sự kiện khi form được nhấn Submit
	 */
	const handleSubmit = async (e: React.FormEvent) => {
		e.preventDefault();
		if (amount <= 0) return;
		if (!note.trim() || note.trim().length > 100) return;
		
		setSubmitting(true);
		await onSubmit(amount, note.trim());
		setSubmitting(false);
		onClose();
	};

	return (
		<div className="fixed inset-0 z-50 flex items-center justify-center p-4">
			{/* Màn đen làm nền (Backdrop) */}
			<div className="absolute inset-0 bg-black/60 backdrop-blur-sm" onClick={onClose} />
			
			<div className="relative bg-[#1A1A1A] border border-white/10 rounded-2xl w-full max-w-md shadow-2xl p-6 overflow-hidden animate-in fade-in zoom-in-95 duration-200">
				
				{/* Đầu đề Modal */}
				<div className="flex items-center justify-between mb-6">
					<div className="flex items-center gap-3">
						<div className="w-10 h-10 rounded-full bg-[var(--warning)]/20 flex items-center justify-center">
							<Coins className="w-5 h-5 text-[var(--warning)]" />
						</div>
						<h2 className="text-xl font-black text-white italic tracking-tight">
							{t('room.payment_offer')}
						</h2>
					</div>
					<button onClick={onClose} className="p-2 text-zinc-400 hover:text-white transition-colors">
						<X className="w-5 h-5" />
					</button>
				</div>
                
				{/* Nội dung Điền form */}
                <form onSubmit={handleSubmit} className="space-y-4">
                    <div>
                        <label className="block text-xs font-bold text-white mb-2 uppercase tracking-wider">
							{t('room.payment_modal_amount_label')}
						</label>
                        <input 
							type="number" 
							min="1" 
							step="1" 
							value={amount} 
							onChange={e => setAmount(Number(e.target.value))} 
							required 
							className="w-full bg-white/5 border border-white/10 rounded-xl px-4 py-3 text-white focus:outline-none focus:border-[var(--warning)]/50 transition-colors" 
						/>
                    </div>
                    <div>
                        <label className="block text-xs font-bold text-white mb-2 uppercase tracking-wider">
							{t('room.payment_modal_note_label')}
						</label>
                        <textarea 
							value={note} 
							onChange={e => setNote(e.target.value)} 
							placeholder={t('room.payment_modal_note_placeholder')}
							rows={3} 
							maxLength={100}
							required
							className="w-full bg-white/5 border border-white/10 rounded-xl px-4 py-3 text-white focus:outline-none focus:border-[var(--warning)]/50 resize-none transition-colors" 
						/>
						<div className="text-[10px] text-[var(--text-tertiary)] mt-1">{note.length}/100</div>
                    </div>
                    
					{/* Nút gửi (Submit Button) */}
                    <button 
						type="submit" 
						disabled={submitting || amount <= 0 || !note.trim() || note.trim().length > 100} 
						className="w-full flex items-center justify-center gap-2 py-3 bg-[var(--warning)] hover:bg-amber-400 text-black font-bold uppercase tracking-widest rounded-xl transition-colors disabled:opacity-50 mt-4"
					>
                        {submitting ? <Loader2 className="w-5 h-5 animate-spin"/> : t('room.payment_modal_submit')}
                    </button>
                </form>
            </div>
        </div>
    );
}
