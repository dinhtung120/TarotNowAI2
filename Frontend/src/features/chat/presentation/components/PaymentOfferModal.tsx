"use client";

import { Coins, Loader2, X } from "lucide-react";
import { usePaymentOfferModalState } from "@/features/chat/presentation/components/usePaymentOfferModalState";
import { cn } from "@/lib/utils";
import { paymentOfferModalClasses } from "./PaymentOfferModal.styles";

interface PaymentOfferModalProps {
 onClose: () => void;
 onSubmit: (amount: number, note: string) => Promise<void>;
}

export default function PaymentOfferModal({ onClose, onSubmit }: PaymentOfferModalProps) {
 const vm = usePaymentOfferModalState({ onClose, onSubmit });
 const c = paymentOfferModalClasses;

 return (
  <div className={cn(c.overlay)}>
   <div className={cn(c.backdrop)} onClick={onClose} />
   <div className={cn(c.panel)}>
    <div className={cn(c.header)}>
     <div className={cn(c.headerLeft)}>
      <div className={cn(c.iconWrap)}>
       <Coins className={cn("h-5", "w-5", "text-amber-500")} />
      </div>
      <h2 className={cn(c.title)}>{vm.t("room.payment_offer")}</h2>
     </div>
     <button type="button" onClick={onClose} className={cn(c.closeButton)}>
      <X className={cn("h-5", "w-5")} />
     </button>
    </div>
    <form onSubmit={vm.submitWithValidation} className={cn(c.form)}>
     <div className={cn(c.fieldWrap)}>
      <label className={cn(c.label)}>{vm.t("room.payment_modal_amount_label")}</label>
      <input
       type="number"
       min="1"
       step="1"
       value={vm.amount}
       onChange={(event) => vm.onAmountChange(Number(event.target.value))}
       required
       className={cn(c.input)}
      />
     </div>
     <div className={cn(c.fieldWrap)}>
      <label className={cn(c.label)}>{vm.t("room.payment_modal_note_label")}</label>
      <textarea
       value={vm.note}
       onChange={(event) => vm.onNoteChange(event.target.value)}
       placeholder={vm.t("room.payment_modal_note_placeholder")}
       rows={3}
       maxLength={100}
       required
       className={cn(c.textarea)}
      />
      <div className={cn(c.counter)}>{vm.note.length}/100</div>
     </div>
     <button type="submit" disabled={vm.isSubmitDisabled} className={cn(c.submit)}>
      {vm.submitting ? <Loader2 className={cn("h-5", "w-5", "animate-spin")} /> : vm.t("room.payment_modal_submit")}
     </button>
    </form>
   </div>
  </div>
 );
}
