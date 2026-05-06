import type { ReadersDirectoryViewModel } from "@/features/reader/directory/useReadersDirectoryViewModel";
import { ReaderDirectoryFilters } from "@/features/reader/directory/components/readers-directory";

interface ReadersDirectoryFiltersSectionProps {
  viewModel: ReadersDirectoryViewModel;
}

export default function ReadersDirectoryFiltersSection({
  viewModel,
}: ReadersDirectoryFiltersSectionProps) {
  return (
    <ReaderDirectoryFilters
      searchInput={viewModel.pageState.searchInput}
      searchPlaceholder={viewModel.searchPlaceholder}
      selectedSpecialty={viewModel.pageState.selectedSpecialty}
      selectedStatus={viewModel.pageState.selectedStatus}
      specialties={viewModel.specialties}
      statuses={viewModel.statuses}
      onSearchChange={viewModel.onSearchChange}
      onSpecialtyChange={viewModel.onSpecialtyChange}
      onStatusChange={viewModel.onStatusChange}
    />
  );
}
