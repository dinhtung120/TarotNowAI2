import type {
  CollectionFilter,
  CollectionSortOrder,
} from "@/features/collection/application/useCollectionPage";

export interface CollectionFilterBarLabels {
  filterAll: string;
  filterOwned: string;
  filterUnowned: string;
  filtersLabel: string;
  sortLabel: string;
}

export interface CollectionFilterBarProps {
  activeFilter: CollectionFilter;
  labels: CollectionFilterBarLabels;
  onFilterChange: (filter: CollectionFilter) => void;
  onSortChange: (sort: CollectionSortOrder) => void;
  sortBy: CollectionSortOrder;
}
