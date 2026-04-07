

import { cn } from "@/lib/utils";

type SkeletonType = "text" | "card" | "avatar" | "row";

interface SkeletonLoaderProps {
 /** Loại skeleton. Mặc định: "text" */
 type?: SkeletonType;

 /** Số lượng skeleton items. Mặc định: 1 */
 count?: number;

 /** Custom className cho sizing */
 className?: string;
}

const typeStyles: Record<SkeletonType, string> = {
 text: "h-4 rounded-lg",
 card: "h-48 rounded-3xl",
 avatar: "w-10 h-10 rounded-full",
 row: "h-16 rounded-2xl",
};

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
 key={`skeleton-${type}-${i}`}
 className={cn(
 /* Base styling theo type */
 typeStyles[type],
 /* Text type: width ngẫu nhiên. Các type khác: full width */
 type === "text" ? randomTextWidth(i) : "w-full",
 /* Shimmer animation — gradient chạy ngang (đã định nghĩa ở globals.css) */
 "animate-shimmer",
 /* Fallback background nếu shimmer chưa load */
 "tn-surface",
 )}
 />
 ))}
 </div>
 );
}
