import Image from "next/image";
import { Sparkles } from "lucide-react";
import { cn } from "@/lib/utils";

interface RevealedCardItemProps {
 cardId: number;
 cardImageUrl?: string;
 cardMeaning: string;
 cardName: string;
 index: number;
 isFlipped: boolean;
 meaningLabel: string;
 orientation: "upright" | "reversed";
 orientationLabel: string;
}

export default function RevealedCardItem({ cardId, cardImageUrl, cardMeaning, cardName, index, isFlipped, meaningLabel, orientation, orientationLabel }: RevealedCardItemProps) {
 const isReversed = orientation === "reversed";
 return (
  <div className={cn("mx-auto flex w-full tn-maxw-180 flex-col items-center gap-3")} key={`revealed-card-${cardId}`}>
   <div className={cn("group relative tn-aspect-14-22 w-full cursor-pointer preserve-3d transition-transform duration-700 ease-out tarot-card-flip", isFlipped ? "tarot-card-flip-rotated" : "tarot-card-flip-reset")}>
    <div className={cn("absolute inset-0 flex h-full w-full backface-hidden flex-col items-center justify-center rounded-xl border-2 shadow-xl tn-border tn-grad-lunar")}><div className={cn("flex tn-frame-90-85 items-center justify-center rounded-lg border tn-border-soft tn-starfield")}><Sparkles className={cn("h-8 w-8 tn-text-muted")} /></div></div>
    <div className={cn("absolute inset-0 flex h-full w-full backface-hidden items-center justify-center bg-transparent tarot-card-face-back")}><div className={cn("relative flex h-full w-full items-center justify-center overflow-hidden rounded-xl border shadow-xl tn-border-soft tn-surface-strong")}><div className={cn("pointer-events-none absolute inset-0 bg-gradient-to-tr from-violet-500/20 to-transparent")} /><div className={cn("pointer-events-none absolute inset-2 rounded-lg border tn-border-accent-20")} />{cardImageUrl ? <Image src={cardImageUrl} alt={cardName} fill unoptimized sizes="(max-width: 1024px) 45vw, 220px" className={cn("h-full w-full object-cover transition-transform duration-300", isReversed && "rotate-180")} /> : <span className={cn("text-5xl font-black drop-shadow-sm font-serif tn-text-primary/10")}>{index + 1}</span>}</div></div>
   </div>
   <div className={cn("mt-4 text-center transition-opacity duration-1000 delay-500", isFlipped ? "opacity-100" : "opacity-0")}><h3 className={cn("mb-2 px-2 text-sm font-bold leading-tight font-serif tn-text-primary")}>{cardName}</h3><p className={cn("mb-1 tn-text-overline font-semibold tn-text-accent")}>{meaningLabel}</p><p className={cn("mb-1 text-[11px] font-semibold uppercase tracking-wider tn-text-muted")}>{orientationLabel}</p><p className={cn("line-clamp-3 px-2 text-xs leading-relaxed tn-text-secondary")}>{cardMeaning}</p></div>
  </div>
 );
}
