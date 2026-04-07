import { cn } from "@/lib/utils";

export default function FeaturedReadersFallback() {
  return (
    <div className={cn("grid grid-cols-1 gap-8 sm:grid-cols-2 lg:grid-cols-4")}>
      {Array.from({ length: 4 }).map((_, index) => (
        <div
          key={`featured-reader-skeleton-${index}`}
          className={cn(
            "h-96 animate-pulse rounded-[2.5rem] border border-[var(--border-default)] bg-[var(--bg-surface)]",
          )}
        />
      ))}
    </div>
  );
}
