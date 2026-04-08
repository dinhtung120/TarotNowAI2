export type CallType = 'audio' | 'video';

export type CallSessionStatus = 'requested' | 'accepted' | 'joining' | 'connected' | 'ending' | 'ended' | 'failed';

export interface CallSessionDto {
 id: string;
 conversationId: string;
 roomName?: string;
 initiatorId: string;
 calleeId?: string;
 type: CallType;
 status: CallSessionStatus;
 startedAt?: string;
 acceptedAt?: string;
 connectedAt?: string;
 endedAt?: string;
 durationSeconds?: number;
 endReason?: string;
 initiatorJoinedAt?: string;
 calleeJoinedAt?: string;
 createdAt: string;
 updatedAt: string;
}

export interface CallTimeoutsDto {
 ringTimeoutSeconds: number;
 joinTimeoutSeconds: number;
 reconnectGracePeriodSeconds: number;
}

export interface CallJoinTicketDto {
 session: CallSessionDto;
 liveKitUrl: string;
 accessToken: string;
 participantIdentity: string;
 timeouts: CallTimeoutsDto;
}

export interface CallPeer {
 userId: string;
 isCaller: boolean;
 type: CallType;
}
