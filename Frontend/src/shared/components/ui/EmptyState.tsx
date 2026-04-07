

import { memo, type ReactNode } from "react";
import { Ghost } from "lucide-react";
import { cn } from "@/lib/utils";

interface EmptyStateProps {
 /** Icon hiển thị — mặc định Ghost icon */
 icon?: ReactNode;

 /** Tiêu đề chính (ví dụ: "Chưa có dữ liệu") */
 title?: string;

 /** Mô tả phụ (ví dụ: "Hãy rút bài Tarot đầu tiên!") */
 message?: string;

 /** CTA button (tùy chọn) */
 action?: ReactNode;

 className?: string;
}

function EmptyStateComponent({
 icon,
 title = "",
 message,
 action,
 className = "",
}: EmptyStateProps) {
 return (
 <div
 className={cn(
 "flex flex-col items-center justify-center text-center py-20 px-6",
 className,
 )}
 >
 {/* Icon — kích thước lớn, opacity thấp → decorative */}
 <div className={cn("mb-6 tn-text-muted")}>
 {icon || <Ghost className={cn("w-16 h-16")} />}
 </div>

 {/* Title — text lớn hơn message */}
 {title && (
 <h3 className={cn("text-sm font-black uppercase tracking-widest tn-text-muted mb-2")}>
 {title}
 </h3>
 )}

 {/* Message — mô tả chi tiết hơn */}
 {message && (
 <p className={cn("text-xs tn-text-muted font-medium max-w-sm leading-relaxed mb-6")}>
 {message}
 </p>
 )}

 {/* CTA action — ví dụ: <Button>Rút bài ngay</Button> */}
 {action}
 </div>
 );
}

const EmptyState = memo(EmptyStateComponent);
EmptyState.displayName = "EmptyState";

export default EmptyState;
