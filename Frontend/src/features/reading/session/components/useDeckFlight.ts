import { useCallback, useEffect, useMemo, useRef, useState } from "react";
import {
  DECK_INDEXES,
  FLIGHT_DURATION_MS,
  PICKED_STACK_CARD_HEIGHT,
  PICKED_STACK_CARD_WIDTH,
  PICKED_STACK_RIGHT_OFFSET,
  PICKED_STACK_TOP_OFFSET,
  RANDOM_PICK_DELAY_MS,
} from "@/features/reading/session/components/constants";
import type { FlyingCard } from "@/features/reading/session/components/types";
import { getStackPlacement, pickRandomIndexes } from "@/features/reading/session/components/utils";
interface UseDeckFlightArgs {
  cardsToDraw: number;
  isRevealing: boolean;
}

export function useDeckFlight({ cardsToDraw, isRevealing }: UseDeckFlightArgs) {
  const [pickedCards, setPickedCards] = useState<number[]>([]);
  const [flyingCards, setFlyingCards] = useState<FlyingCard[]>([]);
  const animationTimersRef = useRef<number[]>([]);
  const pickedCardsRef = useRef<number[]>([]);
  const flightSequenceRef = useRef(0);
  const deckCardRefs = useRef<Map<number, HTMLDivElement>>(new Map());
  const stackAnchorRef = useRef<HTMLDivElement | null>(null);
  useEffect(() => {
    pickedCardsRef.current = pickedCards;
  }, [pickedCards]);

  const clearAnimationTimers = useCallback(() => {
    animationTimersRef.current.forEach((timerId) => window.clearTimeout(timerId));
    animationTimersRef.current = [];
  }, []);
  useEffect(() => clearAnimationTimers, [clearAnimationTimers]);

  const getStackTarget = useCallback((stackIndex: number) => {
    const { xOffset, yOffset, rotate } = getStackPlacement(stackIndex);
    const anchorRect = stackAnchorRef.current?.getBoundingClientRect();

    if (anchorRect) {
      return {
        x: anchorRect.left + PICKED_STACK_CARD_WIDTH / 2 + xOffset,
        y: anchorRect.top + PICKED_STACK_CARD_HEIGHT / 2 + yOffset,
        rotate,
      };
    }

    return {
      x: window.innerWidth - PICKED_STACK_RIGHT_OFFSET - PICKED_STACK_CARD_WIDTH / 2 + xOffset,
      y: PICKED_STACK_TOP_OFFSET + PICKED_STACK_CARD_HEIGHT / 2 + yOffset,
      rotate,
    };
  }, []);

  const launchCardToStack = useCallback(
    (cardId: number, stackIndex: number, source?: HTMLDivElement | null) => {
      const sourceRect = source?.getBoundingClientRect();
      const startX = sourceRect ? sourceRect.left + sourceRect.width / 2 : window.innerWidth / 2;
      const startY = sourceRect ? sourceRect.top + sourceRect.height / 2 : window.innerHeight * 0.72;
      const target = getStackTarget(stackIndex);

      flightSequenceRef.current += 1;
      const flight: FlyingCard = {
        key: `${cardId}-${flightSequenceRef.current}`,
        startX,
        startY,
        deltaX: target.x - startX,
        deltaY: target.y - startY,
        rotate: target.rotate,
        stackIndex,
      };

      setFlyingCards((prev) => [...prev, flight]);
      const timerId = window.setTimeout(
        () => setFlyingCards((prev) => prev.filter((item) => item.key !== flight.key)),
        FLIGHT_DURATION_MS,
      );
      animationTimersRef.current.push(timerId);
    },
    [getStackTarget],
  );

  const setDeckCardRef = useCallback(
    (cardId: number) => (node: HTMLDivElement | null) => {
      if (node) {
        deckCardRefs.current.set(cardId, node);
        return;
      }
      deckCardRefs.current.delete(cardId);
    },
    [],
  );

  const addPickedCard = useCallback(
    (cardId: number, source?: HTMLDivElement | null) => {
      if (isRevealing) return;
      const currentPicks = pickedCardsRef.current;
      if (currentPicks.includes(cardId) || currentPicks.length >= cardsToDraw) return;

      launchCardToStack(cardId, currentPicks.length, source);
      const nextPicks = [...currentPicks, cardId];
      pickedCardsRef.current = nextPicks;
      setPickedCards(nextPicks);
    },
    [cardsToDraw, isRevealing, launchCardToStack],
  );

  const removePickedCard = useCallback(
    (cardId: number) => {
      if (isRevealing) return;
      const nextPicks = pickedCardsRef.current.filter((pickedId) => pickedId !== cardId);
      pickedCardsRef.current = nextPicks;
      setPickedCards(nextPicks);
    },
    [isRevealing],
  );

  const handleRandomSelect = useCallback(() => {
    if (isRevealing || pickedCardsRef.current.length >= cardsToDraw) return;
    const currentPickedSet = new Set(pickedCardsRef.current);
    const availableIndexes = DECK_INDEXES.filter((index) => !currentPickedSet.has(index));
    const randomCards = pickRandomIndexes(availableIndexes, cardsToDraw - pickedCardsRef.current.length);

    randomCards.forEach((index, offsetIndex) => {
      const timerId = window.setTimeout(() => {
        addPickedCard(index, deckCardRefs.current.get(index) ?? null);
      }, offsetIndex * RANDOM_PICK_DELAY_MS);
      animationTimersRef.current.push(timerId);
    });
  }, [addPickedCard, cardsToDraw, isRevealing]);

  const clearTransientAnimations = useCallback(() => {
    clearAnimationTimers();
    setFlyingCards([]);
  }, [clearAnimationTimers]);
  const pickedCardSet = useMemo(() => new Set(pickedCards), [pickedCards]);

  return {
    addPickedCard,
    clearTransientAnimations,
    flyingCards,
    handleRandomSelect,
    pickedCards,
    pickedCardSet,
    removePickedCard,
    setDeckCardRef,
    stackAnchorRef,
  };
}
