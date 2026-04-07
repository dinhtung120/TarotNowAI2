import { ArrowLeft } from "lucide-react";
import { cn } from "@/lib/utils";

interface ReadingSessionHeaderProps {
  backLabel: string;
  sessionLabel: string;
  title: string;
  onBack: () => void;
}

export default function ReadingSessionHeader({
  backLabel,
  sessionLabel,
  title,
  onBack,
}: ReadingSessionHeaderProps) {
  return (
    <div className={cn("mb-0 flex items-center justify-between gap-2 border-b pb-0 tn-border")}>
      <button
        type="button"
        onClick={onBack}
        className={cn(
          "flex min-h-11 items-center whitespace-nowrap rounded-xl px-2 text-xs transition hover:tn-surface-soft hover:tn-text-primary sm:text-sm tn-text-secondary",
        )}
      >
        <ArrowLeft className={cn("mr-2 h-5 w-5")} />
        {backLabel}
      </button>

      <div className={cn("shrink-0 text-right")}>
        <h1
          className={cn(
            "bg-gradient-to-r from-[var(--purple-accent)] to-[var(--warning)] bg-clip-text text-lg font-bold text-transparent sm:text-2xl",
          )}
        >
          {title}
        </h1>
        <p className={cn("mt-1 hidden font-mono text-xs sm:block tn-text-muted")}>
          {sessionLabel}
        </p>
      </div>
    </div>
  );
}
