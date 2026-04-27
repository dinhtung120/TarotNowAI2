import type { LucideIcon } from "lucide-react";
import { cn } from "@/lib/utils";

interface StatsMetricCardProps {
 borderClassName: string;
 iconClassName: string;
 icon: LucideIcon;
 label: string;
 value: string;
 detail?: string;
 progressPercent?: number;
}

export function StatsMetricCard({ borderClassName, iconClassName, icon: Icon, label, value, detail, progressPercent }: StatsMetricCardProps) {
 return (
  <div
   className={cn(
    "flex",
    "flex-col",
    "justify-center",
    "rounded-2xl",
    "border",
    "bg-slate-900/60",
    "p-5",
    "shadow-lg",
    "backdrop-blur-md",
    borderClassName,
   )}
  >
   <div className={cn("mb-2", "flex", "items-center", "gap-2")}>
    <div className={cn("rounded-lg", "p-2", iconClassName)}>
     <Icon className={cn("h-4", "w-4")} />
    </div>
    <span className={cn("text-xs", "font-bold", "uppercase", "tracking-widest", "text-slate-400")}>{label}</span>
   </div>
   <div className={cn("text-2xl", "font-black", "text-white")}>{value}</div>
   {detail ? <p className={cn("mt-2", "truncate", "text-xs")}>{detail}</p> : null}
   {typeof progressPercent === "number" ? (
    <progress
     className={cn("mt-3", "tn-progress", "tn-progress-sm", "tn-progress-indigo")}
     max={100}
     value={Math.max(0, Math.min(progressPercent, 100))}
    />
   ) : null}
  </div>
 );
}
