"use client";

import { BookOpen } from "lucide-react";
import type { AdminReading } from "@/features/admin/readings/hooks/useAdminReadings";
import { useTranslations } from "next-intl";
import { cn } from "@/lib/utils";
import { AdminReadingsStatusBadge } from "./AdminReadingsStatusBadge";
import { AdminReadingsTimelineCell } from "./AdminReadingsTimelineCell";
import { AdminReadingsUserCell } from "./AdminReadingsUserCell";

interface AdminReadingsTableRowProps {
 reading: AdminReading;
 getSpreadLabel: (type: string) => string;
}

export function AdminReadingsTableRow({ reading, getSpreadLabel }: AdminReadingsTableRowProps) {
 const t = useTranslations("Admin");

 return (
  <tr className={cn("group/row tn-row-hover-surface transition-colors")}>
   <AdminReadingsTimelineCell createdAt={reading.createdAt} />
   <AdminReadingsUserCell username={reading.username} userId={reading.userId} />
   <td className={cn("px-8 py-6")}><div className={cn("inline-flex items-center gap-2 px-3 py-1.5 rounded-lg tn-panel-soft tn-text-10 font-black tn-text-primary uppercase tracking-widest italic text-left shadow-inner")}><BookOpen className={cn("w-3.5 h-3.5 tn-text-accent")} />{getSpreadLabel(reading.spreadType)}</div></td>
   <td className={cn("tn-maxw-200 px-8 py-6 truncate text-left")}><p className={cn("tn-text-11 font-bold tn-text-secondary italic uppercase leading-relaxed tracking-tight text-left")}>{reading.question || t("readings.row.question_empty")}</p></td>
   <td className={cn("px-8 py-6 text-center")}><AdminReadingsStatusBadge isCompleted={reading.isCompleted} /></td>
  </tr>
 );
}
