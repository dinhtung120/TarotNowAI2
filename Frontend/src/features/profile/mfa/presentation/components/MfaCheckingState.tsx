import { Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";

interface MfaCheckingStateProps {
 label: string;
}

export function MfaCheckingState({ label }: MfaCheckingStateProps) {
 return (
  <div className={cn("h-[60vh] flex flex-col items-center justify-center space-y-6")}>
   <div className={cn("relative group")}>
    <div className={cn("absolute inset-x-0 top-0 h-40 w-40 bg-[var(--success)]/20 blur-[60px] rounded-full animate-pulse")} />
    <Loader2 className={cn("w-12 h-12 animate-spin text-[var(--success)] relative z-10")} />
   </div>
   <div className={cn("text-[10px] font-black uppercase tracking-[0.3em] text-[var(--text-secondary)]")}>{label}</div>
  </div>
 );
}
