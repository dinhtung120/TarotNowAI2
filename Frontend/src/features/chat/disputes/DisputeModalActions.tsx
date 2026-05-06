import { Loader2 } from "lucide-react";
import { cn } from "@/lib/utils";

interface DisputeModalActionsProps {
  isSubmitting: boolean;
  canSubmit: boolean;
  onClose: () => void;
  onSubmit: () => Promise<void>;
}

export default function DisputeModalActions({
  isSubmitting,
  canSubmit,
  onClose,
  onSubmit,
}: DisputeModalActionsProps) {
  return (
    <div className={cn("flex justify-end gap-2")}>
      <button
        className={cn(
          "rounded-lg border border-white/15 px-3 py-2 text-xs text-[var(--text-secondary)] hover:bg-white/10",
        )}
        type="button"
        onClick={onClose}
      >
        Hủy
      </button>
      <button
        className={cn(
          "rounded-lg border border-[var(--danger)]/30 bg-[var(--danger)]/20 px-3 py-2 text-xs text-[var(--danger)] disabled:opacity-60",
        )}
        disabled={!canSubmit || isSubmitting}
        type="button"
        onClick={() => {
          void onSubmit();
        }}
      >
        {isSubmitting ? (
          <Loader2 className={cn("h-3 w-3 animate-spin")} />
        ) : (
          "Gửi tố cáo"
        )}
      </button>
    </div>
  );
}
