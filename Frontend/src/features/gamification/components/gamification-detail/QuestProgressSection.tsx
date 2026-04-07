import { cn } from "@/lib/utils";

interface QuestProgressSectionProps {
 current: number;
 target: number;
 isCompleted: boolean;
 progressLabel: string;
}

export function QuestProgressSection({ current, target, isCompleted, progressLabel }: QuestProgressSectionProps) {
 return (
  <div className={cn("mt-4", "border-t", "border-slate-700/30", "pt-4")}>
   <div className={cn("mb-2", "flex", "items-end", "justify-between")}>
    <span className={cn("text-xs", "font-bold", "text-slate-400")}>{progressLabel}</span>
    <span className={cn("text-sm", "font-black", "text-indigo-400")}>
     {current} / {target}
    </span>
   </div>
   <div className={cn("h-2.5", "w-full", "overflow-hidden", "rounded-full", "border", "border-slate-800", "bg-slate-950/80")}>
    <div className={cn("h-full", "rounded-full", "transition-all", "duration-1000", "ease-out", isCompleted ? "bg-gradient-to-r from-indigo-500 to-purple-500" : "bg-indigo-500")} style={{ width: `${Math.min((current / target) * 100, 100)}%` }} />
   </div>
  </div>
 );
}
