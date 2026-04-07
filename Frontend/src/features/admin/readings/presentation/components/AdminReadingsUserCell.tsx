import { Hash, User } from "lucide-react";
import { useTranslations } from "next-intl";
import { cn } from "@/lib/utils";

interface AdminReadingsUserCellProps {
 username?: string;
 userId: string;
}

export function AdminReadingsUserCell({ username, userId }: AdminReadingsUserCellProps) {
 const t = useTranslations("Admin");

 return (
  <td className={cn("px-8 py-6")}>
   <div className={cn("flex items-center gap-4")}>
    <div className={cn("w-10 h-10 rounded-[1rem] tn-panel-overlay flex items-center justify-center shadow-inner group-hover/row:scale-110 transition-transform")}>
     <User className={cn("w-4 h-4 text-[var(--text-secondary)]")} />
    </div>
    <div>
     <div className={cn("text-[11px] font-black tn-text-primary uppercase tracking-tighter drop-shadow-sm")}>{username || t("readings.row.user_unknown")}</div>
     <div className={cn("text-[9px] font-bold text-[var(--text-tertiary)] uppercase tracking-tighter flex items-center gap-1 mt-0.5")}><Hash className={cn("w-2.5 h-2.5 opacity-50")} />{userId.split("-")[0]}...</div>
    </div>
   </div>
  </td>
 );
}
