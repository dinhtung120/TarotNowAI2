import { Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";

interface ReaderDecisionBannerProps {
  processingAction: string | null;
  rejectReason: string;
  setRejectReason: (value: string) => void;
  onAcceptConversation: () => Promise<void>;
  onRejectConversation: () => Promise<void>;
}

export default function ReaderDecisionBanner({
  processingAction,
  rejectReason,
  setRejectReason,
  onAcceptConversation,
  onRejectConversation,
}: ReaderDecisionBannerProps) {
  return (
    <div
      className={cn("space-y-2 border-b border-white/10 bg-white/5 px-4 py-3")}
    >
      <p className={cn("text-xs tn-text-secondary")}>
        Yêu cầu đang chờ phản hồi. Reader cần chọn Accept hoặc Reject.
      </p>
      <div className={cn("flex gap-2")}>
        <button
          className={cn(
            "rounded-lg border border-[var(--success)]/30 bg-[var(--success)]/20 px-3 py-2 text-xs font-bold text-[var(--success)]",
          )}
          disabled={processingAction !== null}
          type="button"
          onClick={() => {
            void onAcceptConversation();
          }}
        >
          {processingAction === "accept" ? (
            <Loader2 className={cn("h-3 w-3 animate-spin")} />
          ) : (
            "Accept"
          )}
        </button>
        <input
          className={cn(
            "flex-1 rounded-lg border border-white/10 bg-white/5 px-3 py-2 text-xs text-white",
          )}
          placeholder="Lý do từ chối"
          value={rejectReason}
          onChange={(event) => setRejectReason(event.target.value)}
        />
        <button
          className={cn(
            "rounded-lg border border-[var(--danger)]/30 bg-[var(--danger)]/20 px-3 py-2 text-xs font-bold text-[var(--danger)]",
          )}
          disabled={processingAction !== null}
          type="button"
          onClick={() => {
            void onRejectConversation();
          }}
        >
          {processingAction === "reject" ? (
            <Loader2 className={cn("h-3 w-3 animate-spin")} />
          ) : (
            "Reject"
          )}
        </button>
      </div>
    </div>
  );
}
