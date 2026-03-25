/*
 * ===================================================================
 * COMPONENT: SectionHeader
 * BỐI CẢNH (CONTEXT):
 *   Bố cục Tiêu đề (Header) tiêu chuẩn dành cho các phân vùng (Section) lớn của App.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Gom gọn pattern "Tag nhỏ + Tiêu đề lớn + Mô tả + Nút bấm phụ" thành 
 *     một khối logic thống nhất.
 *   - Hỗ trợ truyền Slot (Action) cho phép đặt Button "Xem thêm", Link chuyển trang 
 *     ở bên phải Tiêu đề (Desktop) hoặc rớt dòng linh hoạt (Mobile).
 * ===================================================================
 */

import { type ReactNode } from "react";
import { cn } from "@/shared/utils/cn";

interface SectionHeaderProps {
 /** Tag nhỏ phía trên title (ví dụ: "Financial Protocol") */
 tag?: string;

 /** Tiêu đề chính (h2) */
 title: string;

 /** Phần mờ/nhạt của title — render sau phần chính */
 titleMuted?: string;

 /** Mô tả phụ dưới title */
 subtitle?: string;

 /** Action slot bên phải (button, link, v.v.) */
 action?: ReactNode;

 /** Icon nhỏ bên cạnh tag */
 tagIcon?: ReactNode;

 /** Kích thước heading. Mặc định: "lg" */
 size?: "sm" | "md" | "lg";

 className?: string;
}

const sizeMap = {
 sm: "text-xl md:text-2xl",
 md: "text-2xl md:text-4xl",
 lg: "text-3xl md:text-5xl",
};

export default function SectionHeader({
 tag,
 title,
 titleMuted,
 subtitle,
 action,
 tagIcon,
 size = "lg",
 className = "",
}: SectionHeaderProps) {
 return (
 <div
 className={cn(
 "flex flex-col md:flex-row md:items-end justify-between gap-4 md:gap-6",
 className,
 )}
 >
 {/* Phần text bên trái */}
 <div className="space-y-3 max-w-2xl">
 {/* Tag — nhãn nhỏ phía trên title */}
 {tag && (
 <div className="inline-flex items-center gap-2 text-[10px] font-black uppercase tracking-[0.25em] sm:tracking-[0.3em] text-[var(--purple-accent)]">
 {tagIcon}
 {tag}
 </div>
 )}

 {/* Title — heading chính */}
 <h2
 className={cn(
 sizeMap[size],
 "font-black tracking-tighter uppercase italic leading-tight lunar-metallic-text",
 )}
 >
 {title}
 {/* Phần muted — màu nhạt hơn, thường italic */}
 {titleMuted && (
 <span className="text-[var(--text-secondary)]"> {titleMuted}</span>
 )}
 </h2>

 {/* Subtitle — mô tả phụ */}
 {subtitle && (
 <p className="text-[var(--text-secondary)] font-medium text-sm leading-relaxed max-w-lg">
 {subtitle}
 </p>
 )}
 </div>

 {/* Action slot bên phải — tùy chọn */}
 {action && <div className="w-full md:w-auto md:flex-shrink-0">{action}</div>}
 </div>
 );
}
