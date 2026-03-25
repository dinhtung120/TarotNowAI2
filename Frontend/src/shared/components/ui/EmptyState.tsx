/*
 * ===================================================================
 * COMPONENT: EmptyState
 * BỐI CẢNH (CONTEXT):
 *   Giao diện hiển thị khi một danh sách hay bảng dữ liệu bị trống (Không có Item nào).
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Xóa bỏ sự bối rối của người dùng bằng một thông điệp kèm Icon trực quan.
 *   - Có thể đính kèm một nút Call-to-Action (CTA) để điều hướng người dùng 
 *     thực hiện hành động tạo mới dữ liệu (Ví dụ: "Rút bài ngay").
 * ===================================================================
 */

import { memo, type ReactNode } from "react";
import { Ghost } from "lucide-react";
import { cn } from "@/shared/utils/cn";

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
 <div className="mb-6 tn-text-muted">
 {icon || <Ghost className="w-16 h-16" />}
 </div>

 {/* Title — text lớn hơn message */}
 {title && (
 <h3 className="text-sm font-black uppercase tracking-widest tn-text-muted mb-2">
 {title}
 </h3>
 )}

 {/* Message — mô tả chi tiết hơn */}
 {message && (
 <p className="text-xs tn-text-muted font-medium max-w-sm leading-relaxed mb-6">
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
