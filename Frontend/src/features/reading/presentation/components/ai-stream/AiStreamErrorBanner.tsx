import { AlertTriangle, RefreshCw } from "lucide-react";
import { cn } from "@/lib/utils";

interface AiStreamErrorBannerProps {
 error: string;
 title: string;
 retryLabel: string;
 onRetry: () => void;
}

export function AiStreamErrorBanner({ error, title, retryLabel, onRetry }: AiStreamErrorBannerProps) {
 return (
  <div className={cn("m-6 p-4 bg-[var(--danger)]/20 border border-[var(--danger)]/30 rounded-xl flex items-start text-[var(--danger)]")}>
   <AlertTriangle className={cn("w-5 h-5 mr-3 mt-0.5 shrink-0")} />
   <div>
    <p className={cn("font-medium")}>{title}</p>
    <p className={cn("text-sm opacity-80")}>{error}</p>
    <button type="button" onClick={onRetry} className={cn("mt-3 flex items-center text-sm bg-[var(--danger)]/20 hover:bg-[var(--danger)]/30 px-4 py-2 rounded-lg transition")}>
     <RefreshCw className={cn("w-4 h-4 mr-2")} />
     {retryLabel}
    </button>
   </div>
  </div>
 );
}
