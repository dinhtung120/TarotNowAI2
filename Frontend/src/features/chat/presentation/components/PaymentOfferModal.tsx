"use client";

import { useState } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { Coins, Loader2, X } from "lucide-react";
import { useTranslations } from "next-intl";
import { useForm } from "react-hook-form";
import { z } from "zod";

interface PaymentOfferModalProps {
 onClose: () => void;
 onSubmit: (amount: number, note: string) => Promise<void>;
}

const paymentOfferModalSchema = z.object({
 amount: z.number().int().min(1),
 note: z.string().trim().min(1).max(100),
});

type PaymentOfferModalFormValues = z.infer<typeof paymentOfferModalSchema>;

export default function PaymentOfferModal({ onClose, onSubmit }: PaymentOfferModalProps) {
 const t = useTranslations("Chat");
 const [submitting, setSubmitting] = useState(false);
 const { handleSubmit, setValue, watch } = useForm<PaymentOfferModalFormValues>({
  resolver: zodResolver(paymentOfferModalSchema),
  defaultValues: {
   amount: 10,
   note: "",
  },
 });
 const amount = watch("amount") ?? 10;
 const note = watch("note") ?? "";
 const isSubmitDisabled = submitting || amount <= 0 || !note.trim() || note.trim().length > 100;

 const submitWithValidation = handleSubmit(async (values) => {
  if (submitting) return;
  setSubmitting(true);
  try {
   await onSubmit(values.amount, values.note.trim());
   onClose();
  } finally {
   setSubmitting(false);
  }
 });

 return (
  <div className="fixed inset-0 z-50 flex items-center justify-center p-4">
   <div className="absolute inset-0 bg-black/60 backdrop-blur-sm" onClick={onClose} />
   <div className="relative bg-[#1A1A1A] border border-white/10 rounded-2xl w-full max-w-md shadow-2xl p-6 overflow-hidden animate-in fade-in zoom-in-95 duration-200">
    <div className="flex items-center justify-between mb-6"><div className="flex items-center gap-3"><div className="w-10 h-10 rounded-full bg-[var(--warning)]/20 flex items-center justify-center"><Coins className="w-5 h-5 text-[var(--warning)]" /></div><h2 className="text-xl font-black text-white italic tracking-tight">{t("room.payment_offer")}</h2></div><button onClick={onClose} className="p-2 text-zinc-400 hover:text-white transition-colors"><X className="w-5 h-5" /></button></div>
    <form onSubmit={submitWithValidation} className="space-y-4">
     <div><label className="block text-xs font-bold text-white mb-2 uppercase tracking-wider">{t("room.payment_modal_amount_label")}</label><input type="number" min="1" step="1" value={amount} onChange={(event) => setValue("amount", Number(event.target.value), { shouldDirty: true, shouldValidate: true })} required className="w-full bg-white/5 border border-white/10 rounded-xl px-4 py-3 text-white focus:outline-none focus:border-[var(--warning)]/50 transition-colors" /></div>
     <div><label className="block text-xs font-bold text-white mb-2 uppercase tracking-wider">{t("room.payment_modal_note_label")}</label><textarea value={note} onChange={(event) => setValue("note", event.target.value, { shouldDirty: true, shouldValidate: true })} placeholder={t("room.payment_modal_note_placeholder")} rows={3} maxLength={100} required className="w-full bg-white/5 border border-white/10 rounded-xl px-4 py-3 text-white focus:outline-none focus:border-[var(--warning)]/50 resize-none transition-colors" /><div className="text-[10px] text-[var(--text-tertiary)] mt-1">{note.length}/100</div></div>
     <button type="submit" disabled={isSubmitDisabled} className="w-full flex items-center justify-center gap-2 py-3 bg-[var(--warning)] hover:bg-amber-400 text-black font-bold uppercase tracking-widest rounded-xl transition-colors disabled:opacity-50 mt-4">{submitting ? <Loader2 className="w-5 h-5 animate-spin" /> : t("room.payment_modal_submit")}</button>
    </form>
   </div>
  </div>
 );
}
