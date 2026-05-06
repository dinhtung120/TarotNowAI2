import { cn } from "@/lib/utils";

interface ChatHeaderEscrowBadgeProps {
  escrowTotalFrozen?: number | null;
}

export default function ChatHeaderEscrowBadge({
  escrowTotalFrozen,
}: ChatHeaderEscrowBadgeProps) {
  if (!escrowTotalFrozen || escrowTotalFrozen <= 0) {
    return null;
  }

  return (
    <div
      className={cn(
        "rounded-lg border border-[var(--warning)]/25 bg-[var(--warning)]/10 px-2 py-1 text-xs text-[var(--warning)]",
      )}
    >
      Đang đóng băng: {escrowTotalFrozen} 💎
    </div>
  );
}
