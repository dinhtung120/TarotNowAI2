/*
 * ===================================================================
 * COMPONENT: LoadingSpinner
 * BỐI CẢNH (CONTEXT):
 *   Biểu tượng vòng xoay (Spinner) báo hiệu ứng dụng đang xử lý dữ liệu.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Đồng bộ hiệu ứng loading trên toàn App thay vì dùng `<Loader2>` rải rác.
 *   - Có thể cấu hình Kích cỡ (Size), đính kèm Dòng chữ chú thích (Message) 
 *     hoặc chiếm toàn bộ chiều cao màn hình (Full Page) khi đang chặn tương tác.
 * ===================================================================
 */

import { Loader2 } from "lucide-react";

type SpinnerSize = "sm" | "md" | "lg";

interface LoadingSpinnerProps {
 /** Kích thước spinner. Mặc định: "md" */
 size?: SpinnerSize;

 /** Text hiển thị dưới spinner (tùy chọn) */
 message?: string;

 /** Chiếm full height container (center spinner giữa trang) */
 fullPage?: boolean;

 className?: string;
}

const sizeMap: Record<SpinnerSize, string> = {
 sm: "w-4 h-4",
 md: "w-6 h-6",
 lg: "w-10 h-10",
};

export default function LoadingSpinner({
 size = "md",
 message,
 fullPage = false,
 className = "",
}: LoadingSpinnerProps) {
 return (
 <div
 className={[
 "flex flex-col items-center justify-center gap-4",
 fullPage ? "min-h-[60vh]" : "py-12",
 className,
 ].join(" ")}
 >
 {/* Spinner icon — màu tím brand, xoay vô hạn */}
 <Loader2
 className={`${sizeMap[size]} animate-spin text-[var(--purple-accent)]`}
 />

 {/* Optional loading message — style label nhỏ uppercase */}
 {message && (
 <span className="text-[10px] font-black uppercase tracking-widest tn-text-muted">
 {message}
 </span>
 )}
 </div>
 );
}
