import { cn } from "@/lib/utils";

export function WalletWidgetLoading() {
 return (
  <div className={cn("animate-pulse", "inline-flex", "items-center", "gap-4", "tn-bg-glass", "p-2", "rounded-full", "px-4", "border", "tn-border-soft")}>
   <div className={cn("h-4", "w-16", "tn-bg-surface-hover", "rounded")} />
  </div>
 );
}
