import type { LucideIcon } from "lucide-react";
import type { ReactNode } from "react";
import { cn } from "@/lib/utils";

interface AdminReadingsFilterFieldProps {
 label: string;
 icon: LucideIcon;
 containerClassName: string;
 children: ReactNode;
}

export function AdminReadingsFilterField({ label, icon: Icon, containerClassName, children }: AdminReadingsFilterFieldProps) {
 return (
  <div className={cn(containerClassName, "space-y-3 text-left")}>
   <label className={cn("text-[10px] font-black uppercase tracking-widest text-[var(--text-secondary)] flex items-center gap-2")}>
    <Icon className={cn("w-3.5 h-3.5")} />
    {label}
   </label>
   {children}
  </div>
 );
}
