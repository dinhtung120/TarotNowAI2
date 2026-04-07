import { Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";

interface ReaderApplyLoadingStateProps {
 label: string;
}

export function ReaderApplyLoadingState({ label }: ReaderApplyLoadingStateProps) {
 return (
  <div className={cn("tn-min-h-60vh flex flex-col items-center justify-center space-y-4")}>
   <Loader2 className={cn("w-10 h-10 tn-text-accent animate-spin")} />
   <span className={cn("tn-text-10 font-black uppercase tn-tracking-03 tn-text-muted")}>{label}</span>
  </div>
 );
}
