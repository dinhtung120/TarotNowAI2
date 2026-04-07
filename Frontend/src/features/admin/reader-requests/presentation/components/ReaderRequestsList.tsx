import type { AdminReaderRequest } from '@/features/admin/application/actions';
import { cn } from '@/lib/utils';
import { ReaderRequestCard } from './ReaderRequestCard';
import type { ReaderRequestLabels } from './ReaderRequestCard.types';

interface ReaderRequestsListProps {
 locale: string;
 requests: AdminReaderRequest[];
 selectedRequestId: string | null;
 adminNote: string;
 processingId: string | null;
 labels: ReaderRequestLabels;
 onSelectRequest: (request: AdminReaderRequest) => void;
 onApprove: (requestId: string) => Promise<void>;
 onReject: (requestId: string) => Promise<void>;
 onAdminNoteChange: (value: string) => void;
}

export function ReaderRequestsList({
 locale,
 requests,
 selectedRequestId,
 adminNote,
 processingId,
 labels,
 onSelectRequest,
 onApprove,
 onReject,
 onAdminNoteChange,
}: ReaderRequestsListProps) {
 return (
  <div className={cn('space-y-6')}>
   {requests.map((request) => (
    <ReaderRequestCard
     key={request.id}
     locale={locale}
     request={request}
     selectedRequestId={selectedRequestId}
     adminNote={adminNote}
     processingId={processingId}
     labels={labels}
     onSelectRequest={() => onSelectRequest(request)}
     onApprove={() => onApprove(request.id)}
     onReject={() => onReject(request.id)}
     onAdminNoteChange={onAdminNoteChange}
    />
   ))}
  </div>
 );
}
