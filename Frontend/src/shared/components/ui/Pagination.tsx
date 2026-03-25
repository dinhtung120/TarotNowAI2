"use client";

/*
 * ===================================================================
 * COMPONENT: Pagination
 * BỐI CẢNH (CONTEXT):
 *   Thanh Phân trang (Pagination) chuẩn hóa cho các danh sách dài.
 * 
 * TÍNH NĂNG CHÍNH:
 *   - Cung cấp hai nút điều hướng Trái/Phải đơn giản thay vì danh sách số 
 *     (1, 2, 3...) gây rối mắt trong Giao diện Glassmorphism.
 *   - Tự động thay đổi trạng thái (Disable) của các nút khi đang ở trang đầu/cuối 
 *     hoặc khi `isLoading` là true.
 * ===================================================================
 */

import { ChevronLeft, ChevronRight } from "lucide-react";
import { useTranslations } from "next-intl";
import { cn } from "@/shared/utils/cn";

interface PaginationProps {
 /** Trang hiện tại (1-indexed) */
 currentPage: number;

 /** Tổng số trang */
 totalPages: number;

 /** Callback khi chuyển trang */
 onPageChange: (page: number) => void;

 /** Có đang loading không (disable buttons khi loading) */
 isLoading?: boolean;

 className?: string;
}

export default function Pagination({
 currentPage,
 totalPages,
 onPageChange,
 isLoading = false,
 className = "",
}: PaginationProps) {
 const t = useTranslations("Pagination");

 /* Không hiển thị pagination nếu chỉ có 1 trang */
 if (totalPages <= 1) return null;

 const canGoPrev = currentPage > 1 && !isLoading;
 const canGoNext = currentPage < totalPages && !isLoading;

 return (
 <div
 className={cn(
 "flex flex-col sm:flex-row items-center justify-between gap-3 px-4 sm:px-6 py-4",
 "border-t border-[var(--border-subtle)]",
 className,
 )}
	 >
	 {/* Text indicator — "Trang X / Y" */}
	 <span className="text-[9px] font-black uppercase tracking-widest tn-text-muted text-center sm:text-left">
	 {t("page_info", { current: currentPage, total: totalPages })}
	 </span>

 {/* Navigation buttons */}
 <div className="flex items-center gap-2">
 <button
 onClick={() => onPageChange(currentPage - 1)}
	 disabled={!canGoPrev}
	 className="p-2.5 min-h-11 min-w-11 rounded-xl bg-[var(--bg-glass)] hover:bg-[var(--bg-glass-hover)] disabled:opacity-30 disabled:cursor-not-allowed transition-all border border-[var(--border-subtle)] cursor-pointer"
	 aria-label={t("prev")}
	 >
 <ChevronLeft className="w-4 h-4 tn-text-secondary" />
 </button>

 <button
 onClick={() => onPageChange(currentPage + 1)}
	 disabled={!canGoNext}
	 className="p-2.5 min-h-11 min-w-11 rounded-xl bg-[var(--bg-glass)] hover:bg-[var(--bg-glass-hover)] disabled:opacity-30 disabled:cursor-not-allowed transition-all border border-[var(--border-subtle)] cursor-pointer"
	 aria-label={t("next")}
	 >
 <ChevronRight className="w-4 h-4 tn-text-secondary" />
 </button>
 </div>
 </div>
 );
}
