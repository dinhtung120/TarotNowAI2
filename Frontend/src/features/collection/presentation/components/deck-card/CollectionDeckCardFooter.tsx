import { Lock } from "lucide-react";
import type { UserCollectionDto } from "@/features/collection/application/actions";
import { cn, formatCardStat } from "@/lib/utils";

interface CollectionDeckCardFooterProps {
  isOwned: boolean;
  userCard: UserCollectionDto | null;
}

export default function CollectionDeckCardFooter({
  isOwned,
  userCard,
}: CollectionDeckCardFooterProps) {
  if (!isOwned || !userCard) {
    return (
      <div
        className={cn(
          "tn-border-soft tn-surface mt-auto flex w-full items-center justify-center rounded-xl border py-1.5",
        )}
      >
        <Lock className={cn("tn-text-muted h-2.5 w-2.5")} />
      </div>
    );
  }

  return (
    <div
      className={cn(
        "mt-auto flex w-full gap-1 text-center text-[9px] font-bold",
      )}
    >
      <div
        className={cn(
          "flex-1 rounded-lg border border-red-500/20 bg-red-500/10 py-1 text-red-400 shadow-inner shadow-red-500/10",
        )}
      >
        ⚔️ {formatCardStat(userCard.totalAtk ?? userCard.atk ?? 0)}
      </div>
      <div
        className={cn(
          "flex-1 rounded-lg border border-blue-500/20 bg-blue-500/10 py-1 text-blue-400 shadow-inner shadow-blue-500/10",
        )}
      >
        🛡️ {formatCardStat(userCard.totalDef ?? userCard.def ?? 0)}
      </div>
    </div>
  );
}
