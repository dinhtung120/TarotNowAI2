import { Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";

interface ReaderProfileLoadingStateProps {
 label: string;
}

export function ReaderProfileLoadingState({ label }: ReaderProfileLoadingStateProps) {
 return (
  <div className={cn("tn-h-60vh flex flex-col items-center justify-center space-y-6")}>
   <div className={cn("relative group")}>
    <div className={cn("tn-reader-profile-glow absolute inset-x-0 top-0 h-40 w-40 rounded-full animate-pulse")} />
    <Loader2 className={cn("w-12 h-12 animate-spin tn-text-accent relative z-10")} />
   </div>
   <div className={cn("tn-text-10 font-black uppercase tn-tracking-03 tn-text-secondary")}>{label}</div>
  </div>
 );
}
