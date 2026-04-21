import type { AdminReaderRequest } from '@/features/admin/application/actions';

export interface ReaderRequestLabels {
 statusPending: string;
 statusApproved: string;
 statusRejected: string;
 userPrefix: (id: string) => string;
 introTitle: string;
 introExpand: string;
 specialtiesTitle: string;
 yearsOfExperienceTitle: string;
 socialLinksTitle: string;
 priceTitle: string;
 adminNoteTitle: string;
 reviewHistoryTitle: string;
 reviewHistoryEmpty: string;
 reviewHistoryAt: string;
 reviewHistoryBy: string;
 reviewHistoryActionApprove: string;
 reviewHistoryActionReject: string;
 adminNotePlaceholder: string;
 approve: string;
 reject: string;
}

export interface ReaderRequestCardProps {
 locale: string;
 request: AdminReaderRequest;
 selectedRequestId: string | null;
 adminNoteDraft: string;
 processingId: string | null;
 labels: ReaderRequestLabels;
 onSelectRequest: () => void;
 onApprove: () => Promise<void>;
 onReject: () => Promise<void>;
 onAdminNoteChange: (value: string) => void;
}
