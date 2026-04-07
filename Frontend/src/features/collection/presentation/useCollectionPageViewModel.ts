import { useMemo } from "react";
import { useTranslations } from "next-intl";
import { useCollectionPage } from "@/features/collection/application/useCollectionPage";
import type { CollectionFilterBarLabels } from "@/features/collection/presentation/components/CollectionFilterBar.types";
import { useCardsCatalog } from "@/shared/application/hooks/useCardsCatalog";

export function useCollectionPageViewModel() {
  const t = useTranslations("Collection");
  const tCommon = useTranslations("Common");
  const tTarot = useTranslations("Tarot");

  const { getCardImageUrl, getCardMeaning, getCardName } = useCardsCatalog();
  const pageState = useCollectionPage();

  const selectedCardImageUrl = useMemo(
    () =>
      pageState.selectedCardData
        ? getCardImageUrl(pageState.selectedCardData.id)
        : undefined,
    [getCardImageUrl, pageState.selectedCardData],
  );

  const selectedCardName = useMemo(
    () =>
      pageState.selectedCardData
        ? (getCardName(pageState.selectedCardData.id) ?? "")
        : "",
    [getCardName, pageState.selectedCardData],
  );

  const selectedCardMeaning = useMemo(
    () =>
      pageState.selectedCardData
        ? (getCardMeaning(pageState.selectedCardData.id) ?? "")
        : "",
    [getCardMeaning, pageState.selectedCardData],
  );

  const selectedSuitLabel = useMemo(
    () =>
      pageState.selectedCardData
        ? tTarot(`suits.${pageState.selectedCardData.suit}.full`)
        : "",
    [pageState.selectedCardData, tTarot],
  );

  const filterBarLabels: CollectionFilterBarLabels = {
    filterAll: t("filter_all"),
    filterOwned: t("filter_owned"),
    filterUnowned: t("filter_unowned"),
    filtersLabel: t("filters_label"),
    sortLabel: tCommon("sort_by") || "Sắp xếp theo",
  };

  return {
    ...pageState,
    deckGridLabels: {
      emptyCta: t("cta_draw"),
      emptyDescription: t("empty_desc"),
      emptyTitle: t("empty_title"),
      unknownCard: t("unknown_card"),
    },
    getCardImageUrl,
    getCardName,
    filterBarLabels,
    headerLabels: {
      progressLabel: t("progress_label"),
      subtitle: t("subtitle"),
      tag: t("tag"),
      title: t("title"),
    },
    selectedCardImageUrl,
    selectedCardMeaning,
    selectedCardName,
    selectedSuitLabel,
    zoomLabels: {
      closeLabel: tCommon("close"),
      copiesLabel: t("copies_label"),
      levelLabel: t("level_label"),
      lockedMeaning: t("locked_meaning"),
      unknownCard: t("unknown_card"),
    },
  };
}
