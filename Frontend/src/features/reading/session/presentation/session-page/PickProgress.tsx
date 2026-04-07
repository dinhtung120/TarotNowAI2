import { Dices } from "lucide-react";
import { cn } from "@/lib/utils";

interface PickProgressProps {
  cardsToDraw: number;
  pickedCount: number;
  doneText: string;
  promptText: string;
  countText: string;
  randomText: string;
  onRandomSelect: () => void;
}

export default function PickProgress({
  cardsToDraw,
  pickedCount,
  doneText,
  promptText,
  countText,
  randomText,
  onRandomSelect,
}: PickProgressProps) {
  return (
    <div className={cn("mb-4 text-center")}>
      <h2
        className={cn(
          "mb-2 text-xl font-medium text-[var(--purple-accent)] drop-shadow-[0_0_10px_var(--c-168-85-247-50)] sm:text-2xl",
        )}
      >
        {pickedCount < cardsToDraw ? promptText : doneText}
      </h2>

      <p className={cn("text-sm tn-text-secondary")}>{countText}</p>

      {pickedCount < cardsToDraw && (
        <button
          type="button"
          onClick={onRandomSelect}
          className={cn(
            "group relative z-50 mt-4 flex min-h-11 items-center gap-2 rounded-full border border-[var(--purple-accent)]/50 px-6 py-2.5 text-xs font-bold text-[var(--purple-accent)] shadow-[0_0_20px_var(--c-168-85-247-20)] transition-all hover:border-[var(--purple-accent)] hover:bg-[var(--purple-accent)]/60 hover:tn-text-primary active:scale-95 tn-surface-strong",
          )}
        >
          <Dices className={cn("h-4 w-4 transition-transform group-hover:rotate-12")} />
          {randomText}
        </button>
      )}
    </div>
  );
}
