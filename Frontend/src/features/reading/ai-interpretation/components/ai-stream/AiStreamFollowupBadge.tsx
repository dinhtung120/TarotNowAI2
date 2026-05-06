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
   <span className={cn("px-2.5 py-1 tn-text-10 uppercase font-bold tracking-wider rounded-md tn-bg-warning-20 tn-text-warning border tn-border-warning-30")}>
    {freeBadgeText}
   </span>
  );
 }

 return (
  <span className={cn("px-2.5 py-1 tn-text-10 uppercase font-bold tracking-wider rounded-md tn-bg-accent-20 tn-text-accent border tn-border-accent-30 flex items-center")}>
   <Sparkles className={cn("w-3 h-3 mr-1")} />
   {paidBadgeText}
  </span>
 );
}
