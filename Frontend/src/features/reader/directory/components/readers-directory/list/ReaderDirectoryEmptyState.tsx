import { Users } from "lucide-react";
import { GlassCard } from "@/shared/ui";
import { cn } from "@/lib/utils";

interface ReaderDirectoryEmptyStateProps {
  label: string;
}

export default function ReaderDirectoryEmptyState({
  label,
}: ReaderDirectoryEmptyStateProps) {
  return (
    <GlassCard
      className={cn(
        "flex tn-h-40vh flex-col items-center justify-center space-y-4 border-dashed",
      )}
    >
      <Users
        className={cn("h-16 w-16 tn-text-tertiary opacity-50")}
      />
      <p className={cn("text-sm font-medium tn-text-secondary")}>
        {label}
      </p>
    </GlassCard>
  );
}
