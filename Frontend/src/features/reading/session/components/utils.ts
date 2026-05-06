import {
  PICKED_STACK_X_STEP,
  PICKED_STACK_Y_STEP,
  SHUFFLE_CARD_COUNT,
} from "@/features/reading/session/components/constants";
import type { ShufflePath } from "@/features/reading/session/components/types";

export const chunkDeckRows = (indexes: number[], cardsPerRow: number): number[][] => {
  const rows: number[][] = [];
  for (let i = 0; i < indexes.length; i += cardsPerRow) {
    rows.push(indexes.slice(i, i + cardsPerRow));
  }
  return rows;
};

export const generateShufflePaths = (): ShufflePath[] =>
  Array.from({ length: SHUFFLE_CARD_COUNT }).map((_, index) => {
    const isLeft = index % 2 === 0;
    const direction = isLeft ? -1 : 1;
    const fanAngle = (index / SHUFFLE_CARD_COUNT) * 40 - 20;
    const offset = 100 * direction;

    return {
      tx: `${offset}px`,
      ty: `${Math.abs(fanAngle)}px`,
      r: `${fanAngle}deg`,
      tx2: `${offset * 0.5}px`,
      ty2: `-${Math.abs(fanAngle) * 2}px`,
      r2: `${fanAngle * 0.5}deg`,
      delay: `${index * 0.15}s`,
      duration: "2s",
      z: index,
    };
  });

export const pickRandomIndexes = (availableIndexes: number[], count: number): number[] => {
  const shuffled = [...availableIndexes];

  for (let index = shuffled.length - 1; index > 0; index -= 1) {
    const randomIndex = Math.floor(Math.random() * (index + 1));
    [shuffled[index], shuffled[randomIndex]] = [
      shuffled[randomIndex],
      shuffled[index],
    ];
  }

  return shuffled.slice(0, count);
};

export const getStackPlacement = (stackIndex: number) => {
  const clampedIndex = Math.min(stackIndex, 7);

  return {
    xOffset: clampedIndex * PICKED_STACK_X_STEP,
    yOffset: clampedIndex * PICKED_STACK_Y_STEP,
    rotate: -8 + (clampedIndex % 5) * 2,
  };
};

export const resolveStackPlacementClass = (stackIndex: number): string => {
  const clampedIndex = Math.max(0, Math.min(stackIndex, 7));
  return `tn-stack-placement-${clampedIndex}`;
};
