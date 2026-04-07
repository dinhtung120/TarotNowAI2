import type { UserCollectionDto } from "@/features/collection/application/actions";
import CollectionZoomStatsGrid from "@/features/collection/presentation/components/zoom-modal/CollectionZoomStatsGrid";
import { cn } from "@/lib/utils";

interface CollectionZoomDetailsProps {
  cardMeaning: string;
  closeLabel: string;
  isOwned: boolean;
  labels: { copiesLabel: string; levelLabel: string };
  lockedMeaning: string;
  title: string;
  userCard: UserCollectionDto | null;
  onClose: () => void;
}

export default function CollectionZoomDetails({
  cardMeaning,
  closeLabel,
  isOwned,
  labels,
  lockedMeaning,
  title,
  userCard,
  onClose,
}: CollectionZoomDetailsProps) {
  return (
    <div className={cn("relative z-10 flex flex-1 flex-col justify-center")}>
      <h2
        className={cn(
          "tn-text-primary mb-4 text-3xl font-black tracking-tight italic md:text-4xl lg:text-5xl",
        )}
      >
        {title}
      </h2>
      <div
        className={cn(
          "mx-auto mb-6 h-1 w-16 rounded-full bg-[var(--warning)]/40 md:mx-0",
        )}
      />
      <p
        className={cn(
          "tn-text-muted border-l-2 border-transparent py-2 text-sm leading-relaxed font-medium italic md:border-[var(--warning)]/20 md:pl-4 md:text-base",
        )}
      >
        {isOwned ? cardMeaning : lockedMeaning}
      </p>
      {isOwned && userCard && (
        <CollectionZoomStatsGrid labels={labels} userCard={userCard} />
      )}
      <div
        className={cn(
          "mt-10 flex justify-center pt-4 md:mt-auto md:justify-start",
        )}
      >
        <button
          type="button"
          onClick={onClose}
          className={cn(
            "tn-surface-strong tn-text-ink rounded-full px-10 py-3.5 text-[10px] font-black tracking-widest uppercase shadow-xl transition-all hover:scale-105 active:scale-95 md:text-xs",
          )}
        >
          {closeLabel}
        </button>
      </div>
    </div>
  );
}
