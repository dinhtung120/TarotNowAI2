/**
 * GlassCard (legacy name) — card dùng chung cho toàn app.
 *
 * Lưu ý: giữ tên component để không phá vỡ import cũ,
 * nhưng style đã chuyển sang Lunar Bloom surface sáng dịu.
 */

import { type HTMLAttributes, type ReactNode } from "react";

type GlassCardVariant = "default" | "elevated" | "interactive";

interface GlassCardProps extends HTMLAttributes<HTMLDivElement> {
 variant?: GlassCardVariant;
 padding?: "none" | "sm" | "md" | "lg";
 children: ReactNode;
}

const variantStyles: Record<GlassCardVariant, string> = {
 default: [
 "tn-panel",
 "shadow-[var(--shadow-card)]",
 "",
 ].join(" "),

 elevated: [
 "tn-panel-strong",
 "border-[var(--border-hover)]",
 "shadow-[var(--shadow-elevated)]",
 "",
 ].join(" "),

 interactive: [
 "tn-panel",
 "shadow-[var(--shadow-card)]",
 "",
 "cursor-pointer",
 "transition-all duration-500",
 "hover:bg-[var(--bg-surface-hover)]",
 "hover:border-[var(--border-hover)]",
 "hover:shadow-[var(--glow-purple-md)]",
 "hover:-translate-y-1",
 "active:scale-[0.985]",
 ].join(" "),
};

const paddingStyles: Record<string, string> = {
 none: "",
 sm: "p-4",
 md: "p-6 md:p-8",
 lg: "p-8 md:p-10",
};

export default function GlassCard({
 variant = "default",
 padding = "md",
 children,
 className = "",
 ...props
}: GlassCardProps) {
 return (
 <div
 className={[
 "rounded-3xl overflow-hidden",
 variantStyles[variant],
 paddingStyles[padding],
 className,
 ].join(" ")}
 {...props}
 >
 {children}
 </div>
 );
}
