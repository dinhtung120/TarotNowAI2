

import { Loader2 } from "lucide-react";
import { memo } from "react";
import { cn } from "@/lib/utils";

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

function LoadingSpinnerComponent({
 size = "md",
 message,
 fullPage = false,
 className = "",
}: LoadingSpinnerProps) {
 return (
 <div
 className={cn(
 "flex flex-col items-center justify-center gap-4",
 fullPage ? "min-h-[60vh]" : "py-12",
 className,
 )}
 >
 {/* Spinner icon — màu tím brand, xoay vô hạn */}
 <Loader2
 className={`${sizeMap[size]} animate-spin text-[var(--purple-accent)]`}
 />

 {/* Optional loading message — style label nhỏ uppercase */}
 {message && (
 <span className={cn("text-[10px] font-black uppercase tracking-widest tn-text-muted")}>
 {message}
 </span>
 )}
 </div>
 );
}

const LoadingSpinner = memo(LoadingSpinnerComponent);
LoadingSpinner.displayName = "LoadingSpinner";

export default LoadingSpinner;
