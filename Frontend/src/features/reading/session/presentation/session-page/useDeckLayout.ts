import { useEffect, useMemo, useState } from "react";
import {
  CARD_ASPECT_HEIGHT_RATIO,
  DECK_CARDS_PER_ROW_DESKTOP,
  DECK_CARDS_PER_ROW_MOBILE,
  DECK_HORIZONTAL_OVERLAP_FACTOR_DESKTOP,
  DECK_HORIZONTAL_OVERLAP_FACTOR_MOBILE,
  DECK_INDEXES,
  DECK_VERTICAL_OVERLAP_FACTOR,
} from "@/features/reading/session/presentation/session-page/constants";
import { chunkDeckRows } from "@/features/reading/session/presentation/session-page/utils";

export function useDeckLayout() {
  const [isMobileViewport, setIsMobileViewport] = useState(false);

  useEffect(() => {
    const mediaQuery = window.matchMedia("(max-width: 639px)");
    const updateViewport = () => setIsMobileViewport(mediaQuery.matches);

    updateViewport();
    mediaQuery.addEventListener("change", updateViewport);

    return () => mediaQuery.removeEventListener("change", updateViewport);
  }, []);

  const deckRowsDesktop = useMemo(
    () => chunkDeckRows(DECK_INDEXES, DECK_CARDS_PER_ROW_DESKTOP),
    [],
  );
  const deckRowsMobile = useMemo(
    () => chunkDeckRows(DECK_INDEXES, DECK_CARDS_PER_ROW_MOBILE),
    [],
  );

  const activeDeckRows = isMobileViewport ? deckRowsMobile : deckRowsDesktop;
  const deckCardWidth = isMobileViewport
    ? "clamp(44px, 11.2vw, 56px)"
    : "clamp(56px, 6.8vw, 98px)";
  const horizontalOverlapFactor = isMobileViewport
    ? DECK_HORIZONTAL_OVERLAP_FACTOR_MOBILE
    : DECK_HORIZONTAL_OVERLAP_FACTOR_DESKTOP;
  const rowOverlapMargin = `calc(var(--deck-card-w) * -${DECK_VERTICAL_OVERLAP_FACTOR * CARD_ASPECT_HEIGHT_RATIO})`;

  return {
    activeDeckRows,
    deckCardWidth,
    horizontalOverlapFactor,
    rowOverlapMargin,
  };
}
