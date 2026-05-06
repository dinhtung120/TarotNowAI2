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
          "mb-2 text-xl font-medium tn-text-accent drop-shadow-md",
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
            "group relative z-50 mt-4 flex min-h-11 items-center gap-2 rounded-full border tn-border-accent-50 px-6 py-2.5 text-xs font-bold tn-text-accent tn-shadow-glow-accent-15 transition-all tn-surface-strong",
          )}
        >
          <Dices className={cn("h-4 w-4 transition-transform tn-group-rotate-12")} />
          {randomText}
        </button>
      )}
    </div>
  );
}
