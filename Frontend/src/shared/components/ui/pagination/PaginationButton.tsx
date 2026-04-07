import { ChevronLeft, ChevronRight } from "lucide-react";
import { cn } from "@/lib/utils";

interface PaginationButtonProps {
  ariaLabel: string;
  disabled: boolean;
  direction: "prev" | "next";
  onClick: () => void;
}

export default function PaginationButton({
  ariaLabel,
  disabled,
  direction,
  onClick,
}: PaginationButtonProps) {
  const Icon = direction === "prev" ? ChevronLeft : ChevronRight;

  return (
    <button
      aria-label={ariaLabel}
      className={cn(
        "min-h-11 min-w-11 cursor-pointer rounded-xl border border-[var(--border-subtle)] bg-[var(--bg-glass)] p-2.5 transition-all hover:bg-[var(--bg-glass-hover)] disabled:cursor-not-allowed disabled:opacity-30",
      )}
      disabled={disabled}
      type="button"
      onClick={onClick}
    >
      <Icon className={cn("tn-text-secondary h-4 w-4")} />
    </button>
  );
}
