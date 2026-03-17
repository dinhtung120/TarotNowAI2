/**
 * Badge — Component hiển thị nhãn trạng thái nhỏ gọn.
 *
 * Dùng cho:
 * - Trạng thái đơn hàng (pending, approved, rejected)
 * - Loại tiền tệ (Gold, Diamond)
 * - Role người dùng (Admin, Reader, User)
 * - Giá spread (Free, 50 Gold, 100 Diamond)
 *
 * Tại sao Badge cần thiết?
 * → Trước đây mỗi trang tự viết span với styling khác nhau cho cùng concept.
 * → Badge component đảm bảo:
 * 1. Màu sắc nhất quán cho cùng loại trạng thái
 * 2. Font size, padding, border-radius chuẩn theo design system
 * 3. Dễ mở rộng thêm variant mới (chỉ thêm 1 entry vào object map)
 */

import { type ReactNode } from "react";

type BadgeVariant =
 | "default" /* Zinc — neutral, mặc định */
 | "purple" /* Tím — brand color, premium */
 | "amber" /* Vàng — gold, diamond */
 | "success" /* Xanh — approved, online, completed */
 | "error" /* Đỏ — rejected, offline, failed */
 | "warning" /* Cam — pending, expiring */
 | "info"; /* Cyan — informational */

type BadgeSize = "sm" | "md";

interface BadgeProps {
 variant?: BadgeVariant;
 size?: BadgeSize;
 /** Nội dung badge — text hoặc icon + text */
 children: ReactNode;
 className?: string;
}

/**
 * Map variant → colours.
 * Mỗi variant có:
 * - Background mờ (opacity 10-15%)
 * - Text color tương ứng
 * - Border color nhẹ (opacity 20-30%)
 *
 * Tại sao không dùng Tailwind class trực tiếp (bg-[var(--success)]/10)?
 * → Đóng gói tất cả vào component → page code ngắn hơn, dễ đọc hơn.
 * → <Badge variant="success">Online</Badge> thay vì
 * <span className="bg-[var(--success)]/10 text-[var(--success)] border border-[var(--success)]/20...">
 */
const variantStyles: Record<BadgeVariant, string> = {
 default: "bg-[var(--bg-elevated)] text-[var(--text-secondary)] border-[var(--border-default)]",
 purple: "bg-[var(--purple-50)] text-[var(--purple-muted)] border-[var(--border-default)]",
 amber: "bg-[color:var(--c-240-230-140-24)] text-[color:var(--c-hex-8c7a2f)] border-[color:var(--c-240-230-140-46)]",
 success: "bg-[var(--success-bg)] text-[var(--success)] border-[color:var(--c-138-184-154-42)]",
 error: "bg-[var(--error-bg)] text-[var(--error)] border-[color:var(--c-204-124-149-38)]",
 warning: "bg-[var(--warning-bg)] text-[color:var(--c-hex-9f8338)] border-[color:var(--c-215-190-125-42)]",
 info: "bg-[var(--info-bg)] text-[var(--info)] border-[color:var(--c-142-167-219-42)]",
};

const sizeStyles: Record<BadgeSize, string> = {
 sm: "px-2 py-0.5 text-[9px]",
 md: "px-3 py-1 text-[10px]",
};

export default function Badge({
 variant = "default",
 size = "sm",
 children,
 className = "",
}: BadgeProps) {
 return (
 <span
 className={[
 "inline-flex items-center gap-1",
 "font-black uppercase tracking-wider",
 "border rounded-full",
 "whitespace-nowrap",
 variantStyles[variant],
 sizeStyles[size],
 className,
 ].join(" ")}
 >
 {children}
 </span>
 );
}
