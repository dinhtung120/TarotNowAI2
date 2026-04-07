import { CheckCircle2, Filter, LayoutGrid, Lock } from "lucide-react";
import type { CollectionFilter } from "@/features/collection/application/useCollectionPage";
import type { CollectionFilterBarLabels } from "@/features/collection/presentation/components/CollectionFilterBar.types";
import { cn } from "@/lib/utils";

interface CollectionFilterTabsProps {
  activeFilter: CollectionFilter;
  labels: CollectionFilterBarLabels;
  onFilterChange: (filter: CollectionFilter) => void;
}

export default function CollectionFilterTabs({
  activeFilter,
  labels,
  onFilterChange,
}: CollectionFilterTabsProps) {
  const filters = [
    {
      id: "all" as const,
      icon: <LayoutGrid className={cn("h-3 w-3")} />,
      label: labels.filterAll,
    },
    {
      id: "owned" as const,
      icon: <CheckCircle2 className={cn("h-3 w-3")} />,
      label: labels.filterOwned,
    },
    {
      id: "unowned" as const,
      icon: <Lock className={cn("h-3 w-3")} />,
      label: labels.filterUnowned,
    },
  ];

  return (
    <div className={cn("flex flex-wrap items-center gap-2")}>
      <div className={cn("tn-text-muted mr-4 flex items-center gap-2")}>
        <Filter className={cn("h-3.5 w-3.5")} />
        <span
          className={cn("tn-text-10 font-black tracking-widest uppercase")}
        >
          {labels.filtersLabel}
        </span>
      </div>
      {filters.map((filter) => (
        <button
          key={filter.id}
          type="button"
          onClick={() => onFilterChange(filter.id)}
          className={cn(
            "flex items-center gap-2 rounded-full border px-5 py-2 text-xs font-black transition-all duration-300",
            activeFilter === filter.id
              ? "tn-border tn-surface-strong scale-105 text-[var(--text-ink)] shadow-xl"
              : "tn-border-soft tn-surface hover:tn-border-strong hover:tn-text-secondary text-[var(--text-secondary)]",
          )}
        >
          {filter.icon}
          {filter.label}
        </button>
      ))}
    </div>
  );
}
