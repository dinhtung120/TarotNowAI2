import { CheckCircle2, Clock } from "lucide-react";
import { useTranslations } from "next-intl";
import { cn } from "@/lib/utils";

interface AdminReadingsStatusBadgeProps {
 isCompleted: boolean;
}

export function AdminReadingsStatusBadge({ isCompleted }: AdminReadingsStatusBadgeProps) {
 const t = useTranslations("Admin");
 const className = isCompleted
  ? "bg-[var(--success)]/10 border-[var(--success)]/30 text-[var(--success)] shadow-md"
  : "bg-[var(--accent)]/10 border-[var(--accent)]/30 text-[var(--accent)]";

 return (
  <div className={cn("inline-flex items-center gap-2 px-3 py-1.5 rounded-xl text-[9px] font-black uppercase tracking-widest border transition-all shadow-inner", className)}>
   {isCompleted ? <CheckCircle2 className={cn("w-3 h-3")} /> : <Clock className={cn("w-3 h-3")} />}
   {isCompleted ? t("readings.status.completed") : t("readings.status.processing")}
  </div>
 );
}
