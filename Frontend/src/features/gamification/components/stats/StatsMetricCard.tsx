import type { LucideIcon } from "lucide-react";

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
  <div className={`bg-slate-900/60 border rounded-2xl p-4 md:p-5 backdrop-blur-md flex flex-col justify-center shadow-lg ${borderClassName}`}>
   <div className="flex items-center gap-2 mb-2"><div className={`p-2 rounded-lg ${iconClassName}`}><Icon className="w-4 h-4" /></div><span className="text-xs font-bold text-slate-400 uppercase tracking-widest">{label}</span></div>
   <div className="text-2xl font-black text-white">{value}</div>
   {detail ? <p className="text-xs truncate mt-2">{detail}</p> : null}
   {typeof progressPercent === "number" ? <div className="h-1.5 w-full bg-slate-800 rounded-full overflow-hidden mt-3"><div className="h-full bg-gradient-to-r from-purple-500 to-indigo-500 rounded-full" style={{ width: `${Math.max(0, Math.min(progressPercent, 100))}%` }} /></div> : null}
  </div>
 );
}
