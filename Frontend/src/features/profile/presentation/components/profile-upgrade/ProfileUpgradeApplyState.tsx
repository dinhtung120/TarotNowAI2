import { ArrowUpRight } from "lucide-react";
import { cn } from "@/lib/utils";

interface ProfileUpgradeApplyStateProps {
  cta: string;
  description: string;
  onApply: () => void;
}

export default function ProfileUpgradeApplyState({
  cta,
  description,
  onApply,
}: ProfileUpgradeApplyStateProps) {
  return (
    <>
      <p className={cn("text-sm leading-relaxed text-[var(--text-secondary)]")}>
        {description}
      </p>
      <button
        type="button"
        onClick={onApply}
        className={cn(
          "group/btn tn-text-primary flex min-h-11 items-center gap-2.5 rounded-xl bg-gradient-to-r from-[var(--purple-accent)] to-[var(--purple-accent)] px-6 py-3 text-[10px] font-black tracking-widest uppercase shadow-xl transition-all hover:scale-[1.02] hover:brightness-110 active:scale-95",
        )}
      >
        <ArrowUpRight
          className={cn(
            "h-3.5 w-3.5 transition-transform group-hover/btn:translate-x-0.5 group-hover/btn:-translate-y-0.5",
          )}
        />
        {cta}
      </button>
    </>
  );
}
