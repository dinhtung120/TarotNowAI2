import Image from "next/image";
import type { TarotCardMeta } from "@/shared/models/tarotData";
import { shouldUseUnoptimizedImage } from "@/shared/http/assetUrl";
import { cn } from "@/lib/utils";

interface CollectionDeckCardVisualProps {
  cardImageUrl?: string;
  cardName: string;
  deckCard: TarotCardMeta;
  isOwned: boolean;
  unknownCardLabel: string;
}

export default function CollectionDeckCardVisual({
  cardImageUrl,
  cardName,
  deckCard,
  isOwned,
  unknownCardLabel,
}: CollectionDeckCardVisualProps) {
  const unoptimizedCardImage = shouldUseUnoptimizedImage(cardImageUrl);

  return (
    <div
      className={cn(
        "relative mb-3 flex aspect-[14/22] w-full items-center justify-center overflow-hidden rounded-2xl transition-transform duration-500",
        isOwned && "group-hover:scale-[1.03]",
        isOwned
          ? "tn-border-soft border bg-gradient-to-br from-[color:var(--c-215-189-226-26)] to-[color:var(--c-168-156-255-28)]"
          : "tn-overlay",
      )}
    >
      {cardImageUrl ? (
        <Image
          src={cardImageUrl}
          alt={cardName || unknownCardLabel}
          fill
          unoptimized={unoptimizedCardImage}
          priority={isOwned && deckCard.id < 3}
          loading={isOwned && deckCard.id < 3 ? "eager" : "lazy"}
          sizes="(max-width: 640px) 46vw, (max-width: 1024px) 28vw, 220px"
          className={cn("h-full w-full object-cover", !isOwned && "blur-sm")}
        />
      ) : (
        <span
          className={cn(
            "font-serif text-4xl font-black tracking-tighter opacity-20",
            isOwned ? "text-[var(--warning)]" : "tn-text-muted blur-[4px]",
          )}
        >
          {deckCard.id + 1}
        </span>
      )}
      {isOwned && (
        <div
          className={cn(
            "absolute inset-0 bg-gradient-to-t from-[var(--warning)]/20 via-transparent to-transparent opacity-60",
          )}
        />
      )}
      {isOwned && (
        <div
          className={cn(
            "absolute -bottom-[10%] -left-[10%] h-1/2 w-[120%] rounded-full bg-[var(--warning)]/[0.03] blur-2xl",
          )}
        />
      )}
    </div>
  );
}
