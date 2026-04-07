import { Sparkles } from "lucide-react";
import { cn } from "@/lib/utils";

interface AiStreamFollowupBadgeProps {
 freeSlotsRemaining: number;
 freeBadgeText: string;
 paidBadgeText: string;
}

export function AiStreamFollowupBadge({ freeSlotsRemaining, freeBadgeText, paidBadgeText }: AiStreamFollowupBadgeProps) {
 if (freeSlotsRemaining > 0) {
  return (
   <span className={cn("px-2.5 py-1 text-[10px] uppercase font-bold tracking-wider rounded-md bg-[var(--warning)]/20 text-[var(--warning)] border border-[var(--warning)]/30")}>
    {freeBadgeText}
   </span>
  );
 }

 return (
  <span className={cn("px-2.5 py-1 text-[10px] uppercase font-bold tracking-wider rounded-md bg-[var(--purple-accent)]/20 text-[var(--purple-accent)] border border-[var(--purple-accent)]/30 flex items-center")}>
   <Sparkles className={cn("w-3 h-3 mr-1")} />
   {paidBadgeText}
  </span>
 );
}
