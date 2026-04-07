import { CheckCircle2, Clock } from "lucide-react";
import { useTranslations } from "next-intl";
import { cn } from "@/lib/utils";

interface AdminReadingsStatusBadgeProps {
 isCompleted: boolean;
}

export function AdminReadingsStatusBadge({ isCompleted }: AdminReadingsStatusBadgeProps) {
 const t = useTranslations("Admin");
 const statusClass = isCompleted
  ? "tn-bg-success-10 tn-border-success-30 tn-text-success shadow-md"
  : "tn-bg-accent-10 tn-border-accent-30 tn-text-accent";

 return (
  <div className={cn("inline-flex items-center gap-2 px-3 py-1.5 rounded-xl tn-text-9 font-black uppercase tracking-widest border transition-all shadow-inner", statusClass)}>
   {isCompleted ? <CheckCircle2 className={cn("w-3 h-3")} /> : <Clock className={cn("w-3 h-3")} />}
   {isCompleted ? t("readings.status.completed") : t("readings.status.processing")}
  </div>
 );
}
