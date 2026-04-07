"use client";

import { BookOpen, Calendar, Search, User } from "lucide-react";
import type { FormEvent } from "react";
import { useTranslations } from "next-intl";
import { Button } from "@/shared/components/ui";
import { cn } from "@/lib/utils";
import { AdminReadingsFilterField } from "./AdminReadingsFilterField";
import { AdminReadingsSpreadSelect } from "./AdminReadingsSpreadSelect";

interface AdminReadingsFiltersProps {
 username: string;
 spreadType: string;
 startDate: string;
 endDate: string;
 onUsernameChange: (value: string) => void;
 onSpreadTypeChange: (value: string) => void;
 onStartDateChange: (value: string) => void;
 onEndDateChange: (value: string) => void;
 onSubmit: (event: FormEvent<HTMLFormElement>) => void;
}

export function AdminReadingsFilters({ username, spreadType, startDate, endDate, onUsernameChange, onSpreadTypeChange, onStartDateChange, onEndDateChange, onSubmit }: AdminReadingsFiltersProps) {
 const t = useTranslations("Admin");

 return (
  <form onSubmit={onSubmit} className={cn("p-6 rounded-[2.5rem] bg-[var(--purple-accent)]/5 border tn-border-soft shadow-inner flex flex-wrap items-end gap-6")}>
   <AdminReadingsFilterField label={t("readings.filters.username_label")} icon={User} containerClassName="flex-1 min-w-[240px]"><input type="text" value={username} onChange={(event) => onUsernameChange(event.target.value)} placeholder={t("readings.filters.username_placeholder")} className={cn("w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all placeholder:text-[var(--text-tertiary)] shadow-inner")} /></AdminReadingsFilterField>
   <AdminReadingsFilterField label={t("readings.filters.spread_label")} icon={BookOpen} containerClassName="w-56"><AdminReadingsSpreadSelect value={spreadType} onChange={onSpreadTypeChange} labels={{ all: t("readings.filters.spread_all"), daily: t("readings.filters.spread_daily"), spread3: t("readings.filters.spread_3"), spread5: t("readings.filters.spread_5"), spread10: t("readings.filters.spread_10") }} /></AdminReadingsFilterField>
   <AdminReadingsFilterField label={t("readings.filters.start_date_label")} icon={Calendar} containerClassName="w-44"><input type="date" value={startDate} onChange={(event) => onStartDateChange(event.target.value)} className={cn("w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all shadow-inner")} /></AdminReadingsFilterField>
   <AdminReadingsFilterField label={t("readings.filters.end_date_label")} icon={Calendar} containerClassName="w-44"><input type="date" value={endDate} onChange={(event) => onEndDateChange(event.target.value)} className={cn("w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all shadow-inner")} /></AdminReadingsFilterField>
   <Button type="submit" variant="primary" className={cn("px-8 py-4 shrink-0 shadow-md flex items-center justify-center min-w-[140px]")}><Search className={cn("w-4 h-4")} />{t("readings.filters.submit")}</Button>
  </form>
 );
}
