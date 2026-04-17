import { cn } from "@/lib/utils";

interface LeaderboardScoreProps {
  score: number;
  currency: "gold" | "diamond";
  index: number;
}

export default function LeaderboardScore({
  score,
  currency,
  index,
}: LeaderboardScoreProps) {
  const isTop3 = index < 3;

  return (
    <div className={cn("shrink-0", "text-right")}>
      <div
        className={cn(
          "text-xl font-black tabular-nums transition-all",
          index === 0
            ? "scale-110 text-amber-400"
            : isTop3
              ? "text-slate-200"
              : "text-slate-500 group-hover:text-slate-300",
        )}
      >
        {score.toLocaleString('en-US')}
      </div>
      <div className={cn("tn-text-10", "font-black", "tracking-widest", "text-slate-600", "uppercase")}>
        {currency === "gold" ? "VÀNG" : "K.CƯƠNG"}
      </div>
    </div>
  );
}
