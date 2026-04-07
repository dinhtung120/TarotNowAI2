interface LeaderboardCurrentUserEmptyProps {
  notOnLeaderboardLabel: string;
}

export default function LeaderboardCurrentUserEmpty({
  notOnLeaderboardLabel,
}: LeaderboardCurrentUserEmptyProps) {
  return (
    <div className="rounded-2xl border border-dashed border-slate-800/50 bg-slate-900/40 p-6 text-center">
      <p className="text-sm font-medium text-slate-500">
        {notOnLeaderboardLabel}
      </p>
      <p className="mt-1 text-[10px] tracking-tight text-slate-600 uppercase">
        Hãy bắt đầu hành trình của bạn để có tên trên bảng vàng!
      </p>
    </div>
  );
}
