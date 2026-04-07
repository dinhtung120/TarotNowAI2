import { cn } from "@/lib/utils";

interface ReaderDirectoryCardSpecialtiesProps {
  specialties: string[];
}

export default function ReaderDirectoryCardSpecialties({
  specialties,
}: ReaderDirectoryCardSpecialtiesProps) {
  if (specialties.length === 0) {
    return null;
  }

  return (
    <div className={cn("flex flex-wrap gap-1.5")}>
      {specialties.slice(0, 3).map((specialty) => (
        <span
          key={specialty}
          className={cn(
            "tn-border tn-surface rounded-md border px-2 py-1 text-[9px] font-black tracking-wider text-[var(--text-secondary)] uppercase transition-colors group-hover:border-[var(--text-secondary)]/30",
          )}
        >
          {specialty}
        </span>
      ))}
      {specialties.length > 3 && (
        <span
          className={cn(
            "tn-border-soft tn-surface flex items-center justify-center rounded-md border px-2 py-1 text-[9px] font-black text-[var(--text-tertiary)]",
          )}
        >
          +{specialties.length - 3}
        </span>
      )}
    </div>
  );
}
