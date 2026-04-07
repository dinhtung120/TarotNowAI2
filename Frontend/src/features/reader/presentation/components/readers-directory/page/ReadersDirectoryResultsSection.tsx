import type { ReadersDirectoryViewModel } from "@/features/reader/presentation/useReadersDirectoryViewModel";
import { ReaderDirectoryListSection } from "@/features/reader/presentation/components/readers-directory";

interface ReadersDirectoryResultsSectionProps {
  viewModel: ReadersDirectoryViewModel;
}

export default function ReadersDirectoryResultsSection({
  viewModel,
}: ReadersDirectoryResultsSectionProps) {
  return (
    <ReaderDirectoryListSection
      getBio={viewModel.getBio}
      labels={viewModel.listLabels}
      loading={viewModel.pageState.loading}
      page={viewModel.pageState.page}
      readers={viewModel.pageState.readers}
      totalPages={viewModel.pageState.totalPages}
      onPageChange={viewModel.pageState.setPage}
      onSelectReader={viewModel.conversationModal.selectReader}
    />
  );
}
