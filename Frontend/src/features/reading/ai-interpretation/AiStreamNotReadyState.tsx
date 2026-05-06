import { RefreshCw } from "lucide-react";
import { cn } from "@/lib/utils";

interface AiStreamNotReadyStateProps {
 waitingLabel: string;
}

export function AiStreamNotReadyState({ waitingLabel }: AiStreamNotReadyStateProps) {
 return (
  <div className={cn("relative", "flex", "h-full", "w-full", "flex-col", "overflow-hidden", "px-2", "animate-in", "fade-in", "duration-1000")}>
   <div className={cn("flex-1", "flex", "flex-col", "items-center", "justify-center", "space-y-4", "px-4", "text-center", "opacity-40")}>
    <RefreshCw className={cn("h-10", "w-10", "animate-spin", "tn-text-muted")} />
    <p className={cn("max-w-xs", "font-serif", "italic", "tn-text-muted")}>{waitingLabel}</p>
   </div>
  </div>
 );
}
