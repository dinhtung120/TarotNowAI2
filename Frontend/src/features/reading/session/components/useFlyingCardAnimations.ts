"use client";

import { useCallback, useEffect, useMemo, useRef } from "react";
import type { FlyingCard } from "@/features/reading/session/components/types";

const FLIGHT_DURATION_MS = 460;
const FLIGHT_EASING = "cubic-bezier(0.2, 0.7, 0.24, 1)";
const CARD_WIDTH = 72;
const CARD_HEIGHT = 108;

function toFlightTransform(x: number, y: number, rotate = 0, scale = 1): string {
  const pxX = Math.round(x - CARD_WIDTH / 2);
  const pxY = Math.round(y - CARD_HEIGHT / 2);
  return `translate3d(${pxX}px, ${pxY}px, 0) scale(${scale}) rotate(${rotate}deg)`;
}

function buildFlightKeyframes(card: FlyingCard): Keyframe[] {
  const targetX = card.startX + card.deltaX;
  const targetY = card.startY + card.deltaY;

  return [
    {
      transform: toFlightTransform(card.startX, card.startY, 0, 1),
      opacity: 0.95,
      offset: 0,
    },
    {
      opacity: 1,
      offset: 0.7,
    },
    {
      transform: toFlightTransform(targetX, targetY, card.rotate, 0.88),
      opacity: 0.96,
      offset: 1,
    },
  ];
}

function prefersReducedMotion(): boolean {
  return typeof window !== "undefined"
    && window.matchMedia("(prefers-reduced-motion: reduce)").matches;
}

export function useFlyingCardAnimations(flyingCards: FlyingCard[]) {
  const sortedFlyingCards = useMemo(
    () => [...flyingCards].sort((left, right) => left.stackIndex - right.stackIndex),
    [flyingCards],
  );
  const cardNodesRef = useRef<Map<string, HTMLDivElement>>(new Map());
  const animationsRef = useRef<Map<string, Animation>>(new Map());

  const setCardNode = useCallback((key: string, node: HTMLDivElement | null) => {
    if (node) {
      cardNodesRef.current.set(key, node);
      return;
    }

    cardNodesRef.current.delete(key);
  }, []);

  useEffect(() => {
    if (prefersReducedMotion()) {
      for (const animation of animationsRef.current.values()) {
        animation.cancel();
      }
      animationsRef.current.clear();
      return;
    }

    const activeKeys = new Set(sortedFlyingCards.map((card) => card.key));
    for (const [key, animation] of animationsRef.current.entries()) {
      if (!activeKeys.has(key)) {
        animation.cancel();
        animationsRef.current.delete(key);
      }
    }

    for (const card of sortedFlyingCards) {
      if (animationsRef.current.has(card.key)) {
        continue;
      }

      const cardNode = cardNodesRef.current.get(card.key);
      if (!cardNode) {
        continue;
      }

      const animation = cardNode.animate(buildFlightKeyframes(card), {
        duration: FLIGHT_DURATION_MS,
        easing: FLIGHT_EASING,
        fill: "forwards",
      });

      animationsRef.current.set(card.key, animation);

      const cleanup = () => {
        const trackedAnimation = animationsRef.current.get(card.key);
        if (trackedAnimation === animation) {
          animationsRef.current.delete(card.key);
        }
      };

      animation.addEventListener("finish", cleanup, { once: true });
      animation.addEventListener("cancel", cleanup, { once: true });
    }
  }, [sortedFlyingCards]);

  useEffect(
    () => () => {
      for (const animation of animationsRef.current.values()) {
        animation.cancel();
      }
      animationsRef.current.clear();
      cardNodesRef.current.clear();
    },
    [],
  );

  return {
    sortedFlyingCards,
    setCardNode,
  };
}
