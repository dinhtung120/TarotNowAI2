

import { memo, type ReactNode } from "react";
import { Ghost } from "lucide-react";
import { cn } from "@/lib/utils";

interface EmptyStateProps {
 
 icon?: ReactNode;

 
 title?: string;

 
 message?: string;

 
 action?: ReactNode;

 className?: string;
}

function EmptyStateComponent({
 icon,
 title = "",
 message,
 action,
 className,
}: EmptyStateProps) {
 return (
 <div
 className={cn(
 "flex flex-col items-center justify-center text-center py-20 px-6",
 className,
 )}
 >
 {}
 <div className={cn("mb-6 tn-text-muted")}>
 {icon || <Ghost className={cn("w-16 h-16")} />}
 </div>

 {}
 {title && (
 <h3 className={cn("text-sm font-black uppercase tracking-widest tn-text-muted mb-2")}>
 {title}
 </h3>
 )}

 {}
 {message && (
 <p className={cn("text-xs tn-text-muted font-medium max-w-sm leading-relaxed mb-6")}>
 {message}
 </p>
 )}

 {}
 {action}
 </div>
 );
}

const EmptyState = memo(EmptyStateComponent);
EmptyState.displayName = "EmptyState";

export default EmptyState;
