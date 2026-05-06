import { useEffect, useState } from "react";
import { SESSION_SHUFFLE_DURATION_MS } from "@/features/reading/session/components/constants";
import { useDeckFlight } from "@/features/reading/session/components/useDeckFlight";
import { useDeckLayout } from "@/features/reading/session/components/useDeckLayout";

interface UseDeckSelectionArgs {
  cardsToDraw: number;
  isRevealing: boolean;
}

export function useDeckSelection({ cardsToDraw, isRevealing }: UseDeckSelectionArgs) {
  const [isShuffling, setIsShuffling] = useState(true);
  const layout = useDeckLayout();
  const deckFlight = useDeckFlight({ cardsToDraw, isRevealing });

  useEffect(() => {
    const timerId = window.setTimeout(
      () => setIsShuffling(false),
      SESSION_SHUFFLE_DURATION_MS,
    );

    return () => window.clearTimeout(timerId);
  }, []);

  return {
    ...layout,
    ...deckFlight,
    isShuffling,
  };
}
