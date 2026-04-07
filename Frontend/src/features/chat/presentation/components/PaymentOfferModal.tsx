"use client";

import { Coins, Loader2, X } from "lucide-react";
import { usePaymentOfferModalState } from "@/features/chat/presentation/components/usePaymentOfferModalState";
import { cn } from "@/lib/utils";

interface PaymentOfferModalProps {
 onClose: () => void;
 onSubmit: (amount: number, note: string) => Promise<void>;
}

export default function PaymentOfferModal({ onClose, onSubmit }: PaymentOfferModalProps) {
 const vm = usePaymentOfferModalState({ onClose, onSubmit });
 const classes = {
  overlay: cn("fixed", "inset-0", "z-50", "flex", "items-center", "justify-center", "p-4"),
  backdrop: cn("absolute", "inset-0", "bg-black/60", "backdrop-blur-sm"),
  panel: cn(
   "relative",
   "w-full",
   "max-w-md",
   "overflow-hidden",
   "rounded-2xl",
   "border",
   "border-white/10",
   "bg-zinc-900",
   "p-6",
   "shadow-2xl",
   "animate-in",
   "fade-in",
   "zoom-in-95",
   "duration-200",
  ),
  header: cn("mb-6", "flex", "items-center", "justify-between"),
  headerLeft: cn("flex", "items-center", "gap-3"),
  iconWrap: cn("flex", "h-10", "w-10", "items-center", "justify-center", "rounded-full", "bg-amber-500/20"),
  title: cn("text-xl", "font-black", "italic", "tracking-tight", "text-white"),
  closeButton: cn("p-2", "text-zinc-400", "transition-colors"),
  form: cn("space-y-4"),
  fieldWrap: cn("space-y-2"),
  label: cn("block", "text-xs", "font-bold", "uppercase", "tracking-wider", "text-white"),
  input: cn(
   "w-full",
   "rounded-xl",
   "border",
   "border-white/10",
   "bg-white/5",
   "px-4",
   "py-3",
   "text-white",
   "transition-colors",
   "focus:border-amber-400/50",
   "focus:outline-none",
  ),
  textarea: cn(
   "w-full",
   "resize-none",
   "rounded-xl",
   "border",
   "border-white/10",
   "bg-white/5",
   "px-4",
   "py-3",
   "text-white",
   "transition-colors",
   "focus:border-amber-400/50",
   "focus:outline-none",
  ),
  counter: cn("mt-1", "text-xs", "text-zinc-400"),
  submit: cn(
   "mt-4",
   "flex",
   "w-full",
   "items-center",
   "justify-center",
   "gap-2",
   "rounded-xl",
   "bg-amber-500",
   "py-3",
   "font-bold",
   "uppercase",
   "tracking-widest",
   "text-black",
   "transition-colors",
   "disabled:opacity-50",
  ),
 };

 return (
  <div className={cn(classes.overlay)}>
   <div className={cn(classes.backdrop)} onClick={onClose} />
   <div className={cn(classes.panel)}>
    <div className={cn(classes.header)}>
     <div className={cn(classes.headerLeft)}>
      <div className={cn(classes.iconWrap)}>
       <Coins className={cn("h-5", "w-5", "text-amber-500")} />
      </div>
      <h2 className={cn(classes.title)}>{vm.t("room.payment_offer")}</h2>
     </div>
     <button type="button" onClick={onClose} className={cn(classes.closeButton)}>
      <X className={cn("h-5", "w-5")} />
     </button>
    </div>
    <form onSubmit={vm.submitWithValidation} className={cn(classes.form)}>
     <div className={cn(classes.fieldWrap)}>
      <label className={cn(classes.label)}>{vm.t("room.payment_modal_amount_label")}</label>
      <input
       type="number"
       min="1"
       step="1"
       value={vm.amount}
       onChange={(event) => vm.onAmountChange(Number(event.target.value))}
       required
       className={cn(classes.input)}
      />
     </div>
     <div className={cn(classes.fieldWrap)}>
      <label className={cn(classes.label)}>{vm.t("room.payment_modal_note_label")}</label>
      <textarea
       value={vm.note}
       onChange={(event) => vm.onNoteChange(event.target.value)}
       placeholder={vm.t("room.payment_modal_note_placeholder")}
       rows={3}
       maxLength={100}
       required
       className={cn(classes.textarea)}
      />
      <div className={cn(classes.counter)}>{vm.note.length}/100</div>
     </div>
     <button type="submit" disabled={vm.isSubmitDisabled} className={cn(classes.submit)}>
      {vm.submitting ? <Loader2 className={cn("h-5", "w-5", "animate-spin")} /> : vm.t("room.payment_modal_submit")}
     </button>
    </form>
   </div>
  </div>
 );
}
