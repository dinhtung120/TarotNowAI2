export type CallType = 'audio' | 'video';

export type CallSessionStatus = 'requested' | 'accepted' | 'rejected' | 'ended';

export interface CallSessionDto {
 id: string;
 conversationId: string;
 initiatorId: string;
 type: CallType;
 status: CallSessionStatus;
 startedAt?: string;
 endedAt?: string;
 durationSeconds?: number;
 endReason?: string;
 createdAt: string;
 updatedAt: string;
}

export interface CallPeer {
 userId: string;
 isCaller: boolean;
 type: CallType;
}

export interface IceCandidatePayload {
 candidate: RTCIceCandidateInit;
 conversationId: string;
}

export interface WebRtcOfferPayload {
 offer: RTCSessionDescriptionInit;
 conversationId: string;
}

export interface WebRtcAnswerPayload {
 answer: RTCSessionDescriptionInit;
 conversationId: string;
}
