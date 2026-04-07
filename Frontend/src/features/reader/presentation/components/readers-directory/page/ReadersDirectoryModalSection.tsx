import type { ReadersDirectoryViewModel } from "@/features/reader/presentation/useReadersDirectoryViewModel";
import { ReaderDetailModal } from "@/features/reader/presentation/components/readers-directory";

interface ReadersDirectoryModalSectionProps {
  viewModel: ReadersDirectoryViewModel;
}

export default function ReadersDirectoryModalSection({
  viewModel,
}: ReadersDirectoryModalSectionProps) {
  return (
    <ReaderDetailModal
      bio={viewModel.selectedBio}
      isStartingConversation={
        viewModel.conversationModal.isStartingConversation
      }
      labels={viewModel.detailLabels}
      reader={viewModel.conversationModal.selectedReader}
      onClose={viewModel.conversationModal.closeReader}
      onStartConversation={viewModel.conversationModal.startConversation}
    />
  );
}
