import { Activity, Filter } from "lucide-react";
import type { ReaderDirectoryFilterOption } from "@/features/reader/presentation/components/readers-directory/types";
import ReaderDirectorySearchInput from "@/features/reader/presentation/components/readers-directory/filters/ReaderDirectorySearchInput";
import ReaderDirectorySelectField from "@/features/reader/presentation/components/readers-directory/filters/ReaderDirectorySelectField";
import { GlassCard } from "@/shared/components/ui";
import { cn } from "@/lib/utils";

interface ReaderDirectoryFiltersProps {
  searchInput: string;
  selectedSpecialty: string;
  selectedStatus: string;
  specialties: ReaderDirectoryFilterOption[];
  statuses: ReaderDirectoryFilterOption[];
  searchPlaceholder: string;
  onSearchChange: (value: string) => void;
  onSpecialtyChange: (value: string) => void;
  onStatusChange: (value: string) => void;
}

export function ReaderDirectoryFilters(props: ReaderDirectoryFiltersProps) {
  return (
    <GlassCard className={cn("!p-4 shadow-2xl md:sticky md:top-24 md:z-30")}>
      <div className={cn("flex flex-col gap-4 md:flex-row")}>
        <ReaderDirectorySearchInput
          placeholder={props.searchPlaceholder}
          value={props.searchInput}
          onChange={props.onSearchChange}
        />
        <ReaderDirectorySelectField
          Icon={Filter}
          options={props.specialties}
          value={props.selectedSpecialty}
          onChange={props.onSpecialtyChange}
        />
        <ReaderDirectorySelectField
          Icon={Activity}
          options={props.statuses}
          value={props.selectedStatus}
          onChange={props.onStatusChange}
        />
      </div>
    </GlassCard>
  );
}
