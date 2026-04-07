import { cn } from "@/lib/utils";

interface HeroScrollIndicatorProps {
  label: string;
}

export default function HeroScrollIndicator({
  label,
}: HeroScrollIndicatorProps) {
  return (
    <div
      className={cn(
        "pointer-events-none absolute bottom-4 left-1/2 z-0 hidden -translate-x-1/2 flex-col items-center gap-4 opacity-50 md:flex",
      )}
    >
      <div
        className={cn(
          "origin-left translate-x-1.5 translate-y-2 rotate-90 text-[9px] font-black tracking-[0.4em] text-[var(--text-muted)] uppercase",
        )}
      >
        {label}
      </div>
      <div
        className={cn(
          "h-16 w-[1px] bg-gradient-to-b from-[var(--purple-accent)] to-transparent",
        )}
      />
    </div>
  );
}
