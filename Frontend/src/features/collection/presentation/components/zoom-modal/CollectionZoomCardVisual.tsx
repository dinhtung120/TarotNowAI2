import type { TarotCardMeta } from "@/shared/domain/tarotData";
import CollectionZoomCardCaption from "@/features/collection/presentation/components/zoom-modal/CollectionZoomCardCaption";
import CollectionZoomCardImage from "@/features/collection/presentation/components/zoom-modal/CollectionZoomCardImage";
import { cn } from "@/lib/utils";

interface CollectionZoomCardVisualProps {
  cardData: TarotCardMeta;
  cardImageUrl?: string;
  cardName: string;
  isOwned: boolean;
  suitLabel: string;
  unknownCardLabel: string;
}

export default function CollectionZoomCardVisual({
  cardData,
  cardImageUrl,
  cardName,
  isOwned,
  suitLabel,
  unknownCardLabel,
}: CollectionZoomCardVisualProps) {
  return (
    <div
      className={cn(
        "tn-border relative mb-2 aspect-[14/22] w-52 flex-shrink-0 rounded-[2.5rem] border bg-gradient-to-br from-[var(--warning)]/10 to-transparent p-5 transition-transform duration-700 hover:scale-[1.02] sm:w-64 md:mb-0 md:w-80",
        isOwned
          ? "shadow-[0_0_50px_var(--c-251-191-36-15)]"
          : "opacity-50 grayscale",
      )}
    >
      <div
        className={cn(
          "pointer-events-none absolute inset-4 rounded-[1.8rem] border border-[var(--warning)]/10",
        )}
      />
      <div className={cn("flex h-full flex-col")}>
        <div
          className={cn(
            "tn-overlay tn-border-soft relative mb-4 flex flex-1 items-center justify-center overflow-hidden rounded-2xl border shadow-inner",
          )}
        >
          <CollectionZoomCardImage
            cardData={cardData}
            cardImageUrl={cardImageUrl}
            cardName={cardName}
            isOwned={isOwned}
            unknownCardLabel={unknownCardLabel}
          />
          <CollectionZoomCardCaption
            cardName={cardName}
            isOwned={isOwned}
            suitLabel={suitLabel}
            unknownCardLabel={unknownCardLabel}
          />
        </div>
      </div>
    </div>
  );
}
