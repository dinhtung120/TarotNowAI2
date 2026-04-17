"use client";

import { useCallback } from "react";
import { useParams } from "next/navigation";
import { useLocale, useTranslations } from "next-intl";
import { cn } from "@/lib/utils";
import { useHistoryDetailPage } from "@/features/reading/history/application/useHistoryDetailPage";
import { useOptimizedNavigation } from "@/shared/infrastructure/navigation/useOptimizedNavigation";
import { useAuthStore } from "@/store/authStore";
import { HistoryDetailAiSummary, HistoryDetailCardsGrid, HistoryDetailHeader, HistoryDetailStates } from "./components/detail";
import { resolveSpreadName } from "./spreadLabels";

export default function HistoryDetailPage() {
 const params = useParams();
 const navigation = useOptimizedNavigation();
 const t = useTranslations("History");
 const tApi = useTranslations("ApiErrors");
 const locale = useLocale();
 const sessionId = String(params.id ?? "");
 const isAuthenticated = useAuthStore((state) => state.isAuthenticated);
 const handleUnauthorized = useCallback(() => {
  navigation.push("/login");
 }, [navigation]);

 const state = useHistoryDetailPage({
  isAuthenticated,
  sessionId,
  networkErrorMessage: tApi("network_error"),
  onUnauthorized: handleUnauthorized,
 });

 const detail = state.detail;
 const showContent = !state.isLoading && !state.error && detail !== null;

 return (
  <div className={cn("tn-maxw-100rem", "mx-auto", "tn-page-x", "pt-8", "pb-32", "font-sans", "relative")}>
   <HistoryDetailHeader locale={locale} sessionId={sessionId} detail={detail} spreadName={resolveSpreadName(detail?.spreadType ?? "", (labelKey) => t(labelKey as "spread_daily"))} labels={{ back: t("prev_page"), statusCompleted: t("status_completed"), statusInterrupted: t("status_interrupted"), detailFragment: (id) => t("detail_fragment", { id }) }} />
   <HistoryDetailStates isLoading={state.isLoading} error={state.error} backLabel={t("prev_page")} />
   {showContent ? (
    <div className={cn("space-y-24", "animate-in", "fade-in", "slide-in-from-bottom-8", "duration-1000")}>
     <HistoryDetailCardsGrid parsedCards={state.parsedCards} essenceLabel={t("essence_label")} fallbackCardName={(index) => `Card ${index}`} />
     <HistoryDetailAiSummary detail={detail} labels={{ title: t("ai_title"), fallbackDescription: (count) => t("ai_desc", { count }) }} />
    </div>
   ) : null}
  </div>
 );
}
