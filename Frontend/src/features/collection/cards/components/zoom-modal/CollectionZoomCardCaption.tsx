import { cn } from "@/lib/utils";

interface CollectionZoomCardCaptionProps {
  cardName: string;
  isOwned: boolean;
  suitLabel: string;
  unknownCardLabel: string;
}

export default function CollectionZoomCardCaption({
  cardName,
  isOwned,
  suitLabel,
  unknownCardLabel,
}: CollectionZoomCardCaptionProps) {
  return (
    <div
      className={cn("absolute right-0 bottom-6 left-0 z-20 px-3 text-center")}
    >
      <span
        className={cn(
          "mb-1 block text-[10px] font-black tracking-[0.2em] text-[var(--warning)]/60 uppercase",
        )}
      >
        {suitLabel}
      </span>
      <h3
        className={cn(
          "tn-text-primary text-xl leading-tight font-black tracking-tighter italic",
        )}
      >
        {isOwned ? cardName || unknownCardLabel : unknownCardLabel}
      </h3>
    </div>
  );
}
