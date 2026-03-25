/*
 * ===================================================================
 * COMPONENT: GlassCard
 * BỐI CẢNH (CONTEXT):
 *   Khối giao diện (Card) dạng kính mờ (Glassmorphism), tạo chiều sâu 3D cho App.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Đóng gói logic UI của nền tối/mờ, đổ bóng (Shadow) và đường viền (Border).
 *   - Hỗ trợ chế độ "interactive" (Có tương tác) khi hover sẽ nảy lên và phát sáng, 
 *     thích hợp làm các nút chọn lớn (Card Button).
 *   - Tên component giữ nguyên từ phiên bản cũ (Legacy) để tương thích code hiện tại.
 * ===================================================================
 */

import { memo, type HTMLAttributes, type ReactNode } from "react";
import { cn } from "@/shared/utils/cn";

type GlassCardVariant = "default" | "elevated" | "interactive";

interface GlassCardProps extends HTMLAttributes<HTMLDivElement> {
 variant?: GlassCardVariant;
 padding?: "none" | "sm" | "md" | "lg";
 children: ReactNode;
}

const variantStyles: Record<GlassCardVariant, string> = {
 default: cn(
 "tn-panel",
 "shadow-[var(--shadow-card)]",
 "",
 ),

 elevated: cn(
 "tn-panel-strong",
 "border-[var(--border-hover)]",
 "shadow-[var(--shadow-elevated)]",
 "",
 ),

 interactive: cn(
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
 ),
};

const paddingStyles: Record<string, string> = {
 none: "",
 sm: "p-4",
 md: "p-6 md:p-8",
 lg: "p-8 md:p-10",
};

function GlassCardComponent({
 variant = "default",
 padding = "md",
 children,
 className = "",
 ...props
}: GlassCardProps) {
 return (
 <div
 className={cn(
 "rounded-3xl overflow-hidden",
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
