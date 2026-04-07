import { Medal } from "lucide-react";
import { cn } from "@/lib/utils";

interface TitleSelectorHeaderProps {
 title: string;
 subtitle: string;
}

export function TitleSelectorHeader({ title, subtitle }: TitleSelectorHeaderProps) {
 return (
  <div className={cn("relative", "z-10", "mb-6", "flex", "items-center", "gap-3")}>
   <div className={cn("rounded-xl", "bg-gradient-to-br", "from-blue-400", "to-indigo-600", "p-3", "shadow-lg", "shadow-blue-500/30")}>
    <Medal className={cn("h-6", "w-6", "text-white")} />
   </div>
   <div>
    <h2 className={cn("bg-gradient-to-r", "from-blue-100", "to-indigo-200", "bg-clip-text", "text-2xl", "font-bold", "text-transparent")}>{title}</h2>
    <p className={cn("text-sm", "text-blue-200/70")}>{subtitle}</p>
   </div>
  </div>
 );
}
