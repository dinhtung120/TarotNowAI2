interface CheckinFreezeButtonProps {
  preBreakStreak: number;
  onClick: () => void;
}

export default function CheckinFreezeButton({
  preBreakStreak,
  onClick,
}: CheckinFreezeButtonProps) {
  return (
    <button
      className="font-inter flex items-center gap-1.5 self-start rounded-md border border-cyan-800/50 bg-cyan-950/40 px-3 py-1.5 text-xs font-medium text-cyan-400 shadow-[0_0_10px_rgba(6,182,212,0.15)] transition-colors hover:text-cyan-300"
      type="button"
      onClick={onClick}
    >
      <svg
        className="h-3.5 w-3.5"
        fill="none"
        stroke="currentColor"
        viewBox="0 0 24 24"
      >
        <path
          d="M13 10V3L4 14h7v7l9-11h-7z"
          strokeLinecap="round"
          strokeLinejoin="round"
          strokeWidth={2}
        />
      </svg>
      Cứu đứt chuỗi {preBreakStreak} ngày
    </button>
  );
}
