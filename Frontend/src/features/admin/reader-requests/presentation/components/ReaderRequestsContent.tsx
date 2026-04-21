'use client';

import type { AdminReaderRequest } from '@/features/admin/application/actions';
import type { ReaderRequestLabels } from './ReaderRequestCard.types';
import { ReaderRequestsList } from './ReaderRequestsList';
import { ReaderRequestsPagination } from './ReaderRequestsPagination';
import { ReaderRequestsStates } from './ReaderRequestsStates';

interface ReaderRequestsContentProps {
 emptyLabel: string;
 labels: ReaderRequestLabels;
 loading: boolean;
 locale: string;
 getAdminNote: (requestId: string) => string;
 onAdminNoteChange: (requestId: string, value: string) => void;
 onApprove: (requestId: string) => Promise<void>;
 onReject: (requestId: string) => Promise<void>;
 onSelectRequest: (requestId: string) => void;
 page: number;
 paginationLabel: string;
 processingId: string | null;
 requests: AdminReaderRequest[];
 selectedRequestId: string | null;
 setPage: (updater: (page: number) => number) => void;
 totalPages: number;
}

export function ReaderRequestsContent(props: ReaderRequestsContentProps) {
 return (
  <>
   <ReaderRequestsStates loading={props.loading} isEmpty={!props.loading && props.requests.length === 0} emptyLabel={props.emptyLabel} />
   {!props.loading && props.requests.length > 0 ? <ReaderRequestsList locale={props.locale} requests={props.requests} selectedRequestId={props.selectedRequestId} getAdminNote={props.getAdminNote} processingId={props.processingId} labels={props.labels} onSelectRequest={props.onSelectRequest} onApprove={props.onApprove} onReject={props.onReject} onAdminNoteChange={props.onAdminNoteChange} /> : null}
   {!props.loading && props.totalPages > 1 ? <ReaderRequestsPagination currentLabel={props.paginationLabel} canPrev={props.page > 1} canNext={props.page < props.totalPages} onPrev={() => props.setPage((currentPage) => Math.max(1, currentPage - 1))} onNext={() => props.setPage((currentPage) => Math.min(props.totalPages, currentPage + 1))} /> : null}
  </>
 );
}
