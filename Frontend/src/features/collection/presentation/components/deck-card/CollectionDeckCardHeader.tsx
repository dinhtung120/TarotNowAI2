import type { UserCollectionDto } from "@/features/collection/application/actions";
import { cn } from "@/lib/utils";

interface CollectionDeckCardHeaderProps {
  userCard: UserCollectionDto | null;
}

export default function CollectionDeckCardHeader({
  userCard,
}: CollectionDeckCardHeaderProps) {
  if (!userCard) {
    return <div className={cn("h-4 w-full")} />;
  }

  const copyProgress = ((userCard.copies % 5) / 5) * 100;

  return (
    <div className={cn("flex w-full flex-col gap-1.5")}>
      <div
        className={cn(
          "tn-text-primary flex items-center justify-between px-1 text-[10px] font-black tracking-tighter uppercase",
        )}
      >
        <span>Lv. {userCard.level}</span>
        <span className={cn("text-[var(--warning)]")}>
          {userCard.copies % 5} / 5
        </span>
      </div>
      <div className={cn("tn-surface h-1 w-full overflow-hidden rounded-full")}>
        <div
          className={cn(
            "h-full bg-[var(--warning)] shadow-[0_0_5px_var(--c-245-158-11-30)] transition-all duration-1000",
          )}
          style={{ width: `${copyProgress}%` }}
        />
      </div>
    </div>
  );
}
