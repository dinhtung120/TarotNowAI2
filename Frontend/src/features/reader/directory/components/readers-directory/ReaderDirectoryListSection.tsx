import type { ReaderProfile } from "@/features/reader/shared";
import ReaderDirectoryEmptyState from "@/features/reader/directory/components/readers-directory/list/ReaderDirectoryEmptyState";
import ReaderDirectoryGrid from "@/features/reader/directory/components/readers-directory/list/ReaderDirectoryGrid";
import ReaderDirectoryLoadingState from "@/features/reader/directory/components/readers-directory/list/ReaderDirectoryLoadingState";
import type {
  ReaderBioResolver,
  ReaderDirectoryListLabels,
} from "@/features/reader/directory/components/readers-directory/types";
import { Pagination } from "@/shared/ui";
import { cn } from "@/lib/utils";

interface ReaderDirectoryListSectionProps {
  readers: ReaderProfile[];
  loading: boolean;
  page: number;
  totalPages: number;
  labels: ReaderDirectoryListLabels;
  getBio: ReaderBioResolver;
  onPageChange: (page: number) => void;
  onSelectReader: (reader: ReaderProfile) => void;
}

export function ReaderDirectoryListSection(
  props: ReaderDirectoryListSectionProps,
) {
  if (props.loading) {
    return <ReaderDirectoryLoadingState label={props.labels.loading} />;
  }

  return (
    <>
      {props.readers.length === 0 ? (
        <ReaderDirectoryEmptyState label={props.labels.empty} />
      ) : (
        <ReaderDirectoryGrid
          getBio={props.getBio}
          labels={props.labels}
          readers={props.readers}
          onSelectReader={props.onSelectReader}
        />
      )}
      <Pagination
        className={cn("mt-8")}
        currentPage={props.page}
        totalPages={props.totalPages}
        onPageChange={props.onPageChange}
      />
    </>
  );
}
