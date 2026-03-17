export interface ReaderProfile {
 id: string;
 userId: string;
 displayName: string;
 bio: string;
 introText?: string;
 pricePerQuestion: number;
 rating: number;
 totalReadings: number;
 status: 'Offline' | 'Online' | 'AcceptingQuestions' | 'Busy';
 registeredAt: string;
}

export interface ReaderRequest {
 id: string;
 userId: string;
 introText: string;
 status: 'Pending' | 'Approved' | 'Rejected';
 adminNotes?: string;
 submittedAt: string;
 processedAt?: string;
}

export interface SubmitReaderRequestData {
 introText: string;
}

export interface UpdateReaderProfileData {
 bio?: string;
 introText?: string;
 pricePerQuestion?: number;
}
