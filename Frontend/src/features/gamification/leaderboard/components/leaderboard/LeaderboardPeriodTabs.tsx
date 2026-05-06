import { Calendar, Clock, Globe } from "lucide-react";
import { cn } from "@/lib/utils";

interface LeaderboardPeriodTabsProps {
 period: "daily" | "monthly" | "all";
 onChange: (period: "daily" | "monthly" | "all") => void;
}

export function LeaderboardPeriodTabs({ period, onChange }: LeaderboardPeriodTabsProps) {
 const tabClassName = (tab: "daily" | "monthly" | "all") => (period === tab ? cn("bg-slate-700", "text-white", "shadow-sm") : cn("text-slate-500"));

 return (
  <div className={cn("mb-8", "flex", "w-full", "flex-wrap", "items-center", "gap-2", "rounded-xl", "bg-slate-900/40", "p-1", "sm:w-fit", "sm:flex-nowrap")}>
   <button type="button" onClick={() => onChange("daily")} className={cn("flex", "min-h-11", "flex-1", "items-center", "justify-center", "gap-2", "rounded-lg", "px-3", "py-1.5", "text-xs", "font-semibold", "uppercase", "tracking-wider", "transition-all", "duration-200", "sm:flex-none", "sm:px-4", tabClassName("daily"))}><Clock className={cn("h-4", "w-4")} />Ngày</button>
   <button type="button" onClick={() => onChange("monthly")} className={cn("flex", "min-h-11", "flex-1", "items-center", "justify-center", "gap-2", "rounded-lg", "px-3", "py-1.5", "text-xs", "font-semibold", "uppercase", "tracking-wider", "transition-all", "duration-200", "sm:flex-none", "sm:px-4", tabClassName("monthly"))}><Calendar className={cn("h-4", "w-4")} />Tháng</button>
   <button type="button" onClick={() => onChange("all")} className={cn("flex", "min-h-11", "flex-1", "items-center", "justify-center", "gap-2", "rounded-lg", "px-3", "py-1.5", "text-xs", "font-semibold", "uppercase", "tracking-wider", "transition-all", "duration-200", "sm:flex-none", "sm:px-4", tabClassName("all"))}><Globe className={cn("h-4", "w-4")} />Tất cả</button>
  </div>
 );
}
