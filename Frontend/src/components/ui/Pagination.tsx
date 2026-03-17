"use client";

/**
 * Pagination — Component phân trang thống nhất.
 *
 * === VẤN ĐỀ TRƯỚC ĐÂY ===
 * Mỗi trang tự viết pagination riêng:
 * - Wallet: Nút prev/next + text "Vũ trụ X / Y"
 * - Admin Users: Inline text + buttons
 * - History: Khác styling hoàn toàn
 *
 * === GIẢI PHÁP ===
 * Một component thống nhất hiển thị:
 * 1. Text "Trang X / Y"
 * 2. Nút Previous và Next
 * 3. Tự động disable khi ở đầu/cuối
 *
 * Tại sao không dùng numbered page buttons (1, 2, 3...)?
 * → Với dark/glass theme, nhiều number buttons trông rối.
 * → Prev/Next đơn giản, phù hợp aesthetic minimalist.
 * → Có thể thêm numbered pagination sau nếu cần.
 */

import { ChevronLeft, ChevronRight } from "lucide-react";
import { useTranslations } from "next-intl";

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
 className={[
 "flex items-center justify-between px-6 py-4",
 "border-t border-[var(--border-subtle)]",
 className,
 ].join(" ")}
	 >
	 {/* Text indicator — "Trang X / Y" */}
	 <span className="text-[9px] font-black uppercase tracking-widest tn-text-muted">
	 {t("page_info", { current: currentPage, total: totalPages })}
	 </span>

 {/* Navigation buttons */}
 <div className="flex items-center gap-2">
 <button
 onClick={() => onPageChange(currentPage - 1)}
	 disabled={!canGoPrev}
	 className="p-2 rounded-xl bg-[var(--bg-glass)] hover:bg-[var(--bg-glass-hover)] disabled:opacity-30 disabled:cursor-not-allowed transition-all border border-[var(--border-subtle)] cursor-pointer"
	 aria-label={t("prev")}
	 >
 <ChevronLeft className="w-4 h-4 tn-text-secondary" />
 </button>

 <button
 onClick={() => onPageChange(currentPage + 1)}
	 disabled={!canGoNext}
	 className="p-2 rounded-xl bg-[var(--bg-glass)] hover:bg-[var(--bg-glass-hover)] disabled:opacity-30 disabled:cursor-not-allowed transition-all border border-[var(--border-subtle)] cursor-pointer"
	 aria-label={t("next")}
	 >
 <ChevronRight className="w-4 h-4 tn-text-secondary" />
 </button>
 </div>
 </div>
 );
}
