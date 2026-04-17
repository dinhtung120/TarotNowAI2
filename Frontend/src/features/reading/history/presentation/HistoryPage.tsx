"use client";

import { useCallback } from "react";
import { useLocale, useTranslations } from "next-intl";
import { cn } from "@/lib/utils";
import { useHistorySessionsPage } from "@/features/reading/history/application/useHistorySessionsPage";
import { useOptimizedNavigation } from "@/shared/infrastructure/navigation/useOptimizedNavigation";
import { useAuthStore } from "@/store/authStore";
import { HistoryEmptyState, HistoryErrorBanner, HistoryLoadingGrid, HistoryPaginationDock, HistoryPageHeader, HistorySessionsGrid } from "./components";
import { resolveSpreadName } from "./spreadLabels";

export default function HistoryPage() {
 const navigation = useOptimizedNavigation();
 const t = useTranslations("History");
 const tApi = useTranslations("ApiErrors");
 const locale = useLocale();
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const handleUnauthorized = useCallback(() => {
  navigation.push("/login");
 }, [navigation]);

 const state = useHistorySessionsPage({
  isAuthenticated,
  networkErrorMessage: tApi("network_error"),
  onUnauthorized: handleUnauthorized,
 });

 const hasItems = !state.isLoading && Boolean(state.historyData?.items.length);
 const isEmpty = !state.isLoading && state.historyData?.items.length === 0;

 return (
  <div className={cn("max-w-5xl", "mx-auto", "tn-page-x", "pt-8", "pb-32", "font-sans")}>
   <HistoryPageHeader title={t("title")} subtitle={t("subtitle")} tag={t("tag")} filterType={state.filterType} filterDate={state.filterDate} labels={{ all: t("all_types"), daily: t("spread_daily"), spread3: t("spread_3"), spread5: t("spread_5"), spread10: t("spread_10") }} onFilterTypeChange={state.setFilterType} onFilterDateChange={state.setFilterDate} />
   {state.error ? <HistoryErrorBanner message={state.error} /> : null}
   {state.isLoading ? <HistoryLoadingGrid /> : null}
   {isEmpty ? <HistoryEmptyState title={t("empty_title")} description={t("empty_desc")} cta={t("cta_draw")} /> : null}
   {hasItems ? (
    <HistorySessionsGrid
     locale={locale}
     items={state.historyData?.items ?? []}
     getSpreadName={(spreadType) => resolveSpreadName(spreadType, (labelKey) => t(labelKey as "spread_daily"))}
     completedLabel={t("status_completed")}
     interruptedLabel={t("status_interrupted")}
    />
   ) : null}
   {state.historyData && state.historyData.totalPages > 1 ? (
    <HistoryPaginationDock
     currentPage={state.currentPage}
     totalPages={state.historyData.totalPages}
     prevLabel={t("prev_page")}
     nextLabel={t("next_page")}
     pageInfo={(current, total) => t("page_info", { current, total })}
     onPrev={state.goToPrevPage}
     onNext={state.goToNextPage}
    />
   ) : null}
  </div>
 );
}
