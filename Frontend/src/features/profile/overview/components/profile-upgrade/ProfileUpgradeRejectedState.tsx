import { ArrowUpRight, XCircle } from "lucide-react";
import { cn } from "@/lib/utils";

interface ProfileUpgradeRejectedStateProps {
  cta: string;
  reasonText?: string;
  title: string;
  onApply: () => void;
}

export default function ProfileUpgradeRejectedState({
  cta,
  reasonText,
  title,
  onApply,
}: ProfileUpgradeRejectedStateProps) {
  return (
    <div className={cn("space-y-3")}>
      <div
        className={cn(
          "space-y-2 rounded-xl border border-[var(--danger)]/20 bg-[var(--danger)]/5 p-4",
        )}
      >
        <div className={cn("flex items-center gap-2")}>
          <XCircle className={cn("h-4 w-4 tn-text-danger")} />
          <span
            className={cn(
              "text-[10px] font-black tracking-widest text-[var(--danger)] uppercase",
            )}
          >
            {title}
          </span>
        </div>
        {reasonText ? (
          <p
            className={cn(
              "text-xs leading-relaxed text-[var(--text-secondary)]",
            )}
          >
            {reasonText}
          </p>
        ) : null}
      </div>
      <button
        type="button"
        onClick={onApply}
        className={cn(
          "group/btn flex min-h-11 items-center gap-2.5 rounded-xl border border-[var(--purple-accent)]/30 bg-[var(--purple-accent)]/10 px-5 py-2.5 text-[10px] font-black tracking-widest text-[var(--purple-accent)] uppercase transition-all hover:scale-[1.02] hover:bg-[var(--purple-accent)]/20 active:scale-95",
        )}
      >
        <ArrowUpRight
          className={cn(
            "h-3.5 w-3.5 transition-transform group-hover/btn:translate-x-0.5 group-hover/btn:-translate-y-0.5",
          )}
        />
        {cta}
      </button>
    </div>
  );
}
