import { cn } from "@/lib/utils";

interface CollectionZoomStatCardProps {
  label: string;
  value: number;
  valueClassName: string;
  wrapperClassName: string;
}

export default function CollectionZoomStatCard({
  label,
  value,
  valueClassName,
  wrapperClassName,
}: CollectionZoomStatCardProps) {
  return (
    <div className={cn("rounded-2xl border p-4 text-center", wrapperClassName)}>
      <span
        className={cn(
          "mb-1 block text-[10px] font-black tracking-widest uppercase md:text-xs",
          wrapperClassName.includes("red-500") && "text-red-500/70",
          wrapperClassName.includes("blue-500") && "text-blue-500/70",
          !wrapperClassName.includes("red-500") &&
            !wrapperClassName.includes("blue-500") &&
            "tn-text-muted",
        )}
      >
        {label}
      </span>
      <span className={cn("tn-text-2-3-md font-black", valueClassName)}>
        {value}
      </span>
    </div>
  );
}
