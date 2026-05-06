import type { UserCollectionDto } from "@/features/collection/cards/actions/actions";
import CollectionZoomStatsGrid from "@/features/collection/cards/components/zoom-modal/CollectionZoomStatsGrid";
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
          "tn-text-primary mb-4 tn-text-3-4-5-responsive font-black tracking-tight italic",
        )}
      >
        {title}
      </h2>
      <div
        className={cn(
          "tn-mx-auto-0-md mb-6 h-1 w-16 rounded-full tn-bg-warning-40",
        )}
      />
      <p
        className={cn(
          "tn-text-muted tn-zoom-meaning text-sm leading-relaxed font-medium italic",
        )}
      >
        {isOwned ? cardMeaning : lockedMeaning}
      </p>
      {isOwned && userCard && (
        <CollectionZoomStatsGrid labels={labels} userCard={userCard} />
      )}
      <div
        className={cn(
          "tn-mt-10-auto-md flex tn-justify-center-start-md pt-4",
        )}
      >
        <button
          type="button"
          onClick={onClose}
          data-tn-collection-modal-close="true"
          className={cn(
            "tn-surface-strong tn-text-ink rounded-full px-10 py-3.5 tn-text-10-12-md font-black tracking-widest uppercase shadow-xl transition-all",
          )}
        >
          {closeLabel}
        </button>
      </div>
    </div>
  );
}
