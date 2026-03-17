/**
 * EmptyState — Component hiển thị khi danh sách/bảng trống.
 *
 * === TẠI SAO CẦN THIẾT? ===
 * Một trang trống không có gì → user bối rối "Lỗi hay không có data?"
 * EmptyState giải quyết bằng:
 * 1. Icon minh họa → visual cue rõ ràng
 * 2. Message giải thích → "Chưa có giao dịch nào"
 * 3. CTA button (tùy chọn) → hướng dẫn hành động tiếp theo
 *
 * Dùng ở:
 * - Lịch sử giao dịch trống (Wallet ledger)
 * - Danh sách conversation trống (Chat inbox)
 * - Bộ sưu tập bài trống (Collection)
 * - Kết quả tìm kiếm không có (Admin users)
 */

import { type ReactNode } from "react";
import { Ghost } from "lucide-react";

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

export default function EmptyState({
  icon,
  title = "Không có dữ liệu",
  message,
  action,
  className = "",
}: EmptyStateProps) {
  return (
    <div
      className={[
        "flex flex-col items-center justify-center text-center py-20 px-6",
        className,
      ].join(" ")}
    >
      {/* Icon — kích thước lớn, opacity thấp → decorative */}
      <div className="mb-6 text-zinc-700">
        {icon || <Ghost className="w-16 h-16" />}
      </div>

      {/* Title — text lớn hơn message */}
      <h3 className="text-sm font-black uppercase tracking-widest text-zinc-500 mb-2">
        {title}
      </h3>

      {/* Message — mô tả chi tiết hơn */}
      {message && (
        <p className="text-xs text-zinc-600 font-medium max-w-sm leading-relaxed mb-6">
          {message}
        </p>
      )}

      {/* CTA action — ví dụ: <Button>Rút bài ngay</Button> */}
      {action}
    </div>
  );
}
