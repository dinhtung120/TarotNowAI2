import { RefreshCw } from "lucide-react";
import type { ShufflePath } from "@/features/reading/session/presentation/session-page/types";
import ShuffleCard from "@/features/reading/session/presentation/session-page/ShuffleCard";
import { cn } from "@/lib/utils";

interface ShuffleIntroProps {
  shufflePaths: ShufflePath[];
  title: string;
  subtitle: string;
}

export default function ShuffleIntro({
  shufflePaths,
  title,
  subtitle,
}: ShuffleIntroProps) {
  return (
    <div
      className={cn(
        "animate-in zoom-in fade-in mt-12 flex flex-col items-center justify-center duration-1000",
      )}
    >
      <div
        className={cn(
          "perspective-1000 relative mt-10 mb-20 h-56 w-36 will-change-transform",
        )}
      >
        {shufflePaths.map((path) => (
          <ShuffleCard key={`shuffle-card-${path.z}`} path={path} />
        ))}
      </div>

      <h2
        className={cn(
          "animate-pulse bg-gradient-to-r from-[var(--purple-accent)] via-[var(--danger)] to-[var(--warning)] bg-clip-text font-serif text-2xl font-medium text-transparent drop-shadow-sm",
        )}
      >
        {title}
      </h2>
      <p className={cn("tn-text-secondary mt-3 flex items-center text-sm")}>
        <RefreshCw
          className={cn(
            "mr-2 h-4 w-4 animate-spin text-[var(--purple-accent)]",
          )}
        />
        {subtitle}
      </p>
    </div>
  );
}
