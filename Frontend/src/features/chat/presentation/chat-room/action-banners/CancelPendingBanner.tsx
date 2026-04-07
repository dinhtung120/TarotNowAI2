import { Loader2, Trash2 } from "lucide-react";
import { cn } from "@/lib/utils";

interface CancelPendingBannerProps {
  processingAction: string | null;
  onCancelPending: () => Promise<void>;
}

export default function CancelPendingBanner({
  processingAction,
  onCancelPending,
}: CancelPendingBannerProps) {
  return (
    <div
      className={cn(
        "flex items-center justify-between gap-3 border-b border-white/10 bg-white/5 px-4 py-2",
      )}
    >
      <p className={cn("text-xs text-[var(--text-secondary)]")}>
        Bạn có thể hủy cuộc trò chuyện pending nếu chưa muốn tiếp tục.
      </p>
      <button
        className={cn(
          "flex items-center gap-1.5 rounded-lg border border-[var(--danger)]/30 bg-[var(--danger)]/20 px-3 py-1.5 text-xs font-semibold text-[var(--danger)] disabled:opacity-60",
        )}
        disabled={processingAction !== null}
        type="button"
        onClick={() => {
          void onCancelPending();
        }}
      >
        {processingAction === "cancel-pending" ? (
          <Loader2 className={cn("h-3 w-3 animate-spin")} />
        ) : (
          <Trash2 className={cn("h-3 w-3")} />
        )}
        Xóa pending
      </button>
    </div>
  );
}
