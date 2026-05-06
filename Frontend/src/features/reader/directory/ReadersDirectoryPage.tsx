"use client";

import { ReaderDirectoryHeader } from "@/features/reader/directory/components/readers-directory";
import ReadersDirectoryFiltersSection from "@/features/reader/directory/components/readers-directory/page/ReadersDirectoryFiltersSection";
import ReadersDirectoryModalSection from "@/features/reader/directory/components/readers-directory/page/ReadersDirectoryModalSection";
import ReadersDirectoryResultsSection from "@/features/reader/directory/components/readers-directory/page/ReadersDirectoryResultsSection";
import { useReadersDirectoryViewModel } from "@/features/reader/directory/useReadersDirectoryViewModel";
import { cn } from "@/lib/utils";

export default function ReaderDirectoryPage() {
  const viewModel = useReadersDirectoryViewModel();

  return (
    <div
      className={cn(
        "animate-in fade-in slide-in-from-bottom-8 mx-auto w-full max-w-7xl space-y-10 px-4 pt-8 pb-32 duration-1000 sm:px-6",
      )}
    >
      <ReadersDirectoryModalSection viewModel={viewModel} />
      <ReaderDirectoryHeader
        labels={viewModel.headerLabels}
        totalCount={viewModel.pageState.totalCount}
      />
      <ReadersDirectoryFiltersSection viewModel={viewModel} />
      <ReadersDirectoryResultsSection viewModel={viewModel} />
    </div>
  );
}
