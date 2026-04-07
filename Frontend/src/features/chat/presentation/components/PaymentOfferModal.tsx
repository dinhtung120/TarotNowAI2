"use client";

import { Coins, Loader2, X } from "lucide-react";
import { usePaymentOfferModalState } from "@/features/chat/presentation/components/usePaymentOfferModalState";

interface PaymentOfferModalProps {
 onClose: () => void;
 onSubmit: (amount: number, note: string) => Promise<void>;
}

export default function PaymentOfferModal({ onClose, onSubmit }: PaymentOfferModalProps) {
 const vm = usePaymentOfferModalState({ onClose, onSubmit });

 return (
  <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
   <div className="absolute inset-0 bg-black/60 backdrop-blur-sm" onClick={onClose} />
   <div className="relative bg-[#1A1A1A] border border-white/10 rounded-2xl w-full max-w-md shadow-2xl p-6 overflow-hidden animate-in fade-in zoom-in-95 duration-200">
    <div className="flex items-center justify-between mb-6"><div className="flex items-center gap-3"><div className="w-10 h-10 rounded-full bg-[var(--warning)]/20 flex items-center justify-center"><Coins className="w-5 h-5 text-[var(--warning)]" /></div><h2 className="text-xl font-black text-white italic tracking-tight">{vm.t("room.payment_offer")}</h2></div><button onClick={onClose} className="p-2 text-zinc-400 hover:text-white transition-colors"><X className="w-5 h-5" /></button></div>
    <form onSubmit={vm.submitWithValidation} className="space-y-4">
     <div><label className="block text-xs font-bold text-white mb-2 uppercase tracking-wider">{vm.t("room.payment_modal_amount_label")}</label><input type="number" min="1" step="1" value={vm.amount} onChange={(event) => vm.onAmountChange(Number(event.target.value))} required className="w-full bg-white/5 border border-white/10 rounded-xl px-4 py-3 text-white focus:outline-none focus:border-[var(--warning)]/50 transition-colors" /></div>
     <div><label className="block text-xs font-bold text-white mb-2 uppercase tracking-wider">{vm.t("room.payment_modal_note_label")}</label><textarea value={vm.note} onChange={(event) => vm.onNoteChange(event.target.value)} placeholder={vm.t("room.payment_modal_note_placeholder")} rows={3} maxLength={100} required className="w-full bg-white/5 border border-white/10 rounded-xl px-4 py-3 text-white focus:outline-none focus:border-[var(--warning)]/50 resize-none transition-colors" /><div className="text-[10px] text-[var(--text-tertiary)] mt-1">{vm.note.length}/100</div></div>
     <button type="submit" disabled={vm.isSubmitDisabled} className="w-full flex items-center justify-center gap-2 py-3 bg-[var(--warning)] hover:bg-amber-400 text-black font-bold uppercase tracking-widest rounded-xl transition-colors disabled:opacity-50 mt-4">{vm.submitting ? <Loader2 className="w-5 h-5 animate-spin" /> : vm.t("room.payment_modal_submit")}</button>
    </form>
   </div>
  </div>
 );
}
