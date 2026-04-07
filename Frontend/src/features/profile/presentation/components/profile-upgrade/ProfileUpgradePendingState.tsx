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
        "space-y-2 rounded-xl border tn-border-warning-20 tn-bg-warning-10 p-4",
      )}
    >
      <div className={cn("flex items-center gap-2")}>
        <Clock className={cn("h-4 w-4 tn-text-warning")} />
        <span
          className={cn(
            "tn-text-10 font-black tracking-widest tn-text-warning uppercase",
          )}
        >
          {title}
        </span>
      </div>
      <p className={cn("text-xs leading-relaxed tn-text-secondary")}>
        {description}
      </p>
    </div>
  );
}
