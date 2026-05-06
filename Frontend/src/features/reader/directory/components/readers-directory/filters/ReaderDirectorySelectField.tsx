import type { LucideIcon } from "lucide-react";
import type { ReaderDirectoryFilterOption } from "@/features/reader/directory/components/readers-directory/types";
import { cn } from "@/lib/utils";

interface ReaderDirectorySelectFieldProps {
  options: ReaderDirectoryFilterOption[];
  value: string;
  Icon: LucideIcon;
  onChange: (value: string) => void;
}

export default function ReaderDirectorySelectField({
  options,
  value,
  Icon,
  onChange,
}: ReaderDirectorySelectFieldProps) {
  return (
    <div className={cn("relative shrink-0 tn-w-48-md")}>
      <select
        className={cn(
          "tn-field tn-field-accent tn-text-primary w-full cursor-pointer appearance-none rounded-xl px-4 py-3 pr-10 text-sm font-medium transition-all",
        )}
        value={value}
        onChange={(event) => onChange(event.target.value)}
      >
        {options.map((option) => (
          <option
            key={option.value}
            className={cn("tn-surface-strong tn-text-primary")}
            value={option.value}
          >
            {option.label}
          </option>
        ))}
      </select>
      <Icon
        className={cn(
          "pointer-events-none absolute top-1/2 right-4 h-4 w-4 -translate-y-1/2 text-[var(--text-tertiary)]",
        )}
      />
    </div>
  );
}
