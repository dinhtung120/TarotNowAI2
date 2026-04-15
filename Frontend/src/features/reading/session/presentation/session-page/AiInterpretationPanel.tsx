import { Suspense, type ComponentType } from "react";
import { RefreshCw, Sparkles } from "lucide-react";
import { cn } from "@/lib/utils";
import type { RevealedReadingCard } from "@/features/reading/application/actions/types";

interface AiInterpretationPanelProps {
 allCardsFlipped: boolean;
 cards: RevealedReadingCard[];
 footerNote: string;
 liveLabel: string;
 sessionId: string;
 subtitle: string;
 title: string;
 AiInterpretationStream: ComponentType<{ sessionId: string; cards?: RevealedReadingCard[]; isReadyToShow?: boolean }>;
}

export default function AiInterpretationPanel({ allCardsFlipped, cards, footerNote, liveLabel, sessionId, subtitle, title, AiInterpretationStream }: AiInterpretationPanelProps) {
 if (cards.length === 0) return null;

 return (
  <div className={cn("tn-sticky-top-24-col1-md")}>
   <div className={cn("flex flex-col overflow-hidden rounded-3xl border shadow-2xl tn-border tn-surface-strong tn-maxh-session-md")}>
    <div className={cn("flex items-center justify-between border-b p-5 tn-border tn-overlay")}>
     <div className={cn("flex items-center gap-3")}>
      <div className={cn("flex h-10 w-10 items-center justify-center rounded-full border tn-border-accent-20 tn-bg-accent-10")}>
       <Sparkles className={cn("h-5 w-5 animate-pulse tn-text-accent")} />
      </div>
      <div>
       <h2 className={cn("text-lg font-bold tn-text-primary")}>{title}</h2>
       <p className={cn("tn-text-overline font-mono tracking-tighter tn-text-muted")}>{subtitle}</p>
      </div>
     </div>
     <div className={cn("rounded-full border tn-border-accent-20 tn-bg-accent-10 px-3 py-1")}>
      <span className={cn("tn-text-overline font-bold tracking-widest tn-text-accent")}>{liveLabel}</span>
     </div>
    </div>
    <div className={cn("flex flex-1 flex-col overflow-hidden tn-grad-accent-soft")}>
     <Suspense
      fallback={
       <div className={cn("flex flex-1 items-center justify-center gap-3 tn-text-muted")}>
        <RefreshCw className={cn("h-4 w-4 animate-spin tn-text-accent")} />
        <span className={cn("tn-text-overline")}>{title}</span>
       </div>
      }
     >
      <AiInterpretationStream sessionId={sessionId} cards={cards} isReadyToShow={allCardsFlipped} />
     </Suspense>
    </div>
    <div className={cn("flex items-center justify-between border-t px-6 py-4 tn-border tn-overlay")}>
     <p className={cn("tn-text-overline italic tn-text-muted")}>{footerNote}</p>
     <div className={cn("flex gap-1")}>
      <div className={cn("h-1.5 w-1.5 rounded-full tn-bg-accent-20")} />
      <div className={cn("h-1.5 w-1.5 rounded-full tn-bg-warning-20")} />
      <div className={cn("h-1.5 w-1.5 rounded-full bg-red-400/40")} />
     </div>
    </div>
   </div>
  </div>
 );
}
