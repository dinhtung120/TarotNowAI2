import { Sparkles } from "lucide-react";
import RevealConfirmActions from "@/features/reading/session/components/RevealConfirmActions";
import { cn } from "@/lib/utils";

interface RevealConfirmModalProps {
  isOpen: boolean;
  isRevealing: boolean;
  title: string;
  description: string;
  revealText: string;
  revealingText: string;
  changeCardText: string;
  onReveal: () => void;
  onChangeCard: () => void;
}

export default function RevealConfirmModal(props: RevealConfirmModalProps) {
  if (!props.isOpen) {
    return null;
  }

  return (
    <div
      className={cn(
        "tn-overlay animate-in zoom-in fade-in fixed inset-0 z-50 flex items-center justify-center duration-500",
      )}
    >
      <div
        className={cn(
          "tn-surface-strong mx-4 flex max-w-md flex-col items-center rounded-3xl border border-[var(--purple-accent)]/30 p-8 text-center shadow-[0_0_50px_var(--c-168-85-247-20)]",
        )}
      >
        <div
          className={cn(
            "mb-6 flex h-16 w-16 items-center justify-center rounded-full bg-[var(--purple-accent)]/20",
          )}
        >
          <Sparkles className={cn("h-8 w-8 animate-pulse tn-text-warning")} />
        </div>
        <h3
          className={cn("tn-text-primary mb-2 font-serif text-2xl font-bold")}
        >
          {props.title}
        </h3>
        <p className={cn("tn-text-secondary mb-8 text-sm")}>
          {props.description}
        </p>
        <RevealConfirmActions
          changeCardText={props.changeCardText}
          isRevealing={props.isRevealing}
          revealText={props.revealText}
          revealingText={props.revealingText}
          onChangeCard={props.onChangeCard}
          onReveal={props.onReveal}
        />
      </div>
    </div>
  );
}
