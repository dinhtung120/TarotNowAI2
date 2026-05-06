"use client";

import { useTranslations } from "next-intl";
import PaginationButton from "@/shared/ui/pagination/PaginationButton";
import { cn } from "@/lib/utils";

interface PaginationProps {
  currentPage: number;
  totalPages: number;
  onPageChange: (page: number) => void;
  isLoading?: boolean;
  className?: string;
}

export default function Pagination({
  currentPage,
  totalPages,
  onPageChange,
  isLoading = false,
  className,
}: PaginationProps) {
  const t = useTranslations("Pagination");
  if (totalPages <= 1) return null;

  const canGoPrev = currentPage > 1 && !isLoading;
  const canGoNext = currentPage < totalPages && !isLoading;

  return (
    <div
      className={cn(
        "flex flex-col items-center justify-between gap-3 border-t border-[var(--border-subtle)] px-4 py-4 sm:flex-row sm:px-6",
        className,
      )}
    >
      <span
        className={cn(
          "tn-text-muted text-center text-[9px] font-black tracking-widest uppercase sm:text-left",
        )}
      >
        {t("page_info", { current: currentPage, total: totalPages })}
      </span>

      <div className={cn("flex items-center gap-2")}>
        <PaginationButton
          ariaLabel={t("prev")}
          direction="prev"
          disabled={!canGoPrev}
          onClick={() => onPageChange(currentPage - 1)}
        />
        <PaginationButton
          ariaLabel={t("next")}
          direction="next"
          disabled={!canGoNext}
          onClick={() => onPageChange(currentPage + 1)}
        />
      </div>
    </div>
  );
}
