import type { MutableRefObject } from 'react';
import type { FlyingCard, ShufflePath } from '@/features/reading/session/presentation/session-page/types';

export interface DrawPhaseSectionProps {
  activeDeckRows: number[][];
  cardsToDraw: number;
  changeCardText: string;
  deckCardWidth: string;
  error: string;
  flyingCards: FlyingCard[];
  horizontalOverlapFactor: number;
  isRevealing: boolean;
  isShuffling: boolean;
  modalDescription: string;
  modalRevealText: string;
  modalRevealingText: string;
  modalTitle: string;
  pickedCardCountText: string;
  pickedCardSet: Set<number>;
  pickedCards: number[];
  pickedDoneText: string;
  pickedPromptText: string;
  randomSelectText: string;
  rowOverlapMargin: string;
  shufflePaths: ShufflePath[];
  shuffleSubtitle: string;
  shuffleTitle: string;
  stackAnchorRef: MutableRefObject<HTMLDivElement | null>;
  onChangeCard: () => void;
  onPickCard: (cardId: number, source: HTMLDivElement | null) => void;
  onRandomSelect: () => void;
  onRemovePickedCard: (cardId: number) => void;
  onReveal: () => void;
  setDeckCardRef: (cardId: number) => (node: HTMLDivElement | null) => void;
}

export type DrawPhaseContentProps = Omit<
  DrawPhaseSectionProps,
  'error' | 'isShuffling' | 'shufflePaths' | 'shuffleSubtitle' | 'shuffleTitle'
>;
