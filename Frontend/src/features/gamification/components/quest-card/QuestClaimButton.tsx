import { Check, Clock } from "lucide-react";
import { cn } from "@/lib/utils";

interface QuestClaimButtonProps {
 isClaimed: boolean;
 isCompleted: boolean;
 isClaimPending: boolean;
 claimedLabel: string;
 claimLabel: string;
 pendingLabel: string;
 onClaim: () => void;
}

export function QuestClaimButton({ isClaimed, isCompleted, isClaimPending, claimedLabel, claimLabel, pendingLabel, onClaim }: QuestClaimButtonProps) {
 if (isClaimed) return <button type="button" disabled className={cn("px-4 py-2 rounded-xl border border-slate-700/50 bg-slate-800/80 text-slate-500 font-medium flex items-center gap-2 cursor-not-allowed")}><Check className={cn("w-4 h-4")} />{claimedLabel}</button>;
 if (!isCompleted) return <button type="button" disabled className={cn("px-4 py-2 rounded-xl border border-slate-700 bg-slate-800/50 text-slate-400 font-medium flex items-center gap-2")}><Clock className={cn("w-4 h-4 opacity-50")} />{pendingLabel}</button>;

 return (
  <button type="button" onClick={onClaim} disabled={isClaimPending} className={cn("px-5 py-2 rounded-xl tn-bg-accent text-white font-medium tn-hover-brightness-110 transition-all duration-200 tn-disabled-opacity-70 flex items-center gap-2 shadow-lg")}>
   {isClaimPending ? <div className={cn("w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin")} /> : <Check className={cn("w-4 h-4")} />}
   {claimLabel}
  </button>
 );
}
