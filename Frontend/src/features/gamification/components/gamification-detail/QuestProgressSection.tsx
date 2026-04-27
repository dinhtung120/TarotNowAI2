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
   <progress
    className={cn("tn-progress", "tn-progress-md", isCompleted ? "tn-progress-indigo" : "tn-progress-accent")}
    max={100}
    value={Math.max(0, Math.min((current / Math.max(1, target)) * 100, 100))}
   />
  </div>
 );
}
