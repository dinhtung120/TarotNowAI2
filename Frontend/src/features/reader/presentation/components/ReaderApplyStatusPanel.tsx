import type { LucideIcon } from "lucide-react";
import { cn } from "@/lib/utils";

interface ReaderApplyStatusPanelProps {
 accent: "success" | "warning";
 description: string;
 footerLabel?: string;
 icon: LucideIcon;
 introLabel?: string;
 introText?: string;
 title: string;
}

const ACCENT_CLASS = {
 success: {
  panel: "from-[var(--success)]/10 border-[var(--success)]/20",
  icon: "text-[var(--success)]",
  iconBox: "bg-[var(--success)]/20 border-[var(--success)]/30",
  label: "text-[var(--success)]",
 },
 warning: {
  panel: "from-[var(--warning)]/10 border-[var(--warning)]/20",
  icon: "text-[var(--warning)]",
  iconBox: "bg-[var(--warning)]/20 border-[var(--warning)]/30",
  label: "text-[var(--warning)]",
 },
} as const;

export function ReaderApplyStatusPanel({
 accent,
 description,
 footerLabel,
 icon: Icon,
 introLabel,
 introText,
 title,
}: ReaderApplyStatusPanelProps) {
 const classes = ACCENT_CLASS[accent];
 return (
  <div className={cn("max-w-2xl mx-auto px-4 sm:px-6 py-20 animate-in fade-in slide-in-from-bottom-8 duration-1000")}>
   <div className={cn("relative overflow-hidden bg-gradient-to-br to-transparent rounded-[3rem] border p-12 shadow-2xl", classes.panel)}>
    <div className={cn("absolute top-0 right-0 p-10 opacity-10 pointer-events-none")}><Icon size={180} className={cn(classes.icon)} /></div>
    <div className={cn("relative z-10 space-y-6 text-center")}>
     <div className={cn("w-16 h-16 mx-auto rounded-2xl flex items-center justify-center border", classes.iconBox)}><Icon className={cn("w-8 h-8", classes.icon)} /></div>
     <h1 className={cn("text-3xl font-black tn-text-primary uppercase italic tracking-tighter")}>{title}</h1>
     <p className={cn("tn-text-secondary text-sm leading-relaxed max-w-md mx-auto")}>{description}</p>
     {introText ? (
      <div className={cn("p-6 rounded-2xl tn-panel-overlay-soft text-left space-y-2")}>
       {introLabel ? <div className={cn("text-[10px] font-black uppercase tracking-widest", classes.label)}>{introLabel}</div> : null}
       <p className={cn("text-xs tn-text-secondary leading-relaxed")}>{introText}</p>
      </div>
     ) : null}
     {footerLabel ? <div className={cn("text-[10px] font-black uppercase tracking-[0.2em] tn-text-muted")}>{footerLabel}</div> : null}
    </div>
   </div>
  </div>
 );
}
