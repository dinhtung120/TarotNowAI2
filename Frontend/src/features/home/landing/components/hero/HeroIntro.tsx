import { cn } from "@/lib/utils";
import type { HeroIntroProps } from "@/features/home/landing/components/hero/types";

export default function HeroIntro({
  heroDesc,
  heroTitle1,
  heroTitle2,
  tagline,
}: HeroIntroProps) {
  return (
    <>
      <div
        className={cn(
          "animate-in fade-in slide-in-from-bottom-4 mb-10 inline-flex items-center gap-3 rounded-full border border-[var(--border-default)] bg-[var(--bg-glass)] px-4 py-2 text-[10px] font-black tracking-[0.3em] text-[color:var(--c-hex-9f8338)] uppercase shadow-[var(--shadow-card)] duration-1000",
        )}
      >
        <span className={cn("relative flex h-2 w-2")}>
          <span
            className={cn(
              "absolute inline-flex h-full w-full animate-ping rounded-full bg-[var(--amber-accent)] opacity-75",
            )}
          />
          <span
            className={cn(
              "relative inline-flex h-2 w-2 rounded-full bg-[var(--amber-accent)]",
            )}
          />
        </span>
        {tagline}
      </div>
      <h1
        className={cn(
          "animate-in fade-in zoom-in-95 relative mb-10 delay-200 duration-1000",
        )}
      >
        <span
          className={cn(
            "mb-2 block text-3xl leading-tight font-light tracking-tight text-[var(--text-secondary)] italic sm:text-5xl lg:text-6xl",
          )}
        >
          {heroTitle1}
        </span>
        <span
          className={cn(
            "block bg-gradient-to-b from-[color:var(--c-hex-8f7bb4)] via-[var(--purple-accent)] to-[var(--mint-accent)] bg-clip-text pb-4 text-5xl leading-none font-black tracking-tighter text-transparent italic drop-shadow-[0_20px_40px_var(--c-168-156-255-30)] sm:text-7xl lg:text-8xl",
          )}
        >
          {heroTitle2}
        </span>
        <div
          className={cn(
            "absolute -bottom-2 left-1/2 h-[1px] w-40 -translate-x-1/2 bg-gradient-to-r from-transparent via-[color:var(--c-168-156-255-50)] to-transparent",
          )}
        />
      </h1>
      <p
        className={cn(
          "animate-in fade-in slide-in-from-bottom-8 mb-14 max-w-2xl text-base leading-relaxed font-medium tracking-wide text-[var(--text-secondary)] delay-500 duration-1000 sm:text-lg",
        )}
      >
        {heroDesc}
      </p>
    </>
  );
}
