import { Medal } from "lucide-react";
import { cn } from "@/lib/utils";

interface LeaderboardRankBadgeProps {
  index: number;
}

export default function LeaderboardRankBadge({
  index,
}: LeaderboardRankBadgeProps) {
  if (index === 0) {
    return (
      <Medal
        className={cn(
          "h-9 w-9 text-amber-400 drop-shadow-[0_0_10px_rgba(251,191,36,0.6)]",
        )}
      />
    );
  }

  if (index === 1) {
    return (
      <Medal
        className={cn(
          "h-8 w-8 text-slate-300 drop-shadow-[0_0_8px_rgba(203,213,225,0.6)]",
        )}
      />
    );
  }

  if (index === 2) {
    return (
      <Medal
        className={cn(
          "h-8 w-8 text-amber-700 drop-shadow-[0_0_8px_rgba(180,83,9,0.6)]",
        )}
      />
    );
  }

  return (
    <span
      className={cn(
        "text-lg font-black text-slate-600 group-hover:text-slate-400",
      )}
    >
      #{index + 1}
    </span>
  );
}
