import { RefreshCw } from "lucide-react";

interface AiStreamNotReadyStateProps {
 waitingLabel: string;
}

export function AiStreamNotReadyState({ waitingLabel }: AiStreamNotReadyStateProps) {
 return (
  <div className="w-full h-full flex flex-col relative animate-in fade-in duration-1000 px-0 md:px-2 overflow-hidden">
   <div className="flex-1 flex flex-col items-center justify-center text-center space-y-4 opacity-40">
    <RefreshCw className="w-10 h-10 tn-text-muted animate-spin" />
    <p className="tn-text-muted font-serif italic max-w-xs px-4">{waitingLabel}</p>
   </div>
  </div>
 );
}
