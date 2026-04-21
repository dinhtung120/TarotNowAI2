import { GlassCard } from '@/shared/components/ui';
import { cn } from '@/lib/utils';
import { ReaderRequestAdminNote } from './ReaderRequestAdminNote';
import { ReaderRequestActions } from './ReaderRequestActions';
import { ReaderRequestIntro } from './ReaderRequestIntro';
import { ReaderRequestMeta } from './ReaderRequestMeta';
import { ReaderRequestReviewHistory } from './ReaderRequestReviewHistory';
import type { ReaderRequestCardProps } from './ReaderRequestCard.types';

export function ReaderRequestCard({
 locale,
 request,
 selectedRequestId,
 adminNoteDraft,
 processingId,
 labels,
 onSelectRequest,
 onApprove,
 onReject,
 onAdminNoteChange,
}: ReaderRequestCardProps) {
 const isSelected = selectedRequestId === request.id;

 return (
  <GlassCard className={cn('space-y-6 group tn-hover-border-accent-30 transition-all !p-8')}>
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
    introTitle={labels.introTitle}
    specialtiesTitle={labels.specialtiesTitle}
    yearsOfExperienceTitle={labels.yearsOfExperienceTitle}
    socialLinksTitle={labels.socialLinksTitle}
    priceTitle={labels.priceTitle}
    request={request}
    isSelected={isSelected}
    onSelectRequest={onSelectRequest}
   />
  <ReaderRequestAdminNote note={request.adminNote || ''} title={labels.adminNoteTitle} />
   <ReaderRequestReviewHistory
    locale={locale}
    history={request.reviewHistory}
    labels={{
     title: labels.reviewHistoryTitle,
     empty: labels.reviewHistoryEmpty,
     at: labels.reviewHistoryAt,
     by: labels.reviewHistoryBy,
     actionApprove: labels.reviewHistoryActionApprove,
     actionReject: labels.reviewHistoryActionReject,
    }}
   />
   {request.status === 'pending' ? (
    <ReaderRequestActions
     requestId={request.id}
     processingId={processingId}
     note={isSelected ? adminNoteDraft : ''}
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
