export type TarotSuitKey = 'major' | 'wands' | 'cups' | 'swords' | 'pentacles';

export interface TarotCardMeta {
 id: number;
 suit: TarotSuitKey;
}

export const TAROT_CARD_COUNT = 78;

function tarotSuitKeyFromId(id: number): TarotSuitKey {
 if (id <= 21) return 'major';
 if (id <= 35) return 'wands';
 if (id <= 49) return 'cups';
 if (id <= 63) return 'swords';
 return 'pentacles';
}

export const TAROT_DECK: TarotCardMeta[] = Array.from(
 { length: TAROT_CARD_COUNT },
 (_, id) => ({ id, suit: tarotSuitKeyFromId(id) })
);
