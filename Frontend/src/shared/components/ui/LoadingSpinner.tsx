

import { Loader2 } from "lucide-react";
import { memo } from "react";
import { cn } from "@/lib/utils";

type SpinnerSize = "sm" | "md" | "lg";

interface LoadingSpinnerProps {
 
 size?: SpinnerSize;

 
 message?: string;

 
 fullPage?: boolean;

 className?: string;
}

const sizeMap: Record<SpinnerSize, string> = {
 sm: "w-4 h-4",
 md: "w-6 h-6",
 lg: "w-10 h-10",
};

function LoadingSpinnerComponent({
 size = "md",
 message,
 fullPage = false,
 className,
}: LoadingSpinnerProps) {
 return (
 <div
 className={cn(
 "flex flex-col items-center justify-center gap-4",
 fullPage ? "tn-min-h-60vh" : "py-12",
 className,
 )}
 >
 {}
 <Loader2
 className={cn(sizeMap[size], "animate-spin", "tn-text-accent")}
 />

 {}
 {message && (
 <span className={cn("tn-text-10 font-black uppercase tracking-widest tn-text-muted")}>
 {message}
 </span>
 )}
 </div>
 );
}

const LoadingSpinner = memo(LoadingSpinnerComponent);
LoadingSpinner.displayName = "LoadingSpinner";

export default LoadingSpinner;
