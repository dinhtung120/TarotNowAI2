import { memo, type HTMLAttributes, type ReactNode } from "react";
import { cn } from "@/lib/utils";

type GlassCardVariant = "default" | "elevated" | "interactive";
type GlassCardPadding = "none" | "sm" | "md" | "lg";

interface GlassCardProps extends HTMLAttributes<HTMLDivElement> {
  variant?: GlassCardVariant;
  padding?: GlassCardPadding;
  children: ReactNode;
}

const variantStyles: Record<GlassCardVariant, string> = {
  default: cn("tn-panel", "shadow-[var(--shadow-card)]"),
  elevated: cn(
    "tn-panel-strong",
    "border-[var(--border-hover)]",
    "shadow-[var(--shadow-elevated)]",
  ),
  interactive: cn(
    "tn-panel cursor-pointer shadow-[var(--shadow-card)] transition-all duration-500",
    "hover:bg-[var(--bg-surface-hover)] hover:border-[var(--border-hover)] hover:shadow-[var(--glow-purple-md)] hover:-translate-y-1 active:scale-[0.985]",
  ),
};

const paddingStyles: Record<GlassCardPadding, string> = {
  none: "",
  sm: "p-4",
  md: "p-6 md:p-8",
  lg: "p-8 md:p-10",
};

function GlassCardComponent({
  variant = "default",
  padding = "md",
  children,
  className,
  ...props
}: GlassCardProps) {
  return (
    <div
      className={cn(
        "overflow-hidden rounded-3xl",
        variantStyles[variant],
        paddingStyles[padding],
        className,
      )}
      {...props}
    >
      {children}
    </div>
  );
}

const GlassCard = memo(GlassCardComponent);
GlassCard.displayName = "GlassCard";

export default GlassCard;
