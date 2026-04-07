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
 if (isClaimed) return <button type="button" disabled className={cn("px-4 py-2 rounded-xl bg-slate-800/80 text-slate-500 border border-slate-700/50 font-medium flex items-center gap-2 cursor-not-allowed")}><Check className={cn("w-4 h-4")} />{claimedLabel}</button>;
 if (!isCompleted) return <button type="button" disabled className={cn("px-4 py-2 rounded-xl bg-slate-800/50 text-slate-400 border border-slate-700 font-medium flex items-center gap-2 transition")}><Clock className={cn("w-4 h-4 opacity-50")} />{pendingLabel}</button>;

 return (
  <button type="button" onClick={onClaim} disabled={isClaimPending} className={cn("relative overflow-hidden px-5 py-2 rounded-xl bg-gradient-to-r from-indigo-500 to-purple-600 text-white font-medium hover:from-indigo-400 hover:to-purple-500 focus:ring-2 focus:ring-purple-500/50 focus:outline-none transition-all duration-200 transform hover:scale-105 active:scale-95 disabled:opacity-70 disabled:hover:scale-100 flex items-center group/btn shadow-[0_0_15px_rgba(99,102,241,0.4)]")}>
   <span className={cn("absolute inset-0 w-full h-full bg-white/20 -translate-x-full group-hover/btn:animate-[shimmer_1s_infinite] skew-x-[-20deg]")} />
   {isClaimPending ? <div className={cn("w-5 h-5 border-2 border-white/30 border-t-white rounded-full animate-spin")} /> : claimLabel}
  </button>
 );
}
