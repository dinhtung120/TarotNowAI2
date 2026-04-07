import { Sparkles } from "lucide-react";
import { cn } from "@/lib/utils";

interface ProfileUpgradeHeaderProps {
  subtitle: string;
  title: string;
}

export default function ProfileUpgradeHeader({
  subtitle,
  title,
}: ProfileUpgradeHeaderProps) {
  return (
    <div className={cn("flex items-center gap-3")}>
      <div
        className={cn(
          "flex h-12 w-12 items-center justify-center rounded-2xl border border-[var(--purple-accent)]/20 bg-gradient-to-br from-[var(--purple-accent)]/20 to-[var(--warning)]/10 shadow-xl",
        )}
      >
        <Sparkles className={cn("h-6 w-6 tn-text-accent")} />
      </div>
      <div>
        <h3
          className={cn(
            "tn-text-primary text-lg font-black tracking-tight italic",
          )}
        >
          {title}
        </h3>
        <p
          className={cn(
            "text-[10px] font-bold tracking-widest text-[var(--text-secondary)] uppercase",
          )}
        >
          {subtitle}
        </p>
      </div>
    </div>
  );
}
