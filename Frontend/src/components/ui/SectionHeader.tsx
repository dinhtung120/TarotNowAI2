/**
 * SectionHeader — Component tiêu đề section thống nhất.
 *
 * === PATTERN LẶP LẠI TRƯỚC ĐÂY ===
 * Mỗi section trong mỗi trang đều có pattern giống nhau:
 * 1. Tag nhỏ uppercase (ví dụ: "Kết nối bậc thầy", "Financial Protocol")
 * 2. Tiêu đề lớn bold italic
 * 3. Mô tả phụ (tùy chọn)
 * 4. Action button/link bên phải (tùy chọn)
 *
 * Ví dụ trước đây (Home page):
 * ```
 * <div className="text-[10px] font-black uppercase tracking-[0.4em] text-[var(--purple-accent)]">
 * Kết nối bậc thầy
 * </div>
 * <h2 className="text-4xl md:text-6xl font-black tn-text-primary ...">
 * Bậc Thầy Dẫn Lối
 * </h2>
 * ```
 *
 * === GIẢI PHÁP ===
 * Component SectionHeader đóng gói pattern này, đảm bảo:
 * - Tag, title, subtitle styling nhất quán giữa tất cả pages
 * - Optional action slot (link "Xem tất cả", button, v.v.)
 */

import { type ReactNode } from "react";

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
 className={[
 "flex flex-col md:flex-row md:items-end justify-between gap-6",
 className,
 ].join(" ")}
 >
 {/* Phần text bên trái */}
 <div className="space-y-3 max-w-2xl">
 {/* Tag — nhãn nhỏ phía trên title */}
 {tag && (
 <div className="inline-flex items-center gap-2 text-[10px] font-black uppercase tracking-[0.3em] text-[var(--purple-accent)]">
 {tagIcon}
 {tag}
 </div>
 )}

 {/* Title — heading chính */}
 <h2
 className={[
 sizeMap[size],
 "font-black tracking-tighter uppercase italic leading-tight lunar-metallic-text",
 ].join(" ")}
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
 {action && <div className="flex-shrink-0">{action}</div>}
 </div>
 );
}
