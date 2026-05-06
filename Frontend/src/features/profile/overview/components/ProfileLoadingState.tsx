import { Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";

interface ProfileLoadingStateProps {
  label: string;
}

export function ProfileLoadingState({ label }: ProfileLoadingStateProps) {
  return (
    <div
      className={cn(
        "flex h-[60vh] flex-col items-center justify-center space-y-6",
      )}
    >
      <div className={cn("group relative")}>
        <div
          className={cn(
            "absolute inset-x-0 top-0 h-40 w-40 animate-pulse rounded-full bg-[var(--purple-accent)]/20 blur-[60px]",
          )}
        />
        <Loader2
          className={cn(
            "relative z-10 h-12 w-12 animate-spin text-[var(--purple-accent)]",
          )}
        />
      </div>
      <div
        className={cn(
          "text-[10px] font-black tracking-[0.3em] text-[var(--text-secondary)] uppercase",
        )}
      >
        {label}
      </div>
    </div>
  );
}
