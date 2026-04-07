import { Calendar, Clock, Globe } from "lucide-react";

interface LeaderboardPeriodTabsProps {
 period: "daily" | "monthly" | "all";
 onChange: (period: "daily" | "monthly" | "all") => void;
}

export function LeaderboardPeriodTabs({ period, onChange }: LeaderboardPeriodTabsProps) {
 const tabClassName = (tab: "daily" | "monthly" | "all") => period === tab ? "bg-slate-700 text-white shadow-sm" : "text-slate-500 hover:text-slate-300";

 return (
  <div className="flex items-center gap-2 mb-8 p-1 bg-slate-900/40 rounded-xl w-fit">
   <button onClick={() => onChange("daily")} className={`flex items-center gap-2 px-4 py-1.5 rounded-lg text-xs font-semibold uppercase tracking-wider transition-all duration-200 ${tabClassName("daily")}`}><Clock className="w-3.5 h-3.5" />Ngày</button>
   <button onClick={() => onChange("monthly")} className={`flex items-center gap-2 px-4 py-1.5 rounded-lg text-xs font-semibold uppercase tracking-wider transition-all duration-200 ${tabClassName("monthly")}`}><Calendar className="w-3.5 h-3.5" />Tháng</button>
   <button onClick={() => onChange("all")} className={`flex items-center gap-2 px-4 py-1.5 rounded-lg text-xs font-semibold uppercase tracking-wider transition-all duration-200 ${tabClassName("all")}`}><Globe className="w-3.5 h-3.5" />Tất cả</button>
  </div>
 );
}
