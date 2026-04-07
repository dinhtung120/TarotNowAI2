import CollectionZoomCardVisual from "@/features/collection/presentation/components/zoom-modal/CollectionZoomCardVisual";
import CollectionZoomDetails from "@/features/collection/presentation/components/zoom-modal/CollectionZoomDetails";
import type { CollectionZoomModalProps } from "@/features/collection/presentation/components/zoom-modal/types";
import { cn } from "@/lib/utils";

export function CollectionZoomModal({
  cardData,
  cardImageUrl,
  cardMeaning,
  cardName,
  labels,
  suitLabel,
  userCard,
  onClose,
}: CollectionZoomModalProps) {
  if (!cardData) return null;

  const isOwned = Boolean(userCard);
  const title = isOwned ? cardName || labels.unknownCard : labels.unknownCard;

  return (
    <div
      className={cn(
        "animate-in fade-in fixed inset-0 z-[100] flex items-center justify-center p-6 duration-500 md:p-12",
      )}
    >
      <div
        className={cn("tn-overlay-strong absolute inset-0")}
        onClick={onClose}
      />
      <div
        className={cn(
          "tn-panel animate-in zoom-in-95 slide-in-from-bottom-10 relative z-10 flex w-full max-w-4xl flex-col items-center gap-8 overflow-hidden rounded-[3rem] p-6 text-center shadow-[0_30px_100px_var(--c-0-0-0-80)] duration-500 md:flex-row md:items-stretch md:gap-12 md:p-12 md:text-left",
        )}
      >
        <div
          className={cn(
            "absolute -top-24 -left-24 h-64 w-64 rounded-full bg-[var(--purple-accent)]/[0.08] blur-[100px]",
          )}
        />
        <div
          className={cn(
            "absolute -right-24 -bottom-24 h-64 w-64 rounded-full bg-[var(--warning)]/[0.05] blur-[100px]",
          )}
        />
        <CollectionZoomCardVisual
          cardData={cardData}
          cardImageUrl={cardImageUrl}
          cardName={cardName}
          isOwned={isOwned}
          suitLabel={suitLabel}
          unknownCardLabel={labels.unknownCard}
        />
        <CollectionZoomDetails
          cardMeaning={cardMeaning}
          closeLabel={labels.closeLabel}
          isOwned={isOwned}
          labels={{
            copiesLabel: labels.copiesLabel,
            levelLabel: labels.levelLabel,
          }}
          lockedMeaning={labels.lockedMeaning}
          title={title}
          userCard={userCard}
          onClose={onClose}
        />
      </div>
    </div>
  );
}
