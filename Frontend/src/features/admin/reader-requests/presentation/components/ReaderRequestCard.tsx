import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import { ReaderRequestAdminNote } from './ReaderRequestAdminNote';
import { ReaderRequestActions } from './ReaderRequestActions';
import { ReaderRequestIntro } from './ReaderRequestIntro';
import { ReaderRequestMeta } from './ReaderRequestMeta';
import type { ReaderRequestCardProps } from './ReaderRequestCard.types';

export function ReaderRequestCard({
 locale,
 request,
 selectedRequestId,
 adminNote,
 processingId,
 labels,
 onSelectRequest,
 onApprove,
 onReject,
 onAdminNoteChange,
}: ReaderRequestCardProps) {
 const isSelected = selectedRequestId === request.id;

 return (
  <GlassCard className={cn('space-y-6 group hover:border-[var(--purple-accent)]/30 transition-all !p-8')}>
   <ReaderRequestMeta
    labels={{
     statusPending: labels.statusPending,
     statusApproved: labels.statusApproved,
     statusRejected: labels.statusRejected,
     userPrefix: labels.userPrefix,
    }}
    locale={locale}
    request={request}
   />
   <ReaderRequestIntro
    introExpand={labels.introExpand}
    introText={request.introText}
    introTitle={labels.introTitle}
    isSelected={isSelected}
    onSelectRequest={onSelectRequest}
   />
   <ReaderRequestAdminNote note={request.adminNote || ''} title={labels.adminNoteTitle} />
   {request.status === 'pending' ? (
    <ReaderRequestActions
     requestId={request.id}
     processingId={processingId}
     note={isSelected ? adminNote : ''}
     notePlaceholder={labels.adminNotePlaceholder}
     approveLabel={labels.approve}
     rejectLabel={labels.reject}
     onApprove={onApprove}
     onReject={onReject}
     onFocus={onSelectRequest}
     onChange={onAdminNoteChange}
    />
   ) : null}
  </GlassCard>
 );
}
