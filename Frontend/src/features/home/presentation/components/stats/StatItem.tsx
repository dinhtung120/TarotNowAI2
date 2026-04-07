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
    { bg: string; text: string }
  > = {
    purple: { bg: "var(--purple-50)", text: "var(--purple-accent)" },
    amber: { bg: "var(--amber-50)", text: "var(--amber-accent)" },
    success: { bg: "var(--success-bg)", text: "var(--success)" },
    info: { bg: "var(--info-bg)", text: "var(--info)" },
  };

  const styles = styleByColor[color];

  return (
    <GlassCard variant="elevated" className={cn("flex flex-col items-center")}>
      <div
        className={cn(
          "mb-4 flex h-10 w-10 items-center justify-center rounded-2xl border border-[var(--border-subtle)]",
        )}
        style={{
          backgroundColor: styles.bg,
          borderColor: `color-mix(in srgb, ${styles.text} 20%, transparent)`,
          color: styles.text,
        }}
      >
        <Icon className={cn("h-5 w-5")} />
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
