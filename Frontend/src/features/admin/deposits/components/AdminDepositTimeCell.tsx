import { cn } from "@/lib/utils";
import { formatDate, formatTime } from "@/shared/utils/format/formatDateTime";

interface AdminDepositTimeCellProps {
 locale: string;
 createdAt: string;
}

export function AdminDepositTimeCell({ locale, createdAt }: AdminDepositTimeCellProps) {
 return (
  <td className={cn("px-8 py-5")}>
   <div className={cn("flex flex-col text-left")}>
    <div className={cn("tn-text-10 font-black tn-text-secondary uppercase tracking-tighter")}>
     {formatDate(createdAt, locale)}
    </div>
    <div className={cn("tn-text-10 font-bold tn-text-tertiary italic")}>{formatTime(createdAt, locale)}</div>
   </div>
  </td>
 );
}
