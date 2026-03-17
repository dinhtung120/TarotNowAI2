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
 *   1. Màu sắc nhất quán cho cùng loại trạng thái
 *   2. Font size, padding, border-radius chuẩn theo design system
 *   3. Dễ mở rộng thêm variant mới (chỉ thêm 1 entry vào object map)
 */

import { type ReactNode } from "react";

type BadgeVariant =
  | "default"     /* Zinc — neutral, mặc định */
  | "purple"      /* Tím — brand color, premium */
  | "amber"       /* Vàng — gold, diamond */
  | "success"     /* Xanh — approved, online, completed */
  | "error"       /* Đỏ — rejected, offline, failed */
  | "warning"     /* Cam — pending, expiring */
  | "info";       /* Cyan — informational */

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
 * Tại sao không dùng Tailwind class trực tiếp (bg-emerald-500/10)?
 * → Đóng gói tất cả vào component → page code ngắn hơn, dễ đọc hơn.
 * → <Badge variant="success">Online</Badge> thay vì
 *   <span className="bg-emerald-500/10 text-emerald-400 border border-emerald-500/20...">
 */
const variantStyles: Record<BadgeVariant, string> = {
  default:  "bg-zinc-500/10  text-zinc-400  border-zinc-500/20",
  purple:   "bg-purple-500/10 text-purple-400 border-purple-500/20",
  amber:    "bg-amber-500/10  text-amber-400  border-amber-500/20",
  success:  "bg-emerald-500/10 text-emerald-400 border-emerald-500/20",
  error:    "bg-red-500/10  text-red-400  border-red-500/20",
  warning:  "bg-amber-500/10 text-amber-500 border-amber-500/20",
  info:     "bg-cyan-500/10  text-cyan-400  border-cyan-500/20",
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
