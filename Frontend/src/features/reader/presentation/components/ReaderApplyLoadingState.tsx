import { Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";

interface ReaderApplyLoadingStateProps {
 label: string;
}

export function ReaderApplyLoadingState({ label }: ReaderApplyLoadingStateProps) {
 return (
  <div className={cn("min-h-[60vh] flex flex-col items-center justify-center space-y-4")}>
   <Loader2 className={cn("w-10 h-10 text-[var(--purple-accent)] animate-spin")} />
   <span className={cn("text-[10px] font-black uppercase tracking-[0.3em] tn-text-muted")}>{label}</span>
  </div>
 );
}
