/**
 * LoadingSpinner — Spinner loading thống nhất cho toàn app.
 *
 * Tại sao cần component riêng thay vì dùng Loader2 trực tiếp?
 * → Đảm bảo styling (màu, size, animation) nhất quán.
 * → Có thể thêm text loading message bên dưới spinner.
 * → Centralized: thay đổi spinner style ở 1 nơi → cập nhật toàn app.
 *
 * Dùng ở:
 * - Trạng thái loading khi fetch data
 * - Placeholder khi chờ AI stream
 * - Loading overlay trên toàn trang
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
        <span className="text-[10px] font-black uppercase tracking-widest text-zinc-500">
          {message}
        </span>
      )}
    </div>
  );
}
