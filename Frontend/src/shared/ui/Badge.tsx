

import { memo, type ReactNode } from "react";
import { cn } from "@/lib/utils";

type BadgeVariant =
 | "default" 
 | "purple" 
 | "amber" 
 | "success" 
 | "error" 
 | "warning" 
 | "info"; 

type BadgeSize = "sm" | "md";

interface BadgeProps {
 variant?: BadgeVariant;
 size?: BadgeSize;
 
 children: ReactNode;
 className?: string;
}

const variantStyles: Record<BadgeVariant, string> = {
 default: "tn-bg-elevated tn-text-secondary tn-border",
 purple: "tn-bg-purple-100 tn-text-accent-soft tn-border",
 amber: "tn-bg-warning-10 tn-text-warning tn-border-warning-30",
 success: "tn-bg-success-soft tn-text-success tn-border-success-30",
 error: "tn-bg-danger-soft tn-text-danger tn-border-danger-50",
 warning: "tn-bg-warning-soft tn-text-warning tn-border-warning-30",
 info: "tn-bg-info-soft tn-text-info tn-border-info-20",
};

const sizeStyles: Record<BadgeSize, string> = {
 sm: "px-2 py-0.5 tn-text-9",
 md: "px-3 py-1 tn-text-10",
};

function BadgeComponent({
 variant = "default",
 size = "sm",
 children,
 className,
}: BadgeProps) {
 return (
 <span
 className={cn(
 "inline-flex items-center gap-1",
 "font-black uppercase tracking-wider",
 "border rounded-full",
 "whitespace-nowrap",
 variantStyles[variant],
 sizeStyles[size],
 className,
 )}
 >
 {children}
 </span>
 );
}

const Badge = memo(BadgeComponent);
Badge.displayName = "Badge";

export default Badge;
