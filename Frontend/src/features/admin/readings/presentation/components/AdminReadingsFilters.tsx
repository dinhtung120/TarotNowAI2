"use client";

import { useEffect } from "react";
import { zodResolver } from "@hookform/resolvers/zod";
import { BookOpen, Calendar, Search, User } from "lucide-react";
import { useTranslations } from "next-intl";
import { useForm, useWatch } from "react-hook-form";
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
 onSubmit: () => void;
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
 const { handleSubmit, setValue, control } = useForm<AdminReadingsFiltersFormValues>({
  resolver: zodResolver(adminReadingsFiltersSchema),
  defaultValues: {
   username,
   spreadType,
   startDate,
   endDate,
  },
 });

 const watchedUsername = useWatch({ control, name: "username" }) ?? "";
 const watchedSpreadType = useWatch({ control, name: "spreadType" }) ?? "";
 const watchedStartDate = useWatch({ control, name: "startDate" }) ?? "";
 const watchedEndDate = useWatch({ control, name: "endDate" }) ?? "";

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
  onSubmit();
 });

 return (
  <form onSubmit={submitWithValidation} className={cn("flex flex-wrap items-end gap-4 border p-5 shadow-inner tn-rounded-2_5xl tn-bg-accent-5 tn-border-soft sm:gap-6 sm:p-6")}>
   <AdminReadingsFilterField label={t("readings.filters.username_label")} icon={User} containerClassName="w-full md:flex-1 md:tn-minw-240"><input type="text" value={watchedUsername} onChange={(event) => setValue("username", event.target.value, { shouldDirty: true, shouldValidate: true })} placeholder={t("readings.filters.username_placeholder")} className={cn("w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all tn-placeholder shadow-inner")} /></AdminReadingsFilterField>
   <AdminReadingsFilterField label={t("readings.filters.spread_label")} icon={BookOpen} containerClassName="w-full sm:w-56"><AdminReadingsSpreadSelect value={watchedSpreadType} onChange={(value) => setValue("spreadType", value, { shouldDirty: true, shouldValidate: true })} labels={{ all: t("readings.filters.spread_all"), daily: t("readings.filters.spread_daily"), spread3: t("readings.filters.spread_3"), spread5: t("readings.filters.spread_5"), spread10: t("readings.filters.spread_10") }} /></AdminReadingsFilterField>
   <AdminReadingsFilterField label={t("readings.filters.start_date_label")} icon={Calendar} containerClassName="w-full sm:w-44"><input type="date" value={watchedStartDate} onChange={(event) => setValue("startDate", event.target.value, { shouldDirty: true, shouldValidate: true })} className={cn("w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all shadow-inner")} /></AdminReadingsFilterField>
   <AdminReadingsFilterField label={t("readings.filters.end_date_label")} icon={Calendar} containerClassName="w-full sm:w-44"><input type="date" value={watchedEndDate} onChange={(event) => setValue("endDate", event.target.value, { shouldDirty: true, shouldValidate: true })} className={cn("w-full tn-field rounded-2xl px-5 py-4 text-xs font-black tn-text-primary tn-field-accent transition-all shadow-inner")} /></AdminReadingsFilterField>
   <Button type="submit" variant="primary" className={cn("flex w-full items-center justify-center px-8 py-4 shadow-md sm:w-auto sm:min-w-[140px]")}><Search className={cn("w-4 h-4")} />{t("readings.filters.submit")}</Button>
  </form>
 );
}
