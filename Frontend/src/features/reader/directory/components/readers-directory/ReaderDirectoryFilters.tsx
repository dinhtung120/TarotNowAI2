import { Activity, Filter } from "lucide-react";
import type { ReaderDirectoryFilterOption } from "@/features/reader/directory/components/readers-directory/types";
import ReaderDirectorySearchInput from "@/features/reader/directory/components/readers-directory/filters/ReaderDirectorySearchInput";
import ReaderDirectorySelectField from "@/features/reader/directory/components/readers-directory/filters/ReaderDirectorySelectField";
import { GlassCard } from "@/shared/ui";
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
    <GlassCard className={cn("!p-4 tn-reader-filters-shell")}>
      <div className={cn("tn-reader-filters-row")}>
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
