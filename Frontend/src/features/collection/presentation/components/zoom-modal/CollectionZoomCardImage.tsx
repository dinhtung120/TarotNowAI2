import Image from "next/image";
import { Sparkles } from "lucide-react";
import type { TarotCardMeta } from "@/shared/domain/tarotData";
import { shouldUseUnoptimizedImage } from "@/shared/infrastructure/http/assetUrl";
import { cn } from "@/lib/utils";

interface CollectionZoomCardImageProps {
  cardData: TarotCardMeta;
  cardImageUrl?: string;
  cardName: string;
  isOwned: boolean;
  unknownCardLabel: string;
}

export default function CollectionZoomCardImage({
  cardData,
  cardImageUrl,
  cardName,
  isOwned,
  unknownCardLabel,
}: CollectionZoomCardImageProps) {
  const unoptimizedCardImage = shouldUseUnoptimizedImage(cardImageUrl);

  if (cardImageUrl) {
    return (
      <Image
        src={cardImageUrl}
        alt={cardName || unknownCardLabel}
        fill
        unoptimized={unoptimizedCardImage}
        priority
        sizes="(max-width: 768px) 13rem, 20rem"
        className={cn(
          "h-full w-full object-cover transition-all duration-500",
          !isOwned && "blur-[6px]",
        )}
      />
    );
  }

  return (
    <>
      <Sparkles
        className={cn(
          "h-20 w-20",
          isOwned ? "text-[var(--warning)]/30" : "tn-text-muted",
        )}
      />
      <span
        className={cn(
          "tn-text-primary absolute font-serif text-8xl font-black tracking-tighter opacity-5",
        )}
      >
        {cardData.id + 1}
      </span>
    </>
  );
}
