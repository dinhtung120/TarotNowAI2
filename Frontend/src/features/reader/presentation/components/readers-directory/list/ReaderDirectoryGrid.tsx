import type { ReaderProfile } from "@/features/reader/application/actions";
import { ReaderDirectoryCard } from "@/features/reader/presentation/components/readers-directory/ReaderDirectoryCard";
import type {
  ReaderBioResolver,
  ReaderDirectoryCardLabels,
} from "@/features/reader/presentation/components/readers-directory/types";
import { cn } from "@/lib/utils";

interface ReaderDirectoryGridProps {
  readers: ReaderProfile[];
  labels: ReaderDirectoryCardLabels;
  getBio: ReaderBioResolver;
  onSelectReader: (reader: ReaderProfile) => void;
}

export default function ReaderDirectoryGrid({
  readers,
  labels,
  getBio,
  onSelectReader,
}: ReaderDirectoryGridProps) {
  return (
    <div
      className={cn(
        "grid grid-cols-1 gap-6 md:grid-cols-2 lg:grid-cols-3 xl:grid-cols-4",
      )}
    >
      {readers.map((reader) => (
        <ReaderDirectoryCard
          key={reader.id}
          bio={getBio(reader)}
          labels={labels}
          reader={reader}
          onSelect={onSelectReader}
        />
      ))}
    </div>
  );
}
