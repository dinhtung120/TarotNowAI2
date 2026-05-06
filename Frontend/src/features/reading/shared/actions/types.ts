export interface InitReadingRequest {
 spreadType: string;
 question?: string;
 currency?: string;
}

export interface InitReadingResponse {
 sessionId: string;
 costGold: number;
 costDiamond: number;
}

export interface RevealReadingRequest {
 sessionId: string;
 language?: "vi" | "en" | "zh";
}

export interface RevealedReadingCard {
 cardId: number;
 position: number;
 orientation: "upright" | "reversed";
}

export interface RevealReadingResponse {
 cards: RevealedReadingCard[];
}
