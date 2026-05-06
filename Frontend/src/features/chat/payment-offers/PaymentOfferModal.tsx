"use client";

import { Coins, Loader2, X } from "lucide-react";
import { usePaymentOfferModalState } from "@/features/chat/payment-offers/usePaymentOfferModalState";
import { cn } from "@/lib/utils";
import Modal from "@/shared/ui/Modal";

interface PaymentOfferModalProps {
 onClose: () => void;
 onSubmit: (amount: number, note: string) => Promise<void>;
}

export default function PaymentOfferModal({ onClose, onSubmit }: PaymentOfferModalProps) {
 const vm = usePaymentOfferModalState({ onClose, onSubmit });

 return (
  <Modal isOpen onClose={onClose} title={vm.t("room.payment_offer")} size="md">
   <div className={cn("mb-5 flex items-center gap-3")}>
    <div className={cn("flex h-10 w-10 items-center justify-center rounded-full bg-amber-500/20")}>
     <Coins className={cn("h-5 w-5 text-amber-500")} />
    </div>
    <h2 className={cn("text-lg font-black tracking-tight tn-text-primary")}>{vm.t("room.payment_offer")}</h2>
   </div>
   <form onSubmit={vm.submitWithValidation} className={cn("space-y-4")}>
    <div className={cn("space-y-2")}>
     <label htmlFor="payment-offer-amount" className={cn("block text-xs font-bold uppercase tracking-wider tn-text-muted")}>
      {vm.t("room.payment_modal_amount_label")}
     </label>
     <input
      id="payment-offer-amount"
      type="number"
      min="1"
      step="1"
      value={vm.amount}
      onChange={(event) => vm.onAmountChange(Number(event.target.value))}
      required
      className={cn("tn-input tn-field-accent w-full rounded-xl px-4 py-3 text-sm")}
     />
    </div>
    <div className={cn("space-y-2")}>
     <label htmlFor="payment-offer-note" className={cn("block text-xs font-bold uppercase tracking-wider tn-text-muted")}>
      {vm.t("room.payment_modal_note_label")}
     </label>
     <textarea
      id="payment-offer-note"
      value={vm.note}
      onChange={(event) => vm.onNoteChange(event.target.value)}
      placeholder={vm.t("room.payment_modal_note_placeholder")}
      rows={3}
      maxLength={100}
      required
      className={cn("tn-input tn-field-accent w-full resize-none rounded-xl px-4 py-3 text-sm")}
     />
     <div className={cn("mt-1 text-right text-xs tn-text-muted")}>{vm.note.length}/100</div>
    </div>
    <div className={cn("flex justify-end gap-3 pt-2")}>
     <button type="button" onClick={onClose} className={cn("rounded-xl border tn-border-soft px-4 py-2 text-xs font-black uppercase tracking-wider tn-text-muted transition-colors hover:tn-text-primary")} aria-label="Close dialog">
      <X className={cn("h-4 w-4")} />
     </button>
     <button type="submit" disabled={vm.isSubmitDisabled} className={cn("flex min-w-32 items-center justify-center gap-2 rounded-xl bg-amber-500 px-5 py-3 text-xs font-black uppercase tracking-widest text-black transition-colors disabled:opacity-50")}>
      {vm.submitting ? <Loader2 className={cn("h-4 w-4 animate-spin")} /> : null}
      {vm.t("room.payment_modal_submit")}
     </button>
    </div>
   </form>
  </Modal>
 );
}
