import { Clock } from "lucide-react";
import { cn } from "@/lib/utils";

interface ProfileUpgradePendingStateProps {
  description: string;
  title: string;
}

export default function ProfileUpgradePendingState({
  description,
  title,
}: ProfileUpgradePendingStateProps) {
  return (
    <div
      className={cn(
        "space-y-2 rounded-xl border border-[var(--warning)]/20 bg-[var(--warning)]/5 p-4",
      )}
    >
      <div className={cn("flex items-center gap-2")}>
        <Clock className={cn("h-4 w-4 text-[var(--warning)]")} />
        <span
          className={cn(
            "text-[10px] font-black tracking-widest text-[var(--warning)] uppercase",
          )}
        >
          {title}
        </span>
      </div>
      <p className={cn("text-xs leading-relaxed text-[var(--text-secondary)]")}>
        {description}
      </p>
    </div>
  );
}
