import type { AdminDepositOrder } from "@/features/admin/application/actions";

export interface AdminDepositRowLabels {
 systemUser: string;
 userIdPrefix: (id: string) => string;
 statusSuccess: string;
 statusFailed: string;
 statusPending: string;
 approveTitle: string;
 rejectTitle: string;
 notAvailable: string;
}

export interface AdminDepositsTableLabels extends AdminDepositRowLabels {
 headingUser: string;
 headingAmount: string;
 headingAssets: string;
 headingTime: string;
 headingStatus: string;
 headingActions: string;
 loading: string;
 empty: string;
 summary: string;
}

export interface AdminDepositsTableProps {
 locale: string;
 orders: AdminDepositOrder[];
 loading: boolean;
 page: number;
 totalCount: number;
 processingId: string | null;
 labels: AdminDepositsTableLabels;
 onApprove: (order: AdminDepositOrder) => void;
 onReject: (order: AdminDepositOrder) => void;
 onPrev: () => void;
 onNext: () => void;
}
