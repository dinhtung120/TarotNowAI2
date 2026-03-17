"use client";

/**
 * Modal — Component dialog overlay thống nhất cho toàn app.
 *
 * === CÁC NƠI CẦN DÙNG ===
 * - MfaChallengeModal (xác thực 2 lớp)
 * - ReportModal (báo cáo vi phạm)
 * - Card detail modal (xem chi tiết bài Tarot)
 * - Confirm actions (xác nhận xóa, approve, reject)
 *
 * === DESIGN ===
 * - Overlay tối mờ (black/70) không blur
 * - Content card: dark surface (bg-surface, border subtle)
 * - Animation: fade-in overlay + scale-in content
 * - ESC key + click outside → đóng modal
 * - Focus trap? Chưa implement (sẽ thêm sau nếu cần accessibility)
 *
 * Tại sao dùng Portal (createPortal)?
 * → Modal cần render ở NGOÀI DOM tree của component cha.
 * → Nếu parent có overflow:hidden hoặc z-index thấp → modal bị cắt/che.
 * → Portal render vào document.body → luôn nổi trên cùng.
 */

import { useEffect, useCallback, type ReactNode } from "react";
import { createPortal } from "react-dom";
import { X } from "lucide-react";

type ModalSize = "sm" | "md" | "lg";

interface ModalProps {
  /** Trạng thái cùng/mở modal */
  isOpen: boolean;

  /** Callback khi đóng modal (ESC, click outside, nút X) */
  onClose: () => void;

  /** Tiêu đề modal (hiển thị ở header) */
  title?: string;

  /** Mô tả ngắn dưới tiêu đề */
  description?: string;

  /** Kích thước max-width. Mặc định: "md" */
  size?: ModalSize;

  /** Nội dung bên trong modal */
  children: ReactNode;

  /** Có hiển thị nút X đóng không? Mặc định: true */
  showCloseButton?: boolean;
}

const sizeStyles: Record<ModalSize, string> = {
  sm: "max-w-md",
  md: "max-w-lg",
  lg: "max-w-2xl",
};

export default function Modal({
  isOpen,
  onClose,
  title,
  description,
  size = "md",
  children,
  showCloseButton = true,
}: ModalProps) {
  /**
   * Xử lý phím ESC để đóng modal.
   *
   * Tại sao dùng useCallback + useEffect thay vì onKeyDown trên div?
   * → onKeyDown chỉ hoạt động khi div đã focused.
   * → document.addEventListener luôn bắt được, dù focus ở đâu.
   * → useCallback tránh tạo function mới mỗi render → cleanup đúng listener.
   */
  const handleEsc = useCallback(
    (e: KeyboardEvent) => {
      if (e.key === "Escape") onClose();
    },
    [onClose],
  );

  useEffect(() => {
    if (isOpen) {
      document.addEventListener("keydown", handleEsc);
      /* Lock body scroll khi modal mở — tránh scroll content phía sau */
      document.body.style.overflow = "hidden";
    }

    return () => {
      document.removeEventListener("keydown", handleEsc);
      document.body.style.overflow = "";
    };
  }, [isOpen, handleEsc]);

  /* Không render gì nếu modal đóng */
  if (!isOpen) return null;

  /**
   * createPortal render content vào document.body.
   * → Overlay luôn full-screen, không bị parent layout ảnh hưởng.
   */
  return createPortal(
    <div
      className="fixed inset-0 z-[9999] flex items-center justify-center p-4"
      role="dialog"
      aria-modal="true"
      aria-label={title}
    >
      {/* === OVERLAY ===
          Click vào overlay (vùng tối) → đóng modal.
          không dùng blur để giữ phong cách non-glass. */}
      <div
        className="absolute inset-0 bg-black/70 animate-in fade-in duration-200"
        onClick={onClose}
      />

      {/* === CONTENT CARD ===
          animate-in + zoom-in-95: card xuất hiện với hiệu ứng scale nhẹ.
          bg-surface (không dùng glass) vì modal cần contrast rõ với nền. */}
      <div
        className={[
          "relative z-10 w-full",
          sizeStyles[size],
          "bg-[var(--bg-surface)]",
          "border border-[var(--border-default)]",
          "rounded-3xl",
          "shadow-[var(--shadow-elevated)]",
          "animate-in fade-in zoom-in-95 duration-300",
        ].join(" ")}
      >
        {/* === HEADER (nếu có title) === */}
        {(title || showCloseButton) && (
          <div className="flex items-start justify-between p-6 pb-0">
            <div className="space-y-1">
              {title && (
                <h2 className="text-lg font-black text-[var(--text-primary)] tracking-tight">
                  {title}
                </h2>
              )}
              {description && (
                <p className="text-sm text-[var(--text-secondary)] font-medium">
                  {description}
                </p>
              )}
            </div>

            {/* Nút đóng X — luôn ở góc phải trên */}
            {showCloseButton && (
              <button
                onClick={onClose}
                className="p-2 rounded-xl hover:bg-[var(--purple-50)] transition-colors text-[var(--text-secondary)] hover:text-[var(--text-primary)] cursor-pointer"
                aria-label="Đóng modal"
              >
                <X className="w-5 h-5" />
              </button>
            )}
          </div>
        )}

        {/* === BODY === */}
        <div className="p-6">{children}</div>
      </div>
    </div>,
    document.body,
  );
}
