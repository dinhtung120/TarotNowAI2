/**
 * SkeletonLoader — Placeholder loading với shimmer animation.
 *
 * === TẠI SAO SKELETON TỐT HƠN SPINNER? ===
 * - Spinner: "Đang tải..." → user không biết CÁI GÌ sắp hiện.
 * - Skeleton: Hiển thị HÌNH DÁNG nội dung sắp tới → user biết trước layout.
 * - Giảm perceived waiting time (thời gian chờ cảm nhận) đáng kể.
 *
 * === CÁC LOẠI SKELETON ===
 * - `text`: Thanh ngang (giả text) — chiều rộng random để trông tự nhiên
 * - `card`: Hình chữ nhật (giả card) — aspect ratio tùy chỉnh
 * - `avatar`: Hình tròn (giả avatar)
 * - `row`: Thanh ngang full width (giả table row)
 *
 * Animation `animate-shimmer` đã khai báo ở globals.css.
 */

type SkeletonType = "text" | "card" | "avatar" | "row";

interface SkeletonLoaderProps {
 /** Loại skeleton. Mặc định: "text" */
 type?: SkeletonType;

 /** Số lượng skeleton items. Mặc định: 1 */
 count?: number;

 /** Custom className cho sizing */
 className?: string;
}

/**
 * Map skeleton type → base styling.
 * → "text": height nhỏ, width ngẫu nhiên (60-100%)
 * → "card": height lớn hơn, full width, ratio 16:9-ish
 * → "avatar": hình tròn
 * → "row": full width, height trung bình (table row)
 */
const typeStyles: Record<SkeletonType, string> = {
 text: "h-4 rounded-lg",
 card: "h-48 rounded-3xl",
 avatar: "w-10 h-10 rounded-full",
 row: "h-16 rounded-2xl",
};

/**
 * Tạo chiều rộng ngẫu nhiên cho text skeleton.
 * → Nhiều line text có width khác nhau → trông giống paragraph thật.
 * → width = 60% ~ 100% (không quá ngắn, không quá dài).
 */
const randomTextWidth = (index: number): string => {
 const widths = ["w-full", "w-5/6", "w-4/5", "w-3/4", "w-2/3"];
 return widths[index % widths.length];
};

export default function SkeletonLoader({
 type = "text",
 count = 1,
 className = "",
}: SkeletonLoaderProps) {
 return (
 <div className={`space-y-3 ${className}`}>
 {Array.from({ length: count }).map((_, i) => (
 <div
 key={i}
 className={[
 /* Base styling theo type */
 typeStyles[type],
 /* Text type: width ngẫu nhiên. Các type khác: full width */
 type === "text" ? randomTextWidth(i) : "w-full",
 /* Shimmer animation — gradient chạy ngang (đã định nghĩa ở globals.css) */
 "animate-shimmer",
 /* Fallback background nếu shimmer chưa load */
 "tn-surface",
 ].join(" ")}
 />
 ))}
 </div>
 );
}
