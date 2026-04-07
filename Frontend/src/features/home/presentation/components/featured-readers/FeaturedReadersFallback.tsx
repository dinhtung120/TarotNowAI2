import { cn } from "@/lib/utils";

export default function FeaturedReadersFallback() {
  return (
    <div className={cn("tn-grid-1-2-4-responsive gap-8")}>
      {Array.from({ length: 4 }).map((_, index) => (
        <div
          key={`featured-reader-skeleton-${index}`}
          className={cn(
            "h-96 animate-pulse tn-rounded-2_5xl border tn-border tn-surface",
          )}
        />
      ))}
    </div>
  );
}
