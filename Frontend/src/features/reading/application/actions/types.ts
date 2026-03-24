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
}

export interface RevealReadingResponse {
 cards: number[];
}
