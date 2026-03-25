"use client";

/*
 * ===================================================================
 * COMPONENT: Button
 * BỐI CẢNH (CONTEXT):
 *   Nút bấm (Button) cốt lõi của toàn hệ thống, áp dụng chuẩn thiết kế "Lunar Bloom".
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Tích hợp 5 Variants (Primary, Brand, Secondary, Ghost, Danger) và 3 Sizes.
 *   - Hỗ trợ hiển thị Spinner quay (Loading) và Icon hai bên rập khuôn.
 *   - Sử dụng `forwardRef` để tương thích hoàn hảo với React Hook Form và các 
 *     thư viện Tooltip/Popover cần truy xuất phần tử DOM thực.
 * ===================================================================
 */

import { forwardRef, type ButtonHTMLAttributes, type ReactNode } from "react";
import { Loader2 } from "lucide-react";
import { cn } from "@/shared/utils/cn";

/**
 * Các variant và size được thiết kế dựa trên Design System:
 * → Border radius, font weight, letter-spacing, padding
 * đều tuân theo chuẩn đã định trong FRONTEND_REDESIGN.md.
 */
type ButtonVariant = "primary" | "brand" | "secondary" | "ghost" | "danger";
type ButtonSize = "sm" | "md" | "lg";

interface ButtonProps extends ButtonHTMLAttributes<HTMLButtonElement> {
 /** Kiểu dáng của nút. Mặc định: "primary" */
 variant?: ButtonVariant;

 /** Kích thước. Mặc định: "md" */
 size?: ButtonSize;

 /** Hiển thị spinner loading và disable nút */
 isLoading?: boolean;

 /** Icon hiển thị bên trái text */
 leftIcon?: ReactNode;

 /** Icon hiển thị bên phải text */
 rightIcon?: ReactNode;

 /** Chiếm toàn bộ chiều ngang container */
 fullWidth?: boolean;
}

/**
 * Map styling string theo variant.
 *
 * Tại sao dùng object map thay vì if-else?
 * → Dễ mở rộng: thêm variant mới chỉ cần thêm 1 key.
 * → TypeScript type-check: nếu variant không có trong map → lỗi compile.
 * → Đọc nhanh hơn chuỗi if-else dài.
 */
const variantStyles: Record<ButtonVariant, string> = {
 primary: cn(
 "bg-[var(--bg-elevated)] text-[var(--text-ink)]",
 "border border-[var(--border-default)]",
 "hover:bg-[var(--bg-surface-hover)] hover:border-[var(--border-hover)]",
 "active:scale-[0.97]",
 "shadow-[var(--shadow-card)]",
 "hover:shadow-[var(--glow-purple-md)]",
 ),

 brand: cn(
 "bg-gradient-to-r from-[var(--purple-gradient-from)] via-[var(--purple-gradient-via)] to-[var(--purple-gradient-to)]",
 "text-[var(--text-ink)]",
 "border border-[var(--border-hover)]",
 "hover:brightness-105",
 "active:scale-[0.97]",
 "shadow-[var(--glow-purple-sm)] hover:shadow-[var(--glow-purple-lg)]",
 ),

 secondary: cn(
 "bg-[var(--bg-glass)]",
 "text-[var(--text-primary)]",
 "border border-[var(--border-default)]",
 "hover:bg-[var(--bg-glass-hover)] hover:border-[var(--border-hover)] hover:shadow-[var(--shadow-card)]",
 "active:scale-[0.97]",
 ),

 ghost: cn(
 "bg-transparent",
 "text-[var(--text-secondary)]",
 "hover:text-[var(--text-ink)] hover:bg-[var(--purple-50)]",
 ),

 danger: cn(
 "bg-[var(--danger)]/10",
 "text-[var(--danger)]",
 "border border-[var(--danger)]/20",
 "hover:bg-[var(--danger)]/20 hover:border-[var(--danger)]/30",
 "active:scale-[0.97]",
 ),
};

/**
 * Map size → padding, font-size, letter-spacing.
 * Tuân theo Design System:
 * - sm: label nhỏ, dùng cho inline actions
 * - md: kích thước chuẩn
 * - lg: CTA lớn (Hero, form submit)
 */
const sizeStyles: Record<ButtonSize, string> = {
 sm: "px-3 py-1.5 min-h-11 text-[10px] font-bold tracking-widest rounded-xl gap-1.5",
 md: "px-5 py-2.5 min-h-11 text-[11px] font-black tracking-widest rounded-2xl gap-2",
 lg: "px-8 py-4 min-h-12 text-xs font-black tracking-[0.2em] rounded-2xl gap-3",
};

const Button = forwardRef<HTMLButtonElement, ButtonProps>(
 (
 {
 variant = "primary",
 size = "md",
 isLoading = false,
 leftIcon,
 rightIcon,
 fullWidth = false,
 disabled,
 children,
 className = "",
 ...props
 },
 ref,
 ) => {
 return (
 <button
 ref={ref}
 disabled={disabled || isLoading}
 className={cn(
 /* Base — áp dụng cho TẤT CẢ variants */
 "inline-flex items-center justify-center",
 "uppercase",
 "transition-all duration-300",
 "disabled:opacity-50 disabled:cursor-not-allowed disabled:scale-100",
 "cursor-pointer",
 /* Variant-specific */
 variantStyles[variant],
 /* Size-specific */
 sizeStyles[size],
 /* Full width */
 fullWidth ? "w-full" : "",
 /* Custom classes từ parent */
 className,
 )}
 {...props}
 >
 {/* Loading spinner thay thế leftIcon khi đang loading */}
 {isLoading ? (
 <Loader2 className="w-4 h-4 animate-spin" />
 ) : (
 leftIcon
 )}

 {children}

 {/* RightIcon ẩn khi loading để focus vào spinner */}
 {!isLoading && rightIcon}
 </button>
 );
 },
);

Button.displayName = "Button";

export default Button;
