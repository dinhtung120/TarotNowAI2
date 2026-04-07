import { cn } from "@/lib/utils";

interface QuestProgressSectionProps {
 current: number;
 target: number;
 isCompleted: boolean;
 progressLabel: string;
}

export function QuestProgressSection({ current, target, isCompleted, progressLabel }: QuestProgressSectionProps) {
 return (
  <div className="mt-4 pt-4 border-t border-slate-700/30">
   <div className="flex justify-between items-end mb-2"><span className="text-xs text-slate-400 font-bold">{progressLabel}</span><span className="text-sm font-black text-indigo-400">{current} / {target}</span></div>
   <div className="h-2.5 w-full bg-slate-950/80 rounded-full overflow-hidden border border-slate-800"><div className={cn("h-full rounded-full transition-all duration-1000 ease-out", isCompleted ? "bg-gradient-to-r from-indigo-500 to-purple-500" : "bg-indigo-500")} style={{ width: `${Math.min((current / target) * 100, 100)}%` }} /></div>
  </div>
 );
}
