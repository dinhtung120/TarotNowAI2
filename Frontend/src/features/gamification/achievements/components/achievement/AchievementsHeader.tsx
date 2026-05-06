import { Award } from "lucide-react";
import { cn } from "@/lib/utils";

interface AchievementsHeaderProps {
 title: string;
 subtitle: string;
}

export function AchievementsHeader({ title, subtitle }: AchievementsHeaderProps) {
 return (
  <div className={cn("mb-6", "flex", "items-center", "gap-3")}>
   <div className={cn("rounded-xl", "bg-gradient-to-br", "from-amber-400", "to-orange-600", "p-3", "shadow-lg", "shadow-amber-500/30")}>
    <Award className={cn("h-6", "w-6", "text-white")} />
   </div>
   <div>
    <h2 className={cn("bg-gradient-to-r", "from-amber-100", "to-orange-200", "bg-clip-text", "text-2xl", "font-bold", "text-transparent")}>{title}</h2>
    <p className={cn("text-sm", "text-amber-200/70")}>{subtitle}</p>
   </div>
  </div>
 );
}
