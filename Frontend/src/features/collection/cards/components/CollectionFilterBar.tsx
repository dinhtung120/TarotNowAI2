import type { CollectionFilterBarProps } from "@/features/collection/cards/components/CollectionFilterBar.types";
import CollectionFilterTabs from "@/features/collection/cards/components/filter-bar/CollectionFilterTabs";
import CollectionSortTabs from "@/features/collection/cards/components/filter-bar/CollectionSortTabs";
import { cn } from "@/lib/utils";

export function CollectionFilterBar({
  activeFilter,
  labels,
  onFilterChange,
  onSortChange,
  sortBy,
}: CollectionFilterBarProps) {
  return (
    <div
      className={cn(
        "animate-in fade-in mb-10 flex flex-col gap-6 delay-300 duration-700 md:flex-row md:items-center md:justify-between",
      )}
    >
      <CollectionFilterTabs
        activeFilter={activeFilter}
        labels={labels}
        onFilterChange={onFilterChange}
      />
      <CollectionSortTabs
        labels={labels}
        onSortChange={onSortChange}
        sortBy={sortBy}
      />
    </div>
  );
}
