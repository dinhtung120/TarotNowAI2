import type { LucideIcon } from "lucide-react";
import { cn } from "@/lib/utils";
import { GlassCard } from "@/shared/components/ui";

interface StatItemProps {
  color: "amber" | "info" | "purple" | "success";
  icon: LucideIcon;
  label: string;
  value: string;
}

export default function StatItem({
  color,
  icon: Icon,
  label,
  value,
}: StatItemProps) {
  const styleByColor: Record<
    StatItemProps["color"],
    { shell: string; icon: string }
  > = {
    purple: { shell: "bg-[var(--purple-50)] tn-border-accent-20", icon: "tn-text-accent" },
    amber: { shell: "bg-[var(--amber-50)] tn-border-warning-20", icon: "tn-text-warning" },
    success: { shell: "tn-bg-success-soft tn-border-success-20", icon: "tn-text-success" },
    info: { shell: "tn-bg-info-soft tn-border-info-20", icon: "tn-text-info" },
  };

  const styles = styleByColor[color];

  return (
    <GlassCard variant="elevated" className={cn("flex flex-col items-center")}>
      <div
        className={cn(
          "mb-4 flex h-10 w-10 items-center justify-center rounded-2xl border border-[var(--border-subtle)]",
          styles.shell,
        )}
      >
        <Icon className={cn("h-5 w-5", styles.icon)} />
      </div>
      <div
        className={cn(
          "mb-1 text-2xl font-black tracking-tighter text-[var(--text-ink)] italic",
        )}
      >
        {value}
      </div>
      <div
        className={cn(
          "text-[9px] font-black tracking-[0.2em] text-[var(--text-secondary)] uppercase",
        )}
      >
        {label}
      </div>
    </GlassCard>
  );
}
