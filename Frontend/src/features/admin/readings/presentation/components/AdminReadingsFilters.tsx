"use client";

import { useEffect } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { BookOpen, Calendar, Search, User } from "lucide-react";
import type { FormEvent } from "react";
import { useTranslations } from "next-intl";
import { useForm } from "react-hook-form";
import { z } from "zod";
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

const adminReadingsFiltersSchema = z
 .object({
  username: z.string().trim().max(100),
  spreadType: z.string().max(50),
  startDate: z.string().max(50),
  endDate: z.string().max(50),
 })
 .refine(
  (values) => {
   if (!values.startDate || !values.endDate) return true;
   return values.endDate >= values.startDate;
  },
  {
   path: ["endDate"],
   message: "End date must be greater than or equal to start date",
  },
 );

type AdminReadingsFiltersFormValues = z.infer<typeof adminReadingsFiltersSchema>;

export function AdminReadingsFilters({ username, spreadType, startDate, endDate, onUsernameChange, onSpreadTypeChange, onStartDateChange, onEndDateChange, onSubmit }: AdminReadingsFiltersProps) {
 const t = useTranslations("Admin");
 const { handleSubmit, setValue, watch } = useForm<AdminReadingsFiltersFormValues>({
  resolver: zodResolver(adminReadingsFiltersSchema),
  defaultValues: {
   username,
   spreadType,
   startDate,
   endDate,
  },
 });

 const watchedUsername = watch("username") ?? "";
 const watchedSpreadType = watch("spreadType") ?? "";
 const watchedStartDate = watch("startDate") ?? "";
 const watchedEndDate = watch("endDate") ?? "";

 useEffect(() => {
  setValue("username", username, { shouldDirty: false, shouldValidate: false });
 }, [setValue, username]);

 useEffect(() => {
  setValue("spreadType", spreadType, { shouldDirty: false, shouldValidate: false });
 }, [setValue, spreadType]);

 useEffect(() => {
  setValue("startDate", startDate, { shouldDirty: false, shouldValidate: false });
 }, [setValue, startDate]);

 useEffect(() => {
  setValue("endDate", endDate, { shouldDirty: false, shouldValidate: false });
 }, [endDate, setValue]);

 useEffect(() => {
  onUsernameChange(watchedUsername);
 }, [onUsernameChange, watchedUsername]);

 useEffect(() => {
  onSpreadTypeChange(watchedSpreadType);
 }, [onSpreadTypeChange, watchedSpreadType]);

 useEffect(() => {
  onStartDateChange(watchedStartDate);
 }, [onStartDateChange, watchedStartDate]);

 useEffect(() => {
  onEndDateChange(watchedEndDate);
 }, [onEndDateChange, watchedEndDate]);

 const submitWithValidation = handleSubmit(() => {
  onSubmit({
   preventDefault: () => undefined,
   stopPropagation: () => undefined,
  } as unknown as FormEvent<HTMLFormElement>);
 });

 return (
  <form onSubmit={submitWithValidation} className={cn("p-6 rounded-[2.5rem] bg-[var(--purple-accent)]/5 border tn-border-soft shadow-inner flex flex-wrap items-end gap-6")}>
   <AdminReadingsFilterField label={t("readings.filters.username_label")} icon={User} containerClassName="flex-1 min-w-[240px]"><input type="text" value={watchedUsername} onChange={(event) => setValue("username", event.target.value, { shouldDirty: true, shouldValidate: true })} placeholder={t("readings.filters.username_placeholder")} className={cn("w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all placeholder:text-[var(--text-tertiary)] shadow-inner")} /></AdminReadingsFilterField>
   <AdminReadingsFilterField label={t("readings.filters.spread_label")} icon={BookOpen} containerClassName="w-56"><AdminReadingsSpreadSelect value={watchedSpreadType} onChange={(value) => setValue("spreadType", value, { shouldDirty: true, shouldValidate: true })} labels={{ all: t("readings.filters.spread_all"), daily: t("readings.filters.spread_daily"), spread3: t("readings.filters.spread_3"), spread5: t("readings.filters.spread_5"), spread10: t("readings.filters.spread_10") }} /></AdminReadingsFilterField>
   <AdminReadingsFilterField label={t("readings.filters.start_date_label")} icon={Calendar} containerClassName="w-44"><input type="date" value={watchedStartDate} onChange={(event) => setValue("startDate", event.target.value, { shouldDirty: true, shouldValidate: true })} className={cn("w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all shadow-inner")} /></AdminReadingsFilterField>
   <AdminReadingsFilterField label={t("readings.filters.end_date_label")} icon={Calendar} containerClassName="w-44"><input type="date" value={watchedEndDate} onChange={(event) => setValue("endDate", event.target.value, { shouldDirty: true, shouldValidate: true })} className={cn("w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all shadow-inner")} /></AdminReadingsFilterField>
   <Button type="submit" variant="primary" className={cn("px-8 py-4 shrink-0 shadow-md flex items-center justify-center min-w-[140px]")}><Search className={cn("w-4 h-4")} />{t("readings.filters.submit")}</Button>
  </form>
 );
}
