import { Award } from "lucide-react";
import { cn } from "@/lib/utils";

interface UnlockedInfoSectionProps {
 unlockedAtLabel: string;
 unlockedAt: string;
}

export function UnlockedInfoSection({ unlockedAtLabel, unlockedAt }: UnlockedInfoSectionProps) {
 return (
  <section className={cn("flex", "items-center", "gap-3", "rounded-2xl", "border", "border-amber-500/20", "bg-amber-500/5", "p-4")}>
   <Award className={cn("h-8", "w-8", "text-amber-500/50")} />
   <div className={cn("text-xs")}>
    <p className={cn("font-bold", "uppercase", "tracking-wider", "text-amber-200/50")}>{unlockedAtLabel}</p>
    <p className={cn("text-sm", "font-black", "text-amber-200")}>{new Date(unlockedAt).toLocaleDateString()}</p>
   </div>
  </section>
 );
}
