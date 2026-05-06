import { useEffect, useState } from "react";
import Image from "next/image";
import { Sparkles } from "lucide-react";
import type { TarotCardMeta } from "@/shared/domain/tarotData";
import { shouldUseUnoptimizedImage } from "@/shared/infrastructure/http/assetUrl";
import { cn } from "@/lib/utils";

interface CollectionZoomCardImageProps {
  cardData: TarotCardMeta;
  cardImageUrl?: string;
  cardPreviewImageUrl?: string;
  cardName: string;
  isOwned: boolean;
  unknownCardLabel: string;
}

export default function CollectionZoomCardImage({
  cardData,
  cardImageUrl,
  cardPreviewImageUrl,
  cardName,
  isOwned,
  unknownCardLabel,
}: CollectionZoomCardImageProps) {
  const [loadedFullImageUrl, setLoadedFullImageUrl] = useState<string | null>(null);
  const previewUrl = cardPreviewImageUrl ?? null;
  const fullUrl = cardImageUrl ?? null;
  const resolvedImageUrl = loadedFullImageUrl === fullUrl ? fullUrl : previewUrl ?? fullUrl;

  useEffect(() => {
    if (!fullUrl || fullUrl === previewUrl || typeof window === "undefined") {
      return;
    }

    let disposed = false;
    const preloader = new window.Image();
    preloader.decoding = "async";
    preloader.src = fullUrl;

    const revealFullImage = () => {
      if (disposed) return;
      setLoadedFullImageUrl(fullUrl);
    };

    if (preloader.complete) {
      revealFullImage();
      return () => {
        disposed = true;
      };
    }

    if (typeof preloader.decode === "function") {
      preloader.decode()
        .then(revealFullImage)
        .catch(revealFullImage);
    } else {
      preloader.onload = revealFullImage;
      preloader.onerror = revealFullImage;
    }

    return () => {
      disposed = true;
      preloader.onload = null;
      preloader.onerror = null;
    };
  }, [cardImageUrl, cardPreviewImageUrl]);

  if (resolvedImageUrl) {
    return (
      <Image
        src={resolvedImageUrl}
        alt={cardName || unknownCardLabel}
        fill
        unoptimized={shouldUseUnoptimizedImage(resolvedImageUrl)}
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
