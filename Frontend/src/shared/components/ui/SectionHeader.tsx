import type { ReactNode } from "react";
import SectionHeaderTextBlock from "@/shared/components/ui/section-header/SectionHeaderTextBlock";
import { cn } from "@/lib/utils";

interface SectionHeaderProps {
  action?: ReactNode;
  className?: string;
  size?: "sm" | "md" | "lg";
  subtitle?: string;
  tag?: string;
  tagIcon?: ReactNode;
  title: string;
  titleMuted?: string;
}

const SIZE_CLASS_NAME: Record<
  NonNullable<SectionHeaderProps["size"]>,
  string
> = {
  sm: "text-xl md:text-2xl",
  md: "text-2xl md:text-4xl",
  lg: "text-3xl md:text-5xl",
};

export default function SectionHeader({
  action,
  className,
  size = "lg",
  subtitle,
  tag,
  tagIcon,
  title,
  titleMuted,
}: SectionHeaderProps) {
  return (
    <div
      className={cn(
        "flex flex-col justify-between gap-4 md:flex-row md:items-end md:gap-6",
        className,
      )}
    >
      <SectionHeaderTextBlock
        sizeClassName={SIZE_CLASS_NAME[size]}
        subtitle={subtitle}
        tag={tag}
        tagIcon={tagIcon}
        title={title}
        titleMuted={titleMuted}
      />
      {action ? (
        <div className={cn("w-full md:w-auto md:flex-shrink-0")}>{action}</div>
      ) : null}
    </div>
  );
}
