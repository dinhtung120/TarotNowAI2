import type { AdminReaderRequest } from '@/features/admin/reader-requests/actions/reader-requests';
import { cn } from '@/lib/utils';
import { ReaderRequestCard } from './ReaderRequestCard';
import type { ReaderRequestLabels } from './ReaderRequestCard.types';

interface ReaderRequestsListProps {
 locale: string;
 requests: AdminReaderRequest[];
 selectedRequestId: string | null;
 getAdminNote: (requestId: string) => string;
 processingId: string | null;
 labels: ReaderRequestLabels;
 onSelectRequest: (requestId: string) => void;
 onApprove: (requestId: string) => Promise<void>;
 onReject: (requestId: string) => Promise<void>;
 onAdminNoteChange: (requestId: string, value: string) => void;
}

export function ReaderRequestsList({
 locale,
 requests,
 selectedRequestId,
 getAdminNote,
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
     adminNoteDraft={getAdminNote(request.id)}
     processingId={processingId}
     labels={labels}
     onSelectRequest={() => onSelectRequest(request.id)}
     onApprove={() => onApprove(request.id)}
     onReject={() => onReject(request.id)}
     onAdminNoteChange={(value) => onAdminNoteChange(request.id, value)}
    />
   ))}
  </div>
 );
}
