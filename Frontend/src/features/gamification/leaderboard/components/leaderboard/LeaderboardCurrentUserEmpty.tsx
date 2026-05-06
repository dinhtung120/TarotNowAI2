import { cn } from "@/lib/utils";

interface LeaderboardCurrentUserEmptyProps {
  notOnLeaderboardLabel: string;
}

export default function LeaderboardCurrentUserEmpty({
  notOnLeaderboardLabel,
}: LeaderboardCurrentUserEmptyProps) {
  return (
    <div className={cn("rounded-2xl", "border", "border-dashed", "border-slate-800/50", "bg-slate-900/40", "p-6", "text-center")}>
      <p className={cn("text-sm", "font-medium", "text-slate-500")}>
        {notOnLeaderboardLabel}
      </p>
      <p className={cn("mt-1", "text-xs", "uppercase", "tracking-tight", "text-slate-600")}>
        Hãy bắt đầu hành trình của bạn để có tên trên bảng vàng!
      </p>
    </div>
  );
}
