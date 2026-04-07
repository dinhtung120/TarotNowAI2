import { Award } from "lucide-react";

interface AchievementsHeaderProps {
 title: string;
 subtitle: string;
}

export function AchievementsHeader({ title, subtitle }: AchievementsHeaderProps) {
 return (
  <div className="flex items-center gap-3 mb-6">
   <div className="p-3 bg-gradient-to-br from-amber-400 to-orange-600 rounded-xl shadow-lg shadow-amber-500/30">
    <Award className="w-6 h-6 text-white" />
   </div>
   <div>
    <h2 className="text-2xl font-bold bg-clip-text text-transparent bg-gradient-to-r from-amber-100 to-orange-200">{title}</h2>
    <p className="text-sm text-amber-200/70">{subtitle}</p>
   </div>
  </div>
 );
}
