import { TAROT_CARD_COUNT } from "@/shared/domain/tarotData";

export const SESSION_SHUFFLE_DURATION_MS = 2200;
export const SHUFFLE_CARD_COUNT = 9;
export const FLIGHT_DURATION_MS = 460;
export const RANDOM_PICK_DELAY_MS = 180;
export const FLIP_CARD_DELAY_MS = 800;

export const DECK_WAVE_Y_AMPLITUDE = 6;
export const DECK_WAVE_ROTATION_AMPLITUDE = 7;
export const DECK_CARDS_PER_ROW_DESKTOP = 26;
export const DECK_CARDS_PER_ROW_MOBILE = 9;
export const DECK_HORIZONTAL_OVERLAP_FACTOR_DESKTOP = 0.5;
export const DECK_HORIZONTAL_OVERLAP_FACTOR_MOBILE = 0.32;
export const DECK_VERTICAL_OVERLAP_FACTOR = 0.2;
export const CARD_ASPECT_HEIGHT_RATIO = 1.5;

export const PICKED_STACK_CARD_WIDTH = 72;
export const PICKED_STACK_CARD_HEIGHT = 108;
export const PICKED_STACK_TOP_OFFSET = 96;
export const PICKED_STACK_RIGHT_OFFSET = 24;
export const PICKED_STACK_X_STEP = 2;
export const PICKED_STACK_Y_STEP = 6;

export const DECK_INDEXES = Array.from(
  { length: TAROT_CARD_COUNT },
  (_, index) => index,
);
