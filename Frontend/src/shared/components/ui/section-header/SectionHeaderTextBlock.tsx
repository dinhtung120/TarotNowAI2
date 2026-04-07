import type { ReactNode } from "react";
import { cn } from "@/lib/utils";

interface SectionHeaderTextBlockProps {
  sizeClassName: string;
  subtitle?: string;
  tag?: string;
  tagIcon?: ReactNode;
  title: string;
  titleMuted?: string;
}

export default function SectionHeaderTextBlock({
  sizeClassName,
  subtitle,
  tag,
  tagIcon,
  title,
  titleMuted,
}: SectionHeaderTextBlockProps) {
  return (
    <div className={cn("max-w-2xl space-y-3")}>
      {tag ? (
        <div
          className={cn(
            "inline-flex items-center gap-2 text-[10px] font-black tracking-[0.25em] text-[var(--purple-accent)] uppercase sm:tracking-[0.3em]",
          )}
        >
          {tagIcon}
          {tag}
        </div>
      ) : null}
      <h2
        className={cn(
          sizeClassName,
          "lunar-metallic-text leading-tight font-black tracking-tighter text-balance uppercase italic",
        )}
      >
        {title}
        {titleMuted ? (
          <span className={cn("tn-text-secondary")}>
            {" "}
            {titleMuted}
          </span>
        ) : null}
      </h2>
      {subtitle ? (
        <p
          className={cn(
            "max-w-lg text-sm leading-relaxed font-medium tn-text-secondary",
          )}
        >
          {subtitle}
        </p>
      ) : null}
    </div>
  );
}
