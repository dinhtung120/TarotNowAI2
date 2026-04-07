

import { cn } from "@/lib/utils";

type SkeletonType = "text" | "card" | "avatar" | "row";

interface SkeletonLoaderProps {
 
 type?: SkeletonType;

 
 count?: number;

 
 className?: string;
}

const typeStyles: Record<SkeletonType, string> = {
 text: "h-4 rounded-lg",
 card: "h-48 rounded-3xl",
 avatar: "w-10 h-10 rounded-full",
 row: "h-16 rounded-2xl",
};

const randomTextWidth = (index: number): string => {
 const widths = ["w-full", "w-5/6", "w-4/5", "w-3/4", "w-2/3"];
 return widths[index % widths.length];
};

export default function SkeletonLoader({
 type = "text",
 count = 1,
 className,
}: SkeletonLoaderProps) {
 return (
 <div className={cn("space-y-3", className)}>
 {Array.from({ length: count }).map((_, i) => (
 <div
 key={`skeleton-${type}-${i}`}
 className={cn(
 
 typeStyles[type],
 
 type === "text" ? randomTextWidth(i) : "w-full",
 
 "animate-shimmer",
 
 "tn-surface",
 )}
 />
 ))}
 </div>
 );
}
