import type { ReaderProfile } from "@/features/reader/application/actions";
import ReaderDirectoryCardAvatar from "@/features/reader/presentation/components/readers-directory/card/ReaderDirectoryCardAvatar";
import { ReaderStatusIndicator } from "@/features/reader/presentation/components/readers-directory/ReaderStatusIndicator";
import type { ReaderDirectoryCardLabels } from "@/features/reader/presentation/components/readers-directory/types";
import { cn } from "@/lib/utils";

interface ReaderDirectoryCardProfileProps {
  labels: ReaderDirectoryCardLabels;
  reader: ReaderProfile;
}

export default function ReaderDirectoryCardProfile({
  labels,
  reader,
}: ReaderDirectoryCardProfileProps) {
  return (
    <div className={cn("flex items-center gap-4")}>
      <ReaderDirectoryCardAvatar
        avatarUrl={reader.avatarUrl}
        displayName={reader.displayName}
      />
      <div>
        <h3
          className={cn(
            "tn-text-primary line-clamp-1 text-base font-black tracking-tight italic",
          )}
        >
          {reader.displayName || labels.readerFallback}
        </h3>
        <div className={cn("mt-1")}>
          <ReaderStatusIndicator status={reader.status} labels={labels} />
        </div>
      </div>
    </div>
  );
}
