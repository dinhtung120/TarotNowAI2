import { Coins, Diamond } from "lucide-react";
import { cn } from "@/lib/utils";

interface LeaderboardCurrencyTabsProps {
 currency: "gold" | "diamond";
 goldLabel: string;
 diamondLabel: string;
 onChange: (currency: "gold" | "diamond") => void;
}

export function LeaderboardCurrencyTabs({ currency, goldLabel, diamondLabel, onChange }: LeaderboardCurrencyTabsProps) {
 const tabClassName = (tab: "gold" | "diamond") =>
  currency === tab
   ? tab === "gold"
    ? cn("bg-emerald-500", "text-white", "shadow-lg", "shadow-emerald-500/30")
    : cn("bg-indigo-500", "text-white", "shadow-lg", "shadow-indigo-500/30")
   : cn("text-slate-400");

 return (
  <div className={cn("flex", "rounded-2xl", "border", "border-slate-700/50", "bg-slate-900/60", "p-1.5", "backdrop-blur-md")}>
   <button type="button" onClick={() => onChange("gold")} className={cn("flex", "items-center", "gap-2", "rounded-xl", "px-6", "py-2.5", "text-sm", "font-bold", "transition-all", "duration-300", tabClassName("gold"))}><Coins className={cn("h-4", "w-4")} />{goldLabel}</button>
   <button type="button" onClick={() => onChange("diamond")} className={cn("flex", "items-center", "gap-2", "rounded-xl", "px-6", "py-2.5", "text-sm", "font-bold", "transition-all", "duration-300", tabClassName("diamond"))}><Diamond className={cn("h-4", "w-4")} />{diamondLabel}</button>
  </div>
 );
}
