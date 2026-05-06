import type { ComponentType, MutableRefObject } from "react";
import type { FlyingCard, ShufflePath } from "@/features/reading/session/components/types";
import type { RevealedReadingCard } from "@/features/reading/shared/actions/types";

export interface ReadingSessionTextBundle {
  aiFooterNote: string;
  aiLive: string;
  aiSubtitle: string;
  aiTitle: string;
  backLabel: string;
  changeCardText: string;
  meaningLabel: string;
  reversedLabel: string;
  modalDescription: string;
  modalRevealText: string;
  modalRevealingText: string;
  modalTitle: string;
  pickCountText: string;
  pickDoneText: string;
  pickPromptText: string;
  pickRandomText: string;
  questionLabel: string;
  sessionLabel: string;
  shuffleSubtitle: string;
  shuffleTitle: string;
  title: string;
  uprightLabel: string;
}

export interface ReadingSessionContentProps {
  activeDeckRows: number[][];
  allCardsFlipped: boolean;
  cards: RevealedReadingCard[];
  cardsToDraw: number;
  error: string;
  flippedIndex: number;
  flyingCards: FlyingCard[];
  getCardImageUrl: (cardId: number) => string | undefined;
  getCardMeaning: (
    cardId: number,
    orientation?: "upright" | "reversed",
  ) => string | null | undefined;
  getCardName: (cardId: number) => string | null | undefined;
  handleChangeCard: () => void;
  handleRandomSelect: () => void;
  handleReveal: () => void;
  isRevealing: boolean;
  isShuffling: boolean;
  onBack: () => void;
  onPickCard: (cardId: number, source: HTMLDivElement | null) => void;
  onRemovePickedCard: (cardId: number) => void;
  pickedCardSet: Set<number>;
  pickedCards: number[];
  question: string;
  sessionId: string;
  setDeckCardRef: (cardId: number) => (node: HTMLDivElement | null) => void;
  shufflePaths: ShufflePath[];
  stackAnchorRef: MutableRefObject<HTMLDivElement | null>;
  texts: ReadingSessionTextBundle;
  AiInterpretationStream: ComponentType<{
    sessionId: string;
    cards?: RevealedReadingCard[];
    isReadyToShow?: boolean;
  }>;
}
