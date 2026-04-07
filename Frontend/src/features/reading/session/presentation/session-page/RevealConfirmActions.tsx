import { RefreshCw, Sparkles } from "lucide-react";
import { cn } from "@/lib/utils";

interface RevealConfirmActionsProps {
  isRevealing: boolean;
  revealText: string;
  revealingText: string;
  changeCardText: string;
  onReveal: () => void;
  onChangeCard: () => void;
}

export default function RevealConfirmActions({
  isRevealing,
  revealText,
  revealingText,
  changeCardText,
  onReveal,
  onChangeCard,
}: RevealConfirmActionsProps) {
  return (
    <>
      <button
        className={cn(
          "group font-pj tn-text-primary relative inline-flex w-full items-center justify-center overflow-hidden rounded-2xl bg-gradient-to-r from-[var(--purple-accent)] to-[var(--danger)] px-8 py-4 font-bold shadow-[0_0_40px_var(--c-168-85-247-40)] transition-all duration-300 hover:scale-[1.02]",
        )}
        disabled={isRevealing}
        type="button"
        onClick={onReveal}
      >
        {isRevealing ? (
          <RefreshCw
            className={cn("tn-text-primary mr-3 h-5 w-5 animate-spin")}
          />
        ) : (
          <Sparkles
            className={cn(
              "mr-3 h-5 w-5 text-[var(--warning)] group-hover:animate-spin",
            )}
          />
        )}
        {isRevealing ? revealingText : revealText}
      </button>

      {!isRevealing && (
        <button
          className={cn(
            "tn-text-secondary hover:tn-surface-soft hover:tn-text-primary mt-6 inline-flex min-h-11 items-center rounded-xl px-2 text-sm font-medium transition-colors",
          )}
          type="button"
          onClick={onChangeCard}
        >
          {changeCardText}
        </button>
      )}
    </>
  );
}
