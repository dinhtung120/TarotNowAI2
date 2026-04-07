import { Suspense, type ComponentType } from "react";
import { RefreshCw, Sparkles } from "lucide-react";
import { cn } from "@/lib/utils";

interface AiInterpretationPanelProps {
 allCardsFlipped: boolean;
 cards: number[];
 footerNote: string;
 liveLabel: string;
 sessionId: string;
 subtitle: string;
 title: string;
 AiInterpretationStream: ComponentType<{ sessionId: string; cards?: number[]; isReadyToShow?: boolean }>;
}

export default function AiInterpretationPanel({ allCardsFlipped, cards, footerNote, liveLabel, sessionId, subtitle, title, AiInterpretationStream }: AiInterpretationPanelProps) {
 if (cards.length === 0) return null;

 return (
  <div className={cn("h-full md:sticky md:top-24 md:col-span-1")}>
   <div className={cn("flex flex-col overflow-hidden rounded-3xl border shadow-2xl tn-border tn-surface-strong md:max-h-[calc(100dvh-160px)]")}>
    <div className={cn("flex items-center justify-between border-b p-5 tn-border tn-overlay")}><div className={cn("flex items-center gap-3")}><div className={cn("flex h-10 w-10 items-center justify-center rounded-full border border-[var(--purple-accent)]/20 bg-[var(--purple-accent)]/10")}><Sparkles className={cn("h-5 w-5 animate-pulse text-[var(--purple-accent)]")} /></div><div><h2 className={cn("text-lg font-bold tn-text-primary")}>{title}</h2><p className={cn("text-[10px] font-mono uppercase tracking-tighter tn-text-muted")}>{subtitle}</p></div></div><div className={cn("rounded-full border border-[var(--purple-accent)]/20 bg-[var(--purple-accent)]/10 px-3 py-1")}><span className={cn("text-[10px] font-bold uppercase tracking-widest text-[var(--purple-accent)]")}>{liveLabel}</span></div></div>
    <div className={cn("flex flex-1 flex-col overflow-hidden bg-gradient-to-b from-transparent to-[var(--purple-accent)]/5")}>
     <Suspense fallback={<div className={cn("flex flex-1 items-center justify-center gap-3 tn-text-muted")}><RefreshCw className={cn("h-4 w-4 animate-spin text-[var(--purple-accent)]")} /><span className={cn("text-[10px] font-black uppercase tracking-widest")}>{title}</span></div>}>
      <AiInterpretationStream sessionId={sessionId} cards={cards} isReadyToShow={allCardsFlipped} />
     </Suspense>
    </div>
    <div className={cn("flex items-center justify-between border-t px-6 py-4 tn-border tn-overlay")}><p className={cn("text-[10px] italic tn-text-muted")}>{footerNote}</p><div className={cn("flex gap-1")}><div className={cn("h-1.5 w-1.5 rounded-full bg-[var(--purple-accent)]/40")} /><div className={cn("h-1.5 w-1.5 rounded-full bg-[var(--warning)]/40")} /><div className={cn("h-1.5 w-1.5 rounded-full bg-[var(--danger)]/40")} /></div></div>
   </div>
  </div>
 );
}
