import { Sparkles } from "lucide-react";
import type { CollectionSortOrder } from "@/features/collection/application/useCollectionPage";
import type { CollectionFilterBarLabels } from "@/features/collection/presentation/components/CollectionFilterBar.types";
import { cn } from "@/lib/utils";

interface CollectionSortTabsProps {
  labels: CollectionFilterBarLabels;
  onSortChange: (sort: CollectionSortOrder) => void;
  sortBy: CollectionSortOrder;
}

const SORT_OPTIONS: Array<{
  icon: string;
  id: CollectionSortOrder;
  label: string;
}> = [
  { icon: "📖", id: "id", label: "ID" },
  { icon: "⭐", id: "level", label: "Level" },
  { icon: "⚔️", id: "atk", label: "ATK" },
  { icon: "🛡️", id: "def", label: "DEF" },
];

export default function CollectionSortTabs({
  labels,
  onSortChange,
  sortBy,
}: CollectionSortTabsProps) {
  return (
    <div className={cn("flex flex-wrap items-center gap-2")}>
      <div className={cn("tn-text-muted mr-4 flex items-center gap-2")}>
        <Sparkles className={cn("h-3.5 w-3.5")} />
        <span
          className={cn("tn-text-10 font-black tracking-widest uppercase")}
        >
          {labels.sortLabel}
        </span>
      </div>
      {SORT_OPTIONS.map((option) => (
        <button
          key={option.id}
          type="button"
          onClick={() => onSortChange(option.id)}
          className={cn(
            "flex items-center gap-2 rounded-xl border px-4 py-1.5 tn-text-10 font-black transition-all duration-300",
            sortBy === option.id
              ? "scale-105 border-[var(--warning)]/30 bg-[var(--warning)]/10 text-[var(--warning)] shadow-lg"
              : "tn-border-soft tn-surface hover:tn-border-strong hover:tn-text-secondary text-[var(--text-secondary)]",
          )}
        >
          <span className={cn("text-xs")}>{option.icon}</span>
          {option.label}
        </button>
      ))}
    </div>
  );
}
