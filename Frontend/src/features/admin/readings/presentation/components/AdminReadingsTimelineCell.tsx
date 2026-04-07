import { useLocale } from "next-intl";
import { cn } from "@/lib/utils";

interface AdminReadingsTimelineCellProps {
 createdAt: string;
}

export function AdminReadingsTimelineCell({ createdAt }: AdminReadingsTimelineCellProps) {
 const locale = useLocale();

 return (
  <td className={cn("px-8 py-6 whitespace-nowrap")}>
   <div className={cn("flex flex-col text-left")}>
    <div className={cn("text-[10px] font-black text-[var(--text-secondary)] uppercase tracking-tighter italic")}>{new Date(createdAt).toLocaleDateString(locale)}</div>
    <div className={cn("text-[10px] font-bold text-[var(--text-tertiary)] italic")}>{new Date(createdAt).toLocaleTimeString(locale)}</div>
   </div>
  </td>
 );
}
