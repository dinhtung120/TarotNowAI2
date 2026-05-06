import { AlertCircle } from "lucide-react";
import { cn } from "@/lib/utils";

interface CollectionDeckErrorProps {
  error: string;
}

export default function CollectionDeckError({
  error,
}: CollectionDeckErrorProps) {
  return (
    <div
      className={cn(
        "animate-in zoom-in-95 mb-10 flex items-center gap-3 rounded-2xl border border-[var(--danger)]/20 bg-[var(--danger)]/10 p-4 text-xs text-[var(--danger)]",
      )}
    >
      <AlertCircle className={cn("h-4 w-4 flex-shrink-0")} />
      <p className={cn("font-medium")}>{error}</p>
    </div>
  );
}
