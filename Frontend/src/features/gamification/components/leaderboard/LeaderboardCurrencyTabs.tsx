import { Coins, Diamond } from "lucide-react";

interface LeaderboardCurrencyTabsProps {
 currency: "gold" | "diamond";
 goldLabel: string;
 diamondLabel: string;
 onChange: (currency: "gold" | "diamond") => void;
}

export function LeaderboardCurrencyTabs({ currency, goldLabel, diamondLabel, onChange }: LeaderboardCurrencyTabsProps) {
 const tabClassName = (tab: "gold" | "diamond") => currency === tab ? tab === "gold" ? "bg-emerald-500 text-white shadow-lg shadow-emerald-500/30" : "bg-indigo-500 text-white shadow-lg shadow-indigo-500/30" : "text-slate-400 hover:text-slate-200 hover:bg-white/5";

 return (
  <div className="flex bg-slate-900/60 p-1.5 rounded-2xl border border-slate-700/50 backdrop-blur-md">
   <button type="button" onClick={() => onChange("gold")} className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-sm font-bold transition-all duration-300 ${tabClassName("gold")}`}><Coins className="w-4 h-4" />{goldLabel}</button>
   <button type="button" onClick={() => onChange("diamond")} className={`flex items-center gap-2 px-6 py-2.5 rounded-xl text-sm font-bold transition-all duration-300 ${tabClassName("diamond")}`}><Diamond className="w-4 h-4" />{diamondLabel}</button>
  </div>
 );
}
