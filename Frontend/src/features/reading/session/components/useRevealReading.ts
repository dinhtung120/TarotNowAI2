import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import { revealReadingSession } from "@/features/reading/shared/actions";
import { FLIP_CARD_DELAY_MS } from "@/features/reading/session/components/constants";
import type { RevealedReadingCard } from "@/features/reading/shared/actions/types";

interface UseRevealReadingArgs {
  sessionId: string;
  revealFailedMessage: string;
  onProfileRefresh: () => Promise<void>;
}

export function useRevealReading({
  sessionId,
  revealFailedMessage,
  onProfileRefresh,
}: UseRevealReadingArgs) {
  const [cards, setCards] = useState<RevealedReadingCard[]>([]);
  const [error, setError] = useState("");
  const [flippedIndex, setFlippedIndex] = useState(-1);
  const [isRevealing, setIsRevealing] = useState(false);

  const flipTimersRef = useRef<number[]>([]);
  const isRevealingRef = useRef(false);

  const clearFlipTimers = useCallback(() => {
    flipTimersRef.current.forEach((timerId) => window.clearTimeout(timerId));
    flipTimersRef.current = [];
  }, []);

  useEffect(() => clearFlipTimers, [clearFlipTimers]);

  const revealCards = useCallback(async () => {
    if (isRevealingRef.current) {
      return false;
    }

    isRevealingRef.current = true;
    setIsRevealing(true);
    setError("");
    clearFlipTimers();
    setFlippedIndex(-1);

    const response = await revealReadingSession({ sessionId });

    if (!response.success || !response.data) {
      setError(response.error || revealFailedMessage);
      isRevealingRef.current = false;
      setIsRevealing(false);
      return false;
    }

    setCards(response.data.cards);
    response.data.cards.forEach((_, index) => {
      const timerId = window.setTimeout(
        () => setFlippedIndex(index),
        (index + 1) * FLIP_CARD_DELAY_MS,
      );
      flipTimersRef.current.push(timerId);
    });

    await onProfileRefresh();
    isRevealingRef.current = false;
    setIsRevealing(false);
    return true;
  }, [clearFlipTimers, onProfileRefresh, revealFailedMessage, sessionId]);

  const allCardsFlipped = useMemo(
    () => cards.length > 0 && flippedIndex >= cards.length - 1,
    [cards.length, flippedIndex],
  );

  return {
    allCardsFlipped,
    cards,
    error,
    flippedIndex,
    isRevealing,
    revealCards,
  };
}
