/**
 * GlassCard — Component card glassmorphism thống nhất.
 *
 * === VẤN ĐỀ TRƯỚC ĐÂY ===
 * Mỗi trang viết card styling khác nhau:
 * - Home: `bg-white/[0.02] border-white/5 rounded-3xl backdrop-blur-md`
 * - Wallet: `bg-zinc-900/40 backdrop-blur-3xl rounded-[3rem] border-white/10`
 * - Admin: `bg-white/[0.01] backdrop-blur-2xl border-white/5`
 *
 * === GIẢI PHÁP ===
 * Một component với 3 variants:
 * - `default`: Nền glass tiêu chuẩn — cho phần lớn cards
 * - `elevated`: Nền hơi sáng hơn + shadow — cho cards nổi bật (balance, stats)
 * - `interactive`: Có hover/active states — cho cards có thể click
 *
 * Tại sao dùng <div> thay vì <section>/<article>?
 * → GlassCard là container bao quát, không mang nghĩa ngữ nghĩa cụ thể.
 * → Parent page quyết định semantic tag phù hợp.
 * → Có thể thêm `as` prop sau nếu cần.
 */

import { type HTMLAttributes, type ReactNode } from "react";

type GlassCardVariant = "default" | "elevated" | "interactive";

interface GlassCardProps extends HTMLAttributes<HTMLDivElement> {
  /** Kiểu hiển thị. Mặc định: "default" */
  variant?: GlassCardVariant;

  /** Padding size. Mặc định: "md" */
  padding?: "none" | "sm" | "md" | "lg";

  /** Inner content */
  children: ReactNode;
}

/**
 * Map variant → Tailwind classes.
 * Interactive variant thêm cursor-pointer + hover/active states.
 */
const variantStyles: Record<GlassCardVariant, string> = {
  default: [
    "bg-[var(--bg-glass)]",
    "backdrop-blur-xl",
    "border border-[var(--border-default)]",
    "shadow-[var(--shadow-card)]",
  ].join(" "),

  elevated: [
    "bg-zinc-900/40",
    "backdrop-blur-2xl",
    "border border-[var(--border-default)]",
    "shadow-[var(--shadow-elevated)]",
  ].join(" "),

  interactive: [
    "bg-[var(--bg-glass)]",
    "backdrop-blur-xl",
    "border border-[var(--border-default)]",
    "shadow-[var(--shadow-card)]",
    "cursor-pointer",
    "transition-all duration-500",
    "hover:bg-[var(--bg-glass-hover)]",
    "hover:border-[var(--border-hover)]",
    "hover:shadow-[var(--glow-purple-sm)]",
    "hover:-translate-y-1",
    "active:scale-[0.98]",
  ].join(" "),
};

/**
 * Map padding size → Tailwind padding classes.
 * → "none" cho phép custom padding từ parent (ví dụ: table bên trong card)
 */
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
