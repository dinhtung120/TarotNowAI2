import { cn } from "@/lib/utils";

interface CollectionZoomStatCardProps {
  label: string;
  value: number | string;
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
    <div className={cn("rounded-xl border p-2.5 text-center transition-all hover:shadow-md md:p-3.5", wrapperClassName)}>
      <span
        className={cn(
          "mb-0.5 block text-[9px] font-black tracking-widest uppercase md:text-[10px]",
          wrapperClassName.includes("red-500") && "text-red-500/70",
          wrapperClassName.includes("blue-500") && "text-blue-500/70",
          !wrapperClassName.includes("red-500") &&
            !wrapperClassName.includes("blue-500") &&
            "tn-text-muted",
        )}
      >
        {label}
      </span>
      <span className={cn("tn-text-1-2-md font-black italic", valueClassName)}>
        {value}
      </span>
    </div>
  );
}
