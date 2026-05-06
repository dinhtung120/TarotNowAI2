import { Sparkles } from "lucide-react";
import type { ShufflePath } from "@/features/reading/session/components/types";
import { cn } from "@/lib/utils";

interface ShuffleCardProps {
  path: ShufflePath;
}

function resolveShufflePathClass(path: ShufflePath): string {
  const normalizedIndex = Number.isFinite(path.z) ? Math.max(0, Math.min(Math.round(path.z), 8)) : 0;
  return `tn-shuffle-path-${normalizedIndex}`;
}

export default function ShuffleCard({ path }: ShuffleCardProps) {
  return (
    <div
      className={cn(
        "tarot-shuffling-card absolute inset-0 rounded-xl border-2 border-[var(--purple-accent)]/30 bg-gradient-to-br from-[var(--purple-accent)] via-[var(--purple-accent)] to-[color:var(--c-61-49-80-55)] shadow-[0_10px_30px_var(--c-168-85-247-20)] motion-reduce:animate-none",
        resolveShufflePathClass(path),
      )}
    >
      <div
        className={cn(
          "tn-starfield absolute inset-0 z-0 flex h-full w-full items-center justify-center rounded-xl opacity-50",
        )}
      />
      <div
        className={cn("absolute inset-0 z-10 flex items-center justify-center")}
      >
        <div
          className={cn(
            "tn-overlay-soft flex h-24 w-16 items-center justify-center rounded border border-[var(--purple-accent)]/30",
          )}
        >
          <Sparkles className={cn("h-6 w-6 tn-text-accent-60")} />
        </div>
      </div>
      <div
        className={cn(
          "pointer-events-none absolute inset-2 rounded-lg border border-[var(--purple-accent)]/20",
        )}
      />
    </div>
  );
}
