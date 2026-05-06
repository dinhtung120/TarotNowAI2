import type { LucideIcon } from "lucide-react";
import { cn } from "@/lib/utils";

interface ReaderApplyStatusPanelProps {
 accent: "success" | "warning";
 details?: ReadonlyArray<{ label: string; value: string }>;
 description: string;
 footerLabel?: string;
 icon: LucideIcon;
 introLabel?: string;
 introText?: string;
 title: string;
}

const ACCENT_CLASS = {
 success: {
  panel: "tn-grad-success-soft tn-border-success-20",
  icon: "tn-text-success",
  iconBox: "tn-bg-success-20 tn-border-success-30",
  label: "tn-text-success",
 },
 warning: {
  panel: "tn-grad-warning-soft tn-border-warning-20",
  icon: "tn-text-warning",
  iconBox: "tn-bg-warning-20 tn-border-warning-30",
  label: "tn-text-warning",
 },
} as const;

export function ReaderApplyStatusPanel({
 accent,
 details,
 description,
 footerLabel,
 icon: Icon,
 introLabel,
 introText,
 title,
}: ReaderApplyStatusPanelProps) {
 const classes = ACCENT_CLASS[accent];
 return (
  <div className={cn("max-w-2xl mx-auto tn-page-x py-20 animate-in fade-in slide-in-from-bottom-8 duration-1000")}>
   <div className={cn("relative overflow-hidden bg-gradient-to-br to-transparent tn-rounded-3xl border p-12 shadow-2xl", classes.panel)}>
    <div className={cn("absolute top-0 right-0 p-10 opacity-10 pointer-events-none")}><Icon size={180} className={cn(classes.icon)} /></div>
    <div className={cn("relative z-10 space-y-6 text-center")}>
     <div className={cn("w-16 h-16 mx-auto rounded-2xl flex items-center justify-center border", classes.iconBox)}><Icon className={cn("w-8 h-8", classes.icon)} /></div>
     <h1 className={cn("text-3xl font-black tn-text-primary uppercase italic tracking-tighter")}>{title}</h1>
     <p className={cn("tn-text-secondary text-sm leading-relaxed max-w-md mx-auto")}>{description}</p>
     {introText ? (
      <div className={cn("p-6 rounded-2xl tn-panel-overlay-soft text-left space-y-2")}>
       {introLabel ? <div className={cn("tn-text-overline", classes.label)}>{introLabel}</div> : null}
       <p className={cn("text-xs tn-text-secondary leading-relaxed")}>{introText}</p>
      </div>
     ) : null}
     {details && details.length > 0 ? (
      <div className={cn("grid grid-cols-1 gap-3 rounded-2xl p-4 tn-panel-overlay-soft text-left sm:grid-cols-2")}>
       {details.map((item) => (
        <div key={`${item.label}-${item.value}`} className={cn("space-y-1")}>
         <div className={cn("tn-text-overline tn-text-muted")}>{item.label}</div>
         <div className={cn("text-xs font-bold tn-text-secondary break-words")}>{item.value}</div>
        </div>
       ))}
      </div>
     ) : null}
     {footerLabel ? <div className={cn("tn-text-overline tn-text-muted")}>{footerLabel}</div> : null}
    </div>
   </div>
  </div>
 );
}
