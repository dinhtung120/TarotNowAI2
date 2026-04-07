import { Search } from "lucide-react";
import { cn } from "@/lib/utils";

interface ReaderDirectorySearchInputProps {
  value: string;
  placeholder: string;
  onChange: (value: string) => void;
}

export default function ReaderDirectorySearchInput({
  value,
  placeholder,
  onChange,
}: ReaderDirectorySearchInputProps) {
  return (
    <div className={cn("relative flex-1")}>
      <Search
        className={cn(
          "absolute top-1/2 left-4 h-4 w-4 -translate-y-1/2 text-[var(--text-tertiary)]",
        )}
      />
      <input
        id="reader-search"
        className={cn(
          "tn-field tn-field-accent tn-text-primary w-full rounded-xl px-4 py-3 pl-11 text-sm font-medium transition-all placeholder:text-[var(--text-tertiary)]",
        )}
        placeholder={placeholder}
        type="text"
        value={value}
        onChange={(event) => onChange(event.target.value)}
      />
    </div>
  );
}
